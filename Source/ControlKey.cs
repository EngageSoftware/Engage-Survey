// <copyright file="ControlKey.cs" company="Engage Software">
// Engage: Survey
// Copyright (c) 2004-2015
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Survey
{
    /// <summary>
    /// The keys to the controls of this module
    /// </summary>
    public enum ControlKey
    {
        /// <summary>
        /// List the existing survey definitions and completed surveys
        /// </summary>
        SurveyListing = 0,

        /// <summary>
        /// View a single survey
        /// </summary>
        ViewSurvey,

        /// <summary>
        /// Add a new survey or edit an existing survey
        /// </summary>
        EditSurvey,

        /// <summary>
        /// Displays responses for a survey
        /// </summary>
        Analyze
    }
}