using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMap.Net.ZSTax
{
    class SupportHelper
    {

        /// 
        /// 转换数字金额主函数（包括小数） 
        /// 
        /// 数字字符串 
        /// 转换成中文大写后的字符串或者出错信息提示字符串 
        public static string convertsum(string str) 
        { 
            if(!ispositvedecimal(str)) 
                return "输入的不是正数字！"; 
            if(double.Parse(str)>999999999999.99) 
                return "数字太大，无法换算，请输入一万亿元以下的金额"; 
            char[] ch=new char[1]; 
            ch[0]='.'; //小数点 
            string[] splitstr=null; //定义按小数点分割后的字符串数组 
            splitstr=str.Split(ch[0]);//按小数点分割字符串 
            if(splitstr.Length==1) //只有整数部分 
                return convertdata(str)+"元整"; 
            else //有小数部分 
            { 
                string rstr; 
                rstr=convertdata(splitstr[0])+"元";//转换整数部分 
                rstr+=convertxiaoshu(splitstr[1]);//转换小数部分 
                return rstr; 
            } 

        }

        /// 
        /// 在分后加正的 
        /// 
        /// 数字字符串 
        /// 转换成中文大写后的字符串或者出错信息提示字符串 
        public static string convertsum1(string str)
        {
            if (!ispositvedecimal(str))
                return "输入的不是正数字！";
            if (double.Parse(str) > 999999999999.99)
                return "数字太大，无法换算，请输入一万亿元以下的金额";
            char[] ch = new char[1];
            ch[0] = '.'; //小数点 
            string[] splitstr = null; //定义按小数点分割后的字符串数组 
            splitstr = str.Split(ch[0]);//按小数点分割字符串 
            if (splitstr.Length == 1) //只有整数部分 
                return convertdata(str) + "元整";
            else //有小数部分 
            {
                string rstr;
                rstr = convertdata(splitstr[0]) + "元";//转换整数部分 
                rstr += convertxiaoshu1(splitstr[1]);//转换小数部分 
                return rstr;
            }

        }
        /// 
        /// 特殊处理在角时要正的情况
        /// 
        /// 需要转换的小数部分数字字符串 
        /// 转换成中文大写后的字符串 
        public static string convertxiaoshu1(string str)
        {
            int strlen = str.Length;
            string rstr;
            if (strlen == 1)
            {
                rstr = convertchinese(str) + "角整";
                return rstr;
            }
            else
            {
                string tmpstr = str.Substring(0, 1);
                rstr = convertchinese(tmpstr) + "角";
                tmpstr = str.Substring(1, 1);
                rstr += convertchinese(tmpstr) + "分";
                rstr = rstr.Replace("零分", "");
                rstr = rstr.Replace("零角", "");
                return rstr;
            }


        }
        /// 
        /// 判断是否是正数字字符串 
        /// 
        /// 判断字符串 
        /// 如果是数字，返回true，否则返回false 
        public static bool ispositvedecimal(string str)
        {
            decimal d;
            try
            {
                d = decimal.Parse(str);

            }
            catch (Exception)
            {
                return false;
            }
            if (d > 0)
                return true;
            else
                return false;
        }
        /// 
        /// 转换数字（整数） 
        /// 
        /// 需要转换的整数数字字符串 
        /// 转换成中文大写后的字符串 
        public static string convertdata(string str)
        {
            string tmpstr = "";
            string rstr = "";
            int strlen = str.Length;
            if (strlen <= 4)//数字长度小于四位 
            {
                rstr = convertdigit(str);

            }
            else
            {

                if (strlen <= 8)//数字长度大于四位，小于八位 
                {
                    tmpstr = str.Substring(strlen - 4, 4);//先截取最后四位数字 
                    rstr = convertdigit(tmpstr);//转换最后四位数字 
                    tmpstr = str.Substring(0, strlen - 4);//截取其余数字 
                    //将两次转换的数字加上萬后相连接 
                    rstr = string.Concat(convertdigit(tmpstr) + "萬", rstr);
                    rstr = rstr.Replace("零萬", "萬");
                    rstr = rstr.Replace("零零", "零");

                }
                else
                    if (strlen <= 12)//数字长度大于八位，小于十二位 
                    {
                        tmpstr = str.Substring(strlen - 4, 4);//先截取最后四位数字 
                        rstr = convertdigit(tmpstr);//转换最后四位数字 
                        tmpstr = str.Substring(strlen - 8, 4);//再截取四位数字 
                        rstr = string.Concat(convertdigit(tmpstr) + "萬", rstr);
                        tmpstr = str.Substring(0, strlen - 8);
                        rstr = string.Concat(convertdigit(tmpstr) + "億", rstr);
                        rstr = rstr.Replace("零億", "億");
                        rstr = rstr.Replace("零萬", "萬");
                        rstr = rstr.Replace("零零", "零");
                        rstr = rstr.Replace("零零", "零");
                    }
            }
            strlen = rstr.Length;
            if (strlen >= 2)
            {
                switch (rstr.Substring(strlen - 2, 2))
                {
                    case "佰零": rstr = rstr.Substring(0, strlen - 2) + "佰"; break;
                    case "仟零": rstr = rstr.Substring(0, strlen - 2) + "仟"; break;
                    case "萬零": rstr = rstr.Substring(0, strlen - 2) + "萬"; break;
                    case "億零": rstr = rstr.Substring(0, strlen - 2) + "億"; break;

                }
            }

            return rstr;
        }
        /// 
        /// 转换数字（小数部分） 
        /// 
        /// 需要转换的小数部分数字字符串 
        /// 转换成中文大写后的字符串 
        public static string convertxiaoshu(string str)
        {
            int strlen = str.Length;
            string rstr;
            if (strlen == 1)
            {
                rstr = convertchinese(str) + "角";
                return rstr;
            }
            else
            {
                string tmpstr = str.Substring(0, 1);
                rstr = convertchinese(tmpstr) + "角";
                tmpstr = str.Substring(1, 1);
                rstr += convertchinese(tmpstr) + "分";
                rstr = rstr.Replace("零分", "");
                rstr = rstr.Replace("零角", "");
                return rstr;
            }


        }
    
        /// 
        /// 转换数字 
        /// 
        /// 转换的字符串（四位以内） 
        /// 
        public static string convertdigit(string str)
        {
            int strlen = str.Length;
            string rstr = "";
            switch (strlen)
            {
                case 1: rstr = convertchinese(str); break;
                case 2: rstr = convert2digit(str); break;
                case 3: rstr = convert3digit(str); break;
                case 4: rstr = convert4digit(str); break;
            }
            rstr = rstr.Replace("拾零", "拾");
            strlen = rstr.Length;

            return rstr;
        }


        /// 
        /// 转换四位数字 
        /// 
        public static string convert4digit(string str)
        {
            string str1 = str.Substring(0, 1);
            string str2 = str.Substring(1, 1);
            string str3 = str.Substring(2, 1);
            string str4 = str.Substring(3, 1);
            string rstring = "";
            rstring += convertchinese(str1) + "仟";
            rstring += convertchinese(str2) + "佰";
            rstring += convertchinese(str3) + "拾";
            rstring += convertchinese(str4);
            rstring = rstring.Replace("零仟", "零");
            rstring = rstring.Replace("零佰", "零");
            rstring = rstring.Replace("零拾", "零");
            rstring = rstring.Replace("零零", "零");
            rstring = rstring.Replace("零零", "零");
            rstring = rstring.Replace("零零", "零");
            return rstring;
        }
        /// 
        /// 转换三位数字 
        /// 
        public static string convert3digit(string str)
        {
            string str1 = str.Substring(0, 1);
            string str2 = str.Substring(1, 1);
            string str3 = str.Substring(2, 1);
            string rstring = "";
            rstring += convertchinese(str1) + "佰";
            rstring += convertchinese(str2) + "拾";
            rstring += convertchinese(str3);
            rstring = rstring.Replace("零佰", "零");
            rstring = rstring.Replace("零拾", "零");
            rstring = rstring.Replace("零零", "零");
            rstring = rstring.Replace("零零", "零");
            return rstring;
        }
        /// 
        /// 转换二位数字 
        /// 
        public static string convert2digit(string str)
        {
            string str1 = str.Substring(0, 1);
            string str2 = str.Substring(1, 1);
            string rstring = "";
            rstring += convertchinese(str1) + "拾";
            rstring += convertchinese(str2);
            rstring = rstring.Replace("零拾", "零");
            rstring = rstring.Replace("零零", "零");
            return rstring;
        }
        /// 
        /// 将一位数字转换成中文大写数字 
        /// 
        public static string convertchinese(string str)
        {
            //"零壹贰叁肆伍陆柒捌玖拾佰仟萬億元整角分" 
            string cstr = "";
            switch (str)
            {
                case "0": cstr = "零"; break;
                case "1": cstr = "壹"; break;
                case "2": cstr = "贰"; break;
                case "3": cstr = "叁"; break;
                case "4": cstr = "肆"; break;
                case "5": cstr = "伍"; break;
                case "6": cstr = "陆"; break;
                case "7": cstr = "柒"; break;
                case "8": cstr = "捌"; break;
                case "9": cstr = "玖"; break;
            }
            return (cstr);
        }

        public static string SimpleConverChinese(double amount,int max)
        {
            string temp1 = amount.ToString("0.00").Replace(".",string.Empty).PadLeft(max.ToString().Length+2,' ');
            string result = string.Empty;
            for (int i = 0; i<temp1.Length; i++)
            {
                char c = temp1[temp1.Length - i - 1];
                string temp2=(c==' ')?c.ToString():convertchinese(c.ToString());
                switch (i)
                {
                    case 0:
                        temp2 += "分";
                        break;
                    case 1:
                        temp2 += "角";
                        break;
                    case 2:
                        temp2 += "元";
                        break;
                    case 3:
                    case 7:
                        temp2 += "拾";
                        break;
                    case 4:
                    case 8:
                        temp2 += "佰";
                        break;
                    case 5:
                    case 9:
                        temp2 += "仟";
                        break;
                    case 6:
                        temp2 += "万";
                        break;
                    case 10:
                        temp2 += "亿";
                        break;
                }
                result = temp2 + result;
            }
            return result;
        }

        public static string numtoUpper(int num)
        {
            String str = num.ToString();
            string rstr = "";
            int n;
            for (int i = 0; i < str.Length; i++)
            {
                n = Convert.ToInt16(str[i].ToString());//char转数字,转换为字符串，再转数字
                switch (n)
                {
                    case 0: rstr = rstr + "零"; break;
                    case 1: rstr = rstr + "壹"; break;
                    case 2: rstr = rstr + "贰"; break;
                    case 3: rstr = rstr + "叁"; break;
                    case 4: rstr = rstr + "肆"; break;
                    case 5: rstr = rstr + "伍"; break;
                    case 6: rstr = rstr + "陆"; break;
                    case 7: rstr = rstr + "柒"; break;
                    case 8: rstr = rstr + "捌"; break;
                    default: rstr = rstr + "玖"; break;
                }
            }
            return rstr;
        }

        //月转化为大写
        public static string monthtoUpper(int month)
        {
            String str = month.ToString();
            if (month < 10)
            {
                return "零" + numtoUpper(month);
            }
            else if (month == 10)
            {
                return "壹拾";
            }
            else
            {
                return "壹拾" + numtoUpper(month - 10);
            }
        }
        //日转化为大写
        public static string daytoUpper(int day)
        {
            String str = day.ToString();
            if (day < 10)
            {
                return "零" + numtoUpper(day);
            }
            else if (day == 10)
            {
                return "壹拾";
            }
            else if ( 10<day && day < 20)
            {
                return "壹拾" + numtoUpper(Convert.ToInt16(str[1].ToString()));
            }
            else if(day == 20)
            {
                return "贰拾";
            }
            else if( 20 < day && day <30)
            {
                return "贰拾" + numtoUpper(Convert.ToInt16(str[1].ToString()));
            }
            else if (day == 30)
            {
                return "叁拾";
            }
            else
            {
                return "叁拾壹";
            }
        }

    }
}
