using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjx.lib
{
    internal class ormExcel
    {
        //orm 实际上可以使用 append模式
        internal static void save(object SortedListx, string dbfl)
        {
            SortedList  SortedList1 = (SortedList )SortedListx;
          

            ArrayList lst=QryExcel(dbfl);
            lst.Add((SortedList)SortedList1);

            wriToDbf(lst,dbfl);

          
        }

        internal static void saveAppendMode(SortedList SortedList1, string dbfl)
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

            //append
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
                    worksheet.Cell(row, column).Value = entry.Value.ToString(); // 写入值
                    column++;
                }

                // 保存工作簿到文件
                workbook.SaveAs(dbfl);
            }
        }


        private static void wriToDbf(object  List_mapx, string dbf)
        {
           Print(" wriToDbf（）：" + dbf);
            File.Delete(dbf);

            System.Collections. ArrayList List_map = (ArrayList)List_mapx;
            // 创建一个新的Excel工作簿
            using (var workbook = new XLWorkbook())
            {

                // 添加一个新的工作表
                var worksheet = workbook.Worksheets.Add("Sheet1");


                //-------------------add title
                int row;
                  wrtTitleClms(List_map, worksheet);

                // 定义初始行和列  for datarow

                row = 2;
                int column = 1;


                foreach (SortedList SortedList1 in List_map)
                {
                    //  worksheet.Cell(row, column).Value = entry.Key.ToString(); // 写入键
                    //worksheet.Cell(row, column).Value = entry.Value.ToString(); // 写入值
                    //column++;
                    column = 1;
                    // 写入哈希表数据到工作表
                    foreach (DictionaryEntry entry in SortedList1)
                    {
                        //  worksheet.Cell(row, column).Value = entry.Key.ToString(); // 写入键
                        worksheet.Cell(row, column).Value = entry.Value.ToString(); // 写入值
                        column++;
                    }
                    row++;
                }


                // 保存工作簿到文件
                workbook.SaveAs(dbf);
            }
        }

        private static void wrtTitleClms(ArrayList List_map, IXLWorksheet worksheet)
        {
            var row = 1;
            int column = 1;
            //must for ,,beir last obj is more colume,cant show write clm
            foreach (SortedList SortedList1 in List_map)
            {
                column = 1;
                SortedList titleMap = SortedList1;
                // 写入哈希表数据到工作表
                foreach (DictionaryEntry entry in titleMap)
                {
                    worksheet.Cell(row, column).Value = entry.Key.ToString(); // 写入键
                                                                              // worksheet.Cell(row + 1, column).Value = entry.Value.ToString(); // 写入值
                    column++;
                }
            }

            return  ;
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

       public static ArrayList QryExcel(string dbf)
        {
            if (!File.Exists(dbf))
            {
                return [];

            }
            // 打开一个现有的Excel工作簿
            using (var workbook = new XLWorkbook(dbf))
            {
                // 选择要读取的工作表
                var worksheet = workbook.Worksheet("Sheet1");

                // 创建一个列表来存储读取到的数据
                var rws = new ArrayList();

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
                    var rowData = new SortedList();
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
