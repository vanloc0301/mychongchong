using System;
using Microsoft.Office.Interop.Excel;
using System.Collections;
namespace ExcelOper
{
    public interface IExcelFromArrayList : IExcelUtil
    {
        /// <summary>
        /// 设置表格的值 ,整个;
        /// </summary>
        /// <param name="value"> ArrayList 二维数组</param>
        /// <param name="wsheet">当前 工作表</param>
        /// <param name="row">索引行</param>
        /// <param name="colum">索引列</param>
        void setCellValue(ArrayList value, Worksheet wsheet, int rowindex, int colum, int count);

        /// <summary>
        /// 替换单格的内容
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="wsheet"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columIndex"></param>
        /// <returns></returns>
        bool replaceCellValue(string oldValue, string newValue, Worksheet wsheet, int rowIndex, int columIndex);
    }
}
