global using static mdsj.lib.bscGet;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Web;

namespace mdsj.lib
{
    internal class bscGet
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
        public static SortedList<string, string> LoadSortedListFromIni(string filePath)
        {
            var result = new SortedList<string, string>(StringComparer.OrdinalIgnoreCase);

            // 读取文件所有行
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();

                // 跳过空行和注释行
                if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith(";") || trimmedLine.StartsWith("#"))
                {
                    continue;
                }

                // 处理键值对
                int equalsIndex = trimmedLine.IndexOf('=');
                if (equalsIndex > 0)
                {
                    string key = trimmedLine.Substring(0, equalsIndex).Trim();
                    string value = trimmedLine.Substring(equalsIndex + 1).Trim();
                    result[key] = value;
                }
            }

            return result;
        }
        public static Dictionary<string, string> ldDicFromQrystr(string queryString)
        {
            return ConvertToDictionary(queryString);
        }

        public static string ReadAllText(string f)
        {
            return System.IO.File.ReadAllText(f);
        }
        public static List<SortedList> ReadAsListHashtable(string f)
        {
            //   File
            return json_decode(System.IO.File.ReadAllText(f));
        }
        public static SortedList ldHstb
            (string f)
        {
            return json_decode<SortedList>(System.IO.File.ReadAllText(f));
        }
        public static SortedList LoadHashtable(string f)
        {
            return json_decode<SortedList>(System.IO.File.ReadAllText(f));
        }
        public static SortedList ReadAsHashtable(string f)
        {
            return json_decode<SortedList>(System.IO.File.ReadAllText(f));
        }
        public static object ReadAsObj(string f)
        {
            return json_decodeObj(System.IO.File.ReadAllText(f));
        }
        public static JsonObject ReadAsJson(string f)
        {
            return json_decodeJonObj(System.IO.File.ReadAllText(f));
        }

        public static HashSet<string> LdHsstWordsFromFile(string filePath)
        {
            var words = new HashSet<string>();

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // 拆分行中的单词，按空格和回车拆分
                        var splitWords = line.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var word in splitWords)
                        {
                            var word1 = word.Trim();
                            words.Add(word1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Print("Error reading file: " + ex.Message);
            }

            return words;
        }

        public static object GetField(object Obj, string fld, object defVal)
        {

            if (Obj is SortedList)
            {
                return LoadFieldFrmStlst((SortedList)Obj, fld, defVal.ToString());
            }
            else
            {
                return ldfld(Obj, fld, defVal);
            }
        }
        public static object getFld(object Obj, string fld, object defVal)
        {

            if (Obj is SortedList)
            {
                return LoadFieldFrmStlst((SortedList)Obj, fld, defVal.ToString());
            }
            else
            {
                return ldfld(Obj, fld, defVal);
            }
        }
        public static string ldfldAsStr(object obj, string fld, object defVal)
        {
            return ldfld(obj, fld, "").ToString();
        }

        public static object LoadField(Hashtable hstb, string fld, object defVal)
        {
            if (hstb.ContainsKey(fld))
                return hstb[fld];
            return defVal;
        }

        public static HashSet<string> LdHsstFrmFJsonDecd(string v)
        {
            return (ReadFileToHashSet(v));
        }
        public static HashSet<string> ReadFileToHashSet(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                HashSet<string> hashSet = JsonConvert.DeserializeObject<HashSet<string>>(json);
                return hashSet;
            }
            catch (Exception ex)
            {
                ConsoleWriteLine($"An error occurred: {ex.Message}");
                return new HashSet<string>();
            }
        }

        public static HashSet<string> LoadHashsetReadFileLinesToHashSet(string filePath)
        {
            HashSet<string> lines = new HashSet<string>();

            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Trim().Length > 0)
                            lines.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"The file could not be read: {e.Message}");
            }

            return lines;
        }

        public static void setFld(object Obj, string fld, object v)
        {
            if (Obj is SortedList)
            {
                SetField938((SortedList)Obj, fld, v);
            }
            else
            {
                SetProperty(Obj, fld, v);
            }

        }
        public static double GetFieldAsNumber(SortedList sortedList, string fieldName)
        {
            var obj = GetField(sortedList, fieldName, 0);
            return ToNumber(obj);
        }

        public static List<SortedList> GetListHashtableFromJsonFil(string dbf)
        {
            var list = new List<SortedList>();

            try
            {
                string json = File.ReadAllText(dbf);
                JArray jsonArray = JArray.Parse(json);

                foreach (JObject obj in jsonArray)
                {
                    var sortedList = new SortedList();

                    foreach (var property in obj.Properties())
                    {
                        sortedList.Add(property.Name, property.Value.ToObject<object>());
                    }

                    list.Add(sortedList);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"The file could not be read: {e.Message}");
            }

            return list;
        }
        public static void WriteObj(string f, object obj)
        {

            WriteAllText(f, json_encode(obj));
        }

        public static void WriteAllText(string f, object update)
        {
            var __METHOD__ = nameof(WriteAllText);
            PrintCallFunArgs(__METHOD__, func_get_args(update, f));

            try
            {
                Mkdir4File(f);
                if (IsStr(update))
                {
                    System.IO.File.WriteAllText(f, (update.ToString()));
                    return;
                }

                System.IO.File.WriteAllText(f, json_encode(update));
            }
            catch (Exception e)
            {
                PrintExcept("WriteAllText", e);
            }
            PrintRet(__METHOD__, 0);

        }

        public static void WriteAllText218(string f, object txt)
        {
            Print($" fun WriteAllText {f}");
            try
            {
                if (IsStr(txt))
                {
                    WriteAllText(f, txt.ToString());
                }
                 else
                    WriteAllText(f,  encodeJson(txt));
            }
            catch (Exception e)
            {
                PrintCatchEx("WriteAllText", e);
            }

        }

        public static void WriteAllText(string f, string txt)
        {
            Print($" fun WriteAllText {f}");
            Mkdir4File(f);
            try
            {
                System.IO.File.WriteAllText(f, txt);
            }
            catch (Exception e)
            {
                PrintCatchEx("WriteAllText", e);
            }

        }

        public static void setHsstToF(HashSet<string> downedUrl, string v)
        {
            WriteAllText(v, encodeJson(downedUrl));
        }
        public static void SetProperty(object obj, string prop, object v)
        {
            Type type = obj.GetType();
            PropertyInfo propertyInfo = type.GetProperty(prop);

            if (propertyInfo != null && propertyInfo.CanWrite)
            {
                propertyInfo.SetValue(obj, v);
            }
            else
            {
                Print("The object does not have a writable 'Name' property.");
            }
        }

        public static HashSet<string> LdHsst(string input)
        {
            // 分割字符串并转换为 HashSet
            string[] elements = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            HashSet<string> stringSet = new HashSet<string>(elements);

            return stringSet;
        }
        public static HashSet<string> LoadHashsetFrmFL(string f)
        {
            return LdHsst(ReadAllText(f));
        }

        public static object ldfld(object obj, string fld, object defVal)
        {
            Type type = obj.GetType();
            PropertyInfo propertyInfo = type.GetProperty(fld);

            if (propertyInfo != null && propertyInfo.CanWrite)
            {
                return propertyInfo.GetValue(obj);
            }
            return defVal;
        }

        public static Hashtable LoadHashtableFromQrystrDep(string queryString)
        {
            var hashtable = new Hashtable();

            // Use HttpUtility to parse the query string
            NameValueCollection queryParams = HttpUtility.ParseQueryString(queryString);

            foreach (string key in queryParams)
            {
                hashtable[key] = queryParams[key];
            }

            return hashtable;
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

        public static List<string> ldLstWdsFrmDataDirHtml(string FolderPath)
        {

            object v = callx(ExtractWordsFromFilesHtml, FolderPath);
            HashSet<string> weds = (HashSet<string>)v;
            weds = RemoveElementsContainingNumbers(weds);
            var wds = ConvertAndSortHashSet(weds);
            WriteAllText("word7000dep.json", wds);
            return wds;

        }

        public static List<string> getListFrmFil(string v)
        {
            return ReadFileToLines(v);
        }
        public static string ldfldDfempty(Dictionary<string, StringValues> whereexprsobj, string v)
        {
            var x = ldfld(ConvertToStringDictionary(whereexprsobj), v);
            return x;
        }
        public static string gettype(object obj)
        {
            return obj.GetType().ToString();
        }


        public static int gtfldInt(SortedList dafenObj, string fld, int df)
        {
            try
            {
                var obj = GetField(dafenObj, fld, df);
                return ToInt(obj);
            }
            catch (Exception e)
            {
                PrintCatchEx(nameof(gtfldInt), e);
                return df;
            }

        }

        public static List<SortedList> GetHashtabEsFrmDbf(string dbFileName)
        {


            // setDbgFunEnter(__METHOD__, func_get_args());
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            PrintCallFunArgs(__METHOD__, func_get_args(MethodBase.GetCurrentMethod(), dbFileName));

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
            PrintRet(MethodBase.GetCurrentMethod().Name, ArrSlice(list, 0, 1));

            return list;
        }


        public static string GetFld(JObject? jo, string fld, string v2)
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
        public static Dictionary<string, string> ldDic4qryCdtn(string qrystr)
        {
            var filters2 = ldDicFromQrystr(qrystr);
            var filters = RemoveKeys(filters2, "page limit pagesize from");
            return filters;
        }
        public static object ldfldDfemp(SortedList row, string v)
        {
            return LoadFieldFrmStlst(row, v, "");
        }

        public static SortedList<string, string> LdHstbEsFrmIni(string v)
        {
            return ReadIniFileToSortedList(v);
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

        public static object LoadFieldFrmStlst(SortedList row, string fld, string dfv)
        {
            if (row.ContainsKey(fld))
                return row[fld];
            return dfv;
        }



        public static string ldfld(Dictionary<string, string> whereExprsObj, string fld)
        {
            if (whereExprsObj.ContainsKey(fld))
                return whereExprsObj[fld];
            return "";
        }
    }
}
