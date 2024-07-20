using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace prjx.lib
{
    internal class sqlCls
    {



        public static string arr_toSqlPrms4insert(SortedList arr)
        {
 


    //var columns = implode(", ", arr.Keys);

            var columns = strCls.JoinHashtbKV(",", arr.Keys);
            //$val = implode(',',array_values($arr));
            //string escaped_values = array_map(function(v) {
            //    return "'"+v+ "'";
            //},  arr.Values);

            //Select，结果为{元素0, 元素1, 元素2}，类型为List<ClassA>
            // c#.net Hashtable.Values 做 select操作，转化为  字符串数组
            //var list1 = (List<string>)arr.Values;
            //List<string> escaped_values = list1.Select(e => "'" + e + "'").ToList();

            // 获取Hashtable的所有值
            ICollection values = arr.Values;

            // 将值转换为字符串数组
            object[] valueArray = values.Cast<object>().ToArray();

            // 使用 LINQ 的 Select 操作对字符串数组进行操作
         //   if(value==null)
            var escaped_values = valueArray.Select(value =>
            {
                if (value == null)
                    return "null";
                else
                    return "'" + value.ToString() + "'";
            }
         
            ).ToArray();


            string values2024 = string.Join(",", escaped_values);
            return "("+ columns+ ")values("+ values2024 + ")";
        }

    }
}
