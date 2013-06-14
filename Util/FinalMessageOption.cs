// <copyright file="FinalMessageOption.cs" company="Engage Software">
// Engage: Survey
// Copyright (c) 2004-2013
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Survey.Util
{
    /// <summary>
    /// The options for what should happen after the survey is completed
    /// </summary>
    public enum FinalMessageOption
    {
        /// <summary>
        /// Don't do anything.  Use this value if it's not possible to complete the survey
        /// </summary>
        None = 0,

        /// <summary>
        /// Show <see cref="ISurvey.FinalMessage"/>
        /// </summary>
        UseFinalMessage = 1,

        /// <summary>
        /// Redirect the user to <see cref="ISurvey.FinalUrl"/>
        /// </summary>
        UseFinalURL = 2
    }
}