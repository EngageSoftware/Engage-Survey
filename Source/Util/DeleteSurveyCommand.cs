//using System;
//using System.Diagnostics;
//using System.Text;
//using System.Windows.Forms;
//using Engage.Console;
//using Engage.Db;
//using Engage.Services;
//using Engage.Survey.Definition;
//using Engage.Util;

//namespace Engage.Survey.Util
//{
//    /// <summary>
//    /// Summary description for DeleteSurveyCommand.
//    /// </summary>
//    public class DeleteSurveyCommand: Command
//    {
//        private IConsoleNode selectedNode;
//        private ISurveyDefinition survey;

//        public DeleteSurveyCommand(Form form, IConsoleNode selectedNode, ISurveyDefinition survey)
//            : base(form)
//        {
//            Debug.Assert(selectedNode != null, "SelectedNode cannot be null");
//            Debug.Assert(survey != null, "Survey cannot be null");

//            this.selectedNode = selectedNode;
//            this.survey = survey;
//        }

//        public override void Execute()
//        {
//            DialogResult result = MessageBox.Show(this.owner, "Are you sure you want to delete this survey? This cannot be undone.", "Delete Survey", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
			
//            if (result == DialogResult.Yes)
//            {
//                //first get a list of all child affiliations (sections) and zap them.
//                RemoveSectionAffiliations();
//                //now zap the survey.
//                RemoveSurvey();

//                IUIElementService service = ServiceHelper.GetUIElementService(ServiceProvider);
//                service.RemoveElement(this.selectedNode);
//                //this.selectedNode.Parent.Nodes.Remove(this.selectedNode);
//            }
//        }

//        public override void UnExecute()
//        {

//        }

//        private void RemoveSectionAffiliations()
//        {
//            this.survey.RemoveAffiliationDefinitions();
//            this.survey.Save();
//        }

//        private void RemoveSurvey()
//        {
//            StringBuilder sql = new StringBuilder(75);
		
//            sql.Append("UPDATE ");
//            sql.Append("lkpObjectType ");
//            sql.Append("SET ");
//            sql.Append("EndDate = GETDATE(), ");
//            sql.Append("RevisingUser = ");
//            sql.Append(Framework.GetUser().ObjectID);
//            sql.Append(" ");
//            sql.Append("WHERE ");
//            sql.Append("objectTypeID = ");
//            sql.Append(this.survey.TypeID);

//            EngageDataProviderBase dp = EngageDataProviderBase.Instance;

//            try
//            {
//                if (dp.ExecuteNonQuery(sql.ToString()) != 1)
//                {
//                    throw new DbException("Unable to end date lkpObjectType row.", sql);
//                }
//            }
//            catch (Exception se)
//            {
//                throw new DbException(se, sql);
//            }
//        }

//    }
//}
