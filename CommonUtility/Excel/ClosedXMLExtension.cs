/*
    透過ClosedXML操作Excel，僅能支援xlsx。
 *  需安裝ClosedXML
 *      https://closedxml.codeplex.com/
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using System.Data;
using System.IO;

namespace CommonUtility.Excel
{
    /// <summary>
    /// CloseXML 擴充
    /// </summary>
    public class ClosedXMLExtension
    {
        /// <summary>
        /// 要讀取的Sheet的起始Index（最小1）
        /// </summary>
        private int SheetIndex { get; set; }

        /// <summary>
        /// 起始讀取Cell的Index（最小1）
        /// </summary>
        private int StartCellIndex {get; set; }

        public ClosedXMLExtension()
        { 
        
        }

        /// <summary>
        /// 匯入Excel 建構子
        /// </summary>
        /// <param name="SheetIndex">要讀取的Sheet的起始Index（最小1）</param>
        /// <param name="StartCellIndex">起始讀取Cell的Index（最小1）</param>
        public ClosedXMLExtension(int SheetIndex, int StartCellIndex = 1)
        {
            this.SheetIndex = SheetIndex;
            this.StartCellIndex = StartCellIndex;

            if (this.SheetIndex < 1)
                throw new ArgumentException("超出索引範圍", "SheetIndex 必須為1開始");

            if (this.StartCellIndex < 1)
                throw new ArgumentException("超出索引範圍", "StartCellIndex 必須為1開始");

        }

        /// <summary>
        /// 匯入Excle成DataTable
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public DataTable Import(string filePath)
        {
            var targetFile = new FileInfo(filePath);

            if (!targetFile.Exists)
                throw new ArgumentException("檔案不存在", "fileName");

            var wb = new XLWorkbook(filePath);

            return GenerateDataTable(wb);
        }

        /// <summary>
        /// 依據Excel文件產出DataTable
        /// </summary>
        /// <param name="workbook">Excel Workbook物件</param>
        /// <returns></returns>
        private DataTable GenerateDataTable(XLWorkbook workbook)
        {
            var sheet = workbook.Worksheet(SheetIndex);

            if (sheet.FirstCellUsed() == null && sheet.LastCellUsed() == null)
                throw new ArgumentException("資料索引異常", "無資料");

            var range = sheet.Range(sheet.FirstCellUsed(), sheet.LastCellUsed());

            int col = range.ColumnCount();

            // add columns hedars
            var dt = new DataTable();

            for (int i = 1; i <= col; i++)
            {
                var column = sheet.Cell(1, i);
                dt.Columns.Add(Convert.ToString(column.Value));
            }

            for (int i = 1; i <= range.RowCount(); i++)
            {
                if (i < this.StartCellIndex)
                    continue;

                var item = range.Row(i);

                var array = new object[col];
                for (int y = 1; y <= col; y++)
                {
                    array[y - 1] = item.Cell(y).Value;
                }
                dt.Rows.Add(array);
            }

            return dt;
        }
    }
}
