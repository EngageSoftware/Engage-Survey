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
    /// <summary>
    /// FinalMessageOptions
    /// </summary>
    public class FinalMessageOptions
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
        /// Initializes a new instance of the <see cref="FinalMessageOptions"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        public FinalMessageOptions(string description)
        {
            this.Description = description;
        }

        /// <summary>
        /// UseFinalMessage
        /// </summary>
        public static FinalMessageOptions UseFinalMessage = new FinalMessageOptions("UseFinalMessage");

        /// <summary>
        /// UseFinalURL
        /// </summary>
        public static FinalMessageOptions UseFinalURL = new FinalMessageOptions("UseFinalURL");
    }
}