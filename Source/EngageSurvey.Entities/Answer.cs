
namespace Engage.Survey.Entities
{
    using Util;

    public partial class Answer : IAnswer
    {
        public string FormattedText
        {
            get
            {
                return Text;
            }
        }

        public string UnformattedText
        {
            get
            {
                return Text;
            }
        }

        public string Formatting
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the rendering key used by the SurveyControl to uniquely identify this element.
        /// </summary>
        /// <value>The rendering key.</value>
        public Key RelationshipKey
        {
            get
            {
                return new Key{ SectionId = this.Question.SectionId, QuestionId = this.QuestionId, AnswerId = this.AnswerId};}
            }
        }
}