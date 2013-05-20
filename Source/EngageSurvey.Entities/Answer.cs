// <copyright file="Answer.cs" company="Engage Software">
// Engage: Survey
// Copyright (c) 2004-2013
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
    using UI;
    using Util;

    /// <summary>
    /// An answer to a <see cref="Question"/>
    /// </summary>
    public partial class Answer : IAnswer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Answer"/> class.
        /// </summary>
        /// <param name="revisingUser">The revising user.</param>
        public Answer(int revisingUser)
                : this()
        {
            this.CreatedBy = this.RevisingUser = revisingUser;
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
                // none
                return string.Empty;
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
                return new Key { SectionId = this.Question.SectionId, QuestionId = this.QuestionId, AnswerId = this.AnswerId };
            }
        }

        /// <summary>
        /// Gets only the text value of Text attribute for the survey element.
        /// </summary>
        /// <value></value>
        public string UnformattedText
        {
            get
            {
                return this.Text;
            }
        }

        /// <summary>
        /// Compares <see cref="Answer"/> instances based on their <see cref="Answer.RelativeOrder"/>
        /// </summary>
        internal class RelativeOrderComparer : IComparer<IAnswer>
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
            public int Compare(IAnswer x, IAnswer y)
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