namespace SkyMap.Net.WebCamLibrary
{
    using SkyMap.Net.Configuration;
    using SkyMap.Net.Core;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.Client.View;
    using System;
    using System.IO;
    using System.Text;
    using VBWebCamSocket;

    public class ProjectCaptureCommand2 : AbstractMenuCommand
    {
        public override void Run()
        {
            IWfView owner = this.Owner as IWfView;
            if (owner != null)
            {
                IProjectCaputure dataForm = owner.DataForm as IProjectCaputure;
                if (dataForm != null)
                {
                    string str = owner.DataForm.DataFormConntroller.GetParamValue("ProjectId", string.Empty).ToString();
                    if (!string.IsNullOrEmpty(str))
                    {
                        string str2 = FileUtility.Combine(new string[] { FileUtility.ApplicationRootPath, "CameraProject", str });
                        CategoryIdentities[] cIS = dataForm.CIS;
                        string str4 = NiniConfigHelper.GetValueByMatchKeyFromXML(Path.Combine(PropertyService.ConfigDirectory, "CamReport.config"), "CamReport", str, string.Empty);
                        StringBuilder builder = new StringBuilder("<c>");
                        for (int i = 0; i < cIS.Length; i++)
                        {
                            if (i > 0)
                            {
                                builder.Append(@"\");
                            }
                            builder.Append(cIS[i].Name);
                        }
                        builder.Append("</c>");
                        for (int j = 0; j < cIS.Length; j++)
                        {
                            builder.AppendFormat("<c{0}>", j + 1);
                            for (int k = 0; k < cIS[j].Identifies.Count; k++)
                            {
                                if (k > 0)
                                {
                                    builder.Append(@"\");
                                }
                                builder.Append(cIS[j].Identifies[k].Name);
                            }
                            builder.AppendFormat("</c{0}>", j + 1);
                        }
                        builder.AppendFormat("<p>{0}</p><l>{1}</l><r>{2}</r><y>{0}</y><EOF>", str, str2, str4);
                        TCPSender sender = new TCPSender();
                        if (LoggingService.IsDebugEnabled)
                        {
                            LoggingService.DebugFormatted("将尝试发送：{0}进行拍照", new object[] { builder.ToString() });
                        }
                        if (!sender.StartClient(builder.ToString()))
                        {
                            LiveShow show = new LiveShow();
                            show.ReportOID = str4;
                            show.ShowDialog(str, str2, str, true, cIS);
                        }
                    }
                }
                else
                {
                    MessageHelper.ShowInfo("该业务未实现拍照取证");
                }
            }
        }
    }
}

