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
                    this.BindData(0);
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        /// <param name="index">The index.</param>
        private void BindData(int index)
        {
            if (index == 0)
            {
                //bind to survey definitions
                SurveyDataGrid.DataSource = Survey.LoadSurveys();
            }
            else
            {
                SurveyDataGrid.DataSource = ReadonlySurvey.LoadSurveys();
            }
            SurveyDataGrid.DataBind();
        }

        #endregion

        protected string BuildPreviewUrl(int id, string key)
        {
            string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=ViewSurvey&" + key + "=" + id + "");
            return href;
        }

        /// <summary>
        /// Builds the new URL.
        /// </summary>
        /// <returns></returns>
        private string BuildNewUrl()
        {
            string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=EditSurvey");
            return href;
        }

        /// <summary>
        /// Builds the edit URL.
        /// </summary>
        /// <param name="surveyId">The survey id.</param>
        /// <returns></returns>
        protected string BuildEditUrl(int surveyId)
        {
            string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=EditSurvey&surveyId=" + surveyId + "");
            return href;
        }

        /// <summary>
        /// Builds the delete URL.
        /// </summary>
        /// <param name="surveyId">The survey id.</param>
        /// <returns></returns>
        protected string BuildDeleteUrl(int surveyId)
        {
            string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=EditSurvey&delete=1&surveyId=" + surveyId + "");
            return href;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the FilterRadioButtonList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void FilterRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindData(FilterRadioButtonList.SelectedIndex);
        }

        /// <summary>
        /// Handles the OnItemDataBound event of the SurveyDataGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridItemEventArgs"/> instance containing the event data.</param>
        protected void SurveyDataGrid_OnItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ISurvey survey = (ISurvey)e.Item.DataItem;
                HyperLink editHyperLink = e.Item.FindControl("EditHyperLink") as HyperLink;
                if (editHyperLink != null)
                {
                    editHyperLink.NavigateUrl = this.BuildEditUrl(survey.SurveyId);
                    editHyperLink.Visible = !survey.IsReadonly;
                }

                HyperLink previewHyperLink = e.Item.FindControl("PreviewHyperLink") as HyperLink;
                if (previewHyperLink != null)
                {
                    if (survey.IsReadonly)
                    {
                        ReadonlySurvey s = (ReadonlySurvey)survey;
                        previewHyperLink.NavigateUrl = this.BuildPreviewUrl(s.ResponseHeaderId, "responseheaderid");
                    }
                    else
                    {
                        previewHyperLink.NavigateUrl = this.BuildPreviewUrl(survey.SurveyId, "surveyId");    
                    }
                }

                Label textLabel = e.Item.FindControl("TextLabel") as Label;
                if (textLabel != null)
                {
                    textLabel.Text = survey.Text;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the NewLinkButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void NewLinkButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(this.BuildNewUrl());
        }

        /// <summary>
        /// Handles the Click event of the BackLinkButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void CancelLinkButton_Click(object sender, EventArgs e)
        {
            ////TODO: Implement ReturnURL functionality.
            Response.Redirect(Globals.NavigateURL());
        }
    }
}

