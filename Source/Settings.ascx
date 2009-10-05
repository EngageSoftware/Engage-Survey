<%@ Control Language="C#" AutoEventWireup="false" Inherits="Engage.Dnn.Survey.Settings" Codebehind="Settings.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<table cellspacing="0" cellpadding="2" border="0">
    <tr>
        <td class="SubHead"><dnn:label id="ChooseDisplayTypeLabel" resourcekey="ChooseDisplayTypeLabel" runat="server" /></td>
        <td class="NormalTextBox"><asp:dropdownlist id="ListingDisplayDropDownList" Runat="server" AutoPostBack="true" OnSelectedIndexChanged="ListingDisplayDropDownList_SelectedIndexChanged"></asp:dropdownlist></td>
    </tr>
</table>                                
<asp:Panel ID="ViewSurveyPanel" runat="server" Visible="true">
    <table cellspacing="0" cellpadding="2" border="0" width="100%">
        <tr>
            <td class="SubHead" width="200"><dnn:label id="SurveyTypeIdLabel" runat="server" controlname="SurveyDropDownList" suffix=":"></dnn:label></td>
            <td valign="bottom">
                <asp:DropDownList id="SurveyDropDownList" cssclass="NormalTextBox" Width="200" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="SubHead" width="200"><dnn:label id="AllowMultipleLabel" runat="server" controlname="AllowMultipleCheckBox" suffix=":"></dnn:label></td>
            <td valign="bottom" >
                <asp:CheckBox id="AllowMultipleCheckBox" cssclass="NormalTextBox" Checked="true" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="SubHead" width="200"><dnn:label id="ShowRequiredNotationLabel" runat="server" controlname="ShowRequiredNotationCheckBox" suffix=":"></dnn:label></td>
            <td valign="bottom" >
                <asp:CheckBox id="ShowRequiredNotationCheckBox" cssclass="NormalTextBox" Checked="false" runat="server" />
            </td>
        </tr>   
    </table>
</asp:Panel>
