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
//    /// Summary description for DeleteSectionCommand.
//    /// </summary>
//    public class DeleteSectionCommand: Command
//    {
//        private IConsoleNode selectedNode;
//        private ISectionDefinition section;

//        public DeleteSectionCommand(Form form, IConsoleNode selectedNode, ISectionDefinition section)
//            : base(form)
//        {
//            Debug.Assert(section != null, "Section cannot be null");

//            this.selectedNode = selectedNode;
//            this.section = section;
//        }

//        public override void Execute()
//        {
//            DialogResult result = MessageBox.Show(this.owner, "Are you sure you want to delete this section? This cannot be undone.", "Delete Section", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
			
//            if (result == DialogResult.Yes)
//            {
//                ISurveyNode node = (ISurveyNode) this.selectedNode.Parent;
//                node.RemoveItem(this.section);

//                IUIElementService service = ServiceHelper.GetUIElementService(ServiceProvider);
//                service.RemoveElement(this.selectedNode);
//            }
//        }

//        public override void UnExecute()
//        {

//        }
//    }
//}
