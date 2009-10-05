// <copyright file="ValidationProviderBase.cs" company="Engage Software">
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
    using System.Web.UI;

    /// <summary>
    /// Different types of controls that can be validated.
    /// </summary>
    public enum ValidationType
    {
        /// <summary>
        /// Phone Field
        /// </summary>
        PhoneField,

        /// <summary>
        /// Email Field
        /// </summary>
        EmailField,

        /// <summary>
        /// Zipcode Field
        /// </summary>
        ZipField,

        /// <summary>
        /// Required Field
        /// </summary>
        RequiredField,

        /// <summary>
        /// Limited Length
        /// </summary>
        LimitedLengthField,

        /// <summary>
        /// Limited number of selection (Composite question)
        /// </summary>
        LimitedSelection
    }

    /// <summary>
    ///  ValidationProviderBase class 
    /// </summary>
    public abstract class ValidationProviderBase 
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
        public abstract void RegisterValidator(ClientScriptManager manager, ValidationType validationType, string cssClass, Control containerControl, string controlToValidate, string errorMessage, string validationGroup, int selectionLimit);
    }
}