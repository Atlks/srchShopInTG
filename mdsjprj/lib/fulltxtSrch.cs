using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static mdsj.lib.dtime;
using static mdsj.lib.fulltxtSrch;
using static mdsj.biz_other;
using static mdsj.clrCls;
using static prjx.timerCls;


using static mdsj.lib.exCls;
using static prjx.lib.arrCls;//  prj202405.lib
using static prjx.lib.dbgCls;
using static mdsj.lib.logCls;
using static prjx.lib.corex;
using static prjx.lib.db;
using static prjx.lib.filex;
using static prjx.lib.ormJSonFL;
using static prjx.lib.strCls;
using static mdsj.lib.bscEncdCls;
using static mdsj.lib.net_http;
using static mdsj.lib.dsl;
using static mdsj.lib.util;
using static libx.storeEngr4Nodesqlt;
using JiebaNet.Segmenter;
using prjx.lib;
using System.Reflection;
using Newtonsoft.Json.Linq;
using prjx;
namespace mdsj.lib
{
    internal class fulltxtSrch
    {

        public static List<SortedList> qry_ContainMatch(string fldIdx_dataDir, string shareNameS_matchKwds,
              Func<SortedList, bool> whereFun = null,
                    Func<SortedList, int> ordFun = null,
                    Func<SortedList, object> selktFun = null
            )
        {
            //存储引擎部分。应该需要注入吧

            //   List<Dictionary<string, object>> maparr = ReadJSONFileToMapArray(fpath);

            Func<string, List<SortedList>> rndFun = (dbf) =>
            {
                return rnd4jsonFl(dbf);
            };

            return qry_ContainMatch(fldIdx_dataDir, shareNameS_matchKwds, rndFun);

        }

        //这里分片读取，做join操作。。以前的qery是做union操作的。。
        //  ContainMatch(string dataDir,string CONTAINS_fld  数据库思维
        //索引文件模式的化，直接所谓datadir就是主记录。。
        //类似查询引擎  datadir,where order fun, storeEngr
        //   ContainMatch
        public static List<SortedList> qry_ContainMatch(string fldIdx_dataDir, string matchKwds, Func<string, List<SortedList>> rndFun)
        {
            // Split the matchKwds string into individual keywords
            string[] sharsStrarrFmt = SplitAndTrim(matchKwds);

            List<SortedList> result = new List<SortedList>();
            bool needini = true;

            // Iterate through each keyword
            foreach (var shareNm1 in sharsStrarrFmt)
            {
                string fpath = Path.Combine(fldIdx_dataDir, $"{shareNm1}");
                //List<SortedList> rnd4jsonFl(string dbf)
                List<SortedList> maparr = rndFun(fpath);
                if (needini)
                {
                    result = maparr;
                    needini = false;
                }
                else
                {
                    result = IntersectArrays(result, maparr);
                }
            }

            return result;
        }

        // Function to read JSON file and return List of Dictionary<string, object>
        //public static List<Dictionary<string, object>> ReadJSONFileToMapArray(string filePath)
        //{
        //    if (!File.Exists(filePath))
        //    {
        //        return new List<Dictionary<string, object>>(); // Return empty list if file doesn't exist
        //    }

        //    string jsonContent = File.ReadAllText(filePath);
        //    JArray jsonArray = JArray.Parse(jsonContent);

        //    List<Dictionary<string, object>> mapArray = new List<Dictionary<string, object>>();

        //    foreach (JObject obj in jsonArray)
        //    {
        //        Dictionary<string, object> dict = obj.ToObject<Dictionary<string, object>>();
        //        mapArray.Add(dict);
        //    }

        //    return mapArray;
        //}

        // Function to compute intersection of two List of Dictionary<string, object>
        public static List<SortedList> IntersectArrays(List<SortedList> arr1, List<SortedList> arr2)
        {
            List<SortedList> result = new List<SortedList>();

            // Create a dictionary for arr1 elements with id as key
            Dictionary<string, SortedList> map1 = new Dictionary<string, SortedList>();
            foreach (var m in arr1)
            {
                string id = m["id"].ToString();
                map1[id] = m;
            }

            // Iterate through arr2 and add to result if exists in map1
            foreach (var m in arr2)
            {
                string id = m["id"].ToString();
                if (map1.ContainsKey(id))
                {
                    result.Add(m);
                }
            }

            return result;
        }

        // Deep comparison function for dictionaries
        public static bool DeepCompare(Dictionary<string, object> dict1, Dictionary<string, object> dict2)
        {
            // Implement your own deep comparison logic here
            // Example uses reference equality for simplicity
            return object.ReferenceEquals(dict1, dict2);
        }

        //遍历输入字符串中的每个字符，检查它是否在标点符号集合中。
        public static bool IsAllPunctuation(string input)
        {
            HashSet<char> punctuationChars = new HashSet<char>()
        {
            '!', '"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/', ':', ';', '<', '=', '>', '?', '@', '[', '\\', ']', '^', '_', '`', '{', '|', '}', '~',
            '。', '，', '、', '；', '：', '？', '！', '“', '”', '‘', '’', '（', '）', '【', '】', '《', '》'
        };

            foreach (char c in input)
            {
                if (!punctuationChars.Contains(c))
                {
                    return false;
                }
            }

            return true;
        }
        public static bool IsNumeric(string input)
        {
            // 尝试解析字符串为双精度浮点数
            return double.TryParse(input, out _);
        }

