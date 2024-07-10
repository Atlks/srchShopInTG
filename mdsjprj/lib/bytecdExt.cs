global using static mdsj.lib.bytecdExt;
using prj202405.lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdsj.lib
{
    internal class bytecdExt
    {
        /*
         这些指令将参数加载到堆栈上，然后可以使用其他指令来处理参数，例如调用 GetType 方法获取其类型。

在实际编写IL代码时，可以结合使用 ldarg 指令加载参数，然后调用 call 指令来调用方法，最后使用 callvirt 指令来调用 GetType 方法。
         */
        public static string gettype(object obj)
        {
            return obj.GetType().ToString();
        }


        public static int gtfldInt(SortedList dafenObj, string fld, int df)
        {
            try
            {
                var obj = gtfld(dafenObj, fld, df);
                return toInt(obj);
            }catch(Exception e)
            {
                print_catchEx(nameof(gtfldInt), e);
                return df;
            }
           
        }
        //todo
//        十 Adam 大鱼 刘洋 汤姆, [9/7/2024 下午 11:28]
//"ToLower",
//  "Replace",
//  "Trim",

//十 Adam 大鱼 刘洋 汤姆, [9/7/2024 下午 11:28]
//        ToUpper

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:28]
//ToSimple

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:29]
//Join

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:29]
//Remove

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:30]
//TryGetValue

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:30]
//ToList

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:30]
//SerializeObject

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:31]
//"Sort",
//  "CompareTo",

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:31]
//Exists

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:32]
//Run

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:32]
//log

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:32]
//StartsWith

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:32]
//endwith

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:33]
//Match

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:33]
//Parse

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:34]
//Insert

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:34]
//"AddDocument",

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:34]
//GetFiles

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:34]
//Execute

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:35]
//ToTraditional

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:35]
//ReadAsStringAsync

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:35]
//"Value<JArray>",

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:36]
//GetStringAsync

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:36]
//Count

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:36]
//AddDays

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:36]
//Find

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:37]
//ElementAt

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:37]
//IsWindowVisible

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:38]
//NewPageAsync

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:38]
//newx

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:38]
//Exit

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:39]
//LoadHtml

//十 Adam 大鱼 刘洋 汤姆, [9 / 7 / 2024 下午 11:39]
//SetValue
        public static List<SortedList> foreach_Sqlt(string sqltFl, Func<SortedList, SortedList> fun)
        {
            List<SortedList> li = new List<SortedList>();


            List<SortedList> liFrmFl = ormSqlt.qryV2(sqltFl);
            foreach (SortedList rw in liFrmFl)
            {
                li.Add(fun(rw));
            }

            ormSqlt.saveMltHiPfm(li, sqltFl);

            return li;
        }

        public static void CopySortedList(SortedList source, SortedList destination)
        {
            foreach (DictionaryEntry entry in source)
            {
                if (destination.ContainsKey(entry.Key))
                    destination[entry.Key] = entry.Value;
                else
                    destination.Add(entry.Key, entry.Value);

            }
        }

        public static List<SortedList> SliceX<SortedList>(List<SortedList> list, int start, int length)
        {
            if (start < 0) start = 0;
            if (start >= list.Count) return new List<SortedList>();

            if (length < 0) length = 0;
            if (start + length > list.Count) length = list.Count - start;

            return list.GetRange(start, length);
        }

        public static Dictionary<string, string> ldDic4qryCdtn(string qrystr)
        {
            var filters2 = ldDicFromQrystr(qrystr);
            var filters = RemoveKeys(filters2, "page limit");
            return filters;
        }
        public static void foreach_DictionaryKeys(Dictionary<string, string> dictionary, Action<string> keyAction)
        {
            foreach (var key in dictionary.Keys)
            {
                keyAction(key);
            }
        }
        public static string ldfld(Dictionary<string, string> whereExprsObj, string fld)
        {
            if (whereExprsObj.ContainsKey(fld))
                return whereExprsObj[fld];
            return "";
        }

        public static void print(object v)
        {
            System.Console.WriteLine(v);
        }
        public static void print(string format, object arg0)
        {
            System.Console.WriteLine(format, arg0);
        }

        public static int toInt(object obj)
        {
            return Convert.ToInt32(obj);
        }
        public static  int len(object obj)
        {
            if (IsString(obj))
                return obj.ToString().Length;
            if (obj is System.Collections.IList list)
            {
                return list.Count;
            }

            if (obj is SortedList sortedList)
            {
                return sortedList.Count;
            }
            return 0;

        }

      
    }
}
