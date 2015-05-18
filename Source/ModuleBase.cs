// <copyright file="ModuleBase.cs" company="Engage Software">
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
    /// <summary>
    /// The base class for all module controls in this module
    /// </summary>
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
        /// Builds a URL for this ModuleId, loading the given <see cref="ControlKey"/>.
        /// </summary>
        /// <param name="moduleId">The module id of the module for which the control key is being used.</param>
        /// <param name="controlKey">The control key to determine which control to load.</param>
        /// <returns>
        /// A URL with the given criteria
        /// </returns>
        protected string BuildLinkUrl(int moduleId, ControlKey controlKey)
        {
            return this.BuildLinkUrl(moduleId, controlKey.ToString());
        }

        /// <summary>
        /// Builds a URL for this ModuleId, loading the given <see cref="ControlKey"/>, and using the given queryString parameters.
        /// </summary>
        /// <param name="moduleId">The module id of the module for which the control key is being used.</param>
        /// <param name="controlKey">The control key to determine which control to load.</param>
        /// <param name="queryStringParameters">Any other queryString parameters.</param>
        /// <returns>
        /// A URL with the given criteria
        /// </returns>
        protected string BuildLinkUrl(int moduleId, ControlKey controlKey, params string[] queryStringParameters)
        {
            return this.BuildLinkUrl(moduleId, controlKey.ToString(), queryStringParameters);
        }

        /// <summary>
        /// Builds a URL for the given <paramref name="tabId"/>, using the given queryString parameters.
        /// </summary>
        /// <param name="tabId">The tab id of the page to navigate to.</param>
        /// <param name="moduleId">The module id of the module for which the control key is being used.</param>
        /// <param name="controlKey">The control key to determine which control to load.</param>
        /// <param name="queryStringParameters">Any other queryString parameters.</param>
        /// <returns>
        /// A URL to the given <paramref name="tabId"/>, with the given queryString parameters
        /// </returns>
        protected string BuildLinkUrl(int tabId, int moduleId, ControlKey controlKey, params string[] queryStringParameters)
        {
            return this.BuildLinkUrl(tabId, moduleId, controlKey.ToString(), queryStringParameters);
        }
    }
}

