using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdsj.lib
{
    internal class basicEasygpl
    {
        /*

 .子程序 call, 公开, 通用型
.参数 callback, 句柄型
.参数 args, 数据组
 */
        public static object 调用(Delegate 函数, params object[] 参数组)
        {

            var __METHOD__ = 函数.Method.Name;
            object o = null;
            try
            {
                o = 函数.DynamicInvoke(参数组);
            }
            catch (Exception e)
            {
                print_catchEx(__METHOD__, e);
                SortedList dbgobj = new SortedList();
                dbgobj.Add("mtth", __METHOD__ + "(((" + encodeJsonNofmt(func_get_args(参数组)) + ")))");
                logErr2024(e, __METHOD__, "errdir", dbgobj);
            }
            return o;
        }

        public static void  调试输出(object obj)
        {
            Console.WriteLine(json文本(obj));
        }
        private static string 参数类型(object obj)
        {
            return obj.GetType().ToString();
        }
        

        private static string json文本(object obj)
        {
            return json_encode_noFmt(obj);
        }


        private static string 到文本(object obj)
        {
            return obj.ToString();
        }

        private static int 取文本长度(string obj)
        {
            return obj.ToString().Length;
        }

        private static string 取成员文本值(object obj)
        {
            return obj.ToString();
        }
        


    }
}
