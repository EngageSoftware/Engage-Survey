// <copyright file="SurveyQueryStringBuilder.cs" company="Engage Software">
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
    using System;
    using System.Collections.Generic;
    using System.Web;

    public enum Mode
    {
        View = 1,
        Edit = 2,
        ViewOnly = 3,
        Add = 4,
        Listing = 5,
        Copy = 6
    }

    public enum Action
    {
        None = 0,
        Delete = 1,
        MoveUp = 2,
        MoveDown = 3,
        Add = 4,
        Remove = 5,
        ForceUpdate = 6
    }

    public class SurveyQueryStringBuilder : Engage.Util.QueryStringBuilder
    {
        public SurveyQueryStringBuilder()
            : base(string.Empty)
        {
        }

        public string[] Parameters
        {
            get 
            {

                List<string> list = new List<string>(Values.Count);
                int i = 0;

                string s = string.Empty;
                foreach (string k in Values.Keys)
                {
                    if (k != "tabid")
                    {
                        //Don't allow spaces in the query string
                        if (Values[k].Equals(String.Empty) || ! Char.IsWhiteSpace(Values[k], 0))
                        {
                            //?value=*
                            //&value=*
                            i += 1;
                            if (i == 1) 
                            {
                                s = k + "=" + Values[k];
                            }
                            else
                            {
                                s = "&" + k + "=" + Values[k];
                            }
                        }
                        list.Add(s);
                    }
                }

                return list.ToArray();
            }
        }

        public string PreviousControl
        {
            get
            {
                object o = HttpContext.Current.Request.QueryString["pc"];
                if (o == null)
                {
                    return String.Empty;
                }
                if (o.Equals(String.Empty) || !Char.IsWhiteSpace(o.ToString(), 0))
                {
                    return o.ToString();
                }
                return String.Empty;
            }
            set
            {
                Add("pc", value);
            }
        }

        public string TargetControl
        {
            get
            {
                object o = HttpContext.Current.Request.QueryString["tc"];
                if (o == null)
                {
                    return String.Empty;
                }
                return o.ToString();
            }
            set
            {
                Add("tc", value);
            }
        }

        public Mode Mode
        {
            get
            {
                object o = HttpContext.Current.Request.QueryString["mode"];
                if (o == null)
                    return Mode.ViewOnly;
                return (Mode)Enum.Parse(typeof(Mode), o.ToString());
            }
            set
            {
                Add("mode", value.ToString());
            }
        }

        public Action Action
        {
            get
            {
                object o = HttpContext.Current.Request.QueryString["action"];
                if (o == null)
                    return Action.None;
                return (Action)Enum.Parse(typeof(Action), o.ToString());
            }
            set
            {
                Add("action", value.ToString());
            }
        }

        public int Tabid
        {
            get
            {
                object o = HttpContext.Current.Request.QueryString["Tabid"];
                return (o == null ? -1 : Convert.ToInt32(o));
            }
            set
            {
                Add("TabId", value);
            }
        }
    }
}
