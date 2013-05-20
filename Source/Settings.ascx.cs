// <copyright file="Settings.ascx.cs" company="Engage Software">
// Engage: Survey
// Copyright (c) 2004-2013
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
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using DotNetNuke.Services.Exceptions;

    using Engage.Survey.Entities;
    using Framework;

    using Telerik.Web.UI;

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
                if (this.IsPostBack)
                {
                    return;
                }

                this.ListingDisplayDropDownList.Items.Add(new ListItem(this.Localize(ControlKey.SurveyListing.ToString()), ControlKey.SurveyListing.ToString()));
                this.ListingDisplayDropDownList.Items.Add(new ListItem(this.Localize(ControlKey.ViewSurvey.ToString()), ControlKey.ViewSurvey.ToString()));
                this.SetSelectedListItem(this.ListingDisplayDropDownList, Dnn.Survey.ModuleSettings.DisplayType);

                this.SurveyDropDownList.DataSource = new SurveyRepository().LoadSurveys(this.ModuleId, true);
                this.SurveyDropDownList.DataTextField = "Text";
                this.SurveyDropDownList.DataValueField = "SurveyId";
                this.SurveyDropDownList.DataBind();
                this.SetSelectedListItem(this.SurveyDropDownList, Dnn.Survey.ModuleSettings.SurveyId);

// ReSharper disable PossibleInvalidOperationException
                this.AllowMultipleCheckBox.Checked = Dnn.Survey.ModuleSettings.AllowMultpleEntries.GetValueAsBooleanFor(this).Value;
                this.ShowRequiredNotationCheckBox.Checked = Dnn.Survey.ModuleSettings.ShowRequiredNotation.GetValueAsBooleanFor(this).Value;
                this.SendNotificationCheckBox.Checked = Dnn.Survey.ModuleSettings.SendNotification.GetValueAsBooleanFor(this).Value;
                this.SendThankYouCheckBox.Checked = Dnn.Survey.ModuleSettings.SendThankYou.GetValueAsBooleanFor(this).Value;

