/*
    http://www.JSON.org/json2.js
    2011-02-23

    Public Domain.

    NO WARRANTY EXPRESSED OR IMPLIED. USE AT YOUR OWN RISK.

    See http://www.JSON.org/js.html


    This code should be minified before deployment.
    See http://javascript.crockford.com/jsmin.html

    USE YOUR OWN COPY. IT IS EXTREMELY UNWISE TO LOAD CODE FROM SERVERS YOU DO
    NOT CONTROL.


    This file creates a global JSON object containing two methods: stringify
    and parse.

        JSON.stringify(value, replacer, space)
            value       any JavaScript value, usually an object or array.

            replacer    an optional parameter that determines how object
                        values are stringified for objects. It can be a
                        function or an array of strings.

            space       an optional parameter that specifies the indentation
                        of nested structures. If it is omitted, the text will
                        be packed without extra whitespace. If it is a number,
                        it will specify the number of spaces to indent at each
                        level. If it is a string (such as '\t' or '&nbsp;'),
                        it contains the characters used to indent at each level.

            This method produces a JSON text from a JavaScript value.

            When an object value is found, if the object contains a toJSON
            method, its toJSON method will be called and the result will be
            stringified. A toJSON method does not serialize: it returns the
            value represented by the name/value pair that should be serialized,
            or undefined if nothing should be serialized. The toJSON method
            will be passed the key associated with the value, and this will be
            bound to the value

            For example, this would serialize Dates as ISO strings.

                Date.prototype.toJSON = function (key) {
                    function f(n) {
                        // Format integers to have at least two digits.
                        return n < 10 ? '0' + n : n;
                    }

                    return this.getUTCFullYear()   + '-' +
                         f(this.getUTCMonth() + 1) + '-' +
                         f(this.getUTCDate())      + 'T' +
                         f(this.getUTCHours())     + ':' +
                         f(this.getUTCMinutes())   + ':' +
                         f(this.getUTCSeconds())   + 'Z';
                };

            You can provide an optional replacer method. It will be passed the
            key and value of each member, with this bound to the containing
            object. The value that is returned from your method will be
            serialized. If your method returns undefined, then the member will
            be excluded from the serialization.

            If the replacer parameter is an array of strings, then it will be
            used to select the members to be serialized. It filters the results
            such that only members with keys listed in the replacer array are
            stringified.

            Values that do not have JSON representations, such as undefined or
            functions, will not be serialized. Such values in objects will be
            dropped; in arrays they will be replaced with null. You can use
            a replacer function to replace those with JSON values.
            JSON.stringify(undefined) returns undefined.

            The optional space parameter produces a stringification of the
            value that is filled with line breaks and indentation to make it
            easier to read.

            If the space parameter is a non-empty string, then that string will
            be used for indentation. If the space parameter is a number, then
            the indentation will be that many spaces.

            Example:

            text = JSON.stringify(['e', {pluribus: 'unum'}]);
            // text is '["e",{"pluribus":"unum"}]'


            text = JSON.stringify(['e', {pluribus: 'unum'}], null, '\t');
            // text is '[\n\t"e",\n\t{\n\t\t"pluribus": "unum"\n\t}\n]'

            text = JSON.stringify([new Date()], function (key, value) {
                return this[key] instanceof Date ?
                    'Date(' + this[key] + ')' : value;
            });
            // text is '["Date(---current time---)"]'


        JSON.parse(text, reviver)
            This method parses a JSON text to produce an object or array.
            It can throw a SyntaxError exception.

            The optional reviver parameter is a function that can filter and
            transform the results. It receives each of the keys and values,
            and its return value is used instead of the original value.
            If it returns what it received, then the structure is not modified.
            If it returns undefined then the member is deleted.

            Example:

            // Parse the text. Values that look like ISO date strings will
            // be converted to Date objects.

            myData = JSON.parse(text, function (key, value) {
                var a;
                if (typeof value === 'string') {
                    a =
/^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)Z$/.exec(value);
                    if (a) {
                        return new Date(Date.UTC(+a[1], +a[2] - 1, +a[3], +a[4],
                            +a[5], +a[6]));
                    }
                }
                return value;
            });

            myData = JSON.parse('["Date(09/09/2001)"]', function (key, value) {
                var d;
                if (typeof value === 'string' &&
                        value.slice(0, 5) === 'Date(' &&
                        value.slice(-1) === ')') {
                    d = new Date(value.slice(5, -1));
                    if (d) {
                        return d;
                    }
                }
                return value;
            });


    This is a reference implementation. You are free to copy, modify, or
    redistribute.
*/

/*jslint evil: true, strict: false, regexp: false */

/*members "", "\b", "\t", "\n", "\f", "\r", "\"", JSON, "\\", apply,
    call, charCodeAt, getUTCDate, getUTCFullYear, getUTCHours,
    getUTCMinutes, getUTCMonth, getUTCSeconds, hasOwnProperty, join,
    lastIndex, length, parse, prototype, push, replace, slice, stringify,
    test, toJSON, toString, valueOf
*/


// Create a JSON object only if one does not already exist. We create the
// methods in a closure to avoid creating global variables.

var JSON;
if (!JSON) {
    JSON = {};
}

