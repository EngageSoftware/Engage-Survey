// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="EditSurvey.ascx.cs" company="Engage Software">
//  Engage Software 
// </copyright>
// <summary>
//   Defines the EditSurvey type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

namespace Engage.Dnn.Survey
{
    using System;
    using DotNetNuke.Common;
    using DotNetNuke.Services.Exceptions;

    /// <summary>
    /// The EditSurvey class is used to manage content
    /// </summary>
    partial class EditSurvey : ModuleBase
    {                
        #region Event Handlers

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(Object sender, EventArgs e)
        {
            //Framework.VirtualRootId = 1;		// Default value

            //try
            //{
            //    if (this.Request.QueryString["SurveyTypeId"] != null)
            //    {
            //        int itemId = Convert.ToInt32(Request.QueryString["SurveyTypeId"]);

            //        if (!Page.IsPostBack)
            //        {
            //            DefaultSurveyBuilder builder = new DefaultSurveyBuilder(itemId);

            //            // create the Director object
            //            SurveyDirector d = new SurveyDirector();

            //            // don't need to save these temporary objects
            //            if (this.Page.IsPostBack == false)
            //            {
            //                Framework.EnableRegistrations = false;
            //            }

            //            // ask the Director to 'constuct' using the builder
            //            d.Construct(builder);
            //            // get back the constructed Survey
            //            ISurvey survey = builder.GetSurvey();

            //            survey.Render(this.SummaryPlaceHolder, false, false, new EngageValidationProvider());
            //        }
            //    }
            //}
            //catch (Exception exc)
            //{
            //    Exceptions.ProcessModuleLoadException(this, exc);
            //}
        }

        /// <summary>
        /// Handles the Click event of the cmdCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void CancelLinkButton_Click(Object sender, EventArgs e)
        {
            try
            {
                this.Response.Redirect(Globals.NavigateURL(this.TabId), true);
            }
            catch (Exception exc) 
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Handles the Click event of the cmdDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void DeleteLinkButton_Click(Object sender, EventArgs e)
        {
            try
            {
                this.Response.Redirect(Globals.NavigateURL(this.TabId), true);
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Handles the Click event of the cmdUpdate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void UpdateLinkButton_Click(Object sender, EventArgs e)
        {
            try
            {
                // Redirect back to the portal home page
                this.Response.Redirect(Globals.NavigateURL(this.TabId), true);
            }
            catch (Exception exc) 
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

    }
}
