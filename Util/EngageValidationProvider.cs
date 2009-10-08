// <copyright file="EngageValidationProvider.cs" company="Engage Software">
// Engage: ContactUs - http://www.engagesoftware.com
// Copyright (c) 2004-2009
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
                RequiredFieldValidator validator = new RequiredFieldValidator
                                                       {
                                                               Display = ValidatorDisplay.Dynamic,
                                                               ControlToValidate = controlToValidate,
                                                               ValidationGroup = validationGroup,
                                                               ErrorMessage = ("<span class=\"error-text\">" + errorMessage + "</span>"),
                                                               CssClass = cssClass
                                                       };
                containerControl.Controls.Add(validator);
            }

            if (validationType == ValidationType.EmailField)
            {
                RegularExpressionValidator validator = new RegularExpressionValidator
                                                           {
                                                                   Display = ValidatorDisplay.Dynamic,
                                                                   ControlToValidate = controlToValidate,
                                                                   ValidationGroup = validationGroup,
                                                                   ValidationExpression = Engage.Utility.EmailsRegEx,
                                                                   ErrorMessage = ("<span class=\"error-text\">" + errorMessage + "</span>"),
                                                                   CssClass = cssClass
                                                           };
                containerControl.Controls.Add(validator);
            }

            if (validationType == ValidationType.LimitedLengthField)
            {
                TextBoxLengthValidator tbv = new TextBoxLengthValidator
                                                 {
                                                         Display = ValidatorDisplay.Dynamic,
                                                         ControlToValidate = controlToValidate,
                                                         ValidationGroup = validationGroup,
                                                         ErrorMessage = ("<span class=\"error-text\">" + errorMessage + "</span>"),
                                                         MaxLength = maxLength
                                                 };
                containerControl.Controls.Add(tbv);
            }

            if (validationType == ValidationType.LimitedSelection)
            {
                if (selectionLimit > 0)
                {
                    StringBuilder sb = new StringBuilder(128);
                    sb.Append("<script language='javascript'>");
                    sb.Append(Environment.NewLine);

                    sb.Append("var checkBoxesSelected = 0;");
                    sb.Append(Environment.NewLine);
                    sb.Append("var checkBoxLimit = ");
                    sb.Append(selectionLimit);
                    sb.Append(";");
                    sb.Append(Environment.NewLine);

                    sb.Append("function CheckSelectedCount(val)");
                    sb.Append(Environment.NewLine);
                    sb.Append("{");
                    sb.Append(Environment.NewLine);
                    sb.Append("			    jQuery('.limit-reached').hide(); ");
                    sb.Append(Environment.NewLine);
                    sb.Append("		var allowCheck = true;");
                    sb.Append(Environment.NewLine);
                    sb.Append("		if (val.checked)");
                    sb.Append(Environment.NewLine);
                    sb.Append("		{");
                    sb.Append(Environment.NewLine);
                    sb.Append("			if ((checkBoxesSelected + 1) <= checkBoxLimit)");
                    sb.Append(Environment.NewLine);
                    sb.Append("			{");
                    sb.Append(Environment.NewLine);
                    sb.Append("				checkBoxesSelected++;");
                    sb.Append(Environment.NewLine);
                    sb.Append("			}");
                    sb.Append(Environment.NewLine);
                    sb.Append("			else");
                    sb.Append(Environment.NewLine);
                    sb.Append("			{");
                    sb.Append(Environment.NewLine);
                    sb.Append("			    jQuery('.limit-reached').show(); ");
                    sb.Append(Environment.NewLine);
                    sb.Append("				allowCheck = false;");
                    sb.Append(Environment.NewLine);
                    sb.Append("			}");
                    sb.Append(Environment.NewLine);
                    sb.Append("		}");
                    sb.Append(Environment.NewLine);
                    sb.Append("		else ");
                    sb.Append(Environment.NewLine);
                    sb.Append("		{");
                    sb.Append(Environment.NewLine);
                    sb.Append("			checkBoxesSelected--;");
                    sb.Append(Environment.NewLine);
                    sb.Append("		}");
                    sb.Append(Environment.NewLine);
                    sb.Append("		return allowCheck;");
                    sb.Append(Environment.NewLine);
                    sb.Append("}");
                    sb.Append(Environment.NewLine);

                    sb.Append("</script>");
                    sb.Append(Environment.NewLine);

                    if (!manager.IsClientScriptBlockRegistered("CheckBoxLimitCheck"))
                    {
                        manager.RegisterClientScriptBlock(this.GetType(), "CheckBoxLimitCheck", sb.ToString());
                    }
                }
            }
        }
    }
}