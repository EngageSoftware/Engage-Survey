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
    using System.Text;
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
            WebControl control = null;
            if (question.ControlType == ControlType.DropDownChoices.Description)
            {
                control = (WebControl)RenderDropDownList(question);
            }

            if (question.ControlType == ControlType.HorizontalOptionButtons.Description)
            {
                control = (WebControl)RenderHorizontalOptionButtons(question);
            }

            if (question.ControlType == ControlType.VerticalOptionButtons.Description)
            {
                control = (WebControl)RenderVerticalOptionButtons(question);
            }

            if (question.ControlType == ControlType.LargeTextInputField.Description)
            {
                control = (WebControl)RenderLargeTextInputField(question);
            }

            if (question.ControlType == ControlType.SmallTextInputField.Description)
            {
                control = (WebControl)RenderSmallInputField(question);
            }

            if (question.ControlType == ControlType.Checkbox.Description)
            {
                return RenderCheckBoxList(question, readOnly);
            }

            if (control == null)
            {
                control = new Label { Text = ("No control info found for ControlType: " + question.ControlType) };
            }

            control.Enabled = !readOnly;
            return control;
        }

        /// <summary>
        /// Renders the vertical option buttons.
        /// </summary>
        /// <param name="question">The question.</param>
        /// <returns></returns>
        private static Control RenderVerticalOptionButtons(IQuestion question)
        {
            RadioButtonList rbl = new RadioButtonList
            {
                RepeatColumns = 1,
                RepeatDirection = RepeatDirection.Vertical,
                RepeatLayout = RepeatLayout.Table,
                ID = question.RelationshipKey.ToString()
            };
            rbl.Attributes.Add("RelationshipKey", question.RelationshipKey.ToString());
            rbl.CssClass = CssClassAnswerVertical;

            foreach (IAnswer answer in question.GetAnswers())
            {
                //// 1 to skip the blank entry
                ListItem li = new ListItem(answer.Text, answer.Text);
                li.Attributes.Add("RelationshipKey", answer.RelationshipKey.ToString());
                rbl.Items.Add(li);

                // preselect answer, if needed
                if (question.Responses != null)
                {
                    if (string.Equals(answer.Text, question.FindResponse(answer).AnswerValue, StringComparison.InvariantCultureIgnoreCase))
                    {
                        li.Selected = true;
                    }
                }
            }

            return rbl;
        }

        /// <summary>
        /// Renders the horizontal option buttons.
        /// </summary>
        /// <param name="question">The question.</param>
        /// <returns></returns>
        private static Control RenderHorizontalOptionButtons(IQuestion question)
        {
            RadioButtonList rbl = new RadioButtonList
            {
                CssClass = CssClassAnswerHorizontal,
                RepeatColumns = question.GetAnswers().Count,
                RepeatDirection = RepeatDirection.Horizontal,
                RepeatLayout = RepeatLayout.Table,
                ID = question.RelationshipKey.ToString()
            };
            rbl.Attributes.Add("RelationshipKey", question.RelationshipKey.ToString());

            foreach (IAnswer answer in question.GetAnswers())
            {
                // 1 to skip the blank entry
                ListItem li = new ListItem(answer.Text, answer.Text);
                li.Attributes.Add("RelationshipKey", answer.RelationshipKey.ToString());
                rbl.Items.Add(li);

                // preselect answer, if needed
                if (question.Responses != null)
                {
                    if (string.Equals(answer.Text, question.FindResponse(answer).AnswerValue, StringComparison.InvariantCultureIgnoreCase))
                    {
                        li.Selected = true;
                    }
                }
            }

            return rbl;
        }

        /// <summary>
        /// Renders the small input field.
        /// </summary>
        /// <param name="question">The question.</param>
        /// <returns></returns>
        private static Control RenderSmallInputField(IQuestion question)
        {
            TextBox tb = new TextBox
            {
                TextMode = TextBoxMode.SingleLine,
                CssClass = CssClassAnswerVertical,
                MaxLength = 256
            };

            // make these a designer variable?
            tb.Attributes.Add("RelationshipKey", question.RelationshipKey.ToString());
            tb.ID = question.RelationshipKey.ToString();

            //pre-select if needed
            if (question.Responses != null && question.Responses.Count == 1)
            {
                tb.Text = question.Responses[0].AnswerValue;
            }

            return tb;
        }

        /// <summary>
        /// Renders the large text input field.
        /// </summary>
        /// <param name="question">The question.</param>
        /// <returns></returns>
        private static Control RenderLargeTextInputField(IQuestion question)
        {
            TextBox tb = new TextBox
            {
                TextMode = TextBoxMode.MultiLine,
                MaxLength = 256,
                CssClass = CssClassAnswerVertical,
                Columns = 25
            };

            tb.Attributes.Add("RelationshipKey", question.RelationshipKey.ToString());
            tb.ID = question.RelationshipKey.ToString();

            //pre-select if needed
            if (question.Responses != null && question.Responses.Count == 1)
            {
                tb.Text = question.Responses[0].AnswerValue;
            }
            return tb;
        }

        /// <summary>
        /// Renders the check box list.
        /// </summary>
        /// <param name="question">The question.</param>
        /// <param name="readOnly">if set to <c>true</c> [read only].</param>
        /// <returns></returns>
        private static Control RenderCheckBoxList(IQuestion question, bool readOnly)
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

            foreach (IAnswer answer in question.GetAnswers())
            {
                CheckBox cb = new CheckBox();
                cb.Attributes.Add("RelationshipKey", answer.RelationshipKey.ToString());
                cb.ID = answer.RelationshipKey.ToString();
                cb.Enabled = !readOnly;
                cb.Text = answer.FormattedText;
                container.Controls.Add(cb);

                // preselect answer, if needed 
                if (question.Responses != null)
                {
                    if (string.Equals(question.FindResponse(answer).AnswerValue, bool.TrueString, StringComparison.InvariantCultureIgnoreCase))
                    {
                        cb.Checked = true;
                    }
                }

                if (question.SelectionLimit > 0)
                {
                    cb.Attributes.Add(HtmlTextWriterAttribute.Onclick.ToString(), "return CheckSelectedCount(this);");
                }
            }
            
            return container;
        }

        /// <summary>
        /// Renders the drop down list.
        /// </summary>
        /// <param name="question">The question.</param>
        /// <returns></returns>
        private static Control RenderDropDownList(IQuestion question)
        {
            DropDownList ddl = new DropDownList { CssClass = CssClassAnswerVertical };
            ddl.Items.Add(new ListItem("[Please make a selection]", string.Empty));
          
            ddl.Attributes.Add("RelationshipKey", question.RelationshipKey.ToString());
            ddl.ID = question.RelationshipKey.ToString();
            foreach (IAnswer answer in question.GetAnswers())
            {
                ListItem li = new ListItem(answer.Text, answer.Text);
                li.Attributes.Add("RelationshipKey", answer.RelationshipKey.ToString());
                ddl.Items.Add(li);

                //preselect the answer, if needed
                if (question.Responses != null)
                {
                    if (string.Equals(question.FindResponse(answer).AnswerValue, answer.Text))
                    {
                        li.Selected = true;
                    }
                }
            }

            return ddl;
        }

        /// <summary>
        /// Converts the number to character.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ConvertNumberToCharacter(int value)
        {

            switch (value)
            {
                case 1:
                    return "A";
                case 2:
                    return "B";
                case 3:
                    return "C";
                case 4:
                    return "D";
                case 5:
                    return "E";
                case 6:
                    return "F";
                case 7:
                    return "G";
                case 8:
                    return "H";
                case 9:
                    return "I";
                case 10:
                    return "J";
                case 11:
                    return "K";
                case 12:
                    return "L";
                case 13:
                    return "M";
                case 14:
                    return "N";
                case 15:
                    return "O";
                case 16:
                    return "P";
                case 17:
                    return "Q";
                case 18:
                    return "R";
                case 19:
                    return "S";
                case 20:
                    return "T";
                case 21:
                    return "U";
                case 22:
                    return "V";
                case 23:
                    return "W";
                case 24:
                    return "X";
                case 25:
                    return "Y";
                case 26:
                    return "Z";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Converts the number to roman numeral.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ConvertNumberToRomanNumeral(int value)
        {
            try
            {
                int number = Convert.ToInt32(value);
                StringBuilder retval = new StringBuilder();
                if (number < 1 || number > 5000)
                {
                    return ("Arabic out of range!");
                }
                retval.Append(GenerateNumber(ref number, 1000, 'M'));
                retval.Append(GenerateNumber(ref number, 500, 'D'));
                retval.Append(GenerateNumber(ref number, 100, 'C'));
                retval.Append(GenerateNumber(ref number, 50, 'L'));
                retval.Append(GenerateNumber(ref number, 10, 'X'));
                retval.Append(GenerateNumber(ref number, 5, 'V'));
                retval.Append(GenerateNumber(ref number, 1, 'I'));
                //let's replace the some substrings like:
                //IIII to IV, VIV to IX, etc.
                retval.Replace("IIII", "IV");
                retval.Replace("VIV", "IX");
                retval.Replace("XXXX", "XL");
                retval.Replace("LXL", "XC");
                retval.Replace("CCCC", "CD");
                retval.Replace("DCD", "CM");
                return (retval.ToString());
            }
            catch
            {
                return ("Arabic value not correct!");
            }
        }

        /// <summary>
        /// Generates the number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="magnitude">The magnitude.</param>
        /// <param name="letter">The letter.</param>
        /// <returns></returns>
        private static string GenerateNumber(ref int value, int magnitude, char letter)
        {
            StringBuilder numberstring = new StringBuilder();
            while (value >= magnitude)
            {
                value -= magnitude;
                numberstring.Append(letter);
            }
            return (numberstring.ToString());
        }

        /// <summary>
        /// Prepends the formatting.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="relativeOrder">The relative order.</param>
        /// <returns></returns>
        public static string PrependFormatting(ElementFormatOptions option, int relativeOrder)
        {
            if (option.Description == ElementFormatOptions.Numbered.Description)
            {
                return relativeOrder + ". ";
            }
            if (option.Description == ElementFormatOptions.Lettered.Description)
            {
                return ConvertNumberToCharacter(relativeOrder) + ". ";
            }
            if (option.Description == ElementFormatOptions.Roman.Description)
            {
                return ConvertNumberToRomanNumeral(relativeOrder) + ". ";
            }
            return string.Empty;
        }
    }
}