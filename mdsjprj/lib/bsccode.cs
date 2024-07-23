global using static mdsj.lib.bsccode;
using HtmlAgilityPack;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using prjx.lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using static SqlParser.Ast.Expression;
using static SqlParser.Ast.Statement;

namespace mdsj.lib
{
    internal class bsccode
    {

        public static double Avg(List<SortedList> list, string fieldName)
        {
            try
            {
                if (list == null || list.Count == 0)
                {
                    return 0;
                }

                var values = new List<double>();

                foreach (var sortedList in list)
                {
                    if (sortedList.ContainsKey(fieldName) && sortedList[fieldName] is double)
                    {
                        //toNumber( sortedList[fieldName]
                        values.Add(GetFieldAsNumber(sortedList, fieldName));
                    }
                }

                if (values.Count == 0)
                {
                    return 0;
                }

                return values.Average();
            }
            catch (Exception e)
            {
                PrintCatchEx("Avg", e);
                return 0;
            }

        }

    

  
        public static void echo(object v)
        {
            Print(v);
        }
        public static void foreach_objKey(object obj, Func<PropertyInfo, object> fun)
        {
            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;
                object propertyValue = property.GetValue(obj);

                try
                {
                    //  if(chatid== -1002206103554)
                    fun(property);
                }
                catch (Exception e)
                {
                    PrintCatchEx("foreach_hashtable", e);
                    //  print(e);
                }
            }

        }

        public static Dictionary<string, string> RemoveKeys(Dictionary<string, string> originalDictionary, string commaSeparatedKeys)
        {
            // 分割逗号分割的字符串并移除前后空白
            var keysToRemove = new HashSet<string>(commaSeparatedKeys.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), StringComparer.OrdinalIgnoreCase);

            // 创建新的字典，只保留不在 keysToRemove 中的键值对
            var newDictionary = new Dictionary<string, string>(originalDictionary.Count);
            foreach (var kvp in originalDictionary)
            {
                if (!keysToRemove.Contains(kvp.Key))
                {
                    newDictionary.Add(kvp.Key, kvp.Value);
                }
            }

