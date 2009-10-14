<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditSurvey.ascx.cs" Inherits="Engage.Dnn.Survey.EditSurvey" %>
<%@ Import Namespace="DotNetNuke.Services.Localization"%>
<div id="engage-evaluation">
    <fieldset class="ee-create-new">
        <legend class="Head"><%=Localization.GetString("CreateNewLabel.Text", LocalResourceFile)%></legend>
        <div class="ee-title">
            <span class="ee-label eval-title"><%=Localization.GetString("EvalTitleLabel.Text", LocalResourceFile)%></span>
            <span class="ee-input"><input class="required" id="EvalTitleInput" minlength="1" maxlength="256" /></span>
        </div>
        <div class="ee-description">
            <span class="ee-label eval-description"><%=Localization.GetString("EvalDescriptionLabel.Text", LocalResourceFile)%></span>
            <span class="ee-input"><textarea id="EvalDescTextArea" maxlength="256"></textarea></span>
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
            <span class="ee-input"><textarea id="QuestionText" class="required" minlength="1" maxlength="256"></textarea></span>
        </div>
        <div class="ee-define-answer">
            <span class="ee-label"><%=Localization.GetString("DefineAnswerLabel.Text", LocalResourceFile)%></span>
            <div class="define-answer">
                <span class="ee-input">
                    <select class="NormalTextBox answer-options" name="DefineAnswerType" id="DefineAnswerType">
                        <option value="select-type"><%=Localization.GetString("SelectAnswerTypeOption.Text", LocalResourceFile)%></option>
                        <option value="short-input"><%=Localization.GetString("ShortAnswerOption.Text", LocalResourceFile)%></option>
                        <option value="long-input"><%=Localization.GetString("LongAnswerOption.Text", LocalResourceFile)%></option>
                        <option value="single-dropdown"><%=Localization.GetString("SingleAnwserDropdownListOption.Text", LocalResourceFile)%></option>
                        <option value="single-radio"><%=Localization.GetString("SingleAnswerRadioButtonOption.Text", LocalResourceFile)%></option>
                        <option value="multiple-checkbox"><%=Localization.GetString("MultipleAnswerCheckboxesOption.Text", LocalResourceFile)%></option>
                    </select>
                </span>
                <div id="ShortTextAnswer" style="display:none;">todo: show the short text preview content here</div>
                <div id="LongTextAnswer" style="display:none;">todo: show the long text preview content here</div>
                <ul id="MultipleAnswer" class="answer-inputs" style="display:none;">
                    <li>
                        <span class="ai-label"><%=Localization.GetString("AnswerNumberTitle.Text", LocalResourceFile)%> <span class="answer-num">1</span></span>
                        <div class="ai-selected">
                            <span class="ai-input"><input type="text" /></span>
                            <a href="" title="Remove this answer, are you sure?" class="ee-delete"><%=Localization.GetString("RemoveAnswerHyperLink.Text", LocalResourceFile)%></a>
                        </div>
                    </li>
                    <li>                        
                        <span class="ai-label"><%=Localization.GetString("AnswerNumberTitle.Text", LocalResourceFile)%> <span class="answer-num">2</span></span>
                        <div class="ai-selected">
                            <span class="ai-input"><input type="text"/></span>
                            <a href="" title="Remove this answer, are you sure?" class="ee-delete"><%=Localization.GetString("RemoveAnswerHyperLink.Text", LocalResourceFile)%></a>
                        </div>
                    </li>
                </ul>
                <span class="primary-btn" style="display:none;"><a href="" title="Add New" class="add-new"><%=Localization.GetString("AddNewAnswerHyperLink.Text", LocalResourceFile)%></a></span>
            </div>
        </div>
        <ul class="ee-action-btns">
            <li class="primary-btn disabled"><a href="" title="Save and Create New" class="save-create-new" id="SaveQuestion"><%=Localization.GetString("SaveAndCreateNewQuestionHyperLink.Text", LocalResourceFile)%></a></li>
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

<% if (false) { %><script type="text/ecmascript" src="JavaScript/jquery-1.3.2.debug-vsdoc.js"></script><% } %>
<script type="text/ecmascript">
var CurrentContextInfo = {
    WebMethodUrl: '/DesktopModules/EngageSurvey/Services.asmx/UpdateSurvey',
    UserId: <%=UserId %>
};

