// <copyright file="SurveyControl.cs" company="Engage Software">
// Engage: Survey
// Copyright (c) 2004-2009
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
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
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
        /// CSS Class to use for horizontal answers
        /// </summary>
        public const string CssClassAnswerHorizontal = "answer-horizontal";

        /// <summary>
        /// CSS Class to use for horizontal answers
        /// </summary>
        public const string CssClassAnswerVertical = "answer-vertical";

        /// <summary>
        /// CSS Class to use for submit button
        /// </summary>
        public const string CssClassSubmitButton = "submit-button";

        /// <summary>
        /// CSS Class to use for submit button
        /// </summary>
        public const string CssClassBackButton = "back-button";

        /// <summary>
        /// CSS Class to use when no survey typeId is defined
        /// </summary>
        public const string CssClassNoSurveyDefined = "no-survey-defined";

        /// <summary>
        /// CSS Class to use for questions
        /// </summary>
        public const string CssClassQuestion = "question";

        /// <summary>
        /// CSS Class to use for required elements.
        /// </summary>
        public const string CssClassRequired = "required";

        /// <summary>
        /// CSS Class to use for section title.
        /// </summary>
        public const string CssClassSectionTitle = "section-title";

        /// <summary>
        /// CSS Class to use for section wrap
        /// </summary>
        public const string CssClassSectionWrap = "section-wrap";

        /// <summary>
        /// CSS Class to use for submit area at bottom
        /// </summary>
        public const string CssClassSubmitArea = "submit-area";

        /// <summary>
        /// CSS Class to use for survey title section
        /// </summary>
        public const string CssClassSurveyTitle = "survey-title";

        /// <summary>
        /// Marker for end survey.
        /// </summary>
        public const string EndSurveyMarker = "<!--survey_ends_here-->";

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

        /// <summary>
        /// Gets or sets the survey that is being rendered.
        /// </summary>
        /// <remarks>The <see cref="ISurvey.Save"/> method will be called when the Submit button is clicked.</remarks>
        public ISurvey CurrentSurvey
        {
            get;
            set;
        }

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
                return this.CurrentSurvey.IsReadonly;
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
            get;
            set;
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
            get;
            set;
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
                var noSurveyDiv = new HtmlGenericControl("DIV");
                noSurveyDiv.Attributes["class"] = CssClassNoSurveyDefined;

                return;
            }

            string thanks = this.Context.Request.QueryString["thankyou"];
            if (thanks == "true")
            {
                this.Visible = false;
                return;
            }

            // Need to make validator construction mechanism as we create new implementations! hk
            // draw the survey
            this.CurrentSurvey.Render(ph, this.IsReadOnly, this.ShowRequiredNotation, new EngageValidationProvider());

            // no need to include the submit button in html
            if (!this.IsReadOnly)
            {
                this.RenderSubmitButton();
            }

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
        /// Gets the value.
        /// </summary>
        /// <param name="c">The control to inspect</param>
        /// <param name="key">The key.</param>
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

                var section = this.CurrentSurvey.GetSections().Where(s => s.SectionId == relationshipKey.SectionId).Single();
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
                this.Controls.Add(new Literal { Text = this.CurrentSurvey.FinalMessage });
                this.RenderBackButton(null);
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
            var submitDiv = new HtmlGenericControl("DIV");
            submitDiv.Attributes["class"] = CssClassSubmitArea;
            this.Controls.Add(submitDiv);

            this.RenderBackButton(submitDiv); 

            var button = new Button { ValidationGroup = "survey", Text = SubmitButtonText, ID = "SubmitButton", CssClass = CssClassSubmitButton };

            // add the handler for the button
            button.Click += this.SubmitButton_Click;
            submitDiv.Controls.Add(button);
        }

        /// <summary>
        /// Renders the back button.
        /// </summary>
        private void RenderBackButton(HtmlGenericControl submitDiv)
        {
            if (submitDiv == null)
            {
                submitDiv = new HtmlGenericControl("DIV");
                submitDiv.Attributes["class"] = CssClassSubmitArea;
                this.Controls.Add(submitDiv);
            }

            var button = new Button { Text = BackButtonText, ID = "BackButton", CssClass = CssClassBackButton };
            button.Click += BackButton_Click;
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
            this.Page.Validate();
            if (this.Page.IsValid)
            {
                this.CollectResponses(this);

                this.WriteSurvey();
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
        /// Gets the return URL if on the querystring.
        /// </summary>
        /// <value>The return URL.</value>
        private static string ReturnUrl
        {
            get
            {
                return HttpContext.Current.Request.QueryString["returnurl"] ?? String.Empty;
            }
        }
    }

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