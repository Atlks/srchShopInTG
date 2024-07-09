using Mono.Web;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
 
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
// mdsj.lib.encdCls
namespace mdsj.lib
{
    internal class encdCls
    {
        /// <summary> 们可以编写类似于 PHP 中的 unserialize 和 serialize 函数。
        /// 这两个函数通常用于将对象序列化为字符串（serialize）和将字符串反序列化为对象（unserialize）。
        /// 在 C# 中，可以使用 JSON 序列化和反序列化来实现类似的功能。下面是示例代码
        /// 将对象序列化为 JSON 格式的字符串。
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>序列化后的字符串</returns>
        //public static string serialize<T>(T obj)
        //{
        //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        serializer.WriteObject(ms, obj);
        //        return System.Text.Encoding.UTF8.GetString(ms.ToArray());
        //    }
        //}

        public static string RemoveExtraNewlines(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            // Use regular expression to replace multiple consecutive newline characters with a single newline
            string result = Regex.Replace(input, @"(\r\n|\r|\n)+", Environment.NewLine);

            return result;
        }
        /// <summary>
        /// Removes HTML tags from a string.
        /// </summary>
        /// <param name="html">The input string containing HTML.</param>
        /// <returns>A string with HTML tags removed.</returns>
        public static string strip_tags(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return string.Empty;
            }


            // Remove <script> tags and their content
            string noScript = Regex.Replace(html, "<script.*?>.*?</script>", string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            // Regular expression to match HTML tags
            string pattern = "<.*?>";

            // Replace HTML tags with an empty string
            //    string result = Regex.Replace(noScript, pattern, string.Empty);
            // Replace HTML tags with an empty string
            string result = Regex.Replace(noScript, pattern, string.Empty, RegexOptions.Singleline);

            result = RemoveExtraNewlines(result);
            result = RemoveInvisibleCharacters(result);
            result = delEmpltyLines(result);

            result = RemoveExtraNewlines(result); result = RemoveExtraNewlines(result);
            //result = RemoveExtraNewlines(result); result = RemoveExtraNewlines(result);
            return result;
        }

        private static string delEmpltyLines(string result)
        {
            string[] lines = result.Split('\n'); //var//Install-Package SQLitePCLRaw.core
            lines = delEmptyLines(lines);
            //for (int i = 0; i < lines.Length; i++)
            //{
            //    if (i < 2)
            //        continue;
            //    string line = lines[i];
            //    line = line.Trim();
            //    char[] charr= line.ToCharArray();

            //}
            result = string.Join("\n", lines);
            return result;
        }

        /// <summary>
        /// Trims each element in the input string array and returns a new array.
        /// </summary>
        /// <param name="lines">The input string array.</param>
        /// <returns>A new string array with each element trimmed.</returns>
        /// <summary>
        /// Trims non-empty elements in the input string array and returns a new array.
        /// </summary>
        /// <param name="lines">The input string array.</param>
        /// <returns>A new string array with trimmed non-empty elements.</returns>
        public static string[] delEmptyLines(string[] lines)
        {
            if (lines == null)
            {
                return new string[0];
            }

            // Use LINQ Select to trim non-empty elements in the input array
            string[] trimmedArray = lines
                .Where(line => !string.IsNullOrWhiteSpace(line)) // Filter out empty or whitespace lines
                .Select(line => line) // Trim each non-empty line
                .ToArray();

            return trimmedArray;
        }

        /// <summary>
        /// Removes all non-visible characters from the input string except for carriage return, newline, and space.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>A string with only visible characters, spaces, carriage return, and newline.</returns>
        public static string RemoveInvisibleCharacters(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            // Define the regular expression pattern to match all non-printable characters
            // except for space (\x20), carriage return (\x0D), and newline (\x0A).
            string pattern = @"[^\x20-\x7E\x0A\x0D]";

            // Replace matched characters with an empty string
            string result = Regex.Replace(input, pattern, string.Empty);


            // Define the regular expression pattern to match tab characters (\t)
            string pattern2 = @"\t";

            // Replace matched tab characters with an empty string
            result = Regex.Replace(result, pattern2, string.Empty);

            return result;
        }
        //我们首先使用 System.IO.Path.GetInvalidFileNameChars 方法获取操作系统支持的非法文件名字符数组
        /*
         
         我们遍历输入的文本，并检查每个字符是否是非法字符。如果字符是非法字符，则使用 HttpUtility.UrlEncode 方法对字符进行 URL 编码，然后将编码后的结果添加到结果字符串中。最后，返回处理后的结果字符串。
         
         */
        public static string ConvertToValidFileName(string input)
        {
            // URL 编码非法字符
            string invalidChars = new string(System.IO.Path.GetInvalidFileNameChars());
            StringBuilder encodedBuilder = new StringBuilder();
            foreach (char c in input)
            {
                if (invalidChars.Contains(c))
                {
                    // 如果字符为非法字符，则使用 URL 编码替换
                    string encoded = HttpUtility.UrlEncode(c.ToString());
                    encodedBuilder.Append(encoded);
                }
                else
                {
                    // 如果字符为合法字符，则直接添加到结果中
                    encodedBuilder.Append(c);
                }
            }
            return encodedBuilder.ToString();
        }

        /// <summary>
        /// 将 JSON 格式的字符串反序列化为对象。
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">要反序列化的 JSON 字符串</param>
        /// <returns>反序列化后的对象</returns>
        //public static T unserialize<T>(string json)
        //{
        //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
        //    using (MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)))
        //    {
        //        return (T)serializer.ReadObject(ms);
        //    }
        //}
        /// <summary>
        /// 将 JSON 字符串解析为动态对象，类似于 PHP 的 json_decode 函数。
        /// </summary>
        /// <param name="jsonString">要解析的 JSON 字符串</param>
        /// <returns>解析后的动态对象</returns>
        public static List<SortedList> json_decode(string jsonString)
        {
            return JsonConvert.DeserializeObject< List<SortedList>>(jsonString );
        }

        public static t json_decode<t>(string jsonString)
        {
            return JsonConvert.DeserializeObject<t>(jsonString);
        }
        public static string json_encode(object results)
        {
            //   options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            };
            //  string json = JsonConvert.SerializeObject(obj, settings);
            string jsonString = JsonConvert.SerializeObject(results, settings);
            //print(jsonString);
            return jsonString;
        }

        public static string json_encode_noFmt(object results)
        {
            //   options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                //  Formatting = Formatting.Indented
            };
            //  string json = JsonConvert.SerializeObject(obj, settings);
            string jsonString = JsonConvert.SerializeObject(results, settings);
            //print(jsonString);
            return jsonString;
        }

    }
}
