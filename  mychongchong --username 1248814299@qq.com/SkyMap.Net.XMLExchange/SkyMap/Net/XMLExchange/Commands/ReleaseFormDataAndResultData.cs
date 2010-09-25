namespace SkyMap.Net.XMLExchange.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.XMLExchange;
    using System;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml;
    using System.Xml.Serialization;

    public class ReleaseFormDataAndResultData : AbstractCommand
    {
        public override void Run()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("import tax payer ...");
            }
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.Title = "选择...";
            dialog.Filter = "xml 文件 (*.xml)|*.xml";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = System.IO.File.OpenRead(dialog.FileName))
                {
                    XmlReader xmlReader = XmlReader.Create(stream);
                    XmlSerializer serializer = new XmlSerializer(typeof(SkyMap.Net.XMLExchange.Message));
                    SkyMap.Net.XMLExchange.Message message = (SkyMap.Net.XMLExchange.Message) serializer.Deserialize(xmlReader);
                    Project item = message.eAppXML.Body.Content.Item as Project;
                    if (((item.Applying != null) && (item.Applying.Form != null)) && ((item.Applying.Form.FormData != null) && (item.Applying.Form.FormData.Length > 0)))
                    {
                        using (StreamWriter writer = new StreamWriter(Path.Combine(Path.GetDirectoryName(dialog.FileName), string.Format(message.eAppXML.Body.CaseIDInfo.ApplyingID + "_form.xml", new object[0]))))
                        {
                            writer.Write(Base64Helper.DecodeBase64("utf-8", Convert.ToBase64String(item.Applying.Form.FormData)));
                        }
                    }
                    if (((item.Result != null) && (item.Result.ResultData != null)) && (item.Result.ResultData.Length > 0))
                    {
                        using (StreamWriter writer2 = new StreamWriter(Path.Combine(Path.GetDirectoryName(dialog.FileName), string.Format(message.eAppXML.Body.CaseIDInfo.ApplyingID + "_result.xml", new object[0]))))
                        {
                            writer2.Write(Base64Helper.DecodeBase64("utf-8", Convert.ToBase64String(item.Result.ResultData)));
                        }
                    }
                }
                MessageHelper.ShowInfo("释放成功!");
            }
        }
    }
}

