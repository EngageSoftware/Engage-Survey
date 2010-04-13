<%@ Control Language="C#" AutoEventWireup="false" Inherits="Engage.Dnn.Survey.Settings" Codebehind="Settings.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<div id="engage-settings">
    <div class="ee-setting-item">
        <span class="ee-label"><dnn:Label ResourceKey="ChooseDisplayTypeLabel" runat="server" ControlName="ListingDisplayDropDownList" Suffix=":" /></span>
        <span class="ee-input"><asp:DropDownList ID="ListingDisplayDropDownList" runat="server" CssClass="NormalTextBox" AutoPostBack="true" /></span>
    </div>                                
    <asp:PlaceHolder ID="ViewSurveyPlaceholder" runat="server">
        <fieldset>
            <legend><%=Localize("View Survey Settings") %></legend>
            <div class="ee-setting-item">
                <span class="ee-label"><dnn:Label ResourceKey="SurveyTypeIdLabel" runat="server" ControlName="SurveyDropDownList" Suffix=":"/></span>
                <span class="ee-input"><asp:DropDownList ID="SurveyDropDownList" runat="server" CssClass="NormalTextBox" /></span>
            </div>
            <div class="ee-setting-item">
                <span class="ee-label"><dnn:Label ResourceKey="AllowMultipleLabel" runat="server" ControlName="AllowMultipleCheckBox" Suffix=":"/></span>
                <span class="ee-input"><asp:CheckBox ID="AllowMultipleCheckBox" runat="server" CssClass="NormalTextBox" /></span>
            </div>
            <div class="ee-setting-item">
                <span class="ee-label"><dnn:Label ResourceKey="ShowRequiredNotationLabel" runat="server" ControlName="ShowRequiredNotationCheckBox" Suffix=":"/></span>
                <span class="ee-input"><asp:CheckBox ID="ShowRequiredNotationCheckBox" runat="server" CssClass="NormalTextBox" /></span>
            </div>   
        </fieldset>
    </asp:PlaceHolder>
    <fieldset>
        <legend><%=Localize("Email Settings") %></legend>
        <div class="ee-setting-item">
            <span class="ee-label"><dnn:Label ResourceKey="SendNotificationLabel" runat="server" ControlName="SendNotificationCheckBox" Suffix=":"/></span>
            <span class="ee-input"><asp:CheckBox ID="SendNotificationCheckBox" runat="server" CssClass="NormalTextBox" /></span>
        </div>   
        <div class="ee-setting-item">
            <span class="ee-label"><dnn:Label ResourceKey="NotificationFromEmailLabel" runat="server" ControlName="NotificationFromEmailTextBox" Suffix=":"/></span>
            <span class="ee-input"><asp:TextBox ID="NotificationFromEmailTextBox" runat="server" CssClass="NormalTextBox" /></span>
        </div>   
        <div class="ee-setting-item">
            <span class="ee-label"><dnn:Label ResourceKey="NotificationToEmailsLabel" runat="server" ControlName="NotificationToEmailsTextBox" Suffix=":"/></span>
            <span class="ee-input"><asp:TextBox ID="NotificationToEmailsTextBox" runat="server" CssClass="NormalTextBox" /></span>
        </div>   
        <div class="ee-setting-item">
            <span class="ee-label"><dnn:Label ResourceKey="SendThankYouLabel" runat="server" ControlName="SendThankYouCheckBox" Suffix=":"/></span>
            <span class="ee-input"><asp:CheckBox ID="SendThankYouCheckBox" runat="server" CssClass="NormalTextBox" /></span>
        </div>   
        <div class="ee-setting-item">
            <span class="ee-label"><dnn:Label ResourceKey="ThankYouFromEmailLabel" runat="server" ControlName="ThankYouFromEmailTextBox" Suffix=":"/></span>
            <span class="ee-input"><asp:TextBox ID="ThankYouFromEmailTextBox" runat="server" CssClass="NormalTextBox" /></span>
        </div>
    </fieldset>

    <div class="em-user-message">
        <asp:RegularExpressionValidator ID="NotificationFromEmailPatternValidator" runat="server" Display="None" ControlToValidate="NotificationFromEmailTextBox" ResourceKey="NotificationFromEmailError" />
        <asp:RegularExpressionValidator ID="NotificationToEmailsPatternValidator" runat="server" Display="None" ControlToValidate="NotificationToEmailsTextBox" ResourceKey="NotificationToEmailsError" />
        <asp:RegularExpressionValidator ID="ThankYouFromEmailPatternValidator" runat="server" Display="None" ControlToValidate="ThankYouFromEmailTextBox" ResourceKey="ThankYouFromEmailError" />
        <asp:ValidationSummary runat="server" CssClass="em-error" ForeColor="" DisplayMode="List" />
    </div>
</div>
<script type="text/javascript">
    (function () {
        // fix bug where Validation Summary causes page to scroll to the top for no good reason
        // workaround from https://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=299399
        var originalValidationSummaryOnSubmit = window.ValidationSummaryOnSubmit;
        window.ValidationSummaryOnSubmit = function (validationGroup) {
            var originalScrollTo = window.scrollTo;
            window.scrollTo = function() { };
            originalValidationSummaryOnSubmit(validationGroup);
            window.scrollTo = originalScrollTo;
        }
    }());
</script>