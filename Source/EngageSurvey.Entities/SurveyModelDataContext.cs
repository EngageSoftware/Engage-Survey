// <copyright file="SurveyModelDataContext.cs" company="Engage Software">
// Engage: ContactUs - http://www.engagesoftware.com
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
    }
}