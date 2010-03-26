// <copyright file="FeatureController.cs" company="Engage Software">
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
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Reflection;
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
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "Interface was defined in VB, with incorrect naming convention")]
        public string UpgradeModule(string version)
        {
            switch (version)
            {
                case "3.0.0":
                    if (GetDotNetNukeFrameworkVersion().Major > 4)
                    {
                        // They moved XmlMerge in DNN 5, so this only works for DNN 4
                        break;
                    }

                    var frameworkVersion = GetNETFrameworkVersion();
                    if (frameworkVersion.Major >= 3 && frameworkVersion.MajorRevision >= 5)
                    {
                        break;
                    }

                    UpdateWebConfigFrameworkVersion();

                    return "Updated web.config to support .NET 3.5";
                default:
                    break;
            }

            return "Module upgraded to version " + version;
        }

        /// <summary>
        /// Gets the <c>DotNetNuke</c> framework version.
        /// </summary>
        /// <remarks>Based on http://www.ifinity.com.au/Blog/EntryId/75/Include-jQuery-in-a-DotNetNuke-Module-with-version-independent-code</remarks>
        /// <returns>The version of <c>DotNetNuke</c> that this module is running under</returns>
        private static Version GetDotNetNukeFrameworkVersion()
        {
            return Assembly.GetAssembly(typeof(ModuleInfo)).GetName().Version;
        }

        /// <summary>
        /// Gets the .NET framework version.
        /// </summary>
        /// <remarks>Based on <c>DotNetNuke.Common.Initialize.GetNETFrameworkVersion</c> from DNN 5.2.2</remarks>
        /// <returns>The version of .NET that this application is running under</returns>
        private static Version GetNETFrameworkVersion()
        {
// ReSharper disable EmptyGeneralCatchClause
            var version = Environment.Version.ToString(2);

            // Try and load a 3.0 Assembly 
            try
            {
                AppDomain.CurrentDomain.Load("System.Runtime.Serialization, Version=3.0.0.0, Culture=neutral, PublicKeyToken=B77A5C561934E089");
                version = "3.0";
            }
            catch
            {
            }

            // Try and load a 3.5 Assembly
            try
            {
                AppDomain.CurrentDomain.Load("System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=B77A5C561934E089");
                version = "3.5";
            }
            catch
            {
            }

// ReSharper restore EmptyGeneralCatchClause
            return new Version(version);
        }

        /// <summary>
        /// Updates the web.config to support .NET 3.5.
        /// </summary>
        private static void UpdateWebConfigFrameworkVersion()
        {
            var configDocument = Config.Load();

            using (Stream xmlMergeManifest = Assembly.GetExecutingAssembly().GetManifestResourceStream("Engage.Dnn.Survey.Util.Net35.withoutNamespaces.config"))
            {
                var configMerge = new XmlMerge(xmlMergeManifest, "3.0.0", "Engage: Survey");
                configMerge.UpdateConfig(configDocument);
            }

            using (Stream bindingRedirectManifest = Assembly.GetExecutingAssembly().GetManifestResourceStream("Engage.Dnn.Survey.Util.Net35.bindingRedirect.config"))
            {
                var bindingRedirectMerge = new XmlMergeWithNamespaceSupport(bindingRedirectManifest);
                bindingRedirectMerge.UpdateConfig(configDocument);
            }

            Config.Save(configDocument);
        }
    }
}