jQuery(function ($) {
    $("#ee-previews, .answer-inputs").sortable({
        placeholder: 'ui-state-highlight'
    });
    $("#ee-previews, .answer-inputs").disableSelection();
    
    // Add selection style back to the inputs, since our CSS is removing or hiding the native style
    $("#engage-evaluation :input").focus(function () {
        $(this).addClass("focus");
    }).blur(function () {
        $(this).removeClass("focus");
    });

    $('#EvalNew').click(function (event) {
        event.preventDefault();
        if($('#Form').validate().form()){
            updateSurvey(function () {
                $('.ee-create-questions').show(); 
            });
        }
    });
    
    $('#EvalUpdate').click(function (event) {
        event.preventDefault();
        
        if($('#Form').validate().form()) {
            updateSurvey(function () {
                hideEditModeButtons();
            });
        }
    });
    
    function updateSurvey(callback) {
        jQuery.ajax({
            type: "POST",
            url: CurrentContextInfo.WebMethodUrl,
            data: JSON.stringify(getSurveyParameters()),
            contentType: "application/json; charset=utf-8",
            dataFilter: function(data) {
                var msg = eval('(' + data + ')');
                if (msg.hasOwnProperty('d'))
                    return msg.d;
                else
                    return msg;
            },
            success: function(msg) { 
                $('.ee-create-new').data('surveyId', msg); 
                makeSurveyReadOnly();
                if (typeof(callback) === 'function') {
                    callback();
                }
            },
            error: function(/*XMLHttpRequest, textStatus, errorThrown*/) { 
                alert('There was an error submitting the survey, please try again.'); 
            }
        });
    }
    
    function getSurveyParameters () {
        return {
            survey : {
                SurveyId: $('.ee-create-new').data('surveyId') || -1,
                Text: $('#EvalTitleInput').val(),
                RevisingUser: CurrentContextInfo.UserId,
                Sections: [{
                    Text: $('#EvalDescTextArea').val()
                }]
            }
        };
    }
    
    $('#EvalEdit').click(function (event) {
        event.preventDefault();
        $('#EvalTitleInput').convertTo('input').removeClass('ee-input-pre');
        $('#EvalDescTextArea').convertTo('textarea').removeClass('ee-input-pre');
        $('.ee-description').show();
        $('#EvalEdit').parent().hide();
        $('#EvalCancel').parent().show();
        $('#EvalUpdate').parent().show();
    });
    
    $('#EvalCancel').click(function (event) {
        event.preventDefault();
        makeSurveyReadOnly();
        hideEditModeButtons();
    });
        
    function makeSurveyReadOnly () {
        if ($('#EvalDescTextArea').val() != '') {
            $('#EvalDescTextArea').convertTo('span').addClass('ee-input-pre');
            $('.ee-description').show();
        }
        else {
            $('.ee-description').hide();
        }
        $('#EvalTitleInput').convertTo('span').addClass('ee-input-pre');
        $('#EvalNew').parent().hide();
        $('#EvalEdit').parent().show();
    }

    function hideEditModeButtons () {
        $('#EvalUpdate').parent().hide();
        $('#EvalCancel').parent().hide();
    }
    
    $(".add-new").click(function (event) {
        event.preventDefault();
        
        var $answerElement = $(".answer-inputs li:last").clone(true).appendTo('.answer-inputs');
        
        // increment answer number
        var $answerNumberElement = $answerElement.find('.answer-num');
        var answerNumber = parseInt($answerNumberElement.text(), 10);
        $answerNumberElement.text(answerNumber + 1);
        
        // clear out cloned textbox
        $answerElement.find('input').val('');
    });
    
    $(".answer-inputs .ee-delete").click(function (event) {
        event.preventDefault();
        
        var $parentAnswerElement = $(this).parents('li');
        $parentAnswerElement.remove();
        
        $(".answer-inputs li").each(function (i, elem) {
            $(elem).find('.answer-num').text(i + 1);
        });
    });

    $('#DefineAnswerType').change(function (event) {
        var questionType = $(this).val();
        if (questionType == "short-input") {
            $('#ShortTextAnswer').show();
            $('#LongTextAnswer').hide();
            $('#MultipleAnswer').hide();
            $('#SaveQuestion').parent().removeClass('disabled');
        }
        else if(questionType == "long-input") {
            $('#ShortTextAnswer').hide();
            $('#LongTextAnswer').show();
            $('#MultipleAnswer').hide();
            $('#SaveQuestion').parent().removeClass('disabled');
        }
        else if(questionType == "select-type") { //default
            $('#ShortTextAnswer').hide();
            $('#LongTextAnswer').hide();
            $('#MultipleAnswer').hide();
            $('#SaveQuestion').parent().addClass('disabled');
        }
        else { //multiple answer
            $('#MultipleAnswer').show();
            $('#ShortTextAnswer').hide();
            $('#LongTextAnswer').hide();
            $('#SaveQuestion').parent().addClass('disabled');
        }
    });

});
</script>