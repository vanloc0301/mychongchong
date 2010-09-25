namespace SkyMap.Net.ZSTax.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security;
    using SkyMap.Net.Workflow.Client.Box;
    using System;
    using System.Data;

    public class GTOAPassToNext : AbstractMenuCommand
    {
        public override void Run()
        {
            try
            {
                IWfBox owner = (IWfBox) this.Owner;
                if (owner != null)
                {
                    CStaff staff = OGMService.GetStaff(SecurityUtil.GetSmIdentity().UserId);
                    foreach (DataRow row in owner.GetSelectedRows())
                    {
                        QueryHelper.ExecuteSqlScalar("OLD_GTOA", string.Format("exec AcceptActins {0},{1}", row["ACTINS_ID"], staff.UserName));
                    }
                }
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
                MessageHelper.ShowInfo("发生错误:" + exception.Message);
            }
        }
    }
}

