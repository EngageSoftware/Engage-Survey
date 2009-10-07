using System.Web.Services;

namespace Engage.Dnn.Survey
{
    using System.Linq;
    using Engage.Survey.Entities;

    /// <summary>
    /// Summary description for Services
    /// </summary>
    [WebService(Namespace = "http://services.engagesoftware.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Services : WebService
    {

        /// <summary>
        /// Gets the survey.
        /// </summary>
        /// <param name="responseHeaderId">The response header id.</param>
        /// <returns></returns>
        [WebMethod]
        public Survey GetSurvey(int responseHeaderId)
        {
            SurveyModelDataContext context = SurveyModelDataContext.Instance;
            var survey = (from s in context.Surveys
                          where s.SurveyId == 1
                          select s).SingleOrDefault();
            return survey;
        }
    }
}
