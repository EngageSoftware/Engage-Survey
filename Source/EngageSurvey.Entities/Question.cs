// <copyright file="Question.cs" company="Engage Software">
// Engage: Survey
// Copyright (c) 2004-2010
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
    using System.Linq;
    using UI;
    using Util;

    /// <summary>
    /// A question within a survey <see cref="Section"/>
    /// </summary>
    public partial class Question : IQuestion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Question"/> class.
        /// </summary>
        /// <param name="revisingUser">The revising user.</param>
        public Question(int revisingUser)
                : this()
        {
            this.CreatedBy = this.RevisingUser = revisingUser;
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
        /// Gets the formatting for the element plus the the unformatted text together. Used primarily by
        /// the Web and Windows viewers only.
        /// </summary>
        public string FormattedText
        {
            get
            {
                return this.Formatting + this.UnformattedText;
            }
        }

        /// <summary>
        /// Gets the formatting that will be used to prefix the unformatted text for the survey element.
        /// </summary>
        public string Formatting
        {
            get
            {
                ISurvey survey = this.Section.Survey;
                if (survey != null)
                {
                    return Utility.PrependFormatting(survey.QuestionFormatOption, this.RelativeOrder);
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is boolean.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is boolean; otherwise, <c>false</c>.
        /// </value>
        public bool IsBoolean
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the rendering key used by the <see cref="SurveyControl"/> to uniquely identify this element.
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
        /// Gets only the text value of Text attribute for the survey element.
        /// </summary>
        public string UnformattedText
        {
            get
            {
                return this.Text;
            }
        }

        /// <summary>
        /// Finds the response.
        /// </summary>
        /// <param name="answer">The answer.</param>
        /// <returns>The <see cref="UserResponse"/> with the given answer, or <c>null</c> if no such response exists</returns>
        public UserResponse FindResponse(IAnswer answer)
        {
            return this.Responses.FirstOrDefault(r => r.RelationshipKey.Equals(answer.RelationshipKey));
        }

        /// <summary>
        /// Gets the answer choice.
        /// </summary>
        /// <param name="key">The key by which to find with answer (i.e. a key with an <see cref="Key.AnswerId"/> that is the answer ID of the answer to get).</param>
        /// <returns>The answer with the same answer ID as the given <paramref name="key"/></returns>
        public IAnswer GetAnswer(Key key)
        {
            return this.GetAnswers().SingleOrDefault(answer => answer.AnswerId == key.AnswerId);
        }

        /// <summary>
        /// Gets the answer choices for this question.
        /// </summary>
        /// <returns>The list of all answers for this question</returns>
        public List<IAnswer> GetAnswers()
        {
            var answers = new List<IAnswer>();

            foreach (Answer a in this.Answers)
            {
                answers.Add(a);
            }

            answers.Sort(new Answer.RelativeOrderComparer());
            return answers;
        }

        /// <summary>
        /// Gets the section.
        /// </summary>
        /// <returns>The <see cref="Section"/> in which this question lives</returns>
        public ISection GetSection()
        {
            return this.Section;
        }

        /// <summary>
        /// Compares <see cref="Question"/> instances based on their <see cref="Question.RelativeOrder"/>
        /// </summary>
        internal class RelativeOrderComparer : IComparer<IQuestion>
        {
            /// <summary>
            /// Whether the sorting is descending or ascending
            /// </summary>
            private readonly bool descending = true;

            /// <summary>
            /// Initializes a new instance of the <see cref="RelativeOrderComparer"/> class.
            /// </summary>
            public RelativeOrderComparer()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="RelativeOrderComparer"/> class.
            /// </summary>
            /// <param name="descending">if set to <c>true</c> sorts by descending relative order; otherwise, ascending.</param>
            public RelativeOrderComparer(bool descending)
            {
                this.descending = descending;
            }

            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            /// <returns>
            /// Value
            /// Condition
            /// Less than zero
            /// <paramref name="x"/> is less than <paramref name="y"/>.
            /// Zero
            /// <paramref name="x"/> equals <paramref name="y"/>.
            /// Greater than zero
            /// <paramref name="x"/> is greater than <paramref name="y"/>.
            /// </returns>
            public int Compare(IQuestion x, IQuestion y)
            {
                if (x == null || y == null)
                {
                    return 0;
                }

                if (this.descending)
                {
                    return x.RelativeOrder.CompareTo(y.RelativeOrder);
                }

                return y.RelativeOrder.CompareTo(x.RelativeOrder);
            }
        }
    }
}