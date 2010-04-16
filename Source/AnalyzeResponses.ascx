<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="AnalyzeResponses.ascx.cs" Inherits="Engage.Dnn.Survey.AnalyzeResponses" %>
<%@ Import Namespace="System.Linq" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div class="survey-analysis">
    <div class="sa-survey">
        <h3 class="sa-survey-title"><%=HttpUtility.HtmlEncode(this.Survey.Text) %></h3>
        <p class="sa-survey-desc"><%=HttpUtility.HtmlEncode(this.Survey.Sections.First().Text) %></p>
    </div>
    <asp:PlaceHolder ID="ResponseGridPlaceholder" runat="server" />
</div>