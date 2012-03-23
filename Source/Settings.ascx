<%@ Control Language="C#" AutoEventWireup="false" Inherits="Engage.Dnn.Survey.Settings" Codebehind="Settings.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="telerik" assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" %>

<div id="engage-settings" class="eng-form">
    <asp:ValidationSummary runat="server" ShowMessageBox="false" ShowSummary="true" CssClass="NormalRed" />
    <fieldset class="ee-display-settings">
        <legend><%= Localize("Display Type") %></legend>
        <ul class="eng-form-items">
            <li class="eng-form-item eng-medium ee-display-setting">
                <dnn:Label ResourceKey="ChooseDisplayTypeLabel" runat="server" ControlName="ListingDisplayDropDownList" Suffix=":" />
                <asp:DropDownList ID="ListingDisplayDropDownList" runat="server" AutoPostBack="true" />
            </li>
        </ul>
    </fieldset>    
    <fieldset class="ee-view-settings">
        <legend><%=Localize("View Survey Settings") %></legend>
        <ul class="eng-form-items">
            <li id="SurveySettingSection" runat="server" class="eng-form-item eng-large ee-survey-setting">
                <dnn:Label ResourceKey="SurveyTypeIdLabel" runat="server" ControlName="SurveyDropDownList" Suffix=":"/>
                <asp:DropDownList ID="SurveyDropDownList" runat="server" />
            </li>
            <li class="eng-form-item eng-tiny ee-multiple-setting">
                <dnn:Label ResourceKey="AllowMultipleLabel" runat="server" ControlName="AllowMultipleCheckBox" Suffix=":"/>
                <asp:CheckBox ID="AllowMultipleCheckBox" runat="server" />
            </li>
            <li class="eng-form-item eng-tiny ee-required-setting">
                <dnn:Label ResourceKey="ShowRequiredNotationLabel" runat="server" ControlName="ShowRequiredNotationCheckBox" Suffix=":"/>
                <asp:CheckBox ID="ShowRequiredNotationCheckBox" runat="server" />
            </li>   
        </ul>
    </fieldset>
    <fieldset class="ee-spam-settings">
        <legend><%=Localize("Survey SPAM Protection Settings") %></legend>
        <ul class="eng-form-items">
            <li class="eng-form-item eng-tiny ee-captcha-setting">
                <dnn:Label ResourceKey="UseCaptchaProtection" runat="server" ControlName="UseCaptchaProtectionCheckBox" Suffix=":"/>
                <asp:CheckBox ID="UseCaptchaProtectionCheckBox" runat="server" />
            </li>   
            <li class="eng-form-item eng-tiny ee-honeypot-setting">
                <dnn:Label ResourceKey="UseInvisibleTextBoxProtection" runat="server" ControlName="UseInvisibleTextBoxProtectionCheckBox" Suffix=":"/>
                <asp:CheckBox ID="UseInvisibleTextBoxProtectionCheckBox" runat="server" />
            </li>   
            <li class="eng-form-item eng-tiny ee-bot-timer-setting">
                <dnn:Label ResourceKey="UseMinimumTimeoutProtection" runat="server" ControlName="UseMinimumTimeoutProtectionCheckBox" Suffix=":"/>
                <asp:CheckBox ID="UseMinimumTimeoutProtectionCheckBox" runat="server" AutoPostBack="True" />
            </li>   
            <li class="eng-form-item eng-small ee-bot-timeout-setting">
                <dnn:Label ResourceKey="MinimumTimeoutLength" runat="server" ControlName="MinimumTimeoutTextBox" Suffix=":"/>
                <telerik:RadNumericTextBox ID="MinimumTimeoutTextBox" runat="server" MinValue="1" MaxValue="15" AllowOutOfRangeAutoCorrect="True" NumberFormat-DecimalDigits="0" />
                <asp:Label runat="server" ResourceKey="seconds" />
                <asp:RequiredFieldValidator ID="MinimumTimeoutRequiredValidator" runat="server" CssClass="NormalRed" Display="None" ControlToValidate="MinimumTimeoutTextBox" ResourceKey="MinimumTimeoutTextBox.Required" />
            </li>
        </ul>
    </fieldset>
    <fieldset class="ee-email-settings">
        <legend><%=Localize("Email Settings") %></legend>
        <ul class="eng-form-items">
            <li class="eng-form-item eng-tiny ee-notification-setting">
                <dnn:Label ResourceKey="SendNotificationLabel" runat="server" ControlName="SendNotificationCheckBox" Suffix=":"/>
                <asp:CheckBox ID="SendNotificationCheckBox" runat="server" />
            </li>   
            <li class="eng-form-item eng-large ee-notification-from-setting">
                <dnn:Label ResourceKey="NotificationFromEmailLabel" runat="server" ControlName="NotificationFromEmailTextBox" Suffix=":"/>
                <asp:TextBox ID="NotificationFromEmailTextBox" runat="server" />
                <asp:RegularExpressionValidator ID="NotificationFromEmailPatternValidator" runat="server" CssClass="NormalRed" Display="None" ControlToValidate="NotificationFromEmailTextBox" ResourceKey="NotificationFromEmailError" />
            </li>   
            <li class="eng-form-item eng-large ee-notification-to-setting">
                <dnn:Label ResourceKey="NotificationToEmailsLabel" runat="server" ControlName="NotificationToEmailsTextBox" Suffix=":"/>
                <asp:TextBox ID="NotificationToEmailsTextBox" runat="server" />
                <asp:RegularExpressionValidator ID="NotificationToEmailsPatternValidator" runat="server" CssClass="NormalRed" Display="None" ControlToValidate="NotificationToEmailsTextBox" ResourceKey="NotificationToEmailsError" />
            </li>   
            <li class="eng-form-item eng-tiny ee-thank-you-setting">
                <dnn:Label ResourceKey="SendThankYouLabel" runat="server" ControlName="SendThankYouCheckBox" Suffix=":"/>
                <asp:CheckBox ID="SendThankYouCheckBox" runat="server" />
            </li>   
            <li class="eng-form-item eng-large ee-thank-you-from-setting">
                <dnn:Label ResourceKey="ThankYouFromEmailLabel" runat="server" ControlName="ThankYouFromEmailTextBox" Suffix=":"/>
                <asp:TextBox ID="ThankYouFromEmailTextBox" runat="server" />
                <asp:RegularExpressionValidator ID="ThankYouFromEmailPatternValidator" runat="server" CssClass="NormalRed" Display="None" ControlToValidate="ThankYouFromEmailTextBox" ResourceKey="ThankYouFromEmailError" />
            </li>
        </ul>
    </fieldset>
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
</div>
<div class="eng-end-form"></div>