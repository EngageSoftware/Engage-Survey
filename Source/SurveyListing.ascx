<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="SurveyListing.ascx.cs" Inherits="Engage.Dnn.Survey.SurveyListing" %>

<div id="survey-filters">
    <p><asp:Label ID="FilterLabel" runat="server" ResourceKey="FilterLabel.Text"/></p>
    <asp:RadioButtonList ID="FilterRadioButtonList" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
        <asp:ListItem Selected="True" ResourceKey="Survey Definitions.Text" Value="0"/>
        <asp:ListItem ResourceKey="Completed Surveys.Text" Value="1"/>
    </asp:RadioButtonList>
</div>

<asp:Repeater ID="SurveyGrid" runat="server">

</asp:Repeater>

<div>
    <asp:linkbutton CssClass="CommandButton" ID="NewSurveyButton" Resourcekey="NewSurveyButton.Text" runat="server" />&nbsp;
    <asp:linkbutton CssClass="CommandButton" ID="CancelButton" ResourceKey="CancelButton.Text" runat="server" CausesValidation="False"/>&nbsp;
</div>

