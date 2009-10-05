// <copyright file="EditableLayout.cs" company="Engage Software">
// Engage: ContactUs - http://www.engagesoftware.com
// Copyright (c) 2004-2009
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Survey
{
    using System.Text;
    using System.Web;
    using System.Web.UI.WebControls;
    using DotNetNuke.Common;
    using Engage.Survey;

    public class EditableLayout: LayoutStrategy 
    {
        public EditableLayout(ISurvey survey, PlaceHolder ph)
            : base(survey, ph)
        {
        }

        public override void Render()
        {
            RegisterScript();

            RenderTitle();
            RenderSurvey();
        }

        private void RegisterScript()
        {
            StringBuilder sb = new StringBuilder(256);

            sb.Append("<script language='javascript'>");
            sb.Append("function ConfirmQuestionDelete()");
            sb.Append("{");
            sb.Append("var x = confirm('Are you sure you want to delete this Question?');");
            sb.Append("return x;");
            sb.Append("}");
            sb.Append("function ConfirmAnswerDelete()");
            sb.Append("{");
            sb.Append("var x = confirm('Are you sure you want to delete this Answer?');");
            sb.Append("return x;");
            sb.Append("}");
            sb.Append("</script>");

            GetPlaceHolder.Page.RegisterClientScriptBlock("Delete", sb.ToString());
        }

        private void RenderTitle()
        {

            Table table = new Table {Width = new Unit("100%")};
            //table.GridLines = GridLines.Both
            GetPlaceHolder.Controls.Add(table);

            //TableRow row = new TableRow();
            //TableCell cell = new TableCell {ColumnSpan = 2};
            //Label lbl = new Label {CssClass = "title", Text = GetSurvey.FormattedText};
            //row.Cells.Add(cell);
            //cell.Controls.Add(lbl);
            //table.Rows.Add(row);

            //blank
            //row = new TableRow();
            //cell = new TableCell {ColumnSpan = 2};
            //row.Cells.Add(cell);
            //table.Rows.Add(row);

            ////add program stuff
            //row = new TableRow();
            //cell = new TableCell {ColumnSpan = 2};
            //lbl = new Label {Text = "SURVEY SUMMARy"};
            //row.Cells.Add(cell);
            //cell.Controls.Add(lbl);
            //table.Rows.Add(row);

            ////Test Name Row
            //row = new TableRow();
            ////add the edit link
            //cell = new TableCell {CssClass = "", Width = new Unit(20)};
            //cell.Controls.Add(CreateSurveyEditUrl());
            //row.Cells.Add(cell);

            ////add  Name
            //cell = new TableCell
            //cell.CssClass = "testTitle"
            //lbl = new Label
            //lbl.Text = CurrentTest.Description
            //row.Cells.Add(cell)
            //cell.Controls.Add(lbl)
            //table.Rows.Add(row)

            //blank
            //row = new TableRow();
            //cell = new TableCell();
            //row.Cells.Add(cell);
            //table.Rows.Add(row);
        }

         private static HyperLink CreateSurveyEditUrl()
         {

            SurveyQueryStringBuilder qs = new SurveyQueryStringBuilder {TargetControl = "testeditor", Mode = Mode.Edit};
             qs.Add("otid", HttpContext.Current.Request.QueryString["otid"]);
            qs.PreviousControl = qs.PreviousControl;

            string href = Globals.NavigateURL(qs.Tabid, "", qs.Parameters);

            HyperLink link = new HyperLink {ImageUrl = (Globals.ApplicationPath + "/images/edit.gif"), NavigateUrl = href};

             return link;

         }

         private void RenderSurvey()
         {
             Table table = new Table {Width = new Unit("100%")};
             //table.GridLines = GridLines.Both;
             GetPlaceHolder.Controls.Add(table);

             TableRow row = new TableRow();
             table.Rows.Add(row);

             TableCell cell = new TableCell {CssClass = "", Text = "&nbsp;", Width = new Unit(20)};
             row.Cells.Add(cell);

             //add question link
             cell = new TableCell {CssClass = "", ColumnSpan = 5};
             //cell.Controls.Add(CreateQuestionAddLink(true));
             //cell.Controls.Add(CreateQuestionAddLink(false));
             row.Cells.Add(cell);

             //add a row after to give some room.
             row = new TableRow();
             cell = new TableCell {Text = "&nbsp;", ColumnSpan = 6};
             row.Cells.Add(cell);
             table.Rows.Add(row);

             ////Code from Swank LMS as example. hk
             ////    int i;
             ////    GetSurvey.GetSections()

             ////    Dim questions As Question() = CurrentTest.GetQuestions()
             ////    For Each q As Question In questions
             ////        i += 1
             ////        row = new TableRow
             ////        table.Rows.Add(row)

             ////        cell = new TableCell
             ////        cell.CssClass = ""
             ////        cell.Text = "&nbsp;"
             ////        cell.Width = new Unit(20)
             ////        cell.Controls.Add(CreateQuestionEditUrl(q))
             ////        row.Cells.Add(cell)

             ////        cell = new TableCell
             ////        cell.CssClass = ""
             ////        cell.Text = "&nbsp;"
             ////        cell.Controls.Add(CreateQuestionDeleteUrl(q))
             ////        row.Cells.Add(cell)

             ////        cell = new TableCell
             ////        cell.ColumnSpan = 3
             ////        cell.CssClass = "testQuestion"
             ////        Dim lbl As new Label
             ////        lbl.EnableViewState = False
             ////        lbl.Text = i.ToString() + ". " + q.Text
             ////        cell.Controls.Add(lbl)
             ////        row.Cells.Add(cell)

             ////        'add the up and down arrows
             ////        cell = new TableCell
             ////        cell.CssClass = ""
             ////        cell.Width = new Unit(20)
             ////        cell.Controls.Add(CreateArrowUpLink(q))
             ////        cell.Controls.Add(CreateArrowDownLink(q))
             ////        row.Cells.Add(cell)

             ////        Select Case q.ControlTypeUid
             ////            Case LookupType.ControlType.GetLookup("Horizontal Radio Buttons").GetID()
             ////                RenderRadioButtons(q, row)
             ////            Case LookupType.ControlType.GetLookup("Vertical Radio Buttons").GetID()
             ////                RenderRadioButtons(q, row)
             ////            Case LookupType.ControlType.GetLookup("Small Text Field").GetID()
             ////                RenderSmallTextBox(q, row, True)
             ////            Case LookupType.ControlType.GetLookup("Large Text Area").GetID()
             ////                RenderLargeTextBox(q, row, True)
             ////            Case LookupType.ControlType.GetLookup("Dropdown List").GetID()
             ////                RenderRadioButtons(q, row)
             ////        End Select

             ////        'Since we are displaying the test in "Edit" mode we always display the answers veritcally.
             ////        'Phase II we could add logic to display using the correct control type as it would be 
             ////        'displayed when taken. HK
             ////        'RenderRadioButtons(PlaceHolder, q, row)
             ////        'End Select

             ////        'add a row after to give some room.
             ////        row = new TableRow
             ////        table.Rows.Add(row)
            ////    Next
         }
    }
}
