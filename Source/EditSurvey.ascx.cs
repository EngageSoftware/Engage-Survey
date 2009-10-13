// <copyright file="EditSurvey.ascx.cs" company="Engage Software">
// Engage: Survey
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
    using System;
    using System.Web.UI;

    /// <summary>
    /// The EditSurvey class is used to manage content
    /// </summary>
    public partial class EditSurvey : ModuleBase
    {
        /// <summary>
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.AddJQueryReference();
            this.Page.ClientScript.RegisterClientScriptResource(typeof(EditSurvey), "Engage.Dnn.Survey.JavaScript.survey.js");
            base.OnInit(e);
        }
    }
}