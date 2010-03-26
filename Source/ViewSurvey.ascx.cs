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
    using System.Web.UI.WebControls;
    using DotNetNuke.Entities.Modules.Communications;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.UI.Utilities;
    using Engage.Survey.Entities;
    using Engage.Survey.UI;

    /// <summary>
    /// This control uses the Engage Survey Control to render a survey. It wires up an event to get in on the saving of a Survey and retrieves the ResponseId
    /// back. It this is raised out to any listeners of this module via the DNN <see cref="IModuleCommunicator"/> interface.
    /// </summary>
    public partial class ViewSurvey : ModuleBase, IModuleCommunicator
    {
        /// <summary>
        /// Occurs when module communication is invoked.
        /// </summary>
        public event ModuleCommunicationEventHandler ModuleCommunication;

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

                return null;
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

                this.SurveyControl.BackButtonText = this.Localize("BackButton.Text", LocalResourceFile);
                this.SurveyControl.SubmitButtonText = this.Localize("SubmitButton.Text", LocalResourceFile);

                // allow module editors to delete user responses
                this.DeleteResponseButton.Click += this.DeleteResponseButton_Click;
                this.DeleteResponseButton.Visible = this.IsEditable && this.ResponseHeaderId != null;
                ClientAPI.AddButtonConfirm(this.DeleteResponseButton, Localize("ConfirmDelete.Text"));
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
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
            if (this.ModuleCommunication != null)
            {
                var args = new ModuleCommunicationEventArgs
                                                        {
                                                                Sender = "ViewSurvey", 
                                                                Target = "Any module", 
                                                                Text = "NewRecord", 
                                                                Value = e.ResponseHeaderId
                                                        };
                this.ModuleCommunication(this, args);
            }
        }
    }
}