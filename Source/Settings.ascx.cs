// <copyright file="Settings.ascx.cs" company="Engage Software">
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
                    var eventListing = new ListItem(Localization.GetString(ControlKey.SurveyListing.ToString(), LocalResourceFile), ControlKey.SurveyListing.ToString());
                    var viewSurvey = new ListItem(Localization.GetString(ControlKey.ViewSurvey.ToString(), LocalResourceFile), ControlKey.ViewSurvey.ToString());
                    var thanks = new ListItem(Localization.GetString(ControlKey.ThankYou.ToString(), LocalResourceFile), ControlKey.ThankYou.ToString());

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

        /// <summary>
        /// UpdateSettings saves the modified settings to the Database
        /// </summary>
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
