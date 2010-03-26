// <copyright file="Key.cs" company="Engage Software">
// Engage: Survey
// Copyright (c) 2004-2010
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Survey.Util
{
    using System;

    /// <summary>
    /// Repesents the a string representation of the section/question/answer relationships that is easily recreated later.
    /// </summary>
    public class Key
    {
        /// <summary>
        /// Parses the key from string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static Key ParseKeyFromString(string key)
        {
            if (key == null)
            {
                return new Key();
            }
            string[] keys = key.Split('-');

            Key k = new Key
                        {
                                SectionId = Convert.ToInt32(keys[0].Replace("S", string.Empty)),
                                QuestionId = Convert.ToInt32(keys[1].Replace("Q", string.Empty)),
                                AnswerId = Convert.ToInt32(keys[2].Replace("A", string.Empty))
                        };
            return k;
        }

        /// <summary>
        /// Gets or sets the section id.
        /// </summary>
        /// <value>The section id.</value>
        public int SectionId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the question id.
        /// </summary>
        /// <value>The question id.</value>
        public int QuestionId
        {
            get;
            set;

        }

        /// <summary>
        /// Gets or sets the answer id.
        /// </summary>
        /// <value>The answer id.</value>
        public int AnswerId
        {
            get;
            set;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return "S" + this.SectionId + "-Q" + this.QuestionId + "-A" + this.AnswerId;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            if (obj is Key)
            {
                return obj.ToString() == this.ToString();
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return this.SectionId.GetHashCode();
        }
    }
}
