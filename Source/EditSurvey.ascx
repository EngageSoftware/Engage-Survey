<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditSurvey.ascx.cs" Inherits="Engage.Dnn.Survey.EditSurvey" %>
<%@ Import Namespace="Engage.Survey.Util"%>
<%@ Import Namespace="DotNetNuke.Services.Localization"%>
<div id="engage-evaluation">
    <fieldset class="ee-create-new">
        <legend class="Head"><%=Localization.GetString("CreateNewLabel.Text", LocalResourceFile)%></legend>
        <div class="ee-title">
            <span class="ee-label eval-title"><%=Localization.GetString("EvalTitleLabel.Text", LocalResourceFile)%></span>
            <span class="ee-input"><input class="NormalTextBox required" id="EvalTitleInput" minlength="1" maxlength="256" /></span>
        </div>
        <div class="ee-description">
            <span class="ee-label eval-description"><%=Localization.GetString("EvalDescriptionLabel.Text", LocalResourceFile)%></span>
            <span class="ee-input"><textarea id="EvalDescTextArea" class="NormalTextBox" maxlength="256"></textarea></span>
        </div>
        <ul class="ee-action-btns">
            <li class="primary-btn"><a href="#" title="Create New" id="EvalNew" class="create-new"><%=Localization.GetString("CreateNewEvalHyperLink.Text", LocalResourceFile)%></a></li>
            <li class="primary-btn" style="display:none;"><a href="#" title="Edit" id="EvalEdit"><%=Localization.GetString("EditEvalHyperLink.Text", LocalResourceFile)%></a></li>
            <li class="primary-btn" style="display:none;"><a href="#" title="Update" id="EvalUpdate"><%=Localization.GetString("UpdateEvalHyperLink.Text", LocalResourceFile)%></a></li>
            <li class="secondary-btn" style="display:none;"><a href="#" title="Cancel" id="EvalCancel"><%=Localization.GetString("CancelHyperLink.Text", LocalResourceFile)%></a></li>
        </ul>
    </fieldset>
    <fieldset class="ee-create-questions">
        <legend class="Head"><%=Localization.GetString("CreateNewQuestionsLabel.Text", LocalResourceFile)%></legend>
        <div class="ee-question">
            <span class="ee-label"><%=Localization.GetString("TypeQuestionLabel.Text", LocalResourceFile)%></span>
            <span class="ee-input"><textarea id="QuestionText" class="NormalTextBox required" minlength="1" maxlength="256"></textarea></span>
        </div>
        <div class="ee-define-answer">
            <span class="ee-label"><%=Localization.GetString("DefineAnswerLabel.Text", LocalResourceFile)%></span>
            <div class="define-answer">
                <span class="ee-input">
                    <select class="NormalTextBox answer-options" name="DefineAnswerType" id="DefineAnswerType">
                        <option value='<%=(int)ControlType.None %>'><%=Localization.GetString("SelectAnswerTypeOption.Text", LocalResourceFile)%></option>
                        <option value='<%=(int)ControlType.SmallTextInputField %>'><%=Localization.GetString("ShortAnswerOption.Text", LocalResourceFile)%></option>
                        <option value='<%=(int)ControlType.LargeTextInputField %>'><%=Localization.GetString("LongAnswerOption.Text", LocalResourceFile)%></option>
                        <option value='<%=(int)ControlType.DropDownChoices %>'><%=Localization.GetString("SingleAnwserDropdownListOption.Text", LocalResourceFile)%></option>
                        <option value='<%=(int)ControlType.VerticalOptionButtons %>'><%=Localization.GetString("SingleAnswerRadioButtonOption.Text", LocalResourceFile)%></option>
                        <option value='<%=(int)ControlType.Checkbox %>'><%=Localization.GetString("MultipleAnswerCheckboxesOption.Text", LocalResourceFile)%></option>
                    </select>
                </span>
                <div id="ShortTextAnswer" style="display:none;">todo: show the short text preview content here</div>
                <div id="LongTextAnswer" style="display:none;">todo: show the long text preview content here</div>
                <ul id="MultipleAnswer" class="answer-inputs" style="display:none;">
                    <li>
                        <span class="ai-label"><%=Localization.GetString("AnswerTitle.Text", LocalResourceFile)%> <span class="answer-num">1</span></span>
                        <div class="ai-selected">
                            <span class="ai-input"><input type="text" /></span>
                            <a href="#" title="Remove this answer, are you sure?" class="ee-delete"><%=Localization.GetString("RemoveAnswerHyperLink.Text", LocalResourceFile)%></a>
                        </div>
                    </li>
                    <li>                        
                        <span class="ai-label"><%=Localization.GetString("AnswerTitle.Text", LocalResourceFile)%> <span class="answer-num">2</span></span>
                        <div class="ai-selected">
                            <span class="ai-input"><input type="text"/></span>
                            <a href="#" title="Remove this answer, are you sure?" class="ee-delete"><%=Localization.GetString("RemoveAnswerHyperLink.Text", LocalResourceFile)%></a>
                        </div>
                    </li>
                </ul>
                <span class="primary-btn" style="display:none;"><a href="#" title="Add New" class="add-new" id="AddNewQuestion"><%=Localization.GetString("AddNewAnswerHyperLink.Text", LocalResourceFile)%></a></span>
            </div>
        </div>
        <ul class="ee-action-btns">
            <li class="primary-btn disabled"><a href="#" title="Save and Create New" class="save-create-new" id="SaveQuestion"><%=Localization.GetString("SaveAndCreateNewQuestionHyperLink.Text", LocalResourceFile)%></a></li>
            <li class="secondary-btn"><a href="#" title="Back" class="back"><%=Localization.GetString("BackHyperLink.Text", LocalResourceFile)%></a></li>
        </ul>
    </fieldset>
    <fieldset id="PreviewArea" class="ee-preview-area">
        <legend class="Head"><%=Localization.GetString("PreviewAreaLabel.Text", LocalResourceFile)%></legend>
        <ul id="ee-previews">
            <li class="ee-preview">
                <ul class="ee-pr-action-links">
                    <li><a href="#" title="Edit this question" class="ee-edit"><%=Localization.GetString("EditQuestionHyperLink.Text", LocalResourceFile)%></a></li>
                    <li><a href="#" title="Copy this question and create new" class="ee-save"><%=Localization.GetString("CopyQuestionHyperLink.Text", LocalResourceFile)%></a></li>
                    <li><a href="#" title="Delete this question, are you sure?" class="ee-delete"><%=Localization.GetString("DeleteAnswerHyperLink.Text", LocalResourceFile)%></a></li>
                </ul>
                <span class="ee-label pv-question-label"><%=Localization.GetString("QuestionTitle.Text", LocalResourceFile)%></span>
                <span class="pv-question"></span>
                <span class="ee-label pv-answer-label"><%=Localization.GetString("AnswerTitle.Text", LocalResourceFile)%></span>
                <span class="pv-answer"></span>
            </li>
        </ul>
    </fieldset>   
</div>

<% if (false) { %><script type="text/ecmascript" src="JavaScript/jquery-1.3.2.debug-vsdoc.js"></script><% } %>
<script type="text/javascript">
var CurrentContextInfo = {
    WebMethodUrl: '<%= ResolveUrl("~/DesktopModules/EngageSurvey/Services.asmx") %>/',
    UserId: <%=UserId %>,
    Survey: <%=SerializedSurvey %>
};
</script>