(function () {
    "use strict";

    function f(n) {
        // Format integers to have at least two digits.
        return n < 10 ? '0' + n : n;
    }

    if (typeof Date.prototype.toJSON !== 'function') {

        Date.prototype.toJSON = function (key) {

            return isFinite(this.valueOf()) ?
                this.getUTCFullYear()     + '-' +
                f(this.getUTCMonth() + 1) + '-' +
                f(this.getUTCDate())      + 'T' +
                f(this.getUTCHours())     + ':' +
                f(this.getUTCMinutes())   + ':' +
                f(this.getUTCSeconds())   + 'Z' : null;
        };

        String.prototype.toJSON      =
            Number.prototype.toJSON  =
            Boolean.prototype.toJSON = function (key) {
                return this.valueOf();
            };
    }

    var cx = /[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,
        escapable = /[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,
        gap,
        indent,
        meta = {    // table of character substitutions
            '\b': '\\b',
            '\t': '\\t',
            '\n': '\\n',
            '\f': '\\f',
            '\r': '\\r',
            '"' : '\\"',
            '\\': '\\\\'
        },
        rep;


    function quote(string) {

// If the string contains no control characters, no quote characters, and no
// backslash characters, then we can safely slap some quotes around it.
// Otherwise we must also replace the offending characters with safe escape
// sequences.

        escapable.lastIndex = 0;
        return escapable.test(string) ? '"' + string.replace(escapable, function (a) {
            var c = meta[a];
            return typeof c === 'string' ? c :
                '\\u' + ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
        }) + '"' : '"' + string + '"';
    }


    function str(key, holder) {

// Produce a string from holder[key].

        var i,          // The loop counter.
            k,          // The member key.
            v,          // The member value.
            length,
            mind = gap,
            partial,
            value = holder[key];

// If the value has a toJSON method, call it to obtain a replacement value.

        if (value && typeof value === 'object' &&
                typeof value.toJSON === 'function') {
            value = value.toJSON(key);
        }

// If we were called with a replacer function, then call the replacer to
// obtain a replacement value.

        if (typeof rep === 'function') {
            value = rep.call(holder, key, value);
        }

// What happens next depends on the value's type.

        switch (typeof value) {
        case 'string':
            return quote(value);

        case 'number':

// JSON numbers must be finite. Encode non-finite numbers as null.

            return isFinite(value) ? String(value) : 'null';

        case 'boolean':
        case 'null':

// If the value is a boolean or null, convert it to a string. Note:
// typeof null does not produce 'null'. The case is included here in
// the remote chance that this gets fixed someday.

            return String(value);

// If the type is 'object', we might be dealing with an object or an array or
// null.

        case 'object':

// Due to a specification blunder in ECMAScript, typeof null is 'object',
// so watch out for that case.

            if (!value) {
                return 'null';
            }

// Make an array to hold the partial results of stringifying this object value.

            gap += indent;
            partial = [];

// Is the value an array?

            if (Object.prototype.toString.apply(value) === '[object Array]') {

// The value is an array. Stringify every element. Use null as a placeholder
// for non-JSON values.

                length = value.length;
                for (i = 0; i < length; i += 1) {
                    partial[i] = str(i, value) || 'null';
                }

// Join all of the elements together, separated with commas, and wrap them in
// brackets.

                v = partial.length === 0 ? '[]' : gap ?
                    '[\n' + gap + partial.join(',\n' + gap) + '\n' + mind + ']' :
                    '[' + partial.join(',') + ']';
                gap = mind;
                return v;
            }

// If the replacer is an array, use it to select the members to be stringified.

            if (rep && typeof rep === 'object') {
                length = rep.length;
                for (i = 0; i < length; i += 1) {
                    if (typeof rep[i] === 'string') {
                        k = rep[i];
                        v = str(k, value);
                        if (v) {
                            partial.push(quote(k) + (gap ? ': ' : ':') + v);
                        }
                    }
                }
            } else {

// Otherwise, iterate through all of the keys in the object.

                for (k in value) {
                    if (Object.prototype.hasOwnProperty.call(value, k)) {
                        v = str(k, value);
                        if (v) {
                            partial.push(quote(k) + (gap ? ': ' : ':') + v);
                        }
                    }
                }
            }

// Join all of the member texts together, separated with commas,
// and wrap them in braces.

            v = partial.length === 0 ? '{}' : gap ?
                '{\n' + gap + partial.join(',\n' + gap) + '\n' + mind + '}' :
                '{' + partial.join(',') + '}';
            gap = mind;
            return v;
        }
    }

// If the JSON object does not yet have a stringify method, give it one.

    if (typeof JSON.stringify !== 'function') {
        JSON.stringify = function (value, replacer, space) {

// The stringify method takes a value and an optional replacer, and an optional
// space parameter, and returns a JSON text. The replacer can be a function
// that can replace values, or an array of strings that will select the keys.
// A default replacer method can be provided. Use of the space parameter can
// produce text that is more easily readable.

            var i;
            gap = '';
            indent = '';

// If the space parameter is a number, make an indent string containing that
// many spaces.

            if (typeof space === 'number') {
                for (i = 0; i < space; i += 1) {
                    indent += ' ';
                }

// If the space parameter is a string, it will be used as the indent string.

            } else if (typeof space === 'string') {
                indent = space;
            }

// If there is a replacer, it must be a function or an array.
// Otherwise, throw an error.

            rep = replacer;
            if (replacer && typeof replacer !== 'function' &&
                    (typeof replacer !== 'object' ||
                    typeof replacer.length !== 'number')) {
                throw new Error('JSON.stringify');
            }

// Make a fake root object containing our value under the key of ''.
// Return the result of stringifying the value.

            return str('', {'': value});
        };
    }


// If the JSON object does not yet have a parse method, give it one.

    if (typeof JSON.parse !== 'function') {
        JSON.parse = function (text, reviver) {

// The parse method takes a text and an optional reviver function, and returns
// a JavaScript value if the text is a valid JSON text.

            var j;

            function walk(holder, key) {

// The walk method is used to recursively walk the resulting structure so
// that modifications can be made.

                var k, v, value = holder[key];
                if (value && typeof value === 'object') {
                    for (k in value) {
                        if (Object.prototype.hasOwnProperty.call(value, k)) {
                            v = walk(value, k);
                            if (v !== undefined) {
                                value[k] = v;
                            } else {
                                delete value[k];
                            }
                        }
                    }
                }
                return reviver.call(holder, key, value);
            }


// Parsing happens in four stages. In the first stage, we replace certain
// Unicode characters with escape sequences. JavaScript handles many characters
// incorrectly, either silently deleting them, or treating them as line endings.

            text = String(text);
            cx.lastIndex = 0;
            if (cx.test(text)) {
                text = text.replace(cx, function (a) {
                    return '\\u' +
                        ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
                });
            }

// In the second stage, we run the text against regular expressions that look
// for non-JSON patterns. We are especially concerned with '()' and 'new'
// because they can cause invocation, and '=' because it can cause mutation.
// But just to be safe, we want to reject all unexpected forms.

// We split the second stage into 4 regexp operations in order to work around
// crippling inefficiencies in IE's and Safari's regexp engines. First we
// replace the JSON backslash pairs with '@' (a non-JSON character). Second, we
// replace all simple value tokens with ']' characters. Third, we delete all
// open brackets that follow a colon or comma or that begin the text. Finally,
// we look to see that the remaining characters are only whitespace or ']' or
// ',' or ':' or '{' or '}'. If that is so, then the text is safe for eval.

            if (/^[\],:{}\s]*$/
                    .test(text.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, '@')
                        .replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, ']')
                        .replace(/(?:^|:|,)(?:\s*\[)+/g, ''))) {

// In the third stage we use the eval function to compile the text into a
// JavaScript structure. The '{' operator is subject to a syntactic ambiguity
// in JavaScript: it can begin a block or an object literal. We wrap the text
// in parens to eliminate the ambiguity.

                j = eval('(' + text + ')');

// In the optional fourth stage, we recursively walk the new structure, passing
// each name/value pair to a reviver function for possible transformation.

                return typeof reviver === 'function' ?
                    walk({'': j}, '') : j;
            }

// If the text is not JSON parseable, then a SyntaxError is thrown.

            throw new SyntaxError('JSON.parse');
        };
    }
}());

