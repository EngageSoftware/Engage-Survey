<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="EditSurvey.ascx.cs" Inherits="Engage.Dnn.Survey.EditSurvey" %>
<%@ Import Namespace="DotNetNuke.Services.Localization"%>
<div id="engage-evaluation">
    <fieldset class="ee-create-new">
        <legend class="Head"><%=Localization.GetString("CreateNewLabel.Text", LocalResourceFile)%></legend>
        <div class="ee-title">
            <span class="ee-label eval-title"><%=Localization.GetString("EvalTitleLabel.Text", LocalResourceFile)%></span>
            <span class="ee-input"><input class="NormalTextBox" /></span>
        </div>
        <div class="ee-description">
            <span class="ee-label eval-description"><%=Localization.GetString("EvalDescriptionLabel.Text", LocalResourceFile)%></span>
            <span class="ee-input"><textarea class="NormalTextBox"></textarea></span>
        </div>
        <ul class="ee-action-btns">
            <li class="primary-btn"><a href="" title="Create New" class="create-new"><%=Localization.GetString("CreateNewEvalHyperLink.Text", LocalResourceFile)%></a></li>
            <li class="secondary-btn"><a href="" title="Back" class="back"><%=Localization.GetString("BackHyperLink.Text", LocalResourceFile)%></a></li>
        </ul>
    </fieldset>
    <fieldset class="ee-create-questions">
        <legend class="Head"><%=Localization.GetString("CreateNewQuestionsLabel.Text", LocalResourceFile)%></legend>
        <div class="ee-question">
                <a href="" class="ee-edit add-section-title" id="AddSectionTitleLabel" runat="server"><%=Localization.GetString("AddSectionTitleLabel.Text", LocalResourceFile)%></a>
                <div id="section-title-input" style="display:none;">
                    <span class="ee-label"><%=Localization.GetString("TypeSectionTitleLabel.Text", LocalResourceFile)%></span>
                    <span class="ee-input"><input id="SectionTitleTextBox" class="NormalTextBox" /></span><a href="" title="Remove section header" class="ee-delete add-section-title">Delete</a>
                </div>
                <span class="ee-label"><%=Localization.GetString("TypeQuestionLabel.Text", LocalResourceFile)%></span>
                <span class="ee-input"><textarea class="NormalTextBox"></textarea></span>
        </div>
        <div class="ee-define-answer">
            <span class="ee-label"><%=Localization.GetString("DefineAnswerLabel.Text", LocalResourceFile)%></span>
            <span class="define-answer">
                <span class="ee-input">
                    <select class="NormalTextBox answer-options" name="DefineAnswerType">
                        <option value="select-type"><%=Localization.GetString("SelectAnswerTypeOption.Text", LocalResourceFile)%></option>
                        <option value="short-input"><%=Localization.GetString("ShortAnswerOption.Text", LocalResourceFile)%></option>
                        <option value="long-input"><%=Localization.GetString("LongAnswerOption.Text", LocalResourceFile)%></option>
                        <option value="single-dropdown"><%=Localization.GetString("SingleAnwserDropdownListOption.Text", LocalResourceFile)%></option>
                        <option value="single-radio"><%=Localization.GetString("SingleAnswerRadioButtonOption.Text", LocalResourceFile)%></option>
                        <option value="multiple-checkbox"><%=Localization.GetString("MultipleAnswerCheckboxesOption.Text", LocalResourceFile)%></option>
                    </select>
                </span>                
                <div class="answer-inputs">
                    <span class="ai-label"><%=Localization.GetString("AnswerNumberTitle.Text", LocalResourceFile)%></span>
                    <div class="ai-selected">
                        <span class="ai-input"><input id="AnswerInputTextBox" class="NormalTextBox" /></span>
                        <a href="" title="Save this answer" class="ee-save"><%=Localization.GetString("SaveAnswerHyperLink.Text", LocalResourceFile)%></a>
                        <a href="" title="Delete this answer, are you sure?" class="ee-delete"><%=Localization.GetString("DeleteAnswerHyperLink.Text", LocalResourceFile)%></a>
                    </div>
                    <span class="ai-label"><%=Localization.GetString("AnswerNumberTitle.Text", LocalResourceFile)%></span>
                    <div class="ai-selected">
                        <span class="ai-input"><input id="Text1" class="NormalTextBox" /></span>
                        <a href="" title="Save this answer" class="ee-save"><%=Localization.GetString("SaveAnswerHyperLink.Text", LocalResourceFile)%></a>
                        <a href="" title="Delete this answer, are you sure?" class="ee-delete"><%=Localization.GetString("DeleteAnswerHyperLink.Text", LocalResourceFile)%></a>
                    </div>
                    <ul class="ee-action-btns">
                        <li class="primary-btn"><a href="" title="Add New" class="add-new"><%=Localization.GetString("AddNewAnswerHyperLink.Text", LocalResourceFile)%></a</li>
                        <li class="primary-btn"><a href="" title="Save all answers" class="save-all"><%=Localization.GetString("SaveAllAnswersHyperLink.Text", LocalResourceFile)%></a></li>
                        <li class="secondary-btn"><a href="" title="Back" class="back"><%=Localization.GetString("BackHyperLink.Text", LocalResourceFile)%></a></li>
                    </ul>
                </div>
            </span>
        </div>
        <ul class="ee-action-btns">
            <li class="primary-btn disabled"><a href="" title="Save and Create New" class="save-create-new"><%=Localization.GetString("SaveAndCreateNewQuestionHyperLink.Text", LocalResourceFile)%></a></li>
            <li class="secondary-btn"><a href="" title="Back" class="back"><%=Localization.GetString("BackHyperLink.Text", LocalResourceFile)%></a></li>
        </ul>
    </fieldset>
    <fieldset class="ee-preview-area">
        <legend class="Head"><%=Localization.GetString("PreviewAreaLabel.Text", LocalResourceFile)%></legend>
        <ul id="ee-previews">
            <li class="ee-preview">
                <ul class="ee-pr-action-links">
                    <li><a href="" title="Edit this question" class="ee-edit"><%=Localization.GetString("EditQuestionHyperLink.Text", LocalResourceFile)%></a></li>
                    <li><a href="" title="Copy this question and create new" class="ee-save"><%=Localization.GetString("CopyQuestionHyperLink.Text", LocalResourceFile)%></a></li>
                    <li><a href="" title="Delete this question, are you sure?" class="ee-delete"><%=Localization.GetString("DeleteAnswerHyperLink.Text", LocalResourceFile)%></a></li>
                </ul>
                <span class="ee-label pv-question-label"><%=Localization.GetString("QuestionNumberTitle.Text", LocalResourceFile)%></span>
                <span class="pv-question">Who invented the toilet?</span>
                <span class="pv-answer">
                    <span class="ee-input">
                        <select class="NormalTextBox answer-options" name="DefineAnswerType">
                            <option value="select-type"><%=Localization.GetString("SelectAnswerTypeOption.Text", LocalResourceFile)%></option>
                            <option value="short-input"><%=Localization.GetString("ShortAnswerOption.Text", LocalResourceFile)%></option>
                            <option value="long-input"><%=Localization.GetString("LongAnswerOption.Text", LocalResourceFile)%></option>
                            <option value="single-dropdown"><%=Localization.GetString("SingleAnwserDropdownListOption.Text", LocalResourceFile)%></option>
                            <option value="single-radio"><%=Localization.GetString("SingleAnswerRadioButtonOption.Text", LocalResourceFile)%></option>
                            <option value="multiple-checkbox"><%=Localization.GetString("MultipleAnswerCheckboxesOption.Text", LocalResourceFile)%></option>
                        </select>
                    </span>                
                </span>
            </li>
            <li class="ee-preview">
                <ul class="ee-pr-action-links">
                    <li><a href="" title="Edit this question" class="ee-edit"><%=Localization.GetString("EditQuestionHyperLink.Text", LocalResourceFile)%></a></li>
                    <li><a href="" title="Copy this question and create new" class="ee-save"><%=Localization.GetString("CopyQuestionHyperLink.Text", LocalResourceFile)%></a></li>
                    <li><a href="" title="Delete this question, are you sure?" class="ee-delete"><%=Localization.GetString("DeleteAnswerHyperLink.Text", LocalResourceFile)%></a></li>
                </ul>
                <span class="ee-label pv-question-label"><%=Localization.GetString("QuestionNumberTitle.Text", LocalResourceFile)%></span>
                <span class="pv-question">What's your invention of the year?</span>
                <span class="pv-answer">
                    <span class="ee-input">
                        <select class="NormalTextBox answer-options" name="DefineAnswerType">
                            <option value="select-type"><%=Localization.GetString("SelectAnswerTypeOption.Text", LocalResourceFile)%></option>
                            <option value="short-input"><%=Localization.GetString("ShortAnswerOption.Text", LocalResourceFile)%></option>
                            <option value="long-input"><%=Localization.GetString("LongAnswerOption.Text", LocalResourceFile)%></option>
                            <option value="single-dropdown"><%=Localization.GetString("SingleAnwserDropdownListOption.Text", LocalResourceFile)%></option>
                            <option value="single-radio"><%=Localization.GetString("SingleAnswerRadioButtonOption.Text", LocalResourceFile)%></option>
                            <option value="multiple-checkbox"><%=Localization.GetString("MultipleAnswerCheckboxesOption.Text", LocalResourceFile)%></option>
                        </select>
                    </span>                
                </span>
            </li>
            <li class="ee-preview">
                <ul class="ee-pr-action-links">
                    <li><a href="" title="Edit this question" class="ee-edit"><%=Localization.GetString("EditQuestionHyperLink.Text", LocalResourceFile)%></a></li>
                    <li><a href="" title="Copy this question and create new" class="ee-save"><%=Localization.GetString("CopyQuestionHyperLink.Text", LocalResourceFile)%></a></li>
                    <li><a href="" title="Delete this question, are you sure?" class="ee-delete"><%=Localization.GetString("DeleteAnswerHyperLink.Text", LocalResourceFile)%></a></li>
                </ul>
                <span class="ee-label pv-question-label"><%=Localization.GetString("QuestionNumberTitle.Text", LocalResourceFile)%></span>
                <span class="pv-question">In what year Abraham Lincoln got assassinated?</span>
                <span class="pv-answer">
                    <span class="ee-input">
                        <select class="NormalTextBox answer-options" name="DefineAnswerType">
                            <option value="select-type"><%=Localization.GetString("SelectAnswerTypeOption.Text", LocalResourceFile)%></option>
                            <option value="short-input"><%=Localization.GetString("ShortAnswerOption.Text", LocalResourceFile)%></option>
                            <option value="long-input"><%=Localization.GetString("LongAnswerOption.Text", LocalResourceFile)%></option>
                            <option value="single-dropdown"><%=Localization.GetString("SingleAnwserDropdownListOption.Text", LocalResourceFile)%></option>
                            <option value="single-radio"><%=Localization.GetString("SingleAnswerRadioButtonOption.Text", LocalResourceFile)%></option>
                            <option value="multiple-checkbox"><%=Localization.GetString("MultipleAnswerCheckboxesOption.Text", LocalResourceFile)%></option>
                        </select>
                    </span>                
                </span>
            </li>
        </ul>
    </fieldset>   
</div>
<script language="javascript" type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3.2/jquery.min.js"></script>
<script language="javascript" type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.7.2/jquery-ui.min.js"></script>
<script type="text/javascript" language="javascript">
    jQuery(document).ready(function() {
        $(".add-section-title").click(function(event) {
            $("#section-title-input").toggle("fast");
            event.preventDefault();
        });
    });
</script>
<script type="text/javascript">
    $(function() {
        $("#ee-previews").sortable({
            placeholder: 'ui-state-highlight'
        });
        $("#ee-previews").disableSelection();
    });
</script>