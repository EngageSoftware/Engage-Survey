//using System.Diagnostics;
//using System.Windows.Forms;
//using Engage.Console;
//using Engage.Services;
//using Engage.Survey.Console;
//using Engage.Survey.Definition;
//using Engage.Util;

//namespace Engage.Survey.Util
//{
//    /// <summary>
//    /// Summary description for DeleteQuestionCommand.
//    /// </summary>
//    public class DeleteQuestionCommand: Command
//    {
//        private IConsoleNode selectedNode;
//        private IQuestionDefinition question;

//        public DeleteQuestionCommand(Form form, IConsoleNode selectedNode, IQuestionDefinition question)
//            : base(form)
//        {
//            Debug.Assert(question != null , "Question can not be null");

//            this.selectedNode = selectedNode;
//            this.question = question;
//        }

//        public override void Execute()
//        {
//            DialogResult result = MessageBox.Show(this.owner, "Are you sure you want to delete this question? This cannot be undone.", "Delete Question", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
			
//            if (result == DialogResult.Yes)
//            {
//                IConsoleNode section = this.selectedNode.Parent;

//                //ISurveyNode node = (ISurveyNode) this.selectedNode.Parent.Tag;
//                ISurveyNode node = (ISurveyNode) this.selectedNode.Parent;
//                node.RemoveItem(question);

//                //this.selectedNode.Parent.Nodes.Remove(this.selectedNode);
//                IUIElementService service = ServiceHelper.GetUIElementService(ServiceProvider);
//                service.RefreshElement(section);

//                //section.Toggle();
//                //section.Toggle();
//            }
//        }

//        public override void UnExecute()
//        {

//        }

//        //		private void RemoveSectionAffiliation()
//        //		{
//        //			StringBuilder sql = new StringBuilder(75);
//        //		
//        //			sql.Append("UPDATE ");
//        //			sql.Append("ObjectAffiliationDefinition ");
//        //			sql.Append("SET ");
//        //			sql.Append("EndDate = GETDATE() ");
//        //			sql.Append("WHERE ");
//        //			sql.Append("ObjectAffiliationDefinitionID = ");
//        //			sql.Append(this.affiliation.GetID());
//        //
//        //			IDbConnection conn = null;
//        //			IDataReader dr = null;
//        //
//        //			try
//        //			{
//        //				conn = session.GetConnection();
//        //				conn.Open();
//        //		
//        //				IDbCommand cmd = conn.CreateCommand();
//        //				cmd.CommandText = sql.ToString();
//        //				dr = cmd.ExecuteReader();
//        //
//        //				if (dr.RecordsAffected != 1)
//        //				{
//        //					throw new DbException("Unable to end date ObjectAffiliationDefinition row.", sql);
//        //				}
//        //			}
//        //
//        //			catch (Exception se)
//        //			{
//        //				throw new DbException(se, sql);
//        //			}
//        //	
//        //			finally
//        //			{
//        //				if (dr != null) dr.Close();
//        //				session.ReturnConnection(conn);
//        //			}
//        //		}
//    }
//}
