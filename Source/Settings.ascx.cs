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
    using System.Linq;
    using System.Web.UI;
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
                if (!this.IsPostBack)
                {
                    this.ListingDisplayDropDownList.Items.Add(new ListItem(Localization.GetString(ControlKey.SurveyListing.ToString(), this.LocalResourceFile), ControlKey.SurveyListing.ToString()));
                    this.ListingDisplayDropDownList.Items.Add(new ListItem(Localization.GetString(ControlKey.ViewSurvey.ToString(), this.LocalResourceFile), ControlKey.ViewSurvey.ToString()));
                    this.ListingDisplayDropDownList.Items.Add(new ListItem(Localization.GetString(ControlKey.ThankYou.ToString(), this.LocalResourceFile), ControlKey.ThankYou.ToString()));
                    this.SetSelectedListItem(this.ListingDisplayDropDownList, Survey.ModuleSettings.DisplayType);

                    this.SurveyDropDownList.DataSource = Engage.Survey.Entities.Survey.LoadSurveys();
                    this.SurveyDropDownList.DataTextField = "Text";
                    this.SurveyDropDownList.DataValueField = "SurveyId";
                    this.SurveyDropDownList.DataBind();
                    this.SetSelectedListItem(this.SurveyDropDownList, Survey.ModuleSettings.SurveyId);

// ReSharper disable PossibleInvalidOperationException
                    this.AllowMultipleCheckBox.Checked = Survey.ModuleSettings.AllowMultpleEntries.GetValueAsBooleanFor(this).Value;
                    this.ShowRequiredNotationCheckBox.Checked = Survey.ModuleSettings.ShowRequiredNotation.GetValueAsBooleanFor(this).Value;
                    this.SendNotificationCheckBox.Checked = Survey.ModuleSettings.SendNotification.GetValueAsBooleanFor(this).Value;
                    this.SendThankYouCheckBox.Checked = Survey.ModuleSettings.SendThankYou.GetValueAsBooleanFor(this).Value;

// ReSharper restore PossibleInvalidOperationException
                    this.NotificationFromEmailTextBox.Text = Survey.ModuleSettings.NotificationFromEmailAddress.GetValueAsStringFor(this)
                                                             ?? this.PortalSettings.Email;
                    this.NotificationToEmailsTextBox.Text = Survey.ModuleSettings.NotificationToEmailAddresses.GetValueAsStringFor(this)
                                                             ?? this.PortalSettings.Email;
                    this.ThankYouFromEmailTextBox.Text = Survey.ModuleSettings.ThankYouFromEmailAddress.GetValueAsStringFor(this)
                                                             ?? this.PortalSettings.Email;

                    this.SetViewSurveySettingsVisibility();

                    this.NotificationFromEmailPatternValidator.ValidationExpression = Engage.Utility.EmailRegEx;
                    this.NotificationToEmailsPatternValidator.ValidationExpression = Engage.Utility.EmailsRegEx;
                    this.ThankYouFromEmailPatternValidator.ValidationExpression = Engage.Utility.EmailRegEx;
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Saves the modified settings to the Database
        /// </summary>
        public override void UpdateSettings()
        {
            if (this.Page.IsValid)
            {
                try
                {
                    Survey.ModuleSettings.IsConfigured.Set(this, true);

                    Survey.ModuleSettings.DisplayType.Set(this, this.ListingDisplayDropDownList.SelectedValue);
                    Survey.ModuleSettings.SurveyId.Set(this, this.SurveyDropDownList.SelectedValue);
                    Survey.ModuleSettings.AllowMultpleEntries.Set(this, this.AllowMultipleCheckBox.Checked);
                    Survey.ModuleSettings.ShowRequiredNotation.Set(this, this.ShowRequiredNotationCheckBox.Checked);

                    Survey.ModuleSettings.SendNotification.Set(this, this.SendNotificationCheckBox.Checked);
                    Survey.ModuleSettings.NotificationFromEmailAddress.Set(this, this.NotificationFromEmailTextBox.Text);
                    Survey.ModuleSettings.NotificationToEmailAddresses.Set(this, this.NotificationToEmailsTextBox.Text);
                    Survey.ModuleSettings.SendThankYou.Set(this, this.SendThankYouCheckBox.Checked);
                    Survey.ModuleSettings.ThankYouFromEmailAddress.Set(this, this.ThankYouFromEmailTextBox.Text);
                }
                catch (Exception exc)
                {
                    Exceptions.ProcessModuleLoadException(this, exc);
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.ListingDisplayDropDownList.SelectedIndexChanged += this.ListingDisplayDropDownList_SelectedIndexChanged;
            base.OnInit(e);
        }

        /// <summary>
        /// Selects the <see cref="ListItem"/> with the value of the given <paramref name="setting"/>.
        /// </summary>
        /// <param name="listControl">The list control.</param>
        /// <param name="setting">The setting.</param>
        private void SetSelectedListItem<T>(ListControl listControl, Setting<T> setting)
        {
            var selectedListItem = listControl.Items.FindByValue(setting.GetValueAsStringFor(this));
            if (selectedListItem != null)
            {
                selectedListItem.Selected = true;
            }
        }

        /// <summary>
        /// Sets the visibility of <see cref="ViewSurveyPlaceholder"/>.
        /// </summary>
        private void SetViewSurveySettingsVisibility()
        {
            this.ViewSurveyPlaceholder.Visible = this.ListingDisplayDropDownList.SelectedValue == ControlKey.ViewSurvey.ToString();
        }

        /// <summary>
        /// Handles the <see cref="DropDownList.SelectedIndexChanged"/> event of the <see cref="ListingDisplayDropDownList"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ListingDisplayDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SetViewSurveySettingsVisibility();
        }
    }
}
