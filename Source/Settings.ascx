<%@ Control Language="C#" AutoEventWireup="false" Inherits="Engage.Dnn.Survey.Settings" Codebehind="Settings.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="telerik" assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" %>

<div id="engage-settings">
    <div class="ee-setting-item">
        <span class="ee-label"><dnn:Label ResourceKey="ChooseDisplayTypeLabel" runat="server" ControlName="ListingDisplayDropDownList" Suffix=":" /></span>
        <span class="ee-input"><asp:DropDownList ID="ListingDisplayDropDownList" runat="server" CssClass="NormalTextBox" AutoPostBack="true" /></span>
    </div>                                
    <fieldset class="ee-view-settings">
        <legend><%=Localize("View Survey Settings") %></legend>
        <div id="SurveySettingSection" runat="server" class="ee-setting-item ee-survey-setting">
            <span class="ee-label"><dnn:Label ResourceKey="SurveyTypeIdLabel" runat="server" ControlName="SurveyDropDownList" Suffix=":"/></span>
            <span class="ee-input"><asp:DropDownList ID="SurveyDropDownList" runat="server" CssClass="NormalTextBox" /></span>
        </div>
        <div class="ee-setting-item ee-multiple-setting">
            <span class="ee-label"><dnn:Label ResourceKey="AllowMultipleLabel" runat="server" ControlName="AllowMultipleCheckBox" Suffix=":"/></span>
            <span class="ee-input"><asp:CheckBox ID="AllowMultipleCheckBox" runat="server" CssClass="NormalTextBox" /></span>
        </div>
        <div class="ee-setting-item ee-required-setting">
            <span class="ee-label"><dnn:Label ResourceKey="ShowRequiredNotationLabel" runat="server" ControlName="ShowRequiredNotationCheckBox" Suffix=":"/></span>
            <span class="ee-input"><asp:CheckBox ID="ShowRequiredNotationCheckBox" runat="server" CssClass="NormalTextBox" /></span>
        </div>   
    </fieldset>
    <fieldset class="ee-spam-settings">
        <legend><%=Localize("Survey SPAM Protection Settings") %></legend>
        <div class="ee-setting-item ee-captcha-setting">
            <span class="ee-label"><dnn:Label ResourceKey="UseCaptchaProtection" runat="server" ControlName="UseCaptchaProtectionCheckBox" Suffix=":"/></span>
            <span class="ee-input"><asp:CheckBox ID="UseCaptchaProtectionCheckBox" runat="server" CssClass="NormalTextBox" /></span>
        </div>   
        <div class="ee-setting-item ee-honeypot-setting">
            <span class="ee-label"><dnn:Label ResourceKey="UseInvisibleTextBoxProtection" runat="server" ControlName="UseInvisibleTextBoxProtectionCheckBox" Suffix=":"/></span>
            <span class="ee-input"><asp:CheckBox ID="UseInvisibleTextBoxProtectionCheckBox" runat="server" CssClass="NormalTextBox" /></span>
        </div>   
        <div class="ee-setting-item ee-bot-timer-setting">
            <span class="ee-label"><dnn:Label ResourceKey="UseMinimumTimeoutProtection" runat="server" ControlName="UseMinimumTimeoutProtectionCheckBox" Suffix=":"/></span>
            <span class="ee-input"><asp:CheckBox ID="UseMinimumTimeoutProtectionCheckBox" runat="server" CssClass="NormalTextBox" AutoPostBack="True" /></span>
        </div>   
        <div class="ee-setting-item ee-bot-timeout-setting">
            <span class="ee-label"><dnn:Label ResourceKey="MinimumTimeoutLength" runat="server" ControlName="MinimumTimeoutTextBox" Suffix=":"/></span>
            <span class="ee-input">
                <telerik:RadNumericTextBox ID="MinimumTimeoutTextBox" runat="server" CssClass="NormalTextBox" MinValue="1" MaxValue="15" AllowOutOfRangeAutoCorrect="True" NumberFormat-DecimalDigits="0" Width="50px" />
                <asp:Label runat="server" ResourceKey="seconds" />
            </span>
            <asp:RequiredFieldValidator ID="MinimumTimeoutRequiredValidator" runat="server" Display="None" ControlToValidate="MinimumTimeoutTextBox" ResourceKey="MinimumTimeoutTextBox.Required" />
        </div>
    </fieldset>
    <fieldset class="ee-email-settings">
        <legend><%=Localize("Email Settings") %></legend>
        <div class="ee-setting-item ee-notification-setting">
            <span class="ee-label"><dnn:Label ResourceKey="SendNotificationLabel" runat="server" ControlName="SendNotificationCheckBox" Suffix=":"/></span>
            <span class="ee-input"><asp:CheckBox ID="SendNotificationCheckBox" runat="server" CssClass="NormalTextBox" /></span>
        </div>   
        <div class="ee-setting-item ee-notification-from-setting">
            <span class="ee-label"><dnn:Label ResourceKey="NotificationFromEmailLabel" runat="server" ControlName="NotificationFromEmailTextBox" Suffix=":"/></span>
            <span class="ee-input"><asp:TextBox ID="NotificationFromEmailTextBox" runat="server" CssClass="NormalTextBox" /></span>
            <asp:RegularExpressionValidator ID="NotificationFromEmailPatternValidator" runat="server" Display="None" ControlToValidate="NotificationFromEmailTextBox" ResourceKey="NotificationFromEmailError" />
        </div>   
        <div class="ee-setting-item ee-notification-to-setting">
            <span class="ee-label"><dnn:Label ResourceKey="NotificationToEmailsLabel" runat="server" ControlName="NotificationToEmailsTextBox" Suffix=":"/></span>
            <span class="ee-input"><asp:TextBox ID="NotificationToEmailsTextBox" runat="server" CssClass="NormalTextBox" /></span>
            <asp:RegularExpressionValidator ID="NotificationToEmailsPatternValidator" runat="server" Display="None" ControlToValidate="NotificationToEmailsTextBox" ResourceKey="NotificationToEmailsError" />
        </div>   
        <div class="ee-setting-item ee-thank-you-setting">
            <span class="ee-label"><dnn:Label ResourceKey="SendThankYouLabel" runat="server" ControlName="SendThankYouCheckBox" Suffix=":"/></span>
            <span class="ee-input"><asp:CheckBox ID="SendThankYouCheckBox" runat="server" CssClass="NormalTextBox" /></span>
        </div>   
        <div class="ee-setting-item ee-thank-you-from-setting">
            <span class="ee-label"><dnn:Label ResourceKey="ThankYouFromEmailLabel" runat="server" ControlName="ThankYouFromEmailTextBox" Suffix=":"/></span>
            <span class="ee-input"><asp:TextBox ID="ThankYouFromEmailTextBox" runat="server" CssClass="NormalTextBox" /></span>
            <asp:RegularExpressionValidator ID="ThankYouFromEmailPatternValidator" runat="server" Display="None" ControlToValidate="ThankYouFromEmailTextBox" ResourceKey="ThankYouFromEmailError" />
        </div>
    </fieldset>

    <div class="em-user-message">
        <asp:ValidationSummary runat="server" CssClass="em-error em-message" ForeColor="" DisplayMode="List" />
    </div>
</div>
<script type="text/javascript">
    (function () {
        // fix bug where Validation Summary causes page to scroll to the top for no good reason
        // workaround from https://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=299399
        var originalValidationSummaryOnSubmit = window.ValidationSummaryOnSubmit;
        window.ValidationSummaryOnSubmit = function (validationGroup) {
            var originalScrollTo = window.scrollTo;
            window.scrollTo = function () { };
            originalValidationSummaryOnSubmit(validationGroup);
            window.scrollTo = originalScrollTo;
        };
    }());
</script>