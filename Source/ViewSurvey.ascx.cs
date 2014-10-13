// <copyright file="ViewSurvey.ascx.cs" company="Engage Software">
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
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    
    using DotNetNuke.Security.Permissions;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.Services.Mail;
    using DotNetNuke.UI.Utilities;

    using Engage.Survey.Entities;
    using Engage.Survey.UI;

    using Telerik.Web.UI;

    /// <summary>
    /// This control uses the Engage Survey Control to render a survey. 
    /// It wires up an event to get in on the saving of a Survey and retrieves the ResponseId back.
    /// </summary>
    public partial class ViewSurvey : ModuleBase
    {
        /// <summary>
        /// The name of the cookie to add to the user's browser when they've taken the survey, 
        /// with a <c>string.format</c>-style placeholder for survey ID
        /// </summary>
        private const string SurveyTakenCookieNameFormat = "Engage_Survey_{0}";

        /// <summary>
        /// Gets the notification from email address either defined as a module setting or a survey setting.
        /// </summary>
        /// <value>The notification from email address.</value>
        protected string NotificationFromEmailAddress
        {
            get
            {
                return this.SurveyControl.CurrentSurvey.NotificationFromEmailAddress;
            }
        }

        /// <summary>
        /// Gets the notification to email addresses either defined as a module setting or a survey
        /// setting.
        /// </summary>
        /// <value>The notification to email addresses.</value>
        protected string NotificationToEmailAddresses
        {
            get
            {
                return this.SurveyControl.CurrentSurvey.NotificationToEmailAddresses;
            }
        }

        /// <summary>Gets a value indicating whether to use the CAPTCHA protection.</summary>
        /// <value><c>true</c> if the form should be protected by a CAPTCHA; otherwise, <c>false</c>.</value>
        protected bool UseCaptchaProtection { get; private set; }

        /// <summary>Gets a value indicating whether to use the minimum timeout protection.</summary>
        /// <value><c>true</c> if the form should be protected by a minimum timeout; otherwise, <c>false</c>.</value>
        protected bool UseMinimumTimeoutProtection { get; private set; }

        /// <summary>Gets a value indicating whether to use the invisible textbox protection.</summary>
        /// <value><c>true</c> if the form should be protected by an invisible textbox; otherwise, <c>false</c>.</value>
        protected bool UseInvisibleTextBoxProtection { get; private set; }

        /// <summary>Gets the minimum timeout to use when <see cref="UseMinimumTimeoutProtection"/> is <c>true</c>.</summary>
        protected int MinimumTimeout
        {
            get { return ModuleSettings.CaptchaMinTimeout.GetValueAsInt32For(this).Value; }
        }

        /// <summary>
        /// Gets the ResponseHeaderId from the QueryString if possible.
        /// </summary>
        /// <value>The survey id.</value>
        private int? ResponseHeaderId
        {
            get
            {
                if (this.Request.QueryString["responseHeaderId"] != null)
                {
                    int id;
                    if (int.TryParse(this.Request.QueryString["responseHeaderId"], NumberStyles.Integer, CultureInfo.InvariantCulture, out id))
                    {
                        return id;
                    }
                }

                return null;
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

                return ModuleSettings.SurveyId.GetValueAsInt32For(this);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the HTTP cookie exists that indicates that the current user has already taken the current survey.
        /// </summary>
        /// <value>
        /// <c>true</c> if the "survey already taken" cookie exists; otherwise, <c>false</c>.
        /// </value>
        private bool SurveyTakenCookieExists
        {
            get { return this.Request.Cookies.AllKeys.Any(cookieName => cookieName == this.SurveyTakenCookieName); }
        }

        /// <summary>
        /// Gets the name of the cookie which indicates that the survey has been taken.
        /// </summary>
        /// <remarks>This property assumes that a survey is currently being viewed (i.e. <see cref="SurveyId"/> is not <c>null</c>)</remarks>
        /// <value>The name of the HTTP cookie whose presence indicates that this user has taken the current survey</value>
        private string SurveyTakenCookieName
        {
            get
            {
                Debug.Assert(
                    this.SurveyId.HasValue,
                    "SurveyId must have a value",
                    "It is only valid to call the SurveyTakenCookieName property when you are sure that SurveyId is set");

                return string.Format(CultureInfo.InvariantCulture, SurveyTakenCookieNameFormat, this.SurveyId.Value);
            }
        }

        /// <summary>
        /// Raises the <see cref="EventArgs"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            try
            {   
                var displayingCompletedSurvey = false;
                if (this.ResponseHeaderId == null || !ModulePermissionController.CanEditModuleContent(this.ModuleConfiguration))
                {
                    this.SurveyControl.CurrentSurvey = new SurveyRepository().LoadSurvey(this.SurveyId.Value, this.ModuleId);
                    this.SurveyControl.ValidationGroup = string.Format(CultureInfo.InvariantCulture, "survey-{0}", this.SurveyId.Value);

                    // Check to see if the user has taken the survey and hide it if true
                    if (!ModuleSettings.AllowMultpleEntries.GetValueAsBooleanFor(this).Value)
                    {
                        this.SurveyControl.ShowAlreadyTakenMessage = (IsLoggedIn && new SurveyRepository().UserHasTaken(this.UserId, this.SurveyId.Value)) ||
                                                                     (!IsLoggedIn && this.SurveyTakenCookieExists);
                    }
                }
                else
                {
                    displayingCompletedSurvey = true;
                    this.SurveyControl.CurrentSurvey = new SurveyRepository().LoadReadOnlySurvey(this.ResponseHeaderId.Value, this.ModuleId);
                }

                this.SurveyControl.UserId = this.UserId;
                this.SurveyControl.SurveyCompleted += this.SurveyControl_SurveyCompleted;

                this.SurveyControl.ShowRequiredNotation = ModuleSettings.ShowRequiredNotation.GetValueAsBooleanFor(this).Value;
                this.SurveyControl.Localizer = new DnnLocalizer(this.LocalResourceFile);

                // allow module editors to delete user responses)
                this.DeleteResponseButton.Click += this.DeleteResponseButton_Click;
                this.DeleteResponseButton.Visible = this.IsEditable && displayingCompletedSurvey;
                ClientAPI.AddButtonConfirm(this.DeleteResponseButton, this.Localize("ConfirmDelete.Text"));

                this.Page.ClientScript.RegisterClientScriptResource(typeof(ViewSurvey), "Engage.Dnn.Survey.JavaScript.viewSurvey-all.js");

                // allow module editors to edit survey
                this.EditDefinitionButton.Click += this.EditDefinitionButton_Click;
                this.EditDefinitionButton.Visible = this.IsEditable && !displayingCompletedSurvey;

                // Check to see if the user has taken the survey and hide it if true
                if (!ModuleSettings.AllowMultpleEntries.GetValueAsBooleanFor(this).Value)
                {
                    string surveyTakenCookieName = string.Format(CultureInfo.InvariantCulture, SurveyTakenCookieNameFormat, this.SurveyId.Value);
                    this.SurveyControl.UserHasTaken = (IsLoggedIn && new SurveyRepository().UserHasTaken(this.UserId, this.SurveyId.Value)) ||
                                                      this.Request.Cookies.AllKeys.Any(cookie => cookie == surveyTakenCookieName);
                }

                var captchaStrategies = ModuleSettings.GetCaptchaStrategies(this).ToArray();
                this.UseCaptchaProtection = captchaStrategies.Contains(RadCaptcha.ProtectionStrategies.Captcha);
                this.UseInvisibleTextBoxProtection = captchaStrategies.Contains(RadCaptcha.ProtectionStrategies.InvisibleTextBox);
                this.UseMinimumTimeoutProtection = captchaStrategies.Contains(RadCaptcha.ProtectionStrategies.MinimumTimeout);
                this.SurveyControl.DataBind();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Parses out the survey HTML for use in an email or printing.
        /// </summary>
        /// <param name="responseHeaderId">The response header id.</param>
        /// <param name="title">The title.</param>
        /// <param name="displayName">The display name.</param>
        /// <returns>
        /// A string containing table based html that represents a survey.
        /// </returns>
        private string GenerateTableBasedSurvey(int responseHeaderId, string title, string displayName)
        {
            var builder = new StringBuilder();
            string body = this.Localize("SurveyCompleted_Body.Text");
            if (body != null)
            {
                body = body.Replace(Utility.UserNameMarker, displayName);
                body = body.Replace(Utility.SurveyInformationMarker, title);

                var survey = new SurveyRepository().LoadReadOnlySurvey(responseHeaderId, this.ModuleId);
                var table = new Table();
                var sb = new StringBuilder();
                var writer = new HtmlTextWriter(new StringWriter(sb));
                survey.Render(table, new DnnLocalizer(this.LocalResourceFile));
                table.RenderControl(writer);

                body = body.Replace(Utility.SurveyTableMarker, sb.ToString());
                builder.Append(body);
            }

            ////Because the output of the controls are type "input" the disabled, checked and selected properties must be changed
            ////to just disabled, checked and selected to be recognized by certain email clients i.e. Hotmail.
            return
                    builder.ToString().Replace('"', '\'').Replace(Environment.NewLine, string.Empty).Replace("\t", string.Empty).Replace(
                            "='selected'", string.Empty).Replace("='disabled'", string.Empty).Replace("='checked'", string.Empty);
        }

        /// <summary>
        /// Sends the administrative notification to the email defined in the module settings.
        /// </summary>
        /// <param name="responseHeaderId">The response header id.</param>
        private void SendAdministrativeNotification(int responseHeaderId)
        {
            // generate table based HTML to represent the sruvey.
            string surveyHtml = this.GenerateTableBasedSurvey(
                    responseHeaderId, 
                    this.SurveyControl.CurrentSurvey.Text, 
                    this.UserInfo.DisplayName);

            if (surveyHtml.Length > 0)
            {
                // Send Email
                try
                {
                    string subject = Localization.GetString("SurveyCompleted_Subject.Text", this.LocalResourceFile);
                    string fromEmail = this.NotificationFromEmailAddress;
                    string recipientAddresses = this.NotificationToEmailAddresses;
                    string s = Mail.SendMail(
                            fromEmail,
                            recipientAddresses,
                            string.Empty,
                            subject,
                            surveyHtml,
                            string.Empty,
                            "HTML",
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            string.Empty);

                    if (s.Length > 0)
                    {
                        // write the exception to the log.
                        Exceptions.LogException(new Exception(s));
                    }
                }
                catch (Exception ex)
                {
                    // do nothing. this should be a show stopper but needs to be logged.
                    Exceptions.LogException(ex);
                }
            }
        }

        /// <summary>
        /// Sends the notifications to the administrator.
        /// </summary>
        /// <param name="responseHeaderId">The response header id.</param>
        private void SendNotifications(int responseHeaderId)
        {
            if (this.SurveyControl.CurrentSurvey.SendNotification)
            {
                this.SendAdministrativeNotification(responseHeaderId);
            }

            if (this.SurveyControl.CurrentSurvey.SendThankYou)
            {
                this.SendThankYouNotification();
            }
        }

        /// <summary>
        /// Sends the thank you notification back to the user who submitted the survey.
        /// </summary>
        private void SendThankYouNotification()
        {
            if (string.IsNullOrEmpty(this.UserInfo.Email))
            {
                return;
            }

            var subject = Localization.GetString("ThankYou_Subject.Text", this.LocalResourceFile);
            var body = Localization.GetString("ThankYou_Body.Text", this.LocalResourceFile);

            if (subject.Length <= 0 || body.Length <= 0)
            {
                return;
            }

            try
            {
                var s = Mail.SendMail(
                    this.SurveyControl.CurrentSurvey.ThankYouFromEmailAddress,
                    this.UserInfo.Email,
                    string.Empty,
                    subject,
                    body,
                    string.Empty,
                    "HTML",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty);

                if (s.Length > 0)
                {
                    // write the exception to the log.
                    Exceptions.LogException(new Exception(s));
                }
            }
            catch (Exception ex)
            {
                // do nothing. this should be a show stopper but needs to be logged.
                Exceptions.LogException(ex);
            }
        }

        /// <summary>
        /// Handles the <see cref="Button.Click"/> event of the <see cref="DeleteResponseButton"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void DeleteResponseButton_Click(object sender, EventArgs e)
        {
            if (!ModulePermissionController.CanEditModuleContent(this.ModuleConfiguration))
            {
                return;
            }

            new SurveyRepository().DeleteReadOnlySurvey(this.ResponseHeaderId);
            this.Response.Redirect(this.BuildLinkUrl(this.TabId));
        }

        /// <summary>
        /// Handles the <see cref="Button.Click"/> event of the <see cref="EditDefinitionButton"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void EditDefinitionButton_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(this.BuildLinkUrl(this.ModuleId, ControlKey.EditSurvey, "surveyId=" + this.SurveyId.GetValueOrDefault().ToString(CultureInfo.InvariantCulture)));
        }

        /// <summary>
        /// Handles the <see cref="Engage.Survey.UI.SurveyControl.SurveyCompleted"/> event of the <see cref="SurveyControl"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SavedEventArgs"/> instance containing the event data.</param>
        private void SurveyControl_SurveyCompleted(object sender, SavedEventArgs e)
        {
            this.Response.SetCookie(new HttpCookie(this.SurveyTakenCookieName, e.ResponseHeaderId.ToString(CultureInfo.InvariantCulture)));
            this.SendNotifications(e.ResponseHeaderId);
        }
    }
}
