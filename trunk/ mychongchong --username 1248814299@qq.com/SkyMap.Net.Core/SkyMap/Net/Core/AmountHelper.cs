namespace SkyMap.Net.Core
{
    using System;

    public class AmountHelper
    {
        public static string convert2digit(string str)
        {
            string str2 = str.Substring(0, 1);
            string str3 = str.Substring(1, 1);
            string str4 = "";
            return ((str4 + convertchinese(str2) + "拾") + convertchinese(str3)).Replace("零拾", "零").Replace("零零", "零");
        }

        public static string convert3digit(string str)
        {
            string str2 = str.Substring(0, 1);
            string str3 = str.Substring(1, 1);
            string str4 = str.Substring(2, 1);
            string str5 = "";
            return (((str5 + convertchinese(str2) + "佰") + convertchinese(str3) + "拾") + convertchinese(str4)).Replace("零佰", "零").Replace("零拾", "零").Replace("零零", "零").Replace("零零", "零");
        }

        public static string convert4digit(string str)
        {
            string str2 = str.Substring(0, 1);
            string str3 = str.Substring(1, 1);
            string str4 = str.Substring(2, 1);
            string str5 = str.Substring(3, 1);
            string str6 = "";
            return ((((str6 + convertchinese(str2) + "仟") + convertchinese(str3) + "佰") + convertchinese(str4) + "拾") + convertchinese(str5)).Replace("零仟", "零").Replace("零佰", "零").Replace("零拾", "零").Replace("零零", "零").Replace("零零", "零").Replace("零零", "零");
        }

        public static string convertchinese(string str)
        {
            switch (str)
            {
                case "0":
                    return "零";

                case "1":
                    return "壹";

                case "2":
                    return "贰";

                case "3":
                    return "叁";

                case "4":
                    return "肆";

                case "5":
                    return "伍";

                case "6":
                    return "陆";

                case "7":
                    return "柒";

                case "8":
                    return "捌";

                case "9":
                    return "玖";
            }
            return "";
        }

        public static string convertdata(string str)
        {
            string str3 = "";
            int length = str.Length;
            if (length <= 4)
            {
                str3 = convertdigit(str);
            }
            else if (length <= 8)
            {
                str3 = convertdigit(str.Substring(length - 4, 4));
                str3 = (convertdigit(str.Substring(0, length - 4)) + "万" + str3).Replace("零万", "万").Replace("零零", "零");
            }
            else if (length <= 12)
            {
                str3 = convertdigit(str.Substring(length - 4, 4));
                str3 = convertdigit(str.Substring(length - 8, 4)) + "万" + str3;
                str3 = (convertdigit(str.Substring(0, length - 8)) + "億" + str3).Replace("零億", "億").Replace("零万", "万").Replace("零零", "零").Replace("零零", "零");
            }
            length = str3.Length;
            if (length < 2)
            {
                return str3;
            }
            string str5 = str3.Substring(length - 2, 2);
            if (str5 == null)
            {
                return str3;
            }
            if (!(str5 == "佰零"))
            {
                if (str5 != "仟零")
                {
                    if (str5 == "万零")
                    {
                        return (str3.Substring(0, length - 2) + "万");
                    }
                    if (str5 != "億零")
                    {
                        return str3;
                    }
                    return (str3.Substring(0, length - 2) + "億");
                }
            }
            else
            {
                return (str3.Substring(0, length - 2) + "佰");
            }
            return (str3.Substring(0, length - 2) + "仟");
        }

        public static string convertdigit(string str)
        {
            int length = str.Length;
            string str2 = "";
            switch (length)
            {
                case 1:
                    str2 = convertchinese(str);
                    break;

                case 2:
                    str2 = convert2digit(str);
                    break;

                case 3:
                    str2 = convert3digit(str);
                    break;

                case 4:
                    str2 = convert4digit(str);
                    break;
            }
            str2 = str2.Replace("拾零", "拾");
            length = str2.Length;
            return str2;
        }

        public static string convertsum(string str)
        {
            if (!ispositvedecimal(str))
            {
                return "输入的不是正数字！";
            }
            if (double.Parse(str) > 999999999999.99)
            {
                return "数字太大，无法换算，请输入一万亿元以下的金额";
            }
            char[] chArray = new char[] { '.' };
            string[] strArray = null;
            strArray = str.Split(new char[] { chArray[0] });
            if (strArray.Length == 1)
            {
                return (convertdata(str) + "元整");
            }
            return (convertdata(strArray[0]) + "元" + convertxiaoshu(strArray[1]));
        }

        public static string convertsum1(string str)
        {
            if (!ispositvedecimal(str))
            {
                return "输入的不是正数字！";
            }
            if (double.Parse(str) > 999999999999.99)
            {
                return "数字太大，无法换算，请输入一万亿元以下的金额";
            }
            char[] chArray = new char[] { '.' };
            string[] strArray = null;
            strArray = str.Split(new char[] { chArray[0] });
            if (strArray.Length == 1)
            {
                return (convertdata(str) + "元整");
            }
            return (convertdata(strArray[0]) + "元" + convertxiaoshu1(strArray[1]));
        }

        public static string convertxiaoshu(string str)
        {
            if (str.Length == 1)
            {
                return (convertchinese(str) + "角");
            }
            string str2 = convertchinese(str.Substring(0, 1)) + "角";
            string str3 = str.Substring(1, 1);
            return (str2 + convertchinese(str3) + "分").Replace("零分", "").Replace("零角", "");
        }

        public static string convertxiaoshu1(string str)
        {
            if (str.Length == 1)
            {
                return (convertchinese(str) + "角整");
            }
            string str2 = convertchinese(str.Substring(0, 1)) + "角";
            string str3 = str.Substring(1, 1);
            return (str2 + convertchinese(str3) + "分").Replace("零分", "").Replace("零角", "");
        }

        public static bool ispositvedecimal(string str)
        {
            decimal num;
            try
            {
                num = decimal.Parse(str);
            }
            catch (Exception)
            {
                return false;
            }
            return (num > 0M);
        }

        public static string SimpleConverChinese(double amount, int max)
        {
            string str = amount.ToString("0.00").Replace(".", string.Empty).PadLeft(max.ToString().Length + 2, '●');
            string str2 = string.Empty;
            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[(str.Length - i) - 1];
                string str3 = (ch == '●') ? ch.ToString() : convertchinese(ch.ToString());
                switch (i)
                {
                    case 0:
                        str3 = str3 + "分";
                        break;

                    case 1:
                        str3 = str3 + "角";
                        break;

                    case 2:
                        str3 = str3 + "元";
                        break;

                    case 3:
                    case 7:
                        str3 = str3 + "拾";
                        break;

                    case 4:
                    case 8:
                        str3 = str3 + "佰";
                        break;

                    case 5:
                    case 9:
                        str3 = str3 + "仟";
                        break;

                    case 6:
                        str3 = str3 + "万";
                        break;

                    case 10:
                        str3 = str3 + "亿";
                        break;
                }
                str2 = str3 + str2;
            }
            return str2;
        }
    }
}

