// <copyright file="ReadonlySurvey.cs" company="Engage Software">
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
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using DotNetNuke.Services.Localization;
    using Util;

    /// <summary>
    /// Represents a read-only survey.
    /// </summary>
    public class ReadonlySurvey : ISurvey
    {
        /// <summary>
        /// Gets or sets the response header id.
        /// </summary>
        /// <value>The response header id.</value>
        public int ResponseHeaderId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public int? UserId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        /// <value>The creation date.</value>
        public DateTime CreationDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the formatting for the element plus the the unformatted text together. Used primarily by
        /// the Web and Windows viewers only. 
        /// </summary>
        /// <value></value>
        public string FormattedText
        {
            get { return this.ShowText ? (this.Formatting + this.UnformattedText) : string.Empty; }
        }

        /// <summary>
        /// Gets only the text value of Text attribute for the survey element.
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
            get { return string.Empty; }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The Text.</value>
        public string Text
        {
            get;
            set;
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
                return true;
            }
        }

        /// <summary>
        /// Gets or sets the survey id.
        /// </summary>
        /// <value>The survey id.</value>
        public int SurveyId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the final message option.
        /// </summary>
        /// <value>The final message option.</value>
        public FinalMessageOption FinalMessageOption
        {
            get
            {
                return FinalMessageOption.None;
            }
        }

        /// <summary>
        /// Gets the final message.
        /// </summary>
        /// <value>The final message.</value>
        public string FinalMessage
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the final URL.
        /// </summary>
        /// <value>The final URL.</value>
        public string FinalUrl
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show title].
        /// </summary>
        /// <value><c>true</c> if [show title]; otherwise, <c>false</c>.</value>
        public bool ShowText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the question format option.
        /// </summary>
        /// <value>The question format option.</value>
        public ElementFormatOption QuestionFormatOption
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the section format option.
        /// </summary>
        /// <value>The answer format option.</value>
        public ElementFormatOption SectionFormatOption
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the title option.
        /// </summary>
        /// <value>The title option.</value>
        public TitleOption TitleOption
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the logo URL.
        /// </summary>
        /// <value>The logo URL.</value>
        public string LogoURL
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to send a notification email to an administrator after someone has completed the survey.
        /// </summary>
        /// <value>
        /// <c>true</c> if notification emails should be sent; otherwise, <c>false</c>.
        /// </value>
        public bool SendNotification
        {
            get { return false; }
            set { }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to send a thank you email to the survey taker (assuming that their email address is known) after they have completed the survey.
        /// </summary>
        /// <value>
        /// <c>true</c> if thank you emails should be sent; otherwise, <c>false</c>.
        /// </value>
        public bool SendThankYou
        {
            get { return false; }
            set { }
        }

        /// <summary>
        /// Gets or sets the email address from which notification emails should be sent.
        /// </summary>
        /// <value>The "from" email address for notification emails.</value>
        public string NotificationFromEmailAddress
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// Gets or sets the email addresses to which notification emails should be sent.
        /// </summary>
        /// <value>The "to" email address(es) for notification emails.</value>
        public string NotificationToEmailAddresses
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// Gets or sets the email address from which thank you emails should be sent.
        /// </summary>
        /// <value>The "from" email address for thank you emails.</value>
        public string ThankYouFromEmailAddress
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// Gets or sets the date and time on which the survey is first available.
        /// </summary>
        /// <value>The start date for the survey.</value>
        public DateTime? StartDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date and time on which the survey expires, i.e. is no longer available to be taken.
        /// </summary>
        /// <value>The end date for the survey.</value>
        public DateTime? EndDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the message to display when the user tries to access a survey which has not yet started.
        /// </summary>
        /// <value>The message to display before this survey's <see cref="ISurvey.StartDate"/>.</value>
        public string PreStartMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the message to display when the user tries to access a survey which has expired.
        /// </summary>
        /// <value>The message to display after this survey's <see cref="ISurvey.EndDate"/>.</value>
        public string PostEndMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Loads a completed survey using the <see cref="ResponseHeader"/> ID for the User/Survey.
        /// </summary>
        /// <param name="responseHeaderId">The <see cref="ResponseHeader"/> ID.</param>
        /// <returns>The survey with the given ID</returns>
        public static ISurvey LoadSurvey(int responseHeaderId)
        {
            return LoadSurveys().Where(s => s.ResponseHeaderId == responseHeaderId).SingleOrDefault();
        }

        /// <summary>
        /// Loads all completed surveys.
        /// </summary>
        /// <returns>A list of surveys.</returns>
        public static IQueryable<ReadonlySurvey> LoadSurveys()
        {
            SurveyModelDataContext context = SurveyModelDataContext.Instance;
            return (from s in context.Responses
                    join r in context.ResponseHeaders on s.ResponseHeaderId equals r.ResponseHeaderId
                    select
                            new ReadonlySurvey
                                {
                                        SurveyId = s.SurveyId,
                                        Text = s.SurveyText,
                                        ShowText = s.ShowSurveyText,
                                        TitleOption = s.TitleOption,
                                        QuestionFormatOption = s.QuestionFormatOption,
                                        SectionFormatOption = s.SectionFormatOption,
                                        ResponseHeaderId = r.ResponseHeaderId,
                                        CreationDate = r.CreationDate,
                                        UserId = r.UserId
                                }).Distinct();
        }

        /// <summary>
        /// Deletes the specified completed survey <see cref="ResponseHeader"/> and responses.
        /// </summary>
        /// <param name="responseHeaderId">The <see cref="ResponseHeader"/> ID</param>
        public static void Delete(int? responseHeaderId)
        {
            SurveyModelDataContext context = SurveyModelDataContext.Instance;

            var responseHeader = (from rh in context.ResponseHeaders 
                                  where rh.ResponseHeaderId == responseHeaderId
                                  select rh).Single();

            context.ResponseHeaders.DeleteOnSubmit(responseHeader);
            context.Responses.DeleteAllOnSubmit(responseHeader.Responses);

            context.SubmitChanges();
        }

        /// <summary>
        /// Gets the sections.
        /// </summary>
        /// <returns>List of ISections for this survey</returns>
        public List<ISection> GetSections()
        {
            SurveyModelDataContext context = SurveyModelDataContext.Instance;
            var results = (from s in context.Responses
                           where s.ResponseHeaderId == this.ResponseHeaderId && s.SurveyId == this.SurveyId
                           select new ReadonlySection
                                       {
                                               SurveyId = s.SurveyId,
                                               SectionId = s.SectionId,
                                               Text = s.SectionText,
                                               ShowText = s.ShowSectionText,
                                               RelativeOrder = s.SectionRelativeOrder,
                                               ResponseHeaderId = s.ResponseHeaderId
                                       }).Distinct();
            var sections = new List<ISection>();
            foreach (ReadonlySection s in results)
            {
                sections.Add(s);
            }

            sections.Sort(new Section.RelativeOrderComparer());
            return sections;
        }

        /// <summary>
        /// Gets the section.
        /// </summary>
        /// <param name="sectionId">The section id.</param>
        /// <returns>An <see cref="ISection"/> using the section ID.</returns>
        public ISection GetSection(string sectionId)
        {
            foreach (ISection section in this.GetSections())
            {
                if (section.SectionId.ToString() == sectionId)
                {
                    return section;
                }
            }

            return null;
        }

        /// <summary>
        /// Renders the survey from this survey.
        /// </summary>
        /// <param name="placeHolder">The place holder.</param>
        /// <param name="readOnly">if set to <c>true</c> [read only].</param>
        /// <param name="showRequiredNotation">if set to <c>true</c> [show required notation].</param>
        /// <param name="validationProvider">The validation provider.</param>
        public void Render(PlaceHolder placeHolder, bool readOnly, bool showRequiredNotation, ValidationProviderBase validationProvider)
        {
            Survey.RenderSurvey(this, placeHolder, readOnly, showRequiredNotation, validationProvider);
        }

        /// <summary>
        /// Renders the survey from this survey.
        /// </summary>
        /// <param name="table">The HTML table to put the survey into.</param>
        /// <param name="resourceFile">The resource file.</param>
        public void Render(Table table, string resourceFile)
        {
            Debug.Assert(table != null, "table cannot be null");

            // add the survey title
            if (this.ShowText)
            {
                string titleStyle = Localization.GetString("TitleInlineStyle", resourceFile);
                var row = new TableRow();
                table.Rows.Add(row);
                var cell = new TableCell();
                row.Cells.Add(cell);
                cell.Text = this.Text;
                cell.Attributes.Add("style", titleStyle);
            }

            foreach (ReadonlySection s in this.GetSections())
            {
                s.Render(table, resourceFile);
            }            
        }

        /// <summary>
        /// Pres the save processing.
        /// </summary>
        /// <param name="control">The control.</param>
        public void PreSaveProcessing(WebControl control)
        {
        }

        /// <summary>
        /// Posts the save processing.
        /// </summary>
        /// <param name="control">The control.</param>
        public void PostSaveProcessing(WebControl control)
        {
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <param name="userId">The ID of the user saving this instance</param>
        /// <returns>The ID of the created <c>ResponseHeader</c>, or <c>0</c> if nothing was saved</returns>
        public int Save(int userId)
        {
            return 0;
        }
    }

    /// <summary>
    /// A read-only section for a read-only survey.
    /// </summary>
    public class ReadonlySection : ISection
    {
        /// <summary>
        /// Gets or sets the response header id.
        /// </summary>
        /// <value>The response header id.</value>
        public int ResponseHeaderId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the formatting for the element plus the the unformatted text together. Used primarily by
        /// the Web and Windows viewers only. 
        /// </summary>
        /// <value></value>
        public string FormattedText
        {
            get { return this.ShowText ? (this.Formatting + this.UnformattedText) : string.Empty; }
        }

        /// <summary>
        /// Gets only the text value of Text attribute for the survey element.
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
            get
            {
                ISurvey survey = this.GetSurvey();
                if (survey != null)
                {
                    return Utility.PrependFormatting(survey.SectionFormatOption, this.RelativeOrder);
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The Text.</value>
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show text].
        /// </summary>
        /// <value><c>true</c> if [show text]; otherwise, <c>false</c>.</value>
        public bool ShowText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the survey id.
        /// </summary>
        /// <value>The survey id.</value>
        public int SurveyId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the section id.
        /// </summary>
        /// <value>The section id.</value>
        public int SectionId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the relative order.
        /// </summary>
        /// <value>The relative order.</value>
        public int RelativeOrder
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the survey.
        /// </summary>
        /// <returns></returns>
        public ISurvey GetSurvey()
        {
            SurveyModelDataContext context = SurveyModelDataContext.Instance;
            return context.Surveys.FirstOrDefault(s => s.SurveyId == this.SurveyId);
        }

        /// <summary>
        /// Gets the questions.
        /// </summary>
        /// <returns>Array of IQuestions</returns>
        public List<IQuestion> GetQuestions()
        {
            SurveyModelDataContext context = SurveyModelDataContext.Instance;
            var results = (from s in context.Responses
                           where s.ResponseHeaderId == this.ResponseHeaderId && s.SectionId == this.SectionId
                           select new ReadonlyQuestion 
                           {
                                   QuestionId = s.QuestionId,
                                   Text = s.QuestionText,
                                   Comments = s.Comments,
                                   RelativeOrder = s.QuestionRelativeOrder,
                                   ControlType = s.ControlType,
                                   SectionId = s.SectionId,
                                   ResponseHeaderId = s.ResponseHeaderId
                           }).Distinct();

            var questions = new List<IQuestion>();
            foreach (ReadonlyQuestion q in results)
            {
                if (q.GetAnswers().Count == 0)
                {
                    // Special case, these are open ended questions with no rows in the asnwer table. LargeTextInputField or SmallTextInputField
                    q.Responses = new List<UserResponse>();

                    // fetch the open ended response since we can't include in distinct list above.
                    ReadonlyQuestion question = q;
                    var result = context.Responses.Where(r => r.ResponseHeaderId == this.ResponseHeaderId && r.QuestionId == question.QuestionId).FirstOrDefault();
                    var response = new UserResponse { RelationshipKey = q.RelationshipKey, AnswerValue = result.UserResponse };
                    q.Responses.Add(response);    
                }

                questions.Add(q);
            }

            questions.Sort(new Question.RelativeOrderComparer());
            return questions;
        }

        /// <summary>
        /// Gets the question.
        /// </summary>
        /// <param name="key">The key name.</param>
        /// <returns>An <see cref="IQuestion"/> using the passed key.</returns>
        public IQuestion GetQuestion(Key key)
        {
            foreach (IQuestion question in this.GetQuestions())
            {
                if (question.QuestionId == key.QuestionId)
                {
                    return question;
                }
            }

            return null;
        }

        /// <summary>
        /// Renders this survey section instance.
        /// </summary>
        /// <param name="placeHolder">The place holder.</param>
        /// <param name="readOnly">if set to <c>true</c> [read only].</param>
        /// <param name="showRequiredNotation">if set to <c>true</c> [show required notation].</param>
        /// <param name="validationProvider">The validation provider.</param>
        public void Render(PlaceHolder placeHolder, bool readOnly, bool showRequiredNotation, ValidationProviderBase validationProvider)
        {
            Section.RenderSection(this, placeHolder, readOnly, showRequiredNotation, validationProvider);
        }

        /// <summary>
        /// Renders the read-only section in a table.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="resourceFile">The resource file.</param>
        public void Render(Table table, string resourceFile)
        {

            var row = new TableRow();
            table.Rows.Add(row);

            // cell for the section table
            var cell = new TableCell();
            row.Cells.Add(cell);

            // let's create a new table for this section
            string sectionWrapStyle = Localization.GetString("SectionWrapInlineStyle", resourceFile);
            var sectionTable = new Table();
            sectionTable.Attributes.Add("style", sectionWrapStyle);
            cell.Controls.Add(sectionTable);

            row = new TableRow();
            sectionTable.Rows.Add(row);

            // row for the section title
            string sectionTitleStyle = Localization.GetString("SectionTitleInlineStyle", resourceFile);
            cell = new TableCell { ColumnSpan = 3, Text = this.FormattedText };
            cell.Attributes.Add("style", sectionTitleStyle);
            row.Cells.Add(cell);

            string answerInlineStyle = Localization.GetString("AnswerInlineStyle", resourceFile);
            foreach (IQuestion question in this.GetQuestions())
            {
                Control formControl = Utility.CreateWebControl(question, true, answerInlineStyle);
                
                row = new TableRow();
                sectionTable.Rows.Add(row);

                // question
                string questionTitleStyle = Localization.GetString("QuestionTitleInlineStyle", resourceFile);
                cell = new TableCell
                           {
                                   ColumnSpan = 2,
                                   Text = question.FormattedText
                           };
                cell.Attributes.Add("style", questionTitleStyle);
                row.Cells.Add(cell);

                row = new TableRow();
                sectionTable.Rows.Add(row);

                // spacer
                cell = new TableCell { Text = Utility.EntityNbsp, Width = 10 };
                row.Cells.Add(cell);

                // answer
                cell = new TableCell();
                row.Cells.Add(cell);

                cell.Controls.Add(formControl);
            }
        }

        /// <summary>
        /// Posts the save processing.
        /// </summary>
        /// <param name="control">The control.</param>
        public void PostSaveProcessing(WebControl control)
        {
        }

        /// <summary>
        /// Pres the save processing.
        /// </summary>
        /// <param name="control">The control.</param>
        public void PreSaveProcessing(WebControl control)
        {
        }
    }

    /// <summary>
    /// A read-only question for a read-only section.
    /// </summary>
    public class ReadonlyQuestion : IQuestion
    {
        /// <summary>
        /// Gets or sets the response header id.
        /// </summary>
        /// <value>The response header id.</value>
        public int ResponseHeaderId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the formatting for the element plus the the unformatted text together. Used primarily by
        /// the Web and Windows viewers only.
        /// </summary>
        /// <value></value>
        public string FormattedText
        {
            get { return this.Formatting + this.UnformattedText; }
        }

        /// <summary>
        /// Gets only the text value of Text attribute for the survey element.
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
            get
            {
                ISurvey survey = this.GetSection().GetSurvey();
                if (survey != null)
                {
                    return Utility.PrependFormatting(survey.QuestionFormatOption, this.RelativeOrder);
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the section id.
        /// </summary>
        /// <value>The section id.</value>
        public int SectionId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The Text.</value>
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is boolean.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is boolean; otherwise, <c>false</c>.
        /// </value>
        public bool IsBoolean
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the rendering key used by the <c>SurveyControl</c> to uniquely identify this element.
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
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        public string Comments
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is required.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is required; otherwise, <c>false</c>.
        /// </value>
        public bool IsRequired
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the question id.
        /// </summary>
        /// <value>The question id.</value>
        public int QuestionId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the control type.
        /// </summary>
        /// <value>The control type.</value>
        public ControlType ControlType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the selection limit.
        /// </summary>
        /// <value>The selection limit.</value>
        public int SelectionLimit
        {
            get;
            set;
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
        /// Gets or sets the relative order.
        /// </summary>
        /// <value>The relative order.</value>
        public int RelativeOrder
        {
            get;
            set;
        }

        /// <summary>
        /// Finds the response.
        /// </summary>
        /// <param name="answer">The answer.</param>
        /// <returns>A user response entered by the user.</returns>
        public UserResponse FindResponse(IAnswer answer)
        {
            foreach (UserResponse r in this.Responses)
            {
                if (r.RelationshipKey.Equals(answer.RelationshipKey))
                {
                    return r;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the answer choice.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>An answer using the passed in key.</returns>
        public IAnswer GetAnswer(Key key)
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
        /// Gets the answer choices.
        /// </summary>
        /// <value>The answer choices.</value>
        public List<IAnswer> GetAnswers()
        {
            // Initialize the responses
            this.Responses = new List<UserResponse>();

            SurveyModelDataContext context = SurveyModelDataContext.Instance;
            var results = (from s in context.Responses
                           where s.ResponseHeaderId == this.ResponseHeaderId && s.QuestionId == this.QuestionId
                           select new ReadonlyAnswer 
                           {
                                AnswerId = s.AnswerId.GetValueOrDefault(0),
                                Text = s.AnswerText,
                                RelativeOrder = s.AnswerRelativeOrder.GetValueOrDefault(0),
                                IsCorrect = s.AnswerIsCorrect.GetValueOrDefault(false),
                                SectionId = s.SectionId,
                                QuestionId = s.QuestionId,
                                ResponseHeaderId = s.ResponseHeaderId,
                                AnswerValue = s.UserResponse
                           }).Distinct();

            var answers = new List<IAnswer>();
            foreach (ReadonlyAnswer a in results)
            {
                // while we are here, load the UserResponse.
                var response = new UserResponse { RelationshipKey = a.RelationshipKey, AnswerValue = a.AnswerValue };
                this.Responses.Add(response);    
                answers.Add(a);
            }

            answers.Sort(new Answer.RelativeOrderComparer());
            return answers;
        }

        /// <summary>
        /// Gets the section.
        /// </summary>
        /// <value>The section for this survey.</value>
        public ISection GetSection()
        {
            SurveyModelDataContext context = SurveyModelDataContext.Instance;
            return (from s in context.Responses
                    where s.ResponseHeaderId == this.ResponseHeaderId && s.SectionId == this.SectionId
                    select
                            new ReadonlySection
                                {
                                        SurveyId = s.SurveyId,
                                        SectionId = s.SectionId,
                                        Text = s.SectionText,
                                        ShowText = s.ShowSectionText,
                                        RelativeOrder = s.SectionRelativeOrder,
                                        ResponseHeaderId = s.ResponseHeaderId
                                }).FirstOrDefault();
        }
    }

    /// <summary>
    /// Represents a read-only answer.
    /// </summary>
    public class ReadonlyAnswer : IAnswer
    {
        /// <summary>
        /// Gets or sets the response header id.
        /// </summary>
        /// <value>The response header id.</value>
        public int ResponseHeaderId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the formatting for the element plus the the unformatted text together. Used primarily by
        /// the Web and Windows viewers only.
        /// </summary>
        /// <value></value>
        public string FormattedText
        {
            get { return this.Formatting + this.UnformattedText; }
        }

        /// <summary>
        /// Gets only the text value of Text attribute for the survey element.
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
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text of the answer.</value>
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the answer id.
        /// </summary>
        /// <value>The answer id.</value>
        public int AnswerId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the question id.
        /// </summary>
        /// <value>The question id.</value>
        public int QuestionId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the section id.
        /// </summary>
        /// <value>The section id.</value>
        public int SectionId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is correct.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is correct; otherwise, <c>false</c>.
        /// </value>
        public bool IsCorrect
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the rendering key used by the <c>SurveyControl</c> to uniquely identify this element.
        /// </summary>
        /// <value>The rendering key.</value>
        public Key RelationshipKey
        {
            get
            {
                return new Key
                           {
                                   SectionId = this.SectionId, QuestionId = this.QuestionId, AnswerId = this.AnswerId
                           };
            }
        }

        /// <summary>
        /// Gets or sets the relative order.
        /// </summary>
        /// <value>The relative order.</value>
        public int RelativeOrder
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the answer value. This is used internally to hold the response by the user.
        /// </summary>
        /// <value>The answer value.</value>
        internal string AnswerValue
        {
            get;
            set;
        }

        // ReSharper disable UnusedMember.Local

        /// <summary>
        /// Gets the question for this answer.
        /// </summary>
        /// <returns></returns>
        private IQuestion GetQuestion()
        {
            SurveyModelDataContext context = SurveyModelDataContext.Instance;
            return (from s in context.Responses
                    where s.ResponseHeaderId == this.ResponseHeaderId && s.QuestionId == this.QuestionId
                    select
                            new ReadonlyQuestion
                                {
                                        QuestionId = s.QuestionId,
                                        Text = s.QuestionText,
                                        Comments = s.Comments,
                                        RelativeOrder = s.QuestionRelativeOrder,
                                        ControlType = s.ControlType,
                                        SectionId = s.SectionId,
                                        ResponseHeaderId = s.ResponseHeaderId
                                }).FirstOrDefault();
        }

// ReSharper restore UnusedMember.Local
    }
}
