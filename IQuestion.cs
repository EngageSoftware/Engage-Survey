// <copyright file="IQuestion.cs" company="Engage Software">
// Engage: Survey
// Copyright (c) 2004-2009
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.;

namespace Engage.Survey
{
    using System.Collections.Generic;
    using Util;

    /// <summary>
    /// IQuestion Class
    /// </summary>
    public interface IQuestion : ISurveyElement
    {
        /// <summary>
        /// Gets the section.
        /// </summary>
        /// <value>The section.</value>
        ISection GetSection();
        
        /// <summary>
        /// Gets the answer choices.
        /// </summary>
        /// <value>The answer choices.</value>
        List<IAnswer> GetAnswers();

        /// <summary>
        /// Gets a value indicating whether this instance is boolean.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is boolean; otherwise, <c>false</c>.
        /// </value>
        bool IsBoolean{ get;}

        /// <summary>
        /// Gets the rendering key used by the SurveyControl to uniquely identify this element.
        /// </summary>
        /// <value>The rendering key.</value>
        Key RelationshipKey { get; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        string Comments { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is required.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is required; otherwise, <c>false</c>.
        /// </value>
        bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the question id.
        /// </summary>
        /// <value>The question id.</value>
        int QuestionId { get; set; }

        /// <summary>
        /// Gets or sets the control type.
        /// </summary>
        /// <value>The control type.</value>
        ControlType ControlType { get; set; }

        /// <summary>
        /// Gets or sets the selection limit.
        /// </summary>
        /// <value>The selection limit.</value>
        int SelectionLimit { get; set; }

        /// <summary>
        /// Gets or sets the answer value.
        /// </summary>
        /// <value>The answer value.</value>
        List<UserResponse> Responses { get; set; }

        /// <summary>
        /// Gets or sets the relative order.
        /// </summary>
        /// <value>The relative order.</value>
        int RelativeOrder { get; set; }

        /// <summary>
        /// Gets the answer choice.
        /// </summary>
        /// <param name="key">The key.</param>
        IAnswer GetAnswers(Key key);

        /// <summary>
        /// Finds the response.
        /// </summary>
        /// <param name="answer">The answer.</param>
        /// <returns></returns>
        UserResponse FindResponse(IAnswer answer);
    }
}