        public static void wrtRowss_ReadAndCreateIndex4tgmsg(string directoryPath_msg)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), directoryPath_msg));

            try
            {
                // 获取目录中所有的 JSON 文件
                string[] jsonFiles = System.IO.Directory.GetFiles(directoryPath_msg, "*.json");

                foreach (string jsonFile in jsonFiles)
                {
                    // 读取 JSON 文件内容
                    string jsonContent = System.IO.File.ReadAllText(jsonFile);

                    // 解析 JSON 内容
                    string DataDir = "fullTxtSrchIdxdataDir";
                    wrt_rows4fulltxt( jsonContent, DataDir);
                }
            }
            catch (Exception ex)
            {
                ConsoleMy.print($"An error occurred: {ex.Message}");
            }
        }

        public static void wrt_rows4fulltxt( string jsonContent, string DataDir)
        {
            try
            {
                using (JsonDocument doc = JsonDocument.Parse(jsonContent))
                {
                    // 检查是否包含 "message" 属性
                    if (doc.RootElement.TryGetProperty("message", out JsonElement messageElement))
                    {
                        // 检查 "message" 属性是否为对象
                        if (messageElement.ValueKind == JsonValueKind.Object)
                        {
                            // 获取 message 对象中的 text 属性
                            if (messageElement.TryGetProperty("text", out JsonElement textElement))
                            {
                                // 输出 text 属性的值
                               Print(textElement.GetString());
                                //    string DataDir = "fullTxtSrchIdxdataDir";
                                SortedList o = tgMsg2row(messageElement, textElement);
                                var msgx = ChineseCharacterConvert.Convert.ToSimple(o["txt"].ToString());
                                o["txt"] = msgx;
                                wrt_row4tgmsgSafe(o, DataDir);
                            }

                        }
                        else
                        {
                           Print($"The 'message' property in the file {jsonContent} is not an object.");
                        }
                    }

                }

            }
            catch (Exception e)
            {
               Print(e);
            }
       }

        private static void wrt_row4tgmsgSafe(SortedList tgmsg, string DataDir)
        {
            try
            {

                wrt_row4tgmsg(tgmsg, DataDir);
            }
            catch (Exception ex) {Print(ex.ToString()); }

        }



        public static void wrt_row4tgmsg(SortedList o, string dataDir)
        {

            var segmenter = new JiebaSegmenter();
            //------------自定词
            segmenter.LoadUserDict($"{prjdir}/cfg/user_dict.txt");
            segmenter.AddWord("会所"); // 可添加一个新词
            segmenter.AddWord("妙瓦底"); // 可添加一个新词
            segmenter.AddWord("御龙湾"); // 可添加一个新词
            HashSet<string> user_dict = GetUser_dict();
            foreach (string line in user_dict)
            {
                segmenter.AddWord(line);
            }
            HashSet<string> postnKywd位置词set = ReadLinesToHashSet("位置词.txt");
            foreach (string line in postnKywd位置词set)
            {
                segmenter.AddWord(line);
            }




            IEnumerable<string> enumerable = segmenter.CutForSearch(o["txt"].ToString());
            // 使用 LINQ 的 ToArray 方法进行转换
            string[] kwds = enumerable.ToArray();

            foreach (string wd in kwds)
            {
                //------停顿词过滤  单个字的过滤
                if (wd.Length <= 1) continue;
                if (IsNumeric(wd)) continue;
                if (IsAllPunctuation(wd)) continue;
                //todo 常见没意义词的过滤 虚词过滤

                //    SortedList doc = new SortedList();

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                string msgidx = o["chatid"] + "." + o["timeStamp"] + "." + o["msgid"];
                //  o.Add("", msgidx);
                SetFieldReplaceKeyV(o, "id", msgidx);
                SetFieldReplaceKeyV(o, "msgid", msgidx);
                //  o.Add("msgid", msgidx);
                //  o.Add("kwd", wd);
                SetFieldReplaceKeyV(o, "kwd", msgidx);
                //        doc.Add("txt", o["txt"]);
                //       doc.Add("grpinfo", o);
                //       o["txt"] = "";
                ormJSonFL.wrt_row(o, $"{dataDir}/{wd}.json");

            }
        }

      

        private static SortedList tgMsg2row(JsonElement messageElement, JsonElement textElement)
        {
            SortedList o = new SortedList();
            setuNameFrmTgmsgJson(messageElement, o);
            setGrpFromTgjson(messageElement, o);
            long stmp = messageElement.GetProperty("date").GetInt64();
            o.Add("timeStamp", stmp);
            o.Add("time", ConvertUnixTimeStampToDateTime((stmp)));
            o.Add("msgid", messageElement.GetProperty("message_id").ToString());

            string msgxv1 = textElement.GetString();
            o.Add("txt", msgxv1);
            return o;
        }
        private static JsonElement setGrpFromTgjson(JsonElement messageElement, SortedList o)
        {
            JsonElement chatObj = ldfld(messageElement, "chat");
            JsonElement title = ldfld(chatObj, "title");
            o.Add("grp", title.GetString());
            o.Add("chatid", ldfld(chatObj, "id").ToString());
            return chatObj;
        }

        private static void setuNameFrmTgmsgJson(JsonElement messageElement, SortedList o)
        {
            JsonElement frmObjct = ldfld(messageElement, "from");
            JsonElement fromUname = ldfld(frmObjct, "first_name");
            o.Add("first_name", fromUname.GetString());
        }

        private static JsonElement ldfld(JsonElement messageElement, string v)
        {
            JsonElement def;
            if (messageElement.TryGetProperty(v, out def))
                return def;
            return def;
        }

    }
}
