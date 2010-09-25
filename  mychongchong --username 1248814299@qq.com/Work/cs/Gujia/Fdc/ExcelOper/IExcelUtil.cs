using System;
using Microsoft.Office.Interop.Excel;
namespace ExcelOper
{
   public interface IExcelUtil
    {
        /// <summary>
        /// 设置表格的值 ,整个;
        /// </summary>
        /// <param name="value"> data table</param>
        /// <param name="wsheet">当前 工作表</param>
        /// <param name="row">索引行</param>
        /// <param name="colum">索引列</param>
        void setCellValue(string value, Worksheet wsheet, int rowindex, int columindex);

        /// <summary>
        /// 设置表格的值 ,整个;
        /// </summary>
        /// <param name="value"> data table</param>
        /// <param name="wsheet">当前 工作表</param>
        /// <param name="row">索引行</param>
        /// <param name="colum">索引列</param>
        void setCellValue(System.Data.DataRow value, Worksheet wsheet, int rowindex, int colum, int count);

        /// <summary>
        /// 设置表格的值 ,整个;
        /// </summary>
        /// <param name="value"> data table</param>
        /// <param name="wsheet">当前 工作表</param>
        /// <param name="row">索引行</param>
        /// <param name="colum">索引列</param>
        void setCellValue(System.Data.DataTable value, Worksheet wsheet, int rowindex, int columindex);
    }
}
