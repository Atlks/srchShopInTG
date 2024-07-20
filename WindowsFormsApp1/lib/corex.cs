 using static prjx.lib.corex;
using mdsj.lib;
 
using Microsoft.Win32;
using Mono.Web;
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
using static prjx.lib.corex;
namespace prjx.lib
{
    //prj202405.lib.corex
    internal class corex
    {
        public static void print(object v)
        {
            System.Console.WriteLine(v);
        }

        public static void print(string format, object arg0)
        {
            System.Console.WriteLine(format, arg0);
        }
        public static object call(string methodName,params object[] args)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            IEnumerable<Type> typeList = assemblies
                            .SelectMany(assembly => assembly.GetTypes());
            IEnumerable<MethodInfo> methodss = typeList
                            .SelectMany(type => type.GetMethods(BindingFlags.Static | BindingFlags.Public));
            var methodInfo = methodss
                .FirstOrDefault(method =>
                    method.Name == methodName
                  );

            if (methodInfo == null) return null;

            var delegateType = typeof(Func<string, List<SortedList>>);
            //  var delegateMethod = methodInfo.CreateDelegate(delegateType);

            // 假设你想要执行 YourMethodName 方法
         //   object[] args = { };
            var result = methodInfo.Invoke(null, args);
            return result;
            //Delegate.CreateDelegate(delegateType, methodInfo);
        }
        //dep name
        public static SortedList urlqry2hashtb(string urlqryStr)
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
        /*
         * 
         * 
         *  传入 11000 是 IE11, 9000 是 IE9, 只不过当试着传入 6000 时, 理应是 IE6, 
         *  可实际却是 Edge, 这时进一步测试, 当传入除 IE 现有版本以外的一些数值时 WebBrowser 都使用 Edge 内核
         *  */
        /// <summary>
        /// 修改注册表信息使WebBrowser使用指定版本IE内核
        /// </summary>
        public static void SetFeatures(UInt32 ieMode)
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime)
            {
                throw new ApplicationException();
            }
            //获取程序及名称
            string appName = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            string featureControlRegKey = "HKEY_CURRENT_USER\\Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\";
            //设置浏览器对应用程序(appName)以什么模式(ieMode)运行
            Registry.SetValue(featureControlRegKey + "FEATURE_BROWSER_EMULATION", appName, ieMode, RegistryValueKind.DWord);
            //不晓得设置有什么用
          //  Registry.SetValue(featureControlRegKey + "FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", appName, 1, RegistryValueKind.DWord);
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
        Action<int, int> add = (a, b) =>print($"Sum: {a + b}");
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
