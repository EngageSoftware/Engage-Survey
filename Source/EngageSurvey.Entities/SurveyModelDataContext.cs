// <copyright file="SurveyModelDataContext.cs" company="Engage Software">
// Engage: Survey
// Copyright (c) 2004-2010
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Survey.Entities
{
    using System;
    using System.Data.Linq;
    using System.Globalization;
    using System.Linq;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Services.Localization;

    /// <summary>
    /// SurveyModelDataContext class
    /// </summary>
    public partial class SurveyModelDataContext
    {
        /// <summary>
        /// Gets an instance of the <see cref="DataContext"/>.
        /// </summary>
        /// <value>The <see cref="DataContext"/> instance.</value>
        public static SurveyModelDataContext Instance
        {
            get
            {
                return new SurveyModelDataContext(Config.GetConnectionString());
            }
        }

        /// <summary>
        /// Updates the given <see cref="Question"/> <paramref name="instance"/>'s <see cref="Question.RequiredMessage"/>.
        /// </summary>
        /// <param name="instance">The <see cref="Question"/> instance to update.</param>
        private static void UpdateQuestionRequiredMessage(Question instance)
        {
            instance.RequiredMessage = string.Format(
                    CultureInfo.CurrentCulture, 
                    Localization.GetString("Required Question.Format", SurveyRepository.SharedResourceFile),
                    instance.Text);
        }

        /// <summary>
        /// Called when this instance is created.
        /// </summary>
        partial void OnCreated()
        {
            var loadOptions = new DataLoadOptions();
            loadOptions.AssociateWith<Survey>(survey => survey.Sections.OrderBy(section => section.RelativeOrder));
            loadOptions.AssociateWith<Section>(section => section.Questions.OrderBy(question => question.RelativeOrder));
            loadOptions.AssociateWith<Question>(question => question.Answers.OrderBy(answer => answer.RelativeOrder));
            this.LoadOptions = loadOptions;
        }

// ReSharper disable UnusedMember.Local

        /// <summary>
        /// Called when a new <see cref="Answer"/> is inserted.
        /// </summary>
        /// <param name="instance">The new <see cref="Answer"/> instance.</param>
        partial void InsertAnswer(Answer instance)
        {
            instance.CreationDate = DateTime.Now;
            instance.RevisionDate = DateTime.Now;
            this.ExecuteDynamicInsert(instance);
        }

        /// <summary>
        /// Called when a new <see cref="Question"/> is inserted.
        /// </summary>
        /// <param name="instance">The new <see cref="Question"/> instance.</param>
        partial void InsertQuestion(Question instance)
        {
            instance.CreationDate = DateTime.Now;
            instance.RevisionDate = DateTime.Now;

            UpdateQuestionRequiredMessage(instance);

            this.ExecuteDynamicInsert(instance);
        }

        /// <summary>
        /// Called when a new <see cref="Section"/> is inserted.
        /// </summary>
        /// <param name="instance">The new <see cref="Section"/> instance.</param>
        partial void InsertSection(Section instance)
        {
            instance.CreationDate = DateTime.Now;
            instance.RevisionDate = DateTime.Now;
            this.ExecuteDynamicInsert(instance);
        }

        /// <summary>
        /// Called when a new <see cref="Survey"/> is inserted.
        /// </summary>
        /// <param name="instance">The new <see cref="Survey"/> instance.</param>
        partial void InsertSurvey(Survey instance)
        {
            instance.CreationDate = DateTime.Now;
            instance.RevisionDate = DateTime.Now;
            this.ExecuteDynamicInsert(instance);
        }

        /// <summary>
        /// Called when an existing <see cref="Answer"/> is updated.
        /// </summary>
        /// <param name="instance">The existing <see cref="Answer"/> instance.</param>
        partial void UpdateAnswer(Answer instance)
        {
            instance.RevisionDate = DateTime.Now;
            this.ExecuteDynamicUpdate(instance);
        }

        /// <summary>
        /// Called when an existing <see cref="Question"/> is updated.
        /// </summary>
        /// <param name="instance">The existing <see cref="Question"/> instance.</param>
        partial void UpdateQuestion(Question instance)
        {
            instance.RevisionDate = DateTime.Now;

            UpdateQuestionRequiredMessage(instance);

            this.ExecuteDynamicUpdate(instance);
        }

        /// <summary>
        /// Called when an existing <see cref="Section"/> is updated.
        /// </summary>
        /// <param name="instance">The existing <see cref="Section"/> instance.</param>
        partial void UpdateSection(Section instance)
        {
            instance.RevisionDate = DateTime.Now;
            this.ExecuteDynamicUpdate(instance);
        }

        /// <summary>
        /// Called when an existing <see cref="Survey"/> is updated.
        /// </summary>
        /// <param name="instance">The existing <see cref="Survey"/> instance.</param>
        partial void UpdateSurvey(Survey instance)
        {
            instance.RevisionDate = DateTime.Now;
            this.ExecuteDynamicUpdate(instance);
        }

// ReSharper restore UnusedMember.Local
    }
}