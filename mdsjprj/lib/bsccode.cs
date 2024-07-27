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


        public static bool IsExistFil(string arg)
        {
            return System.IO.File.Exists(arg);
        }

        public static bool Condt(Func<string, bool> fn, string ctry)
        {
            PrintStr(GetMethodName(fn) + $"({ctry})");
            PrintStr(" => ");
            bool r = fn(ctry);
            if (r)
                PrintStr("TRUE✅");
            else
                PrintStr("FALSE❌");
            PrintStr(" ,  ");
            return r;
        }
        public static void iff(bool cdt, Action act1)
        {
            // cd1 cd2 
            PrintStr("\nIF❓❓ is ");
            if (cdt)
            {
                Print("TRUE✅");

            }

            else
                Print("FALSE❌");

            if (cdt)
            {
                Print("THEN➡️➡️");
                act1();

            }

            Print("\nENDIF🔚");
        }
        public static void iff(bool cdt, Action act1, Action elseAct)
        {
            // cd1 cd2 
            PrintStr("\nIF❓❓ is ");
            if (cdt)
                Print("TRUE✅");
            else
                Print("FALSE❌");

            if (cdt)
            {
                Print("THEN➡️➡️");
                act1();

            }
            else
            {
                Print("ELSE☑️");
                elseAct();
            }
            Print("\nENDIF🔚");
        }
        private static string GetMethodName(Delegate del)
        {
            // 使用反射获取方法信息
            var methodInfo = del.Method;
            return methodInfo.Name;
        }

        public static void echo(object v)
        {
            Print(v);
        }
        public static void foreach_objKey(object obj, Func<PropertyInfo, object> fun)
        {
            Print("🔄🔁♻️🔄🔁♻️🔄🔁♻️");
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

        /// <summary>
        /// 循环的 Emoji
        //🔄🔁♻️ 
        //✅  ☑️
        /// </summary>
        /// <param name="list_aftFltr2"></param>
        /// <param name="qrystr"></param>
        /// <returns></returns>
        public static List<SortedList> SliceByPagemodeByQrystr(List<SortedList> list_aftFltr2, string qrystr)
        {
            //  Jmp2end
            SortedList qryMap = GetHashtableFromQrystr(qrystr);
            int page = GetFieldAsInt(qryMap, "page", 0);
            int pagesize = GetFieldAsInt(qryMap, "pagesize", 10);
            int start = (page - 1) * pagesize;
            List<SortedList> list_rzt = SliceX(list_aftFltr2, start, pagesize);

            //------------add col
            Print("🔄🔁♻️🔄🔁♻️🔄🔁♻️");
            foreach (var sortedList in list_rzt)
            {

                SetField938(sortedList, "pages", CalculateTotalPages(pagesize, list_aftFltr2.Count));

            }
            return list_rzt;
        }
        public static SortedList RemoveKeys(SortedList originalDictionary, string commaSeparatedKeys)
        {
            // 分割逗号分割的字符串并移除前后空白
            var keysToRemove = new HashSet<string>(commaSeparatedKeys.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), StringComparer.OrdinalIgnoreCase);

            // 创建新的字典，只保留不在 keysToRemove 中的键值对
            SortedList newDictionary = new SortedList();
            foreach (DictionaryEntry kvp in originalDictionary)
            {
                if (!keysToRemove.Contains(kvp.Key))
                {
                    newDictionary.Add(kvp.Key, kvp.Value);
                }
            }

            return newDictionary;
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
            Print("🔄🔁♻️🔄🔁♻️🔄🔁♻️");
            HashSet<string> updatedSet = new HashSet<string>();

            foreach (string str in originalSet)
            {
                updatedSet.Add(fun(str));
            }

            return updatedSet;
        }
        public static void ForeachHashSet(HashSet<string> originalSet, Action<string> fun)
        {
            Print("🔄🔁♻️🔄🔁♻️🔄🔁♻️");
            HashSet<string> updatedSet = new HashSet<string>();

            foreach (string str in originalSet)
            {
                try
                {
                    fun(str);
                }
                catch (Exception e)
                {
                    PrintCatchEx("foreach_HashSet", e);
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
            Print("🔄🔁♻️🔄🔁♻️🔄🔁♻️");
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
        public static void ForeachHashtableFlgVer(Hashtable chtsSess, Action<DictionaryEntry> fun)
        {
            Print("🔄🔁♻️🔄🔁♻️🔄🔁♻️");
            foreach (DictionaryEntry de in chtsSess)
            {
                //if (Convert.ToInt64(de.Key) == Program.groupId)
                //    continue;
                //  var chatid = Convert.ToInt64(de.Key);
                try
                {
                    //  if(chatid== -1002206103554)
                    fun(de);
                    if (jmp2exitFlagInThrd.Value == true)
                        break;
                }
                catch (jmp2endEx e2)
                {
                    throw e2;
                }
                catch (Exception e)
                {
                    if (e.ToString().Contains("jmp2endEx"))
                    {
                        Jmp2end();
                    }
                    PrintCatchEx("foreach_hashtable", e);
                    //  print(e);
                }
            }
        }


        public static void ForeachHashtable(Hashtable chtsSess, Action<DictionaryEntry> fun)
        {
            Print("🔄🔁♻️🔄🔁♻️🔄🔁♻️");
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
                    if (e.ToString().Contains("jmp2endEx"))
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
            Console.ForegroundColor = ConsoleColor.White;
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
                return Callx(callback, objs);

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
        /// <summary>
        /// 替换字符串
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="placeHold"></param>
        /// <param name="replaceStr"></param>
        /// <returns></returns>
        public static string[] Replace(string[] lines, string placeHold, string replaceStr)
        {
            if (lines == null)
            {
                return new string[0];
            }

            if (placeHold == null)
            {
                // throw new ArgumentNullException(nameof(placeHold));
            }

            if (replaceStr == null)
            {
                //  throw new ArgumentNullException(nameof(replaceStr));
            }

            // 创建一个新数组来存储替换后的结果
            string[] result = new string[lines.Length];

            // 遍历每一行进行替换
            for (int i = 0; i < lines.Length; i++)
            {
                result[i] = lines[i].Replace(placeHold, replaceStr);
            }

            return result;
        }
        public static void AddElemtStrcomma(string noTrigSrchMsgs, HashSet<string> hs11)
        {
            string[] a = noTrigSrchMsgs.Split(",");
            foreach (string s in a)
            {
                if (s.Trim().Length > 0)
                    hs11.Add(s);
            }
        }
        public static string AddElmts(string e, string strComma)
        {
            if (strComma == "")
                return e;
            return strComma + "," + e;
        }
        public static object DelElmts(string e, string strComma)
        {
            ArrayList li = ConvertToArrayList(strComma);
            li.Remove(e);
            return ConvertArrayListToCommaSeparatedString(li);
        }


        public static string removeDulip(string newParks)
        {
            HashSet<string> hs = new HashSet<string>();
            string[] a = newParks.Split(",");
            foreach (string pk in a)
            {
                if (pk.Trim().Length > 0)
                    hs.Add(pk.Trim().ToUpper());
            }
            return JoinWzComma(hs);
        }
        public static string JoinWzComma(HashSet<string> hashSet)
        {
            if (hashSet == null)
            {
                return "";
            }

            // 使用 string.Join 方法将 HashSet 元素连接成逗号分割的字符串
            return string.Join(",", hashSet);
        }
        public static void DelField(SortedList cfg, string key, string ddd)
        {
            if (cfg.ContainsKey(key))
                cfg.Remove(key);
        }
        public static void DelField(SortedList cfg, string key)
        {
            if (cfg.ContainsKey(key))
                cfg.Remove(key);
        }
        public static object Callx(string authExp, Delegate callback, params object[] args)
        {
            return CallUserFunc409(callback, args);
        }
        public static object Callx(Delegate callback, params object[] args)
        {
     
            //  return CallUserFunc409(callback, args);
            MethodInfo method = callback.Method;
            var __METHOD__ = method.Name;
            jmp2endCurFunInThrd.Value = __METHOD__;
            PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(args));
            object o = null;
            try
            {

                //   else
                o = callback.DynamicInvoke(args);
            }
            catch (jmp2endEx e1)
            {
                throw e1;
            }
            catch (Exception e)
            {
                if (e is System.Reflection.TargetInvocationException)
                {
                    if (e.ToString().Contains("jmp2endEx"))
                    {
                        jmp2exitFlagInThrd.Value = true;
                        PrintTimestamp($" Callx(Delegate) ctch ex ,mtth:{__METHOD__}");
                      
                        dbgpad = dbgpad - 4;
                        Jmp2end925(jmp2endCurFunInThrd.Value);
                    }

                }
                Print($"---catch ex----call mtdh:{__METHOD__}  prm:{json_encode_noFmt(func_get_args(args))}");
                Print(e);
                SortedList dbgobj = new SortedList();
                dbgobj.Add("mtth", __METHOD__ + "(((" + json_encode_noFmt(func_get_args(args)) + ")))");
                logErr2024(e, __METHOD__, "errdir", dbgobj);
            }
            //    call
            if (o != null)
                PrintRet(__METHOD__, o);
            else
                PrintRet(__METHOD__, 0);
            return o;
        }


        public static HashSet<string> NewSet(string f)
        {
            try
            {



                var hashSet = new HashSet<string>();
                if (!IsFileExist(f))
                    hashSet = new HashSet<string>();
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
                    SetHashstToFil(hashSet, f);
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
        public static string AddTimet(int secondsToAdd)
        {
            // 获取当前时间
            DateTime currentTime = DateTime.Now;

            // 计算未来时间
            DateTime futureTime = currentTime.AddSeconds(secondsToAdd);

            // 定义时间格式
            string format = "yyyy-MM-dd HH:mm:ss"; // 根据需要调整格式

            // 返回格式化后的时间字符串
            return futureTime.ToString(format);
        }

        public static void IfNotExist(bool bl,Action act)
        {
            iff(bl, act);
        }

        public static void IfExist(bool bl, Action act)
        {
            iff(bl, act);
        }

        public static object newToken(string uid, int exprtTimeSecsAftr)
        {
            string tkExprt = AddTimet(exprtTimeSecsAftr);
            string tkExpEnc = EncryptAes(tkExprt);
            string tkOri = uid + "_" + tkExpEnc;
            return tkOri;
        }

        public static void Jmp2end925(string levFn)
        {
            throw new jmp2endEx("🛑JMP2END from " + levFn + " ,GOTO END🛑🛑🛑.. ");
        }

        public static void Jmp2end()
        {
            // jmp2exitFlag = true;
            throw new jmp2endEx();
        }


        public static object CallxTryx(string methodName, params object[] args)
        {
            //  Print(" fun CallxTryx()" + methodName);
            var __METHOD__ = methodName;
            PrintCallFunArgs(methodName, dbgCls.func_get_args(args));
            jmp2endCurFunInThrd.Value = __METHOD__;
            var methodInfo = GetMethInfo(methodName);

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

            var __METHOD__ = methodName; jmp2endCurFunInThrd.Value = __METHOD__;
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

            }
            catch (jmp2endEx e)
            {
                return e;
            }
            catch (Exception e)
            {
                if (e.ToString().Contains("jmp2endEx"))
                {
                    PrintRetx(__METHOD__, result);
                    PrintTimestamp($"  Callx(str) catch jmp2endEx mthd:{methodName}");
                    Jmp2end();
                }

                PrintExcept("call", e);
            }


            PrintRetx(__METHOD__, result);
            return result;
            //Delegate.CreateDelegate(delegateType, methodInfo);
        }

        public static List<t> Append<t>(List<t> list2, List<t> list1)
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
