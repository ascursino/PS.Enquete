<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VWPEnqueteUserControl.ascx.cs" Inherits="Amorim.App.Enquete.VWPEnquete.VWPEnqueteUserControl" %>

<div class="AmorimEnquetes">
<asp:Label ID="lblMensagem" runat="server" CssClass="mensagem_enquete"></asp:Label>
<asp:Label ID="lblEnquete" runat="server" CssClass="pergunta_enquete" Visible="false"></asp:Label>

<asp:Panel ID="pnlPerguntas" runat="server">
</asp:Panel>

    <asp:Button ID="btnEnviar" runat="server" Text="Enviar" 
        onclick="btnEnviar_Click" Visible="false" ValidationGroup="AmorimEnquetes" />
<!--
<asp:Panel ID="pnlMensagens" runat="server" HorizontalAlign="Center">
</asp:Panel>
-->
</div>