// ReSharper restore PossibleInvalidOperationException
                this.NotificationFromEmailTextBox.Text = Dnn.Survey.ModuleSettings.NotificationFromEmailAddress.GetValueAsStringFor(this) 
                                                            ?? this.PortalSettings.Email;
                this.NotificationToEmailsTextBox.Text = Dnn.Survey.ModuleSettings.NotificationToEmailAddresses.GetValueAsStringFor(this) 
                                                          ?? this.PortalSettings.Email;
                this.ThankYouFromEmailTextBox.Text = Dnn.Survey.ModuleSettings.ThankYouFromEmailAddress.GetValueAsStringFor(this) 
                                                        ?? this.PortalSettings.Email;

                var captchaStrategies = Dnn.Survey.ModuleSettings.GetCaptchaStrategies(this).ToArray();
                this.UseCaptchaProtectionCheckBox.Checked = captchaStrategies.Contains(RadCaptcha.ProtectionStrategies.Captcha);
                this.UseInvisibleTextBoxProtectionCheckBox.Checked = captchaStrategies.Contains(RadCaptcha.ProtectionStrategies.InvisibleTextBox);
                this.UseMinimumTimeoutProtectionCheckBox.Checked = captchaStrategies.Contains(RadCaptcha.ProtectionStrategies.MinimumTimeout);
                this.MinimumTimeoutTextBox.Value = Dnn.Survey.ModuleSettings.CaptchaMinTimeout.GetValueAsInt32For(this);

                this.SetViewSurveySettingsVisibility();
                this.SetMinimumTimeoutEnable();

                this.NotificationFromEmailPatternValidator.ValidationExpression = Engage.Utility.EmailRegEx;
                this.NotificationToEmailsPatternValidator.ValidationExpression = Engage.Utility.EmailsRegEx;
                this.ThankYouFromEmailPatternValidator.ValidationExpression = Engage.Utility.EmailRegEx;
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
            if (!this.Page.IsValid)
            {
                return;
            }

            try
            {
                Dnn.Survey.ModuleSettings.IsConfigured.Set(this, true);

                Dnn.Survey.ModuleSettings.DisplayType.Set(this, this.ListingDisplayDropDownList.SelectedValue);
                Dnn.Survey.ModuleSettings.SurveyId.Set(this, this.SurveyDropDownList.SelectedValue);
                Dnn.Survey.ModuleSettings.AllowMultpleEntries.Set(this, this.AllowMultipleCheckBox.Checked);
                Dnn.Survey.ModuleSettings.ShowRequiredNotation.Set(this, this.ShowRequiredNotationCheckBox.Checked);

                Dnn.Survey.ModuleSettings.CaptchaStrategies.Set(this, string.Join(",", this.GetSelectedCaptchaStrategies().Select(s => s.ToString()).ToArray()));
                Dnn.Survey.ModuleSettings.CaptchaMinTimeout.Set(this, this.MinimumTimeoutTextBox.Value);

                Dnn.Survey.ModuleSettings.SendNotification.Set(this, this.SendNotificationCheckBox.Checked);
                Dnn.Survey.ModuleSettings.NotificationFromEmailAddress.Set(this, this.NotificationFromEmailTextBox.Text);
                Dnn.Survey.ModuleSettings.NotificationToEmailAddresses.Set(this, this.NotificationToEmailsTextBox.Text);
                Dnn.Survey.ModuleSettings.SendThankYou.Set(this, this.SendThankYouCheckBox.Checked);
                Dnn.Survey.ModuleSettings.ThankYouFromEmailAddress.Set(this, this.ThankYouFromEmailTextBox.Text);
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.ListingDisplayDropDownList.SelectedIndexChanged += this.ListingDisplayDropDownList_SelectedIndexChanged;
            this.UseMinimumTimeoutProtectionCheckBox.CheckedChanged += this.UseMinimumTimeoutProtectionCheckBox_CheckedChanged;
            base.OnInit(e);
        }

        /// <summary>Gets the selected CAPTCHA protection strategies.</summary>
        /// <returns>A sequence of <see cref="RadCaptcha.ProtectionStrategies"/></returns>
        private IEnumerable<RadCaptcha.ProtectionStrategies> GetSelectedCaptchaStrategies()
        {
            if (this.UseCaptchaProtectionCheckBox.Checked)
            {
                yield return RadCaptcha.ProtectionStrategies.Captcha;
            }

            if (this.UseInvisibleTextBoxProtectionCheckBox.Checked)
            {
                yield return RadCaptcha.ProtectionStrategies.InvisibleTextBox;
            }

            if (this.UseMinimumTimeoutProtectionCheckBox.Checked)
            {
                yield return RadCaptcha.ProtectionStrategies.MinimumTimeout;
            }
        }

        /// <summary>
        /// Selects the <see cref="ListItem"/> with the value of the given <paramref name="setting"/>.
        /// </summary>
        /// <typeparam name="T">The type of the setting value</typeparam>
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
        /// Sets the visibility of <see cref="SurveySettingSection"/>.
        /// </summary>
        private void SetViewSurveySettingsVisibility()
        {
            this.SurveySettingSection.Visible = this.ListingDisplayDropDownList.SelectedValue == ControlKey.ViewSurvey.ToString();
        }

        /// <summary>
        /// Sets <see cref="MinimumTimeoutTextBox"/> and its validator to be enabled or disabled.
        /// </summary>
        private void SetMinimumTimeoutEnable()
        {
            this.MinimumTimeoutRequiredValidator.Enabled = this.UseMinimumTimeoutProtectionCheckBox.Checked;
            this.MinimumTimeoutTextBox.Enabled = this.UseMinimumTimeoutProtectionCheckBox.Checked;
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

        /// <summary>
        /// Handles the <see cref="CheckBox.CheckedChanged"/> event of the <see cref="UseMinimumTimeoutProtectionCheckBox"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void UseMinimumTimeoutProtectionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.SetMinimumTimeoutEnable();
        }
    }
}
