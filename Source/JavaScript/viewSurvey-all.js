/// <reference path="jquery-1.3.2.debug-vsdoc.js" />
/*globals jQuery, Page_ClientValidate */
(function ($) {
    'use strict';
    $.fn.engageSurveyView = function (options) {
        var $survey = this,
            opts = $.extend({}, $.fn.engageSurveyView.defaultOptions, options);
        return $survey.find('.submit-button').click(function () {
            var $button = $(this),
                disableButton = function () {
                    if ($button.prop) {
                        $button.prop('disabled', true);
                    } else {
                        $button.attr('disabled', 'disabled');
                    }
                };
            
            if (window.Page_ClientValidate && window.Page_ClientValidate(opts.validationGroup)) {
                setTimeout(disableButton, 25);
                $button.val(opts.submittingText);
                $survey.addClass(opts.submittingClass);
            }
        }).end();
    };

    $.fn.engageSurveyView.defaultOptions = {
        submittingText: 'Submitting...',
        submittingClass: 'submitting',
        validationGroup: ''
    };
}(jQuery));
