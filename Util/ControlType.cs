// <copyright file="ControlType.cs" company="Engage Software">
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
    /// <summary>
    /// ControlType
    /// </summary>
    public class ControlType
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FinalMessageOption"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        public ControlType(string description)
        {
            this.Description = description;
        }

        /// <summary>
        /// LargeTextInputField
        /// </summary>
        public static ControlType LargeTextInputField = new ControlType("LargeTextInputField");

        /// <summary>
        /// SmallTextInputField
        /// </summary>
        public static ControlType SmallTextInputField = new ControlType("SmallTextInputField");

        /// <summary>
        /// VerticalOptionButtons
        /// </summary>
        public static ControlType VerticalOptionButtons = new ControlType("VerticalOptionButtons");

        /// <summary>
        /// HorizontalOptionButtons
        /// </summary>
        public static ControlType HorizontalOptionButtons = new ControlType("HorizontalOptionButtons");

        /// <summary>
        /// DropDownChoices
        /// </summary>
        public static ControlType DropDownChoices = new ControlType("DropDownChoices");

        /// <summary>
        /// Checkbox
        /// </summary>
        public static ControlType Checkbox = new ControlType("Checkbox");

        /// <summary>
        /// EmailInputField
        /// </summary>
        public static ControlType EmailInputField = new ControlType("EmailInputField");
        
    }
}