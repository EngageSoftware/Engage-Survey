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

namespace Engage.Dnn.Survey
{
    using System.Collections;
    using System.Text;
    using System.Web;
    using DotNetNuke.Common;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Portals;
    
    /// <summary>
    /// Summary description for Utility.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// DnnFriendlyModuleName Constant
        /// </summary>
        public const string DesktopModuleName = "Engage: Survey";

        /// <summary>
        /// ModuleDefinitionFriendlyName Constant
        /// </summary>
        public const string ModuleDefinitionFriendlyName = "Engage: Survey";
        /// <summary>
        /// ModuleConfigured Constant
        /// </summary>
        public const string ModuleConfigured = "ModuleConfigured";
        /// <summary>
        /// MainContainer Constant
        /// </summary>
        public const string Container = "MainContainer";
        /// <summary>
        /// CacheTime Constant
        /// </summary>
        public const string CacheTime = "CacheTime";

        /// <summary>
        /// Gets the name of the desktop module folder.
        /// </summary>
        /// <value>The name of the desktop module folder.</value>
        public static string DesktopModuleFolderName
        {
            get
            {
                StringBuilder sb = new StringBuilder(128);
                sb.Append("/DesktopModules/");
                sb.Append(Globals.GetDesktopModuleByName(DesktopModuleName).FolderName);
                sb.Append("/");
                return sb.ToString();
            }
        }

        /// <summary>
        /// A public statically available method in which clients can call to get effectly the "NavigateUrl()"
        /// for this module. Code that references this project can obtain a valid URL to navigate to for another module.
        /// </summary>
        /// <returns></returns>
        public static string NavigateUrl()
        {
            var mc = new ModuleController();
            int portalId = PortalController.GetCurrentPortalSettings().PortalId;
            ArrayList modules = mc.GetModulesByDefinition(portalId, ModuleDefinitionFriendlyName);

            foreach (ModuleInfo module in modules)
            {
                return Globals.NavigateURL(module.TabID);
            }

            //either the freindly name is wrong or no modules defined. Maybe should throw exception instead?
            return Globals.NavigateURL();
        }

        /// <summary>
        /// Gets the web service URL.
        /// </summary>
        /// <returns></returns>
        public static string GetServiceUrl()
        {
            var url = new StringBuilder();
            if (HttpContext.Current != null)
            {
                url.Append(HttpContext.Current.Request.Url.Scheme);
                url.Append("://");
                url.Append(HttpContext.Current.Request.Url.Authority);
                url.Append(Framework.ModuleBase.ApplicationUrl);
                url.Append("/DesktopModules/");
                url.Append(Globals.GetDesktopModuleByName(DesktopModuleName).FolderName);
                url.Append("/Services.asmx");
            }

            return url.ToString();
        }
    }
}