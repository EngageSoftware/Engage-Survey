// <copyright file="ClientService.asmx.cs" company="Engage Software">
// Engage: Survey
// Copyright (c) 2004-2010
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

// TODO: Authentication...
namespace Engage.Dnn.Survey
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Script.Services;
    using System.Web.Services;
    using Engage.Survey.Entities;

    /// <summary>
    /// Web services used on the client
    /// </summary>
    [WebService(Namespace = "http://services.engagesoftware.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    public class ClientService : WebService
    {
        /// <summary>
        /// Deletes the question.
        /// </summary>
        /// <param name="questionId">The question id.</param>
        [WebMethod]
        public void DeleteQuestion(int questionId)
        {
            var dataContext = SurveyModelDataContext.Instance;

            var question = dataContext.Questions.Where(q => q.QuestionId == questionId).Single();

            dataContext.Answers.DeleteAllOnSubmit(question.Answers);
            dataContext.Questions.DeleteOnSubmit(question);

            dataContext.SubmitChanges();
        }

        /// <summary>
        /// Deletes the survey.
        /// </summary>
        /// <param name="surveyId">The survey ID.</param>
        [WebMethod]
        public void DeleteSurvey(int surveyId)
        {
            var dataContext = SurveyModelDataContext.Instance;

            var survey = dataContext.Surveys.Where(s => s.SurveyId == surveyId).Single();

            dataContext.Surveys.DeleteOnSubmit(survey);
            dataContext.Sections.DeleteAllOnSubmit(survey.Sections);
            var questions = survey.Sections.SelectMany(section => section.Questions);
            dataContext.Questions.DeleteAllOnSubmit(questions);
            var answers = questions.SelectMany(question => question.Answers);
            dataContext.Answers.DeleteAllOnSubmit(answers);

            dataContext.SubmitChanges();
        }

        /// <summary>
        /// Reorders the questions for a given <see cref="Survey"/>.
        /// </summary>
        /// <param name="surveyId">The ID of the <see cref="Survey"/> to which the questions belong.</param>
        /// <param name="questionOrderMap">A <see cref="Dictionary{TKey,TValue}"/> mapping question IDs to relative order.</param>
        [WebMethod]
        public void ReorderQuestions(int surveyId, Dictionary<string, int> questionOrderMap)
        {
            var dataContext = SurveyModelDataContext.Instance;
            var survey = dataContext.Surveys.Where(s => s.SurveyId == surveyId).Single();

            foreach (var questionIdOrderPair in questionOrderMap)
            {
                var questionId = int.Parse(questionIdOrderPair.Key, CultureInfo.InvariantCulture);
                var relativeOrder = questionIdOrderPair.Value;
                survey.Sections[0].Questions.Where(q => q.QuestionId == questionId).Single().RelativeOrder = relativeOrder;
            }

            dataContext.SubmitChanges();
        }

        /// <summary>
        /// Inserts or updates the given <paramref name="survey"/>.
        /// </summary>
        /// <param name="survey">The survey.</param>
        /// <returns>The ID of the survey</returns>
        [WebMethod]
        public int UpdateSurvey(Survey survey)
        {
            Survey surveyToUpdate;
            var dataContext = SurveyModelDataContext.Instance;
            if (survey.SurveyId > 0)
            {
                surveyToUpdate = dataContext.Surveys.Where(s => s.SurveyId == survey.SurveyId).Single();
                surveyToUpdate.RevisingUser = surveyToUpdate.Sections[0].RevisingUser = survey.RevisingUser;
            }
            else
            {
                surveyToUpdate = new Survey(survey.RevisingUser)
                                     {
                                             PortalId = survey.PortalId, 
                                             ModuleId = survey.ModuleId
                                     };
                dataContext.Surveys.InsertOnSubmit(surveyToUpdate);
            }

            // TODO: store dates in UTC
            surveyToUpdate.Text = survey.Text;
            surveyToUpdate.PortalId = survey.PortalId;
            surveyToUpdate.ModuleId = survey.ModuleId;
            surveyToUpdate.ShowText = true;
            surveyToUpdate.StartDate = survey.StartDate; ////.HasValue ? survey.StartDate.Value.ToUniversalTime() : (DateTime?)null;
            surveyToUpdate.PreStartMessage = survey.PreStartMessage;
            surveyToUpdate.EndDate = survey.EndDate; ////.HasValue ? survey.EndDate.Value.ToUniversalTime() : (DateTime?)null;
            surveyToUpdate.PostEndMessage = survey.PostEndMessage;
            surveyToUpdate.SendNotification = survey.SendNotification;
            surveyToUpdate.NotificationFromEmailAddress = survey.NotificationFromEmailAddress;
            surveyToUpdate.NotificationToEmailAddresses = survey.NotificationToEmailAddresses;
            surveyToUpdate.SendThankYou = survey.SendThankYou;
            surveyToUpdate.ThankYouFromEmailAddress = survey.ThankYouFromEmailAddress;
            surveyToUpdate.Sections.First().Text = survey.Sections.First().Text;
            surveyToUpdate.Sections.First().ShowText = true;

            dataContext.SubmitChanges();

            return surveyToUpdate.SurveyId;
        }

        /// <summary>
        /// Inserts or updates the give <paramref name="question"/>.
        /// </summary>
        /// <param name="surveyId">The ID of the <see cref="Survey"/> that <paramref name="question"/> belongs to.</param>
        /// <param name="question">The question.</param>
        /// <returns>
        /// The inserted question, with IDs for the question and answers
        /// </returns>
        [WebMethod]
        public object UpdateQuestion(int surveyId, Question question)
        {
            var dataContext = SurveyModelDataContext.Instance;
            var survey = dataContext.Surveys.Where(s => s.SurveyId == surveyId).Single();
            Question questionToUpdate;
            if (question.QuestionId > 0)
            {
                questionToUpdate = survey.Sections.First().Questions.Where(q => q.QuestionId == question.QuestionId).Single();
                questionToUpdate.RevisingUser = question.RevisingUser;
            }
            else
            {
                questionToUpdate = new Question(question.RevisingUser);
                survey.Sections[0].Questions.Add(questionToUpdate);
            }

            questionToUpdate.Text = question.Text;
            questionToUpdate.IsRequired = question.IsRequired;
            questionToUpdate.RelativeOrder = question.RelativeOrder;
            questionToUpdate.ControlType = question.ControlType;

            foreach (var answer in questionToUpdate.Answers)
            {
                var lambdaAnswer = answer;
                if (!question.Answers.Any(a => a.AnswerId == lambdaAnswer.AnswerId))
                {
                    dataContext.Answers.DeleteOnSubmit(answer);
                }
            }

            int answerOrder = 0;
            foreach (var answer in question.Answers)
            {
                Answer answerToUpdate;
                if (answer.AnswerId > 0)
                {
                    var lambdaAnswer = answer;
                    answerToUpdate = questionToUpdate.Answers.Where(a => a.AnswerId == lambdaAnswer.AnswerId).Single();
                    answerToUpdate.RevisingUser = question.RevisingUser;
                }
                else
                {
                    answerToUpdate = new Answer(question.RevisingUser);
                    questionToUpdate.Answers.Add(answerToUpdate);
                }

                answerToUpdate.Text = answer.Text;
                answerToUpdate.RelativeOrder = ++answerOrder;
            }

            dataContext.SubmitChanges();

            return new
            {
                questionToUpdate.QuestionId,
                questionToUpdate.ControlType,
                questionToUpdate.RelativeOrder,
                questionToUpdate.Text,
                questionToUpdate.IsRequired,
                Answers = questionToUpdate.Answers.OrderBy(a => a.RelativeOrder).Select(a => new { a.AnswerId, a.RelativeOrder, a.Text })
            };
        }
    }
}
