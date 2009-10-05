<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditSurvey.ascx.cs" Inherits="Engage.Dnn.Survey.EditSurvey" %>
<asp:PlaceHolder ID="SummaryPlaceHolder" runat="server"></asp:PlaceHolder>
<asp:Table ID="SurveyTable" runat="server"></asp:Table>
<p>
    <asp:linkbutton cssclass="CommandButton" id="UpdateLinkButton" OnClick="UpdateLinkButton_Click" resourcekey="cmdUpdate" runat="server" borderstyle="none" text="Update"></asp:linkbutton>&nbsp;
    <asp:linkbutton cssclass="CommandButton" id="CancelLinkButton" OnClick="CancelLinkButton_Click" resourcekey="cmdCancel" runat="server" borderstyle="none" text="Cancel" causesvalidation="False"></asp:linkbutton>&nbsp;
    <asp:linkbutton cssclass="CommandButton" id="DeleteLinkButton" OnClick="DeleteLinkButton_Click" resourcekey="cmdDelete" runat="server" borderstyle="none" text="Delete" causesvalidation="False"></asp:linkbutton>&nbsp;
</p>