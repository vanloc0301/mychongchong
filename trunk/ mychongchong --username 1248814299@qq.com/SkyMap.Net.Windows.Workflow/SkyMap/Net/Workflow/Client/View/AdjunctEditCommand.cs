namespace SkyMap.Net.Workflow.Client.View
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Security;
    using SkyMap.Net.Security.Principal;
    using SkyMap.Net.Workflow.Instance;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Forms;

    public class AdjunctEditCommand : AbstractMenuCommand
    {
        protected AdjunctEdit ae;

        protected WfAdjunctFile GetAdjunctFile(WfAdjunct adj)
        {
            if (adj.File != null)
            {
                return adj.File;
            }
            WfAdjunctFile file = new WfAdjunctFile(adj);
            IList<WfAdjunctFile> list = QueryHelper.List<WfAdjunctFile>(string.Empty, new string[] { "Id" }, new string[] { adj.Id });
            if (list.Count == 0)
            {
                throw new NullReferenceException("Not find the adjunct file");
            }
            file.Data = list[0].Data;
            return file;
        }

        internal bool IsHaveEditAdjuctPermission(WfAdjunct adj)
        {
            SmIdentity smIdentity = SecurityUtil.GetSmIdentity();
            return (adj.CreateStaffId == smIdentity.UserId);
        }

        internal void OpenAdjunct()
        {
            List<WfAdjunct> selectedAdjuncts = this.ae.GetSelectedAdjuncts();
            foreach (WfAdjunct adjunct in selectedAdjuncts)
            {
                WfAdjunctFile adjunctFile = this.GetAdjunctFile(adjunct);
                this.OpenFile(adjunctFile);
            }
        }

        private void OpenFile(WfAdjunctFile adjunctFile)
        {
            if (adjunctFile == null)
            {
                throw new ArgumentNullException("WfAdjunctFile cannot be null");
            }
            if (adjunctFile.Data.Length > 0)
            {
                try
                {
                    Process.Start(this.SaveFile(adjunctFile));
                }
                catch (Exception exception)
                {
                    MessageHelper.ShowInfo("你可能已经打开同名文件！\r打开文件时发生错误：" + exception.Message);
                }
            }
            else
            {
                MessageHelper.ShowInfo("附件大小等于0，无法打开文件！");
            }
        }

        protected static byte[] Read2Buffer(Stream stream, int BufferLen)
        {
            BinaryReader reader = new BinaryReader(stream);
            byte[] buffer = new byte[stream.Length];
            for (long i = 0L; i < stream.Length; i += 1L)
            {
                buffer[(int) ((IntPtr) i)] = reader.ReadByte();
            }
            return buffer;
        }

        public override void Run()
        {
            this.OpenAdjunct();
        }

        protected string SaveFile(WfAdjunctFile adjunctFile)
        {
            string path = Path.GetTempFileName() + adjunctFile.Type;
            File.WriteAllBytes(path, adjunctFile.Data);
            return path;
        }

        protected string SaveFile(WfAdjunctFile adjunctFile, string path)
        {
            string str = string.Concat(new object[] { path, Path.DirectorySeparatorChar, adjunctFile.Name, adjunctFile.Type });
            File.WriteAllBytes(str, adjunctFile.Data);
            return str;
        }

        internal bool SelectFileToAdjuct(WfAdjunct adjunct)
        {
            if (adjunct == null)
            {
                throw new NullReferenceException("WfAdjunct cannot be null");
            }
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "请选择附件";
            dialog.Filter = "Word 文件 (*.doc)|*.txt|All files (*.*)|*.*";
            dialog.FilterIndex = 2;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Stream stream = dialog.OpenFile();
                if (stream != null)
                {
                    if (adjunct.File == null)
                    {
                        adjunct.File = new WfAdjunctFile(adjunct);
                    }
                    adjunct.File.Data = Read2Buffer(stream, -1);
                    stream.Close();
                    adjunct.Type = Path.GetExtension(dialog.FileName);
                    if (StringHelper.IsNull(adjunct.Name))
                    {
                        adjunct.Name = Path.GetFileNameWithoutExtension(dialog.FileName);
                    }
                    return true;
                }
            }
            return false;
        }

        public override object Owner
        {
            get
            {
                return this.ae;
            }
            set
            {
                this.ae = value as AdjunctEdit;
            }
        }
    }
}

