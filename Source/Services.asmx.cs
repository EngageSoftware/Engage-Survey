// <copyright file="Services.asmx.cs" company="Engage Software">
// Engage: Survey
// Copyright (c) 2004-2009
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

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
    /// Web Services for Engage: Survey
    /// </summary>
    [WebService(Namespace = "http://services.engagesoftware.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    public class Services : WebService
    {
        /// <summary>
        /// Gets a completed survey.
        /// </summary>
        /// <param name="responseHeaderId">The response header id.</param>
        /// <returns>A <see cref="ReadonlySurvey"/> instance</returns>
        [WebMethod]
        public ReadonlySurvey GetCompletedSurvey(int responseHeaderId)
        {
            return (ReadonlySurvey)ReadonlySurvey.LoadSurvey(responseHeaderId);
        }

        /// <summary>
        /// Gets the survey.
        /// </summary>
        /// <param name="surveyId">The survey id.</param>
        /// <returns>A <see cref="Survey"/> instance</returns>
        [WebMethod]
        public Survey GetSurvey(int surveyId)
        {
            return (Survey)Survey.LoadSurvey(surveyId);
        }
        
        /// <summary>
        /// Gets all surveys.
        /// </summary>
        /// <returns>A <see cref="List{Survey}"/> of all <see cref="Survey"/>s</returns>
        [WebMethod]
        public List<Survey> GetSurveys()
        {
            return Survey.LoadSurveys().ToList();
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
                surveyToUpdate.RevisionDate = surveyToUpdate.Sections[0].RevisionDate = DateTime.Now;
            }
            else
            {
                surveyToUpdate = new Survey(survey.RevisingUser);
                dataContext.Surveys.InsertOnSubmit(surveyToUpdate);
            }

            surveyToUpdate.Text = survey.Text;
            surveyToUpdate.Sections.First().Text = survey.Sections.First().Text;

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
        public Question UpdateQuestion(int surveyId, Question question)
        {
            var now = DateTime.Now;
            var dataContext = SurveyModelDataContext.Instance;
            var survey = dataContext.Surveys.Where(s => s.SurveyId == surveyId).Single();
            Question questionToUpdate;
            if (question.QuestionId > 0)
            {
                questionToUpdate = survey.Sections.First().Questions.Where(q => q.QuestionId == question.QuestionId).Single();
                questionToUpdate.RevisingUser = question.RevisingUser;
                questionToUpdate.RevisionDate = now;
            }
            else
            {
                questionToUpdate = new Question(question.RevisingUser);
                survey.Sections[0].Questions.Add(questionToUpdate);
            }

            questionToUpdate.Text = question.Text;
            questionToUpdate.RelativeOrder = question.RelativeOrder;
            questionToUpdate.ControlType = question.ControlType;

            int answerOrder = 0;
            foreach (var answer in question.Answers)
            {
                Answer answerToUpdate;
                if (answer.AnswerId > 0)
                {
                    answerToUpdate = questionToUpdate.Answers.Where(a => a.AnswerId == answer.AnswerId).Single();
                    answerToUpdate.RevisingUser = question.RevisingUser;
                    answerToUpdate.RevisionDate = now;
                }
                else
                {
                    answerToUpdate = new Answer(question.RevisingUser);
                    questionToUpdate.Answers.Add(answerToUpdate);
                }

                answerToUpdate.Text = answer.Text;
                answerToUpdate.RelativeOrder = answerOrder++;
            }

            dataContext.SubmitChanges();

            return questionToUpdate;
        }

        /// <summary>
        /// Reorders the questions for a given <see cref="Survey"/>.
        /// </summary>
        /// <param name="surveyId">The ID of the <see cref="Survey"/> to which the questions belong.</param>
        /// <param name="questionOrderMap">A <see cref="Dictionary{String,Int32}"/> mapping question IDs to relative order.</param>
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
    }
}
