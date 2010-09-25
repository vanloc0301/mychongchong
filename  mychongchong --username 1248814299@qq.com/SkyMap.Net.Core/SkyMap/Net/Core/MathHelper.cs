namespace SkyMap.Net.Core
{
    using System;

    public static class MathHelper
    {
        public static double Round(double d, int decimals)
        {
            if (decimals < 0)
            {
                throw new ArgumentOutOfRangeException("指定的小数位数不能小于0");
            }
            double num = d;
            string str = num.ToString();
            if ((str.IndexOf(".") >= 0) && (((str.Length - str.IndexOf(".")) - 1) >= decimals))
            {
                double num2 = 5.0;
                for (int i = 0; i < (decimals + 1); i++)
                {
                    num2 /= 10.0;
                }
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.DebugFormatted("四舍五入自定义算法加权值:{0}", new object[] { num2 });
                }
                num += num2;
                if (decimals > 0)
                {
                    num *= Math.Pow(10.0, (double) decimals);
                    if (LoggingService.IsDebugEnabled)
                    {
                        LoggingService.DebugFormatted("四舍五入自定义算法放大后是:{0}", new object[] { num });
                    }
                    if (num.ToString().IndexOf(".") > 0)
                    {
                        num = Math.Floor(num);
                    }
                    if (LoggingService.IsDebugEnabled)
                    {
                        LoggingService.DebugFormatted("四舍五入自定义算法放大后的最小整数是:{0}", new object[] { num });
                    }
                    num /= Math.Pow(10.0, (double) decimals);
                    if (LoggingService.IsDebugEnabled)
                    {
                        LoggingService.DebugFormatted("四舍五入自定义算法最后结果是:{0}", new object[] { num });
                    }
                }
                else if (num.ToString().IndexOf(".") > 0)
                {
                    num = Math.Floor(num);
                }
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.DebugFormatted("{0} 四舍五入 {1} 位小数后结果为：{2}", new object[] { d, decimals, num });
                }
            }
            return num;
        }
    }
}

