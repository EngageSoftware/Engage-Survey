// <copyright file="Survey.cs" company="Engage Software">
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
    using System.Diagnostics;
    using System.Linq;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using DotNetNuke.Services.Localization;
    using Util;

    /// <summary>
    /// Survey Instance
    /// </summary>
    public partial class Survey : ISurvey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Survey"/> class.
        /// </summary>
        /// <param name="revisingUser">The revising user.</param>
        public Survey(int revisingUser)
                : this()
        {
            this.CreatedBy = this.RevisingUser = revisingUser;
            this.Sections.Add(new Section { CreatedBy = revisingUser, RevisingUser = revisingUser });
        }

        /// <summary>
        /// Gets the formatting for the element plus the the unformatted text together. Used primarily by
        /// the Web and Windows viewers only. 
        /// </summary>
        /// <value></value>
        public string FormattedText
        {
            get
            {
                return this.ShowText ? (this.Formatting + this.UnformattedText) : string.Empty;
            }
        }

        /// <summary>
        /// Gets the formatting that will be used to prefix the unformatted text for the survey element.
        /// </summary>
        /// <value></value>
        public string Formatting
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is read-only.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is read-only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [send notification].
        /// </summary>
        /// <value><c>true</c> if [send notification]; otherwise, <c>false</c>.</value>
        public bool SendNotification
        {
            get
            {
                return true;
            }

            set
            {
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
        /// Deletes the specified survey.
        /// </summary>
        /// <param name="surveyId">The ID of the survey to delete.</param>
        public static void Delete(int? surveyId)
        {
            SurveyModelDataContext context = SurveyModelDataContext.Instance;

            var answers = from a in context.Answers
                          join q in context.Questions on a.QuestionId equals q.QuestionId
                          join s in context.Sections on q.SectionId equals s.SectionId
                          join su in context.Surveys on s.SurveyId equals su.SurveyId
                          where su.SurveyId == surveyId
                          select a;

            context.Answers.DeleteAllOnSubmit(answers);

            var questions = from q in context.Questions
                            join s in context.Sections on q.SectionId equals s.SectionId
                            join su in context.Surveys on s.SurveyId equals su.SurveyId
                            where su.SurveyId == surveyId
                            select q;

            context.Questions.DeleteAllOnSubmit(questions);

            var sections = from s in context.Sections
                           join su in context.Surveys on s.SurveyId equals su.SurveyId
                           where su.SurveyId == surveyId
                           select s;

            context.Sections.DeleteAllOnSubmit(sections);

            // lastly remove the survey itself
            context.Surveys.DeleteOnSubmit(context.Surveys.Where(evaluation => evaluation.SurveyId == surveyId).Single());

            context.SubmitChanges();
        }

        /// <summary>
        /// Loads the survey.
        /// </summary>
        /// <param name="surveyId">The survey id.</param>
        /// <returns>The survey with the given ID, or <c>null</c> if no survey exists with the given ID</returns>
        public static Survey LoadSurvey(int surveyId)
        {
            return SurveyModelDataContext.Instance.Surveys.SingleOrDefault(s => s.SurveyId == surveyId);
        }

        /// <summary>
        /// Loads all the surveys.
        /// </summary>
        /// <returns>A sequence of all <see cref="Survey"/> instances</returns>
        public static IQueryable<Survey> LoadSurveys()
        {
            return SurveyModelDataContext.Instance.Surveys;
        }

        /// <summary>
        /// Static mechanism to render a survey.
        /// </summary>
        /// <param name="survey">The survey.</param>
        /// <param name="placeHolder">The place holder.</param>
        /// <param name="readOnly">if set to <c>true</c> [read only].</param>
        /// <param name="showRequiredNotation">if set to <c>true</c> [show required notation].</param>
        /// <param name="validationProvider">The validation provider.</param>
        public static void RenderSurvey(ISurvey survey, PlaceHolder placeHolder, bool readOnly, bool showRequiredNotation, ValidationProviderBase validationProvider)
        {
            Debug.Assert(placeHolder != null, "placeHolder cannot be null");
            Debug.Assert(validationProvider != null, "validationProvider cannot be null");

            // add the survey title
            if (survey.ShowText)
            {
                var titleDiv = new HtmlGenericControl("DIV");
                titleDiv.Attributes["class"] = Utility.CssClassSurveyTitle;
                titleDiv.InnerText = survey.Text;
                placeHolder.Controls.Add(titleDiv);
            }

            List<ISection> sections = survey.GetSections();
            foreach (ISection s in sections)
            {
                s.Render(placeHolder, readOnly, showRequiredNotation, validationProvider);
            }
        }

        /// <summary>
        /// Gets the section.
        /// </summary>
        /// <param name="sectionId">The section ID.</param>
        /// <returns>The <see cref="ISection"/> with the given ID, or <c>null</c> if no section exists with the given ID</returns>
        public ISection GetSection(string sectionId)
        {
            return this.GetSections().FirstOrDefault(section => section.SectionId.ToString() == sectionId);
        }

        /// <summary>
        /// Gets the sections for this instance.
        /// </summary>
        /// <returns>List of <see cref="ISection"/> instances for this survey</returns>
        public List<ISection> GetSections()
        {
            var sections = new List<ISection>();

            foreach (var section in this.Sections)
            {
                sections.Add(section);
            }

            sections.Sort(new Section.RelativeOrderComparer());
            return sections;
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
        /// Renders the survey from this survey.
        /// </summary>
        /// <param name="placeHolder">The place holder.</param>
        /// <param name="readOnly">if set to <c>true</c> [read only].</param>
        /// <param name="showRequiredNotation">if set to <c>true</c> [show required notation].</param>
        /// <param name="validationProvider">The validation provider.</param>
        public virtual void Render(PlaceHolder placeHolder, bool readOnly, bool showRequiredNotation, ValidationProviderBase validationProvider)
        {
            RenderSurvey(this, placeHolder, readOnly, showRequiredNotation, validationProvider);
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <param name="userId">The ID of the user saving this instance</param>
        /// <returns>The ID of the created <see cref="ResponseHeader"/></returns>
        public int Save(int userId)
        {
            int responseHeaderId = CreateResponseHeader(userId);
            this.WriteResponses(responseHeaderId);

            return responseHeaderId;
        }

        /// <summary>
        /// Creates the response header.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>The ID of the created <see cref="ResponseHeader"/></returns>
        private static int CreateResponseHeader(int userId)
        {
            SurveyModelDataContext context = SurveyModelDataContext.Instance;
            var header = new ResponseHeader
                             {
                                 CreatedBy = userId, RevisingUser = userId, UserId = userId, RevisionDate = DateTime.Now, CreationDate = DateTime.Now 
                             };
            context.ResponseHeaders.InsertOnSubmit(header);
            context.SubmitChanges();

            return header.ResponseHeaderId;
        }

        /// <summary>
        /// Called when a <see cref="Survey"/> instance is created.
        /// </summary>
        partial void OnCreated()
        {
            this.FinalMessage = Localization.GetString("Survey Complete.Text", "~/DesktopModules/EngageSurvey/App_LocalResources/SharedResources.resx");
            this.FinalMessageOption = FinalMessageOption.UseFinalMessage;
            this.SectionFormatOption = ElementFormatOption.None;
            this.QuestionFormatOption = ElementFormatOption.None;
            this.TitleOption = TitleOption.FirstPageOnly;
        }

        /// <summary>
        /// Writes the response entry.
        /// </summary>
        /// <param name="responseHeaderId">The response header id.</param>
        /// <param name="section">The section.</param>
        /// <param name="question">The question.</param>
        /// <param name="answer">The answer.</param>
        /// <param name="responseText">The response text.</param>
        private void WriteResponseEntry(int responseHeaderId, ISection section, IQuestion question, IAnswer answer, string responseText)
        {
            SurveyModelDataContext context = SurveyModelDataContext.Instance;
            var response = new Response
                             {
                                     SurveyId = this.SurveyId, 
                                     SurveyText = this.Text, 
                                     ShowSurveyText = this.ShowText, 
                                     TitleOption = this.TitleOption, 
                                     SectionText = section.Text, 
                                     SectionRelativeOrder = section.RelativeOrder, 
                                     ShowSectionText = false, 
                                     ResponseHeaderId = responseHeaderId, 
                                     SectionId = section.SectionId
                             };
            response.SectionRelativeOrder = section.RelativeOrder;
            response.SectionFormatOption = this.SectionFormatOption;
            response.QuestionId = question.QuestionId;
            response.QuestionText = question.Text;
            response.QuestionRelativeOrder = question.RelativeOrder;
            response.QuestionFormatOption = this.QuestionFormatOption;
            response.ControlType = question.ControlType;
            if (answer != null)
            {
                response.AnswerId = answer.AnswerId;
                response.AnswerText = answer.Text;
                response.AnswerRelativeOrder = answer.RelativeOrder;
                response.AnswerIsCorrect = answer.IsCorrect;
            }

            response.UserResponse = responseText;
            response.CreatedBy = 1;
            response.CreationDate = DateTime.Now;
            response.RevisingUser = 1;
            response.RevisionDate = DateTime.Now;

            context.Responses.InsertOnSubmit(response);
            context.SubmitChanges();

            Debug.WriteLine(response.ResponseId);
        }

        /// <summary>
        /// Writes the responses.
        /// </summary>
        /// <param name="responseHeaderId">The response header id.</param>
        private void WriteResponses(int responseHeaderId)
        {
            foreach (ISection section in this.GetSections())
            {
                foreach (IQuestion question in section.GetQuestions())
                {
                    if (question.GetAnswers().Count == 0)
                    {
                        // Open ended question.
                        foreach (UserResponse response in question.Responses)
                        {
                            this.WriteResponseEntry(responseHeaderId, section, question, null, response.AnswerValue);
                        }
                    }
                    else
                    {
                        foreach (IAnswer answer in question.GetAnswers())
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

                                    this.WriteResponseEntry(responseHeaderId, section, question, answer, responseText);
                                }
                            }
                            else
                            {
                                UserResponse response = question.FindResponse(answer);
                                this.WriteResponseEntry(responseHeaderId, section, question, answer, response.AnswerValue);
                            }
                        }
                    }
                }
            }
        }
    }
}