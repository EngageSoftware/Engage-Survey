// <copyright file="Survey.cs" company="Engage Software">
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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Util;

    /// <summary>
    /// Survey Instance
    /// </summary>
    public partial class Survey : ISurvey
    {
        /// <summary>
        /// Returns the formatting for the element plus the the unformatted text together. Used primarily by
        /// the Web and Windows viewers only. 
        /// </summary>
        /// <value></value>
        public string FormattedText
        {
            get { return this.ShowText ? (this.Formatting + this.UnformattedText) : string.Empty; }
        }

        /// <summary>
        /// Gets or sets only the text value of Text attribute for the survey element.
        /// </summary>
        /// <value></value>
        public string UnformattedText
        {
            get { return this.Text; }
        }

        /// <summary>
        /// Gets the formatting that will be used to prefix the unformatted text for the survey element.
        /// </summary>
        /// <value></value>
        public string Formatting
        {
            get	 { return string.Empty; }
        }

        public List<ISection> GetSections()
        {
            List<ISection> sections = new List<ISection>();
            
            foreach (Section s in Sections)
            {
                sections.Add(s);
            }

            return sections;
        }

        /// <summary>
        /// Gets the section.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public ISection GetSection(string key)
        {
            foreach (ISection section in this.Sections)
            {
                if (section.SectionId.ToString() == key)
                {
                    return section;
                }
            }

            return null;
        }

        /// <summary>
        /// Renders the survey from this survey.
        /// </summary>
        /// <param name="ph">The place holder to render the survey..</param>
        /// <param name="readOnly">if set to <c>true</c> [read only].</param>
        /// <param name="showRequiredNotation">if set to <c>true</c> [show required notation].</param>
        /// <param name="validationProvider">The validation provider.</param>
        public virtual void Render(PlaceHolder ph, bool readOnly, bool showRequiredNotation, ValidationProviderBase validationProvider)
        {
            Debug.Assert(ph != null, "ph cannot be null");
            Debug.Assert(validationProvider != null, "validationProvider cannot be null");

            // add the survey title
            if (this.ShowText)
            {
                HtmlGenericControl titleDiv = new HtmlGenericControl("DIV");
                titleDiv.Attributes["class"] = Utility.CssClassSurveyTitle;
                titleDiv.InnerText = this.Text;
                ph.Controls.Add(titleDiv);
            }

            List<ISection> sections = this.GetSections();
            foreach (ISection s in sections)
            {
                s.Render(ph, readOnly, showRequiredNotation, validationProvider);
            }
        }

        /// <summary>
        /// Posts the save processing.
        /// </summary>
        /// <param name="control">The control.</param>
        public virtual void PostSaveProcessing(WebControl control)
        {
            List<ISection> sections = this.GetSections();
            foreach (ISection s in sections)
            {
                s.PostSaveProcessing(control);
            }
        }

        /// <summary>
        /// Pres the save processing.
        /// </summary>
        /// <param name="control">The control.</param>
        public virtual void PreSaveProcessing(WebControl control)
        {
            List<ISection> sections = this.GetSections();
            foreach (ISection s in sections)
            {
                s.PreSaveProcessing(control);
            }
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns></returns>
        public int Save()
        {
            foreach (ISection section in Sections)
            {
                foreach (IQuestion question in section.GetQuestions())
                {
                    foreach (IAnswer answer in question.GetAnswerChoices())
                    {
                        if (question.Responses.Count == 1)
                        {
                            foreach (UserResponse response in question.Responses)
                            {
                                string responseText = null;
                                if (response.AnswerValue == answer.Text)
                                {
                                    responseText = answer.Text;
                                }
                                WriteResponseEntry(section, question, answer, responseText);
                            }
                        }
                        else
                        {
                            UserResponse response = question.FindResponse(answer);
                            WriteResponseEntry(section, question, answer, response.AnswerValue);
                        }
                    }
                }
            }

            return 1;
        }

        /// <summary>
        /// Writes the response entry.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="question">The question.</param>
        /// <param name="answer">The answer.</param>
        /// <param name="responseText">The response text.</param>
        private void WriteResponseEntry(ISection section, IQuestion question, IAnswer answer, string responseText)
        {
            SurveyModelDataContext context = SurveyModelDataContext.Instance;
            EngageSurvey_Response r = new EngageSurvey_Response
                                          {
                                                  SurveyId = this.SurveyId,
                                                  SurveyText = this.Text,
                                                  ShowSurveyText = this.ShowText,
                                                  TitleOption = this.TitleOption,
                                                  SectionText = section.Text,
                                                  SectionRelativeOrder = section.RelativeOrder,
                                                  ShowSectionText = false
                                          };
            r.SectionRelativeOrder = section.RelativeOrder;
            r.QuestionText = question.Text;
            r.QuestionRelativeOrder = question.RelativeOrder;
            r.QuestionFormatOption = this.QuestionFormatOption;
            r.ControlType = question.ControlType;
            r.AnswerFormatOption = this.AnswerFormatOption;
            r.AnswerText = answer.Text;
            r.AnswerRelativeOrder = answer.RelativeOrder;
            r.AnswerIsCorrect = answer.IsCorrect;
            r.Response = responseText;
            r.CreatedBy = 1;
            r.CreationDate = DateTime.Now;
            r.RevisingUser = 1;
            r.RevisionDate = DateTime.Now;

            context.EngageSurvey_Responses.InsertOnSubmit(r);
            context.SubmitChanges();

            Debug.WriteLine(r.ResponseId);
        }
    }
}