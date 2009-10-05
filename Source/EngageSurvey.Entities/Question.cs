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
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Summary description for Question.
    /// </summary>
    public partial class Question : IQuestion
    {
        /// <summary>
        /// Gets the rendering key used by the SurveyControl to uniquely identify this element.
        /// </summary>
        /// <value>The rendering key.</value>
        public string RelationshipKey
        {
            get
            {
                return this.SectionId + "-" +  this.QuestionId;
            }
        }

        public string AnswerValue
        {
            get;
            set;
        }

        //public List<Response> Responses
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Gets the section.
        /// </summary>
        /// <returns></returns>
        /// <value>The section.</value>
        public ISection GetSection()
        {
            return this.Section; 
        }

        public List<IAnswer> GetAnswerChoices()
        {
            List<IAnswer> answers = new List<IAnswer>();

            foreach (Answer a in Answers)
            {
                answers.Add(a);
            }

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
            get { return string.Empty; }
        }

        /// <summary>
        /// RelativeOrderComparer
        /// </summary>
        internal class RelativeOrderComparer : IComparer
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

            public int Compare(object q1, object q2)
            {
                if (q1 == null && q2 == null) return 0;

                Question qu1 = q1 as Question;
                Question qu2 = q2 as Question;

                if (qu1 == null) throw new ArgumentException("oa1 is not an instance of Question");
                if (qu2 == null) throw new ArgumentException("oa2 is not an instance of Question");

                if (this.descending)
                    return qu1.RelativeOrder.CompareTo(qu2.RelativeOrder);
                return qu2.RelativeOrder.CompareTo(qu1.RelativeOrder);
            }

            #endregion
        }
    }
}