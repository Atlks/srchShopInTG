global using static mdsj.lib.bscConvert;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Linq;
using System.Xml;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;
using System.Net.Sockets;
using System.Data.Common;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Web;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace mdsj.lib
{
    internal class bscConvert
    {
        public static int ToInt146(object obj)
        {
            return Convert.ToInt32(obj);
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
        public static SortedList ConvertFormToSortedList(IFormCollection formCollection)
        {
            SortedList sortedList = new SortedList();

            foreach (string key in formCollection.Keys)
            {
                if (key != null)
                {
                    sortedList.Add(key, formCollection[key][0]);
                }
            }

            return sortedList;
        }

        public static List<object> CastToSerializableObjs(List<object> list)
        {
            List<object> listRzt = new List<object>();

            foreach (var obj in list)
            {

                listRzt.Add(CastToSerializableObj(obj));

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

        public static object CastToSerializableObj(object obj)
        {
            if (obj is HttpRequest)
            {
                //     var req= 
                var req = (HttpRequest)obj;
                //Hashtable hs = new Hashtable();
                //hs.Add("name",nameof(HttpRequest));
                //hs.Add("url",req.Path+req.QueryString);
                //   maybe qrystr

                return $"@HttpRequest:  {req.Scheme}://{req.Host}" + DecodeUrl(req.Path) + req.QueryString;
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

        public static string ToStr(object val)
        {
            if (val == null)
                return "";
            else
                return val.ToString();
        }
        public static double ToDouble(object obj, double def)
        {
            if (obj == null)
            {
                return def;
            }

            if (obj is double)
            {
                return (double)obj;
            }
            if (obj is float)
            {
                return Convert.ToDouble((float)obj);
            }
            if (obj is int)
            {
                return Convert.ToDouble((int)obj);
            }
            if (obj is long)
            {
                return Convert.ToDouble((long)obj);
            }
            if (obj is decimal)
            {
                return Convert.ToDouble((decimal)obj);
            }
            if (obj is string)
            {
                if (double.TryParse((string)obj, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
                {
                    return result;
                }
            }

            return def;
        }
        public static List<string> ConvertAndSortHashSet(HashSet<string> hashSet)
        {
            // 将 HashSet 转换为 List
            List<string> list = new List<string>(hashSet);

            // 对 List 进行排序
            list.Sort();

            return list;
        }
        public static object CastToSerializableObjsOrSnglobj(object inputArray)
        {
            //todo 
            // if sortedlist hashtable ne?? dic 
            if (IsStr(inputArray))
                return inputArray;
            if (!isArrOrColl(inputArray))
                return CastToSerializableObj(inputArray);
            // 创建一个新的 List 用于存储元素
            List<object> list = new List<object>();
            if (isArrOrColl(inputArray))
            {
                list = CastArrCollToList(inputArray);
            }
            List<object> listRzt = CastToSerializableObjs(list);
            return listRzt;
        }

        public static string CastNormalizePath(string path)
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
        private static List<object> CastArrCollToList(object inputArray)
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


        public static string ToString(object o)
        {
            if (o == null)
                return "";
            // The default for an object is to return the fully qualified name of the class.
            return o.ToString();
        }
        public static Hashtable ToHashtable(JObject jsonObject)
        {
            Hashtable hashtable = new Hashtable();

            foreach (var property in jsonObject.Properties())
            {
                hashtable[property.Name] = property.Value.ToObject<object>();
            }

            return hashtable;
        }
        public static Hashtable ToLower(Hashtable dic)
        {
            ConvertKeysToLowercase(dic);
            return dic;
        }
        public static int ToInt(object obj)
        {
            return Convert.ToInt32(obj);
        }
        public static Dictionary<string, string> ConvertToDictionary(string queryString)
        {
            var dictionary = new Dictionary<string, string>();

            // Use HttpUtility to parse the query string
            var queryParams = HttpUtility.ParseQueryString(queryString);

            foreach (string key in queryParams)
            {
                dictionary[key] = queryParams[key];
            }

            return dictionary;
        }
        public static string ConvertToMarkdownTable(object arr2)
        {
            object[] arr = (object[])arr2;
            StringBuilder sb = new StringBuilder();

            // 添加表头
            sb.AppendLine("| prm\t|Value\t|");
            sb.AppendLine("|-------|-------|");

            // 添加表格行
            for (int i = 0; i < arr.Length; i++)
            {
                object obj = arr[i];
                sb.AppendLine($"| {i}\t|{encodeJson(obj)}\t|");
            }

            return sb.ToString();
        }

        public static Hashtable ConvertToHashtable(string queryString)
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

        public static string castToStr(object args)
        {
            return args.ToString();
        }

        public static double ConvertStringToNumber(object str2)
        {
            try
            {
                string str = ToStr(str2);

                return (double.Parse(str));
            }
            catch (Exception e)
            {
                return 0;
            }


        }

        public static void ConvertXmlToHtml(string xmlFilePath, string htmlFilePath)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<html>");
            sb.AppendLine("<head><title>XML Documentation</title></head>");
            sb.AppendLine("<body>");
            sb.AppendLine("<h1>api文档</h1>");

            using (var reader = XmlReader.Create(xmlFilePath))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "member")
                    {
                        string name = reader.GetAttribute("name");
                        sb.AppendLine($"<h2>{name}</h2>");

                        while (reader.Read() && !(reader.NodeType == XmlNodeType.EndElement && reader.Name == "member"))
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                string elementName = reader.Name;
                                if (elementName == "example")
                                {
                                    reader.Read(); // Move to the text node or CDATA
                                    if (reader.NodeType == XmlNodeType.CDATA || reader.NodeType == XmlNodeType.Text)
                                    {
                                        string exampleText = reader.Value;
                                        sb.AppendLine($"<p><strong>范例:</strong> {exampleText}</p>");
                                    }
                                }
                                else
                                {//summar
                                    string prmname = reader.GetAttribute("name");
                                    SortedList stlst = new SortedList();
                                    stlst.Add("summary", "功能");

                                    stlst.Add("returns", "返回值");
                                    stlst.Add("param", "----参数");
                                    reader.Read(); // Move to the text node

                                    //  if (reader.NodeType == XmlNodeType.Text)
                                    {
                                        string text = reader.Value;

                                        sb.AppendLine($"<p><strong>{stlst[elementName] + "  " + prmname}:</strong> {text}</p>");
                                    }
                                }

                            }
                        }
                    }
                }
            }

            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            System.IO.File.WriteAllText(htmlFilePath, sb.ToString());
        }
        public static string ConvertXmlToJson(string xmlFilePath)
        {
            XDocument doc = XDocument.Load(xmlFilePath);
            return JsonConvert.SerializeXNode(doc, Newtonsoft.Json.Formatting.Indented, true);
        }
    }
}
