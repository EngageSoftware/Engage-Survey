// <copyright file="LayoutStrategy.cs" company="Engage Software">
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
    using System.Web.UI.WebControls;
    using Engage.Survey;

    public abstract class LayoutStrategy
    {
        private readonly ISurvey survey;
        private readonly PlaceHolder ph;

        protected LayoutStrategy(ISurvey survey, PlaceHolder ph)
        {
            this.survey = survey;
            this.ph = ph;
        }

        public abstract void Render();

         protected PlaceHolder GetPlaceHolder
         {
             get {return this.ph;}
         }
         protected ISurvey GetSurvey
         {
             get { return this.survey; }
         }
     }
}
