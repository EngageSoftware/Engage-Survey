// <copyright file="ISurvey.cs" company="Engage Software">
// Engage: Survey
// Copyright (c) 2004-2010
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Survey
{
    using System.Collections.Generic;
    using System.Web.UI.WebControls;
    using Util;

    /// <summary>
    /// ISurvey Interface
    /// </summary>
    public interface ISurvey : ISurveyElement
    {

        /// <summary>
        /// Gets a value indicating whether this instance is readonly.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is readonly; otherwise, <c>false</c>.
        /// </value>
        bool IsReadOnly { get; }

        /// <summary>
        /// Gets or sets the survey id.
        /// </summary>
        /// <value>The survey id.</value>
        int SurveyId { get; set; }

        /// <summary>
        /// Gets the sections.
        /// </summary>
        /// <returns>List of ISections for this survey</returns>
        List<ISection> GetSections();

        /// <summary>
        /// Gets the section.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns>An ISectcion using the title.</returns>
        ISection GetSection(string title);

        /// <summary>
        /// Gets the final message option.
        /// </summary>
        /// <value>The final message option.</value>
        FinalMessageOption FinalMessageOption { get; }

        /// <summary>
        /// Gets the final message.
        /// </summary>
        /// <value>The final message.</value>
        string FinalMessage { get; }

        /// <summary>
        /// Gets the final URL.
        /// </summary>
        /// <value>The final URL.</value>
        string FinalUrl { get; }

        /// <summary>
        /// Gets or sets a value indicating whether [show title].
        /// </summary>
        /// <value><c>true</c> if [show title]; otherwise, <c>false</c>.</value>
        bool ShowText { get; set; }

        /// <summary>
        /// Gets or sets the question format option.
        /// </summary>
        /// <value>The question format option.</value>
        ElementFormatOption QuestionFormatOption { get; set; }

        /// <summary>
        /// Gets or sets the section format option.
        /// </summary>
        /// <value>The answer format option.</value>
        ElementFormatOption SectionFormatOption { get; set; }

        /// <summary>
        /// Gets or sets the title option.
        /// </summary>
        /// <value>The title option.</value>
        TitleOption TitleOption { get; set; }

        /// <summary>
        /// Gets or sets the logo URL.
        /// </summary>
        /// <value>The logo URL.</value>
        string LogoURL { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [send notification].
        /// </summary>
        /// <value><c>true</c> if [send notification]; otherwise, <c>false</c>.</value>
        bool SendNotification
        {
            get;
            set;
        }

        /// <summary>
        /// Renders the survey from this survey.
        /// </summary>
        /// <param name="ph">The place holder to render the survey.</param>
        /// <param name="readOnly">if set to <c>true</c> [read only].</param>
        /// <param name="showRequiredNotation">if set to <c>true</c> [show required notation].</param>
        /// <param name="validationProvider">The validation provider.</param>
        void Render(PlaceHolder ph, bool readOnly, bool showRequiredNotation, ValidationProviderBase validationProvider);

        /// <summary>
        /// Pres the save processing.
        /// </summary>
        /// <param name="control">The control.</param>
        void PreSaveProcessing(WebControl control);

        /// <summary>
        /// Posts the save processing.
        /// </summary>
        /// <param name="control">The control.</param>
        void PostSaveProcessing(WebControl control);

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns></returns>
        int Save(int userId);
    }
}