using DocumentFormat.OpenXml.Office2019.Excel.RichData2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
//  prj202405.lib.db
namespace prj202405.lib
{
    internal class db
    {
        public static List<t> qryFrmSqlt<t>(string dbfFrom,
  Func<SortedList, bool> whereFun,
  Func<SortedList, int> ordFun,
  Func<SortedList, t> selktFun)
        {
            ArrayList rows_rzt4srch = [];
            List<SortedList> rows = ormSqlt.qryV2(dbfFrom);
            foreach (SortedList row in rows)
            {
                if (whereFun(row))
                {
                    rows_rzt4srch.Add(row);
                }
                //  遍历一个大概40ms   case trycat 模式，给为if else 模式，立马变为1ms
                // Console.WriteLine(DateTime.Now.ToString("yyyyMMdd_HHmmss_fff"));  
            }


            List<SortedList> list = rows_rzt4srch.Cast<SortedList>()
                                  .OrderBy(ordFun)
                                  .ToList();


            List<t> rsRztInlnKbdBtn = [];
            for (int i = 0; i < rows_rzt4srch.Count; i++)
            {
                SortedList row = list[i];
                rsRztInlnKbdBtn.Add(selktFun(row));
            }

            return rsRztInlnKbdBtn;

        }


        public static List<t> qryV6<t>(string dbfFrom,
       Func<SortedList, bool> whereFun,
       Func<SortedList, int> ordFun,
       Func<SortedList, t> selktFun)
        {
            ArrayList rows_rzt4srch = [];
            List<SortedList> rows = ormJSonFL.qry(dbfFrom);
            foreach (SortedList row in rows)
            {
                if (whereFun(row))
                {
                    rows_rzt4srch.Add(row);
                }
                //  遍历一个大概40ms   case trycat 模式，给为if else 模式，立马变为1ms
                // Console.WriteLine(DateTime.Now.ToString("yyyyMMdd_HHmmss_fff"));  
            }


            List<SortedList> list = rows_rzt4srch.Cast<SortedList>()
                                  .OrderBy(ordFun)
                                  .ToList();


            List<t> rsRztInlnKbdBtn = [];
            for (int i = 0; i < rows_rzt4srch.Count; i++)
            {
                SortedList row = list[i];
                rsRztInlnKbdBtn.Add(selktFun(row));
            }

            return rsRztInlnKbdBtn;

        }

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
        /// <summary>
        /// Converts a SortedList to a List of SortedLists.
        /// </summary>
        /// <param name="listIot">The SortedList to be converted.</param>
        /// <returns>A List of SortedLists containing the values from the input SortedList.</returns>

        public static List<SortedList> iot2list(SortedList listIot)
        {
            // 创建一个 ArrayList 来存储 SortedList 中的值
            List<SortedList> arrayList = new List<SortedList>();

            // 遍历 SortedList 的值并添加到 ArrayList 中
            foreach (var value in listIot.Values)
            {
                arrayList.Add((SortedList)value);
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
        public static object getRowVal(List<SortedList > lst, string fld, string v2)
        {
            if (lst.Count > 0)
            {
                SortedList d = lst[0];
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
                string path = dir + "/" + dbf+".db";
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
