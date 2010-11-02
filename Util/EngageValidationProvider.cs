// <copyright file="EngageValidationProvider.cs" company="Engage Software">
// Engage: Survey
// Copyright (c) 2004-2010
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Survey.Util
{
    using System;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// EngageValidationProvider Class
    /// </summary>
    public class EngageValidationProvider : ValidationProviderBase
    {
        /// <summary>
        /// Injects the specified manager.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="validationType">Type of the validation.</param>
        /// <param name="cssClass">The CSS class.</param>
        /// <param name="containerControl">The container control.</param>
        /// <param name="controlToValidate">The control to validate.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="validationGroup">The validation group.</param>
        /// <param name="selectionLimit">The selection limit.</param>
        /// <param name="maxLength">Length of the max.</param>
        public override void RegisterValidator(ClientScriptManager manager, ValidationType validationType, string cssClass, Control containerControl, string controlToValidate, string errorMessage, string validationGroup, int selectionLimit, int maxLength)
        {
            if (validationType == ValidationType.RequiredField)
            {
                containerControl.Controls.Add(new RequiredFieldValidator
                    {
                        Display = ValidatorDisplay.Dynamic,
                        ControlToValidate = controlToValidate,
                        ValidationGroup = validationGroup,
                        ErrorMessage = "<span class=\"error-text\">" + errorMessage + "</span>",
                        CssClass = cssClass
                    });
            }

            if (validationType == ValidationType.EmailField)
            {
                containerControl.Controls.Add(new RegularExpressionValidator
                    {
                        Display = ValidatorDisplay.Dynamic,
                        ControlToValidate = controlToValidate,
                        ValidationGroup = validationGroup,
                        ValidationExpression = Engage.Utility.EmailsRegEx,
                        ErrorMessage = "<span class=\"error-text\">" + errorMessage + "</span>",
                        CssClass = cssClass
                    });
            }

            if (validationType == ValidationType.LimitedLengthField)
            {
                containerControl.Controls.Add(new TextBoxLengthValidator
                    {
                        Display = ValidatorDisplay.Dynamic,
                        ControlToValidate = controlToValidate,
                        ValidationGroup = validationGroup,
                        ErrorMessage = "<span class=\"error-text\">" + errorMessage + "</span>",
                        MaxLength = maxLength,
                        CssClass = cssClass
                    });
            }

            if (validationType == ValidationType.LimitedSelection)
            {
                if (selectionLimit > 0)
                {
                    var gnarlyScriptBuilder = new StringBuilder(128);
                    gnarlyScriptBuilder.Append("<script type=\"text/javascript\">");
                    gnarlyScriptBuilder.Append(Environment.NewLine);

                    gnarlyScriptBuilder.Append("var checkBoxesSelected = 0;");
                    gnarlyScriptBuilder.Append(Environment.NewLine);
                    gnarlyScriptBuilder.Append("var checkBoxLimit = ");
                    gnarlyScriptBuilder.Append(selectionLimit);
                    gnarlyScriptBuilder.Append(";");
                    gnarlyScriptBuilder.Append(Environment.NewLine);

                    gnarlyScriptBuilder.Append("function CheckSelectedCount(val)");
                    gnarlyScriptBuilder.Append(Environment.NewLine);
                    gnarlyScriptBuilder.Append("{");
                    gnarlyScriptBuilder.Append(Environment.NewLine);
                    gnarlyScriptBuilder.Append("			    jQuery('.limit-reached').hide(); ");
                    gnarlyScriptBuilder.Append(Environment.NewLine);
                    gnarlyScriptBuilder.Append("		var allowCheck = true;");
                    gnarlyScriptBuilder.Append(Environment.NewLine);
                    gnarlyScriptBuilder.Append("		if (val.checked)");
                    gnarlyScriptBuilder.Append(Environment.NewLine);
                    gnarlyScriptBuilder.Append("		{");
                    gnarlyScriptBuilder.Append(Environment.NewLine);
                    gnarlyScriptBuilder.Append("			if ((checkBoxesSelected + 1) <= checkBoxLimit)");
                    gnarlyScriptBuilder.Append(Environment.NewLine);
                    gnarlyScriptBuilder.Append("			{");
                    gnarlyScriptBuilder.Append(Environment.NewLine);
                    gnarlyScriptBuilder.Append("				checkBoxesSelected++;");
                    gnarlyScriptBuilder.Append(Environment.NewLine);
                    gnarlyScriptBuilder.Append("			}");
                    gnarlyScriptBuilder.Append(Environment.NewLine);
                    gnarlyScriptBuilder.Append("			else");
                    gnarlyScriptBuilder.Append(Environment.NewLine);
                    gnarlyScriptBuilder.Append("			{");
                    gnarlyScriptBuilder.Append(Environment.NewLine);
                    gnarlyScriptBuilder.Append("			    jQuery('.limit-reached').show(); ");
                    gnarlyScriptBuilder.Append(Environment.NewLine);
                    gnarlyScriptBuilder.Append("				allowCheck = false;");
                    gnarlyScriptBuilder.Append(Environment.NewLine);
                    gnarlyScriptBuilder.Append("			}");
                    gnarlyScriptBuilder.Append(Environment.NewLine);
                    gnarlyScriptBuilder.Append("		}");
                    gnarlyScriptBuilder.Append(Environment.NewLine);
                    gnarlyScriptBuilder.Append("		else ");
                    gnarlyScriptBuilder.Append(Environment.NewLine);
                    gnarlyScriptBuilder.Append("		{");
                    gnarlyScriptBuilder.Append(Environment.NewLine);
                    gnarlyScriptBuilder.Append("			checkBoxesSelected--;");
                    gnarlyScriptBuilder.Append(Environment.NewLine);
                    gnarlyScriptBuilder.Append("		}");
                    gnarlyScriptBuilder.Append(Environment.NewLine);
                    gnarlyScriptBuilder.Append("		return allowCheck;");
                    gnarlyScriptBuilder.Append(Environment.NewLine);
                    gnarlyScriptBuilder.Append("}");
                    gnarlyScriptBuilder.Append(Environment.NewLine);

                    gnarlyScriptBuilder.Append("</script>");
                    gnarlyScriptBuilder.Append(Environment.NewLine);

                    if (!manager.IsClientScriptBlockRegistered("CheckBoxLimitCheck"))
                    {
                        manager.RegisterClientScriptBlock(this.GetType(), "CheckBoxLimitCheck", gnarlyScriptBuilder.ToString());
                    }
                }
            }
        }
    }
}