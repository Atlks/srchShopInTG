using DocumentFormat.OpenXml.Office2019.Excel.RichData2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace prj202405.lib
{
    internal class db
    {
        public static ArrayList iot2hpLst(SortedList listIot)
        {
            // 创建一个 ArrayList 来存储 SortedList 中的值
            ArrayList arrayList = new ArrayList();

            // 遍历 SortedList 的值并添加到 ArrayList 中
            foreach (var value in listIot.Values)
            {
                arrayList.Add(value);
            }
            return arrayList;
        }

        public static ArrayList lstFrmIot(SortedList listIot)
        {
            // 创建一个 ArrayList 来存储 SortedList 中的值
            ArrayList arrayList = new ArrayList();

            // 遍历 SortedList 的值并添加到 ArrayList 中
            foreach (var value in listIot.Values)
            {
                arrayList.Add(value);
            }
            return arrayList;
        }


        public static List<InlineKeyboardButton[]> lstFrmIot4inlnKbdBtn(SortedList listIot)
        {
            // 创建一个 ArrayList 来存储 SortedList 中的值
            List<InlineKeyboardButton[]>  arrayList = [];

            // 遍历 SortedList 的值并添加到 ArrayList 中
            foreach (var value in listIot.Values)
            {
                arrayList.Add((InlineKeyboardButton[])value);
            }
            return arrayList;
        }

        public static SortedList lst2IOT(ArrayList arrayList)
        {
            SortedList hash = new SortedList();


            foreach (var item in arrayList)
            {
                SortedList itemx = (SortedList)item;
                hash.Add(itemx["id"], item);
            }

            return hash;
        }


        public static SortedList lst2IOT(List<SortedList> arrayList)
        {
            SortedList hash = new SortedList();


            foreach (SortedList item in arrayList)
            {
                
                hash.Add(item["id"], item);
            }

            return hash;
        }
        public static SortedList lst2IOT4inlKbdBtnArr(List<InlineKeyboardButton[]> arrayList, string idColmName)
        {
            SortedList obj = new SortedList();


            foreach (InlineKeyboardButton[] item in arrayList)
            {
               
                //   SortedList itemx = (SortedList)item;
                try {
                    InlineKeyboardButton btn = item[0];
                    map_add(obj, btn.CallbackData, item);
                   
                }
                catch(Exception e) {
                    Console.WriteLine(e.Message);

                }
                
            }

            return obj;
        }

        private static void map_add(SortedList obj, string? callbackData, object item)
        {
            try { obj.Add(callbackData, item); }
            catch(Exception e)
            {

            }
           
        }

        public static SortedList lst2IOT(ArrayList arrayList,string idColmName)
        {
            SortedList obj = new SortedList();


            foreach (var item in arrayList)
            {
                SortedList itemx = (SortedList)item;
                obj.Add(itemx[idColmName], item);
            }

            return obj;
        }


        public static object getRowVal(List<Dictionary<string, string>> lst, string fld, string v2)
        {
            if (lst.Count > 0)
            {
                Dictionary<string, string> d = lst[0];
                if (d.ContainsKey(fld))
                {
                    object v = d[fld];
                    return v;
                }
                else
                    return v2;


            }
            return v2;
        }

        public static object getRowVal(List<Dictionary<string, object>> lst, string fld, string v2)
        {
            if (lst.Count > 0)
            {
                Dictionary<string, object> d = lst[0];
                if (d.ContainsKey(fld))
                {
                    object v = d[fld];
                    return v;
                }
                else
                    return v2;


            }
            return v2;
        }


        internal static string calcPatns(string dir, string partfile区块文件)
        {  
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), dir, partfile区块文件));

            if (string.IsNullOrEmpty(   partfile区块文件))
            {
              
                string rzt = GetFilePathsCommaSeparated(dir);
                dbgCls.setDbgValRtval(__METHOD__, rzt);
                return rzt;
            }
             ArrayList arrayList = new ArrayList();
            string[] dbArr = partfile区块文件.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach(var dbf in dbArr)
            {
                string path = dir + "/" + dbf+".json";
                if (!File.Exists(path))
                {
                    Console.WriteLine("not exist file dbf=>" + path);
                    continue;
                }
                arrayList.Add(path);
            }

            // 使用 ArrayList 的 ToArray 方法将其转换为对象数组
            object[] objectArray = arrayList.ToArray();

            // 使用 String.Join 方法将数组转换为逗号分割的字符串
            string result = string.Join(",", objectArray);

            dbgCls.setDbgValRtval(__METHOD__, result);
            
            return result;
        }

        static string GetFilePathsCommaSeparated(string directoryPath)
        {
            // 获取目录下的所有文件路径
            string[] filePaths = Directory.GetFiles(directoryPath);

            // 将文件路径数组转换为逗号分割的字符串
            string result = string.Join(",", filePaths);

            return result;
        }
    }
}
