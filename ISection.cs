// <copyright file="ISection.cs" company="Engage Software">
// Engage: Survey
// Copyright (c) 2004-2013
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

    using Engage.Survey.UI;

    using Util;

    /// <summary>
    /// ISection Interface
    /// </summary>
    public interface ISection : ISurveyElement
    {
        /// <summary>
        /// Gets the survey.
        /// </summary>
        /// <returns></returns>
        ISurvey GetSurvey();

        /// <summary>
        /// Gets the questions.
        /// </summary>
        /// <returns>Array of IQuestions</returns>
        List<IQuestion> GetQuestions();

        /// <summary>
        /// Gets the question.
        /// </summary>
        /// <param name="key">The key name.</param>
        /// <returns>An IQuestion using the passed key.</returns>
        IQuestion GetQuestion(Key key);

        /// <summary>
        /// Gets or sets a value indicating whether [show text].
        /// </summary>
        /// <value><c>true</c> if [show text]; otherwise, <c>false</c>.</value>
        bool ShowText { get; set; }

        /// <summary>
        /// Gets or sets the section id.
        /// </summary>
        /// <value>The section id.</value>
        int SectionId { get; set; }

        /// <summary>
        /// Gets or sets the relative order.
        /// </summary>
        /// <value>The relative order.</value>
        int RelativeOrder { get; set; }

        /// <summary>
        /// Renders the specified ph.
        /// </summary>
        /// <param name="ph">The placeholder to render all the content.</param>
        /// <param name="readOnly">if set to <c>true</c> [read only].</param>
        /// <param name="showRequiredNotation">if set to <c>true</c> [show required notation].</param>
        /// <param name="validationProvider">The validation provider.</param>
        /// <param name="localizer">Localizes text</param>
        void Render(PlaceHolder ph, bool readOnly, bool showRequiredNotation, ValidationProviderBase validationProvider, ILocalizer localizer);

        /// <summary>
        /// Posts the save processing.
        /// </summary>
        /// <param name="control">The control.</param>
        void PostSaveProcessing(WebControl control);

        /// <summary>
        /// Pres the save processing.
        /// </summary>
        /// <param name="control">The control.</param>
        void PreSaveProcessing(WebControl control);
    }
}