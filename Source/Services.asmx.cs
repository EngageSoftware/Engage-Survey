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
    }
}
