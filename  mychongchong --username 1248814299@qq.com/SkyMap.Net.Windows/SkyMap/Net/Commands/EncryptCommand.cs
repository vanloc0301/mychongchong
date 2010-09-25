namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using System;
    using System.IO;
    using System.Windows.Forms;

    public class EncryptCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "xml文件(*.xml)|*.xml|所有文件(*.*)|*.*";
                dialog.DefaultExt = "xml文件(*.xml)|*.xml";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string str;
                    using (StreamReader reader = new StreamReader(dialog.FileName))
                    {
                        str = reader.ReadToEnd();
                    }
                    string str2 = new CryptoHelper(CryptoTypes.encTypeDES).Encrypt(str);
                    using (SaveFileDialog dialog2 = new SaveFileDialog())
                    {
                        if (dialog2.ShowDialog() == DialogResult.OK)
                        {
                            using (StreamWriter writer = new StreamWriter(dialog2.FileName))
                            {
                                writer.Write(str2);
                            }
                            MessageBox.Show("生成了加密文件：\n\r" + dialog2.FileName);
                        }
                    }
                }
            }
        }
    }
}

