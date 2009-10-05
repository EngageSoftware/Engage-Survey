// <copyright file="TextBoxLengthValidator.cs" company="Engage Software">
// Engage: ContactUs - http://www.engagesoftware.com
// Copyright (c) 2004-2009
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Engage.Survey.Util
{
    /// <summary>
    /// Summary description for TextBoxLengthValidator.
    /// </summary>
    [ToolboxData("<{0}:TextBoxLengthValidator runat=server ErrorMessage=\"Characters entered exceeds max allowed\"></{0}:TextBoxLengthValidator>")]
    public class TextBoxLengthValidator : BaseCompareValidator
    {
        /// <summary>
        /// Specifies the max length of the TextBox the control is validating. If this value
        /// is 0, then an input of any length is considered valid
        /// </summary>
        [Bindable(true), Description("The maximum number of characters that can be entered"), Category("Behavior"), DefaultValue(850)]
        public int MaxLength
        {
            get
            {
                object length = this.ViewState["MaxLength"];
                return (length == null ? 0 : System.Convert.ToInt32(length));
            }

            set
            {
                this.ViewState["MaxLength"] = value;
            }
        }

        #region Overrides

        /// <summary>
        /// Adds client-side functionality for uplevel browsers by specifying the JavaScript function
        /// to call when validating, as well as the needed parameter, MaxLength
        /// </summary>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            if (this.EnableClientScript)
            {
                writer.AddAttribute("evaluationfunction", "TextBoxLengthIsValid");
                writer.AddAttribute("maxlength", this.MaxLength.ToString());
            }
        }

        /// <summary>
        /// Checks to ensure that the ControlToValidate property is set to a TextBox
        /// </summary>
        protected override bool ControlPropertiesValid()
        {
            if (this.Context == null)
            {
                //only do this is running in the designer
                if (!(this.FindControl(this.GetControlRenderID(this.ControlToValidate)) is TextBox))
                    throw new HttpException("ControlToValidate must be a TextBox.");
            }

            return base.ControlPropertiesValid();
        }

        /// <summary>
        /// Performs the server-side validation.  If MaxLength is 0, always returns true;
        /// otherwise, returns true only if the ControlToValidate's length is less than or equal to the
        /// specified MaxLength
        /// </summary>
        protected override bool EvaluateIsValid()
        {
            if (this.MaxLength == 0) return true;

            string controlValue = this.GetControlValidationValue(this.ControlToValidate);

            return controlValue.Length <= this.MaxLength;
        }

        /// <summary>
        /// Injects the JavaScript function that performs client-side validation for uplevel browsers.
        /// </summary>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (this.EnableClientScript)
            {
                StringBuilder sb = new StringBuilder(128);
                sb.Append("<script language='javascript'>");
                sb.Append("function TextBoxLengthIsValid(val)");
                sb.Append("{");
                sb.Append("var value = ValidatorGetValue(val.controltovalidate);");
                sb.Append("if (ValidatorTrim(value).length == 0) return true;");
                sb.Append("if (val.maxlength == 0) return true;");
                sb.Append("return (value.length <= val.maxlength);");
                sb.Append("}");
                sb.Append("</script>");

                this.Page.RegisterClientScriptBlock("TxtBxLngthValIsValid", sb.ToString());
            }
        }

        #endregion
    }
}