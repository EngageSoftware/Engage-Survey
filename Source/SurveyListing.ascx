<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="SurveyListing.ascx.cs" Inherits="Engage.Dnn.Survey.SurveyListing" %>

<div id="survey-filters">
    <p><asp:Label ID="FilterLabel" runat="server" ResourceKey="FilterLabel.Text"/></p>
    <asp:RadioButtonList ID="FilterRadioButtonList" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
        <asp:ListItem Selected="True" ResourceKey="Survey Definitions.Text" Value="0"/>
        <asp:ListItem ResourceKey="Completed Surveys.Text" Value="1"/>
    </asp:RadioButtonList>
</div>
<asp:DataGrid ID="SurveyDataGrid" runat="server" AutoGenerateColumns="false" GridLines="None" CssClass="Normal" EnableViewState="true">
    <Columns>
	<asp:TemplateColumn>
		    <ItemTemplate>
		        <asp:HyperLink ID="EditHyperLink" runat="server" ResourceKey="EditLink.Text" CssClass="CommandButton" Visible="<%# IsEditable%>"/>
		    </ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn>
		    <ItemTemplate>
		        <asp:HyperLink ID="PreviewHyperLink" runat="server" ResourceKey="PreviewLink.Text" CssClass="CommandButton" />
		    </ItemTemplate>
		</asp:TemplateColumn>
    	<asp:TemplateColumn>
		    <ItemTemplate>
		        <asp:Label ID="TextLabel" runat="server" CssClass="Normal"/>
		    </ItemTemplate>
		</asp:TemplateColumn>
    </Columns>
</asp:DataGrid>

<div>
    <asp:linkbutton CssClass="CommandButton" ID="NewSurveyButton" Resourcekey="NewSurveyButton.Text" runat="server" />&nbsp;
    <asp:linkbutton CssClass="CommandButton" ID="CancelButton" ResourceKey="CancelButton.Text" runat="server" CausesValidation="False"/>&nbsp;
</div>

