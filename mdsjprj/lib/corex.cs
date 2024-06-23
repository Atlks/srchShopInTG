﻿using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Win32;
using NAudio.Wave;

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
        public static string execpath="";

        //D:\0prj\mdsj\WindowsFormsApp1\sqltnode\qry.js
        public  static string call_exe_Pstr(String exec,string scriptPath, string arguments)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), exec, scriptPath, arguments));

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
            string errorOutput="";

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
            dbgCls.setDbgValRtval(__METHOD__, output);
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
        /**
         * // 示例用法
        Action<int, int> add = (a, b) => Console.WriteLine($"Sum: {a + b}");
        object[] args = { 10, 20 };
        
        // 调用委托并传递参数数组
        call_user_func_array(add, args);

        // 通过方法名称调用静态方法并传递参数数组
        call_user_func_array("PrintMessage", args);
         * 
         * 
         * 
         * 
         */
        /// <summary>
        /// 调用指定的委托或方法，并传递参数数组作为参数。
        /// </summary>
        /// <param name="callback">要调用的委托或方法</param>
        /// <param name="args">参数数组</param>
        public static void call_user_func_array(Delegate callback, object[] args)
        {
            callback.DynamicInvoke(args);
        }

        public static void call_user_func(string className, string methodName, object[] parameters)
        {
            try
            {
                // 获取当前程序集中所有的类型
                var type = Assembly.GetExecutingAssembly().GetType(className);
                if (type == null)
                {
                    Console.WriteLine($"找不到类 '{className}'。");
                    return;
                }

                // 获取参数类型数组
                Type[] paramTypes = parameters != null ? Array.ConvertAll(parameters, p => p.GetType()) : Type.EmptyTypes;

                // 获取方法信息，尝试先找静态方法
                var method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, paramTypes, null);

                // 如果没有找到静态方法，再尝试找实例方法
                if (method == null)
                {
                    method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, paramTypes, null);
                }

                if (method == null)
                {
                    Console.WriteLine($"找不到方法 '{methodName}' 或参数不匹配。");
                    return;
                }

                // 如果是静态方法，直接调用
                if (method.IsStatic)
                {
                    method.Invoke(null, parameters);
                }
                else
                {
                    // 如果是实例方法，需要先创建实例
                    var instance = Activator.CreateInstance(type);
                    method.Invoke(instance, parameters);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"调用方法时发生错误：{ex.Message}");
            }
        }

        public static void call_user_func_array(string classAndMethod, object[] parameters)
        {
            // 分割类名和方法名
            var parts = classAndMethod.Split('.');
            if (parts.Length != 2)
            {
                Console.WriteLine("参数格式不正确，请使用 'ClassName.MethodName' 格式。");
                return;
            }

            string className = parts[0];
            string methodName = parts[1];

            try
            {
                // 获取当前程序集中所有的类型
                var type = Assembly.GetExecutingAssembly().GetType(className);
                if (type == null)
                {
                    Console.WriteLine($"找不到类 '{className}'。");
                    return;
                }

                // 获取参数类型数组
                Type[] paramTypes = parameters != null ? Array.ConvertAll(parameters, p => p.GetType()) : Type.EmptyTypes;

                // 获取静态方法信息
                var method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public, null, paramTypes, null);
                if (method == null)
                {
                    Console.WriteLine($"找不到方法 '{methodName}' 或参数不匹配。");
                    return;
                }

                // 调用静态方法
                method.Invoke(null, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"调用方法时发生错误：{ex.Message}");
            }
        }
        /// <summary>
        /// 检查某个类中是否存在指定名称的方法。
        /// </summary>
        /// <param name="type">要检查的类的类型</param>
        /// <param name="methodName">要检查的方法名称</param>
        /// <returns>如果存在具有指定名称的方法，则返回 true；否则返回 false</returns>
        public static bool function_exists(Type type, string methodName)
        {
            // 获取该类型的所有公共方法
            MethodInfo[] methods = type.GetMethods();

            // 检查是否存在指定名称的方法
            foreach (MethodInfo method in methods)
            {
                if (method.Name == methodName)
                {
                    return true;
                }
            }
            return false;
        }
        public  static SortedList ObjectToSortedList(object obj)
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
