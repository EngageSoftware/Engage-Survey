// <copyright file="IAnswer.cs" company="Engage Software">
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
    using Util;

    /// <summary>
    /// IAnswer Interface (Marker)
    /// </summary>
    public interface IAnswer : ISurveyElement
    {
        /// <summary>
        /// Gets or sets the answer id.
        /// </summary>
        /// <value>The answer id.</value>
        int AnswerId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the question id.
        /// </summary>
        /// <value>The question id.</value>
        int QuestionId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is correct.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is correct; otherwise, <c>false</c>.
        /// </value>
        bool IsCorrect
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the rendering key used by the SurveyControl to uniquely identify this element.
        /// </summary>
        /// <value>The rendering key.</value>
        Key RelationshipKey { get; }

        /// <summary>
        /// Gets or sets the relative order.
        /// </summary>
        /// <value>The relative order.</value>
        int RelativeOrder { get; set; }
    }
}