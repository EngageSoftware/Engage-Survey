// <copyright file="SurveyListing.ascx.cs" company="Engage Software">
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
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using DotNetNuke.Entities.Users;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.UI.Utilities;
    using Engage.Survey;
    using Engage.Survey.Entities;

    /// <summary>
    /// The SurveyListing class displays the content
    /// </summary>
    public partial class SurveyListing : ModuleBase
    {
        private enum ListingMode
        {
            Definition,
            Completed
        }

        /// <summary>
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            this.NewSurveyButton.Click += this.NewSurveyButton_Click;
            this.SurveyGrid.ItemDataBound += this.SurveyDataGrid_OnItemDataBound;
            this.FilterRadioButtonList.SelectedIndexChanged += this.FilterRadioButtonList_SelectedIndexChanged;
            this.FilterRadioButtonList.Visible = this.IsAdmin;
            this.NewSurveyButton.Visible = this.IsAdmin;
            base.OnInit(e);
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        /// <param name="mode">The listing mode.</param>
        private void BindData(ListingMode mode)
        {
            if (mode == ListingMode.Definition)
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
        /// <param name="id">The id.</param>
        /// <param name="key">The key.</param>
        /// <returns>
        /// A URL which, when navigated to, deletes the survey with the given <paramref name="id"/>
        /// </returns>
        private string BuildDeleteUrl(int id, string key)
        {
            return BuildLinkUrl(this.ModuleId, string.Empty, "delete=1", key + "=" + id);
        }

        /// <summary>
        /// Builds the URL to edit the given survey.
        /// </summary>
        /// <param name="surveyId">The survey id.</param>
        /// <returns>A URL which takes the user to a page to edit the survey with the given <paramref name="surveyId"/></returns>
        private string BuildEditUrl(int surveyId)
        {
            return BuildLinkUrl(this.ModuleId, "EditSurvey", "surveyId=" + surveyId);
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
        /// <param name="id">The id of the survey or response.</param>
        /// <param name="key">"SurveyId" or "ResponseHeaderId"</param>
        /// <returns>A URL to a read-only preview of the survey with the given <paramref name="id"/></returns>
        private string BuildPreviewUrl(int id, string key)
        {
            return BuildLinkUrl(this.ModuleId, "ViewSurvey", key + "=" + id + "&returnurl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl));
        }

        /// <summary>
        /// Handles the <see cref="ListControl.SelectedIndexChanged"/> event of the <see cref="FilterRadioButtonList"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void FilterRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindData(this.SelectedListingMode);
        }

        private ListingMode SelectedListingMode
        {
            get
            {
                if (Enum.IsDefined(typeof(ListingMode), this.FilterRadioButtonList.SelectedValue))
                {
                    return (ListingMode)Enum.Parse(typeof(ListingMode), this.FilterRadioButtonList.SelectedValue);
                }

                return ListingMode.Definition;
            }
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
                    if (Request.QueryString["delete"] != null)
                    {
                        this.DeleteItem();
                    }

                    this.BindData(0);
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Deletes either a survey definition or a completed survey.
        /// </summary>
        private void DeleteItem()
        {
            if (this.ResponseHeaderId == null)
            {
                Survey.Delete(this.SurveyId);    
            }
            else
            {
                ReadonlySurvey.Delete(this.ResponseHeaderId);
            }
        }

        /// <summary>
        /// Handles the <see cref="Repeater.ItemDataBound"/> event of the <see cref="SurveyGrid"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridItemEventArgs"/> instance containing the event data.</param>
        private void SurveyDataGrid_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var survey = (ISurvey)e.Item.DataItem;
                var completedSurvey = survey as ReadonlySurvey;
                var surveyIsComplete = completedSurvey != null;

                var editHyperLink = e.Item.FindControl("EditHyperLink") as HyperLink;
                if (editHyperLink != null)
                {
                    editHyperLink.NavigateUrl = this.BuildEditUrl(survey.SurveyId);
                    editHyperLink.Visible = !surveyIsComplete && this.IsEditable;
                }

                var previewHyperLink = e.Item.FindControl("ViewHyperLink") as HyperLink;
                if (previewHyperLink != null)
                {
                    previewHyperLink.NavigateUrl = surveyIsComplete
                        ? this.BuildPreviewUrl(completedSurvey.ResponseHeaderId, "responseHeaderId") 
                        : this.BuildPreviewUrl(survey.SurveyId, "SurveyId");
                }

                var deleteHyperLink = e.Item.FindControl("DeleteHyperLink") as HyperLink;
                if (deleteHyperLink != null)
                {
                    if (surveyIsComplete)
                    {
                        deleteHyperLink.NavigateUrl = this.BuildDeleteUrl(completedSurvey.ResponseHeaderId, "responseHeaderId");
                        ClientAPI.AddButtonConfirm(deleteHyperLink, this.Localize("DeleteCompletedSurvey.Text"));
                    }
                    else
                    {
                        deleteHyperLink.NavigateUrl = this.BuildDeleteUrl(survey.SurveyId, "surveyId");
                        ClientAPI.AddButtonConfirm(deleteHyperLink, this.Localize("DeleteSurvey.Text"));
                    }
                    deleteHyperLink.Visible = this.IsAdmin;
                }

                var textLabel = e.Item.FindControl("TextLabel") as Label;
                if (textLabel != null)
                {
                    textLabel.Text = survey.Text;
                }

                var dateLabel = e.Item.FindControl("DateLabel") as Label;
                var userLabel = e.Item.FindControl("UserLabel") as Label;
                if (dateLabel != null)
                {
                    dateLabel.Visible = surveyIsComplete;
                    if (surveyIsComplete)
                    {
                        dateLabel.Text = string.Format(CultureInfo.CurrentCulture, this.Localize("DateLabel.Format"), completedSurvey.CreationDate);
                    }
                }

                if (userLabel != null)
                {
                    userLabel.Visible = surveyIsComplete;
                    if (surveyIsComplete)
                    {
                        var surveyUser = completedSurvey.UserId.HasValue ? UserController.GetUser(this.PortalId, completedSurvey.UserId.Value, false) : null;
                        if (surveyUser != null)
                        {
                            userLabel.Text = string.Format(
                                    CultureInfo.CurrentCulture,
                                    this.Localize("UserLabel.Format"),
                                    surveyUser.DisplayName,
                                    surveyUser.FirstName,
                                    surveyUser.LastName);
                        }
                        else
                        {
                            userLabel.Text = this.Localize("AnonymousUser.Text");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the survey id from the QueryString if possible.
        /// </summary>
        /// <value>The survey id.</value>
        private int? SurveyId
        {
            get
            {
                if (this.Request.QueryString["surveyId"] != null)
                {
                    int id;
                    if (int.TryParse(this.Request.QueryString["surveyId"], NumberStyles.Integer, CultureInfo.InvariantCulture, out id))
                    {
                        return id;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the response header id.
        /// </summary>
        /// <value>The response header id.</value>
        private int? ResponseHeaderId
        {
            get
            {
                if (this.Request.QueryString["responseheaderId"] != null)
                {
                    int id;
                    if (int.TryParse(this.Request.QueryString["responseheaderId"], NumberStyles.Integer, CultureInfo.InvariantCulture, out id))
                    {
                        return id;
                    }
                }
                return null;
            }
        }
    }
}