// <copyright file="ModuleSettings.cs" company="Engage Software">
// Engage: Survey
// Copyright (c) 2004-2014
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
    using System.Collections.Generic;
    using System.Linq;

    using Framework;

    using Telerik.Web.UI;

    /// <summary>
    /// Contains the settings for the Survey module
    /// </summary>
    public static class ModuleSettings
    {
        /// <summary>
        /// The display that is used for unauthenticated users.
        /// </summary>
        public static readonly Setting<ControlKey> DisplayType = new Setting<ControlKey>("DisplayType", SettingScope.TabModule, ControlKey.SurveyListing);

        /// <summary>
        /// The ID of the survey to render.
        /// </summary>
        public static readonly Setting<int> SurveyId = new Setting<int>("SurveyId", SettingScope.TabModule, 0);

        /// <summary>
        /// Allow/disallow the same user to take a survey more than once.
        /// </summary>
        public static readonly Setting<bool> AllowMultpleEntries = new Setting<bool>("AllowMultpleEntries", SettingScope.TabModule, true);

        /// <summary>
        /// Show or hide the * for required fields.
        /// </summary>
        public static readonly Setting<bool> ShowRequiredNotation = new Setting<bool>("ShowRequiredNotation", SettingScope.TabModule, false);

        /// <summary>
        /// Whether this module has been configured
        /// </summary>
        public static readonly Setting<bool> IsConfigured = new Setting<bool>("ModuleConfigured", SettingScope.Portal, false);

        /// <summary>
        /// Whether to send notification emails by default
        /// </summary>
        public static readonly Setting<bool> SendNotification = new Setting<bool>("SendNotification", SettingScope.TabModule, false);

        /// <summary>
        /// The email address from which to send notification emails, by default
        /// </summary>
        public static readonly Setting<string> NotificationFromEmailAddress = new Setting<string>("NotificationFromEmailAddress", SettingScope.TabModule, null);

        /// <summary>
        /// The email addresses to which to send notification emails, by default
        /// </summary>
        public static readonly Setting<string> NotificationToEmailAddresses = new Setting<string>("NotificationToEmailAddresses", SettingScope.TabModule, null);

        /// <summary>
        /// Whether to send thank you emails by default
        /// </summary>
        public static readonly Setting<bool> SendThankYou = new Setting<bool>("SendThankYou", SettingScope.TabModule, false);

        /// <summary>
        /// The email address from which to send thank you emails, by default
        /// </summary>
        public static readonly Setting<string> ThankYouFromEmailAddress = new Setting<string>("ThankYouFromEmailAddress", SettingScope.TabModule, null);

        /// <summary>
        /// The comma-delimited list of CAPTCHA strategies to employ
        /// </summary>
        public static readonly Setting<string> CaptchaStrategies = new Setting<string>("CaptchaStrategies", SettingScope.TabModule, RadCaptcha.ProtectionStrategies.InvisibleTextBox.ToString() + "," + RadCaptcha.ProtectionStrategies.MinimumTimeout.ToString());

        /// <summary>
        /// The minimum number of seconds form must be displayed before it is valid. If you're too fast, you must be a robot. Only used if <see cref="CaptchaStrategies"/> includes <see cref="RadCaptcha.ProtectionStrategies.MinimumTimeout"/>
        /// </summary>
        public static readonly Setting<int> CaptchaMinTimeout = new Setting<int>("CaptchaMinTimeout", SettingScope.TabModule, 3);

        /// <summary>
        /// Gets the CAPTCHA strategies.
        /// </summary>
        /// <param name="moduleControl">The module control.</param>
        /// <returns>A sequence of <see cref="RadCaptcha.ProtectionStrategies"/></returns>
        public static IEnumerable<RadCaptcha.ProtectionStrategies> GetCaptchaStrategies(IModuleControlBase moduleControl)
        {
            var commaDelimited = CaptchaStrategies.GetValueAsStringFor(moduleControl);
            if (string.IsNullOrEmpty(commaDelimited))
            {
                return Enumerable.Empty<RadCaptcha.ProtectionStrategies>();
            }
            
            var split = commaDelimited.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return from value in split
                   where Enum.IsDefined(typeof(RadCaptcha.ProtectionStrategies), value)
                   select (RadCaptcha.ProtectionStrategies)Enum.Parse(typeof(RadCaptcha.ProtectionStrategies), value);
        }
    }
}
