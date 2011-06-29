/// <reference path="jquery-1.3.2.debug-vsdoc.js" />
/// <reference path="json2.js" />
/// <reference path="jquery-ui-1.7.2.min.js" />
/// <reference path="jquery.validate-1.7.min.js" />
/// <reference path="Array.prototype.indexOf.js" />

(function($){
    $(function () {
        $.validator.addMethod("email", function (value, element) {
            return this.optional(element) || CurrentContextInfo.EmailRegex.test(value);
        });
        $.validator.addMethod("emails", function (value, element) {
            return this.optional(element) || CurrentContextInfo.EmailsRegex.test(value);
        });
        $.validator.setDefaults({
            rules: {
                DefineAnswerType: { min: 1 },
                required: { required: true },
                notificationEmail: { 
                    required: { depends: function (element) { return $('#EvalSendNotification').is(':checked'); } },
                    email: true
                },
                notificationEmails: { 
                    required: { depends: function (element) { return $('#EvalSendNotification').is(':checked'); } }, 
                    emails: true
                },
                thankYouEmail: { 
                    required: { depends: function (element) { return $('#EvalSendThankYou').is(':checked'); } }, 
                    email: true
                }
            },
            messages: CurrentContextInfo.ErrorMessages,
            onsubmit: false
        });

        var validator = $('#Form').validate({ignore: '#DefineAnswerType'}),
            AnimationSpeed = 'normal',
            pendingQuestionDeleteCallbacks = [],
            startDatePicker = $find($('.ee-start-date .RadPicker input').attr('id')),
            endDatePicker = $find($('.ee-end-date .RadPicker input').attr('id'));

        window.onbeforeunload = function () { 
            if ($('#CancelQuestion').is(':visible') || $('#EvalCancel').is(':visible')) {
                return CurrentContextInfo.UnsavedChangedWarning;
            }

            return;
        };
                   
        $('.ee-collapsed legend a, .ee-expanded legend a').click(function (event) {
            event.preventDefault();
            
            var $collapsableSectionWrap = $(this).closest('.ee-collapsed, .ee-expanded'),
                $collapsableSection = $collapsableSectionWrap.find('.ee-collapsable');
            
            $collapsableSection.slideToggle(AnimationSpeed, function () {
                $collapsableSectionWrap.toggleClass('ee-collapsed').toggleClass('ee-expanded');
            });            
        });

        $("#ee-previews").sortable({
            items: 'li.ee-preview', 
            placeholder: 'ui-state-highlight'
        });
        $(".answer-inputs").sortable({
            items: 'li.answer-input',
            placeholder: 'ui-state-highlight'
        });
        ////$("#ee-previews, .answer-inputs").disableSelection();
        
        // after reordering questions
        $('#ee-previews').bind('sortupdate', function (event, ui) {
            var questionOrderMap = {};
            $('#ee-previews li.ee-preview:visible').each(function (i, elem) {
                questionOrderMap[$(elem).data('questionId')] = i + 1;
            });
            
            var parameters = {
                surveyId: $('.ee-create-new').data('surveyId'),
                questionOrderMap: questionOrderMap
            };
            
            callWebMethod('ReorderQuestions', parameters);
        });
        
        // after reordering answers
        $('.answer-inputs').bind('sortupdate', function (event, ui) {
            var $answerNumberElements = $(".answer-inputs li.answer-input:visible").find('.answer-num');
            $answerNumberElements.each(function (i, elem) {
                $(elem).text(i + 1);
            });
        });
        
        // Add selection style back to the inputs, since our CSS is removing or hiding the native style
        $("#engage-evaluation :input").focus(function () {
            $(this).addClass("focus");
        }).blur(function () {
            $(this).removeClass("focus");
        });

        // save new survey
        $('#EvalNew').click(function (event) {
            event.preventDefault();
            
            if(validator.form()) {
                var $this = $(this),
                    originalText = $this.text();

                $this.text(CurrentContextInfo.ProgressText);                
                updateSurvey(function () {
                    $('#EvalNew').parent().fadeOut(AnimationSpeed, function () { makeSurveyReadOnly(); });
                    $('.ee-create-questions').show();
                    $this.text(originalText);
                });
            }
        });
        
        // update existing survey
        $('#EvalUpdate').click(function (event) {
            event.preventDefault();
            
            if(validator.form()) {
                var $this = $(this),
                    originalText = $this.text();
                
                $this.text(CurrentContextInfo.ProgressText);
                updateSurvey(function () {
                    hideEditModeButtons(function () { makeSurveyReadOnly(); });
                    $this.text(originalText);
                });
            }
        });
        
        function callWebMethod (methodName, parameters, callback) {
            jQuery.ajax({
                type: "POST",
                url: CurrentContextInfo.WebMethodUrl + methodName,
                data: JSON.stringify(parameters),
                contentType: "application/json; charset=utf-8",
                success: function (msg) { 
                    if ($.isFunction(callback)) {
                        callback(msg.hasOwnProperty('d') ? msg.d : msg);
                    }
                },
                error: function (/*XMLHttpRequest, textStatus, errorThrown*/) { 
                    // TODO provide a more friendly error message
                    alert(CurrentContextInfo.ErrorMessage); 
                }
            });
        }
        
        function updateSurvey(callback) {
            callWebMethod('UpdateSurvey', getSurveyParameters(), function (surveyId) {
                $('.ee-create-new').data('surveyId', surveyId); 
                if ($.isFunction(callback)) {
                    callback(surveyId);
                }
            });
        }
        
        function getSurveyParameters () {
            return {
                survey : {
                    SurveyId: $('.ee-create-new').data('surveyId') || -1,
                    Text: $('#EvalTitleInput').val(),
					StartDate: startDatePicker.get_selectedDate(),
					PreStartMessage: $('#EvalPreStartTextArea').val(),
					EndDate: endDatePicker.get_selectedDate(),
					PostEndMessage: $('#EvalPostEndTextArea').val(),
					SendNotification: $('#EvalSendNotification').is(':checked'),
					NotificationFromEmailAddress: $('#EvalNotificationFromEmail').val(),
					NotificationToEmailAddresses: $('#EvalNotificationToEmails').val(),
					SendThankYou: $('#EvalSendThankYou').is(':checked'),
					ThankYouFromEmailAddress: $('#EvalThankYouFromEmail').val(),
                    PortalId: CurrentContextInfo.PortalId,
                    ModuleId: CurrentContextInfo.ModuleId,
                    RevisingUser: CurrentContextInfo.UserId,
                    Sections: [{
                        Text: $('#EvalDescTextArea').val()
                    }]
                }
            };
        }
        
        // edit survey
        $('#EvalEdit').click(function (event) {
            event.preventDefault();
            
            // save current value to "previous value" data field for usage in the cancel link click event.
            storePreviousValue($('#EvalTitleInput'));
            storePreviousValue($('#EvalDescTextArea'));
            storePreviousValue($('.ee-start-date .RadPicker'), startDatePicker.get_selectedDate());
            storePreviousValue($('#EvalPreStartTextArea'));
            storePreviousValue($('.ee-end-date .RadPicker'), endDatePicker.get_selectedDate());
            storePreviousValue($('#EvalPostEndTextArea'));
            storePreviousValue($('#EvalSendNotification'), $('#EvalSendNotification').is(':checked'));
            storePreviousValue($('#EvalNotificationFromEmail'));
            storePreviousValue($('#EvalNotificationToEmails'));
            storePreviousValue($('#EvalSendThankYou'), $('#EvalSendThankYou').is(':checked'));
            storePreviousValue($('#EvalThankYouFromEmail'));
            
            makeLabelEditable($('#EvalTitleInput'), $('<input type="text"/>'));
            makeLabelEditable($('#EvalDescTextArea'), $('<textarea/>'));
            makeDatePickerEditable(startDatePicker);
            makeLabelEditable($('#EvalPreStartTextArea'), $('<textarea/>'));
            makeDatePickerEditable(endDatePicker);
            makeLabelEditable($('#EvalPostEndTextArea'), $('<textarea/>'));
            makeLabelEditable($('#EvalSendNotification'), $('<input type="checkbox"/>'));
            makeLabelEditable($('#EvalNotificationFromEmail'), $('<input type="text"/>'));
            makeLabelEditable($('#EvalNotificationToEmails'), $('<input type="text"/>'));
            makeLabelEditable($('#EvalSendThankYou'), $('<input type="checkbox"/>'));
            makeLabelEditable($('#EvalThankYouFromEmail'), $('<input type="text"/>'));
            
            $('.ee-create-new .ee-optional').show();
            $('#EvalEdit').parent().fadeOut(AnimationSpeed, function () {
                $('#EvalCancel').parent().fadeIn(AnimationSpeed);
                $('#EvalUpdate').parent().fadeIn(AnimationSpeed);
            });

            validator = $('#Form').validate();
        });
        
        $('#EvalCancel').click(function (event) {
            event.preventDefault();
            
            // retrieve data values and reset the text boxes.
            resetToPreviousValue($('#EvalTitleInput'));
            resetToPreviousValue($('#EvalDescTextArea'));
            resetDatePickerToPreviousValue(startDatePicker);
            resetToPreviousValue($('#EvalPreStartTextArea'));
            resetDatePickerToPreviousValue(endDatePicker);
            resetToPreviousValue($('#EvalPostEndTextArea'));
            resetCheckBoxToPreviousValue($('#EvalSendNotification'));
            resetToPreviousValue($('#EvalNotificationFromEmail'));
            resetToPreviousValue($('#EvalNotificationToEmails'));
            resetCheckBoxToPreviousValue($('#EvalSendThankYou'));
            resetToPreviousValue($('#EvalThankYouFromEmail'));
            
            hideEditModeButtons(function () { makeSurveyReadOnly(); });

            validator.resetForm();
        });

        $('#EvalDelete').click(function (event) {
            event.preventDefault();

            deleteWithUndo($('#engage-evaluation'), true, null, function deleteCallback () { 
                callWebMethod('DeleteSurvey', { surveyId: $('.ee-create-new').data('surveyId') }, function () {
                    window.location = $('.egn-home a').attr('href');
                });
            });
        });
        
        function storePreviousValue ($input, value) {
            $input.parent().data('previousValue', value === undefined ? $input.text() : value);
        }
        
        function resetToPreviousValue ($input) {
            $input.val($input.parent().data('previousValue'));
        }
            
        function resetCheckBoxToPreviousValue ($input) {
            $input.attr('checked', $input.parent().data('previousValue'));
        }
            
        function resetDatePickerToPreviousValue (datePicker) {
             datePicker.set_selectedDate($(datePicker.get_element()).closest('.RadPicker').parent().data('previousValue'));
        }
            
        function makeSurveyReadOnly () {
            var timeframeSectionHasAnyValue,
                timeframeElementHasValue;
            makeElementReadonly($('#EvalTitleInput'));
            makeOptionalElementReadonly($('#EvalDescTextArea'), $('.ee-description'));
            
            timeframeElementHasValue = makeOptionalElementReadonly(startDatePicker, $('.ee-start-date'), startDatePicker.get_selectedDate(), makeDatePickerReadonly);
            timeframeSectionHasAnyValue = timeframeElementHasValue;
            timeframeElementHasValue = makeOptionalElementReadonly($('#EvalPreStartTextArea'), $('.ee-pre-start'));
            timeframeSectionHasAnyValue = timeframeSectionHasAnyValue || timeframeElementHasValue;
            timeframeElementHasValue = makeOptionalElementReadonly(endDatePicker, $('.ee-end-date'), endDatePicker.get_selectedDate(), makeDatePickerReadonly);
            timeframeSectionHasAnyValue = timeframeSectionHasAnyValue || timeframeElementHasValue;
            timeframeElementHasValue = makeOptionalElementReadonly($('#EvalPostEndTextArea'), $('.ee-post-end'));
            timeframeSectionHasAnyValue = timeframeSectionHasAnyValue || timeframeElementHasValue;
            
            $('.ee-timeframe.ee-expanded legend a').click();
            if (!timeframeSectionHasAnyValue) {
                $('.ee-timeframe').slideUp(AnimationSpeed);
            }
            
            makeOptionalElementReadonly($('#EvalSendNotification'), $('.ee-notification'), $('#EvalSendNotification').is(':checked') ? CurrentContextInfo.CheckBoxCheckedText : CurrentContextInfo.CheckBoxUncheckedText);
            makeOptionalElementReadonly($('#EvalNotificationFromEmail'), $('.ee-notification-from'));
            makeOptionalElementReadonly($('#EvalNotificationToEmails'), $('.ee-notification-to'));
            makeOptionalElementReadonly($('#EvalSendThankYou'), $('.ee-thankyou'), $('#EvalSendThankYou').is(':checked') ? CurrentContextInfo.CheckBoxCheckedText : CurrentContextInfo.CheckBoxUncheckedText);
            makeOptionalElementReadonly($('#EvalThankYouFromEmail'), $('.ee-thankyou-from'));
            
            $('.ee-email.ee-expanded legend a').click();
           
            $('#EvalEdit').parent().fadeIn(AnimationSpeed);
            $('#EvalDelete').parent().fadeIn(AnimationSpeed);
        }
        
        function makeOptionalElementReadonly ($element, $wrappingSection, value, makeReadonlyFunction) {
            value = (value === undefined && $.isFunction($element.val)) ? $element.val() : value;
            if (value) {
                makeReadonlyFunction = makeReadonlyFunction || makeElementReadonly
                makeReadonlyFunction($element, value);
                
                $wrappingSection.slideDown(AnimationSpeed);
                
                return true;
            }
            else {
                $wrappingSection.slideUp(AnimationSpeed, function () { $(this).hide(); });
            }
        }
        
        function makeElementReadonly($element, value) {
            $element.slideUp(AnimationSpeed, function () {
                var $this = $(this),
                    maxlength = $this.attr('maxlength'),
                    $readonlyElement;

                // if maxlength is not set (as on NotificationToEmails) then the browser default is returned (http://herr-schuessler.de/blog/selecting-input-fields-with-maxlength-via-jquery/)
                if (maxlength < 0 || maxlength > 500000) {
                    maxlength = '';
                }

                $readonlyElement = 
                    $('<span />')
                        .attr({
                            id: $this.attr('id'),
                            className: $this.attr('class'),
                            name: $this.attr('name')
                        }).data('minlength', $this.attr('minlength') || '') // if $this.attr('minlength') is null, then jQuery sees the data call as an accessor instead of a setter, so we change it to '' if it's null
                        .data('maxlength', maxlength || '')
                        .data('rows', $this.attr('rows') || '')
                        .data('cols', $this.attr('cols') || '')
                        .data('checked', $this.attr('checked') !== undefined ? $this.is(':checked') : '')
                        .addClass('ee-input-pre')
                        .text(value || $this.val())
                        .hide()
                        .insertAfter($this)
                        .fadeIn(AnimationSpeed);
                $this.remove();

                $readonlyElement.html($readonlyElement.html().replace(/\n/g, '\n<br />'));
            }).addClass('ee-input-pre');
        }
        
        function makeLabelEditable($element, $newElement) {
            $element.fadeOut(AnimationSpeed, function () {
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
                    .slideDown(AnimationSpeed);

                // don't set maxlength if it doesn't have one set (since setting it to an invalid/default value can cause the textbox to stop working...)
                if (maxlength !== '') {
                    $newElement.attr('maxlength', maxlength);
                }

                $this.remove();
            }).removeClass('ee-input-pre');
        }
        
        function makeDatePickerReadonly(datePicker) {
            var $inputWrap = $(datePicker.get_element()).closest('.ee-input'),
                $datePickerElement = $inputWrap.find('.RadPicker'),
                $dateLabel = $inputWrap.find('.ee-date-pre');

            if ($dateLabel.length === 0) {
                $dateLabel = $('<span />').addClass('ee-date-pre').insertAfter($datePickerElement);
            }
            
            $datePickerElement.slideUp(AnimationSpeed, function () {
                var dateInput = datePicker.get_dateInput();
                $dateLabel
                    .text(dateInput.get_dateFormatInfo().FormatDate(datePicker.get_selectedDate(), dateInput.get_displayDateFormat()))
                    .hide()
                    .fadeIn(AnimationSpeed);
                    
                $datePickerElement.hide();
            });
        }
        
        function makeDatePickerEditable(datePicker) {
            var $inputWrap = $(datePicker.get_element()).closest('.ee-input'),
                $datePickerElement = $inputWrap.find('.RadPicker'),
                $dateLabel = $inputWrap.find('.ee-date-pre');

            $dateLabel.slideUp(AnimationSpeed, function () {
                $datePickerElement.fadeIn(AnimationSpeed);
            });
        }

        function hideEditModeButtons (callback) {
            $('#EvalUpdate').parent().fadeOut(AnimationSpeed);
            $('#EvalCancel').parent().fadeOut(AnimationSpeed, callback);
        }
        
        // add answer
        $("#AddNewQuestion").click(function (event) {
            event.preventDefault();
            
            var $answerNumberElement = $(".answer-input:visible:last .answer-num");
            var $answerElement = $(".answer-input-template").clone(true).attr('class', 'answer-input').hide().appendTo('.answer-inputs').slideDown(AnimationSpeed);
            
            // increment answer number
            var answerNumber = parseInt($answerNumberElement.text(), 10);
            $answerElement.find('.answer-num').text(answerNumber + 1);

            // clear out cloned textbox
            $answerElement.find('input').val('').focus();
            
            $(".answer-inputs .ee-delete").removeClass('disabled');
        });
        
        // remove answer
        $(".answer-inputs .ee-delete").click(function (event) {
            event.preventDefault();

            var $answers = $(".answer-inputs li.answer-input:visible")
            if ($answers.length > 1) {
                
                var $parentAnswerElement = $(this).closest('li');
                deleteWithUndo($parentAnswerElement, false, function afterFadeOut () {
                    $answers = $(".answer-inputs li.answer-input:visible").each(function (i, elem) {
                        $(elem).find('.answer-num').text(i + 1);
                    });
                    
                    if ($answers.length < 2) {
                        $answers.find('.ee-delete').addClass('disabled');
                    }
                }, null, function afterUndo () {
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
            $('#SaveQuestion').text(CurrentContextInfo.UpdateQuestionButtonText).attr('title', CurrentContextInfo.UpdateQuestionToolTip);
        });
        
        // copy question
        $('.ee-pr-action-links .ee-copy').click(function (event) {
            event.preventDefault();
            
            var $questionLi = $(this).closest('li.ee-preview');
            populateCreateQuestionSection($questionLi, false);
            $('#SaveQuestion').text(CurrentContextInfo.SaveQuestionButtonText).attr('title', CurrentContextInfo.SaveQuestionToolTip);
        });
        
        // delete question
        $('.ee-pr-action-links .ee-delete').click(function (event) {
            event.preventDefault();
            
            var $parentQuestionElement = $(this).closest('li.ee-preview');
            deleteWithUndo($parentQuestionElement, true, null, function deleteCallback () {
                var questionId = $parentQuestionElement.data('questionId');
                callWebMethod('DeleteQuestion', { questionId: questionId }, function () {
                    $parentQuestionElement.remove();
                });
            });
        });
        
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
                
                $undoElement.hide().fadeIn(AnimationSpeed);
                
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
	                (function updateUndoTimer () {
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
                    
                    $undoElement.fadeOut(AnimationSpeed, function() {
                        $element.removeClass('deleted').fadeIn(AnimationSpeed);
                        $undoElement.remove();
                    });
                    
                    if ($.isFunction(afterUndo)) {
                        afterUndo();
                    }
                });
            });
        }
        
        $(window).unload(function () {
            // when the user leaves the page, finish any pending question deletions
            $.each(pendingQuestionDeleteCallbacks, function (i, deleteQuestionFunction) {
                if ($.isFunction(deleteQuestionFunction)) {
                    deleteQuestionFunction();
                }
            });
        });
        
        function populateCreateQuestionSection ($questionLi, setQuestionData) {
            resetCreateQuestionSection();
                    
            var questionType = $questionLi.data('questionType');
            var questionId = $questionLi.data('questionId');
            
            // set the "edit" question text and required-nedd based on the "preview" question text and required-ness
            $('#QuestionText').val($questionLi.children('.pv-question').text());
            $('#QuestionRequiredCheckBox').attr('checked', $questionLi.children('.ee-required-label').text() === '*');
            
            if (setQuestionData) {
                // set the question id on the "edit" section based on the question id in the "preview" section
                $('#CreateQuestions').data('questionId', questionId).data('relativeOrder', $('#ee-previews li.ee-preview').index($questionLi) + 1);
            }
            
            // set the "edit" answer type based on the "preview" answer type
            $('#DefineAnswerType').val(questionType);
            
            ShowAnswersInput(questionType);
            
            if (questionType != 2 && questionType != 1 && questionType != 0) { // Not SmallTextInputField or LargeTextInputField or ControlType.None

                $('#CancelQuestion').parent().show();

                //clone an existing element
                var $baseAnswerElement = $(".answer-inputs li.answer-input:last").clone(true);
                
                //wipe out all of the answers
                $('.answer-inputs li.answer-input').remove();
                
                //get each answer
                $questionLi.find('.pv-answer').find('input, option').each(function (i) {
                
                    var $answerElement = $baseAnswerElement.clone(true);
                
                    // increment answer number
                    var $answerNumberElement = $answerElement.find('.answer-num').text(i + 1);

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

        // change answer type
        $('#DefineAnswerType').change(function (event) {
            ShowAnswersInput(parseInt($(this).val(), 10));
        });

        // question textbox onblur
        $('#QuestionText').blur(function (event) {
            ShowAnswersInput(parseInt($('#DefineAnswerType').val(), 10));
        });
        
        function ShowAnswersInput(questionType) {
            var $multipleAnswer = $('#MultipleAnswer'),
                $shortTextAnswer = $('#ShortTextAnswer'),
                $longAnswerText = $('#LongTextAnswer'),
                $addAnswerButton = $('.ee-define-answer .primary-btn'),
                $saveQuestionButtonWrap = $('#SaveQuestion').parent(),
                $cancelButtonWrap = $('#CancelQuestion').parent();
                
            $saveQuestionButtonWrap.removeClass('disabled');

            if ($('#QuestionText').val() || $('#DefineAnswerType :selected').val() > 0) {
                $cancelButtonWrap.fadeIn(AnimationSpeed);
            }
            else {
                $cancelButtonWrap.fadeOut(AnimationSpeed);
            }

            switch (questionType) {
            case 0:
                // ControlType.None
                $longAnswerText.slideUp(AnimationSpeed);
                $shortTextAnswer.slideUp(AnimationSpeed);
                $multipleAnswer.slideUp(AnimationSpeed);
                $addAnswerButton.slideUp(AnimationSpeed);
                
                $saveQuestionButtonWrap.addClass('disabled');
                break;
            case 2:
                // ControlType.SmallTextInputField
                $multipleAnswer.slideUp(AnimationSpeed);
                $longAnswerText.slideUp(AnimationSpeed);
                $addAnswerButton.fadeOut(AnimationSpeed, function () {
                    $shortTextAnswer.slideDown(AnimationSpeed);
                });
                break;
            case 1:
                // ControlType.LargeTextInputField
                $shortTextAnswer.slideUp(AnimationSpeed);
                $multipleAnswer.slideUp(AnimationSpeed);
                $addAnswerButton.fadeOut(AnimationSpeed, function () {
                    $longAnswerText.slideDown(AnimationSpeed);
                });
                break;
            default: 
                // multiple answer
                $longAnswerText.slideUp(AnimationSpeed);
                $shortTextAnswer.slideUp(AnimationSpeed, function () {
                    $multipleAnswer.slideDown(AnimationSpeed);
                    $addAnswerButton.fadeIn(AnimationSpeed);
                });
            }
        }

        // save questions
        $('#SaveQuestion').click(function (event) {
            event.preventDefault();
            
            var questionType = $('#DefineAnswerType :selected').val(),
                questionIsMultipleChoice = questionType > 2; 
            
            validator = $('#Form').validate();
            if ($('#QuestionText').valid() &&
               (!questionIsMultipleChoice || $('.ai-input input:visible').valid()) &&
               ($('#DefineAnswerType').valid()) ) {
            
                $(this).text(CurrentContextInfo.ProgressText).parent().addClass('disabled');
                callWebMethod('UpdateQuestion', getQuestionParameters(), function (question) {
                    $('#PreviewArea').slideDown(AnimationSpeed);
                    
                    addQuestionPreview(question.QuestionId, $('#QuestionText').val(), $('#QuestionRequiredCheckBox').is(':checked'), parseInt($('#DefineAnswerType :selected').val(), 10), question.Answers);
                        
                    resetCreateQuestionSection();
                });
            }
        });

        // cancel question
        $('#CancelQuestion').click(function (event) {
            event.preventDefault();
            resetCreateQuestionSection();
        });

        function addQuestionPreview(questionId, questionText, isRequired, questionType, answers) {
            var $questionElement, questionOrder = $('#CreateQuestions').data('relativeOrder');
            if (questionOrder) {
                $questionElement = $('.ee-preview').eq(questionOrder - 1);
            }
            else {
                $questionElement = $('.ee-preview-template').clone(true).attr('class', 'ee-preview');
                
                // if this is the first question, just use the hidden element
                // otherwise, clone that element and replace its values
                ////if ($questionElement.data('questionId')) {
                    $('#ee-previews').append($questionElement);
                ////}
            }
            
            // update the new question preview
            $questionElement.find('.pv-question').text(questionText).show();
            $questionElement.find('.ee-required-label').text(isRequired ? '*' : '').show();
            $questionElement.show().data('questionId', questionId).data('questionType', questionType);
            
            // update the preview with answer values
            var $answerDiv = $questionElement.find('.pv-answer').empty();
            switch (questionType) {
            case 2:
                // ControlType.SmallTextInputField
                $answerDiv.html("<input type='text' class='NormalTextBox' />");
                break;
            case 1:
                // ControlType.LargeTextInputField
                $answerDiv.html("<textarea class='NormalTextBox' />");
                break;
            case 5:
                // ControlType.DropDownChoices
                var $dropDown = $("<select class='NormalTextBox dropdown-prev'></select>")
                $answerDiv.append($dropDown);
                $.each(answers, function (i, answer) {
                    $("<option>" + answer.Text + "</option>").appendTo($dropDown).data('answerId', answer.AnswerId);
                });
                break;
            case 3:
                // ControlType.VerticalOptionsButtons
                $.each(answers, function (i, answer) {
                    $("<label><input type='radio' name='" + questionId + "' />" + answer.Text + "</label>").appendTo($answerDiv).find('input').data('answerId', answer.AnswerId);
                });
                break;
            case 6:
                // ControlType.Checkbox
                $.each(answers, function (i, answer) {
                    $("<label><input type='checkbox' />" + answer.Text + "</label>").appendTo($answerDiv).find('input').data('answerId', answer.AnswerId);
                });
                break;
            default:
                alert("todo: implement validation, shouldn't be able to add a question if you have 'select answer type' selected in the drop down.");
            }
        }

        function resetCreateQuestionSection() {
            // reset the "create question" section
            $('#QuestionText').val('');
            $('#QuestionRequiredCheckBox').attr('checked', true);
            $('#DefineAnswerType').find('option:first').attr('selected', true);
            $('#ShortTextAnswer').slideUp(AnimationSpeed);
            $('#LongTextAnswer').slideUp(AnimationSpeed);
            $('#MultipleAnswer').slideUp(AnimationSpeed);
            $('#CancelQuestion').parent().fadeOut(AnimationSpeed);
            $('#AddNewQuestion').parent().fadeOut(AnimationSpeed);
            
            // remove all remove answers and related undo messages
            $('#MultipleAnswer li.answer-input.deleted')
                .add('.answer-inputs li.ee-undo:not(.template)')
                .remove();

            // only should have two answers by default
            $('#MultipleAnswer li.answer-input').remove();
            var $defaultAnswers = $('#MultipleAnswer li.answer-input-template');
            $defaultAnswers.clone(true).attr('class', 'answer-input').show().insertAfter($defaultAnswers).find('.answer-num').text(2);
            $defaultAnswers.clone(true).attr('class', 'answer-input').show().insertAfter($defaultAnswers).find('.answer-num').text(1);
            
            $('.ai-input input').val('');
            
            $('#SaveQuestion').text(CurrentContextInfo.SaveQuestionButtonText).attr('title', CurrentContextInfo.SaveQuestionToolTip).parent().addClass('disabled');
            
            // clear out stored data values
            $('#CreateQuestions').removeData('questionId').removeData('relativeOrder')
                .find('#MultipleAnswer li.answer-input').removeData('answerId');

            validator.resetForm();
        }
        
        function getQuestionParameters () {
            return {
                surveyId: $('.ee-create-new').data('surveyId') || -1,
                question: {
                    QuestionId: $('#CreateQuestions').data('questionId') || -1,
                    Text: $('#QuestionText').val(),
                    IsRequired: $('#QuestionRequiredCheckBox').is(':checked'),
                    RelativeOrder: $('#CreateQuestions').data('relativeOrder') || $('.ee-preview').length + 1,
                    ControlType: $('#DefineAnswerType').val(),
                    RevisingUser: CurrentContextInfo.UserId,
                    Answers: $.map($('#MultipleAnswer:visible .answer-inputs li.answer-input:visible'), function (elem, i) {
                        var $elem = $(elem);
                        return {
                            AnswerId: $elem.data('answerId') || -1,
                            Text: $elem.find(':input').val()
                        };
                    })
                }
            };
        }
        
        function parseDateString(dateValue) {
            if (dateValue) {
                return new Date(parseInt(dateValue.replace("/Date(", "").replace(")/", ""), 10));
            }
            
            return null;
        }

        (function initializeControls () {
            if (startDatePicker === null || endDatePicker === null) {
                setTimeout(function () {
                    startDatePicker = $find($('.ee-start-date .RadPicker input').attr('id')),
                    endDatePicker = $find($('.ee-end-date .RadPicker input').attr('id'));
                    initializeControls();
                }, 1);

                return;
            }

            if (CurrentContextInfo.Survey) {
                $('.ee-create-new').data('surveyId', CurrentContextInfo.Survey.SurveyId);
                $('#EvalTitleInput').val(CurrentContextInfo.Survey.Text);
                $('#EvalDescTextArea').val(CurrentContextInfo.Survey.Sections[0].Text);
                startDatePicker.set_selectedDate(parseDateString(CurrentContextInfo.Survey.StartDate));
                $('#EvalPreStartTextArea').val(CurrentContextInfo.Survey.PreStartMessage);
                endDatePicker.set_selectedDate(parseDateString(CurrentContextInfo.Survey.EndDate));
                $('#EvalPostEndTextArea').val(CurrentContextInfo.Survey.PostEndMessage);
			    $('#EvalSendNotification').attr('checked', CurrentContextInfo.Survey.SendNotification);
			    $('#EvalNotificationFromEmail').val(CurrentContextInfo.Survey.NotificationFromEmailAddress);
			    $('#EvalNotificationToEmails').val(CurrentContextInfo.Survey.NotificationToEmailAddresses);
			    $('#EvalSendThankYou').attr('checked', CurrentContextInfo.Survey.SendThankYou);
			    $('#EvalThankYouFromEmail').val(CurrentContextInfo.Survey.ThankYouFromEmailAddress);
            
                $('#EvalNew').parent().hide();
                makeSurveyReadOnly();
                hideEditModeButtons();
                $('.ee-create-questions').show(); 
            
                if (CurrentContextInfo.Survey.Sections[0].Questions.length) {
                    $('#PreviewArea').show();
                
                    $.each(CurrentContextInfo.Survey.Sections[0].Questions, function (i, question) {
                        addQuestionPreview(question.QuestionId, question.Text, question.IsRequired, question.ControlType, question.Answers);
                    });
                }
            }
            else {
                $('#EvalSendNotification').attr('checked', CurrentContextInfo.DefaultEmailSettings.SendNotification);
			    $('#EvalNotificationFromEmail').val(CurrentContextInfo.DefaultEmailSettings.NotificationFromEmail);
			    $('#EvalNotificationToEmails').val(CurrentContextInfo.DefaultEmailSettings.NotificationToEmails);
                $('#EvalSendThankYou').attr('checked', CurrentContextInfo.DefaultEmailSettings.SendThankYou);
			    $('#EvalThankYouFromEmail').val(CurrentContextInfo.DefaultEmailSettings.ThankYouFromEmail);
            }
            
            resetCreateQuestionSection();
        }());
    });
})(jQuery);
