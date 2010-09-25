namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Gui;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security;
    using SkyMap.Net.Tools.Organize;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows.Forms;

    public class CreateRoleFromAssemblyCommand : AbstractRoleSecurityCommand
    {
        public override void Run()
        {
            if (this.IsEnabled)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "dll files (*.dll)|*.dll|exe files(*.exe)|*.exe";
                dialog.FilterIndex = 1;
                dialog.InitialDirectory = Environment.CurrentDirectory;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Assembly assembly = Assembly.LoadFile(dialog.FileName);
                    if (assembly == null)
                    {
                        MessageHelper.ShowInfo("不能载入表单程序集,请检查程序集是否正确!");
                    }
                    else if (this.Owner is RoleTypeNode)
                    {
                        RoleTypeNode owner = (RoleTypeNode) this.Owner;
                        CRoleType domainObject = owner.DomainObject;
                        List<string> list = new List<string>();
                        foreach (System.Type type2 in assembly.GetTypes())
                        {
                            if (!(type2.IsAbstract || !type2.IsSubclassOf(typeof(AbstractRoleSecurityCommand))))
                            {
                                list.Add(type2.FullName);
                            }
                        }
                        owner.AddNodes<CRole, RoleNode>(OGMDAOService.Instance.CreateRoles(domainObject, list.ToArray()));
                    }
                }
            }
        }
    }
}

