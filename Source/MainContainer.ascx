<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Survey.MainContainer" Codebehind="MainContainer.ascx.cs" %>
<%@ Register TagPrefix="engage" TagName="GlobalNav" Src="Controls/GlobalNavigation.ascx" %>
<div style="clear:both;">
    <engage:GlobalNav ID="GlobalNavigation" runat="server" />
    <asp:PlaceHolder id="SubControlPlaceholder" runat="server" />
</div>