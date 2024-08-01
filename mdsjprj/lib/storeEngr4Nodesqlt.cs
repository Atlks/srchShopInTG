global using static libx.storeEngr4Nodesqlt;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static mdsj.lib.exCls;
using static prjx.lib.arrCls;//  prj202405.lib
using static prjx.lib.dbgCls;
using static mdsj.lib.logCls;
using static prjx.lib.corex;
using static prjx.lib.db;
using static prjx.lib.filex;
using static prjx.lib.ormJSonFL;
using static prjx.lib.strCls;
using static mdsj.lib.bscEncdCls;
using static mdsj.lib.net_http;
using static prjx.lib.corex;

using static libx.funCls;
using static libx.storeEngr4Nodesqlt;
using static mdsj.lib.CallFun;
using Newtonsoft.Json;
using System.Diagnostics;
using prjx.lib;
using System.Reflection;

namespace libx
{
    internal class storeEngr4Nodesqlt
    {

        //cmd 模式可能遇到cache size ouput oversize prblm,can use async mode solu
        public static string ExecuteSqlQueryV2(string dbFilePath, string sqlQuery)
        {
            string resultFilePath = "query_result.txt";

            try
            {
                // Create a process to execute the sqlite3 command
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "sqlite3.exe",
                    Arguments = $"\"{dbFilePath}\" \"{sqlQuery}\" > \"{resultFilePath}\"",
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = new Process { StartInfo = processStartInfo })
                {
                    process.Start();

                    // Capture any errors from the process
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        throw new Exception($"Error executing SQLite query: {error}");
                    }

                    // Read and process the result from the file
                    var rows = new List<Dictionary<string, string>>();
                    using (var reader = new StreamReader(resultFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var columns = line.Split('|');
                            var row = new Dictionary<string, string>();
                            for (int i = 0; i < columns.Length; i++)
                            {
                                row[$"column{i}"] = columns[i];
                            }
                            rows.Add(row);
                        }
                    }

                    // Serialize the list of dictionaries to a JSON string
                    return JsonConvert.SerializeObject(rows);
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { error = ex.Message });
            }
            finally
            {
                // Clean up the result file
                if (File.Exists(resultFilePath))
                {
                    File.Delete(resultFilePath);
                }
            }
        }
        public static string ExecuteSqlQuery(string dbFilePath, string sqlQuery)
        {
            try
            {
                // Create a process to execute the sqlite3 command
                string cmdprm = $"\"{dbFilePath}\" \"{sqlQuery}\"";
               Print(cmdprm);
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "d:\\sqlite3.exe",
                    Arguments = cmdprm,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = new Process { StartInfo = processStartInfo })
                {
                    process.Start();

                    // Read the standard output of the command
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        throw new Exception($"Error executing SQLite query: {error}");
                    }

                    // Process the output and convert it to a list of dictionaries
                    var rows = new List<Dictionary<string, string>>();
                    using (var reader = new StringReader(output))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var columns = line.Split('|');
                            var row = new Dictionary<string, string>();
                            for (int i = 0; i < columns.Length; i++)
                            {
                                row[$"column{i}"] = columns[i];
                            }
                            rows.Add(row);
                        }
                    }

                    // Serialize the list of dictionaries to a JSON string
                    return JsonConvert.SerializeObject(rows);
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { error = ex.Message });
            }
        }

        //非事务性存储引擎通常会忽略*old_data参数的内容，仅处理*new_data缓冲
        public static int update_row(SortedList old_data, SortedList new_data, string wrtFile, SortedList dbg)
        {
            return 0;
        }
        
        public static int write_row4nodeSqlt(SortedList row, string wrtFile, SortedList dbg)
        {
            SortedList prm = new SortedList();

            string prjDir = @"../../";
         
         //   prm.Add("fun", scriptPath);

          
            prm.Add("saveobj", row);



          
            prm.Add("dbf", wrtFile);
            prm.Add("dbg", dbg);

            string scriptPath = $"{prjDir}\\sqltnode\\write_row.js";
          
            string str = call_exe_retStr(execpath, scriptPath, prm);
            return int.Parse(str);
        }

        public static List<SortedList> rnd4jsonFl(string dbf)
        {
            if (dbf.EndsWith(".db"))
                return [];

                if (!dbf.EndsWith(".json"))
            {
                string ext = ".json";
                dbf = dbf + ext;
            }

            SortedList prm = new SortedList();

            //   prm.Add("partns", ($"{mrchtDir}\\{partns}"));


            prm.Add("dbf", ($"{dbf}"));
            return read2list(dbf);
        }
        public static List<SortedList> rnd_next4Sqlt(string dbf)
        {
            PrintTimestamp(" start rnd_next4Sqlt()"+dbf);
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(dbf));

            if (!dbf.EndsWith(".db"))
            {
                string ext = ".db";
                dbf = dbf + ext;
            }

            //SortedList prm = new SortedList();

            ////   prm.Add("partns", ($"{mrchtDir}\\{partns}"));


            //prm.Add("dbf", ($"{dbf}"));
            if (!File.Exists(dbf))
            {
                Print("wanring...dbf not exist dbf=>" + dbf);
                PrintRet(__METHOD__, 0);
                return [];
            }
        
            List<SortedList> sortedLists = ormSqlt.qryV2(dbf);
            PrintRet(__METHOD__, "sortedLists.size("+ sortedLists.Count);
            PrintTimestamp(" endfun rnd_next4Sqlt()" + dbf);
            return sortedLists;
        }

       

        //rd 
        public static List<SortedList> rnd_next4nodeSqlt(string dbf)
        {
            if (!dbf.EndsWith(".db"))
            {
                string ext = ".db";
                dbf = dbf + ext;
            }

            SortedList prm = new SortedList();

            //   prm.Add("partns", ($"{mrchtDir}\\{partns}"));


            prm.Add("dbf", ($"{dbf}"));

            string prjDir = @"../../";
            string scriptPath = $"{prjDir}\\sqltnode\\rnd.js";
            string outputDir = $"{prjDir}\\sqltnode\\tmp";
            string txt = call_exec_RetList(execpath,scriptPath, prm, outputDir);
            List<SortedList> li = json_decode(txt);
            return li;
        }

        //  buf参数包含要删除行的内容。对于大多数存储引擎，该参数可被忽略，
        //  但事务性存储引擎可能需要保存删除的数据，以供回滚操作使用。
        internal static int delete_row4nodeSqlt(SortedList buf_row, string dbf)
        {

            SortedList prm = new SortedList();

            string prjDir = @"../../";

            //   prm.Add("fun", scriptPath);


            prm.Add("buf_row", buf_row);

            prm.Add("buf_row_id", buf_row["id"]);


            prm.Add("dbf", dbf);
         //   prm.Add("dbg", dbg);

            string scriptPath = $"{prjDir}\\sqltnode\\delete_row.js";
            string str = call_exe_retStr(execpath, scriptPath, prm);
            return int.Parse(str);
        }
    }
}
