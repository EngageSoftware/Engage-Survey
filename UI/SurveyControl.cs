// <copyright file="SurveyControl.ascx.cs" company="Engage Software">
// Engage: ContactUs - http://www.engagesoftware.com
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
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
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
        public delegate void SaveEventHandler(object sender, SavedEventArgs e);

        public event SaveEventHandler SurveyCompleted;

        /// <summary>
        /// CSS Class to use when no survey tyepid is defined
        /// </summary>
        public const string CssClassNoSurveyDefined = "no-survey-defined";

        /// <summary>
        /// CSS Class to use for survey title section
        /// </summary>
        public const string CssClassSurveyTitle = "survey-title";

        /// <summary>
        /// CSS Class to use for section title.
        /// </summary>
        public const string CssClassSectionTitle = "section-title";

        /// <summary>
        /// CSS Class to use for required elements.
        /// </summary>
        public const string CssClassRequired = "required";

        /// <summary>
        /// CSS Class to use for questions
        /// </summary>
        public const string CssClassQuestion = "question";

        /// <summary>
        /// CSS Class to use for section wrap
        /// </summary>
        public const string CssClassSectionWrap = "section-wrap";

        /// <summary>
        /// CSS Class to use for submit button
        /// </summary>
        public const string CssClassImageButtonSubmit = "submit-image";

        /// <summary>
        /// CSS Class to use for horizontal answers
        /// </summary>
        public const string CssClassAnswerHorizontal = "answer-horizontal";

        /// <summary>
        /// CSS Class to use for horizontal answers
        /// </summary>
        public const string CssClassAnswerVertical = "answer-vertical";

        /// <summary>
        /// CSS Class to use for submit area at bottom
        /// </summary>
        public const string CssClassSubmitArea = "submit-area";

        protected virtual void OnSurveyCompleted(SavedEventArgs e)
        {
            if (SurveyCompleted != null)
                SurveyCompleted(this, e);
        }

        /// <summary>
        /// The survey that is being rendered
        /// </summary>
        public ISurvey CurrentSurvey
        {
            get;
            set;
        }

        /// <summary>
        /// Adds a reference to jQuery 1.2.6 (minified) to the page.  If you can reference DNN 5.0 or higher, use the built-in methods to register jQuery, instead.
        /// </summary>
        /// <remarks>
        /// Adds the reference to the head of the page, rather than using <see cref="ClientScriptManager.RegisterClientScriptResource"/>
        /// thus "guaranteeing" that it will run before other scripts
        /// that might not react well to losing their $ reference.
        /// Also checks for duplicates, only adding the reference once regardless of how many times this method is called for the <paramref name="page"/>.
        /// </remarks>
        /// <param name="page">The page to which the reference will be added.</param>
        public static void AddJQueryReference(Page page)
        {
            const string JavaScriptReferenceFormat = @"<script src='{0}' type='text/javascript'></script>";
            const string JQueryRegistrationKey = "jQuery Registration key";
            if (!page.ClientScript.IsClientScriptBlockRegistered(typeof(SurveyControl), JQueryRegistrationKey))
            {
                string scriptReference = string.Format(CultureInfo.InvariantCulture, JavaScriptReferenceFormat, page.ClientScript.GetWebResourceUrl(typeof(SurveyControl), "Engage.Survey.JavaScript.jquery-1.2.6.min.js"));
                page.Header.Controls.Add(new LiteralControl(scriptReference));
                page.ClientScript.RegisterStartupScript(typeof(SurveyControl), JQueryRegistrationKey, "jQuery(function($){$.noConflict();});", true);
            }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="c">The control to inspect</param>
        /// <returns>The string value from the Text property.</returns>
        private static string GetValue(Control c)
        {
            if (c is TextBox)
            {
                TextBox tb = (TextBox)c;
                return tb.Text == string.Empty ? null : tb.Text;
            }

            if (c is DropDownList)
            {
                DropDownList ddl = (DropDownList)c;
                if (ddl.SelectedIndex == 0)
                {
                    return null;
                }

                ListItem li = ddl.SelectedItem;
                return li.Value;
            }

            if (c is CheckBox)
            {
                CheckBox cb = (CheckBox)c;
                if (c is RadioButton)
                {
                    string s = cb.Attributes["attributevalue"];
                    if (s == null)
                    {
                        return cb.Checked ? cb.ID : null;
                    }

                    return s;
                }

                return cb.Checked.ToString().ToLower();
            }

            if (c is CheckBoxList)
            {
                CheckBoxList cbl = (CheckBoxList)c;
                return cbl.SelectedValue;
            }

            if (c is RadioButtonList)
            {
                RadioButtonList rbl = (RadioButtonList)c;
                return rbl.SelectedValue;
            }

            return "Control Class: " + c.GetType() + " Value unknown";
        }       

        /// <summary>
        /// Gets or sets a value indicating whether [show required notation].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [show required notation]; otherwise, <c>false</c>.
        /// </value>
        [Bindable(true), Category("Validation"), DefaultValue("")]    
        public bool ShowRequiredNotation { get; set; }

        /// <summary>
        /// Gets or sets the validation provider.
        /// </summary>
        /// <value>The validation provider.</value>
        [Bindable(true), Category("Validation"), DefaultValue("")]
        public ValidationProviders ValidationProvider { get; set; }
        
        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            this.Controls.Clear();

            HtmlGenericControl mainDiv = new HtmlGenericControl("DIV");
            mainDiv.Attributes["class"] = this.CssClass;

            // this.table1.GridLines = GridLines.Both;
            this.Controls.Add(mainDiv);

            PlaceHolder ph = new PlaceHolder();
            this.Controls.Add(ph);
            if (this.CurrentSurvey == null)
            {
                HtmlGenericControl noSurveyDiv = new HtmlGenericControl("DIV");
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
            this.CurrentSurvey.Render(ph, false, this.ShowRequiredNotation, new EngageValidationProvider());

            this.RenderSubmitButton();
        }
        
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            AddJQueryReference(this.Page);
        }

        /// <summary>
        /// Writes the <see cref="T:System.Web.UI.WebControls.CompositeControl"/> content to the specified <see cref="T:System.Web.UI.HtmlTextWriter"/> object, for display on the client.
        /// </summary>
        /// <param name="writer">An <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Page.ClientScript.IsStartupScriptRegistered("ValidatorOverrideScripts") == false)
            {
                const string ValidatorOverrideScripts = "<script src=\"/javascript/validators.js\" type=\"text/javascript\"></script>";
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "ValidatorOverrideScripts", ValidatorOverrideScripts, false);
            }
            
            base.Render(writer);

            //if (this.Page.IsPostBack)
            //{
            //    this.Page.Validate();
            //    if (this.Page.IsValid)
            //    {
            //        this.CollectResponses(this);

            //        this.StoreSurvey();
            //    }
            //    else
            //    {
            //        base.Render(writer);
            //    }
            //}
            //else
            //{
            //    base.Render(writer);
            //}
        }
                       
        /// <summary>
        /// Renders the submit image.
        /// </summary>
        private void RenderSubmitButton()
        {
            HtmlGenericControl submitDiv = new HtmlGenericControl("DIV");
            submitDiv.Attributes["class"] = CssClassSubmitArea;
            this.Controls.Add(submitDiv);

            Button button = new Button
                                {
                                        ValidationGroup = "survey",
                                        Text = "Submit",
                                        ID = "btnSubmit",
                                        CssClass = CssClassImageButtonSubmit
                                };

            //add the handler for the button
            button.Click += this.btnSubmit_Click;
            submitDiv.Controls.Add(button);
        }

        /// <summary>
        /// Handles the Click event of the btnSubmit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            this.Page.Validate();
            if (this.Page.IsValid)
            {
                this.CollectResponses(this);

                this.WriteSurvey();
            }
        }

        /// <summary>
        /// Recursive method to collects the responses.
        /// </summary>
        /// <param name="c">The parent or child control.</param>
        private void CollectResponses(Control c)
        {
            IEnumerator ie = c.Controls.GetEnumerator();
            while (ie.MoveNext())
            {
                this.CollectResponse((Control)ie.Current);               
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
        /// Captures the response.
        /// </summary>
        /// <param name="c">The control to collect id and value from.</param>
        private void CaptureResponse(Control c)
        {
            WebControl child = c as WebControl;
            if (child != null)
            {
                int sectionID = Convert.ToInt32(child.Attributes["SectionID"]);
                string renderingKey = child.Attributes["Attribute"];

                ////find this attribute and update the value
                foreach (ISection section in this.CurrentSurvey.GetSections())
                {
                    if (section.SectionId == sectionID)
                    {
                        // have the correct section, get the question
                        IQuestion question = section.GetQuestion(renderingKey);

                        string value = GetValue(child);
                        if (value != null)
                        {
                            question.AnswerValue = value;
                            break;
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Redirects this instance.
        /// </summary>
        private void Redirect()
        {
            if (this.CurrentSurvey.FinalMessageOption == FinalMessageOptions.UseFinalMessage.Description)
            {
                this.Page.Response.Redirect("ThankYou.aspx?typeid=" + this.CurrentSurvey.SurveyId);
            }
            else
            {
                this.Page.Response.Redirect(this.CurrentSurvey.FinalUrl);
            }
        }

        /// <summary>
        /// Stores the survey.
        /// </summary>
        private void WriteSurvey()
        {
            ////Save to database.
            int id = CurrentSurvey.Save();
            //raise event so others can act on the SurveyId created.
            this.OnSurveyCompleted(new SavedEventArgs(id));
            

            this.Redirect();
        }
    }

    public class SavedEventArgs : EventArgs
    {
        public SavedEventArgs(int surveyId)
        {
            this.SurveyId = surveyId;    
        }

        // Properties.
        public int SurveyId
        {
            get;
            set;
        }
    }

}