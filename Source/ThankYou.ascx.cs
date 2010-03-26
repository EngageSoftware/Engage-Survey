// <copyright file="ThankYou.ascx.cs" company="Engage Software">
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
    using DotNetNuke.Services.Exceptions;

    /// <summary>
    /// The ThankYou control
    /// </summary>
    partial class ThankYou : ModuleBase
    {
        
        #region Event Handlers

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(Object sender, EventArgs e)
        {
            //Framework.VirtualRootId = 1;		//Default value
            //try
            //{
            //    if (this.Request.QueryString["typeId"] != null)
            //    {
            //        int itemId = Convert.ToInt32(Request.QueryString["typeId"]);

            //        if (!Page.IsPostBack)
            //        {
            //            DefaultSurveyBuilder builder = new DefaultSurveyBuilder(itemId);
            //            //create the Director object
            //            SurveyDirector d = new SurveyDirector();

            //            //don't need to save these temporary objects
            //            Framework.EnableRegistrations = false;

            //            //ask the Director to 'constuct' using the builder
            //            d.Construct(builder);
            //            //get back the constructed Survey
            //            ISurvey survey = builder.GetSurvey();

            //            //get message.
            //            this.ThankYouLabel.Text = survey.FinalMessage;
            //        }
            //    }
            //}
            //catch (Exception exc) //Module failed to load
            //{
            //    Exceptions.ProcessModuleLoadException(this, exc);
            //}
        }
        
        #endregion

    }
}

