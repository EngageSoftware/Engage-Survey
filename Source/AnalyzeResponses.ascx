<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="AnalyzeResponses.ascx.cs" Inherits="Engage.Dnn.Survey.AnalyzeResponses" %>
<%@ Import Namespace="System.Linq" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div class="survey-analysis">
    <div class="sa-survey">
        <h3 class="sa-survey-title"><%=HttpUtility.HtmlEncode(this.Survey.Text) %></h3>
        <p class="sa-survey-desc"><%=HttpUtility.HtmlEncode(this.Survey.Sections.First().Text) %></p>
    </div>
    <telerik:RadGrid ID="ResponseGrid" runat="server" Skin="Simple" CssClass="sa-grid" AutoGenerateColumns="false" GridLines="None" AllowPaging="true" AllowCustomPaging="true" PageSize="25">
        <ExportSettings ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true"/>
        <MasterTableView CommandItemDisplay="TopAndBottom">
            <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" />
            <CommandItemSettings ShowExportToWordButton="true" ShowExportToExcelButton="true" ShowExportToCsvButton="true" ShowExportToPdfButton="true" />
            <NoRecordsTemplate>
                <h3 class="no-responses"><%=Localize("No Responses.Text") %></h3>
            </NoRecordsTemplate>
            <Columns>
                <telerik:GridBoundColumn DataField="ResponseHeaderId" HeaderText="Response ID" ItemStyle-CssClass="sa-id" />
                <telerik:GridBoundColumn DataField="CreationDate" HeaderText="Date" ItemStyle-CssClass="sa-date" />
                <telerik:GridTemplateColumn HeaderText="User" ItemStyle-CssClass="sa-user">
                    <ItemTemplate><%#GetUser((int?)Eval("UserId")) %></ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="View" ItemStyle-CssClass="sa-view sa-action-btn">
                    <ItemTemplate><asp:HyperLink runat="server" ResourceKey="ViewLink" NavigateUrl='<%#GetViewLink((int)Eval("ResponseHeaderId")) %>' /></ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</div>