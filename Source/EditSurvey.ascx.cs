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
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(Object sender, EventArgs e)
        {
            this.AddJQueryReference();
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
    }
}