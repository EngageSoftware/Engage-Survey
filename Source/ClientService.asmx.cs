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
            new SurveyRepository().DeleteQuestion(questionId);
        }

        /// <summary>
        /// Deletes the survey.
        /// </summary>
        /// <param name="surveyId">The survey ID.</param>
        [WebMethod]
        public void DeleteSurvey(int surveyId)
        {
            new SurveyRepository().DeleteSurvey(surveyId);
        }

        /// <summary>
        /// Reorders the questions for a given <see cref="Survey"/>.
        /// </summary>
        /// <param name="surveyId">The ID of the <see cref="Survey"/> to which the questions belong.</param>
        /// <param name="questionOrderMap">A <see cref="Dictionary{TKey,TValue}"/> mapping question IDs to relative order.</param>
        [WebMethod]
        public void ReorderQuestions(int surveyId, Dictionary<string, int> questionOrderMap)
        {
            var surveyRepository = new SurveyRepository();
            var survey = surveyRepository.LoadSurvey(surveyId);

            foreach (var questionIdOrderPair in questionOrderMap)
            {
                var questionId = int.Parse(questionIdOrderPair.Key, CultureInfo.InvariantCulture);
                var relativeOrder = questionIdOrderPair.Value;
                survey.Sections[0].Questions.Where(q => q.QuestionId == questionId).Single().RelativeOrder = relativeOrder;
            }

            surveyRepository.SubmitChanges();
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
            var surveyRepository = new SurveyRepository();
            if (survey.SurveyId > 0)
            {
                surveyToUpdate = surveyRepository.LoadSurvey(survey.SurveyId);
                surveyToUpdate.RevisingUser = surveyToUpdate.Sections[0].RevisingUser = survey.RevisingUser;
            }
            else
            {
                surveyToUpdate = surveyRepository.CreateSurvey(survey.RevisingUser, survey.PortalId, survey.ModuleId);
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

            surveyRepository.SubmitChanges();

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
            var surveyRepository = new SurveyRepository();
            var survey = surveyRepository.LoadSurvey(surveyId);
            Question questionToUpdate;
            if (question.QuestionId > 0)
            {
                questionToUpdate = survey.Sections.First().Questions.Where(q => q.QuestionId == question.QuestionId).Single();
                questionToUpdate.RevisingUser = question.RevisingUser;
            }
            else
            {
                questionToUpdate = surveyRepository.CreateQuestion(question.RevisingUser);
                survey.Sections.First().Questions.Add(questionToUpdate);
            }

            questionToUpdate.Text = question.Text;
            questionToUpdate.IsRequired = question.IsRequired;
            questionToUpdate.RelativeOrder = question.RelativeOrder;
            questionToUpdate.ControlType = question.ControlType;

            var answersToDelete = from answer in questionToUpdate.Answers
                                  where !question.Answers.Any(a => a.AnswerId == answer.AnswerId)
                                  select answer;
            surveyRepository.DeleteAnswers(answersToDelete, true);

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
                    answerToUpdate = surveyRepository.CreateAnswer(question.RevisingUser);
                    questionToUpdate.Answers.Add(answerToUpdate);
                }

                answerToUpdate.Text = answer.Text;
                answerToUpdate.RelativeOrder = ++answerOrder;
            }

            surveyRepository.SubmitChanges();

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
