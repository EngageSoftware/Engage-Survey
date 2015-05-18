// <copyright file="GlobalNavigation.ascx.cs" company="Engage Software">
// Engage: Survey
// Copyright (c) 2004-2015
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
    using DotNetNuke.Common;
    using DotNetNuke.Security;
    using DotNetNuke.Security.Permissions;

    /// <summary>
    /// A navigation header visible to administrators
    /// </summary>
    public partial class GlobalNavigation : ModuleBase
    {
        /// <summary>
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);
        }

        /// <summary>
        /// Handles the <see cref="Control.Load"/> event of this control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            this.Visible = this.IsAdmin;

            this.HomeLink.NavigateUrl = Globals.NavigateURL();
            this.AddNewLink.NavigateUrl = this.BuildLinkUrl(this.ModuleId, ControlKey.EditSurvey);
            this.ManageLink.NavigateUrl = this.BuildLinkUrl(this.ModuleId, ControlKey.SurveyListing);
            this.SettingsLink.NavigateUrl = Globals.NavigateURL(this.TabId, "Module", "ModuleId=" + this.ModuleId);

            this.HomeLink.ToolTip = this.Localize("HomeLink.ToolTip");
            this.AddNewLink.ToolTip = this.Localize("AddNewLink.ToolTip");
            this.ManageLink.ToolTip = this.Localize("ManageLink.ToolTip");
            this.SettingsLink.ToolTip = this.Localize("SettingsLink.ToolTip");

            // Settings is actually accessible via URL with SecurityAccessLevel.Edit, but .Admin is the check that ModuleInstanceContext does when adding to the action menu
            var isSettingsAvailableInModuleActions = ModulePermissionController.HasModuleAccess(SecurityAccessLevel.Admin, "MANAGE", this.ModuleConfiguration);
            this.SettingsListItem.Visible = isSettingsAvailableInModuleActions;
            this.ManageListItem.Visible = ModuleSettings.DisplayType.GetValueAsEnumFor<ControlKey>(this) != ControlKey.SurveyListing;
        }
    }
}