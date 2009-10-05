// <copyright file="Setting.cs" company="Engage Software">
// Engage: ContactUs - http://www.engagesoftware.com
// Copyright (c) 2004-2009
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Survey
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Text;

    public class Setting
    {
        
        public static readonly Setting DisplayType = new Setting("DisplayType", "The display that is used for unauthenticated users.");
        public static readonly Setting SurveyTypeId = new Setting("SurveyTypeId", "The SurveyTypeId for the survey to render.");
        public static readonly Setting AllowMultpleEntries = new Setting("AllowMultpleEntries", "Allow/disallow the same user to take a survey more than once.");
        public static readonly Setting ShowRequiredNotation = new Setting("ShowRequiredNotation", "Show or hide the * for required fields.");
       
        private Setting( string propertyName, string description)
        {
            this._propertyName = propertyName;
            this._description = description;
        }

        #region Public Properties

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _propertyName = string.Empty;
        public string PropertyName
        {
            [DebuggerStepThrough]
            get { return _propertyName; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _propertyValue = string.Empty;
        public string PropertyValue
        {
            [DebuggerStepThrough]
            get { return _propertyValue; }
            [DebuggerStepThrough]
            set { _propertyValue = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _description = string.Empty;
        public string Description
        {
            [DebuggerStepThrough]
            get { return _description; }
            [DebuggerStepThrough]
            set { _description = value; }
        }

        #endregion

        public static List<Setting> GetList(Type ct)
        {
            if (ct == null) throw new ArgumentNullException("ct");

            List<Setting> settings = new List<Setting>();

            Type type = ct;
            while (type.BaseType != null)
            {
                FieldInfo[] fi = type.GetFields();

                foreach (FieldInfo f in fi)
                {
                    Object o = f.GetValue(type);
                    if (o is Setting)
                    {
                        settings.Add((Setting)o);
                    }
                }

                type = type.BaseType; //check the super type 
            }

            return settings;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(128);

            builder.Append("Property Name: ");
            builder.Append(_propertyName);
            builder.Append(" Property Value: ");
            builder.Append(_propertyValue);

            return builder.ToString();
        }

    }
}
