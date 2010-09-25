namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using System;
    using System.IO;
    using System.Windows.Forms;

    public class DecryptCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "cxml文件(*.cxml)|*.cxml|所有文件(*.*)|*.*";
                dialog.DefaultExt = "xml文件(*.xml)|*.xml";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string str;
                    using (StreamReader reader = new StreamReader(dialog.FileName))
                    {
                        str = reader.ReadToEnd();
                    }
                    string str2 = new CryptoHelper(CryptoTypes.encTypeDES).Decrypt(str);
                    using (SaveFileDialog dialog2 = new SaveFileDialog())
                    {
                        if (dialog2.ShowDialog() == DialogResult.OK)
                        {
                            using (StreamWriter writer = new StreamWriter(dialog2.FileName))
                            {
                                writer.Write(str2);
                            }
                            MessageBox.Show("生成了解密文件：\n\r" + dialog2.FileName);
                        }
                    }
                }
            }
        }
    }
}

