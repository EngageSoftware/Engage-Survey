// <copyright file="SurveyRepository.cs" company="Engage Software">
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
    using System.Linq;

    using DotNetNuke.Common.Utilities;

    /// <summary>
    /// Provides access to store and retrieve surveys and related types
    /// </summary>
    public class SurveyRepository
    {
        /// <summary>
        /// The path to the resource file in which localized content for this class exists
        /// </summary>
        public static readonly string SharedResourceFile = "~/DesktopModules/EngageSurvey/App_LocalResources/SharedResources.resx";

        /// <summary>
        /// Initializes a new instance of the <see cref="SurveyRepository"/> class.
        /// </summary>
        public SurveyRepository()
            : this(SurveyModelDataContext.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SurveyRepository"/> class.
        /// </summary>
        /// <param name="context">The data context.</param>
        protected SurveyRepository(SurveyModelDataContext context)
        {
            this.Context = context;
        }

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        /// <value>The data context.</value>
        protected SurveyModelDataContext Context
        {
            get;
            set;
        }

        /// <summary>
        /// Checks the database to determine if a user has taken the specified survey.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="surveyId">The id of the survey.</param>
        /// <returns>True if the user has taken the survey, false if not</returns>
        public bool UserHasTaken(int userId, int surveyId)
        {
            return this.LoadResponses(surveyId).Any(responseHeader => responseHeader.Key.UserId == userId);
        }

        /// <summary>
        /// Creates a new <see cref="Answer"/> instance, to be persisted when <see cref="SubmitChanges"/> is called.
        /// </summary>
        /// <param name="userId">The ID of the user creating the instance.</param>
        /// <returns>The new <see cref="Answer"/> instance</returns>
        public Answer CreateAnswer(int userId)
        {
            var answer = new Answer(userId);

            this.Context.Answers.InsertOnSubmit(answer);

            return answer;
        }

        /// <summary>
        /// Creates a new <see cref="Question"/> instance, to be persisted when <see cref="SubmitChanges"/> is called.
        /// </summary>
        /// <param name="userId">The ID of the user creating the instance.</param>
        /// <returns>The new <see cref="Question"/> instance</returns>
        public Question CreateQuestion(int userId)
        {
            var question = new Question(userId);

            this.Context.Questions.InsertOnSubmit(question);

            return question;
        }

        /// <summary>
        /// Creates a new <see cref="Response"/> instance, to be persisted when <see cref="SubmitChanges"/> is called.
        /// </summary>
        /// <param name="responseHeaderId">The ID of the <see cref="ResponseHeader"/> that the instance is connected to.</param>
        /// <param name="userId">The ID of the user creating the instance.</param>
        /// <returns>The new <see cref="Response"/> instance</returns>
        public Response CreateResponse(int responseHeaderId, int userId)
        {
            var response = new Response
                               {
                                       ResponseHeaderId = responseHeaderId, 
                                       CreatedBy = userId, 
                                       RevisingUser = userId,
                                       CreationDate = DateTime.Now,
                                       RevisionDate = DateTime.Now
                               };

            this.Context.Responses.InsertOnSubmit(response);

            return response;
        }

        /// <summary>
        /// Creates a new <see cref="ResponseHeader"/> instance, to be persisted when <see cref="SubmitChanges"/> is called.
        /// </summary>
        /// <param name="userId">The ID of the user creating the instance.</param>
        /// <returns>The new <see cref="ResponseHeader"/> instance</returns>
        public ResponseHeader CreateResponseHeader(int userId)
        {
            var header = new ResponseHeader
                             {
                                     CreatedBy = userId, 
                                     RevisingUser = userId, 
                                     UserId = userId, 
                                     RevisionDate = DateTime.Now, 
                                     CreationDate = DateTime.Now
                             };

            this.Context.ResponseHeaders.InsertOnSubmit(header);

            return header;
        }

        /// <summary>
        /// Creates a new <see cref="Survey"/> instance, to be persisted when <see cref="SubmitChanges"/> is called.
        /// </summary>
        /// <param name="userId">The ID of the user creating the instance.</param>
        /// <param name="portalId">The ID of the survey's portal.</param>
        /// <param name="moduleId">The ID of the module that owns the survey.</param>
        /// <returns>The new <see cref="Survey"/> instance</returns>
        public Survey CreateSurvey(int userId, int portalId, int moduleId)
        {
            var createSurvey = new Survey(userId)
                                   {
                                           PortalId = portalId, 
                                           ModuleId = moduleId
                                   };

            this.Context.Surveys.InsertOnSubmit(createSurvey);

            return createSurvey;
        }

        /// <summary>
        /// Deletes the specified answers.
        /// </summary>
        /// <param name="answers">The answers to delete.</param>
        /// <param name="pendForNextSubmit">
        /// if set to <c>true</c> pends the deletion to occur at the next call to <see cref="SubmitChanges"/>;
        /// otherwise, executes the deletion immediately (by calling <see cref="SubmitChanges"/>).</param>
        public void DeleteAnswers(IEnumerable<Answer> answers, bool pendForNextSubmit)
        {
            this.Context.Answers.DeleteAllOnSubmit(answers);

            if (!pendForNextSubmit)
            {
                this.SubmitChanges();
            }
        }

        /// <summary>
        /// Deletes the specified question.
        /// </summary>
        /// <param name="questionId">The ID of the question.</param>
        public void DeleteQuestion(int questionId)
        {
            var question = this.Context.Questions.Where(q => q.QuestionId == questionId).Single();

            this.Context.Answers.DeleteAllOnSubmit(question.Answers);
            this.Context.Questions.DeleteOnSubmit(question);

            this.Context.SubmitChanges();
        }

        /// <summary>
        /// Deletes the specified completed survey <see cref="ResponseHeader"/> and responses.
        /// </summary>
        /// <param name="responseHeaderId">The <see cref="ResponseHeader"/> ID</param>
        public void DeleteReadOnlySurvey(int? responseHeaderId)
        {
            var responseHeader = (from rh in this.Context.ResponseHeaders 
                                  where rh.ResponseHeaderId == responseHeaderId
                                  select rh).Single();

            this.Context.ResponseHeaders.DeleteOnSubmit(responseHeader);
            this.Context.Responses.DeleteAllOnSubmit(responseHeader.Responses);

            this.Context.SubmitChanges();
        }

        /// <summary>
        /// Deletes the specified survey.
        /// </summary>
        /// <param name="surveyId">The ID of the survey to delete.</param>
        /// <param name="moduleId">The ID of the module in which to find the survey.</param>
        public void DeleteSurvey(int surveyId, int moduleId)
        {
            var survey = this.LoadSurvey(surveyId, moduleId);

            this.Context.Surveys.DeleteOnSubmit(survey);
            this.Context.Sections.DeleteAllOnSubmit(survey.Sections);
            var questions = survey.Sections.SelectMany(section => section.Questions);
            this.Context.Questions.DeleteAllOnSubmit(questions);
            var answers = questions.SelectMany(question => question.Answers);
            this.Context.Answers.DeleteAllOnSubmit(answers);
            var responses = this.Context.Responses.Where(response => response.SurveyId == surveyId);
            this.Context.Responses.DeleteAllOnSubmit(responses);
            var responseHeaders = responses.Select(response => response.ResponseHeader);
            this.Context.ResponseHeaders.DeleteAllOnSubmit(responseHeaders);

            this.Context.SubmitChanges();
        }

        /// <summary>
        /// Gets the ID of the module that the given question belongs to.
        /// </summary>
        /// <param name="questionId">The question's ID.</param>
        /// <returns>The ID of the module, or <see cref="Null.NullInteger"/> if the question doesn't exist</returns>
        public int GetModuleIdForQuestion(int questionId)
        {
            var moduleId = (from question in this.Context.Questions
                            where question.QuestionId == questionId
                            select question.Section.Survey.ModuleId).SingleOrDefault();

            return moduleId == default(int) ? Null.NullInteger : moduleId;
        }

        /// <summary>
        /// Gets the ID of the module that the given survey belongs to.
        /// </summary>
        /// <param name="surveyId">The survey's ID.</param>
        /// <returns>The ID of the module, or <see cref="Null.NullInteger"/> if the survey doesn't exist</returns>
        public int GetModuleIdForSurvey(int surveyId)
        {
            var moduleId = (from survey in this.Context.Surveys
                            where survey.SurveyId == surveyId
                            select survey.ModuleId).SingleOrDefault();

            return moduleId == default(int) ? Null.NullInteger : moduleId;
        }

        /// <summary>
        /// Loads all of the questions, their answers, and the number of responses per answer for the given survey
        /// </summary>
        /// <param name="surveyId">The ID of the survey.</param>
        /// <returns>
        /// A sequence of <see cref="Pair{TFirst,TSecond}"/>s mapping <see cref="Question"/> instances to the list of that question's <see cref="Answer"/>s, 
        /// paired with the number of responses for the answer
        /// </returns>
        public IQueryable<Pair<Question, IEnumerable<Pair<Answer, int>>>> LoadAnswerResponseCounts(int surveyId)
        {
            return from answer in this.Context.Answers
                   join question in this.Context.Questions on answer.QuestionId equals question.QuestionId
                   join section in this.Context.Sections on question.SectionId equals section.SectionId
                   where section.SurveyId == surveyId
                   orderby answer.RelativeOrder
                   group answer by question into answersByQuestion 
                   orderby answersByQuestion.Key.RelativeOrder
                   select new Pair<Question, IEnumerable<Pair<Answer, int>>>
                           {
                               First = answersByQuestion.Key,
                               Second = from answer in answersByQuestion
                                        join response in this.Context.Responses.Where(r => r.UserResponse != null)
                                            on answer.AnswerId equals response.AnswerId into answerResponses
                                        from answerResponse in answerResponses.DefaultIfEmpty()
                                        group answerResponse by answer into responsesByAnswer
                                        select new Pair<Answer, int>
                                                {
                                                    First = responsesByAnswer.Key,
                                                    Second = responsesByAnswer.Count(r => r != null)
                                                }
                           };
        }

        /// <summary>
        /// Loads all of the questions for the given survey (based on its current definition)
        /// </summary>
        /// <param name="surveyId">The ID of the survey.</param>
        /// <returns>
        /// A sequence of <see cref="Question"/> instances
        /// </returns>
        public IQueryable<Question> LoadQuestions(int surveyId)
        {
            return from question in this.Context.Questions
                   join section in this.Context.Sections on question.SectionId equals section.SectionId
                   where section.SurveyId == surveyId
                   orderby question.RelativeOrder
                   select question;
        }

        /// <summary>
        /// Loads the completed answers for a given completed question.
        /// </summary>
        /// <param name="responseHeaderId">The ID of the <see cref="ResponseHeader"/> of the completed survey.</param>
        /// <param name="questionId">The ID of the question.</param>
        /// <returns>A sequence of the answers</returns>
        public IQueryable<ReadonlyAnswer> LoadReadOnlyAnswers(int responseHeaderId, int questionId)
        {
            return (from response in this.Context.Responses
                    where response.ResponseHeaderId == responseHeaderId && response.QuestionId == questionId
                    orderby response.AnswerRelativeOrder
                    select new ReadonlyAnswer 
                               {
                                       AnswerId = response.AnswerId ?? 0,
                                       Text = response.AnswerText,
                                       RelativeOrder = response.AnswerRelativeOrder ?? 0,
                                       IsCorrect = response.AnswerIsCorrect ?? false,
                                       SectionId = response.SectionId,
                                       QuestionId = response.QuestionId,
                                       ResponseHeaderId = response.ResponseHeaderId,
                                       AnswerValue = response.UserResponse
                               }).Distinct();
        }

        /// <summary>
        /// Loads the given completed question.
        /// </summary>
        /// <param name="responseHeaderId">The ID of the <see cref="ResponseHeader"/> of the completed survey.</param>
        /// <param name="questionId">The ID of the question.</param>
        /// <returns>The completed question instance, or <c>null</c> if no matching record exists</returns>
        public IQuestion LoadReadOnlyQuestion(int responseHeaderId, int questionId)
        {
            return (from response in this.Context.Responses
                    where response.ResponseHeaderId == responseHeaderId && response.QuestionId == questionId
                    select new ReadonlyQuestion
                               {
                                       QuestionId = response.QuestionId,
                                       Text = response.QuestionText,
                                       Comments = response.Comments,
                                       RelativeOrder = response.QuestionRelativeOrder,
                                       ControlType = response.ControlType,
                                       SectionId = response.SectionId,
                                       ResponseHeaderId = response.ResponseHeaderId
                               }).FirstOrDefault();
        }

        /// <summary>
        /// Loads the completed questions for a given completed section.
        /// </summary>
        /// <param name="responseHeaderId">The ID of the <see cref="ResponseHeader"/> of the completed survey.</param>
        /// <param name="sectionId">The ID of the section.</param>
        /// <returns>A sequence of the questions</returns>
        public IQueryable<ReadonlyQuestion> LoadReadOnlyQuestions(int responseHeaderId, int sectionId)
        {
            var questions = (from response in this.Context.Responses
                             where response.ResponseHeaderId == responseHeaderId && response.SectionId == sectionId
                             select new ReadonlyQuestion
                                         {
                                                 QuestionId = response.QuestionId,
                                                 Text = response.QuestionText,
                                                 Comments = response.Comments,
                                                 RelativeOrder = response.QuestionRelativeOrder,
                                                 ControlType = response.ControlType,
                                                 SectionId = response.SectionId,
                                                 ResponseHeaderId = response.ResponseHeaderId
                                         }).Distinct().OrderBy(response => response.RelativeOrder);

            ////// Special case, these are open ended questions with no rows in the answer table. LargeTextInputField or SmallTextInputField
            ////foreach (var question in Enumerable.Where(questions, question => !question.GetAnswers().Any()))
            ////{
            ////    // fetch the open ended response since we can't include in distinct list above.
            ////    var lambdaQuestion = question;
            ////    var questionResponse = (from response in this.Context.Responses
            ////                            where response.ResponseHeaderId == responseHeaderId && response.QuestionId == lambdaQuestion.QuestionId
            ////                            select new UserResponse
            ////                                       {
            ////                                               RelationshipKey = lambdaQuestion.RelationshipKey, 
            ////                                               AnswerValue = response.UserResponse
            ////                                       }).FirstOrDefault();

            ////    question.Responses = new List<UserResponse> {
            ////                                     questionResponse
            ////                             };
            ////}

            return questions;
        }

        /// <summary>
        /// Loads the given completed section.
        /// </summary>
        /// <param name="responseHeaderId">The ID of the <see cref="ResponseHeader"/> of the completed survey.</param>
        /// <param name="sectionId">The ID of the section.</param>
        /// <returns>The completed section instance, or <c>null</c> if no matching record exists</returns>
        public ReadonlySection LoadReadOnlySection(int responseHeaderId, int sectionId)
        {
            return (from response in this.Context.Responses
                    where response.ResponseHeaderId == responseHeaderId && response.SectionId == sectionId
                    select new ReadonlySection
                                {
                                        SurveyId = response.SurveyId,
                                        SectionId = response.SectionId,
                                        Text = response.SectionText,
                                        ShowText = response.ShowSectionText,
                                        RelativeOrder = response.SectionRelativeOrder,
                                        ResponseHeaderId = response.ResponseHeaderId
                                }).FirstOrDefault();
        }

        /// <summary>
        /// Loads the completed sections for a given completed survey.
        /// </summary>
        /// <param name="responseHeaderId">The ID of the <see cref="ResponseHeader"/> of the completed survey.</param>
        /// <param name="surveyId">The ID of the survey.</param>
        /// <returns>A sequence of the sections</returns>
        public IQueryable<ReadonlySection> LoadReadOnlySections(int responseHeaderId, int surveyId)
        {
            return (from response in this.Context.Responses
                    where response.ResponseHeaderId == responseHeaderId && response.SurveyId == surveyId
                    orderby response.SectionRelativeOrder
                    select new ReadonlySection
                               {
                                       SurveyId = response.SurveyId,
                                       SectionId = response.SectionId,
                                       Text = response.SectionText,
                                       ShowText = response.ShowSectionText,
                                       RelativeOrder = response.SectionRelativeOrder,
                                       ResponseHeaderId = response.ResponseHeaderId
                               }).Distinct();
        }

        /// <summary>
        /// Loads a completed survey using the <see cref="ResponseHeader"/> ID for the User/Survey.
        /// </summary>
        /// <param name="responseHeaderId">The <see cref="ResponseHeader"/> ID.</param>
        /// <param name="moduleId">The module ID.</param>
        /// <returns>The survey with the given ID</returns>
        public ReadonlySurvey LoadReadOnlySurvey(int responseHeaderId, int moduleId)
        {
            return (from response in this.LoadReadOnlySurveys()
                    join surveyDefinition in this.Context.Surveys on response.SurveyId equals surveyDefinition.SurveyId
                   where response.ResponseHeaderId == responseHeaderId
                      && surveyDefinition.ModuleId == moduleId
                   select response).SingleOrDefault();
        }

        /// <summary>
        /// Gets the responses for the given survey.
        /// </summary>
        /// <param name="surveyId">The ID of the survey for which to get resposnes.</param>
        /// <returns>A list of response headers, each associated with the list of individual question responses for that header</returns>
        public IQueryable<IGrouping<ResponseHeader, Response>> LoadResponses(int surveyId)
        {
            var responses = from response in this.Context.Responses 
                            where response.SurveyId == surveyId 
                                  && response.UserResponse != null 
                            orderby response.QuestionRelativeOrder
                            select response;

            return from response in responses
                   group response by response.ResponseHeader into responsesByHeader
                   orderby responsesByHeader.Key.CreationDate descending
                   select responsesByHeader;
        }

        /// <summary>
        /// Loads the survey.
        /// </summary>
        /// <param name="surveyId">The survey id.</param>
        /// <returns>The survey with the given ID, or <c>null</c> if no survey exists with the given ID</returns>
        public Survey LoadSurvey(int surveyId)
        {
            return this.Context.Surveys.SingleOrDefault(s => s.SurveyId == surveyId);
        }

        /// <summary>
        /// Loads the survey for the given module.
        /// </summary>
        /// <param name="surveyId">The survey ID.</param>
        /// <param name="moduleId">The module ID.</param>
        /// <returns>
        /// The survey with the given ID, or <c>null</c> if no survey exists with the given IDs
        /// </returns>
        public Survey LoadSurvey(int surveyId, int moduleId)
        {
            return this.Context.Surveys.SingleOrDefault(s => s.SurveyId == surveyId && s.ModuleId == moduleId);
        }

        /// <summary>
        /// Loads all the surveys for a particular module instance.
        /// </summary>
        /// <param name="moduleId">The ID of the module by which surveys should be filtered.</param>
        /// <param name="getOutdatedSurveys">Whether to retrieve surveys whose <see cref="Survey.StartDate"/> or <see cref="Survey.EndDate"/> is out of range for the current date</param>
        /// <returns>
        /// A sequence of the <see cref="Survey"/> instances
        /// </returns>
        public IQueryable<Survey> LoadSurveys(int moduleId, bool getOutdatedSurveys)
        {
            var surveys = this.Context.Surveys.Where(survey => survey.ModuleId == moduleId);

            if (!getOutdatedSurveys)
            {
                surveys = from survey in surveys
                          where (survey.StartDate == null || survey.StartDate <= DateTime.Now)
                             && (survey.EndDate == null || survey.EndDate > DateTime.Now)
                          select survey;
            }

            return surveys;
        }

        /// <summary>
        /// Loads all the surveys for a particular portal instance.
        /// </summary>
        /// <param name="portalId">The ID of the portal by which surveys should be filtered.</param>
        /// <returns>
        /// A sequence of the <see cref="Survey"/> instances
        /// </returns>
        public IQueryable<Survey> LoadSurveysForPortal(int portalId)
        {
            return this.Context.Surveys.Where(survey => survey.PortalId == portalId);
        }

        /// <summary>
        /// Submits all changes for connected instances to the database.
        /// </summary>
        public void SubmitChanges()
        {
            this.Context.SubmitChanges();
        }

        /// <summary>
        /// Loads all completed surveys.
        /// </summary>
        /// <returns>A list of surveys.</returns>
        private IQueryable<ReadonlySurvey> LoadReadOnlySurveys()
        {
            return (from response in this.Context.Responses
                    join responseHeader in this.Context.ResponseHeaders on response.ResponseHeaderId equals responseHeader.ResponseHeaderId
                    select new ReadonlySurvey
                               {
                                       SurveyId = response.SurveyId,
                                       Text = response.SurveyText,
                                       ShowText = response.ShowSurveyText,
                                       TitleOption = response.TitleOption,
                                       QuestionFormatOption = response.QuestionFormatOption,
                                       SectionFormatOption = response.SectionFormatOption,
                                       ResponseHeaderId = responseHeader.ResponseHeaderId,
                                       CreationDate = responseHeader.CreationDate,
                                       UserId = responseHeader.UserId
                               }).Distinct();
        }
    }
}