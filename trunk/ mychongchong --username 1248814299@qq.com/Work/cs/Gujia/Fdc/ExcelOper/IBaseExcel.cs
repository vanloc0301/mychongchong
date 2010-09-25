using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Excel;

namespace ExcelOper
{
    public interface IBaseExcel
    {
        /// <summary>
        /// 设置Title
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        bool SetBookTitle(string title, Worksheet wsheet);

        /// <summary>
        /// 根据路径取得工作薄
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        Workbook GetWorkBook(string path);

        /// <summary>
        /// 根据索引取得工作表
        /// </summary>
        /// <param name="index">int 索引从1开始,或工作表名</param>
        /// <returns></returns>
        Worksheet GetSheet(object index);

        /// <summary>
        /// 保存到当前工作表中
        /// </summary>
        /// <returns></returns>
        bool Save();

        /// <summary>
        /// 另存为
        /// </summary>
        ///  <param name="fileName">带路径的文件名</param>
        /// <returns></returns>
        bool SaveAs(string fileName);

        /// <summary>
        /// 打开已经写好的表
        /// </summary>
        /// <returns></returns>
        bool showExcel();

        /// <summary>
        /// 关闭当前工作Excel
        /// </summary>
        /// <returns></returns>
        bool Close();





    }
}
