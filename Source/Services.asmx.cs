using System.Web.Services;

namespace Engage.Dnn.Survey
{
    using System.Collections.Generic;
    using System.Web.Script.Services;
    using Engage.Survey.Entities;

    /// <summary>
    /// Summary description for Services
    /// </summary>
    [WebService(Namespace = "http://services.engagesoftware.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Services : WebService
    {
        /// <summary>
        /// Gets a completed survey.
        /// </summary>
        /// <param name="responseHeaderId">The response header id.</param>
        /// <returns></returns>
        [WebMethod]
        public ReadonlySurvey GetCompletedSurvey(int responseHeaderId)
        {
            return (ReadonlySurvey)ReadonlySurvey.LoadSurvey(responseHeaderId);
        }

        /// <summary>
        /// Gets the survey.
        /// </summary>
        /// <param name="surveyId">The survey id.</param>
        /// <returns></returns>
        [WebMethod]
        public Survey GetSurvey(int surveyId)
        {
            return (Survey)Survey.LoadSurvey(surveyId);
        }
        
        /// <summary>
        /// Gets the surveys.
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public List<Survey> GetSurveys()
        {
            List<Survey> surveys = new List<Survey>(Survey.LoadSurveys());
            return surveys;
        }
    }
}
