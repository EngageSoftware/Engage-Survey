// <copyright file="UserResponse.cs" company="Engage Software">
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
    using Util;

    /// <summary>
    /// Represents a users response along with the relationship key information.
    /// </summary>
    public class UserResponse
    {
        /// <summary>
        /// Gets or sets the relationship key.
        /// </summary>
        /// <value>The relationship key.</value>
        public Key RelationshipKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the answer value.
        /// </summary>
        /// <value>The answer value.</value>
        public string AnswerValue
        {
            get;
            set;
        }
    }
}
