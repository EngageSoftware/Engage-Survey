// <copyright file="SurveyModelDataContext.cs" company="Engage Software">
// Engage: Survey
// Copyright (c) 2004-2009
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Survey.Entities
{
    using System.Data.Linq;
    using System.Linq;

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
                return new SurveyModelDataContext(DotNetNuke.Common.Utilities.Config.GetConnectionString());
            }
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
    }
}