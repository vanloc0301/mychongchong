namespace SkyMap.Net.POP3
{
    using System;
    using System.Text.RegularExpressions;

    public class Pop3Parse
    {
        public const int ComponetsDone = -96;
        public const int ContentDescriptionType = 2;
        public const int ContentDispositionType = 3;
        public const int ContentTransferEncodingType = 1;
        public const int ContentTypeType = 0;
        public const int EndOfHeader = -98;
        public const int FilenameType = 1;
        private static string[] m_lineSubTypeString = new string[] { "Content-Type", "Content-Transfer-Encoding", "Content-Description", "Content-Disposition" };
        private static string[] m_lineUpperTypeString = new string[] { "From", "To", "Subject", "Content-Type" };
        private static string[] m_nextLineTypeString = new string[] { "name", "filename" };
        public const int MultipartBoundaryFound = -97;
        public const int NameType = 0;
        public const int UnknownType = -99;

        public static string ContentDescription(string line)
        {
            return Regex.Replace(line, "^Content-Description: (.*)$", "$1");
        }

        public static string ContentDisposition(string line)
        {
            return Regex.Replace(line, "^Content-Disposition: (.*)$", "$1");
        }

        public static string ContentTransferEncoding(string line)
        {
            return Regex.Replace(line, "^Content-Transfer-Encoding: (.*)$", "$1");
        }

        public static string ContentType(string line)
        {
            return Regex.Replace(line, "^Content-Type: (.*)$", "$1");
        }

        public static string Filename(string line)
        {
            return Regex.Replace(line, "^[ |\t]+filename=[\"]*([^\"]*).*$", "$1");
        }

        public static string From(string line)
        {
            return Regex.Replace(line, @"^From:.*[ |<]([a-z|A-Z|0-9|\.|\-|_]+@[a-z|A-Z|0-9|\.|\-|_]+).*$", "$1");
        }

        public static int GetSubHeaderLineType(string line, string boundary)
        {
            for (int i = 0; i < LineSubTypeString.Length; i++)
            {
                string str = LineSubTypeString[i];
                if (Regex.Match(line, "^" + str + ":.*$").Success)
                {
                    return i;
                }
                if (line.Equals("--" + boundary))
                {
                    return -97;
                }
                if (line.Equals("--" + boundary + "--"))
                {
                    return -96;
                }
                if (line.Length == 0)
                {
                    return -98;
                }
            }
            return -99;
        }

        public static int GetSubHeaderNextLineType(string line)
        {
            for (int i = 0; i < NextLineTypeString.Length; i++)
            {
                string str = NextLineTypeString[i];
                if (Regex.Match(line, "^[ |\t]+" + str + "=.*$").Success)
                {
                    return i;
                }
                if (line.Length == 0)
                {
                    return -98;
                }
            }
            return -99;
        }

        public static bool IsMultipart(string line)
        {
            return Regex.Match(line, "^multipart/.*").Success;
        }

        public static string MultipartBoundary(string line)
        {
            return Regex.Replace(line, "^.*boundary=[\"]*([^\"]*).*$", "$1");
        }

        public static string Name(string line)
        {
            return Regex.Replace(line, "^[ |\t]+name=[\"]*([^\"]*).*$", "$1");
        }

        public static string Subject(string line)
        {
            return Regex.Replace(line, "^Subject: (.*)$", "$1");
        }

        public static string To(string line)
        {
            return Regex.Replace(line, @"^To:.*[ |<]([a-z|A-Z|0-9|\.|\-|_]+@[a-z|A-Z|0-9|\.|\-|_]+).*$", "$1");
        }

        public static string[] LineSubTypeString
        {
            get
            {
                return m_lineSubTypeString;
            }
        }

        public static string[] LineUpperTypeString
        {
            get
            {
                return m_lineUpperTypeString;
            }
        }

        public static string[] NextLineTypeString
        {
            get
            {
                return m_nextLineTypeString;
            }
        }
    }
}

