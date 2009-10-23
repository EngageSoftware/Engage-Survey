// <copyright file="ModuleBase.cs" company="Engage Software">
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
    public class ModuleBase : Framework.ModuleBase
    {
        /// <summary>
        /// Gets the name of the this module's desktop module record in DNN.
        /// </summary>
        /// <value>The name of this module's desktop module record in DNN.</value>
        public override string DesktopModuleName
        {
            get { return Utility.DesktopModuleName; }
        }

        /// <summary>
        /// Builds the link URL.
        /// </summary>
        /// <param name="qsParameters">The qs parameters.</param>
        /// <returns></returns>
        public string BuildLinkUrl(string qsParameters)
        {
            return BuildLinkUrl(TabId, string.Empty, qsParameters);
        }

        /// <summary>
        /// Builds the link URL.
        /// </summary>
        /// <param name="tabId">The tab id.</param>
        /// <param name="controlKey">The control key.</param>
        /// <param name="qsParameters">The qs parameters.</param>
        /// <returns></returns>
        public static string BuildLinkUrl(int tabId, string controlKey, string qsParameters)
        {
            return DotNetNuke.Common.Globals.NavigateURL(tabId, controlKey, qsParameters);
        }
    }
}