/*!
 * from https://developer.mozilla.org/en/Core_JavaScript_1.5_Reference/Global_Objects/Array/indexOf
 * implements FireFox/SpiderMonkey algorithm for the Array's indexOf method in browsers that don't natively support it
 */
if (!Array.prototype.indexOf)
{
  Array.prototype.indexOf = function(searchElement /*, fromIndex */)
  {
    "use strict";

    if (this === void 0 || this === null)
      throw new TypeError();

    var t = Object(this);
    var len = t.length >>> 0;
    if (len === 0)
      return -1;

    var n = 0;
    if (arguments.length > 0)
    {
      n = Number(arguments[1]);
      if (n !== n) // shortcut for verifying if it's NaN
        n = 0;
      else if (n !== 0 && n !== (1 / 0) && n !== -(1 / 0))
        n = (n > 0 || -1) * Math.floor(Math.abs(n));
    }

    if (n >= len)
      return -1;

    var k = n >= 0
          ? n
          : Math.max(len - Math.abs(n), 0);

    for (; k < len; k++)
    {
      if (k in t && t[k] === searchElement)
        return k;
    }
    return -1;
  };
}
/*!
 * jQuery Validation Plugin 1.8.1
 *
 * http://bassistance.de/jquery-plugins/jquery-plugin-validation/
 * http://docs.jquery.com/Plugins/Validation
 *
 * Copyright (c) 2006 - 2011 Jörn Zaefferer
 *
 * Dual licensed under the MIT and GPL licenses:
 *   http://www.opensource.org/licenses/mit-license.php
 *   http://www.gnu.org/licenses/gpl.html
 */

