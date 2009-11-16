<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="SurveyListing.ascx.cs" Inherits="Engage.Dnn.Survey.SurveyListing" %>
<div class="survey-listing">
    <div id="survey-filters">
        <h3><asp:Label ID="FilterLabel" runat="server" ResourceKey="FilterLabel.Text"/></h3>
        <asp:RadioButtonList ID="FilterRadioButtonList" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
            <asp:ListItem Selected="True" ResourceKey="Survey Definitions.Text" Value="Definition"/>
            <asp:ListItem ResourceKey="Completed Surveys.Text" Value="Completed"/>
        </asp:RadioButtonList>
    </div>
    <asp:Repeater ID="SurveyGrid" runat="server">
        <HeaderTemplate><ul class="sl-repeater"></HeaderTemplate>
        <ItemTemplate>
            <li class='<%# Container.ItemIndex % 2 == 0 ? "sl-item" : "sl-alt-item" %>'>
                <span class="sl-repeater-data">
                    <asp:HyperLink ID="TextHyperLink" runat="server" CssClass="sl-title NormalBold" />
                    <asp:Label ID="DateLabel" runat="server" CssClass="sl-date" />
                    <asp:Label ID="UserLabel" runat="server" CssClass="sl-user" />
                </span>
                <span class="sl-repeater-actions">
                    <asp:HyperLink ID="EditHyperLink" runat="server" ResourceKey="EditLink" CssClass="ee-edit" />
                    <asp:HyperLink ID="DeleteHyperLink" runat="server" ResourceKey="DeleteLink" CssClass="ee-delete" />
                </span>
            </li>
        </ItemTemplate>
        <FooterTemplate></ul></FooterTemplate>
    </asp:Repeater>
    <ul class="ee-action-btns">
        <li class="primary-btn"><asp:linkbutton ID="NewSurveyButton" Resourcekey="NewSurveyButton.Text" runat="server" /></li>
    </ul>
</div>