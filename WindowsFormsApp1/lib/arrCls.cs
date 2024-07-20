using static prjx.lib.corex;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
using System.Collections;
 
 

namespace prjx.lib
{
    internal class arrCls
    {


        public static object array_slice<t>(List<t> inputList, int startIdx, int length)
        {
            //  List<Dictionary
            // 确保 length 不超过列表的长度
            if (length > inputList.Count)
            {
                length = inputList.Count;
            }

            // 使用 GetRange 方法来截取列表的前 length 个元素
            var rtz = inputList.GetRange(startIdx, length);
            return rtz;
        }

        public static object array_slice(List<Dictionary<string, object>> inputList, int startIdx, int length)
        {
            //  List<Dictionary
            // 确保 length 不超过列表的长度
            if (length > inputList.Count)
            {
                length = inputList.Count;
            }

            // 使用 GetRange 方法来截取列表的前 length 个元素
            var rtz = inputList.GetRange(startIdx, length);
            return rtz;
        }
        public static object array_slice(ArrayList inputList, int startIdx, int length)
        {
            //  List<Dictionary
            // 确保 length 不超过列表的长度
            if (length > inputList.Count)
            {
                length = inputList.Count;
            }

            // 使用 GetRange 方法来截取列表的前 length 个元素
            ArrayList rtz = inputList.GetRange(startIdx, length);
            return rtz;
        }

        public static List<SortedList> array_slice(List<SortedList> inputList, int startIdx, int length)
        {
            // 确保 length 不超过列表的长度
            if (length > inputList.Count)
            {
                length = inputList.Count;
            }

            // 使用 GetRange 方法来截取列表的前 length 个元素
            List<SortedList> rtz = inputList.GetRange(startIdx, length);
            return rtz;
            //  return arr_rzt;
        }

        /**
         * // 使用 Filter 函数筛选出 age 大于 23 的元素
        var filteredList = Filter(list, sl => (int)sl["age"] > 23).ToList();
         * 
         */
        public static IEnumerable<SortedList> arrar_filter(IEnumerable<SortedList> source, Func<SortedList, bool> predicate)
        {
            // 使用 LINQ 的 Where 方法筛选元素
            return source.Where(predicate);
        }

        static List<SortedList> sort(List<SortedList> list, string key)
        {
            // 使用 List 的 Sort 方法进行排序
            list.Sort((x, y) => Comparer.Default.Compare(x[key], y[key]));
            return list;
        }

        static IEnumerable<SortedList> array_map(IEnumerable<SortedList> source, Func<SortedList, SortedList> mapFunction)
        {
            // 使用 LINQ 的 Select 方法对每个元素应用 mapFunction
            return source.Select(mapFunction);
        }
        public static object getHashtableKv(SortedList hashobj, string fld, object v2)
        {
            try
            {
                if (hashobj.ContainsKey(fld))
                    return hashobj[fld];
                return v2;
            }catch(Exception e)
            {
                return v2;
            }
                
           
        }

