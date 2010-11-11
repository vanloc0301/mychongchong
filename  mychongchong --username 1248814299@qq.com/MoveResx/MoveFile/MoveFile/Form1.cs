using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MoveFile
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.listBoxLog.Items.Clear();
            // 获取当前路径下全部文件名
            String[] files = Directory.GetFiles(Environment.CurrentDirectory);
            foreach (String filename in files)
            {
                // 最后一个"\"
                if (filename.EndsWith(".resx"))
                {
                    int lastpath = filename.LastIndexOf("\\");
                    // 最后一个"."
                    int lastdot = filename.LastIndexOf(".");
                    // 纯文件名字长度
                    int length = lastdot - lastpath - 1;
                    // 文件目录字符串 xx\xx\xx\
                    String beginpart = filename.Substring(0, lastpath + 1);
                    // 纯文件名字
                    String namenoext = filename.Substring(lastpath + 1, length);
                    // 扩展名
                    String ext = filename.Substring(lastdot);

                    // 补齐为3位，组成新的文件名
                    String namenew, tmpnew;
                    namenew = namenoext.Substring(namenoext.LastIndexOf(".") + 1);
                    tmpnew = namenoext.Replace(namenew, "").Replace(".", "\\");
                    String fullnewname = beginpart + tmpnew + namenew + ext;

                    // 改名
                    File.Move(filename, fullnewname);

                    // log
                    this.listBoxLog.Items.Add(namenoext + "--->" + namenew);
                    this.listBoxLog.SelectedIndex = this.listBoxLog.Items.Count - 1;

                }
            }
        }
    }
}