(function($) {

$.extend($.fn, {
	// http://docs.jquery.com/Plugins/Validation/validate
	validate: function( options ) {

		// if nothing is selected, return nothing; can't chain anyway
		if (!this.length) {
			options && options.debug && window.console && console.warn( "nothing selected, can't validate, returning nothing" );
			return;
		}

		// check if a validator for this form was already created
		var validator = $.data(this[0], 'validator');
		if ( validator ) {
			return validator;
		}

		validator = new $.validator( options, this[0] );
		$.data(this[0], 'validator', validator);

		if ( validator.settings.onsubmit ) {

			// allow suppresing validation by adding a cancel class to the submit button
			this.find("input, button").filter(".cancel").click(function() {
				validator.cancelSubmit = true;
			});

			// when a submitHandler is used, capture the submitting button
			if (validator.settings.submitHandler) {
				this.find("input, button").filter(":submit").click(function() {
					validator.submitButton = this;
				});
			}

			// validate the form on submit
			this.submit( function( event ) {
				if ( validator.settings.debug )
					// prevent form submit to be able to see console output
					event.preventDefault();

				function handle() {
					if ( validator.settings.submitHandler ) {
						if (validator.submitButton) {
							// insert a hidden input as a replacement for the missing submit button
							var hidden = $("<input type='hidden'/>").attr("name", validator.submitButton.name).val(validator.submitButton.value).appendTo(validator.currentForm);
						}
						validator.settings.submitHandler.call( validator, validator.currentForm );
						if (validator.submitButton) {
							// and clean up afterwards; thanks to no-block-scope, hidden can be referenced
							hidden.remove();
						}
						return false;
					}
					return true;
				}

				// prevent submit for invalid forms or custom submit handlers
				if ( validator.cancelSubmit ) {
					validator.cancelSubmit = false;
					return handle();
				}
				if ( validator.form() ) {
					if ( validator.pendingRequest ) {
						validator.formSubmitted = true;
						return false;
					}
					return handle();
				} else {
					validator.focusInvalid();
					return false;
				}
			});
		}

		return validator;
	},
	// http://docs.jquery.com/Plugins/Validation/valid
	valid: function() {
        if ( $(this[0]).is('form')) {
            return this.validate().form();
        } else {
            var valid = true;
            var validator = $(this[0].form).validate();
            this.each(function() {
				valid &= validator.element(this);
            });
            return valid;
        }
    },
	// attributes: space seperated list of attributes to retrieve and remove
	removeAttrs: function(attributes) {
		var result = {},
			$element = this;
		$.each(attributes.split(/\s/), function(index, value) {
			result[value] = $element.attr(value);
			$element.removeAttr(value);
		});
		return result;
	},
	// http://docs.jquery.com/Plugins/Validation/rules
	rules: function(command, argument) {
		var element = this[0];

		if (command) {
			var settings = $.data(element.form, 'validator').settings;
			var staticRules = settings.rules;
			var existingRules = $.validator.staticRules(element);
			switch(command) {
			case "add":
				$.extend(existingRules, $.validator.normalizeRule(argument));
				staticRules[element.name] = existingRules;
				if (argument.messages)
					settings.messages[element.name] = $.extend( settings.messages[element.name], argument.messages );
				break;
			case "remove":
				if (!argument) {
					delete staticRules[element.name];
					return existingRules;
				}
				var filtered = {};
				$.each(argument.split(/\s/), function(index, method) {
					filtered[method] = existingRules[method];
					delete existingRules[method];
				});
				return filtered;
			}
		}

		var data = $.validator.normalizeRules(
		$.extend(
			{},
			$.validator.metadataRules(element),
			$.validator.classRules(element),
			$.validator.attributeRules(element),
			$.validator.staticRules(element)
		), element);

		// make sure required is at front
		if (data.required) {
			var param = data.required;
			delete data.required;
			data = $.extend({required: param}, data);
		}

		return data;
	}
});

// Custom selectors
$.extend($.expr[":"], {
	// http://docs.jquery.com/Plugins/Validation/blank
	blank: function(a) {return !$.trim("" + a.value);},
	// http://docs.jquery.com/Plugins/Validation/filled
	filled: function(a) {return !!$.trim("" + a.value);},
	// http://docs.jquery.com/Plugins/Validation/unchecked
	unchecked: function(a) {return !a.checked;}
});

// constructor for validator
$.validator = function( options, form ) {
	this.settings = $.extend( true, {}, $.validator.defaults, options );
	this.currentForm = form;
	this.init();
};

$.validator.format = function(source, params) {
	if ( arguments.length == 1 )
		return function() {
			var args = $.makeArray(arguments);
			args.unshift(source);
			return $.validator.format.apply( this, args );
		};
	if ( arguments.length > 2 && params.constructor != Array  ) {
		params = $.makeArray(arguments).slice(1);
	}
	if ( params.constructor != Array ) {
		params = [ params ];
	}
	$.each(params, function(i, n) {
		source = source.replace(new RegExp("\\{" + i + "\\}", "g"), n);
	});
	return source;
};

$.extend($.validator, {

	defaults: {
		messages: {},
		groups: {},
		rules: {},
		errorClass: "error",
		validClass: "valid",
		errorElement: "label",
		focusInvalid: true,
		errorContainer: $( [] ),
		errorLabelContainer: $( [] ),
		onsubmit: true,
		ignore: [],
		ignoreTitle: false,
		onfocusin: function(element) {
			this.lastActive = element;

			// hide error label and remove error class on focus if enabled
			if ( this.settings.focusCleanup && !this.blockFocusCleanup ) {
				this.settings.unhighlight && this.settings.unhighlight.call( this, element, this.settings.errorClass, this.settings.validClass );
				this.addWrapper(this.errorsFor(element)).hide();
			}
		},
		onfocusout: function(element) {
			if ( !this.checkable(element) && (element.name in this.submitted || !this.optional(element)) ) {
				this.element(element);
			}
		},
		onkeyup: function(element) {
			if ( element.name in this.submitted || element == this.lastElement ) {
				this.element(element);
			}
		},
		onclick: function(element) {
			// click on selects, radiobuttons and checkboxes
			if ( element.name in this.submitted )
				this.element(element);
			// or option elements, check parent select in that case
			else if (element.parentNode.name in this.submitted)
				this.element(element.parentNode);
		},
		highlight: function(element, errorClass, validClass) {
			if (element.type === 'radio') {
				this.findByName(element.name).addClass(errorClass).removeClass(validClass);
			} else {
				$(element).addClass(errorClass).removeClass(validClass);
			}
		},
		unhighlight: function(element, errorClass, validClass) {
			if (element.type === 'radio') {
				this.findByName(element.name).removeClass(errorClass).addClass(validClass);
			} else {
				$(element).removeClass(errorClass).addClass(validClass);
			}
		}
	},

	// http://docs.jquery.com/Plugins/Validation/Validator/setDefaults
	setDefaults: function(settings) {
		$.extend( $.validator.defaults, settings );
	},

	messages: {
		required: "This field is required.",
		remote: "Please fix this field.",
		email: "Please enter a valid email address.",
		url: "Please enter a valid URL.",
		date: "Please enter a valid date.",
		dateISO: "Please enter a valid date (ISO).",
		number: "Please enter a valid number.",
		digits: "Please enter only digits.",
		creditcard: "Please enter a valid credit card number.",
		equalTo: "Please enter the same value again.",
		accept: "Please enter a value with a valid extension.",
		maxlength: $.validator.format("Please enter no more than {0} characters."),
		minlength: $.validator.format("Please enter at least {0} characters."),
		rangelength: $.validator.format("Please enter a value between {0} and {1} characters long."),
		range: $.validator.format("Please enter a value between {0} and {1}."),
		max: $.validator.format("Please enter a value less than or equal to {0}."),
		min: $.validator.format("Please enter a value greater than or equal to {0}.")
	},

	autoCreateRanges: false,

	prototype: {

		init: function() {
			this.labelContainer = $(this.settings.errorLabelContainer);
			this.errorContext = this.labelContainer.length && this.labelContainer || $(this.currentForm);
			this.containers = $(this.settings.errorContainer).add( this.settings.errorLabelContainer );
			this.submitted = {};
			this.valueCache = {};
			this.pendingRequest = 0;
			this.pending = {};
			this.invalid = {};
			this.reset();

			var groups = (this.groups = {});
			$.each(this.settings.groups, function(key, value) {
				$.each(value.split(/\s/), function(index, name) {
					groups[name] = key;
				});
			});
			var rules = this.settings.rules;
			$.each(rules, function(key, value) {
				rules[key] = $.validator.normalizeRule(value);
			});

			function delegate(event) {
				var validator = $.data(this[0].form, "validator"),
					eventType = "on" + event.type.replace(/^validate/, "");
				validator.settings[eventType] && validator.settings[eventType].call(validator, this[0] );
			}
			$(this.currentForm)
				.validateDelegate(":text, :password, :file, select, textarea", "focusin focusout keyup", delegate)
				.validateDelegate(":radio, :checkbox, select, option", "click", delegate);

			if (this.settings.invalidHandler)
				$(this.currentForm).bind("invalid-form.validate", this.settings.invalidHandler);
		},

		// http://docs.jquery.com/Plugins/Validation/Validator/form
		form: function() {
			this.checkForm();
			$.extend(this.submitted, this.errorMap);
			this.invalid = $.extend({}, this.errorMap);
			if (!this.valid())
				$(this.currentForm).triggerHandler("invalid-form", [this]);
			this.showErrors();
			return this.valid();
		},

		checkForm: function() {
			this.prepareForm();
			for ( var i = 0, elements = (this.currentElements = this.elements()); elements[i]; i++ ) {
				this.check( elements[i] );
			}
			return this.valid();
		},

		// http://docs.jquery.com/Plugins/Validation/Validator/element
		element: function( element ) {
			element = this.clean( element );
			this.lastElement = element;
			this.prepareElement( element );
			this.currentElements = $(element);
			var result = this.check( element );
			if ( result ) {
				delete this.invalid[element.name];
			} else {
				this.invalid[element.name] = true;
			}
			if ( !this.numberOfInvalids() ) {
				// Hide error containers on last error
				this.toHide = this.toHide.add( this.containers );
			}
			this.showErrors();
			return result;
		},

		// http://docs.jquery.com/Plugins/Validation/Validator/showErrors
		showErrors: function(errors) {
			if(errors) {
				// add items to error list and map
				$.extend( this.errorMap, errors );
				this.errorList = [];
				for ( var name in errors ) {
					this.errorList.push({
						message: errors[name],
						element: this.findByName(name)[0]
					});
				}
				// remove items from success list
				this.successList = $.grep( this.successList, function(element) {
					return !(element.name in errors);
				});
			}
			this.settings.showErrors
				? this.settings.showErrors.call( this, this.errorMap, this.errorList )
				: this.defaultShowErrors();
		},

		// http://docs.jquery.com/Plugins/Validation/Validator/resetForm
		resetForm: function() {
			if ( $.fn.resetForm )
				$( this.currentForm ).resetForm();
			this.submitted = {};
			this.prepareForm();
			this.hideErrors();
			this.elements().removeClass( this.settings.errorClass );
		},

		numberOfInvalids: function() {
			return this.objectLength(this.invalid);
		},

		objectLength: function( obj ) {
			var count = 0;
			for ( var i in obj )
				count++;
			return count;
		},

		hideErrors: function() {
			this.addWrapper( this.toHide ).hide();
		},

		valid: function() {
			return this.size() == 0;
		},

		size: function() {
			return this.errorList.length;
		},

		focusInvalid: function() {
			if( this.settings.focusInvalid ) {
				try {
					$(this.findLastActive() || this.errorList.length && this.errorList[0].element || [])
					.filter(":visible")
					.focus()
					// manually trigger focusin event; without it, focusin handler isn't called, findLastActive won't have anything to find
					.trigger("focusin");
				} catch(e) {
					// ignore IE throwing errors when focusing hidden elements
				}
			}
		},

		findLastActive: function() {
			var lastActive = this.lastActive;
			return lastActive && $.grep(this.errorList, function(n) {
				return n.element.name == lastActive.name;
			}).length == 1 && lastActive;
		},

		elements: function() {
			var validator = this,
				rulesCache = {};

			// select all valid inputs inside the form (no submit or reset buttons)
			return $(this.currentForm)
			.find("input, select, textarea")
			.not(":submit, :reset, :image, [disabled]")
			.not( this.settings.ignore )
			.filter(function() {
				!this.name && validator.settings.debug && window.console && console.error( "%o has no name assigned", this);

				// select only the first element for each name, and only those with rules specified
				if ( this.name in rulesCache || !validator.objectLength($(this).rules()) )
					return false;

				rulesCache[this.name] = true;
				return true;
			});
		},

		clean: function( selector ) {
			return $( selector )[0];
		},

		errors: function() {
			return $( this.settings.errorElement + "." + this.settings.errorClass, this.errorContext );
		},

		reset: function() {
			this.successList = [];
			this.errorList = [];
			this.errorMap = {};
			this.toShow = $([]);
			this.toHide = $([]);
			this.currentElements = $([]);
		},

		prepareForm: function() {
			this.reset();
			this.toHide = this.errors().add( this.containers );
		},

		prepareElement: function( element ) {
			this.reset();
			this.toHide = this.errorsFor(element);
		},

		check: function( element ) {
			element = this.clean( element );

			// if radio/checkbox, validate first element in group instead
			if (this.checkable(element)) {
				element = this.findByName( element.name ).not(this.settings.ignore)[0];
			}

			var rules = $(element).rules();
			var dependencyMismatch = false;
			for (var method in rules ) {
				var rule = { method: method, parameters: rules[method] };
				try {
					var result = $.validator.methods[method].call( this, element.value.replace(/\r/g, ""), element, rule.parameters );

					// if a method indicates that the field is optional and therefore valid,
					// don't mark it as valid when there are no other rules
					if ( result == "dependency-mismatch" ) {
						dependencyMismatch = true;
						continue;
					}
					dependencyMismatch = false;

					if ( result == "pending" ) {
						this.toHide = this.toHide.not( this.errorsFor(element) );
						return;
					}

					if( !result ) {
						this.formatAndAdd( element, rule );
						return false;
					}
				} catch(e) {
					this.settings.debug && window.console && console.log("exception occured when checking element " + element.id
						 + ", check the '" + rule.method + "' method", e);
					throw e;
				}
			}
			if (dependencyMismatch)
				return;
			if ( this.objectLength(rules) )
				this.successList.push(element);
			return true;
		},

		// return the custom message for the given element and validation method
		// specified in the element's "messages" metadata
		customMetaMessage: function(element, method) {
			if (!$.metadata)
				return;

			var meta = this.settings.meta
				? $(element).metadata()[this.settings.meta]
				: $(element).metadata();

			return meta && meta.messages && meta.messages[method];
		},

		// return the custom message for the given element name and validation method
		customMessage: function( name, method ) {
			var m = this.settings.messages[name];
			return m && (m.constructor == String
				? m
				: m[method]);
		},

		// return the first defined argument, allowing empty strings
		findDefined: function() {
			for(var i = 0; i < arguments.length; i++) {
				if (arguments[i] !== undefined)
					return arguments[i];
			}
			return undefined;
		},

		defaultMessage: function( element, method) {
			return this.findDefined(
				this.customMessage( element.name, method ),
				this.customMetaMessage( element, method ),
				// title is never undefined, so handle empty string as undefined
				!this.settings.ignoreTitle && element.title || undefined,
				$.validator.messages[method],
				"<strong>Warning: No message defined for " + element.name + "</strong>"
			);
		},

		formatAndAdd: function( element, rule ) {
			var message = this.defaultMessage( element, rule.method ),
				theregex = /\$?\{(\d+)\}/g;
			if ( typeof message == "function" ) {
				message = message.call(this, rule.parameters, element);
			} else if (theregex.test(message)) {
				message = jQuery.format(message.replace(theregex, '{$1}'), rule.parameters);
			}
			this.errorList.push({
				message: message,
				element: element
			});

			this.errorMap[element.name] = message;
			this.submitted[element.name] = message;
		},

		addWrapper: function(toToggle) {
			if ( this.settings.wrapper )
				toToggle = toToggle.add( toToggle.parent( this.settings.wrapper ) );
			return toToggle;
		},

		defaultShowErrors: function() {
			for ( var i = 0; this.errorList[i]; i++ ) {
				var error = this.errorList[i];
				this.settings.highlight && this.settings.highlight.call( this, error.element, this.settings.errorClass, this.settings.validClass );
				this.showLabel( error.element, error.message );
			}
			if( this.errorList.length ) {
				this.toShow = this.toShow.add( this.containers );
			}
			if (this.settings.success) {
				for ( var i = 0; this.successList[i]; i++ ) {
					this.showLabel( this.successList[i] );
				}
			}
			if (this.settings.unhighlight) {
				for ( var i = 0, elements = this.validElements(); elements[i]; i++ ) {
					this.settings.unhighlight.call( this, elements[i], this.settings.errorClass, this.settings.validClass );
				}
			}
			this.toHide = this.toHide.not( this.toShow );
			this.hideErrors();
			this.addWrapper( this.toShow ).show();
		},

		validElements: function() {
			return this.currentElements.not(this.invalidElements());
		},

		invalidElements: function() {
			return $(this.errorList).map(function() {
				return this.element;
			});
		},

		showLabel: function(element, message) {
			var label = this.errorsFor( element );
			if ( label.length ) {
				// refresh error/success class
				label.removeClass().addClass( this.settings.errorClass );

				// check if we have a generated label, replace the message then
				label.attr("generated") && label.html(message);
			} else {
				// create label
				label = $("<" + this.settings.errorElement + "/>")
					.attr({"for":  this.idOrName(element), generated: true})
					.addClass(this.settings.errorClass)
					.html(message || "");
				if ( this.settings.wrapper ) {
					// make sure the element is visible, even in IE
					// actually showing the wrapped element is handled elsewhere
					label = label.hide().show().wrap("<" + this.settings.wrapper + "/>").parent();
				}
				if ( !this.labelContainer.append(label).length )
					this.settings.errorPlacement
						? this.settings.errorPlacement(label, $(element) )
						: label.insertAfter(element);
			}
			if ( !message && this.settings.success ) {
				label.text("");
				typeof this.settings.success == "string"
					? label.addClass( this.settings.success )
					: this.settings.success( label );
			}
			this.toShow = this.toShow.add(label);
		},

		errorsFor: function(element) {
			var name = this.idOrName(element);
    		return this.errors().filter(function() {
				return $(this).attr('for') == name;
			});
		},

		idOrName: function(element) {
			return this.groups[element.name] || (this.checkable(element) ? element.name : element.id || element.name);
		},

		checkable: function( element ) {
			return /radio|checkbox/i.test(element.type);
		},

		findByName: function( name ) {
			// select by name and filter by form for performance over form.find("[name=...]")
			var form = this.currentForm;
			return $(document.getElementsByName(name)).map(function(index, element) {
				return element.form == form && element.name == name && element  || null;
			});
		},

		getLength: function(value, element) {
			switch( element.nodeName.toLowerCase() ) {
			case 'select':
				return $("option:selected", element).length;
			case 'input':
				if( this.checkable( element) )
					return this.findByName(element.name).filter(':checked').length;
			}
			return value.length;
		},

		depend: function(param, element) {
			return this.dependTypes[typeof param]
				? this.dependTypes[typeof param](param, element)
				: true;
		},

		dependTypes: {
			"boolean": function(param, element) {
				return param;
			},
			"string": function(param, element) {
				return !!$(param, element.form).length;
			},
			"function": function(param, element) {
				return param(element);
			}
		},

		optional: function(element) {
			return !$.validator.methods.required.call(this, $.trim(element.value), element) && "dependency-mismatch";
		},

		startRequest: function(element) {
			if (!this.pending[element.name]) {
				this.pendingRequest++;
				this.pending[element.name] = true;
			}
		},

		stopRequest: function(element, valid) {
			this.pendingRequest--;
			// sometimes synchronization fails, make sure pendingRequest is never < 0
			if (this.pendingRequest < 0)
				this.pendingRequest = 0;
			delete this.pending[element.name];
			if ( valid && this.pendingRequest == 0 && this.formSubmitted && this.form() ) {
				$(this.currentForm).submit();
				this.formSubmitted = false;
			} else if (!valid && this.pendingRequest == 0 && this.formSubmitted) {
				$(this.currentForm).triggerHandler("invalid-form", [this]);
				this.formSubmitted = false;
			}
		},

		previousValue: function(element) {
			return $.data(element, "previousValue") || $.data(element, "previousValue", {
				old: null,
				valid: true,
				message: this.defaultMessage( element, "remote" )
			});
		}

	},

	classRuleSettings: {
		required: {required: true},
		email: {email: true},
		url: {url: true},
		date: {date: true},
		dateISO: {dateISO: true},
		dateDE: {dateDE: true},
		number: {number: true},
		numberDE: {numberDE: true},
		digits: {digits: true},
		creditcard: {creditcard: true}
	},

	addClassRules: function(className, rules) {
		className.constructor == String ?
			this.classRuleSettings[className] = rules :
			$.extend(this.classRuleSettings, className);
	},

	classRules: function(element) {
		var rules = {};
		var classes = $(element).attr('class');
		classes && $.each(classes.split(' '), function() {
			if (this in $.validator.classRuleSettings) {
				$.extend(rules, $.validator.classRuleSettings[this]);
			}
		});
		return rules;
	},

	attributeRules: function(element) {
		var rules = {};
		var $element = $(element);

		for (var method in $.validator.methods) {
			var value = $element.attr(method);
			if (value) {
				rules[method] = value;
			}
		}

		// maxlength may be returned as -1, 2147483647 (IE) and 524288 (safari) for text inputs
		if (rules.maxlength && /-1|2147483647|524288/.test(rules.maxlength)) {
			delete rules.maxlength;
		}

		return rules;
	},

	metadataRules: function(element) {
		if (!$.metadata) return {};

		var meta = $.data(element.form, 'validator').settings.meta;
		return meta ?
			$(element).metadata()[meta] :
			$(element).metadata();
	},

	staticRules: function(element) {
		var rules = {};
		var validator = $.data(element.form, 'validator');
		if (validator.settings.rules) {
			rules = $.validator.normalizeRule(validator.settings.rules[element.name]) || {};
		}
		return rules;
	},

	normalizeRules: function(rules, element) {
		// handle dependency check
		$.each(rules, function(prop, val) {
			// ignore rule when param is explicitly false, eg. required:false
			if (val === false) {
				delete rules[prop];
				return;
			}
			if (val.param || val.depends) {
				var keepRule = true;
				switch (typeof val.depends) {
					case "string":
						keepRule = !!$(val.depends, element.form).length;
						break;
					case "function":
						keepRule = val.depends.call(element, element);
						break;
				}
				if (keepRule) {
					rules[prop] = val.param !== undefined ? val.param : true;
				} else {
					delete rules[prop];
				}
			}
		});

		// evaluate parameters
		$.each(rules, function(rule, parameter) {
			rules[rule] = $.isFunction(parameter) ? parameter(element) : parameter;
		});

		// clean number parameters
		$.each(['minlength', 'maxlength', 'min', 'max'], function() {
			if (rules[this]) {
				rules[this] = Number(rules[this]);
			}
		});
		$.each(['rangelength', 'range'], function() {
			if (rules[this]) {
				rules[this] = [Number(rules[this][0]), Number(rules[this][1])];
			}
		});

		if ($.validator.autoCreateRanges) {
			// auto-create ranges
			if (rules.min && rules.max) {
				rules.range = [rules.min, rules.max];
				delete rules.min;
				delete rules.max;
			}
			if (rules.minlength && rules.maxlength) {
				rules.rangelength = [rules.minlength, rules.maxlength];
				delete rules.minlength;
				delete rules.maxlength;
			}
		}

		// To support custom messages in metadata ignore rule methods titled "messages"
		if (rules.messages) {
			delete rules.messages;
		}

		return rules;
	},

	// Converts a simple string to a {string: true} rule, e.g., "required" to {required:true}
	normalizeRule: function(data) {
		if( typeof data == "string" ) {
			var transformed = {};
			$.each(data.split(/\s/), function() {
				transformed[this] = true;
			});
			data = transformed;
		}
		return data;
	},

	// http://docs.jquery.com/Plugins/Validation/Validator/addMethod
	addMethod: function(name, method, message) {
		$.validator.methods[name] = method;
		$.validator.messages[name] = message != undefined ? message : $.validator.messages[name];
		if (method.length < 3) {
			$.validator.addClassRules(name, $.validator.normalizeRule(name));
		}
	},

	methods: {

		// http://docs.jquery.com/Plugins/Validation/Methods/required
		required: function(value, element, param) {
			// check if dependency is met
			if ( !this.depend(param, element) )
				return "dependency-mismatch";
			switch( element.nodeName.toLowerCase() ) {
			case 'select':
				// could be an array for select-multiple or a string, both are fine this way
				var val = $(element).val();
				return val && val.length > 0;
			case 'input':
				if ( this.checkable(element) )
					return this.getLength(value, element) > 0;
			default:
				return $.trim(value).length > 0;
			}
		},

		// http://docs.jquery.com/Plugins/Validation/Methods/remote
		remote: function(value, element, param) {
			if ( this.optional(element) )
				return "dependency-mismatch";

			var previous = this.previousValue(element);
			if (!this.settings.messages[element.name] )
				this.settings.messages[element.name] = {};
			previous.originalMessage = this.settings.messages[element.name].remote;
			this.settings.messages[element.name].remote = previous.message;

			param = typeof param == "string" && {url:param} || param;

			if ( this.pending[element.name] ) {
				return "pending";
			}
			if ( previous.old === value ) {
				return previous.valid;
			}

			previous.old = value;
			var validator = this;
			this.startRequest(element);
			var data = {};
			data[element.name] = value;
			$.ajax($.extend(true, {
				url: param,
				mode: "abort",
				port: "validate" + element.name,
				dataType: "json",
				data: data,
				success: function(response) {
					validator.settings.messages[element.name].remote = previous.originalMessage;
					var valid = response === true;
					if ( valid ) {
						var submitted = validator.formSubmitted;
						validator.prepareElement(element);
						validator.formSubmitted = submitted;
						validator.successList.push(element);
						validator.showErrors();
					} else {
						var errors = {};
						var message = response || validator.defaultMessage( element, "remote" );
						errors[element.name] = previous.message = $.isFunction(message) ? message(value) : message;
						validator.showErrors(errors);
					}
					previous.valid = valid;
					validator.stopRequest(element, valid);
				}
			}, param));
			return "pending";
		},

		// http://docs.jquery.com/Plugins/Validation/Methods/minlength
		minlength: function(value, element, param) {
			return this.optional(element) || this.getLength($.trim(value), element) >= param;
		},

		// http://docs.jquery.com/Plugins/Validation/Methods/maxlength
		maxlength: function(value, element, param) {
			return this.optional(element) || this.getLength($.trim(value), element) <= param;
		},

		// http://docs.jquery.com/Plugins/Validation/Methods/rangelength
		rangelength: function(value, element, param) {
			var length = this.getLength($.trim(value), element);
			return this.optional(element) || ( length >= param[0] && length <= param[1] );
		},

		// http://docs.jquery.com/Plugins/Validation/Methods/min
		min: function( value, element, param ) {
			return this.optional(element) || value >= param;
		},

		// http://docs.jquery.com/Plugins/Validation/Methods/max
		max: function( value, element, param ) {
			return this.optional(element) || value <= param;
		},

		// http://docs.jquery.com/Plugins/Validation/Methods/range
		range: function( value, element, param ) {
			return this.optional(element) || ( value >= param[0] && value <= param[1] );
		},

		// http://docs.jquery.com/Plugins/Validation/Methods/email
		email: function(value, element) {
			// contributed by Scott Gonzalez: http://projects.scottsplayground.com/email_address_validation/
			return this.optional(element) || /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i.test(value);
		},

		// http://docs.jquery.com/Plugins/Validation/Methods/url
		url: function(value, element) {
			// contributed by Scott Gonzalez: http://projects.scottsplayground.com/iri/
			return this.optional(element) || /^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i.test(value);
		},

		// http://docs.jquery.com/Plugins/Validation/Methods/date
		date: function(value, element) {
			return this.optional(element) || !/Invalid|NaN/.test(new Date(value));
		},

		// http://docs.jquery.com/Plugins/Validation/Methods/dateISO
		dateISO: function(value, element) {
			return this.optional(element) || /^\d{4}[\/-]\d{1,2}[\/-]\d{1,2}$/.test(value);
		},

		// http://docs.jquery.com/Plugins/Validation/Methods/number
		number: function(value, element) {
			return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:,\d{3})+)(?:\.\d+)?$/.test(value);
		},

		// http://docs.jquery.com/Plugins/Validation/Methods/digits
		digits: function(value, element) {
			return this.optional(element) || /^\d+$/.test(value);
		},

		// http://docs.jquery.com/Plugins/Validation/Methods/creditcard
		// based on http://en.wikipedia.org/wiki/Luhn
		creditcard: function(value, element) {
			if ( this.optional(element) )
				return "dependency-mismatch";
			// accept only digits and dashes
			if (/[^0-9-]+/.test(value))
				return false;
			var nCheck = 0,
				nDigit = 0,
				bEven = false;

			value = value.replace(/\D/g, "");

			for (var n = value.length - 1; n >= 0; n--) {
				var cDigit = value.charAt(n);
				var nDigit = parseInt(cDigit, 10);
				if (bEven) {
					if ((nDigit *= 2) > 9)
						nDigit -= 9;
				}
				nCheck += nDigit;
				bEven = !bEven;
			}

			return (nCheck % 10) == 0;
		},

		// http://docs.jquery.com/Plugins/Validation/Methods/accept
		accept: function(value, element, param) {
			param = typeof param == "string" ? param.replace(/,/g, '|') : "png|jpe?g|gif";
			return this.optional(element) || value.match(new RegExp(".(" + param + ")$", "i"));
		},

		// http://docs.jquery.com/Plugins/Validation/Methods/equalTo
		equalTo: function(value, element, param) {
			// bind to the blur event of the target in order to revalidate whenever the target field is updated
			// TODO find a way to bind the event just once, avoiding the unbind-rebind overhead
			var target = $(param).unbind(".validate-equalTo").bind("blur.validate-equalTo", function() {
				$(element).valid();
			});
			return value == target.val();
		}

	}

});

