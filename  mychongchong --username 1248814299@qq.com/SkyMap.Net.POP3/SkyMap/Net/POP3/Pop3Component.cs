namespace SkyMap.Net.POP3
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;

    public class Pop3Component
    {
        public byte[] m_binaryData;
        private string m_contentDescription;
        private string m_contentDisposition;
        private string m_contentTransferEncoding;
        private string m_contentType;
        private string m_data;
        private string m_filename;
        private string m_filePath;
        private string m_name;

        public Pop3Component(string contentType, string data)
        {
            this.m_contentType = contentType;
            this.m_data = data;
        }

        public Pop3Component(string contentType, string name, string filename, string contentTransferEncoding, string contentDescription, string contentDisposition, string data)
        {
            this.m_contentType = contentType;
            this.m_name = name;
            this.m_filename = filename;
            this.m_contentTransferEncoding = contentTransferEncoding;
            this.m_contentDescription = contentDescription;
            this.m_contentDisposition = contentDisposition;
            this.m_data = data;
            this.DecodeData();
        }

        private void DecodeData()
        {
            if (this.m_contentDisposition != null)
            {
                if (!Directory.Exists(@"c:\POP3Temp"))
                {
                    Directory.CreateDirectory(@"c:\POP3Temp");
                }
                this.m_filePath = @"c:\POP3Temp\" + this.m_filename;
                if (this.m_contentDisposition.Equals("attachment;") && this.m_contentTransferEncoding.ToUpper().Equals("BASE64"))
                {
                    this.m_binaryData = Convert.FromBase64String(this.m_data.Replace("\n", ""));
                    BinaryWriter writer = new BinaryWriter(new FileStream(this.m_filePath, FileMode.Create));
                    writer.Write(this.m_binaryData);
                    writer.Flush();
                    writer.Close();
                }
                else if (this.m_contentDisposition.Equals("attachment;") && this.m_contentTransferEncoding.ToUpper().Equals("QUOTED-PRINTABLE"))
                {
                    using (StreamWriter writer2 = File.CreateText(this.m_filePath))
                    {
                        writer2.Write(Pop3Statics.FromQuotedPrintable(this.m_data));
                        writer2.Flush();
                        writer2.Close();
                    }
                }
            }
        }

        public override string ToString()
        {
            return ("Content-Type: " + this.m_contentType + "\r\nName: " + this.m_name + "\r\nFilename: " + this.m_filename + "\r\nContent-Transfer-Encoding: " + this.m_contentTransferEncoding + "\r\nContent-Description: " + this.m_contentDescription + "\r\nContent-Disposition: " + this.m_contentDisposition + "\r\nData :" + this.m_data);
        }

        public string ContentDescription
        {
            get
            {
                return this.m_contentDescription;
            }
        }

        public string ContentDisposition
        {
            get
            {
                return this.m_contentDisposition;
            }
        }

        public string ContentTransferEncoding
        {
            get
            {
                return this.m_contentTransferEncoding;
            }
        }

        public string ContentType
        {
            get
            {
                return this.m_contentType;
            }
        }

        public string Data
        {
            get
            {
                return this.m_data;
            }
        }

        public string FileExtension
        {
            get
            {
                string str = null;
                if ((this.m_filename != null) && Regex.Match(this.m_filename, @"^.*\..*$").Success)
                {
                    str = Regex.Replace(this.m_name, @"^[^\.]*\.([^\.]+)$", "$1");
                }
                return str;
            }
        }

        public string Filename
        {
            get
            {
                return this.m_filename;
            }
        }

        public string FileNoExtension
        {
            get
            {
                string str = null;
                if ((this.m_filename != null) && Regex.Match(this.m_filename, @"^.*\..*$").Success)
                {
                    str = Regex.Replace(this.m_name, @"^([^\.]*)\.[^\.]+$", "$1");
                }
                return str;
            }
        }

        public string FilePath
        {
            get
            {
                return this.m_filePath;
            }
        }

        public bool IsAttachment
        {
            get
            {
                bool success = false;
                if (this.m_contentDisposition != null)
                {
                    success = Regex.Match(this.m_contentDisposition, "^attachment.*$").Success;
                }
                return success;
            }
        }

        public bool IsBody
        {
            get
            {
                return (this.m_contentDisposition == null);
            }
        }

        public string Name
        {
            get
            {
                return this.m_name;
            }
        }
    }
}

