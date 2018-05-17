using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Amorim.App.Enquete.VWPEnquete;
using System.Xml.Serialization;

namespace Amorim.App.Enquete.VWPEnquete
{
    [ToolboxItemAttribute(false)]
    public class VWPEnquete : WebPart
    {
        const string c_MensagemSucesso = "Obrigado por responder a enquete!";
        const string c_MensagemValidacao = "Favor informar sua resposta";
        const string c_MensagemInformacao = "Você já respondeu a esta enquete";
        public VWPEnquete()
        {
            this.MensagemSucesso = c_MensagemSucesso;
            this.MensagemValidacao = c_MensagemValidacao;
            this.MensagemInformacao = c_MensagemInformacao;
        }

        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/Amorim.App.Enquete/VWPEnquete/VWPEnqueteUserControl.ascx";

        protected override void CreateChildControls()
        {
            Control control = Page.LoadControl(_ascxPath);
            ((VWPEnqueteUserControl)control).LoadData(this.MensagemSucesso, this.MensagemValidacao,this.MensagemInformacao);
            Controls.Add(control);
        }

        [WebBrowsable(true),
         Category("Amorim Enquete"),
         Personalizable(PersonalizationScope.Shared),
         WebDisplayName("Mensagem de Sucesso")]
        public string MensagemSucesso { get; set; }

        [WebBrowsable(true),
         Category("Amorim Enquete"),
         Personalizable(PersonalizationScope.Shared),
         WebDisplayName("Mensagem de Validação")]
        public string MensagemValidacao { get; set; }

        [WebBrowsable(true),
         Category("Amorim Enquete"),
         Personalizable(PersonalizationScope.Shared),
         WebDisplayName("Mensagem Informativa")]
        public string MensagemInformacao { get; set; }
    }
}