// deprecated, use $.validator.format instead
$.format = $.validator.format;

})(jQuery);

// ajax mode: abort
// usage: $.ajax({ mode: "abort"[, port: "uniqueport"]});
// if mode:"abort" is used, the previous request on that port (port can be undefined) is aborted via XMLHttpRequest.abort()
;(function($) {
	var pendingRequests = {};
	// Use a prefilter if available (1.5+)
	if ( $.ajaxPrefilter ) {
		$.ajaxPrefilter(function(settings, _, xhr) {
			var port = settings.port;
			if (settings.mode == "abort") {
				if ( pendingRequests[port] ) {
					pendingRequests[port].abort();
				}
				pendingRequests[port] = xhr;
			}
		});
	} else {
		// Proxy ajax
		var ajax = $.ajax;
		$.ajax = function(settings) {
			var mode = ( "mode" in settings ? settings : $.ajaxSettings ).mode,
				port = ( "port" in settings ? settings : $.ajaxSettings ).port;
			if (mode == "abort") {
				if ( pendingRequests[port] ) {
					pendingRequests[port].abort();
				}
				return (pendingRequests[port] = ajax.apply(this, arguments));
			}
			return ajax.apply(this, arguments);
		};
	}
})(jQuery);

// provides cross-browser focusin and focusout events
// IE has native support, in other browsers, use event caputuring (neither bubbles)

