using System;
using System.Collections.Generic;
using System.Text;

namespace ZBPM
{
    using System;
    using System.Collections;
    using System.Text;
    using System.Text.RegularExpressions;

    public class BaseConvert
    {
        public static string BB192S0(DateTime aa)
        {
            return string.Format("{0}年{1}月{2}日", aa.Year, aa.Month, aa.Day);
        }

        public static string GetYearMonthDay(DateTime aa)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(GetYearMonth(aa));
            builder.Append(GetDay("{cs}日", (long)aa.Day));
            return builder.ToString();
        }

        public static string GetYearMonth(DateTime aa)
        {
            StringBuilder builder = new StringBuilder();
            string str = aa.Year.ToString();
            for (int i = 0; i < str.Length; i++)
            {
                builder.Append(CovertChineseNumberString((long)(Convert.ToInt32(str[i]) - Convert.ToInt32('0'))));
            }
            builder.Append("年");
            builder.Append(GetDay("{cs}月", (long)aa.Month));
            return builder.ToString();
        }

        public static string GetDay(string aa, long ab)
        {
            ArrayList list = new ArrayList();
            Regex regex = new Regex("({[^}]+})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            for (Match match = regex.Match(aa); match.Success; match = match.NextMatch())
            {
                list.Add(match.Groups[1].ToString());
            }
            StringBuilder builder = new StringBuilder(aa);
            for (int i = 0; i < list.Count; i++)
            {
                switch (list[i].ToString())
                {
                    case "{n}":
                        builder.Replace("{n}", InttoString(ab));
                        break;

                    case "{cs}":
                        builder.Replace("{cs}", CovertChineseNumberString(ab));
                        break;

                    case "{ct}":
                        builder.Replace("{ct}", ConvertChineseMoneyBigString(ab));
                        break;

                    case "{su}":
                        builder.Replace("{su}", UpperChar(ab));
                        break;

                    case "{sl}":
                        builder.Replace("{sl}", LowerChar(ab));
                        break;
                }
            }
            return builder.ToString();
        }

        private static string InttoString(long aa)
        {
            return aa.ToString();
        }

        private static string UpperChar(long aa)
        {
            byte num = (byte)(Convert.ToByte('A') + ((byte)(aa - 1L)));
            return Convert.ToChar(num).ToString();
        }

        private static string LowerChar(long aa)
        {
            byte num = (byte)(Convert.ToByte('a') + ((byte)(aa - 1L)));
            return Convert.ToChar(num).ToString();
        }

        private static string CovertChineseNumberString(long aa)
        {
            string[] strArray = new string[] { "Ｏ", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };
            if (aa >= 0L)
            {
                if (aa <= 10L)
                {
                    return strArray[(int)((IntPtr)aa)];
                }
                if (aa < 20L)
                {
                    return ("十" + strArray[(int)((IntPtr)(aa % 10L))]);
                }
                if (aa < 100L)
                {
                    return (strArray[(int)((IntPtr)(aa / 10L))] + "十" + (((aa % 10L) == 0L) ? "" : strArray[(int)((IntPtr)(aa % 10L))]));
                }
            }
            return "";
        }

        private static string ConvertChineseMoneyBigString(long aa)
        {
            if (aa < 0L)
            {
                return "";
            }
            return Money.ConvertToCn(Convert.ToDecimal(aa.ToString()));
        }
    }

    public class Money
    {
        public static string ConvertToCn(decimal num)
        {
            string str1 = "零壹贰叁肆伍陆柒捌玖";            //0-9所对应的汉字 
            string str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字 
            string str3 = "";    //从原num值中取出的值 
            string str4 = "";    //数字的字符串形式 
            string str5 = "";  //人民币大写金额形式 
            int i;    //循环变量 
            int j;    //num的值乘以100的字符串长度 
            string ch1 = "";    //数字的汉语读法 
            string ch2 = "";    //数字位的汉字读法 
            int nzero = 0;  //用来计算连续的零值是几个 
            int temp;            //从原num值中取出的值 

            num = Math.Round(Math.Abs(num), 2);    //将num取绝对值并四舍五入取2位小数 
            str4 = ((long)(num * 100)).ToString();        //将num乘100并转换成字符串形式 
            j = str4.Length;      //找出最高位 
            if (j > 15) { return "溢出"; }
            str2 = str2.Substring(15 - j);   //取出对应位数的str2的值。如：200.55,j为5所以str2=佰拾元角分 

            //循环取出每一位需要转换的值 
            for (i = 0; i < j; i++)
            {
                str3 = str4.Substring(i, 1);          //取出需转换的某一位的值 
                temp = Convert.ToInt32(str3);      //转换为数字 
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //当所取位数不为元、万、亿、万亿上的数字时 
                    if (str3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "零" + str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //该位是万亿，亿，万，元位等关键位 
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "零" + str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (str3 != "0" && nzero == 0)
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (j - 11) || i == (j - 3))
                {
                    //如果该位是亿位或元位，则必须写上 
                    ch2 = str2.Substring(i, 1);
                }
                str5 = str5 + ch1 + ch2;

                if (i == j - 1 && str3 == "0")
                {
                    //最后一位（分）为0时，加上“整” 
                    str5 = str5 + '整';
                }
            }
            if (num == 0)
            {
                str5 = "零元整";
            }
            return str5;
        }
    }
}
