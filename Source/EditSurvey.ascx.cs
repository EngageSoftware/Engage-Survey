// <copyright file="EditSurvey.ascx.cs" company="Engage Software">
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
    using System.Globalization;
    using System.Web.Script.Serialization;
    using System.Web.UI;
    using Engage.Survey.Entities;

    /// <summary>
    /// The EditSurvey class is used to manage content
    /// </summary>
    public partial class EditSurvey : ModuleBase
    {
        /// <summary>
        /// Backing function for <see cref="SurveyId"/>
        /// </summary>
        private int? surveyId;

        /// <summary>
        /// Gets the survey id, or <c>null</c> if creating a new survey.
        /// </summary>
        /// <value>The survey id.</value>
        protected int? SurveyId
        {
            get
            {
                if (this.surveyId == null)
                {
                    int value;
                    if (int.TryParse(this.Request.QueryString["surveyId"], NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
                    {
                        this.surveyId = value;
                    }
                }

                return this.surveyId;
            }
        }

        /// <summary>
        /// Gets the survey serialized to a JSON object, or <c>"null"</c> if there is no survey.
        /// </summary>
        /// <value>The serialized survey.</value>
        protected string SerializedSurvey
        {
            get
            {
                var survey = this.SurveyId == null ? null : Survey.LoadSurvey(this.SurveyId.Value);
                return new JavaScriptSerializer().Serialize(survey);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);
        }

        /// <summary>
        /// Handles the <see cref="Control.Load"/> event of this control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            this.AddJQueryReference();
            this.Page.ClientScript.RegisterClientScriptResource(typeof(EditSurvey), "Engage.Dnn.Survey.JavaScript.survey.js");
        }
    }
}