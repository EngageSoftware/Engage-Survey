<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SurveyListing.ascx.cs" Inherits="Engage.Dnn.Survey.SurveyListing" %>
<asp:DataGrid ID="SurveyDataGrid" runat="server" AutoGenerateColumns="false" CellPadding="4"  GridLines="none" CellSpacing="0" 
    CssClass="Normal" EnableViewState="true">
    <Columns>
	<asp:TemplateColumn HeaderText= "">
		    <ItemTemplate>
		        <asp:HyperLink NavigateUrl= '<%# BuildEditUrl(DataBinder.Eval(Container.DataItem,"SurveyId")) %>' ID="EditHyperLink" runat="server" Text="edit" CssClass="Normal" Visible="<%# IsEditable%>"></asp:HyperLink>
		    </ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText= "">
		    <ItemTemplate>
		        <asp:HyperLink NavigateUrl= '<%# BuildPreviewUrl(DataBinder.Eval(Container.DataItem,"SurveyId")) %>' ID="PreviewHyperLink" runat="server" Text="preview" CssClass="Normal" Visible="true" ></asp:HyperLink>
		    </ItemTemplate>
		</asp:TemplateColumn>
    	<asp:TemplateColumn HeaderText= "">
		    <ItemTemplate>
		        <asp:Label Text='<%# DataBinder.Eval(Container.DataItem,"Text") %>' ID="TitleLabel" runat="server" CssClass="Normal" ></asp:Label>
		    </ItemTemplate>
		</asp:TemplateColumn>
    </Columns>
</asp:DataGrid>

