using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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
        public static string serialize<T>(T obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                return System.Text.Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// 将 JSON 格式的字符串反序列化为对象。
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">要反序列化的 JSON 字符串</param>
        /// <returns>反序列化后的对象</returns>
        public static T unserialize<T>(string json)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)))
            {
                return (T)serializer.ReadObject(ms);
            }
        }
        /// <summary>
        /// 将 JSON 字符串解析为动态对象，类似于 PHP 的 json_decode 函数。
        /// </summary>
        /// <param name="jsonString">要解析的 JSON 字符串</param>
        /// <returns>解析后的动态对象</returns>
        public static dynamic json_decode(string jsonString)
        {
            return System.Text.Json.JsonSerializer.Deserialize<dynamic>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
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
            // Console.WriteLine(jsonString);
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
            // Console.WriteLine(jsonString);
            return jsonString;
        }

    }
}
