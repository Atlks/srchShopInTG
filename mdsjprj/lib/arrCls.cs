global using static prj202405.lib.arrCls;
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
using System.Reflection;
using System.Text.Json;




namespace prj202405.lib
{
    public class arrCls
    {
        public static SortedList<string, string> MergeSortedLists(SortedList<string, string> list1, SortedList<string, string> list2)
        {
            // 创建一个新的 SortedList 来存储合并后的结果
            SortedList<string, string> mergedList = new SortedList<string, string>();

            // 添加第一个 SortedList 的所有元素
            foreach (var kvp in list1)
            {
                mergedList[kvp.Key] = kvp.Value;
            }

            // 添加第二个 SortedList 的所有元素，如果键已存在，则覆盖其值
            foreach (var kvp in list2)
            {
                mergedList[kvp.Key] = kvp.Value;
            }

            return mergedList;
        }
        public static void CleanupSortedListValuesStartWzAlphbt(SortedList<string, string> sortedList)
        {
            // 创建一个列表，存储需要移除的键
            HashSet<string> keysToRemove = new HashSet<string>();

            // 遍历 SortedList，找出以字母开头的值
            foreach (var kvp in sortedList)
            {
                if (string.IsNullOrEmpty(kvp.Value))
                {
                    keysToRemove.Add(kvp.Key); return;

                }

                char c = kvp.Value[0];
                if (!string.IsNullOrEmpty(kvp.Value) && IsEnglishLetter(c))
                {
                    keysToRemove.Add(kvp.Key);
                }
            }

            // 移除找到的键
            foreach (var key in keysToRemove)
            {
                sortedList.Remove(key);
            }
        }
    public    static List<string> ReadJsonFileToList(string filePath)
        {
            List<string> jsonStringList = new List<string>();

            try
            {
                // 读取 JSON 文件内容
                string jsonString = System.IO.File.ReadAllText(filePath);

                // 使用 Newtonsoft.Json 库将 JSON 字符串反序列化为 List<string>
                jsonStringList = JsonConvert.DeserializeObject<List<string>>(jsonString);
            }
            catch (FileNotFoundException)
            {
                ConsoleWriteLine($"File not found: {filePath}");
            }
            catch (Newtonsoft.Json.JsonException)
            {
                ConsoleWriteLine($"Invalid JSON format in file: {filePath}");
            }
            catch (Exception ex)
            {
                ConsoleWriteLine($"Error reading JSON file: {ex.Message}");
            }

            return jsonStringList;
        }
        static List<string> getListFrmJsonFil(string filePath)
        {
            List<string> jsonStringList = new List<string>();

            try
            {
                // 读取 JSON 文件内容
                string jsonString = System.IO.File.ReadAllText(filePath);
                // Configure JsonSerializerOptions to allow reflection-based serialization
                var options = new JsonSerializerOptions
                {
                    // Enable reflection-based serialization
                    //   TypeNameHandling = TypeNameHandling.All, // or TypeNameHandling.Auto
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };
                // 将 JSON 字符串解析为 List<string>
                jsonStringList = System.Text.Json.JsonSerializer.Deserialize<List<string>>(jsonString, options);
            }
            catch (FileNotFoundException)
            {
                ConsoleWriteLine($"File not found: {filePath}");
            }
            catch (System.Text.Json.JsonException)
            {
                ConsoleWriteLine($"Invalid JSON format in file: {filePath}");
            }
            catch (Exception ex)
            {
                ConsoleWriteLine($"Error reading JSON file: {ex.Message}");
            }

            return jsonStringList;
        }
        public static bool EndsWith(string ext, string extss)
        {
            string[] a = extss.Split(" ");
            foreach (string ex in a)
            {
                if (ext.EndsWith(ex))
                    return true;
            }
            return false;
        }


       

        public static SortedList<string, string> ReadJsonFileToSortedList(string filePath)
        {
            // 创建一个新的 SortedList 来存储结果
            SortedList<string, string> sortedList = new SortedList<string, string>();

            // 检查文件是否存在
            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified file does not exist.", filePath);
            }

            // 读取文件的内容
            string jsonContent = System.IO.File.ReadAllText(filePath);

            // 解析 JSON 数据为字典
            Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonContent);

