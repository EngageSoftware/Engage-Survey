// <copyright file="Settings.ascx.cs" company="Engage Software">
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
    using System.Data;
    using System.Globalization;
    using System.Web.UI.WebControls;
    using DotNetNuke.Entities.Host;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    /// <summary>
    /// Settings control
    /// </summary>
    public partial class Settings : ModuleSettingsBase
    {      
        #region Base Method Implementations

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            base.LoadSettings();
            try
            {
                if (Page.IsPostBack == false)
                {
                    ListItem eventListing = new ListItem(Localization.GetString("SurveyListing", LocalResourceFile), "SurveyListing");
                    ListItem viewSurvey = new ListItem(Localization.GetString("ViewSurvey", LocalResourceFile), "ViewSurvey");
                    ListItem thanks = new ListItem(Localization.GetString("ThankYou", LocalResourceFile), "ThankYou");

                    ListingDisplayDropDownList.Items.Add(eventListing);
                    ListingDisplayDropDownList.Items.Add(viewSurvey);
                    ListingDisplayDropDownList.Items.Add(thanks);

                    object o = Settings[Setting.DisplayType.PropertyName];
                    if (o != null && !String.IsNullOrEmpty(o.ToString()))
                    {
                        ListItem li = ListingDisplayDropDownList.Items.FindByValue(Settings["DisplayType"].ToString());
                        if (li != null)
                        {
                            li.Selected = true;
                        }
                    }

                    o = Settings[Setting.AllowMultpleEntries.PropertyName];
                    if (o != null && !String.IsNullOrEmpty(o.ToString()))
                    {
                        AllowMultipleCheckBox.Checked = Convert.ToBoolean(o);
                    }

                    o = Settings[Setting.ShowRequiredNotation.PropertyName];
                    if (o != null && !String.IsNullOrEmpty(o.ToString()))
                    {
                        ShowRequiredNotationCheckBox.Checked = Convert.ToBoolean(o);
                    }

                    //DataTable dt = Engage.Survey.Db.DbUtil.GetAssignedSurveys();

                    //// bind the survey's
                    //foreach (DataRow row in dt.Rows)
                    //{
                    //    ListItem li = new ListItem(ModuleBase.GetCleanTitle(row["SurveyTitle"].ToString()), row["ObjectTypeId"].ToString());
                    //    SurveyDropDownList.Items.Add(li);
                    //}

                    o = Settings[Setting.SurveyTypeId.PropertyName];
                    if (o != null && !String.IsNullOrEmpty(o.ToString()))
                    {
                        ListItem li = SurveyDropDownList.Items.FindByValue(o.ToString());
                        if (li != null)
                        {
                            li.Selected = true;
                        }
                    }

                    ViewSurveyPanel.Visible = (ListingDisplayDropDownList.SelectedValue == "ViewSurvey");
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpdateSettings saves the modified settings to the Database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public override void UpdateSettings()
        {
            if (Page.IsValid)
            {
                try
                {
                    HostSettingsController controller = new HostSettingsController();
                    controller.UpdateHostSetting(Util.Utility.ModuleConfigured + PortalId.ToString(CultureInfo.InvariantCulture), "true");

                    ModuleController modules = new ModuleController();
                    modules.UpdateTabModuleSetting(this.TabModuleId, Setting.DisplayType.PropertyName, this.ListingDisplayDropDownList.SelectedValue);
                    modules.UpdateTabModuleSetting(this.TabModuleId, Setting.SurveyTypeId.PropertyName, SurveyDropDownList.SelectedValue);
                    modules.UpdateTabModuleSetting(this.TabModuleId, Setting.AllowMultpleEntries.PropertyName, AllowMultipleCheckBox.Checked.ToString());
                    modules.UpdateTabModuleSetting(this.TabModuleId, Setting.ShowRequiredNotation.PropertyName, ShowRequiredNotationCheckBox.Checked.ToString());
                }
                catch (Exception exc)
                {
                    Exceptions.ProcessModuleLoadException(this, exc);
                }
            }
        }

        #endregion

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddListingDisplay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ListingDisplayDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewSurveyPanel.Visible = (ListingDisplayDropDownList.SelectedValue == "ViewSurvey");
        }      
    }
}
