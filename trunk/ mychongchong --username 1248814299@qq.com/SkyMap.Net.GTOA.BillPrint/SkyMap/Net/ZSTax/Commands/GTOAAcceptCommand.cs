namespace SkyMap.Net.ZSTax.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security;
    using SkyMap.Net.Workflow.Client;
    using SkyMap.Net.Workflow.Client.Box;
    using System;
    using System.Data;

    public class GTOAAcceptCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            try
            {
                IWfBox owner = (IWfBox) this.Owner;
                if (owner != null)
                {
                    CStaff staff = OGMService.GetStaff(SecurityUtil.GetSmIdentity().UserId);
                    DataRow[] selectedRows = owner.GetSelectedRows();
                    foreach (DataRow row in selectedRows)
                    {
                        QueryHelper.ExecuteSqlScalar(owner.DAONameSpace, string.Format("exec AcceptActins {0},{1}", row["ACTINS_ID"], staff.UserName));
                    }
                    owner.DeleteRows(selectedRows);
                }
            }
            catch (NotSelectException)
            {
                MessageHelper.ShowInfo("请选择要签收的一个或多个业务!");
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
                MessageHelper.ShowInfo("发生错误:" + exception.Message);
            }
        }
    }
}

