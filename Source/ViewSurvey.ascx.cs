// <copyright file="ViewSurvey.ascx.cs" company="Engage Software">
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
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using DotNetNuke.Entities.Modules.Communications;
    using DotNetNuke.Entities.Portals;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.Services.Mail;
    using DotNetNuke.UI.Utilities;
    using Engage.Survey.Entities;
    using Engage.Survey.UI;

    /// <summary>
    /// This control uses the Engage Survey Control to render a survey. It wires up an event to get in on the saving of a Survey and retrieves the ResponseId
    /// back. It this is raised out to any listeners of this module via the DNN <see cref="IModuleCommunicator"/> interface.
    /// </summary>
    public partial class ViewSurvey : ModuleBase
    {     
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
        /// Raises the <see cref="EventArgs"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            try
            {
                this.SurveyControl.UserId = this.UserId;
                this.SurveyControl.CurrentSurvey = this.ResponseHeaderId == null
                                                           ? Survey.LoadSurvey(this.SurveyId.GetValueOrDefault())
                                                           : ReadonlySurvey.LoadSurvey(this.ResponseHeaderId.Value);

                this.SurveyControl.SurveyCompleted += this.SurveyControl1_SurveyCompleted;

                this.SurveyControl.BackButtonText = this.Localize("BackButton.Text");
                this.SurveyControl.SubmitButtonText = this.Localize("SubmitButton.Text");
                this.SurveyControl.PreStartMessageTemplate = this.Localize("PreStartMessage.Format");
                this.SurveyControl.PostEndMessageTemplate = this.Localize("PostEndMessage.Format");

                // allow module editors to delete user responses
                this.DeleteResponseButton.Click += this.DeleteResponseButton_Click;
                this.DeleteResponseButton.Visible = this.IsEditable && this.ResponseHeaderId != null;
                ClientAPI.AddButtonConfirm(this.DeleteResponseButton, this.Localize("ConfirmDelete.Text"));
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
            string body = Localization.GetString("SurveyCompleted_Body.Text", this.LocalResourceFile);
            if (body != null)
            {
                body = body.Replace(Utility.UserNameMarker, displayName);
                body = body.Replace(Utility.SurveyInformationMarker, title);

                var survey = (ReadonlySurvey)ReadonlySurvey.LoadSurvey(responseHeaderId);
                var table = new Table();
                var sb = new StringBuilder();
                var writer = new HtmlTextWriter(new StringWriter(sb));
                survey.Render(table, this.LocalResourceFile);
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
                    responseHeaderId, this.SurveyControl.CurrentSurvey.Text, this.UserInfo.DisplayName);

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
            string subject = Localization.GetString("ThankYou_Subject.Text", this.LocalResourceFile);
            string body = Localization.GetString("ThankYou_Body.Text", this.LocalResourceFile);

            if (subject.Length > 0 && body.Length > 0)
            {
                // Send Email
                try
                {
                    string adminEmail = PortalController.GetCurrentPortalSettings().Email;
                    string s = Mail.SendMail(
                            adminEmail,
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
        }

        /// <summary>
        /// Handles the <see cref="Button.Click"/> event of the <see cref="DeleteResponseButton"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void DeleteResponseButton_Click(object sender, EventArgs e)
        {
            ReadonlySurvey.Delete(this.ResponseHeaderId);
            this.Response.Redirect(this.BuildLinkUrl(this.TabId));
        }

        /// <summary>
        /// Handles the <see cref="Engage.Survey.UI.SurveyControl.SurveyCompleted"/> event of the <see cref="SurveyControl"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SavedEventArgs"/> instance containing the event data.</param>
        private void SurveyControl1_SurveyCompleted(object sender, SavedEventArgs e)
        {
            this.SendNotifications(e.ResponseHeaderId);
        }
    }
}