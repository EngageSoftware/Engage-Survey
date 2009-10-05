// <copyright file="TitleOptions.cs" company="Engage Software">
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
    /// TitleOptions
    /// </summary>
    public class TitleOptions
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
        /// Initializes a new instance of the <see cref="TitleOptions"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        public TitleOptions(string description) 
        {
            this.Description = description;
        }

        /// <summary>
        /// FirstPageOnly
        /// </summary>
        public static TitleOptions FirstPageOnly = new TitleOptions("FirstPageOnly");

        /// <summary>
        /// EveryPage
        /// </summary>
        public static TitleOptions EveryPage = new TitleOptions("EveryPage");
    }
}