// provides delegate(type: String, delegate: Selector, handler: Callback) plugin for easier event delegation
// handler is only called when $(event.target).is(delegate), in the scope of the jquery-object for event.target
;(function($) {
	// only implement if not provided by jQuery core (since 1.4)
	// TODO verify if jQuery 1.4's implementation is compatible with older jQuery special-event APIs
	if (!jQuery.event.special.focusin && !jQuery.event.special.focusout && document.addEventListener) {
		$.each({
			focus: 'focusin',
			blur: 'focusout'
		}, function( original, fix ){
			$.event.special[fix] = {
				setup:function() {
					this.addEventListener( original, handler, true );
				},
				teardown:function() {
					this.removeEventListener( original, handler, true );
				},
				handler: function(e) {
					arguments[0] = $.event.fix(e);
					arguments[0].type = fix;
					return $.event.handle.apply(this, arguments);
				}
			};
			function handler(e) {
				e = $.event.fix(e);
				e.type = fix;
				return $.event.handle.call(this, e);
			}
		});
	};
	$.extend($.fn, {
		validateDelegate: function(delegate, type, handler) {
			return this.bind(type, function(event) {
				var target = $(event.target);
				if (target.is(delegate)) {
					return handler.apply(target, arguments);
				}
			});
		}
	});
})(jQuery);

