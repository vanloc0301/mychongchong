using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AppraiseMethod.Excel
{
    public class KillExcel
    {
        /// <summary>
        /// 结束Excel进程
        /// </summary>
        public static void KillExcelProcess(DateTime beforetime, DateTime aftertime)
        {
            Process[] myProcesses;
            DateTime startTime;
            myProcesses = Process.GetProcessesByName("Excel");

            //得不到Excel进程ID，暂时只能判断进程启动时间
            foreach (Process myProcess in myProcesses)
            {
                //加入try,因为有可能myProcess已退出 20091203
                try
                {
                    startTime = myProcess.StartTime;
                    string title = myProcess.MainWindowTitle;// 返回标题,在这里可以考虑做些判断，那样的话，杀进程将更加精确;

                    if (startTime > beforetime && startTime < aftertime)
                    {
                        myProcess.Kill();
                    }
                }
                catch
                { }
            }
        }

        public static void KillExcelProcess()
        {
            Process[] myProcesses;

            myProcesses = Process.GetProcessesByName("Excel");

            foreach (Process myProcess in myProcesses)
            {
                //加入try,因为有可能myProcess已退出 20091203
                try
                {

                    myProcess.Kill();
                }
                catch
                {
                }
            }
        }
    }
}
