// <copyright file="SurveyControl.cs" company="Engage Software">
// Engage: Survey
// Copyright (c) 2004-2015
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Survey.UI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    using Engage.Annotations;

    using Util;

    /// <summary>
    /// Validation Providers
    /// </summary>
    public enum ValidationProviders
    {
        /// <summary>
        /// Engage Validation Provider
        /// </summary>
        Engage
    }

    /// <summary>
    /// SurveyControl used to render a Survey
    /// </summary>
    [Designer(typeof(SurveyControlDesigner))]
    [ToolboxData("<{0}:SurveyControl SurveyTypeId='-1' runat=server />")]
    public class SurveyControl : CompositeControl
    {
        /// <summary>
        /// Marker for begin survey
        /// </summary>
        public const string BeginSurveyMarker = "<!--survey_begins_here-->";

        /// <summary>
        /// CSS Class to use for submit button
        /// </summary>
        public const string CssClassSubmitButton = "submit-button";

        /// <summary>
        /// CSS Class to use for submit button
        /// </summary>
        public const string CssClassBackButton = "back-button";

        /// <summary>
        /// CSS class used for styling thank you verbiage.
        /// </summary>
        public const string CssClassThankYou = "thankyou-label";

        /// <summary>
        /// CSS class used for styling thank you verbiage.
        /// </summary>
        public const string CssClassThankYouWrap = "thankyou-wrap";

        /// <summary>
        /// CSS Class to use when no survey typeId is defined
        /// </summary>
        public const string CssClassNoSurveyDefined = "no-survey-defined";

        /// <summary>
        /// Marker for end survey.
        /// </summary>
        public const string EndSurveyMarker = "<!--survey_ends_here-->";

        /// <summary>
        /// Backing field for <see cref="BackButtonText"/>
        /// </summary>
        private string backButtonText;

        /// <summary>
        /// Backing field for <see cref="SubmitButtonText"/>
        /// </summary>
        private string submitButtonText;

        /// <summary>
        /// SaveEventHandler used for Save Completed event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SavedEventArgs"/> instance containing the event data.</param>
        public delegate void SaveEventHandler(object sender, SavedEventArgs e);

        /// <summary>
        /// Occurs when the survey is completed.
        /// </summary>
        public event SaveEventHandler SurveyCompleted;

        /// <summary>Gets or sets the validation group with which this <see cref="SurveyControl"/> should validate.</summary>
        public string ValidationGroup { get; set; }

        /// <summary>
        /// Gets or sets the survey that is being rendered.
        /// </summary>
        /// <remarks>The <see cref="ISurvey.Save"/> method will be called when the Submit button is clicked.</remarks>
        public ISurvey CurrentSurvey { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get
            {
                return this.CurrentSurvey.IsReadOnly;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show required notation].
        /// </summary>
        /// <value>
        /// <c>true</c> if [show required notation]; otherwise, <c>false</c>.
        /// </value>
        [Bindable(true)]
        [Category("Validation")]
        [DefaultValue("")]
        public bool ShowRequiredNotation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [back button text].
        /// </summary>
        /// <value><c>true</c> if [back button text]; otherwise, <c>false</c>.</value>
        [DefaultValue("Back")]
        [Browsable(true)]
        [Description("Set the text that appears on the button.")]
        [Category("")]
        public string BackButtonText
        {
            get
            {
                if (!string.IsNullOrEmpty(this.backButtonText))
                {
                    return this.backButtonText;
                }

                return this.Localizer.Localize("BackButton.Text");
            }

            set
            {
                this.backButtonText = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [submit button text].
        /// </summary>
        /// <value><c>true</c> if [submit button text]; otherwise, <c>false</c>.</value>
        [DefaultValue("Submit")]
        [Browsable(true)]
        [Description("Set the text that appears on the button.")]
        [Category("")]
        public string SubmitButtonText
        {
            get
            {
                if (!string.IsNullOrEmpty(this.submitButtonText))
                {
                    return this.submitButtonText;
                }

                return this.Localizer.Localize("SubmitButton.Text");
            }
            
            set
            {
                this.submitButtonText = value;
            }
        }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <remarks>You can optionally set this property and it will be stored with the <c>ResponseHeader</c> record.</remarks>
        /// <value>The user id.</value>
        public int UserId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show a message that the current user has already taken this survey and cannot take it again.
        /// </summary>
        /// <value><c>true</c> if the already taken message should be shown instead of the survey; otherwise, <c>false</c>.</value>
        public bool ShowAlreadyTakenMessage
        {
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user has taken the survey previously.
        /// </summary>
        /// <value>A boolean showing whether the user has taken the survey.</value>
        public bool UserHasTaken
        {
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the validation provider.
        /// </summary>
        /// <value>The validation provider.</value>
        [Bindable(true)]
        [Category("Validation")]
        [DefaultValue("")]
        public ValidationProviders ValidationProvider
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a template containing any extra validator controls.
        /// </summary>
        /// <value> The validators template. </value>
        [TemplateContainer(typeof(ValidatorsTemplateContainer))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate Validators { get; set; }

        /// <summary>
        /// Gets or sets the localizer.
        /// </summary>
        /// <value>
        /// The localizer.
        /// </value>
        public ILocalizer Localizer { get; set; }

        /// <summary>
        /// Gets the return URL if on the querystring.
        /// </summary>
        /// <value>The return URL.</value>
        private static string ReturnUrl
        {
            get
            {
                return HttpContext.Current.Request.QueryString["returnurl"] ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the message to display when the user has already taken this survey.
        /// </summary>
        /// <value>The already taken message.</value>
        private string AlreadyTakenMessage
        {
            get { return this.Localizer.Localize("AlreadyTakenMessage.Text"); }
        }

        /// <summary>
        /// Gets the message template to use when the survey's <see cref="ISurvey.StartDate"/> is in the future.
        /// </summary>
        /// <value>The message template for surveys that haven't started.</value>
        private string PreStartMessageTemplate
        {
            get { return this.Localizer.Localize("PreStartMessage.Format"); }
        }

        /// <summary>
        /// Gets the message template to use when the survey's <see cref="ISurvey.EndDate"/> is in the past.
        /// </summary>
        /// <value>The message template for surveys that have ended.</value>
        private string PostEndMessageTemplate
        {
            get { return this.Localizer.Localize("PostEndMessage.Format"); }
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            this.Controls.Clear();

            // Insert our begin marker for the survey control. We can parse this out easy later if needed.
            this.Controls.Add(new Literal { Text = BeginSurveyMarker });

            var mainDiv = new HtmlGenericControl("DIV");
            mainDiv.Attributes["class"] = this.CssClass;
            this.Controls.Add(mainDiv);

            var ph = new PlaceHolder();
            this.Controls.Add(ph);
            if (this.CurrentSurvey == null)
            {
                var nosurveyDiv = new HtmlGenericControl("DIV");
                nosurveyDiv.Attributes["class"] = CssClassNoSurveyDefined;

                return;
            }

            string thanks = this.Context.Request.QueryString["thankyou"];
            if (thanks == "true")
            {
                this.Visible = false;
                return;
            }

            // TODO: switch to UTC dates
            bool beforeStartDate = this.CurrentSurvey.StartDate > DateTime.Now;
            bool afterEndDate = this.CurrentSurvey.EndDate <= DateTime.Now;
            if (beforeStartDate || afterEndDate || this.ShowAlreadyTakenMessage)
            {
                string messageHtml;
                if (this.ShowAlreadyTakenMessage)
                {
                    messageHtml = this.AlreadyTakenMessage;
                }
                else
                {
                    var message = beforeStartDate ? this.CurrentSurvey.PreStartMessage : this.CurrentSurvey.PostEndMessage;
                    var messageWrapperHtmlFormat = beforeStartDate ? this.PreStartMessageTemplate : this.PostEndMessageTemplate;
                    messageHtml = string.Format(CultureInfo.CurrentCulture, messageWrapperHtmlFormat, HttpUtility.HtmlEncode(message));
                }

                ph.Controls.Add(new Literal { Text = messageHtml });
            }
            else
            {
                // Need to make validator construction mechanism as we create new implementations! hk
                // draw the survey
                this.CurrentSurvey.Render(ph, this.IsReadOnly, this.ShowRequiredNotation, new EngageValidationProvider(this.ValidationGroup), this.Localizer);

                // no need to include the submit button in html
                if (!this.IsReadOnly)
                {
                    this.RenderSubmitButton();
                }

                if (this.Validators != null)
                {
                    var validatorsContainer = new ValidatorsTemplateContainer(this.ValidationGroup);
                    this.Validators.InstantiateIn(validatorsContainer);
                    ph.Controls.Add(validatorsContainer);
                }
            }

            this.RenderBackButton(this);
            this.Controls.Add(new Literal { Text = EndSurveyMarker });
        }

        /// <summary>
        /// Raises the <see cref="SurveyCompleted"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Engage.Survey.UI.SavedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnSurveyCompleted(SavedEventArgs e)
        {
            if (this.SurveyCompleted != null)
            {
                this.SurveyCompleted(this, e);
            }
        }

        /// <summary>
        /// Handles the Click event of the back button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private static void BackButton_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Redirect(ReturnUrl, true);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="c">The control to inspect</param>
        /// <param name="key">The key of the control.</param>
        /// <returns>The string value from the Text property.</returns>
        private static string GetValue(Control c, out string key)
        {
            var textBox = c as TextBox;
            if (textBox != null)
            {
                key = null;
                return textBox.Text;
            }

            var list = c as ListControl;
            if (list != null)
            {
                if ((list is DropDownList && list.SelectedIndex == 0) || list.SelectedItem == null)
                {
                    key = null;
                    return string.Empty;
                }

                ListItem li = list.SelectedItem;
                key = li.Attributes["RelationshipKey"];
                return li.Value;
            }

            var checkBox = c as CheckBox;
            if (checkBox != null)
            {
                var radioButton = checkBox as RadioButton;
                if (radioButton != null)
                {
                    string s = radioButton.Attributes["attributevalue"];
                    if (s == null)
                    {
                        key = radioButton.Attributes["RelationshipKey"];
                        return radioButton.Checked ? radioButton.ID : null;
                    }

                    key = null;
                    return s;
                }

                key = checkBox.Attributes["RelationshipKey"];
                return checkBox.Checked.ToString().ToLower();
            }

            key = null;
            return "Control Class: " + c.GetType() + " Value unknown";
        }

        /// <summary>
        /// Captures the response.
        /// </summary>
        /// <param name="c">The control to collect id and value from.</param>
        private void CaptureResponse(Control c)
        {
            var child = c as WebControl;
            if (child != null)
            {
                string key = child.Attributes["RelationshipKey"];
                if (key == null)
                {
                    return;
                }

                Key relationshipKey = Key.ParseKeyFromString(key);

                var section = this.CurrentSurvey.GetSections().Single(s => s.SectionId == relationshipKey.SectionId);
                var question = section.GetQuestion(relationshipKey);

                string answerRelationshipKey;
                string value = GetValue(child, out answerRelationshipKey);
                if (value != null)
                {
                    if (question.Responses == null)
                    {
                        question.Responses = new List<UserResponse>();
                    }

                    Key answerKey = Key.ParseKeyFromString(answerRelationshipKey);
                    question.Responses.Add(new UserResponse { RelationshipKey = answerKey, AnswerValue = value });
                }
            }
        }

        /// <summary>
        /// Collects the response.
        /// </summary>
        /// <param name="c">The child control.</param>
        private void CollectResponse(Control c)
        {
            if (c != null)
            {
                this.CaptureResponse(c);
                this.CollectResponses(c);
            }
        }

        /// <summary>
        /// Recursive method to collects the responses.
        /// </summary>
        /// <param name="c">The parent or child control.</param>
        private void CollectResponses(Control c)
        {
            foreach (Control childControl in c.Controls)
            {
                this.CollectResponse(childControl);
            }
        }

        /// <summary>
        /// Redirects this instance.
        /// </summary>
        private void Redirect()
        {
            if (this.CurrentSurvey.FinalMessageOption == FinalMessageOption.UseFinalMessage)
            {
                this.Controls.Clear(); // remove everything.

                var thankYouDiv = new HtmlGenericControl("DIV");
                thankYouDiv.Attributes["class"] = CssClassThankYouWrap;
                this.Controls.Add(thankYouDiv);

                thankYouDiv.Controls.Add(new Label { Text = this.CurrentSurvey.FinalMessage, CssClass = CssClassThankYou });

                this.RenderBackButton(this);
            }
            else
            {
                this.Page.Response.Redirect(this.CurrentSurvey.FinalUrl);
            }
        }

        /// <summary>
        /// Renders the submit button.
        /// </summary>
        private void RenderSubmitButton()
        {
            var button = new Button
                {
                    ValidationGroup = string.Format(CultureInfo.InvariantCulture, "survey-{0}", this.CurrentSurvey.SurveyId), 
                    Text = this.SubmitButtonText, ID = "SubmitButton", 
                    CssClass = CssClassSubmitButton,
                    Enabled = this.CurrentSurvey.GetSections()[0].GetQuestions().Count > 0
                };

            // add the handler for the button
            button.Click += this.SubmitButton_Click;
            this.Controls.Add(button);
        }

        /// <summary>
        /// Renders the back button.
        /// </summary>
        /// <param name="submitDiv">The div to put the .</param>
        private void RenderBackButton(Control submitDiv)
        {
            var button = new Button { Text = this.BackButtonText, ID = "BackButton", CssClass = CssClassBackButton };
            button.Click += BackButton_Click;
            button.Visible = !string.IsNullOrEmpty(ReturnUrl);
            submitDiv.Controls.Add(button);
        }

        /// <summary>
        /// Stores the survey.
        /// </summary>
        private void WriteSurvey()
        {
            ////Save to database.
            int responseHeaderId = this.CurrentSurvey.Save(this.UserId);

            // raise event so others can act on the SurveyId created.
            this.OnSurveyCompleted(new SavedEventArgs(responseHeaderId, this.CurrentSurvey.SendNotification));

            this.Redirect();
        }

        /// <summary>
        /// Handles the Click event of the submit button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SubmitButton_Click(object sender, EventArgs e)
        {
            ////this.Page.Validate(string.Format(CultureInfo.InvariantCulture, "survey-{0}", this.CurrentSurvey.SurveyId));
            if (!this.Page.IsValid)
            {
                return;
            }

            this.CollectResponses(this);
            this.WriteSurvey();
        }

        /// <summary>
        /// A container for the <see cref="SurveyControl.Validators"/> template
        /// </summary>
        public class ValidatorsTemplateContainer : Control, INamingContainer
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ValidatorsTemplateContainer"/> class.
            /// </summary>
            /// <param name="validationGroup">The validation group.</param>
            public ValidatorsTemplateContainer(string validationGroup)
            {
                this.ValidationGroup = validationGroup;
            }

            /// <summary>
            /// Gets the validation group.
            /// </summary>
            /// <value>The validation group.</value>
            [UsedImplicitly]
            public string ValidationGroup { get; private set; }
        }
    }

    /// <summary>
    /// Event args class that contains save information about a Survey.
    /// </summary>
    public class SavedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SavedEventArgs"/> class.
        /// </summary>
        /// <param name="responseHeaderId">The response header id.</param>
        /// <param name="sendNotification">if set to <c>true</c> [send notification].</param>
        public SavedEventArgs(int responseHeaderId, bool sendNotification)
        {
            this.ResponseHeaderId = responseHeaderId;
            this.SendNotification = sendNotification;
        }

        /// <summary>
        /// Gets or sets the response id.
        /// </summary>
        /// <value>The response id.</value>
        public int ResponseHeaderId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [send notification].
        /// </summary>
        /// <value><c>true</c> if [send notification]; otherwise, <c>false</c>.</value>
        public bool SendNotification
        {
            get;
            set;
        }
    }
}
