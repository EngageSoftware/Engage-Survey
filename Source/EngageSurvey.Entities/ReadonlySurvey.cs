// <copyright file="ReadonlySurvey.cs" company="Engage Software">
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
    using System.Linq;
    using System.Web.UI.WebControls;
    using Util;

    public class ReadonlySurvey: ISurvey
    {
        /// <summary>
        /// Loads a completed survey using the ResponseHeaderId for the User/Survey.
        /// </summary>
        /// <param name="responseHeaderId">The response header id.</param>
        /// <returns></returns>
        public static ISurvey LoadSurvey(int responseHeaderId)
        {
            SurveyModelDataContext context = SurveyModelDataContext.Instance;
            var survey = (from s in context.Responses
                          where s.ResponseHeaderId == responseHeaderId

                          select
                                  new ReadonlySurvey
                                      {
                                              SurveyId = s.SurveyId,
                                              Text = s.SurveyText,
                                              ShowText = s.ShowSurveyText,
                                              TitleOption = s.TitleOption,
                                              QuestionFormatOption = s.QuestionFormatOption,
                                              SectionFormatOption = s.SectionFormatOption
                                      }).FirstOrDefault();

            survey.ResponseHeaderId = responseHeaderId;
            return survey;
        }

        /// <summary>
        /// Loads all completed surveys.
        /// </summary>
        /// <returns></returns>
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
                                ResponseHeaderId = r.ResponseHeaderId
                            }).Distinct();
        }

        /// <summary>
        /// ResponseHeaderId
        /// </summary>
        public int ResponseHeaderId
        {
            get;
            set;
        }

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
        /// Gets a value indicating whether this instance is readonly.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is readonly; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadonly
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
        /// Gets the sections.
        /// </summary>
        /// <returns>List of ISections for this survey</returns>
        public List<ISection> GetSections()
        {
            SurveyModelDataContext context = SurveyModelDataContext.Instance;
            var results = (from s in context.Responses
                          where s.ResponseHeaderId == this.ResponseHeaderId && s.SurveyId == this.SurveyId
                          select new ReadonlySection {
                                  SurveyId = s.SurveyId,
                                  SectionId = s.SectionId,
                                  Text = s.SectionText,
                                  ShowText = s.ShowSectionText,
                                  RelativeOrder = s.SectionRelativeOrder,
                                  ResponseHeaderId = s.ResponseHeaderId
                              }).Distinct();
            List<ISection> sections = new List<ISection>();
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
        /// <returns>An ISection using the sectionId.</returns>
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
        /// Gets or sets a value indicating whether [send notification].
        /// </summary>
        /// <value><c>true</c> if [send notification]; otherwise, <c>false</c>.</value>
        public bool SendNotification
        {
            get { return false; }
            set {}
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
        /// <returns></returns>
        public int Save(int userId)
        {
            return 0;
        }
    }

    public class ReadonlySection : ISection
    {
        /// <summary>
        /// ResponseHeaderId
        /// </summary>
        public int ResponseHeaderId
        {
            get;
            set;
        }

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
                           select
                                   new ReadonlyQuestion {
                                               QuestionId = s.QuestionId,
                                               Text = s.QuestionText,
                                               Comments = s.Comments,
                                               RelativeOrder = s.QuestionRelativeOrder,
                                               ControlType = s.ControlType,
                                               SectionId = s.SectionId,
                                               ResponseHeaderId = s.ResponseHeaderId
                                       }).Distinct();

            List<IQuestion> questions = new List<IQuestion>();
            foreach (ReadonlyQuestion q in results)
            {
                if (q.GetAnswers().Count == 0)
                {
                    //Special case, these are open ended questions with no rows in the asnwer table. LargeTextInputField or SmallTextInputField
                    q.Responses = new List<UserResponse>();
                    //fetch the open ended response since we can't include in distinct list above.
                    ReadonlyQuestion question = q;
                    var result = (context.Responses.Where(r => r.ResponseHeaderId == this.ResponseHeaderId && r.QuestionId == question.QuestionId)).FirstOrDefault();
                    UserResponse response = new UserResponse { RelationshipKey = q.RelationshipKey, AnswerValue = result.UserResponse };
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
        /// <returns>An IQuestion using the passed key.</returns>
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
        /// Renders the specified ph.
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

    public class ReadonlyQuestion : IQuestion
    {
        /// <summary>
        /// ResponseHeaderId
        /// </summary>
        public int ResponseHeaderId
        {
            get;
            set;
        }

        /// <summary>
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
        /// Gets the section.
        /// </summary>
        /// <value>The section.</value>
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

        /// <summary>
        /// Gets the answer choices.
        /// </summary>
        /// <value>The answer choices.</value>
        public List<IAnswer> GetAnswers()
        {

            //Initialize the responses
            this.Responses = new List<UserResponse>();

            SurveyModelDataContext context = SurveyModelDataContext.Instance;
            var results = (from s in context.Responses
                           where s.ResponseHeaderId == this.ResponseHeaderId && s.QuestionId == this.QuestionId
                           select
                                   new ReadonlyAnswer {
                                               AnswerId = s.AnswerId.GetValueOrDefault(0),
                                               Text = s.AnswerText,
                                               RelativeOrder = s.AnswerRelativeOrder.GetValueOrDefault(0),
                                               IsCorrect = s.AnswerIsCorrect.GetValueOrDefault(false),
                                               SectionId = s.SectionId,
                                               QuestionId = s.QuestionId,
                                               ResponseHeaderId = s.ResponseHeaderId,
                                               AnswerValue =  s.UserResponse
                                       }).Distinct();

            List<IAnswer> answers = new List<IAnswer>();
            foreach (ReadonlyAnswer a in results)
            {
                //while we are here, load the UserResponse.
                UserResponse response = new UserResponse { RelationshipKey = a.RelationshipKey, AnswerValue = a.AnswerValue };
                this.Responses.Add(response);    
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
            get;
            set;
        }

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
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        public string Comments
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is required.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is required; otherwise, <c>false</c>.
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
        /// Gets the answer choice.
        /// </summary>
        /// <param name="key">The key.</param>
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
    }

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
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the question for this answer.
        /// </summary>
        /// <returns></returns>
// ReSharper disable UnusedMember.Local
        private IQuestion GetQuestion()
// ReSharper restore UnusedMember.Local
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
        /// Gets or sets the answer id.
        /// </summary>
        /// <value>The answer id.</value>
        public int AnswerId
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
        /// 	<c>true</c> if this instance is correct; otherwise, <c>false</c>.
        /// </value>
        public bool IsCorrect
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the rendering key used by the SurveyControl to uniquely identify this element.
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
    }
}
