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
using static prj202405.timerCls;


using static mdsj.lib.exCls;
using static prj202405.lib.arrCls;//  prj202405.lib
using static prj202405.lib.dbgCls;
using static mdsj.lib.logCls;
using static prj202405.lib.corex;
using static prj202405.lib.db;
using static prj202405.lib.filex;
using static prj202405.lib.ormJSonFL;
using static prj202405.lib.strCls;
using static mdsj.lib.encdCls;
using static mdsj.lib.net_http;
using static mdsj.lib.dsl;
using static mdsj.lib.util;
using JiebaNet.Segmenter;
using prj202405.lib;
using System.Reflection;
namespace mdsj.lib
{
    internal class fulltxtSrch
    {


      
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

        public static void ReadAndCreateIndex4tgmsg(string directoryPath)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), directoryPath));

            try
            {
                // 获取目录中所有的 JSON 文件
                string[] jsonFiles = System.IO.Directory.GetFiles(directoryPath, "*.json");

                foreach (string jsonFile in jsonFiles)
                {
                    // 读取 JSON 文件内容
                    string jsonContent = System.IO.File.ReadAllText(jsonFile);

                    // 解析 JSON 内容
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
                                    Console.WriteLine(textElement.GetString());

                                    crtIdx(messageElement, textElement);
                                }

                            }
                            else
                            {
                                Console.WriteLine($"The 'message' property in the file {jsonFile} is not an object.");
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static void crtIdx(JsonElement messageElement, JsonElement textElement)
        {
            try
            {
                SortedList o = new SortedList();
                setuNameFrmTgmsgJson(messageElement, o);
                setGrpFromTgjson(messageElement, o);
                long stmp = messageElement.GetProperty("date").GetInt64();
                o.Add("timeStamp", stmp);
                o.Add("time", ConvertUnixTimeStampToDateTime((stmp)));

                CreateIndex_part2(textElement.GetString(), o);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }

        }

        public static void CreateIndex_part2(string? msgxv1, SortedList o)
        {
            var msgx = ChineseCharacterConvert.Convert.ToSimple(msgxv1);
            var segmenter = new JiebaSegmenter();
            //------------自定词
            segmenter.LoadUserDict("user_dict.txt");
            segmenter.AddWord("会所"); // 可添加一个新词
            segmenter.AddWord("妙瓦底"); // 可添加一个新词
            segmenter.AddWord("御龙湾"); // 可添加一个新词
            HashSet<string> user_dict = ReadLinesToHashSet("user_dict.txt");
            foreach (string line in user_dict)
            {
                segmenter.AddWord(line);
            }
            HashSet<string> postnKywd位置词set = ReadLinesToHashSet("位置词.txt");
            foreach (string line in postnKywd位置词set)
            {
                segmenter.AddWord(line);
            }




            IEnumerable<string> enumerable = segmenter.CutForSearch(msgx);
            // 使用 LINQ 的 ToArray 方法进行转换
            string[] kwds = enumerable.ToArray();

            foreach (string wd in kwds)
            {
                //------停顿词过滤  单个字的过滤
                if (wd.Length <= 1) continue;
                if (IsNumeric(wd)) continue;
                if (IsAllPunctuation(wd)) continue;
                //todo 常见没意义词的过滤 虚词过滤

                SortedList doc = new SortedList();

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                doc.Add("id", timestamp);

                doc.Add("kwd", wd); doc.Add("txt", msgxv1);
                doc.Add("grpinfo", o);
                ormJSonFL.save(doc, $"fullTxtSrchIdxdataDir/{wd}.json");

            }
        }

        private static JsonElement setGrpFromTgjson(JsonElement messageElement, SortedList o)
        {
            JsonElement frmObjct = ldfld(messageElement, "chat");
            JsonElement title = ldfld(frmObjct, "title");
            o.Add("grp", title.GetString());
            return frmObjct;
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
