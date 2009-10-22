<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditSurvey.ascx.cs" Inherits="Engage.Dnn.Survey.EditSurvey" %>
<%@ Import Namespace="Engage.Survey.Util"%>
<div id="engage-evaluation">
    <fieldset class="ee-create-new">
        <legend class="Head"><%=Localize("CreateNewLabel.Text")%></legend>
        <div class="ee-title">
            <span class="ee-label eval-title"><%=Localize("EvalTitleLabel.Text")%></span>
            <span class="ee-input"><input class="NormalTextBox ee-required" id="EvalTitleInput" minlength="1" maxlength="256" name="required" /></span>
        </div>
        <div class="ee-description">
            <span class="ee-label eval-description"><%=Localize("EvalDescriptionLabel.Text")%></span>
            <span class="ee-input"><textarea id="EvalDescTextArea" class="NormalTextBox" maxlength="256" cols="40" rows="4"></textarea></span>
        </div>
        <ul class="ee-action-btns">
            <li class="primary-btn"><a href="#" title='<%=Localize("CreateNewEvalHyperLink.ToolTip") %>' id="EvalNew" class="create-new"><%=Localize("CreateNewEvalHyperLink.Text")%></a></li>
            <li class="primary-btn" style="display:none;"><a href="#" title='<%=Localize("EditEvalHyperLink.ToolTip") %>' id="EvalEdit"><%=Localize("EditEvalHyperLink.Text")%></a></li>
            <li class="primary-btn" style="display:none;"><a href="#" title='<%=Localize("UpdateEvalHyperLink.ToolTip") %>' id="EvalUpdate"><%=Localize("UpdateEvalHyperLink.Text")%></a></li>
            <li class="secondary-btn" style="display:none;"><a href="#" title='<%=Localize("CancelHyperLink.ToolTip") %>' id="EvalCancel"><%=Localize("CancelHyperLink.Text")%></a></li>
            <li class="secondary-btn"><a href="<%=DotNetNuke.Common.Globals.NavigateURL() %>" title='<%=Localize("BackHyperLink.ToolTip") %>' id="EvalBack"><%=Localize("BackHyperLink.Text")%></a></li>
        </ul>
    </fieldset>
    <fieldset class="ee-create-questions" id="CreateQuestions">
        <legend class="Head"><%=Localize("CreateNewQuestionsLabel.Text")%></legend>
        <div class="ee-question">
            <span class="ee-label"><%=Localize("TypeQuestionLabel.Text")%></span>
            <span class="ee-input"><textarea id="QuestionText" class="NormalTextBox ee-required" minlength="1" maxlength="256" cols="40" rows="4" name="required"></textarea></span>
        </div>
        <div class="ee-define-answer">
            <span class="ee-label"><%=Localize("DefineAnswerLabel.Text")%></span>
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
                    <img src='<%=ResolveUrl("images/short-answer-input.gif")%>' alt='<%=Localize("ShortAnswerPreviewImage.AltText") %>' title='<%=Localize("ShortAnswerPreviewImage.ToolTip") %>' />
                </div>
                <div id="LongTextAnswer" style="display:none;" class="ee-answer-inputs">
                    <p><%=Localize("LongAnswerPreview.Text") %></p>
                    <img src='<%=ResolveUrl("images/long-answer-input.gif")%>' alt='<%=Localize("LongAnswerPreviewImage.AltText") %>' title='<%=Localize("LongAnswerPreviewImage.ToolTip") %>' />
                </div>
                <div class="ee-answer-inputs" id="MultipleAnswer" style="display:none;">
                    <ul class="answer-inputs">
                        <li class="answer-input">
                            <span class="ai-label"><%=Localize("AnswerTitle.Text")%> <span class="answer-num">1</span></span>
                            <div class="ai-selected">
                                <span class="ai-input"><input class="NormalTextBox" type="text" maxlength="256" /></span>
                                <a href="#" title='<%=Localize("RemoveAnswerHyperLink.ToolTip")%>' class="ee-delete"><%=Localize("RemoveAnswerHyperLink.Text")%></a>
                            </div>
                        </li>
                        <li class="answer-input">                        
                            <span class="ai-label"><%=Localize("AnswerTitle.Text")%> <span class="answer-num">2</span></span>
                            <div class="ai-selected">
                                <span class="ai-input"><input class="NormalTextBox" type="text" maxlength="256" /></span>
                                <a href="#" title='<%=Localize("RemoveAnswerHyperLink.ToolTip")%>' class="ee-delete"><%=Localize("RemoveAnswerHyperLink.Text")%></a>
                            </div>
                        </li>
                        <li class="ee-undo" style="display:none;">
                            <%=Localize("UndoAnswerDelete.Text") %> <a href="#" title='<%=Localize("UndoAnswerDeleteButton.ToolTip") %>'><%=Localize("UndoAnswerDeleteButton.Text") %></a>
                        </li>
                    </ul>
                    <ul class="ee-action-btns">
                        <li class="primary-btn" style="display:none;"><a href="#" title='<%=Localize("AddNewAnswerHyperLink.ToolTip")%>' class="add-new" id="AddNewQuestion"><%=Localize("AddNewAnswerHyperLink.Text")%></a></li>
                    </ul>
                </div>
            </div>
        </div>
        <ul class="ee-action-btns">
            <li class="primary-btn disabled"><a href="#" title='<%=Localize("SaveQuestion.ToolTip")%>' class="save-create-new" id="SaveQuestion"><%=Localize("SaveQuestion.Text")%></a></li>
            <li class="secondary-btn" style="display:none;"><a href="#" id="CancelQuestion" title='<%=Localize("CancelQuestionHyperLink.ToolTip")%>'><%=Localize("CancelQuestionHyperLink.Text")%></a></li>
        </ul>
    </fieldset>
    <fieldset id="PreviewArea" class="ee-preview-area">
        <legend class="Head"><%=Localize("PreviewAreaLabel.Text")%></legend>
        <hr />
        <ul id="ee-previews">
            <li class="ee-preview">
                <ul class="ee-pr-action-links">
                    <li><a href="#" title='<%=Localize("EditQuestionHyperLink.ToolTip")%>' class="ee-edit"><%=Localize("EditQuestionHyperLink.Text")%></a></li>
                    <li><a href="#" title='<%=Localize("CopyQuestionHyperLink.ToolTip")%>' class="ee-copy"><%=Localize("CopyQuestionHyperLink.Text")%></a></li>
                    <li><a href="#" title='<%=Localize("DeleteQuestionHyperLink.ToolTip")%>' class="ee-delete"><%=Localize("DeleteQuestionHyperLink.Text")%></a></li>
                </ul>
                <span class="ee-label pv-question-label"><%=Localize("QuestionTitle.Text")%></span>
                <span class="pv-question"></span>
                <div>
                    <span class="ee-label pv-answer-label"><%=Localize("AnswerTitle.Text")%></span>
                    <span class="pv-answer"></span>
                </div>
            </li>
            <li class="ee-undo" style="display:none;">
                <%=Localize("UndoQuestionDelete.Text") %> <a href="#" title='<%=Localize("UndoQuestionDeleteButton.ToolTip") %>'><%=Localize("UndoQuestionDeleteButton.Text") %></a>
            </li>
        </ul>
    </fieldset>   
</div>

<% if (false) { %><script type="text/ecmascript" src="JavaScript/jquery-1.3.2.debug-vsdoc.js"></script><% } %>
<script type="text/javascript">
var CurrentContextInfo = {
    WebMethodUrl: '<%= ResolveUrl("ClientService.asmx") %>/',
    UserId: <%=UserId %>,
    Survey: <%=SerializedSurvey %>,
    ErrorMessage: '<%= Localize("AjaxError.Text") %>',
    SaveQuestionButtonText: '<%= Localize("SaveQuestion.Text") %>',
    SaveQuestionToolTip: '<%= Localize("SaveQuestion.ToolTip") %>',
    UpdateQuestionButtonText: '<%= Localize("UpdateQuestion.Text") %>',
    UpdateQuestionToolTip: '<%= Localize("UpdateQuestion.ToolTip") %>'
};
</script>