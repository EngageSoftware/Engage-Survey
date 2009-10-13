// <copyright file="ModuleSettings.cs" company="Engage Software">
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
    using Framework;

    /// <summary>
    /// Contains the settings for the Survey module
    /// </summary>
    public class ModuleSettings
    {
        /// <summary>
        /// The display that is used for unauthenticated users.
        /// </summary>
        public static readonly Setting<string> DisplayType = new Setting<string>("DisplayType", SettingScope.TabModule, "SurveyListing");

        /// <summary>
        /// The SurveyTypeId for the survey to render.
        /// </summary>
        public static readonly Setting<int> SurveyTypeId = new Setting<int>("SurveyTypeId", SettingScope.TabModule, 0);

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
    }
}