            // 将字典数据添加到 SortedList 中
            foreach (var kvp in data)
            {
                sortedList[kvp.Key] = kvp.Value;
            }

            return sortedList;
        }
        public static SortedList<string, string> ReadIniFileToSortedList(string filePath)
        {
            // 创建一个新的 SortedList 来存储结果
            SortedList<string, string> sortedList = new SortedList<string, string>();

            // 检查文件是否存在
            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified file does not exist.", filePath);
            }

            // 读取文件的所有行
            string[] lines = System.IO.File.ReadAllLines(filePath);

            // 遍历每一行
            foreach (var line in lines)
            {
                // 跳过空行和注释行
                if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith(";") || line.TrimStart().StartsWith("#"))
                {
                    continue;
                }

                // 找到等号的位置
                int equalsIndex = line.IndexOf('=');
                if (equalsIndex > 0)
                {
                    // 提取键和值
                    string key = line.Substring(0, equalsIndex).Trim();
                    string value = line.Substring(equalsIndex + 1).Trim();

                    // 将键值对添加到 SortedList 中
                    sortedList[key] = value;
                }
            }

            return sortedList;
        }


        public static HashSet<string> FilterUrlsEndwithHtm(HashSet<string> urls)
        {
            return new HashSet<string>(urls.Where(url => url.EndsWith(".html", StringComparison.OrdinalIgnoreCase) || url.EndsWith(".htm", StringComparison.OrdinalIgnoreCase)));
        }
        public static string GetExtension(string url)
        {
            try
            {
                var ext = Path.GetExtension(url);
                ext = ext.ToLower().Trim();
                return ext;
            }
            catch (Exception e)
            {
                return "";
            }

        }

        public static void CleanupSortedListKeysLenLessthan3(SortedList<string, string> sortedList)
        {
            // 创建一个列表，存储需要移除的键
            List<string> keysToRemove = new List<string>();

            // 遍历 SortedList，找出长度小于 4 的键
            foreach (var key in sortedList.Keys)
            {
                if (key.Length < 3)
                {
                    keysToRemove.Add(key);
                }
            }

            // 移除找到的键
            foreach (var key in keysToRemove)
            {
                sortedList.Remove(key);
            }
        }

        public static SortedList arr_ReverseSortedList(SortedList originalList)
        {
            SortedList reversedList = new SortedList();

            foreach (DictionaryEntry entry in originalList)
            {
                var value = entry.Value.ToString();
                string[] a = value.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                foreach (string w in a)
                {
                    reversedList.Add(w.Trim().ToUpper(), entry.Key);
                }

            }

            return reversedList;
        }

        public static List<SortedList> arr_fltr(List<SortedList> list, Func<SortedList, bool> fn)
        {
            List<SortedList> list22 = new List<SortedList>();
            foreach (SortedList rw in list)
            {
                if (fn(rw))
                    list22.Add(rw);
            }
            return list22;
        }
        public static void transfmVal(SortedList list,Func<string,object> fun            )
        {
            // 创建一个临时的 ArrayList 来存储键
            ArrayList keys = new ArrayList(list.Keys);

            // 遍历每个键并更新值
            foreach (string key in keys)
            {
                string value = (string)list[key];
                list.Remove(key);
                list.Add(key, fun(value)) ;
            }
        }
        public static void castVal2hashtable(SortedList list)
        {
            // 创建一个临时的 ArrayList 来存储键
            ArrayList keys = new ArrayList(list.Keys);

            // 遍历每个键并更新值
            foreach (string key in keys)
            {
                string value = (string)list[key];
                list.Remove(key);
                list.Add(key, castUrlQueryString2hashtable(value));
            //    list[key] = castUrlQueryString2hashtable(value); ;
            }
        }

        public   static string GetKeysCommaSeparated(SortedList list)
        {
            // 检查输入参数是否为 null
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            // 创建一个 StringBuilder 来构建结果字符串
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach (var key in list.Keys)
            {
                // 将每个键添加到 StringBuilder 中，并附加逗号
                sb.Append(key).Append(",");
            }

            // 移除最后一个多余的逗号
            if (sb.Length > 0)
            {
                sb.Length--; // 或者 sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }
        public static void setFld(SortedList cfg, string f, object v)
        {
            if (cfg.ContainsKey(f))
                cfg.Remove(f);

            cfg.Add(f, v);
        }
        public static void RemoveWordsFromHashSet(HashSet<string> words, string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return;

            string[] wordsToRemove = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in wordsToRemove)
            {
                if (word.Trim().Length > 0)
                    words.Remove(word);
            }
        }
        public static HashSet<string> MergeArrayWithHashSet(string s, HashSet<string> set)
        {
            string[] array = s.Split(" ");
            // 创建一个新的 HashSet<string>，包含 HashSet<string> 的所有元素
            HashSet<string> resultSet = new HashSet<string>(set);

            // 将字符串数组中的所有元素添加到新的 HashSet<string> 中
            foreach (string item in array)
            {
                resultSet.Add(item);
            }

            return resultSet;
        }
        private static string ldfld(Dictionary<string, string> parse_str1, string fld)
        {
            if (parse_str1.ContainsKey(fld))
                return parse_str1[fld];
            else
                return "";
        }

        public static string ldfld2str(Dictionary<string, string> parse_str1, string fld)
        {
            if (parse_str1.ContainsKey(fld))
                return parse_str1[fld];
            else
                return "";
        }
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
        public static object ldfld(SortedList hashobj, string fld, object dfval)
        {
            try
            {
                if (hashobj.ContainsKey(fld))
                    return hashobj[fld];
                else
                    return dfval;
            }
            catch (Exception e)
            {
                return dfval;
            }


        }

        public static object ldfld(List<Dictionary<string, object>> lst, string fld, string v2)
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


        //internal static ArrayList dedulip(ArrayList List1, string idClmName)
        //{
        //    ArrayList list = (ArrayList)List1;
        //    SortedList listIot = db.lst2IOT(list, idClmName);

        //    //  listIot.Add(((SortedList)objSave)["id"], objSave);

        //    ArrayList saveList_hpmod = db.lstFrmIot(listIot);
        //    return saveList_hpmod;
        //}

        //internal static ArrayList dedulipV2(ArrayList List1, string idClmName)
        //{
        //    ArrayList list = (ArrayList)List1;
        //    SortedList listIot = db.lst2IOT(list, idClmName);

        //    //  listIot.Add(((SortedList)objSave)["id"], objSave);

        //    ArrayList saveList_hpmod = db.lstFrmIot(listIot);
        //    return saveList_hpmod;
        //}
        //internal static ArrayList dedulip(ArrayList List1, string idClmName)
        //{
        //    ArrayList list = (ArrayList)List1;
        //    SortedList listIot = db.lst2IOT(list, idClmName);

        //    //  listIot.Add(((SortedList)objSave)["id"], objSave);

        //    ArrayList saveList_hpmod = db.lstFrmIot(listIot);
        //    return saveList_hpmod;
        //}

        //internal static ArrayList dedulipV2(ArrayList List1, string idClmName)
        //{
        //    ArrayList list = (ArrayList)List1;
        //    SortedList listIot = db.lst2IOT(list, idClmName);

        //    //  listIot.Add(((SortedList)objSave)["id"], objSave);

        //    ArrayList saveList_hpmod = db.lstFrmIot(listIot);
        //    return saveList_hpmod;
        //}
        //public static ArrayList dedulip(ArrayList List1)
        //{
        //    ArrayList list = (ArrayList)List1;
        //    SortedList listIot = db.lst2IOT(list);

        //    //  listIot.Add(((SortedList)objSave)["id"], objSave);

        //    ArrayList saveList_hpmod = db.lstFrmIot(listIot);
        //    return saveList_hpmod;
        //}
        public static HashSet<string> array_merge (HashSet<string> list1, HashSet<string>  list2)
        {
            return MergeHashSets(list1, list2);
        }
        public static bool isMmsgHasMatchPostWd(HashSet<string> postnKywd位置词set, string[] kwds)
        {
            //if (text == null)
            //    return null;
            string[] spltWds = kwds;
            foreach (string wd in spltWds)
            {
                if (postnKywd位置词set.Contains(wd))
                {
                   print("msgHasMatchPostWd():: postnKywd位置词set.Contains wd=>" + wd);
                    return true;
                }

            }
            return false;
        }

        public static string[] RemoveShortWords(string[] words)
        {
            List<string> result = new List<string>();
            foreach (string word in words)
            {
                if (word.Length >= 2)
                {
                    result.Add(word);
                }
            }
            return result.ToArray();
        }

        public static bool isCcontainKwds42(HashSet<string> curRowKywdSset, string[] kwds)
        {
            kwds = Array.ConvertAll(kwds, s => s.ToUpper());
            curRowKywdSset = ConvertToUpperCase(curRowKywdSset);

            return isMmsgHasMatchPostWd(curRowKywdSset, kwds);
        }
        public static void SetIdProperties(ArrayList arrayList)
        {
            foreach (var item in arrayList)
            {
                SortedList sortedList1 = (SortedList)item;
                sortedList1.Add("id", sortedList1["Guid"]);

            }
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

        //public static List<t> MergeLists<t>(List<t> list1, List<t> list2)
        //{
        //    List<t> result = new List<t>();

        //    // 获取最长列表的长度
        //    int maxLength = Math.Max(list1.Count, list2.Count);

        //    // 遍历并合并列表
        //    //for (int i = 0; i < maxLength; i++)
        //    //{
        //    for (int i = 0; i < list1.Count; i++)
        //    {
        //        result.Add(list1[i]);
        //    }

        //    for (int i = 0; i < list2.Count; i++)
        //    {
        //        result.Add(list2[i]);
        //    }
        //    //}

        //    return result;
        //}
        public static List<T> rdmList<T>(List<T> results)
        {
            List<T> results22;
            Random rng = new Random();

            results22 = results.OrderBy(x => rng.Next()).ToList();
            return results22;
        }



        //private static void findd()
        //{
        //    long timestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        //    var users_txt = System.IO.File.ReadAllText("db.json");

        //    showSpanTime(timestamp, "readFile");

        //    ////JsonSerializerSettings settings = new JsonSerializerSettings();
        //    ////settings.DefaultValueHandling = DefaultValueHandling.Ignore;
        //    JArray rows = JsonConvert.DeserializeObject<JArray>(users_txt);
        //    showSpanTime(timestamp, "delzobj");
        //    var results = (from jo in rows
        //                   where jo.Value<int>("key") == 10
        //                   select jo).ToList();


        //   print(JsonConvert.SerializeObject(results));


        //    string showtitle = "spatime(ms):";
        //    showSpanTime(timestamp, showtitle);

        //}



        //internal static List<InlineKeyboardButton[]> dedulip4inlnKbdBtnArr(List<InlineKeyboardButton[]> List1, string idClmName)
        //{
        //    //  ArrayList list = new ArrayList(List1);
        //    SortedList listIot = db.lst2IOT4inlKbdBtnArr(List1, idClmName);

        //    //  listIot.Add(((SortedList)objSave)["id"], objSave);

        //    List<InlineKeyboardButton[]> saveList_hpmod = db.lstFrmIot4inlnKbdBtn(listIot);
        //    return saveList_hpmod;
        //}

        internal static void Increment(SortedList<string, int> ordMap, string? fld)
        {
            if (!ordMap.ContainsKey(fld))
                ordMap.Add(fld, 1);
            else
            {
                ordMap[fld] = ordMap[fld] + 1;
            }
        }

        internal static string ldFldDefEmpty(SortedList row, string fld)
        {
            if (row[fld] == null)
                return "";
            return row[fld].ToString();
        }


        internal static string ldfld_TryGetValueDfEmpy(Dictionary<string, StringValues> whereExprsObj, string k)
        {
            // 使用 TryGetValue 方法获取值
            object value;
            try
            {
                return whereExprsObj[k];

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

        public static string ldfld_TryGetValueAsStrDfEmpty(SortedList whereExprsObj, string fld)
        {
            // 使用 TryGetValue 方法获取值
            object value;
            if (whereExprsObj.ContainsKey(fld))
                if (whereExprsObj[fld] == null)
                    return "";
                else
                    return whereExprsObj[fld].ToString();
            else
                return "";
        }

        internal static string ldfld_TryGetValueAsStrDefNull(SortedList whereExprsObj, string fld)
        {
            // 使用 TryGetValue 方法获取值
            object value;
            if (whereExprsObj.ContainsKey(fld))
                if (whereExprsObj[fld] == null)
                    return null;
                else
                    return whereExprsObj[fld].ToString();
            else
                return null;

            //if (whereExprsObj.TryGetValue(k, out (StringValues)value))
            //{
            //    return (string)value;
            //}

        }

        public static void copyPropSortedListToMerchant(SortedList sortedList, Merchant merchant)
        {
            Type merchantType = typeof(Merchant);
            foreach (DictionaryEntry entry in sortedList)
            {
                try
                {


                    string key = entry.Key.ToString();
                    PropertyInfo property = merchantType.GetProperty(key);
                    if (property != null && property.CanWrite)
                    {
                        try
                        {
                            property.SetValue(merchant, Convert.ChangeType(entry.Value, property.PropertyType));
                        }
                        catch (Exception e)
                        {
                          print(e);
                        }

                    }
                }
                catch (Exception e)
                {
                   print(e);
                }
            }
        }

        public static string ldfld_TryGetValue(Dictionary<string, StringValues> whereExprsObj, string fld)
        {
            // 使用 TryGetValue 方法获取值
            object value;
            if (whereExprsObj.ContainsKey(fld))
                return whereExprsObj[fld];
            else
                return null;

            //if (whereExprsObj.TryGetValue(k, out (StringValues)value))
            //{
            //    return (string)value;
            //}

        }

        internal static void stfld_replaceKeyV(SortedList obj, string fld, object v)
        {
            if (fld == null)
                return;
            if (obj.ContainsKey(fld))
                obj[fld] = v;
            else
                obj.Add(fld, v);
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
                if (newx.Key != null)
                    arrCls.stfld_addRplsKeyV(oldList, newx.Key.ToString(), newx.Value);
                //   newList.Add(entry.Key, entry.Value);
            }

            return newList;
        }
        internal static void stfld_addRplsKeyV(SortedList SortedList1_iot, string key, SortedList objSave)
        {
            if (SortedList1_iot.ContainsKey(key))
            {
                SortedList1_iot.Remove(key.ToString());
            }
            SortedList1_iot.Add(key, objSave);
        }
        public static string getElmt(string[] array, int index)
        {
            if (index < 0 || index >= array.Length)
            {
                return "";
            }

            string element = array[index];

            return element?.Trim().ToUpper();
            //   return   array[index].Trim().ToUpper();
        }
        public static Dictionary<string, StringValues> CopySortedListToDictionary(SortedList sortedList)
        {
            Dictionary<string, StringValues> dictionary = new Dictionary<string, StringValues>();

            foreach (DictionaryEntry entry in sortedList)
            {
                string key = entry.Key as string;
                string value = entry.Value as string;

                if (key != null && value != null)
                {
                    dictionary[key] = new StringValues(value);
                }
            }

            return dictionary;
        }


       
        public static HashSet<string> arr_remove(HashSet<string> hashSet2, string v)
        {
            string[] a = v.Split(" ");
            foreach (string wd in a)
            {
                hashSet2.Remove(wd);
            }

            return hashSet2;
        }
        public static void stfld4447(SortedList SortedList1_iot, string key, object objSave)
        {
            if (SortedList1_iot.ContainsKey(key))
            {
                //remove moshi 更好，因为可能不同的类型 原来的
                SortedList1_iot.Remove(key.ToString());
            }
            SortedList1_iot.Add(key, objSave);
        }

        internal static void stfld_addRplsKeyV(SortedList listIot, string? key, object objSave)
        {
            if (listIot.ContainsKey(key))
                listIot[key] = objSave;
            else
                listIot.Add(key, objSave);
        }


        public static HashSet<string> MergeHashSets(HashSet<string> set1, HashSet<string> set2)
        {
            HashSet<string> resultSet = new HashSet<string>(set1);
            resultSet.UnionWith(set2);
            return resultSet;
        }

        internal static HashSet<string> add_elmts2hsst(HashSet<string> set, string txtWds)
        {
            string[] a = txtWds.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            foreach (string wd1 in a)
            {
              var  wd = wd1.Trim().ToUpper();
                if(wd.Length >0)  
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