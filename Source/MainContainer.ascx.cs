// <copyright file="MainContainer.ascx.cs" company="Engage Software">
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
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using DotNetNuke.Common;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using Framework;

    /// <summary>
    /// MainContainer for project.
    /// </summary>
    public partial class MainContainer : ModuleBase
    {
        /// <summary>
        /// The control key for the <see cref="DefaultSubControl"/>
        /// </summary>
        protected internal static readonly ControlKey DefaultControlKey = ControlKey.SurveyListing;

        /// <summary>
        /// The default sub-control to load when no control key is provided and no other default is set in the module settings
        /// </summary>
        private static readonly SubControlInfo DefaultSubControl = new SubControlInfo("SurveyListing.ascx", false);

        /// <summary>
        /// A dictionary mapping control keys to user controls
        /// </summary>
        private static readonly IDictionary<ControlKey, SubControlInfo> ControlKeys = FillControlKeys();

        /// <summary>
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            SubControlInfo controlToLoad = this.GetControlToLoad();

            if (!controlToLoad.RequiresEditPermission || PortalSecurity.HasNecessaryPermission(SecurityAccessLevel.Edit, this.PortalSettings, this.ModuleConfiguration, this.UserInfo))
            {
                this.LoadChildControl(controlToLoad);
            }
            else if (IsLoggedIn)
            {
                this.Response.Redirect(Globals.NavigateURL(this.TabId), true);
            }
            else
            {
                this.Response.Redirect(Dnn.Utility.GetLoginUrl(this.PortalSettings, this.Request), true);
            }

            this.GlobalNavigation.ModuleConfiguration = this.ModuleConfiguration;
        }

        /// <summary>
        /// Fills <see cref="ControlKeys"/>.
        /// </summary>
        /// <returns>A dictionary mapping control keys to user controls.</returns>
        private static IDictionary<ControlKey, SubControlInfo> FillControlKeys()
        {
            return new Dictionary<ControlKey, SubControlInfo>(6)
                       {
                               { DefaultControlKey, DefaultSubControl },
                               { ControlKey.ViewSurvey, new SubControlInfo("ViewSurvey.ascx", false) },
                               { ControlKey.EditSurvey, new SubControlInfo("EditSurvey.ascx", true) },
                               { ControlKey.ThankYou, new SubControlInfo("ThankYou.ascx", false) },
                               { ControlKey.Analyze, new SubControlInfo("AnalyzeResponses.ascx", true) }
                       };
        }

        /// <summary>
        /// Gets the control to load, based on the key (or lack thereof) that is passed on the querystring.
        /// </summary>
        /// <returns>A relative path to the control that should be loaded into this container</returns>
        private SubControlInfo GetControlToLoad()
        {
            ControlKey? keyParameter = this.GetControlKey();
            if (keyParameter.HasValue)
            {
                return ControlKeys[keyParameter.Value];
            }
            
            var displayType = ModuleSettings.DisplayType.GetValueAsEnumFor<ControlKey>(this);
            if (displayType.HasValue && ControlKeys.ContainsKey(displayType.Value))
            {
                return ControlKeys[displayType.Value];
            }

            return DefaultSubControl;
        }

        /// <summary>
        /// Gets the <see cref="HttpRequest.QueryString"/> control key for the current <see cref="Page.Request"/>.
        /// </summary>
        /// <returns>The <see cref="HttpRequest.QueryString"/> control key for the current <see cref="Page.Request"/></returns>
        private ControlKey? GetControlKey()
        {
            string currentControlKey = this.GetCurrentControlKey();
            if (!string.IsNullOrEmpty(currentControlKey))
            {
                try
                {
                    return (ControlKey)Enum.Parse(typeof(ControlKey), currentControlKey, true);
                }
                catch (ArgumentException)
                {
                }
                catch (OverflowException)
                {
                }
            }

            return null;
        }

        /// <summary>
        /// Loads the child control to be displayed in this container.
        /// </summary>
        /// <param name="controlToLoad">The control to load.</param>
        private void LoadChildControl(SubControlInfo controlToLoad)
        {
            try
            {
                var childControl = (PortalModuleBase)this.LoadControl(controlToLoad.ControlPath);
                childControl.ModuleConfiguration = this.ModuleConfiguration;
                childControl.ID = Path.GetFileNameWithoutExtension(controlToLoad.ControlPath);
                this.SubControlPlaceholder.Controls.Add(childControl);
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}

