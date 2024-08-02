global using static mdsj.lib.bscUi;
using HtmlAgilityPack;
using Nustache.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace mdsj.lib
{
    internal class bscUi
    {
        public static void RenderMarkdownTableToConsole(string markdownTable)
        {
            var lines = markdownTable.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            if (lines.Length == 0)
            {
                Console.WriteLine("Empty table");
                return;
            }

            // Trim and split lines into rows and cells
            var rows = lines.Select(line => line.Trim().Split('|').Select(cell => cell.Trim()).ToArray()).ToList();
            if (rows.Count < 2)
            {
                Console.WriteLine("Table does not contain enough rows.");
                return;
            }

            // Calculate column widths
            var columnWidths = Enumerable.Range(0, rows.Max(r => r.Length))
                                         .Select(col => rows.Max(row => row.ElementAtOrDefault(col)?.Length ?? 0))
                                         .ToArray();

            // Print the table with | symbols
            PrintRowWithSeparators(rows[0], columnWidths); // Header
            PrintSeparator(columnWidths); // Separator
            for (int i = 1; i < rows.Count; i++)
            {
                PrintRowWithSeparators(rows[i], columnWidths); // Data rows
            }
        }

        public static void PrintRowWithSeparators(string[] row, int[] columnWidths)
        {
            Console.Write("|"); // Start with the opening |
            for (int i = 0; i < columnWidths.Length; i++)
            {
                var cell = i < row.Length ? row[i] : string.Empty;
                Console.Write(cell.PadRight(columnWidths[i] + 2)); // +2 for padding
                Console.Write("|"); // End each cell with |
            }
            Console.WriteLine();
        }
     
        // 提取 <%= 和 %> 之间的表达式
        public static List<string> ExtractExpressions(string filePath)
        {
            var expressions = new List<string>();

            // 确保文件路径存在
            if (!System.IO.File.Exists(filePath))
            {
                return expressions;
            }

            // 读取文件内容
            string fileContent = System.IO.File.ReadAllText(filePath);

            // 正则表达式匹配 <%= ... %>
            string pattern = @"<%=([^%>]+)%>";
            Regex regex = new Regex(pattern, RegexOptions.Singleline);

            // 查找所有匹配的表达式
            MatchCollection matches = regex.Matches(fileContent);

            foreach (Match match in matches)
            {
                if (match.Groups.Count > 1)
                {
                    // 提取表达式并添加到列表
                    string expression = match.Groups[1].Value.Trim();
                    expressions.Add(expression);
                }
            }

            return expressions;
        }

        // 调用函数来渲染 HTML 表格
        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        public static void RenderTableToConsole(string html)
        {
            // 解析 HTML
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var table = doc.DocumentNode.SelectSingleNode("//table");
            if (table == null)
            {
                Console.WriteLine("No table found in the HTML.");
                return;
            }

            var rows = table.SelectNodes(".//tr");
            if (rows == null || !rows.Any())
            {
                Console.WriteLine("No rows found in the table.");
                return;
            }

            // 计算每列的最大宽度
            var maxColumnWidths = new List<int>();
            foreach (var row in rows)
            {
                var cells = row.SelectNodes(".//th|.//td");
                if (cells != null)
                {
                    for (int i = 0; i < cells.Count; i++)
                    {
                        int width = cells[i].InnerText.Length;
                        if (i >= maxColumnWidths.Count)
                        {
                            maxColumnWidths.Add(width);
                        }
                        else if (width > maxColumnWidths[i])
                        {
                            maxColumnWidths[i] = width;
                        }
                    }
                }
            }

            // 使用 StringBuilder 生成格式化输出
            var sb = new StringBuilder();

            foreach (var row in rows)
            {
                var cells = row.SelectNodes(".//th|.//td");
                if (cells != null)
                {
                    foreach (var cell in cells)
                    {
                        int index = cells.IndexOf(cell);
                        sb.Append(cell.InnerText.PadRight(maxColumnWidths[index] + 2)); // +2 为分隔符的额外空间
                    }
                    sb.AppendLine();
                }
            }

            // 输出结果到控制台
            Console.WriteLine(sb.ToString());
        }
        public static string ConvertHtmlTableToMarkdown(string htmlTable)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlTable);

            var table = htmlDoc.DocumentNode.SelectSingleNode("//table");
            if (table == null)
            {
                throw new ArgumentException("No table found in the provided HTML.");
            }

            var markdown = new StringBuilder();
            var rows = table.SelectNodes(".//tr");

            if (rows != null && rows.Count > 0)
            {
                // Process the header row
                var headerCells = rows[0].SelectNodes(".//th");
                if (headerCells != null)
                {
                    markdown.Append("| ");
                    foreach (var cell in headerCells)
                    {
                        markdown.Append(cell.InnerText.Trim() + " | ");
                    }
                    markdown.AppendLine();

                    // Process the separator row
                    markdown.Append("|");
                    foreach (var _ in headerCells)
                    {
                        markdown.Append(" --- |");
                    }
                    markdown.AppendLine();
                }

                //====ati add
                int startRowIdx = 1;
                if (headerCells == null)
                {
                    startRowIdx = 0;
                    //no hed 
                }
                // Process the data rows
                for (int i = startRowIdx; i < rows.Count; i++)
                {
                    var dataCells = rows[i].SelectNodes(".//td");
                    if (dataCells != null)
                    {
                        markdown.Append(" | ");
                        foreach (var cell in dataCells)
                        {
                            markdown.Append(cell.InnerText.Trim() + " | ");
                        }
                        markdown.AppendLine();
                    }
                }
            }

            return markdown.ToString();
        }

        public static void PrintSeparator(int[] columnWidths)
        {
            Console.Write("+"); // Start with the opening +
            foreach (var width in columnWidths)
            {
                Console.Write(new string('-', width + 2)); // +2 for padding
                Console.Write("+"); // End each column with +
            }
            Console.WriteLine();
        }
        /// <summary>
        /// //Nustache 渲染包含循环的 Mustache 模板
        ///  {{#mrchts}}  <li>id: {{id  }} </li> { {/ mrchts} }
        /// </summary>
        public static void RenderMstch(string f, object data)
        {
            //$"{prjdir}/webroot/tmpltMstch.htm"
            // 定义模板
            string template = ReadAllText(f);

            //// 定义数据
            //List<Dictionary<string, string>> rws = GetListFrmIniFL("mrcht.ini");
            //rws = SliceX(rws, 0, 3);
            //// 定义数据
            //  data = new
            //{
            //    mrchts = rws
            //};
            // 渲染模板
            var result = Render.StringToString(template, data);

            // 输出结果
            Console.WriteLine(result);
        }

    }
}
