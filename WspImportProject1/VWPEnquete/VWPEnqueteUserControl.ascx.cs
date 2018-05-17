using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Linq;
using Microsoft.SharePoint.WebControls;
namespace Amorim.App.Enquete.VWPEnquete
{
    public partial class VWPEnqueteUserControl : UserControl
    {
        protected string _mensagemSucesso;
        protected string _mensagemValidacao;
        protected string _mensagemInformacao;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void LoadData(string MensagemSucesso, string MensagemValidacao, string MensagemInformacao)
        {
            this._mensagemSucesso = MensagemSucesso;
            this._mensagemValidacao = MensagemValidacao;
            this._mensagemInformacao = MensagemInformacao;
            try
            {
                using (SPWeb web = SPControl.GetContextWeb(Context))
                {
                    using (ModelDataContext _db = new ModelDataContext(web.Url))
                    {
                        var _listEnquetes = (from x in _db.ListaDeRespostas.ToList()
                                             where x.AutorId == SPContext.Current.Web.CurrentUser.ID
                                             select x.Enquete).ToList();

                        foreach (var item in _db.Enquete.Where(x => x.DataDeVencimento >= DateTime.Now).ToList())
                        {
                            if (!_listEnquetes.Contains(item))
                            {
                                CarregarEnquete(item);
                                this.btnEnviar.Visible = true;
                                break;
                            }
                            else
                            {
                                LimparTela(item);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private void LimparTela(EnqueteItem Enquete)
        {
            this.btnEnviar.Visible = false;
            this.pnlPerguntas.Controls.Clear();
            this.lblEnquete.Text = Enquete.Título;
            this.lblMensagem.Text = _mensagemInformacao;
            this.lblMensagem.CssClass = "mensagem_enquete";

        }

        private void CarregarEnquete(EnqueteItem Enquete)
        {
            this.lblEnquete.Text = Enquete.Título;
            this.lblMensagem.Text = string.Empty;
            using (SPWeb web = SPControl.GetContextWeb(Context))
            {
                using (ModelDataContext _db = new ModelDataContext(web.Url))
                {
                    var _listPerguntas = (from x in _db.Perguntas
                                          where x.Enquete.Id == Enquete.Id
                                          select x).ToList();

                    foreach (var _pergunta in _listPerguntas)
                    {
                        string idControleResposta = "rblrblRespostas" + _pergunta.Id;
                        string nomeGrupoValidacao = "AmorimEnquetes";
                        string nomeValidador = "validador" + _pergunta.Id;

                        RadioButtonList _rblRespostas = new RadioButtonList();
                        _rblRespostas.ID = idControleResposta;
                        //_rblRespostas.CssClass = "AmorimEnquetesRespostas";

                        _rblRespostas.CssClass = "resposta_enquete";

                        _rblRespostas.ValidationGroup = nomeGrupoValidacao;
                        _rblRespostas.Attributes.Add("EnqueteID", Enquete.Id.ToString());
                        _rblRespostas.Attributes.Add("PerguntaID", _pergunta.Id.ToString());

                        RequiredFieldValidator _rfv = new RequiredFieldValidator();
                        _rfv.ID = nomeValidador;
                        _rfv.CssClass = "AmorimEnquetesValidadores";
                        //_rfv.CssClass = "resposta_enquete";

                        _rfv.ErrorMessage = this._mensagemValidacao;
                        _rfv.ControlToValidate = _rblRespostas.ID;
                        _rfv.ValidationGroup = nomeGrupoValidacao;

                        //this.pnlPerguntas.Controls.Add(_rfv);
                        //this.pnlPerguntas.Controls.Add(new Literal { Text = "<br />" });

                        this.pnlPerguntas.Controls.Add(new Label { Text = _pergunta.Título, CssClass = "pergunta_enquete" });
                        this.pnlPerguntas.Controls.Add(new Literal { Text = "<br /><br />" });

                        var _listRespostas = (from x in _db.Respostas
                                              where x.Pergunta.Id == _pergunta.Id
                                              select x).ToList();

                        _listRespostas.ForEach(x => _rblRespostas.Items.Add(new ListItem { Text = x.Título, Value = x.Id.ToString() }));

                        this.pnlPerguntas.Controls.Add(_rblRespostas);
                        //this.pnlPerguntas.Controls.Add(new Literal { Text = "<br />" });
                        this.pnlPerguntas.Controls.Add(_rfv);
                        //this.pnlPerguntas.Controls.Add(new Literal { Text = "<br />" });

                        //this.pnlMensagens.Controls.Add(new Literal { Text = "<br /><br />" });
                        //this.pnlMensagens.Controls.Add(_rfv);
                    }

                }
            }
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SPWeb web = SPControl.GetContextWeb(Context);

                using (ModelDataContext _db = new ModelDataContext(web.Url))
                {
                    EnqueteItem enqueteRetorno = new EnqueteItem();
                    foreach (var item in this.pnlPerguntas.Controls)
                    {
                        if (item.GetType() == new RadioButtonList().GetType())
                        {
                            RadioButtonList _listResposta = (RadioButtonList)item;
                            ListaDeRespostasItem resposta = new ListaDeRespostasItem();

                            resposta.Enquete = (from x in _db.Enquete
                                                where x.Id == int.Parse(_listResposta.Attributes["EnqueteID"].ToString())
                                                select x).SingleOrDefault();

                            enqueteRetorno = resposta.Enquete;

                            resposta.Pergunta = (from x in _db.Perguntas
                                                 where x.Id == int.Parse(_listResposta.Attributes["PerguntaID"].ToString())
                                                 select x).SingleOrDefault();

                            resposta.Resposta = (from x in _db.Respostas
                                                 where x.Id == int.Parse(_listResposta.SelectedValue)
                                                 select x).SingleOrDefault();

                            var respostaDaPergunta = (from x in _db.Respostas.ToList()
                                             where x.Pergunta == resposta.Pergunta
                                             select x).ToList();

                            if (respostaDaPergunta.Where(x => x.RespostaCerta == true).ToList().Count() == 0)
                                resposta.RespostaCerta = resposta.Resposta;
                            else
                                resposta.RespostaCerta = respostaDaPergunta.Where(x => x.RespostaCerta == true).SingleOrDefault();

                            if (resposta.Resposta.Id == resposta.RespostaCerta.Id)
                                resposta.Acertou = true;
                            else
                                resposta.Acertou = false;

                            resposta.AutorId = SPContext.Current.Web.CurrentUser.ID;
                            _db.ListaDeRespostas.InsertOnSubmit(resposta);
                        }
                    }
                    _db.SubmitChanges();

                    Page.ClientScript.RegisterStartupScript(this.GetType(), "ScriptAgradecimento", "<script type=\"text/javascript\">alert('" + this._mensagemSucesso + "');</script>");
                    LimparTela(enqueteRetorno);
                }
            }
        }
    }
}
