namespace SkyMap.Net.Gui.Components
{
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.IO;
    using System.Windows.Forms;

    public class BlobPropertyEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (((context != null) && (context.Instance != null)) && (provider != null))
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Title = "请选择文件";
                dialog.Filter = "Word 文件 (*.doc)|*.doc|Rdlc 文件 (*.rdlc)|*.rdlc|Word 文件 (*.xml)|*.xml|All files (*.*)|*.*";
                if (value != null)
                {
                    if (value.ToString().EndsWith(".doc"))
                    {
                        dialog.FilterIndex = 1;
                    }
                    if (value.ToString().EndsWith(".rdlc"))
                    {
                        dialog.FilterIndex = 2;
                    }
                    if (value.ToString().EndsWith(".xml"))
                    {
                        dialog.FilterIndex = 3;
                    }
                }
                else
                {
                    dialog.FilterIndex = 2;
                }
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    value = dialog.FileName;
                }
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if ((context != null) && (context.Instance != null))
            {
                return UITypeEditorEditStyle.Modal;
            }
            return base.GetEditStyle(context);
        }

        private static byte[] Read2Buffer(Stream stream, int BufferLen)
        {
            BinaryReader reader = new BinaryReader(stream);
            byte[] buffer = new byte[stream.Length];
            for (long i = 0L; i < stream.Length; i += 1L)
            {
                buffer[(int) ((IntPtr) i)] = reader.ReadByte();
            }
            return buffer;
        }
    }
}

