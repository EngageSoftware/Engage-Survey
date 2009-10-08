using System.Web.Services;

namespace Engage.Dnn.Survey
{
    using System.Linq;
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
        public Survey GetCompletedSurvey(int responseHeaderId)
        {
            SurveyModelDataContext context = SurveyModelDataContext.Instance;
            var survey = (from s in context.Surveys
                          where s.SurveyId == 1
                          select s).SingleOrDefault();
            return survey;
        }

        /// <summary>
        /// Gets the survey.
        /// </summary>
        /// <param name="surveyId">The survey id.</param>
        /// <returns></returns>
        [WebMethod]
        public Survey GetSurvey(int surveyId)
        {
            SurveyModelDataContext context = SurveyModelDataContext.Instance;
            var survey = (from s in context.Surveys
                          where s.SurveyId == surveyId
                          select s).SingleOrDefault();
            return survey;
        }
    }
}
