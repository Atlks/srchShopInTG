using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace prj202405.lib
{
    internal class dbgCls
    {

        public static object array_slice(object arr_rzt, int v1, int v2)
        {
            return arr_rzt;
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
            Console.WriteLine("\n\n\n" + msglog + "");
        }

        public static void setDbgVal(string METHOD__, string vname, string val)
        {
            //if($GLOBALS['dbg_show']==false)
            //    return;
            var msglog = str_repeat(" ", dbgpad+3) + "" + METHOD__ + $"():: {vname}=>{val}";
            // array_push($GLOBALS['dbg'],        $msg);
            Console.WriteLine(msglog + "");

        }

        public static void setDbgValRtval(object mETHOD__, object results)
        {
            //string jsonString = JsonConvert.SerializeObject(results, Formatting.Indented);
            //Console.WriteLine(jsonString);

            //    if ($GLOBALS['dbg_show'] == false)
            //return;
            // ENDFUN
            var msglog = str_repeat(" ", dbgpad) + " ENDFUN " + mETHOD__ + "():: ret=>" + json_encode(results);
            Console.WriteLine(msglog + "");
            //    array_push($GLOBALS['dbg'], $msglog);
            dbgpad = dbgpad - 4;
        }

        //public static object array_slice(object arr_rzt, int v1, int v2)
        //{
        //    return arr_rzt;
        //}


        public static object func_get_args(MethodBase method, params object[] paramValues)
        {
            // 获取当前方法
           // MethodBase method = new StackFrame(1).GetMethod();

            // 获取当前方法的参数
            ParameterInfo[] parameters = method.GetParameters();

            // 获取当前方法的参数值
            // 将参数名称和值配对
            var parameterValues = parameters.Select((p, index) => new
            {
                p = p.Name,
                v = paramValues[index]
            }).ToList();
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
        public static string json_encode(object results)
        {
            //   options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            };
            //  string json = JsonConvert.SerializeObject(obj, settings);
            string jsonString = JsonConvert.SerializeObject(results, settings);
            // Console.WriteLine(jsonString);
            return jsonString;
        }

        public static string str_repeat(string v, int count)
        {
            return new string(' ', count);
        }
    }
}
//$GLOBALS['dbg']=[];

//$GLOBALS['dbg_show']=true;





