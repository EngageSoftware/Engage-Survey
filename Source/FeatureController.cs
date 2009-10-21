// <copyright file="FeatureController.cs" company="Engage Software">
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
    using System.IO;
    using System.Linq;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Packages;

    /// <summary>
    /// Registered with DNN to indicate which features this module supports
    /// </summary>
    public class FeatureController : IUpgradeable
    {
        /// <summary>
        /// Called during the install and upgrade process for the module, once for each version of the module
        /// </summary>
        /// <param name="version">The version number.</param>
        /// <returns>A status message</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "Interface was defined in VB, with incorrect naming convention")]
        public string UpgradeModule(string version)
        {
            switch (version)
            {
                case "3.0.0":
                    var configDocument = Config.Load();

                    using (Stream xmlMergeManifest = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Engage.Dnn.Survey.Util.Net35.withoutNamespaces.config"))
                    {
                        var configMerge = new XmlMerge(xmlMergeManifest, "3.0.0", "Engage: Survey");
                        configMerge.UpdateConfig(configDocument);
                    }

                    using (Stream bindingRedirectManifest = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Engage.Dnn.Survey.Util.Net35.BindingRedirect.config"))
                    {
                        var bindingRedirectMerge = new XmlMergeWithNamespaceSupport(bindingRedirectManifest);
                        bindingRedirectMerge.UpdateConfig(configDocument);
                    }

                    Config.Save(configDocument);

                    return "Updated web.config to support .NET 3.5";
                default:
                    return "Module upgraded to version " + version;
            }
        }
    }
}