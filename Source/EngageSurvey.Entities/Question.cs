// <copyright file="Question.cs" company="Engage Software">
// Engage: ContactUs - http://www.engagesoftware.com
// Copyright (c) 2004-2009
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Survey.Entities
{
    using System.Collections.Generic;
    using Engage.Util;
    using Util;

    /// <summary>
    /// Summary description for Question.
    /// </summary>
    public partial class Question : IQuestion
    {
        /// <summary>
        /// Gets the rendering key used by the SurveyControl to uniquely identify this element.
        /// </summary>
        /// <value>The rendering key.</value>
        public Key RelationshipKey
        {
            get
            {
                return new Key { SectionId = this.SectionId, QuestionId = this.QuestionId };
            }
        }

        /// <summary>
        /// Gets or sets the answer value.
        /// </summary>
        /// <value>The answer value.</value>
        public List<UserResponse> Responses
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the answer choice.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public IAnswer GetAnswers(Key key)
        {
            foreach (IAnswer answer in this.GetAnswers())
            {
                if (answer.AnswerId == key.AnswerId)
                {
                    return answer;
                }
            }
            return null;
        }

        /// <summary>
        /// Finds the response.
        /// </summary>
        /// <param name="answer">The answer.</param>
        /// <returns></returns>
        public UserResponse FindResponse(IAnswer answer)
        {
            foreach (UserResponse r in Responses)
            {
                if (r.RelationshipKey.Equals(answer.RelationshipKey))
                {
                    return r;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets or sets the comments for a Question. Questions can optionally have a "Allow Comments" on each of them in a future phase.
        /// </summary>
        /// <value>The comments.</value>
        public string Comments
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the section.
        /// </summary>
        /// <returns></returns>
        /// <value>The section.</value>
        public ISection GetSection()
        {
            return this.Section; 
        }

        /// <summary>
        /// Gets the answer choices.
        /// </summary>
        /// <returns></returns>
        /// <value>The answer choices.</value>
        public List<IAnswer> GetAnswers()
        {
            List<IAnswer> answers = new List<IAnswer>();

            foreach (Answer a in Answers)
            {
                answers.Add(a);
            }

            answers.Sort(new Answer.RelativeOrderComparer());
            return answers;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is boolean.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is boolean; otherwise, <c>false</c>.
        /// </value>
        public bool IsBoolean
        {
            get
            {
                return false;
            }
        }
        
        /// <summary>
        /// Returns the formatting for the element plus the the unformatted text together. Used primarily by
        /// the Web and Windows viewers only.
        /// </summary>
        /// <value></value>
        public string FormattedText
        {
            get {return this.Formatting + this.UnformattedText;}
        }

        /// <summary>
        /// Returns only the text value of Text attribute for the survey element.
        /// </summary>
        /// <value></value>
        public string UnformattedText
        {
            get { return this.Text; }
        }

        /// <summary>
        /// Returns the formatting that will be used to prefix the unformatted text for the survey element.
        /// </summary>
        /// <value></value>
        public string Formatting
        {
            get
            {
                ISurvey survey = this.Section.Survey;
                if (survey != null)
                {
                    return Util.Utility.PrependFormatting(survey.QuestionFormatOption, this.RelativeOrder);
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// RelativeOrderComparer
        /// </summary>
        internal class RelativeOrderComparer : IComparer<IQuestion>
        {
            private readonly bool descending = true;

            public RelativeOrderComparer()
            {
            }

            public RelativeOrderComparer(bool descending)
            {
                this.descending = descending;
            }

            #region IComparer Members

            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <returns>
            /// Value 
            ///                     Condition 
            ///                     Less than zero
            ///                 <paramref name="x"/> is less than <paramref name="y"/>.
            ///                     Zero
            ///                 <paramref name="x"/> equals <paramref name="y"/>.
            ///                     Greater than zero
            ///                 <paramref name="x"/> is greater than <paramref name="y"/>.
            /// </returns>
            /// <param name="x">The first object to compare.
            ///                 </param><param name="y">The second object to compare.
            ///                 </param>
            public int Compare(IQuestion x, IQuestion y)
            {
                if (x == null || y == null) return 0;

                if (this.descending)
                    return x.RelativeOrder.CompareTo(y.RelativeOrder);
                return y.RelativeOrder.CompareTo(x.RelativeOrder);
            }

            #endregion
        }
    }
}