// <copyright file="ILocalizer.cs" company="Engage Software">
// Engage: Survey
// Copyright (c) 2004-2014
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Survey.UI
{
    /// <summary>
    /// A contract allowing retrieving localized text
    /// </summary>
    public interface ILocalizer
    {
        /// <summary>
        /// Localizes the specified resource key.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <returns>The localized text</returns>
        string Localize(string resourceKey);
    }
}