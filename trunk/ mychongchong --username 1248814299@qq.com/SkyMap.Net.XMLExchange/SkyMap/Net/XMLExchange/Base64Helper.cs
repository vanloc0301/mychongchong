namespace SkyMap.Net.XMLExchange
{
    using SkyMap.Net.Core;
    using System;
    using System.IO;
    using System.Text;

    public static class Base64Helper
    {
        public static string DecodeBase64(string code_type, string code)
        {
            byte[] bytes = Convert.FromBase64String(code);
            try
            {
                return Encoding.GetEncoding(code_type).GetString(bytes);
            }
            catch
            {
                return code;
            }
        }

        public static string EncodeBase64(string code_type, string code)
        {
            byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(code);
            try
            {
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return code;
            }
        }

        public static string XMLFileEncode(string file)
        {
            try
            {
                LoggingService.InfoFormatted("对文件'{0}'进行BASE64编码...", new object[] { file });
                if (!System.IO.File.Exists(file))
                {
                    return string.Empty;
                }
                StreamReader reader = System.IO.File.OpenText(file);
                return EncodeBase64("UTF-8", reader.ReadToEnd().Replace("<?xml version=\"1.0\" standalone=\"yes\"?>", "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"));
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}

