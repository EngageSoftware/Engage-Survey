// <copyright file="SurveyListing.ascx.cs" company="Engage Software">
// Engage: ContactUs - http://www.engagesoftware.com
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
    using System.Globalization;
    using System.Linq;
    using DotNetNuke.Services.Exceptions;
    using Engage.Survey.Entities;

    /// <summary>
    /// The SurveyListing class displays the content
    /// </summary>
    public partial class SurveyListing : ModuleBase
    {
        #region Event Handlers

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(Object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    SurveyModelDataContext context = SurveyModelDataContext.Instance;
                    
                    //bind to survey's
                    SurveyDataGrid.DataSource = context.Surveys;
                    SurveyDataGrid.DataBind();
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
        
        #endregion

        protected string BuildPreviewUrl(object surveyId)
        {
            string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=ViewSurvey&surveyId=" + surveyId + "");
            return href;
        }

        protected string BuildEditUrl(object surveyId)
        {
            string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=EditSurvey&surveyId=" + surveyId + "");
            return href;
        }
    }
}

