// <copyright file="DnnLocalizer.cs" company="Engage Software">
// Engage: Survey
// Copyright (c) 2004-2013
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Survey
{
    using DotNetNuke.Services.Localization;

    using Engage.Survey.UI;

    /// <summary>
    /// Provides the ability to get localized text from DotNetNuke
    /// </summary>
    public class DnnLocalizer : ILocalizer
    {
        /// <summary>
        /// The resource file from which to get localized text
        /// </summary>
        private readonly string resourceFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="DnnLocalizer"/> class.
        /// </summary>
        /// <param name="resourceFile">The resource file from which to get localized text.</param>
        public DnnLocalizer(string resourceFile)
        {
            this.resourceFile = resourceFile;
        }

        /// <summary>
        /// Localizes the specified resource key.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <returns>The localized text</returns>
        public string Localize(string resourceKey)
        {
            return Localization.GetString(resourceKey, this.resourceFile);
        }
    }
}