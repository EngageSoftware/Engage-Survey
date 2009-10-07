// <copyright file="ModuleBase.cs" company="Engage Software">
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
    using System;
    using System.Web;
    using DotNetNuke.Entities.Host;
    using DotNetNuke.Entities.Modules;

    /// <summary>
    /// Summary description for ModuleBase.
    /// </summary>
    public class ModuleBase : PortalModuleBase
    {
        private bool allowTitleUpdate = true;
        private bool useCache = true;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (DotNetNuke.Framework.AJAX.IsInstalled())
            {
                DotNetNuke.Framework.AJAX.RegisterScriptManager();
            }
        }
        
        protected bool IsSetup
        {
            get
            {
                string s = HostSettings.GetHostSetting(Utility.ModuleConfigured + PortalId);
                return !String.IsNullOrEmpty(s);
            }
        }

        protected static bool IsHostMailConfigured
        {
            get
            {
                string s = HostSettings.GetHostSetting("SMTPServer");
                return Engage.Util.Utility.HasValue(s);
            }
        }

        protected bool UseCache
        {
            get {
                return this.useCache && CacheTime > 0;
            }
            set { this.useCache = value; }
        }

        protected bool AllowTitleUpdate
        {
            get
            {
                object o = Settings["AllowTitleUpdate"];
                if (o == null || !bool.TryParse(o.ToString(), out this.allowTitleUpdate))
                {
                    this.allowTitleUpdate = true;
                }
                return this.allowTitleUpdate;
            }
            set
            {
                this.allowTitleUpdate = value;
            }
        }

        protected int CacheTime
        {
            get
            {
                object o = Settings["CacheTime"];
                if (o != null)
                {
                    return Convert.ToInt32(o.ToString());
                }
                if (GetDefaultCacheSetting(PortalId) > 0)
                {
                    return GetDefaultCacheSetting(PortalId);
                }
                return 0;
            }
        }

        protected static int GetDefaultCacheSetting(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.CacheTime + portalId);
            if (Engage.Util.Utility.HasValue(s))
            {
                return Convert.ToInt32(s);
            }
            return 0;
        }

        /// <summary>
        /// Gets the application URL.
        /// </summary>
        /// <value>The application URL.</value>
        public static string ApplicationUrl
        {
            get
            {
                if (HttpContext.Current.Request.ApplicationPath == "/")
                {
                    return "";
                }
                return HttpContext.Current.Request.ApplicationPath;
            }
        }

        /// <summary>
        /// Builds the link URL.
        /// </summary>
        /// <param name="qsParameters">The qs parameters.</param>
        /// <returns></returns>
        public string BuildLinkUrl(string qsParameters)
        {
            return DotNetNuke.Common.Globals.NavigateURL(TabId, "", qsParameters);
        }

        /// <summary>
        /// Gets the name of the desktop module folder.
        /// </summary>
        /// <value>The name of the desktop module folder.</value>
        public static string DesktopModuleFolderName
        {
            get
            {
                return Utility.DesktopModuleFolderName;
            }
        }

        /// <summary>
        /// Gets the clean title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns></returns>
        public static string GetCleanTitle(object title)
        {
            return Engage.Util.Utility.RemoveHtmlMarkup(title.ToString(), false);
        }
    }
}

