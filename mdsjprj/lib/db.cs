using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    }
}
