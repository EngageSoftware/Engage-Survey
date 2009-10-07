// <copyright file="Key.cs" company="Engage Software">
// Engage: ContactUs - http://www.engagesoftware.com
// Copyright (c) 2004-2009
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

    public class Key
    {
        public static Key ParseKeyFromString(string key)
        {
            if (key == null)
            {
                return new Key();
            }
            string[] keys = key.Split('-');

            Key k = new Key();
            k.SectionId = Convert.ToInt32(keys[0].Replace("S", string.Empty));
            k.QuestionId = Convert.ToInt32(keys[1].Replace("Q", string.Empty));
            k.AnswerId = Convert.ToInt32(keys[2].Replace("A", string.Empty));
            return k;
        }

        public int SectionId
        {
            get;
            set;
        }

        public int QuestionId
        {
            get;
            set;

        }

        public int AnswerId
        {
            get;
            set;
        }

        public override string ToString()
        {
            return "S" + this.SectionId + "-Q" + this.QuestionId + "-A" + this.AnswerId;
        }
        
        public override bool Equals(object obj)
        {
            if (obj is Key)
            {
                return obj.ToString() == this.ToString();
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.SectionId.GetHashCode();
        }
    }
}
