/// <reference path="jquery-1.3.2.debug-vsdoc.js" />
/// <reference path="json2.js" />
/// <reference path="jquery-ui-1.8.14.js" />
/// <reference path="jquery.validate-1.8.1.js" />
/// <reference path="Array.prototype.indexOf.js" />
/*globals jQuery, Sys, currentContextInfo, alert */
(function ($, window) {
    'use strict';

    $(function () {
        var $find = window.Sys.Application.findComponent,
            alert = window.alert,
            currentContextInfo = window.currentContextInfo,
            animationSpeed = 'normal',
            pendingQuestionDeleteCallbacks = [],
            validator,
            $form = $('#Form'),
            $moduleWrap = $('#engage-evaluation'),
            $surveyTitleTextBox = $('#EvalTitleInput'),
            $surveyDescriptionTextArea = $('#EvalDescTextArea'),
            startDatePicker = $find($('.ee-start-date .RadPicker input').attr('id')),
            $preStartTextArea = $('#EvalPreStartTextArea'),
            endDatePicker = $find($('.ee-end-date .RadPicker input').attr('id')),
            $postEndTextArea = $('#EvalPostEndTextArea'),
            $sendNotificationCheckBox = $('#EvalSendNotification'),
            $notificationFromEmailTextBox = $('#EvalNotificationFromEmail'),
            $notificationToEmailsTextBox = $('#EvalNotificationToEmails'),
            $sendThankYouCheckBox = $('#EvalSendThankYou'),
            $thankYourFromEmailTextBox = $('#EvalThankYouFromEmail'),
            $completionActionDropDown = $('#EvalCompletionAction'),
            $completionMessageTextArea = $('#EvalCompletionMessage'),
            $completionUrlTextBox = $('#EvalCompletionUrl'),
            $newSurveyButton = $('#EvalNew'),
            $editSurveyButton = $('#EvalEdit'),
            $updateSurveyButton = $('#EvalUpdate'),
            $cancelSurveyEditButton = $('#EvalCancel'),
            $deleteSurveyButton = $('#EvalDelete'),
            $createQuestionArea = $('#CreateQuestions'),
            $questionTextArea = $('#QuestionText'),
            $questionRequiredCheckBox = $('#QuestionRequiredCheckBox'),
            $answerTypeDropDown = $('#DefineAnswerType'),
            $shortTextAnswerPreview = $('#ShortTextAnswer'),
            $longTextAnswerPreview = $('#LongTextAnswer'),
            $multipleAnswerSection = $('#MultipleAnswer'),
            $addNewQuestionButton = $('#AddNewQuestion'),
            $saveQuestionButton = $('#SaveQuestion'),
            $cancelQuestionEditButton = $('#CancelQuestion'),
            $previewArea = $('#PreviewArea'),
            $questionPreviewList = $('#ee-previews'),
            completionAction = { message: 1, url: 2 },
            answerType = {
                none: 0,
                textArea: 1,
                textBox: 2,
                verticalRadioButtons: 3,
                dropDown: 5,
                checkBox: 6
            };

        $.validator.addMethod("email", function (value, element) {
            return this.optional(element) || currentContextInfo.EmailRegex.test(value);
        });
        $.validator.addMethod("emails", function (value, element) {
            return this.optional(element) || currentContextInfo.EmailsRegex.test(value);
        });
        $.validator.setDefaults({
            rules: {
                DefineAnswerType: { min: 1 },
                required: { required: true },
                notificationEmail: {
                    required: {
                        depends: function () {
                            return $sendNotificationCheckBox.is(':checked');
                        }
                    },
                    email: true
                },
                notificationEmails: { 
                    required: {
                        depends: function () {
                            return $sendNotificationCheckBox.is(':checked');
                        }
                    }, 
                    emails: true
                },
                thankYouEmail: { 
                    required: {
                        depends: function () {
                            return $sendThankYouCheckBox.is(':checked');
                        }
                    }, 
                    email: true
                },
                completionMessage: {
                    required: {
                        depends: function () {
                            return $completionActionDropDown.val() !== completionAction.url.toString();
                        }
                    }
                },
                completionUrl: {
                    required: {
                        depends: function () {
                            return $completionActionDropDown.val() === completionAction.url.toString();
                        }
                    },
                    url: true
                }
            },
            messages: currentContextInfo.ErrorMessages,
            onsubmit: false
        });

        validator = $form.validate({ ignore: '#DefineAnswerType' });

        window.onbeforeunload = function () { 
            if ($cancelQuestionEditButton.is(':visible') || $cancelSurveyEditButton.is(':visible')) {
                return currentContextInfo.UnsavedChangedWarning;
            }

            return undefined;
        };
        
        $(window).unload(function () {
            // when the user leaves the page, finish any pending question deletions
            $.each(pendingQuestionDeleteCallbacks, function (i, deleteQuestionFunction) {
                if ($.isFunction(deleteQuestionFunction)) {
                    deleteQuestionFunction();
                }
            });
        });

        function callWebMethod(methodName, parameters, callback) {
            jQuery.ajax({
                type: "POST",
                url: currentContextInfo.WebMethodUrl + methodName + "?portalId=" + currentContextInfo.PortalId,
                data: JSON.stringify(parameters),
                contentType: "application/json",
                success: function (msg) { 
                    if ($.isFunction(callback)) {
                        if (typeof msg === 'string') {
                            msg = JSON.parse(msg);
                        }
                        
                        callback(msg.hasOwnProperty('d') ? msg.d : msg);
                    }
                },
                error: function () { 
                    // TODO provide a more friendly error message
                    alert(currentContextInfo.ErrorMessage); 
                }
            });
        }
        
        function getHtmlForCompletionMessage(value) {
            return $('<div/>').append($('<p/>').text(value)).html().replace(/\n/g, '<br />');
        }

        function getSurveyParameters() {
            return {
                survey : {
                    SurveyId: $('.ee-create-new').data('surveyId') || -1,
                    Text: $surveyTitleTextBox.val(),
                    StartDate: startDatePicker.get_selectedDate(),
                    PreStartMessage: $preStartTextArea.val(),
                    EndDate: endDatePicker.get_selectedDate(),
                    PostEndMessage: $postEndTextArea.val(),
                    SendNotification: $sendNotificationCheckBox.is(':checked'),
                    NotificationFromEmailAddress: $notificationFromEmailTextBox.val(),
                    NotificationToEmailAddresses: $notificationToEmailsTextBox.val(),
                    SendThankYou: $sendThankYouCheckBox.is(':checked'),
                    ThankYouFromEmailAddress: $thankYourFromEmailTextBox.val(),
                    FinalMessageOption: $completionActionDropDown.val(),
                    FinalMessage: getHtmlForCompletionMessage($completionMessageTextArea.val()),
                    FinalUrl: $completionUrlTextBox.val(),
                    PortalId: currentContextInfo.PortalId,
                    ModuleId: currentContextInfo.ModuleId,
                    RevisingUser: currentContextInfo.UserId,
                    Sections: [{
                        Text: $surveyDescriptionTextArea.val()
                    }]
                }
            };
        }

        function updateSurvey(callback) {
            callWebMethod('UpdateSurvey', getSurveyParameters(), function (surveyId) {
                $('.ee-create-new').data('surveyId', surveyId); 
                if ($.isFunction(callback)) {
                    callback(surveyId);
                }
            });
        }

        function getQuestionParameters() {
            return {
                surveyId: $('.ee-create-new').data('surveyId') || -1,
                question: {
                    QuestionId: $createQuestionArea.data('questionId') || -1,
                    Text: $questionTextArea.val(),
                    IsRequired: $questionRequiredCheckBox.is(':checked'),
                    RelativeOrder: $createQuestionArea.data('relativeOrder') || $('.ee-preview').length + 1,
                    ControlType: $answerTypeDropDown.val(),
                    RevisingUser: currentContextInfo.UserId,
                    Answers: !$multipleAnswerSection.is(':visible') ? 
                                [] :
                                $.map(
                                    $multipleAnswerSection.find('.answer-inputs li.answer-input:visible'), 
                                    function (elem) {
                                        var $elem = $(elem);
                                        return {
                                            AnswerId: $elem.data('answerId') || -1,
                                            Text: $elem.find(':input').val()
                                        };
                                    })
                }
            };
        }
        
        function deleteWithUndo($element, withTimer, afterFadeOut, deleteCallback, afterUndo) {
            $element.fadeOut('slow', function () {
                var deleteTimeoutHandle, 
                    $undoElement = $element.siblings('.ee-undo').eq(0).clone().show().removeClass('template'),
                    undoHtml = $undoElement.html(),
                    undoTimeLimit = 11, // it'll take a second to actually show the timer, so it shows up to the user as 10
                    startTime = new Date(),
                    deleteTimeoutCallback;
                    
                if ($.isFunction(afterFadeOut)) {
                    afterFadeOut();
                }
                
                $undoElement.html(undoHtml.replace('{0}', '<span class="undo-limit"></span>'));
                
                $element.addClass('deleted');
                $element.before($undoElement);
                
                $undoElement.hide().fadeIn(animationSpeed);
                
                // set timer to delete question
			    if (withTimer) {
			        deleteTimeoutCallback = function () {
	                    $undoElement.remove();
	                    
	                    // remove this callback from the list
	                    pendingQuestionDeleteCallbacks.splice(pendingQuestionDeleteCallbacks.indexOf(deleteTimeoutCallback), 1);

	                    if ($.isFunction(deleteCallback)) {
	                        deleteCallback();
	                    }
	                };
			        
			        pendingQuestionDeleteCallbacks.push(deleteTimeoutCallback);
	                deleteTimeoutHandle = setTimeout(deleteTimeoutCallback, undoTimeLimit * 1000);

	                // update the time remaining until deleted
	                (function updateUndoTimer() {
	                    var currentTime = new Date(),
	                        msElapsed = currentTime.getTime() - startTime.getTime(),
	                        msLeft = (undoTimeLimit * 1000) - msElapsed,
	                        secondsLeft = parseInt(msLeft / 1000, 10);
	                    $undoElement.find('span.undo-limit').text(secondsLeft.toString(10));

	                    if (secondsLeft > 0) {
	                        setTimeout(updateUndoTimer, 300);
	                    }
	                }());
                }

                // undo button
                $undoElement.find('a').click(function (event) {
                    event.preventDefault();
                    
                    clearTimeout(deleteTimeoutHandle);
                    
                    // remove this callback from the list
                    pendingQuestionDeleteCallbacks.splice(pendingQuestionDeleteCallbacks.indexOf(deleteTimeoutCallback), 1);
                    
                    $undoElement.fadeOut(animationSpeed, function () {
                        $element.removeClass('deleted').fadeIn(animationSpeed);
                        $undoElement.remove();
                    });
                    
                    if ($.isFunction(afterUndo)) {
                        afterUndo();
                    }
                });
            });
        }
                   
        function hideEditModeButtons(callback) {
            $updateSurveyButton.parent().fadeOut(animationSpeed);
            $cancelSurveyEditButton.parent().fadeOut(animationSpeed, callback);
        }
        
        function storePreviousValue($input, value) {
            $input.parent().data('previousValue', value === undefined ? $input.text() : value);
        }
        
        function resetToPreviousValue($input) {
            $input.val($input.parent().data('previousValue'));
        }
            
        function resetCheckBoxToPreviousValue($input) {
            $input.attr('checked', $input.parent().data('previousValue'));
        }
            
        function resetDatePickerToPreviousValue(datePicker) {
            datePicker.set_selectedDate($(datePicker.get_element()).closest('.RadPicker').parent().data('previousValue'));
        }
            
        function makeElementReadonly($element, value) {
            var $readonlyElement = $('<span />');
            $element.slideUp(animationSpeed, function () {
                var $this = $(this),
                    maxlength = $this.attr('maxlength'),
                    minlength = $this.attr('minlength');

                // if maxlength is not set (as on NotificationToEmails) then the browser default is returned 
                // (http://herr-schuessler.de/blog/selecting-input-fields-with-maxlength-via-jquery/)
                if (maxlength < 0 || maxlength > 500000) {
                    maxlength = '';
                }
                
                // if minlength is null, then jQuery sees the data call as an accessor instead of a setter, so we change it to '' if it's null
                if (!minlength) {
                    minlength = '';
                }

                $readonlyElement
                    .attr({
                        id: $this.attr('id'),
                        className: $this.attr('class'),
                        name: $this.attr('name')
                    }).data('minlength',  minlength) 
                    .data('maxlength', maxlength || '')
                    .data('rows', $this.attr('rows') || '')
                    .data('cols', $this.attr('cols') || '')
                    .data('checked', $this.attr('checked') !== undefined ? $this.is(':checked') : '')
                    .addClass('ee-input-pre')
                    .text(value || $this.val())
                    .hide()
                    .insertAfter($this)
                    .fadeIn(animationSpeed);
                $this.remove();

                $readonlyElement.html($readonlyElement.html().replace(/\n/g, '\n<br />'));
            }).addClass('ee-input-pre');

            return $readonlyElement;
        }
        
        function makeOptionalElementReadonly($element, $wrappingSection, value, makeReadonlyFunction) {
            value = (value === undefined && $.isFunction($element.val)) ? $element.val() : value;
            if (value) {
                makeReadonlyFunction = makeReadonlyFunction || makeElementReadonly;
                $element = makeReadonlyFunction($element, value);
                
                $wrappingSection.slideDown(animationSpeed);
            } else {
                $wrappingSection.slideUp(animationSpeed, function () {  
                    $(this).hide(); 
                });

            }
            
            return $element;
        }
        
        function makeLabelEditable($element, $newElement) {
            $element.fadeOut(animationSpeed, function () {
                var $this = $(this),
                    maxlength = $this.data('maxlength');
                $newElement
                    .attr({
                        id: $this.attr('id'),
                        className: $this.attr('class'),
                        name: $this.attr('name'),
                        minlength: $this.data('minlength'),
                        rows: $this.data('rows'),
                        cols: $this.data('cols'),
                        checked: $this.data('checked')
                    })
                    .val($this.text())
                    .hide()
                    .insertAfter($this)
                    .slideDown(animationSpeed);

                // don't set maxlength if it doesn't have one set 
                // since setting it to an invalid/default value can cause the textbox to stop working...
                if (maxlength !== '') {
                    $newElement.attr('maxlength', maxlength);
                }

                $this.remove();
            }).removeClass('ee-input-pre');

            return $newElement;
        }
        
        function makeDatePickerReadonly(datePicker) {
            var $inputWrap = $(datePicker.get_element()).closest('.ee-input'),
                $datePickerElement = $inputWrap.find('.RadPicker'),
                $dateLabel = $inputWrap.find('.ee-date-pre');

            if ($dateLabel.length === 0) {
                $dateLabel = $('<span />').addClass('ee-date-pre').insertAfter($datePickerElement);
            }
            
            $datePickerElement.slideUp(animationSpeed, function () {
                var dateInput = datePicker.get_dateInput();
                $dateLabel
                    .text(dateInput.get_dateFormatInfo().FormatDate(datePicker.get_selectedDate(), dateInput.get_displayDateFormat()))
                    .hide()
                    .fadeIn(animationSpeed);
                    
                $datePickerElement.hide();
            });
        }
        
        function makeDatePickerEditable(datePicker) {
            var $inputWrap = $(datePicker.get_element()).closest('.ee-input'),
                $datePickerElement = $inputWrap.find('.RadPicker'),
                $dateLabel = $inputWrap.find('.ee-date-pre');

            $dateLabel.slideUp(animationSpeed, function () {
                $dateLabel.hide();
                $datePickerElement.fadeIn(animationSpeed);
            });
        }
        
        function makeSelectReadonly($select) {
            var $inputWrap = $select.closest('.ee-input'),
                $label = $inputWrap.find('.ee-select-pre');

            if ($label.length === 0) {
                $label = $('<span />').addClass('ee-select-pre').insertAfter($select);
            }
            
            $select.slideUp(animationSpeed, function () {
                $label
                    .text($select.find('option:selected').text())
                    .hide()
                    .fadeIn(animationSpeed);
                    
                $select.hide();
            });

            return $label;
        }
        
        function makeSelectEditable($select) {
            var $label = $select.closest('.ee-input').find('.ee-select-pre');

            $label.slideUp(animationSpeed, function () {
                $label.hide();
                $select.fadeIn(animationSpeed);
            });
        }

        function makeSurveyReadOnly() {
            var timeframeLabel,
                timeframeSectionHasAnyValue;
            
            $surveyTitleTextBox = makeElementReadonly($surveyTitleTextBox);
            $surveyDescriptionTextArea = makeOptionalElementReadonly($surveyDescriptionTextArea, $('.ee-description'));

            timeframeLabel = makeOptionalElementReadonly(
                startDatePicker, 
                $('.ee-start-date'), 
                startDatePicker.get_selectedDate(), 
                makeDatePickerReadonly
            );
            timeframeSectionHasAnyValue = timeframeLabel !== startDatePicker; // if the element was converted to a label, then it has a value
            $preStartTextArea = makeOptionalElementReadonly($preStartTextArea, $('.ee-pre-start'));
            timeframeSectionHasAnyValue = timeframeSectionHasAnyValue || $preStartTextArea.is('span');
            timeframeLabel = makeOptionalElementReadonly(
                endDatePicker, 
                $('.ee-end-date'), 
                endDatePicker.get_selectedDate(), 
                makeDatePickerReadonly
            );
            timeframeSectionHasAnyValue = timeframeSectionHasAnyValue || timeframeLabel !== endDatePicker;
            $postEndTextArea = makeOptionalElementReadonly($postEndTextArea, $('.ee-post-end'));
            timeframeSectionHasAnyValue = timeframeSectionHasAnyValue || $postEndTextArea.is('span');
            
            $('.ee-timeframe.ee-expanded legend a').click();
            if (!timeframeSectionHasAnyValue) {
                $('.ee-timeframe').slideUp(animationSpeed);
            }
            
            $sendNotificationCheckBox = makeOptionalElementReadonly(
                $sendNotificationCheckBox, 
                $('.ee-notification'), 
                $sendNotificationCheckBox.is(':checked') ? currentContextInfo.CheckBoxCheckedText : currentContextInfo.CheckBoxUncheckedText
            );
            $notificationFromEmailTextBox = makeOptionalElementReadonly($notificationFromEmailTextBox, $('.ee-notification-from'));
            $notificationToEmailsTextBox = makeOptionalElementReadonly($notificationToEmailsTextBox, $('.ee-notification-to'));
            $sendThankYouCheckBox = makeOptionalElementReadonly(
                $sendThankYouCheckBox, 
                $('.ee-thankyou'), 
                $sendThankYouCheckBox.is(':checked') ? currentContextInfo.CheckBoxCheckedText : currentContextInfo.CheckBoxUncheckedText
            );
            $thankYourFromEmailTextBox = makeOptionalElementReadonly($thankYourFromEmailTextBox, $('.ee-thankyou-from'));

            makeOptionalElementReadonly(
                $completionActionDropDown, 
                $('.ee-completion-action'), 
                $completionActionDropDown.find('option:selected').text(),
                makeSelectReadonly
            );
            $completionMessageTextArea = makeOptionalElementReadonly($completionMessageTextArea, $('.ee-completion-message'));
            $completionUrlTextBox = makeOptionalElementReadonly($completionUrlTextBox, $('.ee-completion-url'));

            $('.ee-email.ee-expanded legend a').click();
           
            $editSurveyButton.parent().fadeIn(animationSpeed);
            $deleteSurveyButton.parent().fadeIn(animationSpeed);
        }

        function resetCreateQuestionSection() {
            // reset the "create question" section
            $questionTextArea.val('');
            $questionRequiredCheckBox.attr('checked', true);
            $answerTypeDropDown.find('option:first').attr('selected', true);
            $shortTextAnswerPreview.slideUp(animationSpeed);
            $longTextAnswerPreview.slideUp(animationSpeed);
            $multipleAnswerSection.slideUp(animationSpeed);
            $cancelQuestionEditButton.parent().fadeOut(animationSpeed);
            $addNewQuestionButton.parent().fadeOut(animationSpeed);
            
            // remove all remove answers and related undo messages
            $multipleAnswerSection.find('li.answer-input.deleted')
                .add('.answer-inputs li.ee-undo:not(.template)')
                .remove();

            // only should have two answers by default
            $multipleAnswerSection.find('li.answer-input').remove();
            var $defaultAnswers = $multipleAnswerSection.find('li.answer-input-template');
            $defaultAnswers.clone(true).attr('class', 'answer-input').show().insertAfter($defaultAnswers).find('.answer-num').text(2);
            $defaultAnswers.clone(true).attr('class', 'answer-input').show().insertAfter($defaultAnswers).find('.answer-num').text(1);
            
            $('.ai-input input').val('');
            
            $saveQuestionButton
                .text(currentContextInfo.SaveQuestionButtonText)
                .attr('title', currentContextInfo.SaveQuestionToolTip)
                .parent()
                .addClass('disabled');
            
            // clear out stored data values
            $createQuestionArea.removeData('questionId').removeData('relativeOrder')
                .find('#MultipleAnswer li.answer-input').removeData('answerId');

            validator.resetForm();
        }
        
        function showAnswersInput(questionType) {
            var $multipleAnswer = $multipleAnswerSection,
                $shortTextAnswer = $shortTextAnswerPreview,
                $longAnswerText = $longTextAnswerPreview,
                $addAnswerButton = $('.ee-define-answer .primary-btn'),
                $saveQuestionButtonWrap = $saveQuestionButton.parent(),
                $cancelButtonWrap = $cancelQuestionEditButton.parent();
                
            $saveQuestionButtonWrap.removeClass('disabled');

            // TODO: does .find('option:selected').val() give any different result than just .val()?
            if ($questionTextArea.val() || $answerTypeDropDown.find('option:selected').val() > 0) {
                $cancelButtonWrap.fadeIn(animationSpeed);
            } else {
                $cancelButtonWrap.fadeOut(animationSpeed);
            }

            switch (questionType) {
            case answerType.none:
                $longAnswerText.slideUp(animationSpeed);
                $shortTextAnswer.slideUp(animationSpeed);
                $multipleAnswer.slideUp(animationSpeed);
                $addAnswerButton.slideUp(animationSpeed);
                
                $saveQuestionButtonWrap.addClass('disabled');
                break;
            case answerType.textBox:
                $multipleAnswer.slideUp(animationSpeed);
                $longAnswerText.slideUp(animationSpeed);
                $addAnswerButton.fadeOut(animationSpeed, function () {
                    $shortTextAnswer.slideDown(animationSpeed);
                });
                break;
            case answerType.textArea:
                $shortTextAnswer.slideUp(animationSpeed);
                $multipleAnswer.slideUp(animationSpeed);
                $addAnswerButton.fadeOut(animationSpeed, function () {
                    $longAnswerText.slideDown(animationSpeed);
                });
                break;
            default: // multiple answer
                $longAnswerText.slideUp(animationSpeed);
                $shortTextAnswer.slideUp(animationSpeed, function () {
                    $multipleAnswer.slideDown(animationSpeed);
                    $addAnswerButton.fadeIn(animationSpeed);
                });
            }
        }
        
        function populateCreateQuestionSection($questionLi, setQuestionData) {
            resetCreateQuestionSection();
                    
            var questionType = $questionLi.data('questionType'),
                questionId = $questionLi.data('questionId'),
                $baseAnswerElement;
            
            // set the "edit" question text and required-nedd based on the "preview" question text and required-ness
            $questionTextArea.val($questionLi.children('.pv-question').text());
            $questionRequiredCheckBox.attr('checked', $questionLi.children('.ee-required-label').text() === '*');
            
            if (setQuestionData) {
                // set the question id on the "edit" section based on the question id in the "preview" section
                $createQuestionArea
                    .data('questionId', questionId)
                    .data('relativeOrder', $questionPreviewList.find('li.ee-preview').index($questionLi) + 1);
            }
            
            // set the "edit" answer type based on the "preview" answer type
            $answerTypeDropDown.val(questionType);
            
            showAnswersInput(questionType);
            
            if (questionType !== answerType.textBox && questionType !== answerType.textArea && questionType !== answerType.none) {

                $cancelQuestionEditButton.parent().show();

                //clone an existing element
                $baseAnswerElement = $(".answer-inputs li.answer-input:last").clone(true);
                
                //wipe out all of the answers
                $('.answer-inputs li.answer-input').remove();
                
                //get each answer
                $questionLi.find('.pv-answer').find('input, option').each(function (i) {
                
                    var $answerElement = $baseAnswerElement.clone(true);
                
                    // increment answer number
                    $answerElement.find('.answer-num').text(i + 1);

                    // update cloned textbox's value
                    $answerElement.find('input').val($(this).text() || $(this).parent().text());
                    
                    //append answer LI to UL and set the answer id
                    $answerElement.appendTo('.answer-inputs');
                    
                    if (setQuestionData) {
                        $answerElement.data('answerId', $(this).data('answerId'));
                    }
                });
            }
        }
        
        function addQuestionPreview(questionId, questionText, isRequired, questionType, answers) {
            var questionOrder = $createQuestionArea.data('relativeOrder'),
                $questionElement, 
                $answerDiv,
                $dropDown;
            if (questionOrder) {
                $questionElement = $('.ee-preview').eq(questionOrder - 1);
            } else {
                $questionElement = $('.ee-preview-template').clone(true).attr('class', 'ee-preview');
                
                // if this is the first question, just use the hidden element
                // otherwise, clone that element and replace its values
                ////if ($questionElement.data('questionId')) {
                $questionPreviewList.append($questionElement);
                ////}
            }
            
            // update the new question preview
            $questionElement.find('.pv-question').text(questionText).show();
            $questionElement.find('.ee-required-label').text(isRequired ? '*' : '').show();
            $questionElement.show().data('questionId', questionId).data('questionType', questionType);
            
            // update the preview with answer values
            $answerDiv = $questionElement.find('.pv-answer').empty();
            switch (questionType) {
            case answerType.textBox:
                $answerDiv.html("<input type='text' class='NormalTextBox' />");
                break;
            case answerType.textArea:
                $answerDiv.html("<textarea class='NormalTextBox' />");
                break;
            case answerType.dropDown:
                $dropDown = $("<select class='NormalTextBox dropdown-prev'></select>");
                $answerDiv.append($dropDown);
                $.each(answers, function (i, answer) {
                    $("<option>" + answer.Text + "</option>").appendTo($dropDown).data('answerId', answer.AnswerId);
                });
                break;
            case answerType.verticalRadioButtons:
                $.each(answers, function (i, answer) {
                    $("<label><input type='radio' name='" + questionId + "' />" + answer.Text + "</label>")
                        .appendTo($answerDiv)
                        .find('input')
                        .data('answerId', answer.AnswerId);
                });
                break;
            case answerType.checkBox:
                $.each(answers, function (i, answer) {
                    $("<label><input type='checkbox' />" + answer.Text + "</label>")
                        .appendTo($answerDiv)
                        .find('input')
                        .data('answerId', answer.AnswerId);
                });
                break;
            default:
                alert("todo: implement validation, shouldn't be able to add a question if you have 'select answer type' selected in the drop down.");
            }
        }

        $('.ee-collapsed legend a, .ee-expanded legend a').click(function (event) {
            event.preventDefault();
            
            var $collapsableSectionWrap = $(this).closest('.ee-collapsed, .ee-expanded'),
                $collapsableSection = $collapsableSectionWrap.find('.ee-collapsable');
            
            $collapsableSection.slideToggle(animationSpeed, function () {
                $collapsableSectionWrap.toggleClass('ee-collapsed').toggleClass('ee-expanded');
            });            
        });

        $questionPreviewList.sortable({
            items: 'li.ee-preview', 
            placeholder: 'ui-state-highlight'
        }).bind('sortupdate', function () {
            // after reordering questions
            var questionOrderMap = {},
                parameters;
            $questionPreviewList.find('li.ee-preview:visible').each(function (i, elem) {
                questionOrderMap[$(elem).data('questionId')] = i + 1;
            });
            
            parameters = {
                surveyId: $('.ee-create-new').data('surveyId'),
                questionOrderMap: questionOrderMap
            };
            
            callWebMethod('ReorderQuestions', parameters);
        });
        
        $(".answer-inputs").sortable({
            items: 'li.answer-input',
            placeholder: 'ui-state-highlight'
        }).bind('sortupdate', function () {
            // after reordering answers
            var $answerNumberElements = $(".answer-inputs li.answer-input:visible").find('.answer-num');
            $answerNumberElements.each(function (i, elem) {
                $(elem).text(i + 1);
            });
        });
        
        // Add selection style back to the inputs, since our CSS is removing or hiding the native style
        $moduleWrap.find(":input").focus(function () {
            $(this).addClass("focus");
        }).blur(function () {
            $(this).removeClass("focus");
        });

        $newSurveyButton.click(function (event) {
            event.preventDefault();
            
            if (validator.form()) {
                var $this = $(this),
                    originalText = $this.text();

                $this.text(currentContextInfo.ProgressText);                
                updateSurvey(function () {
                    $newSurveyButton.parent().fadeOut(animationSpeed, function () {
                        makeSurveyReadOnly();
                    });
                    $('.ee-create-questions').show();
                    $this.text(originalText);
                });
            }
        });
        
        $updateSurveyButton.click(function (event) {
            event.preventDefault();
            
            if (validator.form()) {
                var $this = $(this),
                    originalText = $this.text();
                
                $this.text(currentContextInfo.ProgressText);
                updateSurvey(function () {
                    hideEditModeButtons(function () {
                        makeSurveyReadOnly();
                    });
                    $this.text(originalText);
                });
            }
        });

        $editSurveyButton.click(function (event) {
            event.preventDefault();
            
            // save current value to "previous value" data field for usage in the cancel link click event.
            storePreviousValue($surveyTitleTextBox);
            storePreviousValue($surveyDescriptionTextArea);
            storePreviousValue($('.ee-start-date .RadPicker'), startDatePicker.get_selectedDate());
            storePreviousValue($preStartTextArea);
            storePreviousValue($('.ee-end-date .RadPicker'), endDatePicker.get_selectedDate());
            storePreviousValue($postEndTextArea);
            storePreviousValue($sendNotificationCheckBox, $sendNotificationCheckBox.is(':checked'));
            storePreviousValue($notificationFromEmailTextBox);
            storePreviousValue($notificationToEmailsTextBox);
            storePreviousValue($sendThankYouCheckBox, $sendThankYouCheckBox.is(':checked'));
            storePreviousValue($thankYourFromEmailTextBox);
            storePreviousValue($completionActionDropDown);
            storePreviousValue($completionMessageTextArea);
            storePreviousValue($completionUrlTextBox);
            
            $surveyTitleTextBox = makeLabelEditable($surveyTitleTextBox, $('<input type="text"/>'));
            $surveyDescriptionTextArea = makeLabelEditable($surveyDescriptionTextArea, $('<textarea/>'));
            makeDatePickerEditable(startDatePicker);
            $preStartTextArea = makeLabelEditable($preStartTextArea, $('<textarea/>'));
            makeDatePickerEditable(endDatePicker);
            $postEndTextArea = makeLabelEditable($postEndTextArea, $('<textarea/>'));
            $sendNotificationCheckBox = makeLabelEditable($sendNotificationCheckBox, $('<input type="checkbox"/>'));
            $notificationFromEmailTextBox = makeLabelEditable($notificationFromEmailTextBox, $('<input type="text"/>'));
            $notificationToEmailsTextBox = makeLabelEditable($notificationToEmailsTextBox, $('<input type="text"/>'));
            $sendThankYouCheckBox = makeLabelEditable($sendThankYouCheckBox, $('<input type="checkbox"/>'));
            $thankYourFromEmailTextBox = makeLabelEditable($thankYourFromEmailTextBox, $('<input type="text"/>'));
            makeSelectEditable($completionActionDropDown);
            $completionMessageTextArea = makeLabelEditable($completionMessageTextArea, $('<textarea/>'));
            $completionUrlTextBox = makeLabelEditable($completionUrlTextBox, $('<input type="text"/>'));
            
            $('.ee-create-new .ee-optional').show();
            $editSurveyButton.parent().fadeOut(animationSpeed, function () {
                $cancelSurveyEditButton.parent().fadeIn(animationSpeed);
                $updateSurveyButton.parent().fadeIn(animationSpeed);
            });

            validator = $form.validate();
        });
        
        $cancelSurveyEditButton.click(function (event) {
            event.preventDefault();
            
            // retrieve data values and reset the text boxes.
            resetToPreviousValue($surveyTitleTextBox);
            resetToPreviousValue($surveyDescriptionTextArea);
            resetDatePickerToPreviousValue(startDatePicker);
            resetToPreviousValue($preStartTextArea);
            resetDatePickerToPreviousValue(endDatePicker);
            resetToPreviousValue($postEndTextArea);
            resetCheckBoxToPreviousValue($sendNotificationCheckBox);
            resetToPreviousValue($notificationFromEmailTextBox);
            resetToPreviousValue($notificationToEmailsTextBox);
            resetCheckBoxToPreviousValue($sendThankYouCheckBox);
            resetToPreviousValue($thankYourFromEmailTextBox);
            
            hideEditModeButtons(function () {
                makeSurveyReadOnly();
            });

            validator.resetForm();
        });

        $deleteSurveyButton.click(function (event) {
            event.preventDefault();

            deleteWithUndo($moduleWrap, true, null, function deleteCallback() { 
                callWebMethod('DeleteSurvey', { surveyId: $('.ee-create-new').data('surveyId') }, function () {
                    window.location = $('.egn-home a').attr('href');
                });
            });
        });
        
        $addNewQuestionButton.click(function (event) {
            event.preventDefault();
            
            var $answerNumberElement = $(".answer-input:visible:last .answer-num"),
                $answerElement = $(".answer-input-template")
                                .clone(true)
                                .attr('class', 'answer-input')
                                .hide()
                                .appendTo('.answer-inputs')
                                .slideDown(animationSpeed),
                answerNumber = parseInt($answerNumberElement.text(), 10);
            
            $answerElement.find('.answer-num').text(answerNumber + 1);

            // clear out cloned textbox
            $answerElement.find('input').val('').focus();
            
            $(".answer-inputs .ee-delete").removeClass('disabled');
        });
        
        // remove answer
        $(".answer-inputs .ee-delete").click(function (event) {
            event.preventDefault();

            var $answers = $(".answer-inputs li.answer-input:visible"),
                $parentAnswerElement;
            
            if ($answers.length > 1) {
                
                $parentAnswerElement = $(this).closest('li');
                deleteWithUndo($parentAnswerElement, false, function afterFadeOut() {
                    $answers = $(".answer-inputs li.answer-input:visible").each(function (i, elem) {
                        $(elem).find('.answer-num').text(i + 1);
                    });
                    
                    if ($answers.length < 2) {
                        $answers.find('.ee-delete').addClass('disabled');
                    }
                }, null, function afterUndo() {
                    $(".answer-inputs li.answer-input:visible").each(function (i, elem) {
                        $(elem).find('.answer-num').text(i + 1);
                    }).find('.ee-delete').removeClass('disabled');
                });
            }
        });
        
        // edit question
        $('.ee-pr-action-links .ee-edit').click(function (event) {
            event.preventDefault();

            var $questionLi = $(this).closest('li.ee-preview');
            populateCreateQuestionSection($questionLi, true);
            $saveQuestionButton.text(currentContextInfo.UpdateQuestionButtonText).attr('title', currentContextInfo.UpdateQuestionToolTip);
        });
        
        // copy question
        $('.ee-pr-action-links .ee-copy').click(function (event) {
            event.preventDefault();
            
            var $questionLi = $(this).closest('li.ee-preview');
            populateCreateQuestionSection($questionLi, false);
            $saveQuestionButton.text(currentContextInfo.SaveQuestionButtonText).attr('title', currentContextInfo.SaveQuestionToolTip);
        });
        
        // delete question
        $('.ee-pr-action-links .ee-delete').click(function (event) {
            event.preventDefault();
            
            var $parentQuestionElement = $(this).closest('li.ee-preview');
            deleteWithUndo($parentQuestionElement, true, null, function deleteCallback() {
                var questionId = $parentQuestionElement.data('questionId');
                callWebMethod('DeleteQuestion', { questionId: questionId }, function () {
                    $parentQuestionElement.remove();
                });
            });
        });
        
        $answerTypeDropDown.change(function () {
            showAnswersInput(parseInt($(this).val(), 10));
        });

        $questionTextArea.blur(function () {
            showAnswersInput(parseInt($answerTypeDropDown.val(), 10));
        });
        
        $saveQuestionButton.click(function (event) {
            event.preventDefault();
            
            var questionType = $answerTypeDropDown.find(':selected').val(),
                questionIsMultipleChoice = questionType > 2; 
            
            validator = $form.validate();
            if ($questionTextArea.valid() &&
                    (!questionIsMultipleChoice || $('.ai-input input:visible').valid()) &&
                    ($answerTypeDropDown.valid())) {
            
                $(this).text(currentContextInfo.ProgressText).parent().addClass('disabled');
                callWebMethod('UpdateQuestion', getQuestionParameters(), function (question) {
                    $previewArea.slideDown(animationSpeed);
                    
                    addQuestionPreview(
                        question.QuestionId, 
                        $questionTextArea.val(), 
                        $questionRequiredCheckBox.is(':checked'), 
                        parseInt(questionType, 10), 
                        question.Answers
                    );
                        
                    resetCreateQuestionSection();
                });
            }
        });

        $cancelQuestionEditButton.click(function (event) {
            event.preventDefault();
            resetCreateQuestionSection();
        });

        function parseDateString(dateValue) {
            if (dateValue) {
                return new Date(parseInt(dateValue.replace("/Date(", "").replace(")/", ""), 10));
            }
            
            return null;
        }
        
        function processHtmlTextForTextarea(value) {
            return $('<div/>')
                .html(value)
                    .find('br')
                    .replaceWith('\n')
                    .end()
                .text();
        }

        (function initializeControls() {
            if (startDatePicker === null || endDatePicker === null) {
                setTimeout(function () {
                    startDatePicker = $find($('.ee-start-date .RadPicker input').attr('id'));
                    endDatePicker = $find($('.ee-end-date .RadPicker input').attr('id'));
                    initializeControls();
                }, 1);

                return;
            }

            if (currentContextInfo.Survey) {
                $('.ee-create-new').data('surveyId', currentContextInfo.Survey.SurveyId);
                $surveyTitleTextBox.val(currentContextInfo.Survey.Text);
                $surveyDescriptionTextArea.val(currentContextInfo.Survey.Sections[0].Text);
                startDatePicker.set_selectedDate(parseDateString(currentContextInfo.Survey.StartDate));
                $preStartTextArea.val(currentContextInfo.Survey.PreStartMessage);
                endDatePicker.set_selectedDate(parseDateString(currentContextInfo.Survey.EndDate));
                $postEndTextArea.val(currentContextInfo.Survey.PostEndMessage);
			    $sendNotificationCheckBox.attr('checked', currentContextInfo.Survey.SendNotification);
			    $notificationFromEmailTextBox.val(currentContextInfo.Survey.NotificationFromEmailAddress);
			    $notificationToEmailsTextBox.val(currentContextInfo.Survey.NotificationToEmailAddresses);
			    $sendThankYouCheckBox.attr('checked', currentContextInfo.Survey.SendThankYou);
			    $thankYourFromEmailTextBox.val(currentContextInfo.Survey.ThankYouFromEmailAddress);
			    $completionActionDropDown.val(currentContextInfo.Survey.FinalMessageOption);
                $completionMessageTextArea.val(processHtmlTextForTextarea(currentContextInfo.Survey.FinalMessage));
			    $completionUrlTextBox.val(currentContextInfo.Survey.FinalUrl);

                $newSurveyButton.parent().hide();
                makeSurveyReadOnly();
                hideEditModeButtons();
                $('.ee-create-questions').show(); 

                if (currentContextInfo.Survey.Sections[0].Questions.length) {
                    $previewArea.show();

                    $.each(currentContextInfo.Survey.Sections[0].Questions, function (i, question) {
                        addQuestionPreview(question.QuestionId, question.Text, question.IsRequired, question.ControlType, question.Answers);
                    });
                }
            } else {
                $sendNotificationCheckBox.attr('checked', currentContextInfo.DefaultEmailSettings.SendNotification);
			    $notificationFromEmailTextBox.val(currentContextInfo.DefaultEmailSettings.NotificationFromEmail);
			    $notificationToEmailsTextBox.val(currentContextInfo.DefaultEmailSettings.NotificationToEmails);
                $sendThankYouCheckBox.attr('checked', currentContextInfo.DefaultEmailSettings.SendThankYou);
			    $thankYourFromEmailTextBox.val(currentContextInfo.DefaultEmailSettings.ThankYouFromEmail);
                $completionMessageTextArea.val(processHtmlTextForTextarea(currentContextInfo.DefaultCompletionMessage));
            }
            
            resetCreateQuestionSection();
        }());
    });
}(jQuery, this));