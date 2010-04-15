// <copyright file="SurveyListing.ascx.cs" company="Engage Software">
// Engage: Survey
// Copyright (c) 2004-2010
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
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.UI;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Users;
    using DotNetNuke.Services.Exceptions;
    using Engage.Survey.Entities;
    using Telerik.Web.UI;

    /// <summary>
    /// Displays a view of responses to a particular survey
    /// </summary>
    public partial class AnalyzeResponses : ModuleBase
    {
        /// <summary>
        /// A regular expression to match (one or more) invalid filename characters or an underscore (to be used to replace the invalid characters with underscores, without having multiple underscores in a row)
        /// </summary>
        private static readonly Regex InvalidFilenameCharactersExpression = new Regex("[" + Regex.Escape(new string(Path.GetInvalidFileNameChars())) + "_]+", RegexOptions.Compiled);

        /// <summary>
        /// Backing field for <see cref="Survey"/>
        /// </summary>
        private Survey survey;

        /// <summary>
        /// Backing field for <see cref="SurveyId"/>
        /// </summary>
        private int? surveyId;

        /// <summary>
        /// Gets the survey for which to display responses.
        /// </summary>
        /// <value>The survey.</value>
        protected Survey Survey
        {
            get
            {
                if (this.survey == null)
                {
                    this.survey = new SurveyRepository().LoadSurvey(this.SurveyId);
                }

                return this.survey;
            }
        }

        /// <summary>
        /// Gets the ID of the survey for which to display responses.
        /// </summary>
        /// <value>The survey ID.</value>
        private int SurveyId
        {
            get
            {
                if (!this.surveyId.HasValue)
                {
                    int value;
                    if (!int.TryParse(this.Request.QueryString["SurveyId"], NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
                    {
                        throw new InvalidOperationException("Survey ID was not available");
                    }

                    this.surveyId = value;
                }

                return this.surveyId.Value;
            }
        }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <param name="userId">The user ID, or <c>null</c> for anonymous users.</param>
        /// <returns>The user's name</returns>
        protected string GetUser(int? userId)
        {
            var user = UserController.GetUser(this.PortalId, userId ?? Null.NullInteger, false);
            if (user == null)
            {
                return this.Localize("AnonymousUser.Text");
            }

            return string.Format(
                CultureInfo.CurrentCulture, 
                this.Localize("UserLabel.Format"), 
                user.DisplayName, 
                user.FirstName, 
                user.LastName);
        }

        /// <summary>
        /// Gets the URL to view the completed survey.
        /// </summary>
        /// <param name="responseHeaderId">The ID of the <see cref="ResponseHeader"/>.</param>
        /// <returns>A URL which points to a read-only view of the completed survey</returns>
        protected string GetViewLink(int responseHeaderId)
        {
            return this.BuildLinkUrl(
                    this.ModuleId, 
                    ControlKey.ViewSurvey, 
                    "responseHeaderId=" + responseHeaderId.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            this.ResponseGrid.NeedDataSource += this.ResponseGrid_NeedDataSource;
            this.ResponseGrid.ItemCreated += ResponseGrid_ItemCreated;
            base.OnInit(e);
        }

        /// <summary>
        /// Handles the <see cref="RadGrid.NeedDataSource"/> event of the <see cref="ResponseGrid"/> control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="GridNeedDataSourceEventArgs"/> instance containing the event data.</param>
        private void ResponseGrid_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            var surveys = new SurveyRepository().LoadReadOnlySurveys(this.ModuleId);
            this.ResponseGrid.MasterTableView.VirtualItemCount = surveys.Count();
            this.ResponseGrid.DataSource = surveys.Skip(this.ResponseGrid.CurrentPageIndex * this.ResponseGrid.PageSize).Take(this.ResponseGrid.PageSize);
        }

        /// <summary>
        /// Replaces characters in the given <paramref name="filename"/> which are invalid for filenames.
        /// </summary>
        /// <param name="filename">The filename to fix.</param>
        /// <returns>The filename with all invalid characters replaced with an underscore</returns>
        private static string RemoveInvalidCharacters(string filename)
        {
            return InvalidFilenameCharactersExpression.Replace(filename, "_");
        }

        /// <summary>
        /// Handles the <see cref="RadGrid.ItemCreated"/> event of the <see cref="ResponseGrid"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridItemEventArgs"/> instance containing the event data.</param>
        private static void ResponseGrid_ItemCreated(object sender, GridItemEventArgs e)
        {
            var commandItem = e.Item as GridCommandItem;
            if (commandItem == null)
            {
                return;
            }

            commandItem.FindControl("AddNewRecordButton").Visible = false;
            commandItem.FindControl("InitInsertButton").Visible = false;
            commandItem.FindControl("RefreshButton").Visible = false;
            commandItem.FindControl("RebindGridButton").Visible = false;
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        {
            ////var surveys = new SurveyRepository().LoadReadOnlySurveys(this.ModuleId);
            ////this.ResponseGrid.DataSource = surveys;)
            ////this.ResponseGrid.DataBind();


        }

        /// <summary>
        /// Localizes the headers of the <see cref="ResponseGrid"/>.
        /// </summary>
        private void LocalizeGridHeaders()
        {
            foreach (GridColumn column in this.ResponseGrid.MasterTableView.Columns)
            {
                column.HeaderText = this.Localize(column.HeaderText);
            }
        }

        /// <summary>
        /// Sets up the grid's export stuff.
        /// </summary>
        private void SetupExport()
        {
            var filename = string.Format(
                    CultureInfo.CurrentCulture, 
                    this.Localize("FileName.Format"), 
                    DateTime.Now, 
                    this.Survey.Text, 
                    this.Survey.SurveyId);
            this.ResponseGrid.ExportSettings.FileName = RemoveInvalidCharacters(filename);
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
                if (string.IsNullOrEmpty(this.Request.QueryString["SurveyId"]))
                {
                    this.Response.Redirect(this.BuildLinkUrl(this.ModuleId, ControlKey.SurveyListing));
                }

                if (!this.IsPostBack)
                {
                    this.SetupExport();
                    this.LocalizeGridHeaders();
                    this.BindData();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}