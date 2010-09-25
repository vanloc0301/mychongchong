namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.Client.Box;
    using SkyMap.Net.Workflow.Client.View;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Text;

    public class ShowZSGTJProjectInfoCommand : AbstractCommand
    {
        private string GetProinstStatus(int p)
        {
            string str = string.Empty;
            switch (p)
            {
                case 1:
                    return "办理中";

                case 2:
                    return "已办结";

                case 3:
                    return "已归档";

                case 4:
                    return "被挂起";

                case 5:
                    return "被回收";

                case 6:
                    return "被退回";

                case 7:
                    return "被退件";
            }
            return str;
        }

        public override void Run()
        {
            DoWorkEventHandler handler = null;
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("准备执行显示中山市国土局市局业务信息命令...");
            }
            string project_id = null;
            try
            {
                if (this.Owner is FlowInfo)
                {
                    project_id = (this.Owner as FlowInfo).ProjectID;
                }
                else if (this.Owner is IWfBox)
                {
                    int firstSelectedIndex = BoxHelper.GetFirstSelectedIndex(this.Owner as IWfBox);
                    if (firstSelectedIndex >= 0)
                    {
                        project_id = BoxHelper.GetDataRowView(this.Owner as IWfBox, firstSelectedIndex)["PROJECT_ID"].ToString();
                    }
                    else
                    {
                        MessageHelper.ShowInfo("你没有选择任何业务!");
                        return;
                    }
                }
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
            }
            if (!string.IsNullOrEmpty(project_id))
            {
                BackgroundWorker worker = new BackgroundWorker();
                if (handler == null)
                {
                    handler = delegate (object sender, DoWorkEventArgs e) {
                        Exception exception;
                        WaitDialogHelper.Show();
                        try
                        {
                            DataTable table = null;
                            try
                            {
                                table = QueryHelper.ExecuteSqlQuery("OLD_GTOA", string.Empty, "Get_GTOA_Proinst", new string[] { project_id, project_id });
                            }
                            catch (Exception exception1)
                            {
                                exception = exception1;
                                LoggingService.Error(exception);
                                MessageHelper.ShowInfo("网络无法不能连接至市局，查询失败");
                                return;
                            }
                            if (table.Rows.Count > 0)
                            {
                                DataRow row = table.Rows[0];
                                StringBuilder builder = new StringBuilder();
                                builder.AppendFormat("业务名称:{0},业务号:{1}\r\n", row["PRODEF_NAME"], row["PROJECT_ID"]);
                                LoggingService.DebugFormatted(builder.ToString(), new object[0]);
                                builder.AppendFormat("开始时间:{0}; 预计办理天数:{1};\r\n实际办结时间:{2};", Convert.IsDBNull(row["start_date"]) ? "" : Convert.ToDateTime(row["start_date"]).ToString("yyyy年MM月dd日"), Convert.IsDBNull(row["prodef_time"]) ? "未定义" : string.Format("{0}天; {1}", row["prodef_time"].ToString(), Convert.IsDBNull(row["def_date"]) ? "" : ("预计办结时间:" + Convert.ToDateTime(row["def_date"]).ToString("yyyy年MM月dd日"))), Convert.IsDBNull(row["end_date"]) ? string.Format("已办理{0}天,尚未办结", row["totalday"]) : Convert.ToDateTime(row["end_date"]).ToString("yyyy年MM月dd日"));
                                LoggingService.Debug(builder.ToString());
                                double num = Convert.ToDouble(row["ep"]);
                                builder.Append((num > 0.0) ? string.Format("业务已超期{0}天", num) : "业务总办理时间未超时");
                                LoggingService.DebugFormatted(builder.ToString(), new object[0]);
                                string proinstStatus = this.GetProinstStatus(Convert.ToInt32(row["proins_status"]));
                                if (!string.IsNullOrEmpty(proinstStatus))
                                {
                                    builder.AppendFormat("业务状态:{0}", proinstStatus);
                                    LoggingService.DebugFormatted(builder.ToString(), new object[0]);
                                }
                                DataTable table2 = QueryHelper.ExecuteSqlQuery("OLD_GTOA", string.Empty, "Get_GTOA_Actinst", new string[] { row["PROINS_ID"].ToString() });
                                builder.AppendFormat("\r\n\r\n", new object[0]);
                                string str2 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                                for (int k = 0; k < table2.Rows.Count; k++)
                                {
                                    DataRow row2 = table2.Rows[k];
                                    builder.AppendFormat("步骤{0}:{1}由{2}办理:时间从{3}至{4}\r\n", new object[] { k + 1, row2["actdef_name"], row2["staff_name"], Convert.ToDateTime(row2["st"]).ToString("yyyy年MM月dd日"), Convert.IsDBNull(row2["et"]) ? "今未完" : Convert.ToDateTime(row2["et"]).ToString("yyyy年MM月dd日") });
                                    LoggingService.DebugFormatted(builder.ToString(), new object[0]);
                                    if (row2["demo"].ToString().Length > 0)
                                    {
                                        string[] strArray = row2["demo"].ToString().Split(new char[] { ';' });
                                        int num3 = 0;
                                        for (int i = 0; i < strArray.Length; i++)
                                        {
                                            if (!string.IsNullOrEmpty(strArray[i]))
                                            {
                                                builder.AppendFormat("     {0}.{1}\r\n", str2[num3], strArray[i]);
                                                num3++;
                                                LoggingService.DebugFormatted(builder.ToString(), new object[0]);
                                            }
                                        }
                                    }
                                }
                                MessageService.ShowCustomDialog(string.Format("{0}市局业务办理情况", project_id), builder.ToString(), new string[] { "确定", "取消" });
                            }
                            else
                            {
                                MessageHelper.ShowInfo("找不到{0}相关联的市局关联业务,请确认市局是否已经开始办理本业务", project_id);
                            }
                        }
                        catch (Exception exception2)
                        {
                            exception = exception2;
                            LoggingService.Error(exception);
                        }
                        finally
                        {
                            WaitDialogHelper.Close();
                        }
                    };
                }
                worker.DoWork += handler;
                worker.RunWorkerAsync();
            }
            else
            {
                MessageHelper.ShowInfo("你没有选择任何业务!");
            }
        }
    }
}

