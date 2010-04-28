<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ViewSurvey.ascx.cs" Inherits="Engage.Dnn.Survey.ViewSurvey" %>
<%@ Register TagPrefix="engage" Namespace="Engage.Survey.UI" Assembly="Engage.Survey" %>
<engage:SurveyControl id="SurveyControl" runat="server" ShowRequiredNotation="true" ValidationProvider="Engage"/>
<asp:Button ID="DeleteResponseButton" runat="server" CssClass="delete-button" ResourceKey="DeleteButton" />
<asp:Button ID="EditDefinitionButton" runat="server" CssClass="edit-button" ResourceKey="EditButton" />