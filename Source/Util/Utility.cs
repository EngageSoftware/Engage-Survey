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

namespace Engage.Dnn.Survey.Util
{
    using System.Text;
    using DotNetNuke.Common;

    /// <summary>
    /// Summary description for Utility.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// DnnFriendlyModuleName Constant
        /// </summary>
        public const string DnnFriendlyModuleName = "Engage: Survey";

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
                sb.Append(Globals.GetDesktopModuleByName(DnnFriendlyModuleName).FolderName);
                sb.Append("/");
                return sb.ToString();
            }
        }
    }
}

