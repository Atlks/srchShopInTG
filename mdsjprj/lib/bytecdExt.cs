global using static mdsj.lib.bytecdExt;
using libx;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using prj202405.lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace mdsj.lib
{
    internal class bytecdExt
    {
        public static string[] ReadAllLines(string filePath)
        {
            return System.IO.File.ReadAllLines(filePath);
        }

        public static Hashtable getHstbFromIniFl(string v)
        {
            return ReadIniFile(v);
        }

        public static Hashtable ReadIniFile(string filePath)
        {
            Hashtable iniData = new Hashtable();

            // 按行读取 INI 文件内容
            string[] lines = ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();

                // 忽略空行和注释行
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith(";") || trimmedLine.StartsWith("#"))
                    continue;

                // 处理键值对行
                int equalIndex = trimmedLine.IndexOf('=');
                if (equalIndex > 0)
                {
                    string key = trimmedLine.Substring(0, equalIndex).Trim();
                    string value = trimmedLine.Substring(equalIndex + 1).Trim();
                    iniData[key] = value;
                }
            }

            return iniData;
        }


        public static List<string> ldLstWdsFrmDataDirHtml(string FolderPath)
        {

            object v = callx(ExtractWordsFromFilesHtml, FolderPath);
            HashSet<string> weds = (HashSet<string>)v;
            weds = RemoveElementsContainingNumbers(weds);
            var wds = ConvertAndSortHashSet(weds);
            WriteAllText("word7000dep.json", wds);
            return wds;

        }


        public static List<string> ConvertAndSortHashSet(HashSet<string> hashSet)
        {
            // 将 HashSet 转换为 List
            List<string> list = new List<string>(hashSet);

            // 对 List 进行排序
            list.Sort();

            return list;
        }

        public static HashSet<string> RemoveWordsStartingWithDigit(HashSet<string> words)
        {
            // 使用 LINQ 和正则表达式过滤掉数字开头的单词
            return new HashSet<string>(words.Where(word => !Regex.IsMatch(word, @"^\d")));
        }
        public static HashSet<string> RemoveElementsWithShortLength(HashSet<string> hashSet)
        {
            // 使用 LINQ 过滤掉长度小于 3 的元素
            return new HashSet<string>(hashSet.Where(word => word.Length >= 3));
        }
        public static Hashtable ToLower(Hashtable dic)
        {
            ConvertKeysToLowercase(dic);
            return dic;
        }
        public static List<string> getListFrmFil(string v)
        {
            return ReadFileToLines(v);
        }

        public static List<string> ReadFileToLines(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines;
        }

        public static string JoinStringsWithNewlines(List<string> stringList)
        {

            if (stringList == null || stringList.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();
            foreach (string item in stringList)
            {
                builder.Append(item);
                builder.AppendLine();
            }

            // Remove the last newline character
            builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }

        public static bool StartsWithUppercase(string input)
        {
            // 判断字符串是否为空或为null
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            // 检查第一个字符是否为大写字母
            return char.IsUpper(input[0]);
        }
        public static bool IsWord(string input)
        {
            // 使用 Char 类的方法判断是否是字母或数字
            return input.All(c => char.IsLetter(c));
        }
        public static void foreach_listStr(List<string> li, Action<string> value)
        {
            foreach (string line in li)
            {
                value(line);
            }
        }
        public static void ConsoleWriteLine(string v)
        {
            System.Console.WriteLine(v);
        }


        public static string DecodeUrl(string path)
        {
            string decodedUrl = WebUtility.UrlDecode(path);
            return decodedUrl;
        }
        public static void WaitTaskExecFinish(System.Threading.Tasks.Task 结果task)
        {
            结果task.Wait();
        }

        public static SortedList ConvertFormToSortedList(IFormCollection formCollection)
        {
            SortedList sortedList = new SortedList();

            foreach (string key in formCollection.Keys)
            {
                if (key != null)
                {
                    sortedList.Add(key, formCollection[key]);
                }
            }

            return sortedList;
        }

        public static void SetRespContentTypeNencode
            (HttpResponse http上下文, string 内容类型和编码)
        {
            http上下文.ContentType = 内容类型和编码;
        }
        public static void SendResp(object 输出结果, HttpResponse http上下文)
        {
            http上下文.WriteAsync(输出结果.ToString(), Encoding.UTF8).GetAwaiter().GetResult(); ;

        }
        public static void SendResp(object 输出结果, string 内容类型和编码,HttpResponse http上下文)
        {
            http上下文.ContentType = 内容类型和编码;
           
            http上下文.WriteAsync(输出结果.ToString(), Encoding.UTF8).GetAwaiter().GetResult(); ;

        }
        public static object castToSerializableObjsOrSnglobj(object inputArray)
        {
            if (IsStr(inputArray))
                return inputArray;
            if (!isArrOrColl(inputArray))
                return castToSerializableObj(inputArray);
            // 创建一个新的 List 用于存储元素
            List<object> list = new List<object>();
            if (isArrOrColl(inputArray))
            {
                list= castArrCollToList(inputArray);
            }         
            List<object> listRzt = castToSerializableObjs(list);
            return listRzt;
        }

        private static List<object> castArrCollToList(object inputArray )
        {
            List<object> list = new List<object>();
            // 检查对象是否为数组
            if (IsArray(inputArray))
            {
                List<object> li1 = ConvertArrayToList(inputArray);
             //   list.AddRange(li1);
                return li1;
            }
            // 检查对象是否为集合
            if (IsCollection(inputArray))
            {
                List<object> li1 = ConvertCollectionToList(inputArray);
             //   list.AddRange(li1);
                return li1;
            }
            return list;
        }

        private static bool isArrOrColl(object inputArray)
        {
            if  ( IsArray(inputArray))
                   return true;
            if(IsCollection(inputArray))
                return true;

            return false;
        }

        private static List<object> castToSerializableObjs(List<object> list)
        {
            List<object> listRzt = new List<object>();

            foreach (var obj in list)
            {

                listRzt.Add(castToSerializableObj(obj));

            }
            return listRzt;
        }

        public static List<object> ConvertArrayToList(object obj)
        {
            // 创建一个新的 List 用于存储元素
            List<object> list = new List<object>();

            // 检查对象是否为数组
            if (obj is Array array)
            {
                // 循环遍历数组的每个元素并添加到 List 中
                foreach (var element in array)
                {
                    list.Add(element);
                }
            }

            return list;
        }
        public static List<object> ConvertCollectionToList(object obj)
        {
            // 创建一个新的 List 用于存储元素
            List<object> list = new List<object>();

            // 检查对象是否为集合 (实现了 IEnumerable 接口)
            if (obj is IEnumerable collection)
            {
                // 循环遍历集合的每个元素并添加到 List 中
                foreach (var element in collection)
                {
                    list.Add(element);
                }
            }

            return list;
        }

        private static object castToSerializableObj(object obj)
        {
            if (obj is HttpRequest)
            {
                //     var req= 
                var req = (HttpRequest)obj;
                //Hashtable hs = new Hashtable();
                //hs.Add("name",nameof(HttpRequest));
                //hs.Add("url",req.Path+req.QueryString);
                //   maybe qrystr
              
                return $"@HttpRequest:  {req.Scheme}://{req.Host}" +DecodeUrl( req.Path) + req.QueryString;
            }
            if (obj is HttpResponse)
            {
                return nameof(HttpResponse);
            }
            // 过滤掉各种会导致序列化 JSON 出错的对象类型
            if (obj is HttpContext)
                return false;
            if (obj is DbConnection)
                return false;
            if (obj is Stream || obj is StreamReader || obj is StreamWriter)
                return false;
            if (obj is NetworkStream || obj is TcpClient || obj is TcpListener)
                return false;
            if (obj is Delegate)
                return false;
            if (obj is IntPtr || obj is UIntPtr)
                return false;

            return obj;
        }
        public static bool isStrEndWz(string 路径, string 扩展名)
        {
            return 路径.ToUpper().Trim().EndsWith("." + 扩展名.Trim().ToUpper());
        }
        public static bool isPathEndwithExt(string 路径, string 扩展名)
        {
            return 路径.ToUpper().Trim().EndsWith("." + 扩展名.Trim().ToUpper());
        }
        public static void SendRespsJJJresNotExist404(HttpResponse HTTP响应对象)
        {
            const string 提示 = "file not find文件没有找到";
            HTTP响应对象.StatusCode = (int)HttpStatusCode.NotFound;
            StreamWriter writer = new StreamWriter(HTTP响应对象.Body);
            writer.Write(提示);
        }
        public static string 格式化路径2(string 路径)
        {
            return castNormalizePath(路径);

        }
        public static bool fileHasExtname(string 路径)
        {
            string 文件扩展名 = Path.GetExtension(路径);
            //  string 文件路径 = $"{web根目录}{路径}";
            //   文件路径 = 格式化路径(文件路径);
            if (文件扩展名 == "")
                return false;
            else
                return true;
        }
        public static bool isFileExist(string 文件路径)
        {
            return ExistFil(文件路径);
        }
        public static bool isFileNotExist(string 文件路径)
        {
            return !ExistFil(文件路径);
        }
        public static string castNormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return "";
                //   throw new ArgumentException("Path cannot be null or empty.", nameof(path));
            }

            // Replace all forward slashes with backslashes
            string normalizedPath = path.Replace('/', '\\');

            // Remove redundant backslashes
            normalizedPath = Regex.Replace(normalizedPath, @"\\+", "\\");

            return normalizedPath;
        }
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
            }
            catch (Exception e)
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

        public static List<SortedList> LdHstbEsFrmDbf(string dbFileName)
        {


            // setDbgFunEnter(__METHOD__, func_get_args());
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.print_call_FunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), dbFileName));

            if (!File.Exists(dbFileName))
                File.WriteAllText(dbFileName, "[]");

            // 将JSON字符串转换为List<Dictionary<string, object>>
            string txt = File.ReadAllText(dbFileName);
            if (txt.Trim().Length == 0)
                txt = "[]";
            var list = JsonConvert.DeserializeObject<List<SortedList>>(txt);

            //   ArrayList list = (ArrayList)JsonConvert.DeserializeObject(File.ReadAllText(dbFileName));

            // 获取当前方法的信息
            //MethodBase method = );

            //// 输出当前方法的名称
            //Console.WriteLine("Current Method Name: " + method.Name);
            dbgCls.print_ret(MethodBase.GetCurrentMethod().Name, array_slice(list, 0, 1));

            return list;
        }
        public static List<object> foreach_hstbEs(List<SortedList> list2, Func<SortedList, object> act)
        {
            List<object> listRzt = new List<object>();
            foreach (SortedList rw in list2)
            {
                try
                {
                    listRzt.Add(act(rw));
                }
                catch (Exception e)
                {
                    print_catchEx(nameof(foreach_hstbEs), e);
                }

            }
            return listRzt;
        }


        public static bool isIn(object hour, string[]? times)
        {
            foreach (string o in times)
            {
                if (hour.ToString().ToUpper().Equals(o.ToUpper()))
                    return true;
            }
            return false;
        }
        public static string getFld(JObject? jo, string fld, string v2)
        {
            // 获取 chat.type 属性
            JToken chatTypeToken = jo.SelectToken(fld);

            if (chatTypeToken != null)
            {
                string chatType = chatTypeToken.ToString();
                return chatType;
                // print("chat.type: " + chatType);
            }
            else
            {
                return v2;
            }
        }
        public static SortedList<string, string> LdHstbEsFrmJsonFile(string v)
        {
            return ReadJsonFileToSortedList(v);
        }
        public static int count(object collection)
        {
            return 计算长度(collection);
        }
        public static SortedList<string, string> LdHstbEsFrmIni(string v)
        {
            return ReadIniFileToSortedList(v);
        }

        public static bool IsLetter(char character)
        {
            return (character >= 'A' && character <= 'Z') || (character >= 'a' && character <= 'z');
        }

        public static bool IsEnglishLetter(char character)
        {
            return (character >= 'A' && character <= 'Z') || (character >= 'a' && character <= 'z');
        }

        public static List<Hashtable> foreach_listHstb(List<Hashtable> list, Action<Hashtable> act)
        {
            // List<Hashtable> listRzt = new List<object>();
            foreach (Hashtable rw in list)
            {
                try
                {
                    act(rw);
                }
                catch (Exception e)
                {
                    print_catchEx(nameof(foreach_listHstb), e);
                }

            }
            return list;
        }

        public static List<Hashtable> getListFrmDir(string directoryPath)
        {
            List<Hashtable> fileList = new List<Hashtable>();

            // 获取目录中所有 JSON 文件的路径
            string[] jsonFiles = Directory.GetFiles(directoryPath, "*.json");

            foreach (string filePath in jsonFiles)
            {
                try
                {
                    // 读取文件内容
                    string jsonContent = ReadAllText(filePath);

                    // 解析 JSON 文件为 JObject
                    JObject jsonObject = JObject.Parse(jsonContent);

                    // 转换为 Hashtable
                    Hashtable hashtable = ToHashtable(jsonObject);
                    hashtable.Add("fname", Path.GetFileName(filePath));
                    hashtable.Add("fpath", filePath);
                    // 获取文件名（不包括扩展名）
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                    hashtable.Add("basename", fileNameWithoutExtension);
                    // 添加到 List 中
                    fileList.Add(hashtable);
                }
                catch (Exception ex)
                {
                    ConsoleWriteLine($"Error reading or parsing file {filePath}: {ex.Message}");
                }
            }

            return fileList;
        }
        // 将 JObject 转换为 Hashtable
        public static Hashtable ToHashtable(JObject jsonObject)
        {
            Hashtable hashtable = new Hashtable();

            foreach (var property in jsonObject.Properties())
            {
                hashtable[property.Name] = property.Value.ToObject<object>();
            }

            return hashtable;
        }
        public static void encodeurl()
        {
        //    encodeJsonNofmt
        }
        public static void arr_join()
        {

        }
        public static void run()
        {

        }
        public static void exec()
        {

        }
        public static void file_rw()
        {

        }
        public static void str_join()
        {

        }


        public static void foreach_hstbEs(List<SortedList> list2, Action<SortedList> act)
        {
            foreach (SortedList rw in list2)
            {
                try
                {
                    act(rw);
                }
                catch (Exception e)
                {
                    print_catchEx(nameof(foreach_hstbEs), e);
                }

            }
        }
        public static string ldfldDfempty(Dictionary<string, StringValues> whereexprsobj, string v)
        {
            var x = ldfld(ConvertToStringDictionary(whereexprsobj), v);
            return x;
        }

        public static string Substring(string queryString, int v)
        {
            if (queryString == "")
                return "";
            return queryString.Substring(v);
        }

        public static bool isChkfltrOk(List<bool> li)
        {
            if (!ChkAllFltrTrue(li))
                return false;
            return true;
        }
        public static Dictionary<string, string> ConvertToStringDictionary(Dictionary<string, StringValues> input)
        {
            var result = new Dictionary<string, string>();

            foreach (var kvp in input)
            {
                result[kvp.Key] = kvp.Value.ToString();
            }

            return result;
        }
        public static List<SortedList> SliceX<SortedList>(List<SortedList> list, int start, int length)
        {
            if (start < 0) start = 0;
            if (start >= list.Count) return new List<SortedList>();

            if (length < 0) length = 0;
            if (start + length > list.Count) length = list.Count - start;

            return list.GetRange(start, length);
        }
        public static bool ExistFil(string path1)
        {
            return File.Exists(path1);
        }
        public static Dictionary<string, string> ldDic4qryCdtn(string qrystr)
        {
            var filters2 = ldDicFromQrystr(qrystr);
            var filters = RemoveKeys(filters2, "page limit pagesize from");
            return filters;
        }
        public static void foreach_DictionaryKeys(Dictionary<string, string> dictionary, Action<string> keyAction)
        {
            foreach (var key in dictionary.Keys)
            {
                keyAction(key);
            }
        }


        public static bool isFldValEq111(SortedList row, string Fld, Dictionary<string, StringValues> whereExprsObj)
        {
            //  string Fld = "城市";
            if (hasCondt(whereExprsObj, Fld))
                if (!strCls.str_eq(row[Fld], arrCls.ldfld_TryGetValue(whereExprsObj, Fld)))   //  cityname not in (citysss) 
                    return false;

            return true;
        }

        public static bool isFldValEq111(SortedList row, string Fld, Dictionary<string, string> whereExprsObj)
        {
            //  string Fld = "城市";
            if (hasCondt(whereExprsObj, Fld))
                if (!strCls.str_eq(row[Fld], ldfld(whereExprsObj, Fld)))   //  cityname not in (citysss) 
                    return false;

            return true;
        }

        public static object ldfldDfemp(SortedList row, string v)
        {
            return ldfld(row, v, "");
        }

        public static object ldfld(SortedList row, string fld, string dfv)
        {
            if (row.ContainsKey(fld))
                return row[fld];
            return dfv;
        }

        public static bool isEq4qrycdt(object rowVal, object cdtVal)
        {
            if (cdtVal == null || cdtVal.ToString() == "")
                return true;
            return rowVal.ToString().ToUpper().Equals(cdtVal.ToString().ToUpper());
        }
        public static bool isChkOK(List<Filtr> li)
        {
            if (!ChkAllFltrTrueDep(li))
                return false;
            return true;
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
        public static int len(object obj)
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
