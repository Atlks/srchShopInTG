using DocumentFormat.OpenXml.Vml.Office;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 缅甸商家.lib
{
    internal class ormIni
    {
        public static void save(SortedList sortedList, string Strfile)
        {
            //if (!File.Exists(Strfile))
            //    File.WriteAllText(Strfile, "[]");

            // 使用StreamWriter追加写入文件
            using (StreamWriter writer = new StreamWriter(Strfile, true, Encoding.UTF8))
            {
                writer.WriteLine($"\n\n[{sortedList["id"]}]");
                foreach (DictionaryEntry entry in sortedList)
                {
                    writer.WriteLine($"{entry.Key}={entry.Value}");
                }
            }
        }


     public   static List<Dictionary<string, string>> qry(string iniFilePath)
        {
            var dataList = new List<Dictionary<string, string>>();
            var currentSection = new Dictionary<string, string>();
            var sectionName = string.Empty;

            foreach (var line in File.ReadLines(iniFilePath))
            {
                var trimmedLine = line.Trim();

                // 忽略空行和注释行
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith(";") || trimmedLine.StartsWith("#"))
                {
                    continue;
                }

                // 处理节（section）
                if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                {
                    // 如果当前节非空，添加到列表中
                    if (currentSection.Count > 0)
                    {
                        dataList.Add(currentSection);
                        currentSection = new Dictionary<string, string>();
                    }

                    sectionName = trimmedLine.Substring(1, trimmedLine.Length - 2);
                    currentSection["SectionName"] = sectionName;
                }
                else
                {
                    // 处理键值对
                    var keyValue = trimmedLine.Split(new[] { '=' }, 2);
                    if (keyValue.Length == 2)
                    {
                        var key = keyValue[0].Trim();
                        var value = keyValue[1].Trim();
                        currentSection[key] = value;
                    }
                }
            }

            // 添加最后一个节到列表中
            if (currentSection.Count > 0)
            {
                dataList.Add(currentSection);
            }

            return dataList;
        }

    }
}
