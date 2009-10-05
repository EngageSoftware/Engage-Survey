// <copyright file="Utility.cs" company="Engage Software">
// Engage: ContactUs - http://www.engagesoftware.com
// Copyright (c) 2004-2009
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Survey.Util
{
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Summary description for Utility.
    /// </summary>
    public static class Utility
    {

        /// <summary>
        /// CSS Class to use when no survey tyepid is defined
        /// </summary>
        public const string CssClassNoSurveyDefined = "no-survey-defined";

        /// <summary>
        /// CSS Class to use for survey title section
        /// </summary>
        public const string CssClassSurveyTitle = "survey-title";

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

        /// <summary>
        /// CSS Class to use for section wrap
        /// </summary>
        public const string CssClassSectionWrap = "section-wrap";

        /// <summary>
        /// CSS Class to use for section title.
        /// </summary>
        public const string CssClassSectionTitle = "section-title";

        /// <summary>
        /// CSS Class to use for questions
        /// </summary>
        public const string CssClassQuestion = "question";

        /// <summary>
        /// CSS Class to use for required elements.
        /// </summary>
        public const string CssClassRequired = "required";


        /// <summary>
        /// Create a web control for the survey renderer.
        /// </summary>
        /// <param name="question">The question.</param>
        /// <param name="readOnly">if set to <c>true</c> [read only].</param>
        /// <returns>A Div with controls in it.</returns>
        public static Control CreateWebControl(IQuestion question, bool readOnly)
        {
            ISection section = question.GetSection();
            
            if (question.ControlType == ControlType.DropDownChoices.Description)
            {
                return RenderDropDownList(section, question);
            }

            if (question.ControlType == ControlType.HorizontalOptionButtons.Description)
            {
                return RenderHorizontalOptionButtons(section, question);
            }

            if (question.ControlType == ControlType.VerticalOptionButtons.Description)
            {
                return RenderVerticalOptionButtons(section, question);
            }

            if (question.ControlType == ControlType.LargeTextInputField.Description)
            {
                return RenderLargeTextInputField(section, question);
            }

            if (question.ControlType == ControlType.SmallTextInputField.Description)
            {
                return RenderSmallInputField(section, question);
            }

            if (question.ControlType == ControlType.Checkbox.Description)
            {
                return RenderCheckBoxList(section, question);
            }

            Label l = new Label { Text = ("No control info found for ControlType: " + question.ControlType) };
            return l;
        }

        private static Control RenderVerticalOptionButtons(ISection section, IQuestion question)
        {
            RadioButtonList rbl = new RadioButtonList
            {
                RepeatColumns = 1,
                RepeatDirection = RepeatDirection.Vertical,
                RepeatLayout = RepeatLayout.Table,
                ID = question.QuestionId.ToString()
            };
            rbl.Attributes.Add("SectionID", section.SectionId.ToString());
            rbl.Attributes.Add("Attribute", question.RelationshipKey);
            rbl.CssClass = CssClassAnswerVertical;

            foreach (IAnswer answer in question.GetAnswerChoices())
            {
                //// 1 to skip the blank entry
                ListItem li = new ListItem(answer.Text, answer.Text);
                rbl.Items.Add(li);
            }

            return rbl;
        }

        private static Control RenderHorizontalOptionButtons(ISection section, IQuestion question)
        {
            RadioButtonList rbl = new RadioButtonList
            {
                CssClass = CssClassAnswerHorizontal,
                RepeatColumns = question.GetAnswerChoices().Count,
                RepeatDirection = RepeatDirection.Horizontal,
                RepeatLayout = RepeatLayout.Table,
                ID = (question.RelationshipKey)
            };
            rbl.Attributes.Add("SectionID", section.SectionId.ToString());
            rbl.Attributes.Add("Attribute", question.RelationshipKey);

            foreach (IAnswer answer in question.GetAnswerChoices())
            {
                // 1 to skip the blank entry
                ListItem li = new ListItem(answer.Text, answer.Text);
                rbl.Items.Add(li);

                // preselect answer, if needed
                if (string.Equals(question.ControlType, answer.Text, StringComparison.InvariantCultureIgnoreCase))
                {
                    li.Selected = true;
                }
            }

            return rbl;
        }

        private static Control RenderSmallInputField(ISection section, IQuestion question)
        {
            TextBox tb = new TextBox
            {
                TextMode = TextBoxMode.SingleLine,
                CssClass = CssClassAnswerVertical,
                MaxLength = 256
            };

            // make these a designer variable?
            tb.Attributes.Add("SectionID", section.SectionId.ToString());
            tb.Attributes.Add("Attribute", question.RelationshipKey);
            tb.ID = question.QuestionId.ToString();

            return tb;
        }

        private static Control RenderLargeTextInputField(ISection section, IQuestion question)
        {
            TextBox tb = new TextBox
            {
                TextMode = TextBoxMode.MultiLine,
                MaxLength = 256,
                CssClass = CssClassAnswerVertical,
                Columns = 25
            };

            // make these a designer variable?
            tb.Attributes.Add("SectionID", section.SectionId.ToString());
            tb.Attributes.Add("Attribute", question.RelationshipKey);
            tb.ID = question.QuestionId.ToString();

            return tb;
        }
        
        private static Control RenderCheckBoxList(ISection section, IQuestion question)
        {
            HtmlGenericControl container = new HtmlGenericControl("SPAN") { ID = ("CheckBoxSpan" + question.QuestionId) };
            container.Attributes["class"] = CssClassAnswerVertical;

            if (question.SelectionLimit > 0)
            {
                HtmlGenericControl limitDiv = new HtmlGenericControl("DIV");
                limitDiv.Attributes["class"] = "limit-reached";
                limitDiv.InnerText = "You may only select up to " + question.SelectionLimit + " items(s)";
                limitDiv.Style.Add("display", "none");
                container.Controls.Add(limitDiv);
            }

            foreach (IAnswer answer in question.GetAnswerChoices())
            {
                CheckBox cb = new CheckBox();
                cb.Attributes.Add("SectionID", section.SectionId.ToString());
                cb.Attributes.Add("Attribute", question.RelationshipKey);
                cb.ID = answer.RelationshipKey.ToString();

                cb.Text = answer.FormattedText;
                container.Controls.Add(cb);

                // preselect answer, if needed
                if (string.Equals(answer.Text, bool.TrueString, StringComparison.InvariantCultureIgnoreCase))
                {
                    cb.Checked = true;
                }
                if (question.SelectionLimit > 0)
                {
                    cb.Attributes.Add(HtmlTextWriterAttribute.Onclick.ToString(), "return CheckSelectedCount(this);");
                }
            }

            return container;

            //CheckBox check = new CheckBox { CssClass = CssClassAnswerVertical };
            //check.Attributes.Add("SectionID", section.SectionId.ToString());
            //check.Attributes.Add("Attribute", question.RelationshipKey);
            //check.ID = section.RelationshipKey;
            //check.Text = bool.TrueString;

            //// preselect answer, if needed
            //if (string.Equals(question.AnswerValue, bool.TrueString, StringComparison.InvariantCultureIgnoreCase))
            //{
            //    check.Checked = true;
            //}

            //return check;
        }

        private static Control RenderDropDownList(ISection section, IQuestion question)
        {
            DropDownList ddl = new DropDownList { CssClass = CssClassAnswerVertical };
            ddl.Items.Add(new ListItem("[Please make a selection]", string.Empty));

            ddl.Attributes.Add("SectionId", section.SectionId.ToString());
            ddl.Attributes.Add("Attribute", question.RelationshipKey);
            ddl.ID = question.RelationshipKey;

            foreach (IAnswer answer in question.GetAnswerChoices())
            {
                ListItem li = new ListItem(answer.Text, answer.Text);
                ddl.Items.Add(li);
            }

            return ddl;
        }
    }
}