        public static object getRowVal(List<Dictionary<string, object>> lst, string fld, string v2)
        {
            if (lst.Count > 0)
            {
                Dictionary<string, object> d = lst[0];
                if (d.ContainsKey(fld))
                    return d[fld];

                else
                    return v2;
               

            }
            return v2;
        }
        internal static void map_add(SortedList map, string idClmName, object item)
        {

            //   SortedList itemx = (SortedList)item;
            try
            {

                map.Add(idClmName, item);
            }
            catch (Exception e)
            {
               print(e.Message);

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
        

                 public static List<t> array_merge<t>(List<t> list1, List<t> list2)
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


           print(JsonConvert.SerializeObject(results));


            string showtitle = "spatime(ms):";
            showSpanTime(timestamp, showtitle);

        }

        private static void showSpanTime(long timestamp, string showtitle)
        {
            long timestamp_end = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long spantime = (timestamp_end - timestamp);

           print(showtitle + spantime);
        }
 

        internal static void saveIncrs(SortedList<string, int> ordMap, string callbackData)
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


        public static string TryGetValueDfEmpy(Dictionary<string, object> whereExprsObj, string k)
        {
            // 使用 TryGetValue 方法获取值
            object value;
            try
            {
                return (string)whereExprsObj[k];

            }
            catch (Exception ex)
            {
                return null;
            }
            //if (whereExprsObj.TryGetValue(k, out (StringValues)value))
            //{
            //    return (string)value;
            //}

        }

        public static string TryGetValueAsStrDfEmpty(SortedList whereExprsObj, string k)
        {
            // 使用 TryGetValue 方法获取值
            object value;
            if (whereExprsObj.ContainsKey(k))
                if (whereExprsObj[k] == null)
                    return "";
                else
                    return whereExprsObj[k].ToString();
            else
                return "";
        }

            internal static string TryGetValueAsStrDefNull(SortedList whereExprsObj, string k)
        {
            // 使用 TryGetValue 方法获取值
            object value;
            if (whereExprsObj.ContainsKey(k))
                if (whereExprsObj[k] == null)
                    return null;
                else
                    return whereExprsObj[k].ToString();
            else
                return null;

            //if (whereExprsObj.TryGetValue(k, out (StringValues)value))
            //{
            //    return (string)value;
            //}

        }

        internal static string TryGetValue(Dictionary<string, object> whereExprsObj, string k)
        {
            // 使用 TryGetValue 方法获取值
            object value;
            if (whereExprsObj.ContainsKey(k))
                return (string)whereExprsObj[k];
            else
                return null;

            //if (whereExprsObj.TryGetValue(k, out (StringValues)value))
            //{
            //    return (string)value;
            //}

        }

        internal static void replaceKeyV(SortedList obj, string k, object v)
        {
            if (k == null)
                return;
            if (obj.ContainsKey(k))
                obj[k] = v;
            else
                obj.Add(k, v);
        }

        public static int count(object collection)
        {
            return 计算长度(collection);
        }

        /// <summary>
        /// 计算集合的长度。
        /// </summary>
        /// <param name="collection">集合对象。</param>
        /// <returns>集合的长度。</returns>
        public static int 计算长度(object collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection), "Collection cannot be null");
            }

            if (collection is ICollection col)
            {
                return col.Count;
            }

            if (collection is IEnumerable enumerable)
            {
                int count = 0;
                foreach (var item in enumerable)
                {
                    count++;
                }
                return count;
            }

            throw new ArgumentException("Unsupported collection type", nameof(collection));
        }
        public static SortedList CopyToOldSortedList(SortedList newList, SortedList oldList)
        {
            // 创建一个新的 SortedList
            // SortedList newList = new SortedList();

            // 遍历旧的 SortedList 并将每个键值对复制到新的 SortedList
            foreach (DictionaryEntry newx in newList)
            {
                if(newx.Key!=null)
                   arrCls.addRplsKeyV(oldList, newx.Key.ToString(), newx.Value);
                //   newList.Add(entry.Key, entry.Value);
            }

            return newList;
        }
        internal static void addRplsKeyV(SortedList listIot, string key, SortedList objSave)
        {
            if (listIot.ContainsKey(key))
                listIot[key] = objSave;
            else
                listIot.Add(key, objSave);
        }

        internal static void addRplsKeyV(SortedList listIot, string key, object objSave)
        {
            if (listIot.ContainsKey(key))
                listIot[key] = objSave;
            else
                listIot.Add(key, objSave);
        }
		
		    internal static HashSet<string> addSetNStr(HashSet<string> set, string v)
        {
           
            string[] a = v.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string wd in a)
            {
                set.Add(wd);
            }
            return set;
        }

        //internal static int getRowVal(object s1, string v1, int v2)
        //{
        //    throw new NotImplementedException();
        //}        //internal static int getRowVal(object s1, string v1, int v2)
        //{
        //    throw new NotImplementedException();
        //}
    }
}