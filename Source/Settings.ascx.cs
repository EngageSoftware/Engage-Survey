// <copyright file="Settings.ascx.cs" company="Engage Software">
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
    using System.Web.UI.WebControls;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using Framework;

    /// <summary>
    /// Settings control
    /// </summary>
    public partial class Settings : SettingsBase
    {
        /// <summary>
        /// Gets the name of the this module's desktop module record in DNN.
        /// </summary>
        /// <value>The name of this module's desktop module record in DNN.</value>
        public override string DesktopModuleName
        {
            get { return Utility.DesktopModuleName; }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            base.LoadSettings();
            try
            {
                if (Page.IsPostBack == false)
                {
                    var eventListing = new ListItem(Localization.GetString("SurveyListing", LocalResourceFile), "SurveyListing");
                    var viewSurvey = new ListItem(Localization.GetString("ViewSurvey", LocalResourceFile), "ViewSurvey");
                    var thanks = new ListItem(Localization.GetString("ThankYou", LocalResourceFile), "ThankYou");

                    this.ListingDisplayDropDownList.Items.Add(eventListing);
                    this.ListingDisplayDropDownList.Items.Add(viewSurvey);
                    this.ListingDisplayDropDownList.Items.Add(thanks);

                    ListItem listingDisplayListItem = this.ListingDisplayDropDownList.Items.FindByValue(Survey.ModuleSettings.DisplayType.GetValueAsStringFor(this));
                    if (listingDisplayListItem != null)
                    {
                        listingDisplayListItem.Selected = true;
                    }

                    this.AllowMultipleCheckBox.Checked = Survey.ModuleSettings.AllowMultpleEntries.GetValueAsBooleanFor(this).Value;
                    this.ShowRequiredNotationCheckBox.Checked = Survey.ModuleSettings.ShowRequiredNotation.GetValueAsBooleanFor(this).Value;

                    ////DataTable dt = Engage.Survey.Db.DbUtil.GetAssignedSurveys();

                    ////// bind the survey's
                    ////foreach (DataRow row in dt.Rows)
                    ////{
                    ////    ListItem li = new ListItem(ModuleBase.GetCleanTitle(row["SurveyTitle"].ToString()), row["ObjectTypeId"].ToString());
                    ////    SurveyDropDownList.Items.Add(li);
                    ////}

                    ListItem surveyTypeListItem = this.SurveyDropDownList.Items.FindByValue(Survey.ModuleSettings.SurveyTypeId.GetValueAsStringFor(this));
                    if (surveyTypeListItem != null)
                    {
                        surveyTypeListItem.Selected = true;
                    }

                    this.ViewSurveyPanel.Visible = this.ListingDisplayDropDownList.SelectedValue == "ViewSurvey";
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpdateSettings saves the modified settings to the Database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public override void UpdateSettings()
        {
            if (Page.IsValid)
            {
                try
                {
                    Survey.ModuleSettings.IsConfigured.Set(this, true);

                    Survey.ModuleSettings.DisplayType.Set(this, this.ListingDisplayDropDownList.SelectedValue);
                    Survey.ModuleSettings.SurveyTypeId.Set(this, this.SurveyDropDownList.SelectedValue);
                    Survey.ModuleSettings.AllowMultpleEntries.Set(this, this.AllowMultipleCheckBox.Checked);
                    Survey.ModuleSettings.ShowRequiredNotation.Set(this, this.ShowRequiredNotationCheckBox.Checked);
                }
                catch (Exception exc)
                {
                    Exceptions.ProcessModuleLoadException(this, exc);
                }
            }
        }

        /// <summary>
        /// Handles the <see cref="DropDownList.SelectedIndexChanged"/> event of the <see cref="ListingDisplayDropDownList"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ListingDisplayDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ViewSurveyPanel.Visible = this.ListingDisplayDropDownList.SelectedValue == "ViewSurvey";
        }      
    }
}
