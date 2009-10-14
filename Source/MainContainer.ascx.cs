// <copyright file="MainContainer.ascx.cs" company="Engage Software">
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
    using System.Collections.Specialized;
    using System.Globalization;
    using System.IO;
    using System.Web.UI;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.Services.Exceptions;

    /// <summary>
    /// MainContainer for project.
    /// </summary>
    public partial class MainContainer : ModuleBase
    {
        private static StringDictionary ControlKeys;
        private string controlToLoad;

        /// <summary>
        /// Gets the admin control keys.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not a property")]
        public static StringDictionary GetAdminControlKeys()
        {
            if (ControlKeys == null)
            {
                FillControlKeys();
            }

            return ControlKeys;
        }

        /// <summary>
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.ReadItemType();
            this.LoadControlType();

            this.GlobalNavigation.ModuleConfiguration = this.ModuleConfiguration;
        }

        private static void FillControlKeys()
        {
            ControlKeys = new StringDictionary
                              {
                                      { "ViewSurvey", "ViewSurvey.ascx" }, 
                                      { "EditSurvey", "EditSurvey.ascx" }, 
                                      { "SurveyListing", "SurveyListing.ascx" }, 
                                      { "ThankYou", "ThankYou.ascx" }
                              };
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Code paths are easy to understand, test, and maintain")]
        private void ReadItemType()
        {

            StringDictionary returnDict = GetAdminControlKeys();
            string keyParam = Request.Params["key"];

            if (Util.Utility.HasValue(keyParam))
            {
                this.controlToLoad = returnDict[keyParam.ToLower(CultureInfo.InvariantCulture)];
            }
            else
            {
                // display unauthenticated version to user based on setting by administrator.
                string displayType = ModuleSettings.DisplayType.GetValueAsStringFor(this);
                if (!string.IsNullOrEmpty(displayType))
                {
                    this.controlToLoad = displayType + ".ascx";
                }
                else
                {
                    this.controlToLoad = "SurveyListing.ascx";
                }
            }
        }

        private void LoadControlType()
        {
            try
            {
                if (this.controlToLoad == null)
                {
                    return;
                }

                var loadedControl = (ModuleBase)this.LoadControl(this.controlToLoad);
                loadedControl.ModuleConfiguration = ModuleConfiguration;
                loadedControl.ID = Path.GetFileNameWithoutExtension(this.controlToLoad);
                this.ControlsPlaceHolder.Controls.Add(loadedControl);

            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}

