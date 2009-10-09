// <copyright file="ElementFormatOptions.cs" company="Engage Software">
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
    using Engage.Util;

    /// <summary>
    /// ElementFormatOptions
    /// </summary>
    public class ElementFormatOptions : EngageType
    {
        ///// <summary>
        ///// Gets or sets the description.
        ///// </summary>
        ///// <value>The description.</value>
        //public string Description
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementFormatOptions"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        public ElementFormatOptions(string description): base(0, description)
        {
            //this.Description = description;
        }

        /// <summary>
        /// None 
        /// </summary>
        public static ElementFormatOptions None = new ElementFormatOptions("None");

        /// <summary>
        /// Numbered
        /// </summary>
        public static ElementFormatOptions Numbered = new ElementFormatOptions("Numbered");

        /// <summary>
        /// Lettered
        /// </summary>
        public static ElementFormatOptions Lettered = new ElementFormatOptions("Lettered");

        /// <summary>
        /// Roman
        /// </summary>
        public static ElementFormatOptions Roman = new ElementFormatOptions("Roman");
		
    }
}