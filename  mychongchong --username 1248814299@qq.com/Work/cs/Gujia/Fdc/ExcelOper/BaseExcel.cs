using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Diagnostics;

namespace ExcelOper
{
 public  class BaseExcel:IBaseExcel
    {
        protected Application app = new Application();
        protected Object  value = Missing.Value;
        protected Workbook work = null;


        #region IBaseExcel Members
        public Workbook GetWorkBook(string path)
        {
            try
            {
              
                this.work = app.Workbooks.Open(path, value, value, value, value, value, value, value, value, value, value, value, value, value, value);
                return work;

            }
            catch (Exception ex)
            {
                return null;

            }
        }

        public Worksheet GetSheet(object index)
        {
            try
            {
                return (Worksheet)work.Sheets[index];

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public bool Save()
        {
            try
			{
                work.Save();
			}
			catch
			{
				this.Close();
				return false;
			}
			return true;
        }

        public bool SaveAs(string fileName)
        {
            try
            {
                work.SaveAs(fileName, value, value, value, value, value, XlSaveAsAccessMode.xlNoChange, value, value, value, value, value);
            }
            catch
            {
                this.Close();
                return false;
            }
            return true;
        }

        public bool Close()
        {
            try
            {

                if (work != null) work.Close(false, null, null);

                if (app != null)
                {
                    app.DisplayAlerts = false;

                    app.Quit();

                    //关闭EXCEL进程

                    Process[] process = Process.GetProcesses();

                    foreach (Process p in process)
                    {
                        if (p.ProcessName == "EXCEL")
                        {
                            p.Kill();
                        }

                    }
                    return true;

                }
                return false;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message+"\n进程关闭错误!");
            }

        }

       

        public bool showExcel()
        {
            try
            {
                app.Visible = true;
                
                return true;
            }
            catch
            {
                return false;
            }

        }

        #endregion



        #region IBaseExcel Members

        public bool SetBookTitle(string title, Worksheet wsheet)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
