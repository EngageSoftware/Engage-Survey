// <copyright file="SurveyListing.ascx.cs" company="Engage Software">
// Engage: Survey
// Copyright (c) 2004-2014
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
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using DotNetNuke.Services.Exceptions;
    using Engage.Survey;
    using Engage.Survey.Entities;

    /// <summary>
    /// Lists this module's surveys, with links to edit the surveys and view responses
    /// </summary>
    public partial class SurveyListing : ModuleBase
    {
        /// <summary>
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            this.NewSurveyButton.Click += this.NewSurveyButton_Click;
            this.SurveyGrid.ItemDataBound += this.SurveyDataGrid_OnItemDataBound;
            
            this.ActionButtonsPlaceholder.Visible = this.IsAdmin;
            
            base.OnInit(e);
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        {
            var surveys = new SurveyRepository().LoadSurveys(this.ModuleId, this.IsAdmin);

            // Hides surveys with no questions from non-administrators
            if (!this.IsEditable)
            {
                surveys = surveys.Where(s => s.Sections.Any(section => section.Questions.Any()));
            }

            this.SurveyGrid.DataSource = surveys;
            this.SurveyGrid.DataBind();
        }

        /// <summary>
        /// Builds the URL to analyze the responses for the given survey.
        /// </summary>
        /// <param name="surveyId">The survey ID.</param>
        /// <returns>A URL which takes the user to a page to analyze the responses to the survey with the given <paramref name="surveyId"/></returns>
        private string BuildAnalyzeUrl(int surveyId)
        {
            return this.BuildLinkUrl(this.ModuleId, ControlKey.Analyze, "surveyId=" + surveyId.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Builds the URL to edit the given survey.
        /// </summary>
        /// <param name="surveyId">The survey ID.</param>
        /// <returns>A URL which takes the user to a page to edit the survey with the given <paramref name="surveyId"/></returns>
        private string BuildEditUrl(int surveyId)
        {
            return BuildLinkUrl(this.ModuleId, "EditSurvey", "surveyId=" + surveyId.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Builds the URL to create a new survey.
        /// </summary>
        /// <returns>A URL which takes the user to a page to create a new survey</returns>
        private string BuildNewUrl()
        {
            return BuildLinkUrl(this.ModuleId, "EditSurvey");
        }

        /// <summary>
        /// Build the URL to the read-only preview of the given survey.
        /// </summary>
        /// <param name="surveyId">The survey ID.</param>
        /// <returns>A URL to a read-only preview of the survey with the given <paramref name="surveyId"/></returns>
        private string BuildPreviewUrl(int surveyId)
        {
            return BuildLinkUrl(
                    this.ModuleId,
                    "ViewSurvey",
                    "SurveyId=" + surveyId.ToString(CultureInfo.InvariantCulture),
                    "returnurl=" + HttpUtility.UrlEncode(this.Request.RawUrl));
        }

        /// <summary>
        /// Handles the <see cref="LinkButton.Click"/> event of the <see cref="NewSurveyButton"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void NewSurveyButton_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(this.BuildNewUrl());
        }

        /// <summary>
        /// Handles the <see cref="Control.Load"/> event of this control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    this.BindData();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Handles the <see cref="Repeater.ItemDataBound"/> event of the <see cref="SurveyGrid"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridItemEventArgs"/> instance containing the event data.</param>
        private void SurveyDataGrid_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
            {
                return;
            }

            var survey = (ISurvey)e.Item.DataItem;

            var textHyperLink = e.Item.FindControl("TextHyperLink") as HyperLink;
            if (textHyperLink != null)
            {
                textHyperLink.Text = survey.Text;
                textHyperLink.NavigateUrl = this.BuildPreviewUrl(survey.SurveyId);
            }

            var descriptionLabel = e.Item.FindControl("DescriptionLabel") as Label;
            if (descriptionLabel != null)
            {
                descriptionLabel.Text = HttpUtility.HtmlEncode(survey.GetSections().First().Text).Replace("\n", "<br />");
            }

            var editHyperLink = e.Item.FindControl("EditHyperLink") as HyperLink;
            if (editHyperLink != null)
            {
                editHyperLink.NavigateUrl = this.BuildEditUrl(survey.SurveyId);
                editHyperLink.Visible = this.IsEditable;
            }

            var analyzeLink = e.Item.FindControl("AnalyzeLink") as HyperLink;
            if (analyzeLink != null)
            {
                analyzeLink.NavigateUrl = BuildAnalyzeUrl(survey.SurveyId);
                analyzeLink.Visible = this.IsEditable;
            }
        }
    }
}