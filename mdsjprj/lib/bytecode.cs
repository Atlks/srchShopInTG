global using static mdsj.lib.bytecode;
using HtmlAgilityPack;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using prj202405.lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static SqlParser.Ast.Statement;

namespace mdsj.lib
{
    internal class bytecode
    {
        public static void WriteObj(string f, object obj)
        {
            System.IO.File.WriteAllText(f, json_encode(obj));
        }
        public static void WriteAllText(string f, string txt)
        {
            System.IO.File.WriteAllText(f, txt);
        }
        public static string ReadAllText(string f)
        {
            return System.IO.File.ReadAllText(f);
        }
        public static List<SortedList> ReadAsListHashtable(string f)
        {
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
            Console.WriteLine(v);
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
                    print_catchEx("foreach_hashtable", e);
                    //   Console.WriteLine(e);
                }
            }

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

        static async Task foreachProcessFilesAsync(string folderPath, Func<string, Task> fileAction)
        {
            if (System.IO.Directory.Exists(folderPath))
            {
                string[] files = System.IO.Directory.GetFiles(folderPath);
                foreach (string file in files)
                {
                    await fileAction(file);
                }
            }
            else
            {
                Console.WriteLine("The specified folder does not exist.");
            }
        }
        public static void foreach_hashtable(Hashtable chtsSess, Func<DictionaryEntry, object> fun)
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
                    print_catchEx("foreach_hashtable", e);
                    //   Console.WriteLine(e);
                }
            }
        }
        public static object getFld(object Obj, string fld, object defVal)
        {

            if (Obj is SortedList)
            {
                return arrCls.ldfld((SortedList)Obj, fld, defVal);
            }
            else
            {
                return ldfld(Obj, fld, defVal);
            }
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
                stfld4447((SortedList)Obj, fld, v);
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
                Console.WriteLine("The object does not have a writable 'Name' property.");
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

        public static async Task<object> callAsync(Func<object> task1)
        {

            try
            {
                return task1();
            }
            catch (Exception e)
            {
                print_ex("call", e);
                return null;
            }


            //   await Task.Run(action);
        }

        public static void print_ex(string mthdName, Exception e)
        {

            Console.WriteLine($"------{mthdName}() catch ex----------_");
            Console.WriteLine(e);
            Console.WriteLine($"------{mthdName}() catch ex finish----------_");
        }

        public static void print_catchEx(string v, Exception e)
        {
            Console.WriteLine($"------{v}() catch ex----------_");
            Console.WriteLine(e);
            Console.WriteLine($"------{v}() catch ex finish----------_");
        }
        public static object call(string authExprs, Delegate callback, params object[] args)
        {

            var __METHOD__ = callback.Method.Name;
            object o = null;
            try
            {
                o = callback.DynamicInvoke(args);
            }
            catch (Exception e)
            {
                print_catchEx(__METHOD__, e);
                SortedList dbgobj = new SortedList();
                dbgobj.Add("mtth", __METHOD__ + "(((" + encodeJsonNofmt(func_get_args(args)) + ")))");
                logErr2024(e, __METHOD__, "errdir", dbgobj);
            }
            return o;
        }

        public static object call(Delegate callback, params object[] args)
        {

            var __METHOD__ = callback.Method.Name;
            object o = null;
            try
            {
                o = callback.DynamicInvoke(args);
            }
            catch (Exception e)
            {
                print_catchEx(__METHOD__, e);
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
                Console.WriteLine("Error reading file: " + ex.Message);
            }

            return words;
        }
        public static void callxTryJmp(Delegate callback, Update update)
        {
            try
            {
                callx(callback, update);

            }
            catch (jmp2endEx e)
            {
                Console.WriteLine("jmp2endEx from meth=>" + callback.Method.Name);
            }
            //catch (Exception e)
            //{
            //    print_catchEx(nameof(MsgHdlrProcess), e);
            //}

        }

        public static object callx(string authExp, Delegate callback, params object[] args)
        {
            return call_user_func(callback, args);
        }
        public static object callx(Delegate callback, params object[] args)
        {
            return call_user_func(callback, args);
        }
        //wz auth
        public static async Task<object> callxAsyncV2(string authExprs, Delegate callback, params object[] args)
        {

            var __METHOD__ = callback.Method.Name;
            print_call_FunArgs(__METHOD__, dbgCls.func_get_args(args));
            object o = null;
            //      try
            // Get the MethodInfo of the delegate
            MethodInfo methodInfo = callback.Method;

            try
            {
                // Check if the method is asynchronous (returns a Task or Task<T>)
                if (typeof(Task).IsAssignableFrom(methodInfo.ReturnType))
                {
                    // Invoke the delegate and get the Task
                    var task = (Task)methodInfo.Invoke(callback.Target, args);
                    await task.ConfigureAwait(false);

                    // If the task has a result (i.e., it's a Task<T>), get the result
                    if (methodInfo.ReturnType.IsGenericType && methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                    {
                        var resultProperty = methodInfo.ReturnType.GetProperty("Result");
                        o = resultProperty.GetValue(task);
                    }
                }
                else
                {
                    // Invoke the delegate synchronously
                    o = methodInfo.Invoke(callback.Target, args);
                }

            }
            catch (jmp2endEx e1)
            {
                throw e1;
            }
            catch (Exception ex)
            {
                print_catchEx(__METHOD__, ex);
                SortedList dbgobj = new SortedList();
                dbgobj.Add("mtth", __METHOD__ + "(((" + json_encode_noFmt(func_get_args(args)) + ")))");
                logErr2024(ex, __METHOD__, "errdir", dbgobj);
            }
            //    call
            if (o != null)
                print_ret(__METHOD__, o);
            else
                print_ret(__METHOD__, 0);
            return o;

        }


        // await callback
        public static async Task<object> callxAsync(Delegate callback, params object[] args)
        {

            var __METHOD__ = callback.Method.Name;
            print_call_FunArgs(__METHOD__, dbgCls.func_get_args(args));
            object o = null;
            //      try
            // Get the MethodInfo of the delegate
            MethodInfo methodInfo = callback.Method;

            try
            {
                // Check if the method is asynchronous (returns a Task or Task<T>)
                if (typeof(Task).IsAssignableFrom(methodInfo.ReturnType))
                {
                    // Invoke the delegate and get the Task
                    var task = (Task)methodInfo.Invoke(callback.Target, args);
                    await task.ConfigureAwait(false);

                    // If the task has a result (i.e., it's a Task<T>), get the result
                    if (methodInfo.ReturnType.IsGenericType && methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                    {
                        var resultProperty = methodInfo.ReturnType.GetProperty("Result");
                        o = resultProperty.GetValue(task);
                    }
                }
                else
                {
                    // Invoke the delegate synchronously
                    o = methodInfo.Invoke(callback.Target, args);
                }

            }
            catch (jmp2endEx e1)
            {
                throw e1;
            }
            catch (Exception ex)
            {
                print_catchEx(__METHOD__, ex);
                SortedList dbgobj = new SortedList();
                dbgobj.Add("mtth", __METHOD__ + "(((" + json_encode_noFmt(func_get_args(args)) + ")))");
                logErr2024(ex, __METHOD__, "errdir", dbgobj);
            }
            //    call
            if (o != null)
                print_ret(__METHOD__, o);
            else
                print_ret(__METHOD__, 0);
            return o;

        }




        public static void jmp2end()
        {
            // jmp2exitFlag = true;
            throw new jmp2endEx();
        }

        public static object callx(string methodName, params object[] args)
        {

            var __METHOD__ = methodName;
            print_call_FunArgs(methodName, dbgCls.func_get_args(args));

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            IEnumerable<Type> typeList = assemblies
                            .SelectMany(assembly => assembly.GetTypes());
            IEnumerable<MethodInfo> methodss = typeList
                            .SelectMany(type => type.GetMethods(BindingFlags.Static | BindingFlags.Public));
            var methodInfo = methodss
                .FirstOrDefault(method =>
                    method.Name == methodName
                  );

            if (methodInfo == null)
            {
                Console.WriteLine("..waring  .methodinfo is null");
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
                print_ex("call", e);
            }


            print_ret_adv(__METHOD__, result);
            return result;
            //Delegate.CreateDelegate(delegateType, methodInfo);
        }

        private static void print_ret_adv(string mETHOD__, object? result)
        {
            //try
            //{

            //}cat
            if (result is System.Collections.IList)
            {
                IList lst = (IList)result;
                try
                {

                    print("lst.size=>" + lst.Count);
                    if (lst.Count > 0)
                        print_ret(mETHOD__, lst[0]);
                    else
                        print_ret(mETHOD__, "list.size=0");
                }
                catch (Exception e)
                {
                    print_ex("print_ret_ex", e);
                    print_ret(mETHOD__, "lst.size=>" + lst.Count);
                }

            }
            else
                print_ret(mETHOD__, result);
        }
    }
}
