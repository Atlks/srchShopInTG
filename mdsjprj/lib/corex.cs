using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace prj202405.lib
{
    //prj202405.lib.corex
    internal class corex
    {




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
