<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditSurvey.ascx.cs" Inherits="Engage.Dnn.Survey.EditSurvey" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Import Namespace="Engage.Survey.Util"%>

<div id="engage-evaluation">
    <p class="ee-note"><%=Localize("RequiredNote.Text")%></p>
    <fieldset class="ee-create-new">
        <legend class="Head"><%=Localize("CreateNewLabel.Text")%></legend>
        <div class="ee-title">
            <span class="ee-label eval-title"><span class="ee-required-label">*</span><%=Localize("EvalTitleLabel.Text")%></span>
            <span class="ee-input"><input class="NormalTextBox ee-required" id="EvalTitleInput" minlength="1" maxlength="256" name="required" /></span>
        </div>
        <div class="ee-description ee-optional">
            <span class="ee-label eval-description"><%=Localize("EvalDescriptionLabel.Text")%></span>
            <span class="ee-input"><textarea id="EvalDescTextArea" class="NormalTextBox" cols="40" rows="4"></textarea></span>
        </div>
        <fieldset class="ee-timeframe ee-collapsed ee-optional">
            <legend><a href="#" class="CommandButton"><%=Localize("EvalTimeframeLabel.Text")%></a></legend>
            <div class="ee-collapsable">
                <div class="ee-start-date ee-optional">
                    <span class="ee-label eval-start-date"><%=Localize("EvalStartDateLabel.Text")%></span>
                    <span class="ee-input"><telerik:RadDateTimePicker runat="server" Calendar-ShowRowHeaders="false" Skin="Simple" /></span>
                </div>
                <div class="ee-pre-start ee-optional">
                    <span class="ee-label eval-pre-start"><%=Localize("EvalPreStartLabel.Text")%></span>
                    <span class="ee-input"><textarea id="EvalPreStartTextArea" class="NormalTextBox" cols="40" rows="4"></textarea></span>
                </div>
                <div class="ee-end-date ee-optional">
                    <span class="ee-label eval-end-date"><%=Localize("EvalEndDateLabel.Text")%></span>
                    <span class="ee-input"><telerik:RadDateTimePicker runat="server" Calendar-ShowRowHeaders="false" Skin="Simple" /></span>
                </div>
                <div class="ee-post-end ee-optional">
                    <span class="ee-label eval-post-end"><%=Localize("EvalPostEndLabel.Text")%></span>
                    <span class="ee-input"><textarea id="EvalPostEndTextArea" class="NormalTextBox" cols="40" rows="4"></textarea></span>
                </div>
            </div>
        </fieldset>
        <fieldset class="ee-email ee-collapsed ee-optional">
            <legend><a href="#" class="CommandButton"><%=Localize("EvalEmailLabel.Text")%></a></legend>
            <div class="ee-collapsable">
                <div class="ee-notification ee-optional">
                    <span class="ee-label eval-notification"><%=Localize("EvalSendNotificationEmailLabel.Text")%></span>
                    <span class="ee-input"><input id="EvalSendNotification" type="checkbox" /></span>
                </div>
                <div class="ee-notification-from ee-optional">
                    <span class="ee-label eval-notification-from"><%=Localize("EvalNotificationFromEmailLabel.Text")%></span>
                    <span class="ee-input"><input id="EvalNotificationFromEmail" type="text" class="NormalTextBox" maxlength="320" name="notificationEmail"/></span>
                </div>
                <div class="ee-notification-to ee-optional">
                    <span class="ee-label eval-notification-to"><%=Localize("EvalNotificationToEmailsLabel.Text")%></span>
                    <span class="ee-input"><input id="EvalNotificationToEmails" type="text" class="NormalTextBox" maxlength="" name="notificationEmails" /></span>
                </div>
                <div class="ee-thankyou ee-optional">
                    <span class="ee-label eval-thankyou"><%=Localize("EvalSendThankYouEmailLabel.Text")%></span>
                    <span class="ee-input"><input id="EvalSendThankYou" type="checkbox" /></span>
                </div>
                <div class="ee-thankyou-from ee-optional">
                    <span class="ee-label eval-thankyou-from"><%=Localize("EvalThankYouFromEmailLabel.Text")%></span>
                    <span class="ee-input"><input id="EvalThankYouFromEmail" type="text" class="NormalTextBox" maxlength="320" name="thankYouEmail"/></span>
                </div>
            </div>
        </fieldset>
        <fieldset class="ee-completion ee-collapsed ee-optional">
            <legend><a href="#" class="CommandButton"><%=Localize("EvalCompletionLabel.Text")%></a></legend>
            <div class="ee-collapsable">
                <div class="ee-completion-action ee-optional">
                    <span class="ee-label eval-completion-action"><%=Localize("EvalCompletionActionLabel.Text")%></span>
                    <span class="ee-input">
                        <select id="EvalCompletionAction" class="NormalTextBox">
                            <option value="<%=(int)FinalMessageOption.UseFinalMessage %>"><%=Localize("CompletionActionMessage.Text") %></option>
                            <option value="<%=(int)FinalMessageOption.UseFinalURL %>"><%=Localize("CompletionActionUrl.Text") %></option>
                        </select>
                    </span>
                </div>
                <div class="ee-completion-message ee-optional">
                    <span class="ee-label eval-completion-message"><%=Localize("EvalCompletionMessageLabel.Text")%></span>
                    <span class="ee-input"><textarea id="EvalCompletionMessage" class="NormalTextBox" rows="5" cols="20" name="completionMessage"></textarea></span>
                </div>
                <div class="ee-completion-url ee-optional">
                    <span class="ee-label eval-completion-url"><%=Localize("EvalCompletionUrlLabel.Text")%></span>
                    <span class="ee-input"><input id="EvalCompletionUrl" type="text" class="NormalTextBox" name="completionUrl" /></span>
                </div>
            </div>
        </fieldset>
        <ul class="ee-action-btns">
            <li class="primary-btn"><a href="#" title="<%=HttpUtility.HtmlAttributeEncode(Localize("CreateNewEvalHyperLink.ToolTip")) %>" id="EvalNew" class="create-new"><%=Localize("CreateNewEvalHyperLink.Text")%></a></li>
            <li class="primary-btn" style="display:none;"><a href="#" title="<%=HttpUtility.HtmlAttributeEncode(Localize("EditEvalHyperLink.ToolTip")) %>" id="EvalEdit"><%=Localize("EditEvalHyperLink.Text")%></a></li>
            <li class="primary-btn" style="display:none;"><a href="#" title="<%=HttpUtility.HtmlAttributeEncode(Localize("UpdateEvalHyperLink.ToolTip")) %>" id="EvalUpdate"><%=Localize("UpdateEvalHyperLink.Text")%></a></li>
            <li class="secondary-btn" style="display:none;"><a href="#" title="<%=HttpUtility.HtmlAttributeEncode(Localize("CancelHyperLink.ToolTip")) %>" id="EvalCancel"><%=Localize("CancelHyperLink.Text")%></a></li>
            <li class="secondary-btn" style="display:none;"><a href="#" title='<%=HttpUtility.HtmlAttributeEncode(Localize("DeleteHyperLink.ToolTip")) %>' id="EvalDelete"><%=Localize("DeleteHyperLink.Text")%></a></li>
        </ul>
    </fieldset>
    <fieldset class="ee-create-questions" id="CreateQuestions">
        <legend class="Head"><%=Localize("CreateNewQuestionsLabel.Text")%></legend>
        <div class="ee-question">
            <span class="ee-label"><span class="ee-required-label">*</span><%=Localize("TypeQuestionLabel.Text")%></span>
            <span class="ee-input"><textarea id="QuestionText" class="NormalTextBox ee-required" minlength="1" maxlength="256" cols="40" rows="4" name="required"></textarea></span>
        </div>
        <div class="ee-question-required">
            <span class="ee-label"><span class="ee-required-label">*</span><%=Localize("QuestionRequiredLabel.Text")%></span>
            <span class="ee-input"><input type="checkbox" id="QuestionRequiredCheckBox" class="ee-required" checked="checked" /></span>
        </div>
        <div class="ee-define-answer">
            <span class="ee-label"><span class="ee-required-label">*</span><%=Localize("DefineAnswerLabel.Text")%></span>
            <div class="define-answer">
                <span class="ee-input">
                    <select class="NormalTextBox answer-options" name="DefineAnswerType" id="DefineAnswerType">
                        <option value='<%=(int)ControlType.None %>'><%=Localize("SelectAnswerTypeOption.Text")%></option>
                        <option value='<%=(int)ControlType.SmallTextInputField %>'><%=Localize("ShortAnswerOption.Text")%></option>
                        <option value='<%=(int)ControlType.LargeTextInputField %>'><%=Localize("LongAnswerOption.Text")%></option>
                        <option value='<%=(int)ControlType.DropDownChoices %>'><%=Localize("SingleAnwserDropdownListOption.Text")%></option>
                        <option value='<%=(int)ControlType.VerticalOptionButtons %>'><%=Localize("SingleAnswerRadioButtonOption.Text")%></option>
                        <option value='<%=(int)ControlType.Checkbox %>'><%=Localize("MultipleAnswerCheckboxesOption.Text")%></option>
                    </select>
                </span>
                <div id="ShortTextAnswer" style="display:none;" class="ee-answer-inputs">
                    <p><%=Localize("ShortAnswerPreview.Text") %></p>
                    <img src='<%=ResolveUrl("images/short-answer-input.gif")%>' alt="<%=HttpUtility.HtmlAttributeEncode(Localize("ShortAnswerPreviewImage.AltText")) %>" title="<%=HttpUtility.HtmlAttributeEncode(Localize("ShortAnswerPreviewImage.ToolTip")) %>" />
                </div>
                <div id="LongTextAnswer" style="display:none;" class="ee-answer-inputs">
                    <p><%=Localize("LongAnswerPreview.Text") %></p>
                    <img src='<%=ResolveUrl("images/long-answer-input.gif")%>' alt="<%=HttpUtility.HtmlAttributeEncode(Localize("LongAnswerPreviewImage.AltText")) %>" title="<%=HttpUtility.HtmlAttributeEncode(Localize("LongAnswerPreviewImage.ToolTip")) %>" />
                </div>
                <div class="ee-answer-inputs" id="MultipleAnswer" style="display:none;">
                    <ul class="answer-inputs">
                        <li class="answer-input-template" style="display: none;">
                            <span class="ai-label"><span class="ee-required-label">*</span><%=Localize("AnswerTitle.Text")%> <span class="answer-num">1</span></span>
                            <div class="ai-selected">
                                <span class="ai-input"><input class="NormalTextBox" type="text" maxlength="256" name="required" /></span>
                                <a href="#" title="<%=HttpUtility.HtmlAttributeEncode(Localize("RemoveAnswerHyperLink.ToolTip"))%>" class="ee-delete"><%=Localize("RemoveAnswerHyperLink.Text")%></a>
                            </div>
                        </li>
                        <li class="ee-undo template" style="display:none;">
                            <%=Localize("UndoAnswerDelete.Text") %> <a href="#" title="<%=HttpUtility.HtmlAttributeEncode(Localize("UndoAnswerDeleteButton.ToolTip")) %>"><%=Localize("UndoAnswerDeleteButton.Text") %></a>
                        </li>
                    </ul>
                    <ul class="ee-action-btns">
                        <li class="primary-btn" style="display:none;"><a href="#" title="<%=HttpUtility.HtmlAttributeEncode(Localize("AddNewAnswerHyperLink.ToolTip"))%>" class="add-new" id="AddNewQuestion"><%=Localize("AddNewAnswerHyperLink.Text")%></a></li>
                    </ul>
                </div>
            </div>
        </div>
        <ul class="ee-action-btns">
            <li class="primary-btn disabled"><a href="#" title="<%=HttpUtility.HtmlAttributeEncode(Localize("SaveQuestion.ToolTip"))%>" class="save-create-new" id="SaveQuestion"><%=Localize("SaveQuestion.Text")%></a></li>
            <li class="secondary-btn" style="display:none;"><a href="#" id="CancelQuestion" title="<%=HttpUtility.HtmlAttributeEncode(Localize("CancelQuestionHyperLink.ToolTip"))%>"><%=Localize("CancelQuestionHyperLink.Text")%></a></li>
        </ul>
    </fieldset>
    <fieldset id="PreviewArea" class="ee-preview-area">
        <legend class="Head"><%=Localize("PreviewAreaLabel.Text")%></legend>
        <hr />
        <ul id="ee-previews">
            <li class="ee-preview-template" style="display:none;">
                <ul class="ee-pr-action-links">
                    <li><a href="#" title="<%=HttpUtility.HtmlAttributeEncode(Localize("EditQuestionHyperLink.ToolTip"))%>" class="ee-edit"><%=Localize("EditQuestionHyperLink.Text")%></a></li>
                    <li><a href="#" title="<%=HttpUtility.HtmlAttributeEncode(Localize("CopyQuestionHyperLink.ToolTip"))%>" class="ee-copy"><%=Localize("CopyQuestionHyperLink.Text")%></a></li>
                    <li><a href="#" title="<%=HttpUtility.HtmlAttributeEncode(Localize("DeleteQuestionHyperLink.ToolTip"))%>" class="ee-delete"><%=Localize("DeleteQuestionHyperLink.Text")%></a></li>
                </ul>
                <span class="ee-label pv-question-label"><%=Localize("QuestionTitle.Text")%></span>
                <span class="pv-question"></span><span class="ee-required-label"></span>
                <div>
                    <span class="ee-label pv-answer-label"><%=Localize("AnswerTitle.Text")%></span>
                    <span class="pv-answer"></span>
                </div>
            </li>
            <li class="ee-undo" style="display:none;">
                <%=Localize("UndoQuestionDelete.Text") %> <a href="#" title="<%=HttpUtility.HtmlAttributeEncode(Localize("UndoQuestionDeleteButton.ToolTip")) %>"><%=Localize("UndoQuestionDeleteButton.Text") %></a>
            </li>
        </ul>
    </fieldset>   