            return newDictionary;
        }
       public static HashSet<string> foreach_HashSet(HashSet<string> originalSet, Func<string, string> fun)
        {
            HashSet<string> updatedSet = new HashSet<string>();

            foreach (string str in originalSet)
            {
                updatedSet.Add(fun(str));
            }

            return updatedSet;
        }
        public static void ForeachHashSet(HashSet<string> originalSet, Action<string> fun)
        {
            HashSet<string> updatedSet = new HashSet<string>();

            foreach (string str in originalSet)
            {
                try
                {
                    fun(str);
                }catch(Exception e)
                {
                    PrintCatchEx("foreach_HashSet",e);
                }
              
            }

            
        }
        static void FforeachProcessFilesAsyncDp(string folderPath, Func<string, Task> fileAction)
        {
            if (System.IO.Directory.Exists(folderPath))
            {
                string[] files = System.IO.Directory.GetFiles(folderPath);
                foreach (string file in files)
                {

                    fileAction(file);
                }
            }
            else
            {
                Print("The specified folder does not exist.");
            }
        }
        public static void ForeachHashtable(Hashtable chtsSess, Func<DictionaryEntry, object> fun)
        {
            foreach (DictionaryEntry de in chtsSess)
            {
                //if (Convert.ToInt64(de.Key) == Program.groupId)
                //    continue;
                //  var chatid = Convert.ToInt64(de.Key);
                try
                {
                    //  if(chatid== -1002206103554)
                    fun(de);
                }
                catch (Exception e)
                {
                    PrintCatchEx("foreach_hashtable", e);
                    //  print(e);
                }
            }
        }
        public static void ForeachHashtable(Hashtable chtsSess, Action<DictionaryEntry> fun)
        {
            foreach (DictionaryEntry de in chtsSess)
            {
                //if (Convert.ToInt64(de.Key) == Program.groupId)
                //    continue;
                //  var chatid = Convert.ToInt64(de.Key);
                try
                {
                    //  if(chatid== -1002206103554)
                    fun(de);
                }
                catch (jmp2endEx e2)
                {
                    throw e2;
                }               
                catch (Exception e)
                {
                    if(e.ToString().Contains("jmp2endEx"))
                    {
                        Jmp2end();
                    }
                    PrintCatchEx("foreach_hashtable", e);
                    //  print(e);
                }
            }
        }

        public static bool And(bool left, bool right)
        {
            return left && right;
        }
        public static bool Or(bool left, bool right)
        {
            return left && right;
        }
       public static object Invoke(string methodName, params object[] args)
        {
            return Callx(methodName, args);
        }

      

        /// <summary>
        ///     PrintColoredText("This is blue text.", ConsoleColor.Blue);
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public static void PrintColoredText(string text, ConsoleColor color)
        {
            // 输出一个笑脸
            Console.OutputEncoding = Encoding.UTF8;
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = originalColor;
        }
        public static void TaskRunNewThrd(Action act1)
        {
            // 使用 Task.Run 启动一个新的任务
            System.Threading.Tasks.Task newTask = System.Threading.Tasks.Task.Run(() =>
            {
                // callxTryJmp(task1);
                try
                {
                    act1();
                }
                catch (Exception e)
                {
                    PrintCatchEx("TaskRunNewThrd", e);
                    logErr2025(e, nameof(TaskRunNewThrd), "errlog");
                }


                //  callxTryJmp(OnMsg, update, reqThreadId);

            });
        }
        public static void TaskRun(Func<System.Threading.Tasks.Task> value)
        {
            callAsyncNewThrdx(value);
        }
        public static void CallAsyncNewThrd(Action task1)
        {
            // 使用 Task.Run 启动一个新的任务
            System.Threading.Tasks.Task newTask = System.Threading.Tasks.Task.Run(() =>
            {
                // callxTryJmp(task1);
                try
                {
                    task1();
                }
                catch (Exception e)
                {
                    PrintCatchEx("callAsync", e);
                    logErr2025(e, nameof(callAsyncNewThrdx), "errlog");
                }


                //  callxTryJmp(OnMsg, update, reqThreadId);

            });


            //   await Task.Run(action);
        }

        public static void callAsyncNewThrdx(Func<object> task1)
        {
            // 使用 Task.Run 启动一个新的任务
            System.Threading.Tasks.Task newTask = System.Threading.Tasks.Task.Run(() =>
            {
                // callxTryJmp(task1);
                try
                {
                    task1();
                }
                catch (Exception e)
                {
                    PrintCatchEx("callAsync", e);
                }


                //  callxTryJmp(OnMsg, update, reqThreadId);

            });


            //   await Task.Run(action);
        }

        public static void PrintExcept(string mthdName, Exception e)
        {

            Print($"------{mthdName}() catch ex----------_");
            Print(e);
            Print($"------{mthdName}() catch ex finish----------_");
        }

        public static void PrintCatchEx(string v, Exception e)
        {
            Print($"------{v}() catch ex----------_");
            Print(e);
            Print($"------end {v}() catch ex finish----------_");
        }
        public static object Call(string authExprs, Delegate callback, params object[] args)
        {

            var __METHOD__ = callback.Method.Name;
            object o = null;
            try
            {
                o = callback.DynamicInvoke(args);
            }
            catch (Exception e)
            {
                PrintCatchEx(__METHOD__, e);
                SortedList dbgobj = new SortedList();
                dbgobj.Add("mtth", __METHOD__ + "(((" + encodeJsonNofmt(func_get_args(args)) + ")))");
                logErr2024(e, __METHOD__, "errdir", dbgobj);
            }
            return o;
        }

        /*
         
         .子程序 call, 公开, 通用型
.参数 callback, 句柄型
.参数 args, 数据组
         */
        public static object Call(Delegate callback, params object[] args)
        {

            var __METHOD__ = callback.Method.Name;
            object o = null;
            try
            {
                o = callback.DynamicInvoke(args);
            }
            catch (Exception e)
            {
                PrintCatchEx(__METHOD__, e);
                SortedList dbgobj = new SortedList();
                dbgobj.Add("mtth", __METHOD__ + "(((" + encodeJsonNofmt(func_get_args(args)) + ")))");
                logErr2024(e, __METHOD__, "errdir", dbgobj);
            }
            return o;
        }

        public static object Callx(bool authExpRzt, Delegate callback, params object[] args)
        {
            return CallUserFunc409(callback, args);
        }

       public static object CallxTryJmp(Delegate callback, params object[] objs)
        {
            try
            {
                return callx(callback, objs);

            }
            catch (jmp2endEx e)
            {
                PrintCatchEx("callxTryJmp", e);
                Print("callxTryJmp  callmeth=>" + callback.Method.Name);
            }
            //catch (Exception e)
            //{
            //    print_catchEx(nameof(MsgHdlrProcess), e);
            //}
            return 0;
        }

        public static object Callx(string authExp, Delegate callback, params object[] args)
        {
            return CallUserFunc409(callback, args);
        }
        public static object callx(Delegate callback, params object[] args)
        {
            return CallUserFunc409(callback, args);
        }
       

        public static HashSet<string> NewSet(string f)
        {
            try
            {
                
               

                var hashSet = new HashSet<string>();
                if (!isFileExist(f))
                    hashSet= new HashSet<string>();
                else
                {
                    string json = File.ReadAllText(f);
                     hashSet = JsonConvert.DeserializeObject<HashSet<string>>(json);


                }
                //定时持久化downedUrl
                // setHsstToF(rmvs3, "rmvs3.json");
                // 创建一个定时器，每2秒触发一次
                System.Timers.Timer timer = new System.Timers.Timer(2000);
                timer.Elapsed += (sender, e) =>
                {
                    setHsstToF(hashSet, f);
                };
                timer.AutoReset = true;
                timer.Enabled = true;
                timer.Start();


                return hashSet;
            }
            catch (Exception ex)
            {
                ConsoleWriteLine($"An error occurred: {ex.Message}");
                return new HashSet<string>();
            }
          
        }

    


        public static void callTryAll(Action value)
        {
            try
            {
                value();
            }
            catch (Exception e)
            {
                PrintCatchEx("callTryAll", e);
            }

        }
    
   
     
        
        public static void Jmp2end()
        {
            // jmp2exitFlag = true;
            throw new jmp2endEx();
        }

 
        public static object CallxTryx(string methodName, params object[] args)
        {

            var __METHOD__ = methodName;
            PrintCallFunArgs(methodName, dbgCls.func_get_args(args));

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Print("assemblies.Len=>" + assemblies.Length);
            IEnumerable<Type> typeList = assemblies
                            .SelectMany(assembly => assembly.GetTypes());
            Print("typeList.Len=>" + typeList.Count());
            IEnumerable<MethodInfo> methodss = typeList
                            .SelectMany(type => type.GetMethods());  //BindingFlags.Static| BindingFlags.Public
            Print("methodss.Len=>" + methodss.Count());
            var methodInfo = methodss
                .FirstOrDefault(method =>
                    method.Name == methodName
                  );

            if (methodInfo == null)
            {
                Print("......$$waring  .methodinfo is null");
                PrintRetx(__METHOD__, "");
                return null;
            }


            var delegateType = typeof(Func<string, List<SortedList>>);
            //  var delegateMethod = methodInfo.CreateDelegate(delegateType);

            // 假设你想要执行 YourMethodName 方法
            //   object[] args = { };
            object result = null;
            try
            {
                result = methodInfo.Invoke(null, args);

            }
            catch (Exception e)
            {
                PrintExcept(nameof(CallxTryx), e);
            }


            PrintRetx(__METHOD__, result);
            return result;
            //Delegate.CreateDelegate(delegateType, methodInfo);
        }
    
      
        public static object Callx(string methodName, params object[] args)
        {

            var __METHOD__ = methodName;
            PrintCallFunArgs(methodName, func_get_args(args));
            var argsMkdFmt = ConvertToMarkdownTable(args);
            Print(argsMkdFmt);

            MethodInfo? methodInfo = GetMethInfo(methodName);

            if (methodInfo == null)
            {
                Print("......$$waring  .methodinfo is null");
                PrintRetx(__METHOD__, "");
                return null;
            }


      //      var delegateType = typeof(Func<string, List<SortedList>>);
            //  var delegateMethod = methodInfo.CreateDelegate(delegateType);

            // 假设你想要执行 YourMethodName 方法
            //   object[] args = { };
            object result = null;
            try
            {
                result = methodInfo.Invoke(null, args);

            }catch(jmp2endEx e)
            {
                return e;
            }
            catch (Exception e)
            {
                if (e.ToString().Contains("jmp2endEx"))
                {
                    PrintRetx(__METHOD__, result);
                    Jmp2end();
                }
                  
                PrintExcept("call", e);
            }


            PrintRetx(__METHOD__, result);
            return result;
            //Delegate.CreateDelegate(delegateType, methodInfo);
        }

        public static List<t> Append<t>(List<t> list2, List<t>   list1)
        {
            //List<t> result = new List<t>();

            //// 获取最长列表的长度
            //int maxLength = Math.Max(list1.Count, list2.Count);

            //// 遍历并合并列表
            ////for (int i = 0; i < maxLength; i++)
            ////{
            //for (int i = 0; i < list1.Count; i++)
            //{
            //    result.Add(list1[i]);
            //}

            for (int i = 0; i < list2.Count; i++)
            {
                list1.Add(list2[i]);
            }
            //}

            return list1;
        }


        public static void CallAsAsyncTaskRun(Action act)
        {
            try
            {
                act();
            }
            catch (Exception e)
            {
                PrintCatchEx(nameof(CallAsAsyncTaskRun), e);
            }
        }
        public static void PrintRetx(string mETHOD__, object? result)
        {
            //try
            //{

            //}cat
            if (result is System.Collections.IList)
            {
                IList lst = (IList)result;
                try
                {

                    Print("lst.size=>" + lst.Count);
                    if (lst.Count > 0)
                        PrintRet(mETHOD__, lst[0]);
                    else
                        PrintRet(mETHOD__, "list.size=0");
                }
                catch (Exception e)
                {
                    PrintExcept("print_ret_ex", e);
                    PrintRet(mETHOD__, "lst.size=>" + lst.Count);
                }

            }
            else
                PrintRet(mETHOD__, result);
        }
    }
}
