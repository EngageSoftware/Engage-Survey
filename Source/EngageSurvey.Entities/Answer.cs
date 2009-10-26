// <copyright file="Answer.cs" company="Engage Software">
// Engage: Survey
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
    using System;
    using System.Collections.Generic;
    using Util;

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
        /// Returns the formatting for the element plus the the unformatted text together. Used primarily by
        /// the Web and Windows viewers only.
        /// </summary>
        /// <value></value>
        public string FormattedText
        {
            get { return this.Formatting + this.UnformattedText; }
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
                //none
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the rendering key used by the SurveyControl to uniquely identify this element.
        /// </summary>
        /// <value>The rendering key.</value>
        public Key RelationshipKey
        {
            get
            {
                return new Key{ SectionId = this.Question.SectionId, QuestionId = this.QuestionId, AnswerId = this.AnswerId};
            }
        }

        /// <summary>
        /// RelativeOrderComparer
        /// </summary>
        internal class RelativeOrderComparer : IComparer<IAnswer>
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
            public int Compare(IAnswer x, IAnswer y)
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