namespace SkyMap.Net.WebCamLibrary
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Util;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;

    public class ZQESFWebCamReportVistor : IReportVistor
    {
        private IProjectCaputure caller;

        private DataTable AddImgTable(string localpath, List<CategoryIdentities.Identify> ciis, string tableName)
        {
            DataTable table = new DataTable(tableName);
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Img", typeof(byte[]));
            foreach (CategoryIdentities.Identify identify in ciis)
            {
                string path = string.Format(@"{0}\{1}.jpg", localpath, identify.Name);
                if (File.Exists(path))
                {
                    DataRow row = table.NewRow();
                    row[0] = identify.Name;
                    row[1] = File.ReadAllBytes(path);
                    table.Rows.Add(row);
                }
            }
            return table;
        }

        public void Execute(byte[] reportData, DataSet ds)
        {
            if (this.caller != null)
            {
                string str = ds.Tables["YW_ZQRSF"].Rows[0]["PROJECT_ID"].ToString();
                string localpath = FileUtility.Combine(new string[] { FileUtility.ApplicationRootPath, "CameraProject", str });
                DataSet set = ds.Copy();
                set.Tables.Add(this.AddImgTable(localpath, this.caller.CIS[0].Identifies, "ZRFPics"));
                set.Tables.Add(this.AddImgTable(localpath, this.caller.CIS[1].Identifies, "SRFPics"));
                PrintHelper.PrintOrShowRDLC("二手房转让确认书", true, reportData, set, null, null, null);
            }
        }

        public object Caller
        {
            set
            {
                if (!(value is IProjectCaputure))
                {
                    throw new ArgumentOutOfRangeException("表单没有实现视频摄像接口：IProjectCaputure");
                }
                this.caller = value as IProjectCaputure;
            }
        }
    }
}

