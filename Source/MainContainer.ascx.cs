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

        #region Web Form Designer generated code
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ReadItemType();
            LoadControlType();

            //add on a query string param so that we can grab ALL content, not just for this moduleId. hk
            string title = Localization.GetString(ModuleActionType.ExportModule, Localization.GlobalResourceFile);
            foreach (ModuleAction action in Actions)
            {
                if (action.Title == title)
                {
                    action.Url = action.Url + "?all=1";
                    break;
                }

            }
        }

        #endregion

        #region Private Methods

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

        private static void FillControlKeys()
        {
            StringDictionary controlKeys = new StringDictionary { { "ViewSurvey", "ViewSurvey.ascx" }, { "EditSurvey", "EditSurvey.ascx" }, { "SurveyListing", "SurveyListing.ascx" }, { "ThankYou", "ThankYou.ascx" } };

            ControlKeys = controlKeys;
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Code paths are easy to understand, test, and maintain")]
        private void ReadItemType()
        {

            StringDictionary returnDict = GetAdminControlKeys();
            string keyParam = Request.Params["key"];

            if (Engage.Util.Utility.HasValue(keyParam))
            {
                controlToLoad = returnDict[keyParam.ToLower(CultureInfo.InvariantCulture)];
            }
            else
            {
                if (IsSetup == false)
                {
                    //display the admin version
                    controlToLoad = "SurveyListing.ascx";
                }
                else
                {
                    //display unathenticated version to user based on setting by administrator.
                    object o = Settings[Setting.DisplayType.PropertyName];
                    if (o != null && !String.IsNullOrEmpty(o.ToString()))
                    {
                        controlToLoad = o + ".ascx";
                    }
                    else
                    {
                        controlToLoad = "SurveyListing.ascx";
                    }
                }
            }
        }

        private void LoadControlType()
        {
            try
            {
                if (controlToLoad == null) return;

                ModuleBase mb = (ModuleBase)LoadControl(controlToLoad);
                mb.ModuleConfiguration = ModuleConfiguration;
                mb.ID = System.IO.Path.GetFileNameWithoutExtension(controlToLoad);
                ControlsPlaceHolder.Controls.Add(mb);

            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion
    }
}

