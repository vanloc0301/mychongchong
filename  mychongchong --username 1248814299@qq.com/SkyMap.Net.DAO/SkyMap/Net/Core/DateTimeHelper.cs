namespace SkyMap.Net.Core
{
    using SkyMap.Net.DAO;
    using System;
    using System.Collections;
    using System.Data;
    using System.Diagnostics;

    public class DateTimeHelper
    {
        public const string CnDateFormat = "yyyy年MM月dd日";
        private const string fldHoliDate = "HOLI_DATE";
        private static DataView holiDays;
        private static DateTime? serverTime;
        private static Stopwatch stopWatch;

        public static string GetCnDayOfWeek(DateTime d)
        {
            switch (d.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return "星期天";

                case DayOfWeek.Monday:
                    return "星期一";

                case DayOfWeek.Tuesday:
                    return "星期二";

                case DayOfWeek.Wednesday:
                    return "星期三";

                case DayOfWeek.Thursday:
                    return "星期四";

                case DayOfWeek.Friday:
                    return "星期五";

                case DayOfWeek.Saturday:
                    return "星期六";
            }
            throw new ExecutionEngineException("无法解析的日期");
        }

        public static double GetCostTimeExcludingHoli(DateTime from, DateTime to)
        {
            double num = (to.Ticks - from.Ticks) / 0xc92a69c000L;
            num -= GetHoliCount(from, to);
            if (LoggingService.IsWarnEnabled)
            {
                LoggingService.Warn("from : " + from.ToShortDateString() + ",to:" + to.ToShortDateString() + ",cost time : " + num.ToString());
            }
            return num;
        }

        public static DateTime GetDateExcludingHoli(DateTime from, double duetime)
        {
            DateTime to = from.AddDays(duetime);
            if (holiDays != null)
            {
                for (double i = GetHoliCount(from, to); i > 0.0; i = GetHoliCount(from, to))
                {
                    from = to.AddDays(1.0);
                    to = to.AddDays(i - 1.0);
                }
            }
            return to;
        }

        public static double GetHoliCount(DateTime from, DateTime to)
        {
            if (holiDays != null)
            {
                holiDays.RowFilter = "HOLI_DATE>= '" + from.ToShortDateString() + "' and HOLI_DATE<= '" + to.ToShortDateString() + "'";
                if (LoggingService.IsWarnEnabled)
                {
                    LoggingService.Warn("from :" + from.ToShortDateString() + ",to : " + to.ToShortDateString() + ",have holidays:" + holiDays.Count.ToString());
                }
                return (double) holiDays.Count;
            }
            return 0.0;
        }

        public static DateTime GetNow()
        {
            if (!serverTime.HasValue)
            {
                stopWatch = new Stopwatch();
                stopWatch.Start();
                try
                {
                    serverTime = new DateTime?(Convert.ToDateTime(QueryHelper.ExecuteScalar("SkyMap.Net.Core", "GetServerTime")));
                    LoggingService.InfoFormatted("设置服务器时间为：{0}", new object[] { serverTime });
                }
                catch (Exception exception)
                {
                    LoggingService.Error(exception);
                    serverTime = new DateTime?(DateTime.Now);
                }
            }
            return serverTime.Value.AddMilliseconds((double) stopWatch.ElapsedMilliseconds);
        }

        public static DateTime[] GetWeekends(DateTime st, DateTime et)
        {
            if (st > et)
            {
                throw new ApplicationException("Start date must less than end date");
            }
            ArrayList list = new ArrayList();
            for (DateTime time = st.Date; time <= et; time = time.AddDays(1.0))
            {
                if ((time.DayOfWeek == DayOfWeek.Saturday) || (time.DayOfWeek == DayOfWeek.Sunday))
                {
                    list.Add(time);
                }
            }
            return (DateTime[]) list.ToArray(typeof(DateTime));
        }

        public static bool IsNull(DateTime dt)
        {
            return (Null.CompareTo(dt) == 0);
        }

        public static DataView HoliDays
        {
            get
            {
                if (holiDays == null)
                {
                    holiDays = QueryHelper.ExecuteSqlQuery("SkyMap.Net.Core", "WF_HOLIDAYS", "GetHolidays").DefaultView;
                    holiDays.Sort = "HOLI_DATE";
                }
                return holiDays;
            }
        }

        public static DateTime Null
        {
            get
            {
                return DateTime.Parse("0001/1/1");
            }
        }
    }
}

