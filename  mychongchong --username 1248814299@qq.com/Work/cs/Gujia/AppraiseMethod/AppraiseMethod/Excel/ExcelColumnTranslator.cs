
using System;
using System.Text;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace AppraiseMethod.Excel
{
    public class ExcelColumnTranslator
    {
        private ExcelColumnTranslator()
        {
        }

        public static int ToIndex(string columnName)
        {
            if (!Regex.IsMatch(columnName.ToUpper(), @"[A-Z]+"))
                throw new Exception("invalid parameter");
            int index = 0;
            char[] chars = columnName.ToUpper().ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                index += ((int)chars[i] - (int)'A' + 1) * (int)Math.Pow(26, chars.Length - i - 1);
            }
            return index - 1;
        }

        public static string ToName(int index)
        {
            if (index < 0)
                throw new Exception("invalid parameter");
            List<string> chars = new List<string>();
            do
            {
                if (chars.Count > 0) index--;
                chars.Insert(0, ((char)(index % 26 + (int)'A')).ToString());
                index = (int)((index - index % 26) / 26);
            } while (index > 0);

            return String.Join(string.Empty, chars.ToArray());
        }


        public static List<int> showMatches(string ms)
        {
            string expression = @"(?<col>[A-Z]+)(?<row>[0-9]+)";
            RegexOptions option = RegexOptions.IgnoreCase | RegexOptions.Multiline;
            List<int> li = new List<int>();
            Regex regex = new Regex(expression, option);
            MatchCollection matches = regex.Matches(ms);
            //show matches 
            //Console.WriteLine("////////////////----------------------------------////////////////"); 
            //Console.WriteLine(" string: "{0}" expression: "{1}" match result is:", ms, expression); 
            if (matches.Count != 1)
            {
                throw new Exception(string.Format("{0}：不是有效的描述方式，请检查数据", ms));
            }
            foreach (Match m in matches)
            {
                //foreach (string name in regex.GetGroupNames())
                //{
                //    string tmp = string.Format(" capture group {0} value is:{1}", name, m.Groups[name].Value);
                //}
                int tmprow = Convert.ToInt32(m.Groups["row"].Value) - 1;
                int tmpcol = Convert.ToInt32(ToIndex(m.Groups["col"].Value));
                li.Add(tmprow);
                li.Add(tmpcol);
                return li;
            }
            return null;
            //Console.WriteLine("matched count: {0}", matches.Count); 
        }
    }
}