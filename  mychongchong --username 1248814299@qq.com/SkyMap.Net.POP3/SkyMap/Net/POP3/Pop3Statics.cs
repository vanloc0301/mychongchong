namespace SkyMap.Net.POP3
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    public class Pop3Statics
    {
        public const string DataFolder = @"c:\POP3Temp";

        public static string FromQuotedPrintable(string inString)
        {
            string str = null;
            string str2 = inString.Replace("=\n", "");
            if (str2.Length > 3)
            {
                str = "";
                int startIndex = 0;
                while (startIndex < str2.Length)
                {
                    string str3 = str2.Substring(startIndex, 1);
                    if (str3.Equals("=") && ((startIndex + 2) < str2.Length))
                    {
                        string str4 = str2.Substring(startIndex + 1, 2);
                        if (Regex.Match(str4.ToUpper(), "^[A-F|0-9]+[A-F|0-9]+$").Success)
                        {
                            str = str + Encoding.ASCII.GetString(new byte[] { Convert.ToByte(str4, 0x10) });
                            startIndex += 3;
                        }
                        else
                        {
                            str = str + str3;
                            startIndex++;
                        }
                    }
                    else
                    {
                        str = str + str3;
                        startIndex++;
                    }
                }
            }
            else
            {
                str = str2;
            }
            return str.Replace("\n", "\r\n");
        }
    }
}

