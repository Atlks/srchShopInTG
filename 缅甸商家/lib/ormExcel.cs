using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prj202405.lib
{
    internal class ormExcel
    {
        internal static void save(SortedList SortedList1, string dbfl)
        {

            if (!File.Exists(dbfl))
            {
              //  File.WriteAllText(dbfl, "[]");
                var workbook = new XLWorkbook();
                // 添加一个新的工作表
                var worksheet = workbook.Worksheets.Add("Sheet1");


                var row = 1;
                int column = 1;

                // 写入哈希表数据到工作表
                foreach (DictionaryEntry entry in SortedList1)
                {
                    worksheet.Cell(row, column).Value = entry.Key.ToString(); // 写入键
                   // worksheet.Cell(row + 1, column).Value = entry.Value.ToString(); // 写入值
                    column++;
                }
                // 保存工作簿到文件
                workbook.SaveAs(dbfl);

            }

            // 创建一个新的Excel工作簿
            using (var workbook = new XLWorkbook(dbfl))
            {
               
                // 选择要读取的工作表
                var worksheet = workbook.Worksheet("Sheet1");

                // 定义初始行和列

                var row = getNewRowIdx(worksheet);
                int column = 1;

                // 写入哈希表数据到工作表
                foreach (DictionaryEntry entry in SortedList1)
                {
                  //  worksheet.Cell(row, column).Value = entry.Key.ToString(); // 写入键
                    worksheet.Cell(row, column ).Value = entry.Value.ToString(); // 写入值
                    column++;
                }

                // 保存工作簿到文件
                workbook.SaveAs(dbfl);
            }
        }

        private static int getNewRowIdx(IXLWorksheet worksheet)
        {
            // 获取工作表的行和列
            var rows = worksheet.RowsUsed();

            if (worksheet.LastRowUsed() == null)
                return 1;


            // 找到最后一行
            var lastRowUsed = worksheet.LastRowUsed().RowNumber();
          
            // 下一行的起始位置
            int newRow = lastRowUsed + 1;
            // 读取每一行数据
            //foreach (var row in rows.Skip(1)) // 跳过标题行
            //{
                 
            //}
            return newRow;
        }

       public static List<Dictionary<string, object>> qry(string dbf)
        {
            // 打开一个现有的Excel工作簿
            using (var workbook = new XLWorkbook(dbf))
            {
                // 选择要读取的工作表
                var worksheet = workbook.Worksheet("Sheet1");

                // 创建一个列表来存储读取到的数据
                var rws = new List<Dictionary<string, object>>();

                // 获取工作表的行和列
                var rows = worksheet.RowsUsed();

                // 假设第一行是标题行
                var headers = new List<string>();
                foreach (var headerCell in rows.First().Cells())
                {
                    headers.Add(headerCell.GetValue<string>());
                }

                // 读取每一行数据
                foreach (var row in rows.Skip(1)) // 跳过标题行
                {
                    var rowData = new Dictionary<string, object>();
                    for (int i = 0; i < headers.Count; i++)
                    {
                        rowData[headers[i]] = row.Cell(i + 1).Value.ToString();
                    }
                    rws.Add(rowData);
                }
                return rws;

            }
          
        }
    }
}
