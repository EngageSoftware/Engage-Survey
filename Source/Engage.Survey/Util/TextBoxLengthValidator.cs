// <copyright file="TextBoxLengthValidator.cs" company="Engage Software">
// Engage: Survey
// Copyright (c) 2004-2014
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Survey.Util
{
    using System.ComponentModel;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Validates that the length of a <see cref="TextBox"/>'s value is no longer than a maximum length
    /// </summary>
    [ToolboxData("<{0}:TextBoxLengthValidator runat=server ErrorMessage=\"Characters entered exceeds max allowed\"></{0}:TextBoxLengthValidator>")]
    public sealed class TextBoxLengthValidator : BaseCompareValidator
    {
        /// <summary>
        /// Gets or sets the max length of the TextBox the control is validating. If this value
        /// is 0, then an input of any length is considered valid
        /// </summary>
        [Bindable(true), Description("The maximum number of characters that can be entered"), Category("Behavior"), DefaultValue(850)]
        public int MaxLength
        {
            get
            {
                object length = this.ViewState["MaxLength"];
                return length == null ? 0 : System.Convert.ToInt32(length);
            }

            set
            {
                this.ViewState["MaxLength"] = value;
            }
        }

        #region Overrides

        /// <summary>
        /// Checks to ensure that the ControlToValidate property is set to a TextBox
        /// </summary>
        /// <returns>
        /// <c>true</c> if the control specified by <see cref="BaseValidator.ControlToValidate"/> is a valid control; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.Web.HttpException">
        /// No value is specified for the <see cref="BaseValidator.ControlToValidate"/> property.
        /// - or -
        /// The input control specified by the <see cref="BaseValidator.ControlToValidate"/> property is not found on the page.
        /// - or -
        /// The input control specified by the <see cref="BaseValidator.ControlToValidate"/> property does not have a <see cref="ValidationPropertyAttribute"/> attribute associated with it; therefore, it cannot be validated with a validation control.
        /// </exception>
        protected override bool ControlPropertiesValid()
        {
            if (this.Context == null)
            {
                // only do this if running in the designer
                if (!(this.FindControl(this.GetControlRenderID(this.ControlToValidate)) is TextBox))
                {
                    throw new HttpException("ControlToValidate must be a TextBox.");
                }
            }

            return base.ControlPropertiesValid();
        }

        /// <summary>
        /// Performs the server-side validation.  If MaxLength is 0, always returns true;
        /// otherwise, returns true only if the ControlToValidate's length is less than or equal to the
        /// specified MaxLength
        /// </summary>
        /// <returns>
        /// <c>true</c> if the value in the input control is valid; otherwise, <c>false</c>.
        /// </returns>
        protected override bool EvaluateIsValid()
        {
            if (this.MaxLength == 0)
            {
                return true;
            }

            string controlValue = this.GetControlValidationValue(this.ControlToValidate);

            return controlValue.Length <= this.MaxLength;
        }

        #endregion
    }
}