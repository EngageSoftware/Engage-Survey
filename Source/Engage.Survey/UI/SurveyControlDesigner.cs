// <copyright file="SurveyControlDesigner.ascx.cs" company="Engage Software">
// Engage: Survey
// Copyright (c) 2004-2014
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Survey.UI
{
    using System.ComponentModel;

    /// <summary>
    /// Summary description for SurveyControlDesigner.
    /// </summary>

    [Designer("Engage.Survey.Web.SurveControlDesigner, Engage.Survey.Web")]
    public class SurveyControlDesigner : System.Web.UI.Design.ControlDesigner 
    {
        public override string GetDesignTimeHtml() 
        {
            return "Survey Viewer Control. Be sure that this control is configured with the correct SurveyTypeid.";
			
            //			// Component is the instance of the component or control that
            //			// this designer object is associated with. This property is 
            //			// inherited from System.ComponentModel.ComponentDesigner.
            //			SurveyControl simple = (SurveyControl) Component;
            //
            //			if (simple.CompletionUrl.Length > 0) 
            //			{
            //				StringWriter sw = new StringWriter();
            //				HtmlTextWriter tw = new HtmlTextWriter(sw);
            //
            //				HyperLink placeholderLink = new HyperLink();
            //            
            //				// Put simple.Text into the link's Text.
            //				placeholderLink.Text = simple.CompletionUrl;
            //				placeholderLink.NavigateUrl = simple.CompletionUrl;
            //				placeholderLink.RenderControl(tw);
            //
            //				return sw.ToString();
            //			}
            //			else
            //				return GetEmptyDesignTimeHtml();
        }
    }
}