/// <reference path="jquery-1.3.2.debug-vsdoc.js" />
/// <reference path="json2.js" />
/// <reference path="jquery-ui-1.8.14.js" />
/// <reference path="jquery.validate-1.8.1.js" />
/// <reference path="Array.prototype.indexOf.js" />
/*jslint maxlen: 150, browser: true */
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
                                }
                            )
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
            $questionTextArea.val($questionLi.children('.pv-question').html());
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
                    $answerElement.find('input').val($(this).html() || $(this).siblings('.pv-answer-option').html());

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
            $questionElement.find('.pv-question').html(questionText).show();
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
                    $("<option/>").html(answer.Text).appendTo($dropDown).data('answerId', answer.AnswerId);
                });
                break;
            case answerType.verticalRadioButtons:
                $.each(answers, function (i, answer) {
                    $("<label/>")
                        .prepend($("<input/>").attr('type', 'radio').attr('name', questionId).data('answerId', answer.AnswerId))
                        .prepend($("<span/>").attr('class', 'pv-answer-option').html(answer.Text))
                        .appendTo($answerDiv);
                });
                break;
            case answerType.checkBox:
                $.each(answers, function (i, answer) {
                    $("<label/>")
                        .prepend($("<input/>").attr('type', 'checkbox').data('answerId', answer.AnswerId))
                        .prepend($("<span/>").attr('class', 'pv-answer-option').html(answer.Text))
                        .appendTo($answerDiv);
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
            var $element = $('<div/>').html(value),
                $breaks = $element.find('br');

            // until jQuery 1.5.2, replaceWith fails on empty set
            if ($breaks.length) {
                $breaks.replaceWith('\n');
            }

            return $element.text();
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
