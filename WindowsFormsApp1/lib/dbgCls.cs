 
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
using static mdsj.lib.encdCls;
namespace prjx.lib
{
    internal class dbgCls
    {

        /// <summary>
        /// 打印对象的类型和值，类似于 PHP 的 var_dump 函数。
        /// </summary>
        /// <param name="obj">要打印的对象</param>
        /// <param name="indentLevel">缩进级别，用于递归调用时控制缩进</param>
        public static void var_dump(object obj, int indentLevel = 0)
        {
            string indent = new string(' ', indentLevel * 4);

            if (obj == null)
            {
               print($"{indent}null");
            }
            else
            {
                Type type = obj.GetType();
               print($"{indent}{type}({GetObjectSize(obj)}) {obj}");
                indentLevel++;

                if (obj is IDictionary dictionary)
                {
                    foreach (DictionaryEntry entry in dictionary)
                    {
                        Console.Write($"{indent}  [{entry.Key}] => ");
                        var_dump(entry.Value, indentLevel + 1);
                    }
                }
                else if (obj is IEnumerable enumerable && !(obj is string))
                {
                    foreach (var item in enumerable)
                    {
                        var_dump(item, indentLevel);
                    }
                }
                else
                {
                    if (!type.IsPrimitive && !(obj is string))
                    {
                        foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                        {
                            object value = property.GetValue(obj, null);
                           print($"{indent}{property.Name}:");
                            var_dump(value, indentLevel + 1);
                        }

                        foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
                        {
                            object value = field.GetValue(obj);
                           print($"{indent}{field.Name}:");
                            var_dump(value, indentLevel + 1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取对象的近似大小。
        /// </summary>
        /// <param name="obj">要计算大小的对象</param>
        ///// <returns>对象的近似大小</returns>
        //private static long GetObjectSize(object obj)
        //{
        //    if (obj == null)
        //    {
        //        return 0;
        //    }

        //    using (var ms = new System.IO.MemoryStream())
        //    {
        //        var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        //        formatter.Serialize(ms, obj);
        //        return ms.Length;
        //    }
        //}


        /// <summary>
        /// 估算对象的近似大小。
        /// </summary>
        /// <param name="obj">要计算大小的对象</param>
        /// <returns>对象的近似大小（字节）</returns>
        private static long GetObjectSize(object obj)
        {
            if (obj == null)
            {
                return 0;
            }

            Type type = obj.GetType();
            if (type.IsPrimitive || obj is string)
            {
                return GetPrimitiveSize(obj);
            }

            long size = 0;
            if (obj is IDictionary dictionary)
            {
                foreach (DictionaryEntry entry in dictionary)
                {
                    size += GetObjectSize(entry.Key);
                    size += GetObjectSize(entry.Value);
                }
            }
            else if (obj is IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    size += GetObjectSize(item);
                }
            }
            else
            {
                foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    size += GetObjectSize(property.GetValue(obj, null));
                }

                foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
                {
                    size += GetObjectSize(field.GetValue(obj));
                }
            }

            return size;
        }

        /// <summary>
        /// 获取基本类型和字符串的大小。
        /// </summary>
        /// <param name="obj">基本类型或字符串对象</param>
        /// <returns>对象的大小（字节）</returns>
        private static long GetPrimitiveSize(object obj)
        {
            if (obj is bool)
                return sizeof(bool);
            if (obj is byte)
                return sizeof(byte);
            if (obj is sbyte)
                return sizeof(sbyte);
            if (obj is char)
                return sizeof(char);
            if (obj is decimal)
                return sizeof(decimal);
            if (obj is double)
                return sizeof(double);
            if (obj is float)
                return sizeof(float);
            if (obj is int)
                return sizeof(int);
            if (obj is uint)
                return sizeof(uint);
            if (obj is long)
                return sizeof(long);
            if (obj is ulong)
                return sizeof(ulong);
            if (obj is short)
                return sizeof(short);
            if (obj is ushort)
                return sizeof(ushort);
            if (obj is string str)
                return sizeof(char) * str.Length;

            return 0;
        }


        /// <summary>
        /// 打印对象的结构和内容，类似于 PHP 的 print_r 函数。
        /// </summary>
        /// <param name="obj">要打印的对象</param>
        /// <param name="indentLevel">缩进级别，用于递归调用时控制缩进</param>
        public static void print_r(object obj, int indentLevel = 0)
        {
            try
            {
                string indent = new string(' ', indentLevel * 4);

                if (obj == null)
                {
                   print($"{indent}null");
                }
                else if (obj is IDictionary dictionary)
                {
                   print($"{indent}Dictionary:");
                    foreach (DictionaryEntry entry in dictionary)
                    {
                        Console.Write($"{indent}  [{entry.Key}] => ");
                        print_r(entry.Value, indentLevel + 1);
                    }
                }
                else if (obj is IEnumerable enumerable && !(obj is string))
                {
                   print($"{indent}List:");
                    foreach (var item in enumerable)
                    {
                        print_r(item, indentLevel + 1);
                    }
                }
                else
                {
                   print($"{indent}{obj}");
                }
            }catch(Exception e)
            {
               print(e);
            }
           
        }

        /**
         * 
         * 
         * 在 C# 中，我们可以编写一个类似于 PHP 中的 error_reporting 函数来控制错误报告级别。
         * 尽管 C# 中没有与 PHP 中的错误报告级别完全相同的概念，
         * 但我们可以利用 C# 的异常处理机制来实现类似的功能。下面是一个示例：
         * 
         */
        /// <summary>
        /// 设置错误报告级别。0 表示禁用错误报告，其他值表示启用。
        /// </summary>
        /// <param name="level">错误报告级别</param>
        public static void error_reporting(int level)
        {
            if (level == 0)
            {
                AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                {
                    // 禁用错误报告时，什么也不做
                };
            }
            else
            {
                AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                {
                    Exception e = (Exception)args.ExceptionObject;
                   print($"An error occurred: {e.Message}");
                };
            }
        }



        public static int dbgpad = 0;


        /*
         * @param string $METHOD__
         * @return void
         */
        public static void setDbgFunEnter(string METHOD__, object func_get_args)
        {
            //if($GLOBALS['dbg_show']==false)
            //    return;
            //  $GLOBALS['dbgpad']=$GLOBALS['dbgpad']+4;
            dbgpad = dbgpad + 4;
            var msglog = str_repeat(" ", dbgpad) + " FUN " + METHOD__ + "((" + JsonConvert.SerializeObject(func_get_args) + "))";
            // array_push($GLOBALS['dbg'],$logmsg   );
           print("\n\n\n" + msglog + "");
        }

        public static void setDbgVal(string METHOD__, string vname, string val)
        {
            //if($GLOBALS['dbg_show']==false)
            //    return;
            var msglog = str_repeat(" ", dbgpad + 3) + "" + METHOD__ + $"():: {vname}=>{val}";
            // array_push($GLOBALS['dbg'],        $msg);
           print(msglog + "");

        }

        public static void setDbgValRtval(object mETHOD__, object results)
        {
            //string jsonString = JsonConvert.SerializeObject(results, Formatting.Indented);
            //Console.WriteLine(jsonString);

            //    if ($GLOBALS['dbg_show'] == false)
            //return;
            // ENDFUN
            var msglog = str_repeat(" ", dbgpad) + " ENDFUN " + mETHOD__ + "():: ret=>" + json_encode_noFmt(results);
           print(msglog + "");
            //    array_push($GLOBALS['dbg'], $msglog);
            dbgpad = dbgpad - 4;
        }

        //public static object array_slice(object arr_rzt, int v1, int v2)
        //{
        //    return arr_rzt;
        //}


        /// <summary>
        /// 类似于 PHP 中的 func_get_args 函数，输出传递给函数的所有参数。
        /// </summary>
        /// <param name="args">参数数组</param>
        public static object func_get_args(params object[] args)
        {
            // 输出每个参数的值
           print("Arguments:");
            //foreach (object arg in args)
            //{
            //   print(arg);
            //}
            return args;
        }
        /**
         * 
         * 在C#中，由于其类型系统的不同，无法直接实现类似于PHP的func_get_args函数，因为C#的方法参数必须在编译时指定类型。
         * 但是，我们可以使用可变参数（params）来实现一个类似的功能。以
         * 
         */
        public static object func_get_args(MethodBase method, params object[] paramValues)
        {
            // 获取当前方法
            // MethodBase method = new StackFrame(1).GetMethod();

            // 获取当前方法的参数
            ParameterInfo[] parameters = method.GetParameters();


            // 检查参数数量是否匹配
            if (parameters.Length != paramValues.Length)
            {
              //  throw new ArgumentException("Parameter count does not match.");
            }

            // 获取当前方法的参数值
            // 将参数名称和值配对
            var parameterValues = parameters.Select<ParameterInfo, object>(
                (ParameterInfo p, int index) =>
                {
                    try
                    {
                        return new
                        {
                            p = p.Name,
                            v = paramValues[index]
                        };
                    }catch(Exception e)
                    {
                        Hashtable hash = new Hashtable();
                        hash.Add("func_get_args.p1.method_Name", method.Name);
                        hash.Add("func_get_args.p2.prm", paramValues);
                         
                        logErr2024(e, "func_get_args", "errlogDir2024",hash);
                        print_r(e);
                        return new
                        {
                            p = "pxx",
                            v = "vxxx"
                        };
                    }
                   
                }
             
            ).ToList();
            return parameterValues;
            // 序列化为 JSON 字符串
            //  return JsonConvert.SerializeObject(parameterValues, Formatting.Indented);
        }

        static string GetCurrentMethodParametersJson()
        {
            // 获取当前方法
            MethodBase method = new StackFrame(1).GetMethod();

            // 获取当前方法的参数
            ParameterInfo[] parameters = method.GetParameters();

            // 获取当前方法的参数值
            var parameterValues = parameters.Select(p =>
            {
                // 使用反射获取参数值
                object value = p.RawDefaultValue; // 默认值
                if (value == DBNull.Value) value = null; // 如果是数据库的 NULL 值

                return new { p.Name, Value = value };
            }).ToList();

            // 序列化为 JSON 字符串
            return JsonConvert.SerializeObject(parameterValues, Formatting.Indented);
        }

        public static string str_repeat(string v, int count)
        {
            return new string(' ', count);
        }
    }
}
//$GLOBALS['dbg']=[];

//$GLOBALS['dbg_show']=true;





