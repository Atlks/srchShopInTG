global using static mdsj.lib.bsccode;
using HtmlAgilityPack;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Newtonsoft.Json;
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
        public static void WriteObj(string f, object obj)
        {

             WriteAllText(f, json_encode(obj));
        }
        public static void WriteAllText(string f, string txt)
        {
            Print($" fun WriteAllText {f}");
            Mkdir4File(f);
            try
            {
                System.IO.File.WriteAllText(f, txt);
            }catch(Exception e)
            {
                PrintCatchEx("WriteAllText", e);
            }
           
        }

      
        public static string ReadAllText(string f)
        {
            return System.IO.File.ReadAllText(f);
        }
        public static List<SortedList> ReadAsListHashtable(string f)
        {
            //   File
            return json_decode(System.IO.File.ReadAllText(f));
        }
        public static SortedList ldHstb
            (string f)
        {
            return json_decode<SortedList>(System.IO.File.ReadAllText(f));
        }
        public static SortedList LoadHashtable(string f)
        {
            return json_decode<SortedList>(System.IO.File.ReadAllText(f));
        }
        public static SortedList ReadAsHashtable(string f)
        {
            return json_decode<SortedList>(System.IO.File.ReadAllText(f));
        }
        public static object ReadAsObj(string f)
        {
            return json_decodeObj(System.IO.File.ReadAllText(f));
        }
        public static JsonObject ReadAsJson(string f)
        {
            return json_decodeJonObj(System.IO.File.ReadAllText(f));
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
        public static Dictionary<string, string> ldDicFromQrystr(string queryString)
        {
            return ConvertToDictionary(queryString);
        }
        public static Dictionary<string, string> ConvertToDictionary(string queryString)
        {
            var dictionary = new Dictionary<string, string>();

            // Use HttpUtility to parse the query string
            var queryParams = HttpUtility.ParseQueryString(queryString);

            foreach (string key in queryParams)
            {
                dictionary[key] = queryParams[key];
            }

            return dictionary;
        }
        public static Hashtable LoadHashtableFromQrystrDep(string queryString)
        {
            var hashtable = new Hashtable();

            // Use HttpUtility to parse the query string
            NameValueCollection queryParams = HttpUtility.ParseQueryString(queryString);

            foreach (string key in queryParams)
            {
                hashtable[key] = queryParams[key];
            }

            return hashtable;
        }
        public static Hashtable ConvertToHashtable(string queryString)
        {
            var hashtable = new Hashtable();

            // Use HttpUtility to parse the query string
            NameValueCollection queryParams = HttpUtility.ParseQueryString(queryString);

            foreach (string key in queryParams)
            {
                hashtable[key] = queryParams[key];
            }

            return hashtable;
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

        public static object GetField(object Obj, string fld, object defVal)
        {

            if (Obj is SortedList)
            {
                return arrCls.LoadField((SortedList)Obj, fld, defVal);
            }
            else
            {
                return ldfld(Obj, fld, defVal);
            }
        }
        public static object getFld(object Obj, string fld, object defVal)
        {

            if (Obj is SortedList)
            {
                return arrCls.LoadField((SortedList)Obj, fld, defVal);
            }
            else
            {
                return ldfld(Obj, fld, defVal);
            }
        }
        public static string ldfldAsStr(object obj, string fld, object defVal)
        {
            return ldfld(obj, fld, "").ToString();
        }

        public static object LoadField(Hashtable hstb, string fld, object defVal)
        {
            if (hstb.ContainsKey(fld))
                return hstb[fld];
            return defVal;
        }
        public static object ldfld(object obj, string fld, object defVal)
        {
            Type type = obj.GetType();
            PropertyInfo propertyInfo = type.GetProperty(fld);

            if (propertyInfo != null && propertyInfo.CanWrite)
            {
                return propertyInfo.GetValue(obj);
            }
            return defVal;
        }

        public static bool And(bool left, bool right)
        {
            return left && right;
        }
        public static bool Or(bool left, bool right)
        {
            return left && right;
        }
        public static void setFld(object Obj, string fld, object v)
        {
            if (Obj is SortedList)
            {
                SetField938((SortedList)Obj, fld, v);
            }
            else
            {
                SetProperty(Obj, fld, v);
            }

        }


        static void SetProperty(object obj, string prop, object v)
        {
            Type type = obj.GetType();
            PropertyInfo propertyInfo = type.GetProperty(prop);

            if (propertyInfo != null && propertyInfo.CanWrite)
            {
                propertyInfo.SetValue(obj, v);
            }
            else
            {
                Print("The object does not have a writable 'Name' property.");
            }
        }
        public static object invoke(string methodName, params object[] args)
        {
            return callx(methodName, args);
        }

        public static string castToStr(object args)
        {
            return args.ToString();
        }

        /// <summary>
        ///     PrintColoredText("This is blue text.", ConsoleColor.Blue);
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public static void PrintColoredText(string text, ConsoleColor color)
        {
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

        public static object callx(bool authExpRzt, Delegate callback, params object[] args)
        {
            return call_user_func(callback, args);
        }

        public static HashSet<string> LdHsstWordsFromFile(string filePath)
        {
            var words = new HashSet<string>();

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // 拆分行中的单词，按空格和回车拆分
                        var splitWords = line.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var word in splitWords)
                        {
                            var word1 = word.Trim();
                            words.Add(word1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Print("Error reading file: " + ex.Message);
            }

            return words;
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

        public static object callx(string authExp, Delegate callback, params object[] args)
        {
            return call_user_func(callback, args);
        }
        public static object callx(Delegate callback, params object[] args)
        {
            return call_user_func(callback, args);
        }
        public static void setHsstToF(HashSet<string> downedUrl, string v)
        {
            WriteAllText(v, encodeJson( downedUrl));
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

        public static HashSet<string> LdHsstFrmFJsonDecd(string v)
        {
            return (ReadFileToHashSet(v));
        }
        public static HashSet<string> ReadFileToHashSet(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                HashSet<string> hashSet = JsonConvert.DeserializeObject<HashSet<string>>(json);
                return hashSet;
            }
            catch (Exception ex)
            {
                ConsoleWriteLine($"An error occurred: {ex.Message}");
                return new HashSet<string>();
            }
        }

        public static HashSet<string> LoadHashsetReadFileLinesToHashSet(string filePath)
        {
            HashSet<string> lines = new HashSet<string>();

            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if(line.Trim().Length>0)
                        lines.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"The file could not be read: {e.Message}");
            }

            return lines;
        }
        public static HashSet<string> LdHsst(string input)
        {
            // 分割字符串并转换为 HashSet
            string[] elements = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            HashSet<string> stringSet = new HashSet<string>(elements);

            return stringSet;
        }
        public static HashSet<string> LoadHashsetFrmFL(string f)
        {
            return LdHsst(ReadAllText(f));
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
        public static bool IsStr(object obj1)
        {

            return obj1 is string;
        }
        public static bool IsInt(string str)
        {
            return int.TryParse(str, out _);
        }
        public static bool IsNumeric(object str)
        {
            var s = ToString(str);
            // 匹配整数或带小数点的数字
            return Regex.IsMatch(s, @"^[0-9]+(\.[0-9]+)?$");
        }
        public static bool IsNumeric(string str)
        {
            // 匹配整数或带小数点的数字
            return Regex.IsMatch(str, @"^[0-9]+(\.[0-9]+)?$");
        }
        public static string ToString(object o)
        {
            if (o == null)
                return "";
            // The default for an object is to return the fully qualified name of the class.
            return o.ToString();
        }
     
        
        public static void Jmp2end()
        {
            // jmp2exitFlag = true;
            throw new jmp2endEx();
        }

        public static bool IsArray(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Type type = obj.GetType();

            // 检查类型是否为数组
            return type.IsArray;
        }
        public static object callxTryx(string methodName, params object[] args)
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
                PrintExcept(nameof(callxTryx), e);
            }


            PrintRetx(__METHOD__, result);
            return result;
            //Delegate.CreateDelegate(delegateType, methodInfo);
        }
        public static bool IsCollection(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Type type = obj.GetType();

            // 检查类型是否实现了 IEnumerable 接口
            return typeof(IEnumerable).IsAssignableFrom(type);
        }


        public static string ConvertToMarkdownTable(object arr2)
        {
            object[] arr = (object[])arr2;
            StringBuilder sb = new StringBuilder();

            // 添加表头
            sb.AppendLine("| prm\t|Value\t|");
            sb.AppendLine("|-------|-------|");

            // 添加表格行
            for (int i = 0; i < arr.Length; i++)
            {
                object obj = arr[i];
                sb.AppendLine($"| {i}\t|{encodeJson(obj)}\t|");
            }

            return sb.ToString();
        }

        public static object callx(string methodName, params object[] args)
        {

            var __METHOD__ = methodName;
            PrintCallFunArgs(methodName, func_get_args(args));
            var argsMkdFmt = ConvertToMarkdownTable(args);
            Print(argsMkdFmt);

            MethodInfo? methodInfo = getMethInfo(methodName);

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

        private static MethodInfo? getMethInfo(string methodName)
        {
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
            return methodInfo;
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
