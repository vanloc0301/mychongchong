namespace SkyMap.Net.Core
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    public class StringHelper
    {
        public static char[] ABC = new char[] { 
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'O', 'P', 'Q', 
            'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
         };
        public const char SplitChar = ',';
        public static char[] SplitChars = new char[] { ',', ';' };

        public static bool ContainChineseChar(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                Regex regex = new Regex("^[一-龥]$");
                for (int i = 0; i < input.Length; i++)
                {
                    char ch = input[i];
                    if (regex.IsMatch(ch.ToString()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool Equals(string a, string b)
        {
            return Equals(a, b, true);
        }

        public static bool Equals(string a, string b, bool ignoreCase)
        {
            string str;
            string str2;
            bool flag = IsNull(a);
            bool flag2 = IsNull(b);
            if (flag && flag2)
            {
                return true;
            }
            if (flag || flag2)
            {
                return false;
            }
            if (ignoreCase)
            {
                str = a.ToUpper();
                str2 = b.ToUpper();
            }
            else
            {
                str = a;
                str2 = b;
            }
            return str.Equals(str2);
        }

        public static string GetNewGuid()
        {
            Guid guid = Guid.NewGuid();
            return Guid.NewGuid().ToString("N");
        }

        public static bool IsChinese(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            Regex regex = new Regex("^[一-龥]$");
            for (int i = 0; i < input.Length; i++)
            {
                char ch = input[i];
                if (!regex.IsMatch(ch.ToString()))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsNull(string s)
        {
            return (((s == null) || (s == string.Empty)) || (s.Trim().Length == 0));
        }

        public static string LinkForQuery(string[] vals)
        {
            StringBuilder builder = new StringBuilder();
            int num = vals.Length - 1;
            for (int i = 0; i <= num; i++)
            {
                builder.Append("'");
                builder.Append(vals[i]);
                builder.Append("'");
                if (i < num)
                {
                    builder.Append(",");
                }
            }
            if (builder.Length == 0)
            {
                builder.Append("''");
            }
            return builder.ToString();
        }

        public static string Replace(string template, string placeholder, string replacement)
        {
            if (template == null)
            {
                return null;
            }
            int index = template.IndexOf(placeholder);
            if (index < 0)
            {
                return template;
            }
            return new StringBuilder(template.Substring(0, index)).Append(replacement).Append(Replace(template.Substring(index + placeholder.Length), placeholder, replacement)).ToString();
        }

        public static string[] Split(string text)
        {
            return Split(text, SplitChars);
        }

        public static string[] Split(string text, char[] splitChars)
        {
            return text.Split(splitChars);
        }

        public static string[] Split(string text, char splitChar)
        {
            return text.Split(new char[] { splitChar });
        }
    }
}