</div>
<span class="ee-undo template" style="display:none;">
    <%=Localize("UndoSurveyDelete.Text") %> <a href="#" title="<%=HttpUtility.HtmlAttributeEncode(Localize("UndoSurveyDeleteButton.ToolTip")) %>"><%=Localize("UndoSurveyDeleteButton.Text") %></a>
</span>

<% if (false) { %><script type="text/ecmascript" src="JavaScript/jquery-1.3.2.debug-vsdoc.js"></script><% } %>
<script type="text/javascript">
var currentContextInfo = {
    WebMethodUrl: '<%= ResolveUrl("ClientService.asmx") %>/',
    UserId: <%=UserId %>,    
    Survey: <%=SerializedSurvey %>,
    PortalId: <%= PortalId %>,
    ModuleId: <%= ModuleId %>,
    ErrorMessage: <%= GetJavaScriptString(Localize("AjaxError.Text")) %>,
    SaveQuestionButtonText: <%= GetJavaScriptString(Localize("SaveQuestion.Text")) %>,
    SaveQuestionToolTip: <%= GetJavaScriptString(Localize("SaveQuestion.ToolTip")) %>,
    UpdateQuestionButtonText: <%= GetJavaScriptString(Localize("UpdateQuestion.Text")) %>,
    UpdateQuestionToolTip: <%= GetJavaScriptString(Localize("UpdateQuestion.ToolTip")) %>,
    ProgressText: <%= GetJavaScriptString(Localize("ProgressText.Text")) %>,
    UnsavedChangedWarning : <%= GetJavaScriptString(Localize("UnsavedChangedWarning.Text")) %>,
    CheckBoxCheckedText: <%= GetJavaScriptString(Localize("CheckBoxCheckedText.Text")) %>,
    CheckBoxUncheckedText: <%= GetJavaScriptString(Localize("CheckBoxUncheckedText.Text")) %>,
    DefaultEmailSettings: {
        SendNotification: <%= this.DefaultSendNofitication ? "true" : "false" %>,
        NotificationFromEmail: <%= GetJavaScriptString(this.DefaultNotificationFromEmailAddress) %>,
        NotificationToEmails: <%= GetJavaScriptString(this.DefaultNotificationToEmailAddresses) %>,
        SendThankYou: <%= this.DefaultSendThankYou ? "true" : "false" %>,
        ThankYouFromEmail: <%= GetJavaScriptString(this.DefaultThankYouFromEmailAddress) %>
    },
    DefaultCompletionMessage: <%= GetJavaScriptString(this.DefaultCompletionMessage) %>,
    EmailRegex: /<%=Engage.Utility.EmailRegEx %>/,
    EmailsRegex: /<%=Engage.Utility.EmailsRegEx %>/,
    ErrorMessages: {
        required: <%= GetJavaScriptString(Localize("Error: Required.Text")) %>,
        DefineAnswerType: { 
            min: <%= GetJavaScriptString(Localize("TypeRequired.Text")) %>
        },
        notificationEmail: {
            required: <%= GetJavaScriptString(Localize("Error: Required.Text")) %>,
            email: <%= GetJavaScriptString(Localize("Error: Email.Text")) %>
        },
        thankYouEmail: {
            required: <%= GetJavaScriptString(Localize("Error: Required.Text")) %>,
            email: <%= GetJavaScriptString(Localize("Error: Email.Text")) %>
        },
        notificationEmails: {
            required: <%= GetJavaScriptString(Localize("Error: Required.Text")) %>,
            emails: <%= GetJavaScriptString(Localize("Error: Emails.Text")) %>
        },
        completionMessage: {
            required: <%= GetJavaScriptString(Localize("Error: Required.Text")) %>
        },
        completionUrl: {
            required: <%= GetJavaScriptString(Localize("Error: Required.Text")) %>,
            url: <%= GetJavaScriptString(Localize("Error: Url.Text")) %>
        }
    }
};
</script>
