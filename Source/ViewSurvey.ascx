<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ViewSurvey.ascx.cs" Inherits="Engage.Dnn.Survey.ViewSurvey" %>
<%@ Register TagPrefix="engage" Namespace="Engage.Survey.UI" Assembly="Engage.Survey" %>
<%@ Register TagPrefix="telerik" namespace="Telerik.Web.UI" assembly="Telerik.Web.UI" %>

<engage:SurveyControl id="SurveyControl" runat="server" ShowRequiredNotation="true" ValidationProvider="Engage">
    <Validators>
        <telerik:RadCaptcha runat="server" Display="Dynamic" 
                            ProtectionMode="MinimumTimeout" 
                            Enabled="<%# this.UseMinimumTimeoutProtection %>" 
                            Visible="<%# this.UseMinimumTimeoutProtection %>" 
                            ValidationGroup="<%# Container.ValidationGroup %>"
                            CssClass="em-bot-timer"
                            MinTimeout="<%# this.MinimumTimeout %>" 
                            ErrorMessage='<%# this.Localize("Minimum Timeout Error Message") %>' />
        <telerik:RadCaptcha runat="server" Display="Dynamic" 
                            ProtectionMode="InvisibleTextBox" 
                            Enabled="<%# this.UseInvisibleTextBoxProtection %>" 
                            Visible="<%# this.UseInvisibleTextBoxProtection %>" 
                            ValidationGroup="<%# Container.ValidationGroup %>" 
                            CssClass="em-honeypot"
                            ErrorMessage='<%# this.Localize("Invisible Text Box Error Message") %>' 
                            InvisibleTextBoxLabel='<%# this.Localize("Invisible Text Box Label") %>' />        
        <telerik:RadCaptcha runat="server" Display="Dynamic" 
                            ProtectionMode="Captcha" 
                            Enabled="<%# this.UseCaptchaProtection %>" 
                            Visible="<%# this.UseCaptchaProtection %>" 
                            ValidationGroup="<%# Container.ValidationGroup %>" 
                            CssClass="em-CAPTCHA" 
                            EnableRefreshImage="True" 
                            ErrorMessage='<%# this.Localize("CAPTCHA Error Message") %>' 
                            CaptchaLinkButtonText='<%# this.Localize("New CAPTCHA") %>' 
                            CaptchaTextBoxLabel='<%# this.Localize("CAPTCHA Label") %>' />        
    </Validators>
</engage:SurveyControl>
<asp:Button ID="DeleteResponseButton" runat="server" CssClass="delete-button" ResourceKey="DeleteButton" />
<asp:Button ID="EditDefinitionButton" runat="server" CssClass="edit-button" ResourceKey="EditButton" />