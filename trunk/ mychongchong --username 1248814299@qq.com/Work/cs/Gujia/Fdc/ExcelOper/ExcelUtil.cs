using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Data;

namespace ExcelOper
{
    public class ExcelUtil :BaseExcel, IExcelUtil,IBaseExcel
    { 
        public ExcelUtil():base()
        {
        }
        /// <summary>
        /// 设置表格的值 ,整个;
        /// </summary>
        /// <param name="value"> data table</param>
        /// <param name="wsheet">当前 工作表</param>
        /// <param name="row">索引行</param>
        /// <param name="colum">索引列</param>
        public void setCellValue(System.Data.DataTable value, Worksheet wsheet, int rowindex, int columindex)
        {
            if (value == null) return;
            if (value.Rows.Count <= 0) return;
            for (int i = 0; i < value.Rows.Count; i++)
            {
                setCellValue(value.Rows[i], wsheet, rowindex + i, columindex,value.Columns.Count);
            }


        }
        /// <summary>
        /// 设置表格的值 ,行;
        /// </summary>
        /// <param name="value"> 值   datarow 代表一行</param>
        /// <param name="wsheet">当前 工作表</param>
        /// <param name="row">索引行</param>
        /// <param name="colum">索引列</param>
        public void setCellValue(System.Data.DataRow value, Worksheet wsheet, int rowindex, int colum,int count)
        {
            
            for (int i = 0; i <count; i++)
            {
                setCellValue(value[i].ToString().Trim(), wsheet, rowindex, colum+i);
                
            }


        }

        /// <summary>
        /// 设置表格的值 ,单个;
        /// </summary>
        /// <param name="value">   单个单元格的值 </param>
        /// <param name="wsheet">当前 工作表</param>
        /// <param name="row">索引行</param>
        /// <param name="colum">索引列</param>
        public void setCellValue(string value, Worksheet wsheet, int rowindex, int columindex)
        {
            try
            {                
                wsheet.Cells[rowindex, columindex] = value;
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void setCellValue(string value, Worksheet wsheet, object rowindex, object columindex)
        {
            try
            {
                wsheet.Cells[rowindex, columindex] = value;
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new Exception(ex.Message);

            }
        }









    }
}
