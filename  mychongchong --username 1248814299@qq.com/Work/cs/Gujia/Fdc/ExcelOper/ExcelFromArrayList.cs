using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Microsoft.Office.Interop.Excel;

namespace ExcelOper
{
    public class ExcelFromArrayList : ExcelUtil, IExcelFromArrayList
    {
        public ExcelFromArrayList()
            : base()
        {

        }

        public bool SetBookTitle(string Title, Worksheet wsheet)
        {
            try
            {
                wsheet.Cells[1, 1] = Title;
                return true;
            }
            catch
            {
                return false;
            }
        }

        #region IExcelFromArrayList Members
        public void setCellValue(System.Collections.ArrayList value, Microsoft.Office.Interop.Excel.Worksheet wsheet, int rowindex, int colum, int count)
        {
            if (value.Count > 0)
            {
                string[,] strs = new string[value.Count, ((ArrayList)value[0]).Count];
                for (int i = 0; i < value.Count; i++)
                    for (int j = 0; j < ((ArrayList)value[i]).Count; j++)
                    {

                        strs[i, j] = (((ArrayList)value[i])[j]).ToString() == "0" ? "" : (((ArrayList)value[i])[j]).ToString();
                    }
                wsheet.get_Range(wsheet.Cells[rowindex, colum], wsheet.Cells[value.Count + rowindex - 1, ((ArrayList)value[0]).Count + colum - 1]).Value2 = strs;
            }
        }
        #endregion

        #region IExcelFromArrayList Members
        public bool replaceCellValue(string oldValue, string newValue, Worksheet wsheet, int rowIndex, int columIndex)
        {
            try
            {
                string val = ((Range)wsheet.Cells[rowIndex, columIndex]).Text.ToString();
                wsheet.Cells[rowIndex, columIndex] = val.Replace(oldValue, newValue);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
