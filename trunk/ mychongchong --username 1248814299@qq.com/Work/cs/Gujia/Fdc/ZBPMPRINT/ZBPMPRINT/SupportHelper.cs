using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMap.Net.ZSTax
{
    class SupportHelper
    {

        /// 
        /// ת�����ֽ��������������С���� 
        /// 
        /// �����ַ��� 
        /// ת�������Ĵ�д����ַ������߳�����Ϣ��ʾ�ַ��� 
        public static string convertsum(string str) 
        { 
            if(!ispositvedecimal(str)) 
                return "����Ĳ��������֣�"; 
            if(double.Parse(str)>999999999999.99) 
                return "����̫���޷����㣬������һ����Ԫ���µĽ��"; 
            char[] ch=new char[1]; 
            ch[0]='.'; //С���� 
            string[] splitstr=null; //���尴С����ָ����ַ������� 
            splitstr=str.Split(ch[0]);//��С����ָ��ַ��� 
            if(splitstr.Length==1) //ֻ���������� 
                return convertdata(str)+"Ԫ��"; 
            else //��С������ 
            { 
                string rstr; 
                rstr=convertdata(splitstr[0])+"Ԫ";//ת���������� 
                rstr+=convertxiaoshu(splitstr[1]);//ת��С������ 
                return rstr; 
            } 

        }

        /// 
        /// �ڷֺ������ 
        /// 
        /// �����ַ��� 
        /// ת�������Ĵ�д����ַ������߳�����Ϣ��ʾ�ַ��� 
        public static string convertsum1(string str)
        {
            if (!ispositvedecimal(str))
                return "����Ĳ��������֣�";
            if (double.Parse(str) > 999999999999.99)
                return "����̫���޷����㣬������һ����Ԫ���µĽ��";
            char[] ch = new char[1];
            ch[0] = '.'; //С���� 
            string[] splitstr = null; //���尴С����ָ����ַ������� 
            splitstr = str.Split(ch[0]);//��С����ָ��ַ��� 
            if (splitstr.Length == 1) //ֻ���������� 
                return convertdata(str) + "Ԫ��";
            else //��С������ 
            {
                string rstr;
                rstr = convertdata(splitstr[0]) + "Ԫ";//ת���������� 
                rstr += convertxiaoshu1(splitstr[1]);//ת��С������ 
                return rstr;
            }

        }
        /// 
        /// ���⴦���ڽ�ʱҪ�������
        /// 
        /// ��Ҫת����С�����������ַ��� 
        /// ת�������Ĵ�д����ַ��� 
        public static string convertxiaoshu1(string str)
        {
            int strlen = str.Length;
            string rstr;
            if (strlen == 1)
            {
                rstr = convertchinese(str) + "����";
                return rstr;
            }
            else
            {
                string tmpstr = str.Substring(0, 1);
                rstr = convertchinese(tmpstr) + "��";
                tmpstr = str.Substring(1, 1);
                rstr += convertchinese(tmpstr) + "��";
                rstr = rstr.Replace("���", "");
                rstr = rstr.Replace("���", "");
                return rstr;
            }


        }
        /// 
        /// �ж��Ƿ����������ַ��� 
        /// 
        /// �ж��ַ��� 
        /// ��������֣�����true�����򷵻�false 
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
        /// ת�����֣������� 
        /// 
        /// ��Ҫת�������������ַ��� 
        /// ת�������Ĵ�д����ַ��� 
        public static string convertdata(string str)
        {
            string tmpstr = "";
            string rstr = "";
            int strlen = str.Length;
            if (strlen <= 4)//���ֳ���С����λ 
            {
                rstr = convertdigit(str);

            }
            else
            {

                if (strlen <= 8)//���ֳ��ȴ�����λ��С�ڰ�λ 
                {
                    tmpstr = str.Substring(strlen - 4, 4);//�Ƚ�ȡ�����λ���� 
                    rstr = convertdigit(tmpstr);//ת�������λ���� 
                    tmpstr = str.Substring(0, strlen - 4);//��ȡ�������� 
                    //������ת�������ּ����f�������� 
                    rstr = string.Concat(convertdigit(tmpstr) + "�f", rstr);
                    rstr = rstr.Replace("���f", "�f");
                    rstr = rstr.Replace("����", "��");

                }
                else
                    if (strlen <= 12)//���ֳ��ȴ��ڰ�λ��С��ʮ��λ 
                    {
                        tmpstr = str.Substring(strlen - 4, 4);//�Ƚ�ȡ�����λ���� 
                        rstr = convertdigit(tmpstr);//ת�������λ���� 
                        tmpstr = str.Substring(strlen - 8, 4);//�ٽ�ȡ��λ���� 
                        rstr = string.Concat(convertdigit(tmpstr) + "�f", rstr);
                        tmpstr = str.Substring(0, strlen - 8);
                        rstr = string.Concat(convertdigit(tmpstr) + "�|", rstr);
                        rstr = rstr.Replace("��|", "�|");
                        rstr = rstr.Replace("���f", "�f");
                        rstr = rstr.Replace("����", "��");
                        rstr = rstr.Replace("����", "��");
                    }
            }
            strlen = rstr.Length;
            if (strlen >= 2)
            {
                switch (rstr.Substring(strlen - 2, 2))
                {
                    case "����": rstr = rstr.Substring(0, strlen - 2) + "��"; break;
                    case "Ǫ��": rstr = rstr.Substring(0, strlen - 2) + "Ǫ"; break;
                    case "�f��": rstr = rstr.Substring(0, strlen - 2) + "�f"; break;
                    case "�|��": rstr = rstr.Substring(0, strlen - 2) + "�|"; break;

                }
            }

            return rstr;
        }
        /// 
        /// ת�����֣�С�����֣� 
        /// 
        /// ��Ҫת����С�����������ַ��� 
        /// ת�������Ĵ�д����ַ��� 
        public static string convertxiaoshu(string str)
        {
            int strlen = str.Length;
            string rstr;
            if (strlen == 1)
            {
                rstr = convertchinese(str) + "��";
                return rstr;
            }
            else
            {
                string tmpstr = str.Substring(0, 1);
                rstr = convertchinese(tmpstr) + "��";
                tmpstr = str.Substring(1, 1);
                rstr += convertchinese(tmpstr) + "��";
                rstr = rstr.Replace("���", "");
                rstr = rstr.Replace("���", "");
                return rstr;
            }


        }
    
        /// 
        /// ת������ 
        /// 
        /// ת�����ַ�������λ���ڣ� 
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
            rstr = rstr.Replace("ʰ��", "ʰ");
            strlen = rstr.Length;

            return rstr;
        }


        /// 
        /// ת����λ���� 
        /// 
        public static string convert4digit(string str)
        {
            string str1 = str.Substring(0, 1);
            string str2 = str.Substring(1, 1);
            string str3 = str.Substring(2, 1);
            string str4 = str.Substring(3, 1);
            string rstring = "";
            rstring += convertchinese(str1) + "Ǫ";
            rstring += convertchinese(str2) + "��";
            rstring += convertchinese(str3) + "ʰ";
            rstring += convertchinese(str4);
            rstring = rstring.Replace("��Ǫ", "��");
            rstring = rstring.Replace("���", "��");
            rstring = rstring.Replace("��ʰ", "��");
            rstring = rstring.Replace("����", "��");
            rstring = rstring.Replace("����", "��");
            rstring = rstring.Replace("����", "��");
            return rstring;
        }
        /// 
        /// ת����λ���� 
        /// 
        public static string convert3digit(string str)
        {
            string str1 = str.Substring(0, 1);
            string str2 = str.Substring(1, 1);
            string str3 = str.Substring(2, 1);
            string rstring = "";
            rstring += convertchinese(str1) + "��";
            rstring += convertchinese(str2) + "ʰ";
            rstring += convertchinese(str3);
            rstring = rstring.Replace("���", "��");
            rstring = rstring.Replace("��ʰ", "��");
            rstring = rstring.Replace("����", "��");
            rstring = rstring.Replace("����", "��");
            return rstring;
        }
        /// 
        /// ת����λ���� 
        /// 
        public static string convert2digit(string str)
        {
            string str1 = str.Substring(0, 1);
            string str2 = str.Substring(1, 1);
            string rstring = "";
            rstring += convertchinese(str1) + "ʰ";
            rstring += convertchinese(str2);
            rstring = rstring.Replace("��ʰ", "��");
            rstring = rstring.Replace("����", "��");
            return rstring;
        }
        /// 
        /// ��һλ����ת�������Ĵ�д���� 
        /// 
        public static string convertchinese(string str)
        {
            //"��Ҽ��������½��ƾ�ʰ��Ǫ�f�|Ԫ���Ƿ�" 
            string cstr = "";
            switch (str)
            {
                case "0": cstr = "��"; break;
                case "1": cstr = "Ҽ"; break;
                case "2": cstr = "��"; break;
                case "3": cstr = "��"; break;
                case "4": cstr = "��"; break;
                case "5": cstr = "��"; break;
                case "6": cstr = "½"; break;
                case "7": cstr = "��"; break;
                case "8": cstr = "��"; break;
                case "9": cstr = "��"; break;
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
                        temp2 += "��";
                        break;
                    case 1:
                        temp2 += "��";
                        break;
                    case 2:
                        temp2 += "Ԫ";
                        break;
                    case 3:
                    case 7:
                        temp2 += "ʰ";
                        break;
                    case 4:
                    case 8:
                        temp2 += "��";
                        break;
                    case 5:
                    case 9:
                        temp2 += "Ǫ";
                        break;
                    case 6:
                        temp2 += "��";
                        break;
                    case 10:
                        temp2 += "��";
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
                n = Convert.ToInt16(str[i].ToString());//charת����,ת��Ϊ�ַ�������ת����
                switch (n)
                {
                    case 0: rstr = rstr + "��"; break;
                    case 1: rstr = rstr + "Ҽ"; break;
                    case 2: rstr = rstr + "��"; break;
                    case 3: rstr = rstr + "��"; break;
                    case 4: rstr = rstr + "��"; break;
                    case 5: rstr = rstr + "��"; break;
                    case 6: rstr = rstr + "½"; break;
                    case 7: rstr = rstr + "��"; break;
                    case 8: rstr = rstr + "��"; break;
                    default: rstr = rstr + "��"; break;
                }
            }
            return rstr;
        }

        //��ת��Ϊ��д
        public static string monthtoUpper(int month)
        {
            String str = month.ToString();
            if (month < 10)
            {
                return "��" + numtoUpper(month);
            }
            else if (month == 10)
            {
                return "Ҽʰ";
            }
            else
            {
                return "Ҽʰ" + numtoUpper(month - 10);
            }
        }
        //��ת��Ϊ��д
        public static string daytoUpper(int day)
        {
            String str = day.ToString();
            if (day < 10)
            {
                return "��" + numtoUpper(day);
            }
            else if (day == 10)
            {
                return "Ҽʰ";
            }
            else if ( 10<day && day < 20)
            {
                return "Ҽʰ" + numtoUpper(Convert.ToInt16(str[1].ToString()));
            }
            else if(day == 20)
            {
                return "��ʰ";
            }
            else if( 20 < day && day <30)
            {
                return "��ʰ" + numtoUpper(Convert.ToInt16(str[1].ToString()));
            }
            else if (day == 30)
            {
                return "��ʰ";
            }
            else
            {
                return "��ʰҼ";
            }
        }

    }
}
