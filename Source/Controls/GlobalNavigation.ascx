<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="GlobalNavigation.ascx.cs" Inherits="Engage.Dnn.Survey.GlobalNavigation" %>
<ul class="em-global-nav">
	<li class="egn-home"><asp:HyperLink ID="HomeLink" runat="server" ResourceKey="HomeLink"/></li>
    <li class="egn-add-new"><asp:HyperLink ID="AddNewLink" runat="server" ResourceKey="AddNewLink" /></li>
    <li id="ManageListItem" runat="server" class="egn-manage"><asp:HyperLink ID="ManageLink" runat="server" ResourceKey="ManageLink" /></li>    
    <li class="egn-settings"><asp:HyperLink ID="SettingsLink" runat="server" ResourceKey="SettingsLink" /></li>
</ul>
