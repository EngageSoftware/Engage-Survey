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
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using DotNetNuke.Common;
    using DotNetNuke.Services.Exceptions;
    using Engage.Survey;
    using Engage.Survey.Entities;

    /// <summary>
    /// The SurveyListing class displays the content
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
            this.CancelButton.Click += this.CancelButton_Click;
            this.SurveyGrid.ItemDataBound += this.SurveyDataGrid_OnItemDataBound;
            this.FilterRadioButtonList.SelectedIndexChanged += this.FilterRadioButtonList_SelectedIndexChanged;
            base.OnInit(e);
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        /// <param name="index">The index.</param>
        private void BindData(int index)
        {
            if (index == 0)
            {
                // bind to survey definitions
                this.SurveyGrid.DataSource = Survey.LoadSurveys();
            }
            else
            {
                this.SurveyGrid.DataSource = ReadonlySurvey.LoadSurveys();
            }

            this.SurveyGrid.DataBind();
        }

        /// <summary>
        /// Builds the URL to delete the given survey.
        /// </summary>
        /// <param name="surveyId">The survey id.</param>
        /// <returns>A URL which, when navigated to, deletes the survey with the given <paramref name="surveyId"/></returns>
        private string BuildDeleteUrl(int surveyId)
        {
            return this.BuildLinkUrl("&mid=" + this.ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=EditSurvey&delete=1&surveyId=" + surveyId);
        }

        /// <summary>
        /// Builds the URL to edit the given survey.
        /// </summary>
        /// <param name="surveyId">The survey id.</param>
        /// <returns>A URL which takes the user to a page to edit the survey with the given <paramref name="surveyId"/></returns>
        private string BuildEditUrl(int surveyId)
        {
            return this.BuildLinkUrl("&mid=" + this.ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=EditSurvey&surveyId=" + surveyId);
        }

        /// <summary>
        /// Builds the URL to create a new survey.
        /// </summary>
        /// <returns>A URL which takes the user to a page to create a new survey</returns>
        private string BuildNewUrl()
        {
            return this.BuildLinkUrl("&mid=" + this.ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=EditSurvey");
        }

        /// <summary>
        /// Build the URL to the read-only preview of the given survey.
        /// </summary>
        /// <param name="id">The id of the survey or response.</param>
        /// <param name="key">"SurveyId" or "ResponseHeaderId"</param>
        /// <returns>A URL to a read-only preview of the survey with the given <paramref name="id"/></returns>
        private string BuildPreviewUrl(int id, string key)
        {
            return this.BuildLinkUrl("&mid=" + this.ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=ViewSurvey&" + key + "=" + id);
        }

        /// <summary>
        /// Handles the <see cref="LinkButton.Click"/> event of the <see cref="CancelButton"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            // TODO: Implement ReturnURL functionality.
            this.Response.Redirect(Globals.NavigateURL());
        }

        /// <summary>
        /// Handles the <see cref="ListControl.SelectedIndexChanged"/> event of the <see cref="FilterRadioButtonList"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void FilterRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindData(this.FilterRadioButtonList.SelectedIndex);
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
                if (!this.Page.IsPostBack)
                {
                    this.BindData(0);
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Handles the <see cref="DataGrid.ItemDataBound"/> event of the <see cref="SurveyGrid"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridItemEventArgs"/> instance containing the event data.</param>
        private void SurveyDataGrid_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var survey = (ISurvey)e.Item.DataItem;
                var editHyperLink = e.Item.FindControl("EditHyperLink") as HyperLink;
                if (editHyperLink != null)
                {
                    editHyperLink.NavigateUrl = this.BuildEditUrl(survey.SurveyId);
                    editHyperLink.Visible = !survey.IsReadonly;
                }

                var previewHyperLink = e.Item.FindControl("PreviewHyperLink") as HyperLink;
                if (previewHyperLink != null)
                {
                    if (survey.IsReadonly)
                    {
                        previewHyperLink.NavigateUrl = this.BuildPreviewUrl(((ReadonlySurvey)survey).ResponseHeaderId, "ResponseHeaderId");
                    }
                    else
                    {
                        previewHyperLink.NavigateUrl = this.BuildPreviewUrl(survey.SurveyId, "SurveyId");
                    }
                }

                var textLabel = e.Item.FindControl("TextLabel") as Label;
                if (textLabel != null)
                {
                    textLabel.Text = survey.Text;
                }
            }
        }
    }
}