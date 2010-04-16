// <copyright file="AnalyzeResponses.ascx.cs" company="Engage Software">
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
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Users;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

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
        /// Backing field for <see cref="Responses"/>
        /// </summary>
        private IQueryable<IGrouping<ResponseHeader, Response>> responses;

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
        /// Gets or sets the grid control in which responses are listed.
        /// </summary>
        private RadGrid ResponseGrid { get; set; }

        /// <summary>
        /// Gets the list of responses (a list of response header, grouped with their related responses).
        /// </summary>
        /// <value>A queryable sequence of <see cref="ResponseHeader"/> instances, grouped with the header's corresponding <see cref="Response"/>s.</value>
        private IQueryable<IGrouping<ResponseHeader, Response>> Responses
        {
            get
            {
                if (this.responses == null)
                {
                    this.responses = new SurveyRepository().LoadResponses(this.SurveyId);
                }

                return this.responses;
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
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.CreateGrid();

            this.Load += this.Page_Load;
            this.ResponseGrid.NeedDataSource += this.ResponseGrid_NeedDataSource;
            this.ResponseGrid.ItemCreated += this.ResponseGrid_ItemCreated;
            base.OnInit(e);
        }

        /// <summary>
        /// Gets the name of the column for the question text in the given <paramref name="response"/>.
        /// </summary>
        /// <param name="response">The response from which to get the question info.</param>
        /// <returns>The name of the column containing the text of the responser to the question</returns>
        private static string GetQuestionTextColumnName(Response response)
        {
            return "Question-Text-" + response.QuestionId.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the name of the column for the question's order in the given <paramref name="response"/>.
        /// </summary>
        /// <param name="response">The response from which to get the question info.</param>
        /// <returns>The name of the column containing the order of the responses to the question</returns>
        private static string GetRelativeOrderColumnName(Response response)
        {
            return "Question-Order-" + response.QuestionId.ToString(CultureInfo.InvariantCulture);
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
        /// Creates the <see cref="ResponseGrid"/>.
        /// </summary>
        /// <remarks>Because we're creating the question columns dynamically, it's easier to just create the whole grid dynamically</remarks>
        private void CreateGrid()
        {
            this.ResponseGrid = new RadGrid
                {
                    ID = "ResponseGrid",
                    Skin = "Simple",
                    CssClass = "sa-grid",
                    AutoGenerateColumns = false,
                    GridLines = GridLines.None,
                    AllowSorting = true,
                    AllowPaging = true,
                    AllowCustomPaging = true,
                    PageSize = 25,
                    ExportSettings =
                        {
                            ExportOnlyData = true, 
                            IgnorePaging = true, 
                            OpenInNewWindow = true,
                            FileName = this.GetExportFilename()
                        },
                    SortingSettings =
                        {
                            SortedAscToolTip = this.Localize("Sorted Ascending.ToolTip"),
                            SortedDescToolTip = this.Localize("Sorted Descending.ToolTip"),
                            SortToolTip = this.Localize("Sort.ToolTip")
                        },
                    MasterTableView =
                        {
                            AllowNaturalSort = false,
                            SortExpressions = { new GridSortExpression { FieldName = "CreationDate", SortOrder = GridSortOrder.Descending } },
                            CommandItemDisplay = GridCommandItemDisplay.TopAndBottom,
                            PagerStyle =
                                {
                                    Mode = GridPagerMode.NextPrevNumericAndAdvanced,
                                    AlwaysVisible = true,
                                    FirstPageToolTip = this.Localize("First Page.ToolTip"),
                                    PrevPageToolTip = this.Localize("Previous Page.ToolTip"),
                                    NextPageToolTip = this.Localize("Next Page.ToolTip"),
                                    LastPageToolTip = this.Localize("Last Page.ToolTip"),
                                    FirstPageText = this.Localize("First Page.Text"),
                                    PrevPageText = this.Localize("Previous Page.Text"),
                                    NextPageText = this.Localize("Next Page.Text"),
                                    LastPageText = this.Localize("Last Page.Text"),
                                    PagerTextFormat = this.Localize("Pager.Format")
                                },
                            CommandItemSettings =
                                {
                                    ShowExportToWordButton = true,
                                    ShowExportToExcelButton = true,
                                    ShowExportToCsvButton = true,
                                    ShowExportToPdfButton = true,
                                    ExportToWordText = this.Localize("Export To Word.ToolTip"),
                                    ExportToExcelText = this.Localize("Export To Excel.ToolTip"),
                                    ExportToCsvText = this.Localize("Export To CSV.ToolTip"),
                                    ExportToPdfText = this.Localize("Export To PDF.ToolTip")
                                },
                            NoRecordsTemplate = new NoRecordsTemplate(this.LocalResourceFile)
                        }
                };

            // add column for each question
            foreach (var response in Enumerable.Last(this.Responses))
            {
                var questionCssClass = string.Format(
                    CultureInfo.InvariantCulture,
                    "sa-question sa-question-{0} sa-question-id-{1}",
                    response.QuestionRelativeOrder,
                    response.QuestionId);

                this.ResponseGrid.MasterTableView.Columns.Add(
                    new GridBoundColumn
                        {
                            DataField = GetQuestionTextColumnName(response),
                            SortExpression = GetRelativeOrderColumnName(response),
                            HeaderText = response.QuestionText,
                            HeaderStyle = { CssClass = questionCssClass + " rgHeader" },
                            ItemStyle = { CssClass = questionCssClass },
                        });
            }

            this.ResponseGrid.MasterTableView.Columns.Add(
                new GridBoundColumn
                    {
                        DataField = "CreationDate",
                        HeaderText = this.Localize("Date.Header"),
                        HeaderStyle = { CssClass = "sa-date rgHeader" },
                        ItemStyle = { CssClass = "sa-date" }
                    });
            this.ResponseGrid.MasterTableView.Columns.Add(
                new GridBoundColumn
                    {
                        DataField = "User",
                        HeaderText = this.Localize("User.Header"),
                        HeaderStyle = { CssClass = "sa-user rgHeader" },
                        ItemStyle = { CssClass = "sa-user" },
                        HtmlEncode = true
                    });
            this.ResponseGrid.MasterTableView.Columns.Add(
                new GridBoundColumn
                    {
                        DataField = "ResponseHeaderId",
                        HeaderText = this.Localize("Response ID.Header"),
                        HeaderStyle = { CssClass = "sa-id rgHeader" },
                        ItemStyle = { CssClass = "sa-id" },
                        Display = false
                    });
            this.ResponseGrid.MasterTableView.Columns.Add(
                new GridHyperLinkColumn
                    {
                        DataNavigateUrlFields = new[] { "ResponseHeaderId" },
                        HeaderText = this.Localize("View.Header"),
                        Text = this.Localize("View.Text"),
                        HeaderStyle = { CssClass = "sa-view rgHeader" },
                        ItemStyle = { CssClass = "sa-view sa-action-btn" },
                        DataNavigateUrlFormatString = this.BuildLinkUrl(this.ModuleId, ControlKey.ViewSurvey, "responseHeaderId={0}")
                    });

            this.ResponseGridPlaceholder.Controls.Add(this.ResponseGrid);
        }

        /// <summary>
        /// Gets the name of the file when exporting data
        /// </summary>
        private string GetExportFilename()
        {
            var filename = string.Format(
                CultureInfo.CurrentCulture, 
                this.Localize("FileName.Format"), 
                DateTime.Now, 
                this.Survey.Text, 
                this.Survey.SurveyId);
            return RemoveInvalidCharacters(filename);
        }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <param name="userId">The user ID, or <c>null</c> for anonymous users.</param>
        /// <returns>The user's name</returns>
        private string GetUser(int? userId)
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
        /// Pivots the questions into columns.
        /// </summary>
        /// <param name="responsesByHeader">A collection of <see cref="ResponseHeader"/>s, grouped with each header's <see cref="Response"/>s.</param>
        /// <returns>A <see cref="DataTable"/> with columns for header information and for each question, and a row for each header</returns>
        private DataTable PivotQuestions(IQueryable<IGrouping<ResponseHeader, Response>> responsesByHeader)
        {
            var table = new DataTable { Locale = CultureInfo.CurrentCulture };

            if (!responsesByHeader.Any())
            {
                return table;
            }

            var questionTextColumnMap = new Dictionary<int, DataColumn>();
            var relativeOrderColumnMap = new Dictionary<int, DataColumn>();

            foreach (var response in Enumerable.Last(responsesByHeader))
            {
                var questionTextcolumn = table.Columns.Add(GetQuestionTextColumnName(response), typeof(string));
                questionTextColumnMap.Add(response.QuestionId, questionTextcolumn);
                var relativeOrdercolumn = table.Columns.Add(GetRelativeOrderColumnName(response), typeof(int));
                relativeOrderColumnMap.Add(response.QuestionId, relativeOrdercolumn);
            }

            // add header columns
            var responseHeaderIdColumn = table.Columns.Add("ResponseHeaderId", typeof(int));
            var creationDateColumn = table.Columns.Add("CreationDate", typeof(DateTime));
            var userColumn = table.Columns.Add("User", typeof(string));

            // create rows for each header
            foreach (var headerWithResponses in responsesByHeader)
            {
                var row = table.NewRow();
                table.Rows.Add(row);

                foreach (var response in headerWithResponses)
                {
                    row[questionTextColumnMap[response.QuestionId]] = response.UserResponse;
                    row[relativeOrderColumnMap[response.QuestionId]] = response.AnswerRelativeOrder ?? (object)DBNull.Value;
                }

                row[responseHeaderIdColumn] = headerWithResponses.Key.ResponseHeaderId;
                row[creationDateColumn] = headerWithResponses.Key.CreationDate;
                row[userColumn] = HttpUtility.HtmlEncode(this.GetUser(headerWithResponses.Key.UserId));
            }

            return table;
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
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Handles the <see cref="RadGrid.NeedDataSource"/> event of the <see cref="ResponseGrid"/> control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="GridNeedDataSourceEventArgs"/> instance containing the event data.</param>
        private void ResponseGrid_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            var pagedResponsesByHeader = this.Responses.Skip(this.ResponseGrid.CurrentPageIndex * this.ResponseGrid.PageSize).Take(this.ResponseGrid.PageSize);
            this.ResponseGrid.DataSource = this.PivotQuestions(pagedResponsesByHeader);
            this.ResponseGrid.MasterTableView.VirtualItemCount = this.Responses.Count();
        }

        /// <summary>
        /// Handles the <see cref="RadGrid.ItemCreated"/> event of the <see cref="ResponseGrid"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridItemEventArgs"/> instance containing the event data.</param>
        private void ResponseGrid_ItemCreated(object sender, GridItemEventArgs e)
        {
            var commandItem = e.Item as GridCommandItem;
            if (commandItem != null)
            {
                // control information from http://www.telerik.com/help/aspnet-ajax/grddefaultbehavior.html
                commandItem.FindControl("AddNewRecordButton").Visible = false;
                commandItem.FindControl("InitInsertButton").Visible = false;
                commandItem.FindControl("RefreshButton").Visible = false;
                commandItem.FindControl("RebindGridButton").Visible = false;
            }
            else
            {
                // control information from http://www.telerik.com/help/aspnet-ajax/grdaccessingdefaultpagerbuttons.html
                var pagerItem = e.Item as GridPagerItem;
                if (pagerItem != null)
                {
                    ((Label)pagerItem.FindControl("GoToPageLabel")).Text = this.Localize("Go to Page Label.Text");
                    ((Button)pagerItem.FindControl("GoToPageLinkButton")).Text = this.Localize("Go to Page Button.Text");
                    ((Label)pagerItem.FindControl("PageOfLabel")).Text = string.Format(CultureInfo.CurrentCulture, this.Localize("Page of.Format"), pagerItem.Paging.PageCount);
                    ((Label)pagerItem.FindControl("ChangePageSizeLabel")).Text = this.Localize("Change Page Size Label.Text");
                    ((Button)pagerItem.FindControl("ChangePageSizeLinkButton")).Text = this.Localize("Change Page Size Button.Text");
                }
            }
        }

        /// <summary>
        /// The template instantiated by the <see cref="AnalyzeResponses.ResponseGrid"/> when there are no responses to display
        /// </summary>
        private class NoRecordsTemplate : ITemplate
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Engage.Dnn.Survey.AnalyzeResponses.NoRecordsTemplate"/> class.
            /// </summary>
            /// <param name="resourceFile">The resource file.</param>
            public NoRecordsTemplate(string resourceFile)
            {
                this.ResourceFile = resourceFile;
            }

            /// <summary>
            /// Gets or sets he resource file.
            /// </summary>
            /// <value>The resource file.</value>
            private string ResourceFile { get; set; }

            /// <summary>
            /// Defines the <see cref="Control"/> object that child controls and templates belong to. These child controls are in turn defined within an inline template.
            /// </summary>
            /// <param name="container">The <see cref="Control"/> object to contain the instances of controls from the inline template.</param>
            public void InstantiateIn(Control container)
            {
// ReSharper disable LocalizableElement
                container.Controls.Add(new Literal { Text = "<h3 class='no-responses'>" + Localization.GetString("No Responses.Text", this.ResourceFile) + "</h3>" });

// ReSharper restore LocalizableElement
            }
        }
    }
}