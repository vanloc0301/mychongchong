namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Workflow.Client.Box;
    using SkyMap.Net.Workflow.Client.Services;
    using SkyMap.Net.Workflow.Client.View;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Windows.Forms;

    public class OpenStaticFormCommand : AbstractBoxCommand
    {
        public override void Run()
        {
            Static owner = this.Owner as Static;
            if (owner != null)
            {
                TreeNode selectedNode = owner.tvStaticList.SelectedNode;
                if (selectedNode != null)
                {
                    Prodef tag = selectedNode.Tag as Prodef;
                    if (tag != null)
                    {
                        if (LoggingService.IsInfoEnabled)
                        {
                            LoggingService.InfoFormatted("获取表单定义：{0}", new object[] { tag.DaoDataFormId });
                        }
                        DAODataForm daoDataForm = WorkflowService.GetDaoDataForm(tag.DaoDataFormId);
                        if (daoDataForm != null)
                        {
                            IDataForm newDataFormInstance = WfViewHelper.GetNewDataFormInstance(daoDataForm);
                            Form form3 = new Form();
                            form3.StartPosition = FormStartPosition.CenterScreen;
                            if (newDataFormInstance is Control)
                            {
                                Control control = newDataFormInstance as Control;
                                control.Dock = DockStyle.Fill;
                                form3.ClientSize = control.Size;
                                form3.Controls.Add(control);
                            }
                            newDataFormInstance.SetPropertys();
                            newDataFormInstance.SetElse();
                            newDataFormInstance.LoadMe();
                            form3.Text = selectedNode.Text;
                            form3.Show();
                        }
                    }
                }
            }
        }
    }
}

