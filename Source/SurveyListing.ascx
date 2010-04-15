<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="SurveyListing.ascx.cs" Inherits="Engage.Dnn.Survey.SurveyListing" %>
<div class="survey-listing">
    <asp:Repeater ID="SurveyGrid" runat="server">
        <HeaderTemplate><ul class="sl-repeater"></HeaderTemplate>
        <ItemTemplate>
            <li class='<%# Container.ItemIndex % 2 == 0 ? "sl-item" : "sl-alt-item" %>'>
                <span class="sl-repeater-data">
                    <asp:HyperLink ID="TextHyperLink" runat="server" CssClass="sl-title NormalBold" />
                    <asp:Label ID="DescriptionLabel" runat="server" CssClass="sl-desc"/>
                </span>
                <span class="sl-repeater-actions">
                    <asp:HyperLink ID="EditHyperLink" runat="server" ResourceKey="EditLink" CssClass="ee-edit" />
                    <asp:HyperLink ID="AnalyzeLink" runat="server" ResourceKey="AnalyzeLink" CssClass="ee-analyze" />
                </span>
            </li>
        </ItemTemplate>
        <FooterTemplate></ul></FooterTemplate>
    </asp:Repeater>
    <ul class="ee-action-btns">
        <li class="primary-btn"><asp:linkbutton ID="NewSurveyButton" runat="server" Resourcekey="NewSurveyButton.Text" /></li>
    </ul>
</div>