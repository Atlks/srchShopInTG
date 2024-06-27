using mdsj.lib;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Win32;
using NAudio.Wave;
using Nethereum.Model;


//using Mono.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static prj202405.lib.corex;
namespace prj202405.lib
{

    //prj202405.lib.corex
    internal class corex
    {
        public static void ExecuteAfterDelay(int millisecondsDelay, Action action)
        {
            Task.Delay(millisecondsDelay).ContinueWith(_ => action());
        }

        public static object 运行(string 代码)
        {
            try
            {
                var 结果 = CSharpScript.EvaluateAsync(代码, ScriptOptions.Default).Result;
                return 结果;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static object Eval(string code)
        {
            try
            {
                var result = CSharpScript.EvaluateAsync(code, ScriptOptions.Default).Result;
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static SortedList parse_str_qrystr(string urlqryStr)
        {

            // 解析查询字符串为字典
            NameValueCollection queryString = HttpUtility.ParseQueryString(urlqryStr);
            var QueryHashtb = new System.Collections.Generic.Dictionary<string, string>();

            // 将解析结果存入字典
            foreach (string key in queryString.AllKeys)
            {
                QueryHashtb.Add(key, queryString[key]);
            }



            // 创建一个 SortedList 并初始化大小
            SortedList sortedList = new SortedList(QueryHashtb.Count);

            // 将 Dictionary 中的项添加到 SortedList 中
            foreach (var pair in QueryHashtb)
            {
                sortedList.Add(pair.Key, pair.Value.ToString());
            }

            return sortedList;
        }
        public static bool IsString(object input)
        {
            return input is string;
        }

        public static string soluPath = "";
        public static string execpath = "";

        //D:\0prj\mdsj\WindowsFormsApp1\sqltnode\qry.js
        public static string call_exe_Pstr(String exec, string scriptPath, string arguments)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.dbg_setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), exec, scriptPath, arguments));

            // Create a new process to run the Node.js script
            Process process = new Process();
            process.StartInfo.FileName = exec;
            process.StartInfo.Arguments = $"\"{scriptPath}\" \"{arguments}\"";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.StandardOutputEncoding = Encoding.UTF8; // 设置标准输出编码

            process.StartInfo.StandardErrorEncoding = Encoding.UTF8;  // 设置标准错误输出编码
            // Capture the output from the process
            string output;
            string errorOutput = "";

            try
            {
                process.Start();

                // Read the standard output and error output streams
                using (StreamReader outputReader = process.StandardOutput)
                {
                    output = outputReader.ReadToEnd();
                }
                using (StreamReader errorReader = process.StandardError)
                {
                    errorOutput = errorReader.ReadToEnd();
                }

                process.WaitForExit();
            }
            catch (Exception ex)
            {
                output = $"An error occurred while executing the Node.js script: {ex.Message}";
            }

            // If there is any error output, append it to the main output
            if (!string.IsNullOrEmpty(errorOutput))
            {
                output += Environment.NewLine + "Error output: " + errorOutput;
            }
            dbgCls.dbg_setDbgValRtval(__METHOD__, output);
            return output;
        }


        public static SortedList DictionaryToSortedList<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            // 创建一个 SortedList
            SortedList sortedList = new SortedList();

            // 将 Dictionary 中的键值对添加到 SortedList 中
            foreach (KeyValuePair<TKey, TValue> pair in dictionary.OrderBy(p => p.Key))
            {
                sortedList.Add(pair.Key.ToString(), pair.Value);
            }

            return sortedList;
        }
         public static SortedList ObjectToSortedList(object obj)
        {
            SortedList sortedList = new SortedList();

            // 获取对象的所有属性
            PropertyInfo[] properties = obj.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                // 获取属性名和值
                string propertyName = property.Name;
                object propertyValue = property.GetValue(obj);

                // 将属性名和值添加到SortedList
                sortedList.Add(propertyName, propertyValue);
            }

            return sortedList;
        }
    }
}
