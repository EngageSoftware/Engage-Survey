// <copyright file="ISurveyElement.cs" company="Engage Software">
// Engage: ContactUs - http://www.engagesoftware.com
// Copyright (c) 2004-2009
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Survey
{
    /// <summary>
    /// Summary description for ISurveyElement.
    /// </summary>
    public interface ISurveyElement
    {
        /// <summary>
        /// Returns the formatting for the element plus the the unformatted text together. Used primarily by
        /// the Web and Windows viewers only.
        /// </summary>
        string FormattedText {get;}

        /// <summary>
        /// Returns only the text value of Text attribute for the survey element.
        /// </summary>
        string UnformattedText {get;}

        /// <summary>
        /// Returns the formatting that will be used to prefix the unformatted text for the survey element.
        /// </summary>
        string Formatting{get;}
        
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The Text.</value>
        string Text { get; set; }
    }
}