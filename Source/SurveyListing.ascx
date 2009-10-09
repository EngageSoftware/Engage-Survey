<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SurveyListing.ascx.cs" Inherits="Engage.Dnn.Survey.SurveyListing" %>

<div id="survey-filters">
    <asp:RadioButtonList ID="FilterRadioButtonList" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="FilterRadioButtonList_SelectedIndexChanged" AutoPostBack="true">
        <asp:ListItem Selected="True" Text="Survey Definitions" Value="0"></asp:ListItem>
        <asp:ListItem Text="Completed Surveys" Value="1"></asp:ListItem>
    </asp:RadioButtonList>
    <p><asp:Label ID="FilterLabel" runat="server" ResourceKey="FilterLabel"></asp:Label></p>
</div>
<asp:DataGrid ID="SurveyDataGrid" runat="server" AutoGenerateColumns="false" CellPadding="4"  GridLines="none" CellSpacing="0" 
    CssClass="Normal" EnableViewState="true" OnItemDataBound="SurveyDataGrid_OnItemDataBound">
    <Columns>
	<asp:TemplateColumn HeaderText= "">
		    <ItemTemplate>
		        <asp:HyperLink NavigateUrl= ID="EditHyperLink" runat="server" Text="edit" CssClass="Normal" Visible="<%# IsEditable%>"></asp:HyperLink>
		    </ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText= "">
		    <ItemTemplate>
		        <asp:HyperLink NavigateUrl= ID="PreviewHyperLink" runat="server" Text="preview" CssClass="Normal" Visible="true" ></asp:HyperLink>
		    </ItemTemplate>
		</asp:TemplateColumn>
    	<asp:TemplateColumn HeaderText= "">
		    <ItemTemplate>
		        <asp:Label ID="TextLabel" runat="server" CssClass="Normal" ></asp:Label>
		    </ItemTemplate>
		</asp:TemplateColumn>
    </Columns>
</asp:DataGrid>

<div align="center">
    <asp:linkbutton cssclass="CommandButton" id="NewLinkButton" OnClick="NewLinkButton_Click" resourcekey="cmdNew" runat="server" borderstyle="none" text="Create Survey"></asp:linkbutton>&nbsp;
    <asp:linkbutton cssclass="CommandButton" id="CancelLinkButton" OnClick="CancelLinkButton_Click" resourcekey="cmdCancel" runat="server" borderstyle="none" text="Cancel" causesvalidation="False"></asp:linkbutton>&nbsp;
</div>

