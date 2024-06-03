using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using System.Collections;
using Microsoft.Extensions.Primitives;
using Telegram.Bot.Types;

namespace prj202405.lib
{
    internal class arrCls
    {
        internal static void map_add(SortedList map, string idClmName, object item)
        {

            //   SortedList itemx = (SortedList)item;
            try
            {

                map.Add(idClmName, item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
        }


        internal static ArrayList dedulip(ArrayList List1, string idClmName)
        {
            ArrayList list = (ArrayList)List1;
            SortedList listIot = db.lst2IOT(list, idClmName);

            //  listIot.Add(((SortedList)objSave)["id"], objSave);

            ArrayList saveList_hpmod = db.lstFrmIot(listIot);
            return saveList_hpmod;
        }

        internal static ArrayList dedulipV2(ArrayList List1, string idClmName)
        {
            ArrayList list = (ArrayList)List1;
            SortedList listIot = db.lst2IOT(list, idClmName);

            //  listIot.Add(((SortedList)objSave)["id"], objSave);

            ArrayList saveList_hpmod = db.lstFrmIot(listIot);
            return saveList_hpmod;
        }
        public static ArrayList dedulip(ArrayList List1)
        {
            ArrayList list = (ArrayList)List1;
            SortedList listIot = db.lst2IOT(list);

            //  listIot.Add(((SortedList)objSave)["id"], objSave);

            ArrayList saveList_hpmod = db.lstFrmIot(listIot);
            return saveList_hpmod;
        }

        public static List<t> MergeLists<t>(List<t> list1, List<t> list2)
        {
            List<t> result = new List<t>();

            // 获取最长列表的长度
            int maxLength = Math.Max(list1.Count, list2.Count);

            // 遍历并合并列表
            //for (int i = 0; i < maxLength; i++)
            //{
            for (int i = 0; i < list1.Count; i++)
            {
                result.Add(list1[i]);
            }

            for (int i = 0; i < list2.Count; i++)
            {
                result.Add(list2[i]);
            }
            //}

            return result;
        }
        public static List<T> rdmList<T>(List<T> results)
        {
            List<T> results22;
            Random rng = new Random();

            results22 = results.OrderBy(x => rng.Next()).ToList();
            return results22;
        }



        private static void findd()
        {
            long timestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            var users_txt = System.IO.File.ReadAllText("db.json");

            showSpanTime(timestamp, "readFile");

            ////JsonSerializerSettings settings = new JsonSerializerSettings();
            ////settings.DefaultValueHandling = DefaultValueHandling.Ignore;
            JArray rows = JsonConvert.DeserializeObject<JArray>(users_txt);
            showSpanTime(timestamp, "delzobj");
            var results = (from jo in rows
                           where jo.Value<int>("key") == 10
                           select jo).ToList();


            Console.WriteLine(JsonConvert.SerializeObject(results));


            string showtitle = "spatime(ms):";
            showSpanTime(timestamp, showtitle);

        }

        private static void showSpanTime(long timestamp, string showtitle)
        {
            long timestamp_end = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long spantime = (timestamp_end - timestamp);

            Console.WriteLine(showtitle + spantime);
        }

        internal static List<InlineKeyboardButton[]> dedulip4inlnKbdBtnArr(List<InlineKeyboardButton[]> List1, string idClmName)
        {
            //  ArrayList list = new ArrayList(List1);
            SortedList listIot = db.lst2IOT4inlKbdBtnArr(List1, idClmName);

            //  listIot.Add(((SortedList)objSave)["id"], objSave);

            List<InlineKeyboardButton[]> saveList_hpmod = db.lstFrmIot4inlnKbdBtn(listIot);
            return saveList_hpmod;
        }

        internal static void saveIncrs(SortedList<string, int> ordMap, string? callbackData)
        {
            if (!ordMap.ContainsKey(callbackData))
                ordMap.Add(callbackData, 1);
            else
            {
                ordMap[callbackData] = ordMap[callbackData] + 1;
            }
        }

        internal static string rowValDefEmpty(SortedList row, string v)
        {
            if (row[v] == null)
                return "";
            return row[v].ToString();
        }

        internal static string TryGetValue(Dictionary<string, StringValues> whereExprsObj, string k)
        {
            // 使用 TryGetValue 方法获取值
            object value;
            try
            {
                return whereExprsObj[k];

            }
            catch(Exception ex){
                return null;
            }
            //if (whereExprsObj.TryGetValue(k, out (StringValues)value))
            //{
            //    return (string)value;
            //}
           
        }
    }
}