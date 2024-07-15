global using static mdsj.lib.bytecdExt;
using libx;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using prj202405.lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace mdsj.lib
{
    internal class bytecdExt
    {
        public static void ConsoleWriteLine(string v)
        {
            System.Console.WriteLine(v);
        }
      
      
        public static string decodeUrl(string path)
        {
            string decodedUrl = WebUtility.UrlDecode(path);
            return decodedUrl;
        }
        public static void WaitTaskExecFinish(System.Threading.Tasks.Task 结果task)
        {
            结果task.Wait();
        }
        public static void SetRespContentTypeNencode
            (HttpContext http上下文, string 内容类型和编码)
        {
            http上下文.Response.ContentType = 内容类型和编码;
        }
        public static void SendResp(object 输出结果, HttpContext http上下文)
        {
            http上下文.Response.WriteAsync(输出结果.ToString(), Encoding.UTF8).GetAwaiter().GetResult(); ;

        }
        public static bool isStrEndWz(string 路径, string 扩展名)
        {
            return 路径.ToUpper().Trim().EndsWith("." + 扩展名.Trim().ToUpper());
        }
        public static bool isPathEndwithExt(string 路径, string 扩展名)
        {
            return 路径.ToUpper().Trim().EndsWith("." + 扩展名.Trim().ToUpper());
        }
        public static void sendResp_resNotExist404(HttpResponse HTTP响应对象)
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
            return existFil(文件路径);
        }
        public static bool isFileNotExist(string 文件路径)
        {
            return !existFil(文件路径);
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
        public static List<object> foreach_hstbEs(List<SortedList> list2, Func<SortedList,object> act)
        {
            List<object> listRzt = new List<object>();
            foreach (SortedList rw in list2)
            {
                try
                {
                    listRzt.Add( act(rw));
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
                    hashtable.Add("fname", Path.GetFileName( filePath));
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
                }catch(Exception e)
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
        public static bool existFil(string path1)
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
