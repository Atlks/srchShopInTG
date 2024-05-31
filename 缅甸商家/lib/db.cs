using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static SortedList lst2IOT(ArrayList arrayList)
        {
            SortedList obj = new SortedList();


            foreach (var item in arrayList)
            {
                SortedList itemx = (SortedList)item;
                obj.Add(itemx["id"], item);
            }

            return obj;
        }
    }
}
