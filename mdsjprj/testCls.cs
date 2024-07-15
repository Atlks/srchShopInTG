
using JiebaNet.Segmenter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using prj202405.lib;
using static System.Net.Mime.MediaTypeNames;
using prj202405.lib;
using ClosedXML.Excel;
using prj202405.lib;
using System.Text.RegularExpressions;
using mdsj;
using DocumentFormat.OpenXml.Bibliography;
using SqlParser;
using SqlParser.Ast;
using mdsj.lib;
using static mdsj.biz_other;
using System.Collections.Generic;
using static libx.qryEngrParser;
using static mdsj.biz_other;
using static mdsj.clrCls;
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

using static mdsj.lib.CallFun;
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
using DocumentFormat.OpenXml.Office2010.Excel;
using RG3.PF.Abstractions.Entity;
using System.Security.Cryptography;
using DocumentFormat.OpenXml.Drawing.Diagrams;

using System.Reflection;
using System.Threading;

using System.Text.Json;
using DocumentFormat.OpenXml.Spreadsheet;


using DocumentFormat.OpenXml.Wordprocessing;

using Microsoft.EntityFrameworkCore.Metadata;
using Xabe.FFmpeg.Downloader;
using static mdsj.lib.avClas;
using static mdsj.lib.dtime;
using static mdsj.lib.fulltxtSrch;
using static prj202405.lib.tglib;
using static mdsj.lib.web3;
using mdsj.libBiz;
using Microsoft.ClearScript.V8;
using static WindowsFormsApp1.libbiz.storeEngFunRefCls;
using WindowsFormsApp1.libbiz;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using HtmlAgilityPack;
namespace prj202405
{
    internal class testCls
    {



        //private static void CreateIndex(string texts)
        //{
        //    var analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
        //    var indexDir = FSDirectory.Open(new DirectoryInfo(IndexPath));
        //    var indexConfig = new IndexWriterConfig(LuceneVersion.LUCENE_48, analyzer);
        //    using var writer = new IndexWriter(indexDir, indexConfig);

        //    foreach (var text in texts)
        //    {
        //        var doc = new Document
        //    {
        //        new TextField("content", text, Field.Store.YES)
        //    };
        //        writer.AddDocument(doc);
        //    }

        //    writer.Flush(triggerMerge: false, applyAllDeletes: false);
        //}

        public static HashSet<string> ProcessFiles(string directoryPath)
        {
            var resultSet = new HashSet<string>();

            // Get all .cs files in the directory and subdirectories
            var csFiles = Directory.GetFiles(directoryPath, "*.cs", SearchOption.AllDirectories);

            foreach (var file in csFiles)
            {
                // Read all lines from the current file
                var lines = System.IO.File.ReadAllLines(file);
                foreach (var line in lines)
                {
                    // Use regular expression to find characters between dot and left parenthesis
                    var matches = Regex.Matches(line, @"\.(.*?)\(");
                    foreach (Match match in matches)
                    {
                        if (match.Groups.Count > 1)
                        {
                            resultSet.Add(match.Groups[1].Value);
                        }
                    }
                }
            }

            return resultSet;
        }

   

        internal static async System.Threading.Tasks.Task testAsync()
        {
         //    mergeTransWdlib();
        //    tmrTask1startNow();
          //  ticyWdRoot();
         //   ticyuWEdsTest();
          //  spdrTest();
            //
           // getwdRoots();
            var root = GetRoot("running");
            transltTest();
            var sss = string.Join("\n", hs_mswd);
            WriteAllText("misswdFmt.txt",sss);
          //  tmrTask1start();
          //  CallTmrTasks();

            ConvertXmlToHtml("mdsj.xml", "mdsj.htm");
            WriteAllText("mdsj.xml.json", ConvertXmlToJson("mdsj.xml"));

            var set = ProcessFiles("D:\\0prj\\mdsj");
            WriteAllText("wds.json", set);
            print("Column1\tColumn2\tColumn3");
            print("Data1\tData2\tData3");
            print("  thrdid:" + Thread.CurrentThread.ManagedThreadId);

            // 使用 Task.Run 启动一个新的任务
            //Task newTask = Task.Run(() =>{
            //    asyncF();
            //});
            print("sync  log");
            //  tts("此消息来了11");
            geenBtns();
            try
            {
                int x = 1;
                var y = 2;
            }
            catch (Exception ex)
            {
                print("除法错误：" + ex.Message);
            }
            //for(int i=0;i<100;i++)
            //{
            //    Thread.Sleep(50);
            //   print(str_repeatV2("=", i) + "=>");
            //}
            var mymm4shareCfg = "name=缅甸&fmt=sqlt&storeEngr=rnd_next4SqltRf";
            SortedList valueMM = castUrlQueryString2hashtable(mymm4shareCfg);
            //  testShareCfg();





            // tmrEvt_sendMsg4keepmenu("今日促销商家.gif",  plchdTxt);

            //var s222 = "C:\\Users\\Administrator\\OneDrive\\song cn\\龙梅子 - 离别的眼泪.mp3";
            //var rzt = await RecognizeMusic(s222);

            //  await   AaveCollateralInfo.GetCollateralInfo("0xc54931775f7b9f2f9648c38c52b96ccb828bf8af");
            //  chkTgVld();

            //  call_user_func(qry5829,  "xxx.json" );

            using (var engine = new V8ScriptEngine())
            {
                engine.Execute("a= 555");
                var result = engine.Script.a;
                print(result); // 输出 8
            }

            var id = "0624按摩552110457";
            long uid = 879006550;
            //     ormJSonFL.del(id, $"blshtDir/blsht{uid}.json");
            //  Qunzhushou.logic_addCashflow(uid, "嗨小爱童鞋 记账 0625 13 吃饭");
            // 设置 FFmpeg 路径
            await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official);

            int btr = GetBatteryPercentage();
            string apiKey = "sk-proj-N2Fq9Z6KNZ7xx98ssXshT3BlbkFJ2HyoaRNCbxEkQtYcGOu6";
            string question = "世界一共多少个城市";

            //     string answer = await Program2024.srchByChtgpt(apiKey, question);
            //    print(answer);


            // 指定加密货币符号，例如 "bitcoin,ethereum,ripple"
            string cryptoSymbols = "bitcoin,ethereum,optimism,arbitrum,chainlink,dogecoin,binancecoin,solana,shiba-inu,ripple";
            //  
            // 启动一个新线程执行获取加密货币价格的任务
            TaskRun(async () =>
            {

                var prices = await GetCryptoPricesAsync(cryptoSymbols);
                print(json_encode(prices));
            });

            //  

            // 替换为你想下载的歌曲名称
            string songName = "Sweet Like Cola";
            songName = "sweet like cola";
            //  await DownloadSongAsMp3(songName,"downld");
            //   SendMp3ToGroupAsync("D:\\0prj\\mdsj\\mdsjprj\\bin\\Debug\\net8.0\\Sweet Like Cola.mp3", -1002206103554);
            //  sim2trand("D:\\0prj\\inputmthd\\lib\\常用字3000.txt");
            //   z_wucan();   tglib    //  if(chatid== -1002206103554)
            //     ReadAndCreateIndex4tgmsg("D:\\0prj\\mdsj\\mdsjprj\\bin\\Debug\\net8.0\\msgRcvDir");
            int n = 3513;
            double pre = n * 0.85;
            double next = n * 1.015;
            string mp3FilePath = "C:\\Users\\Administrator\\OneDrive\\90后非主流的歌曲 v2 w11\\Darin-Be What You Wanna Be HQ.mp3"; // 替换为你的 MP3 文件路径

            //   playMp3(mp3FilePath);
            //    logErr2024(111, "test", "errlog", null);

            //rdCnPrs();
            //  parse_str_dsl();

            if (System.IO.File.Exists("c:/teststart.txt"))
            {
                object o = new Hashtable();
                //   wrtLgTypeDate("msgrcvDir", o);
                // timerCls.z_actSj();
                //   z_actSj();
                //   callx((id: 11, dbf: "dbf"));
                //    rdCateGeneH5list();
                //   getProdSvrWdlib();

                //    增加分类addcate();




                //    json2dbMrcht();

                //  var o = (ex: 111, method_Name: "mthnamxxx", prm: "paramValues");
                //   logErr2025(o, "func_get_args", "errlogDir2024");

                // exportCftFrmDb();
                //var sql_dbf = "mrcht.json";
                //List<SortedList> lst_hash = ormJSonFL.qrySglFL(sql_dbf);

                //  ormIni.saveRplsMlt(lst_hash,"mrcht.ini");

                //   string sql_dbf = setCtry();

                return;


                //     HashSet<City> dataObjPark= mrcht.qry4byParknameExprs2Dataobj( "city=妙瓦底&park=世纪新城园区", Program._shangjiaFL());

                //export 
                //联系商家城市
                //HashSet<City> _citys = [];
                //var merchants = System.IO.File.ReadAllText("Merchant.json");
                //if (!string.IsNullOrEmpty(merchants))
                //    _citys = JsonConvert.DeserializeObject<HashSet<City>>(merchants)!;

                //   午餐餐饮关键词 午餐 餐饮 鱼肉 牛肉 火锅 炒饭 炒粉

                //搜索关键词  Merchant.json to citys

                //    ExtractLinks("D:\\0prj\\缅甸商家\\缅甸商家\\dbx\\web.htm","shibo.htm");


                // wucan();
                // timerCls.  xiawucha();
                sqlParser.MainTEst();

                return;

                var sql = "select * from my_table where 列名1 > 99 and col2<98 ";

                //Sequence<Statement> ast = new Parser().ParseSql(sql_dbf);
                //var updateString = JsonConvert.SerializeObject(ast, Formatting.Indented);

                //    print(updateString);
                //   ast.
                //    ArrayList a = filex.rdWdsFromFile("底部公共菜单.txt");
                //   timerCls.tmrEvt_sendMsg4keepmenu("今日促销商家.gif", timerCls.plchdTxt, Program._btmBtns());
                return;

                //     timerCls. sendMsg4keepmenu("今日促销商家.gif",timerCls. plchdTxt, Program._btmBtns());
                ArrayList lst = testCls.kwdSeasrchInGrp("kwdSearchINGrp.txt");

                //export mercht
                //  exptMrcht();
                Merchant? merchant = new Merchant();
                merchant.Guid = "123456";
                merchant.Name = "shjjj";
                var text = "pinlunxxxx";
                SortedList pinlunobj = new SortedList();
                pinlunobj.Add("id", DateTime.Now.ToString());
                pinlunobj.Add("商家guid", merchant.Guid);
                pinlunobj.Add("商家", merchant.Name);
                pinlunobj.Add("时间", DateTime.Now.ToString());
                pinlunobj.Add("评论内容", text);
                pinlunobj.Add("消息", "tttttt111111111111");
                System.IO.Directory.CreateDirectory("pinlunDir");
                ormJSonFL.save(pinlunobj, "pinlunDir/" + merchant.Guid + merchant.Name + ".json");


                ormSqlt.save(pinlunobj, "pinlunDir/" + merchant.Guid + merchant.Name + ".db");

                ormExcel.save(pinlunobj, "pinlunDir/" + merchant.Guid + merchant.Name + ".xlsx");
                ormIni.save(pinlunobj, "pinlunDir/" + merchant.Guid + merchant.Name + ".ini");
                print("line1633");

                print(JsonConvert.SerializeObject(ormIni.qry("pinlunDir/" + merchant.Guid + merchant.Name + ".ini")));



                print(JsonConvert.SerializeObject(ormExcel.qry("pinlunDir/" + merchant.Guid + merchant.Name + ".xlsx")));


                print(JsonConvert.SerializeObject(ormJSonFL.qryDep("pinlunDir/ziluxwubxeaktvrvcmsrryfzrmH13 红楼 一楼 按摩.json")));

                print(JsonConvert.SerializeObject(ormSqlt.qryDep("pinlunDir/ziluxwubxeaktvrvcmsrryfzrmH13 红楼 一楼 按摩商家评论表.db")));
                //    ormTest.   testorm();

                var segmenter = new JiebaSegmenter();
                segmenter.LoadUserDict(userDictFile);
                segmenter.AddWord("会所"); // 可添加一个新词

                //var segments = segmenter.Cut("我来到北京清华大学", cutAll: true);
                //Console.WriteLine("【全模式】：{0}", string.Join("/ ", segments));

                //segments = segmenter.Cut("我来到北京清华大学");  // 默认为精确模式
                //Console.WriteLine("【精确模式】：{0}", string.Join("/ ", segments));

                //segments = segmenter.Cut("他来到了网易杭研大厦");  // 默认为精确模式，同时也使用HMM模型
                //Console.WriteLine("【新词识别】：{0}", string.Join("/ ", segments));

                var segments = segmenter.CutForSearch("谁知道会所联系方式呢"); // 搜索引擎模式
                print("【搜索引擎模式】：{0}", string.Join("/ ", segments));
                // timerCls.z_actSj();
                //  timerCls.renqi();
                //     timerCls.z21_yule();
                //     timerCls.zaocan();
                //  timerCls.wucan();
                //  timerCls.xiawucha();
                //   timerCls.actSj();
                //   addData();

                //   findd();
            }

            // 

        }

        private static void mergeTransWdlib()
        {
            string inif = $"{prjdir}/cfgNlp/word5000.ini";
          
            string jsonf = $"{prjdir}/cfgNlp/wd.engCns5k.json";

            inif = $"{prjdir}/cfgNlp/wdlib.enNcn5k.delKenLenLess3Fnl.ini";
            jsonf = "C:\\Users\\Administrator\\GolandProjects\\awesomeProject\\wd.tmp3k.json";
            SortedList<string, string> hs1 = LdHstbEsFrmIni(inif);
            SortedList<string, string> hs2 = LdHstbEsFrmJsonFile(jsonf);
            SortedList<string, string> hs4 = MergeSortedLists(hs1, hs2);
          //  CleanupSortedListKeysLenLessthan3(hs4);
            WriteAllText("wdlib.enNcn5k.delKenLenLess3.json", hs4);
            CleanupSortedListValuesStartWzAlphbt(hs4);
            WriteAllText("wdlib.enNcn5k.delKenLenLess3Fnl.json", hs4);
            ormIni.saveIni(hs4, "wdlib.enNcn5k.v2.ini");
        }

        private static SortedList<string, string> LdHstbEsFrmJsonFile(string v)
        {
            return ReadJsonFileToSortedList(v);
        }

        static SortedList<string, string> ReadJsonFileToSortedList(string filePath)
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
        static SortedList<string, string> ReadIniFileToSortedList(string filePath)
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

        private static SortedList<string, string> LdHstbEsFrmIni(string v)
        {
            return ReadIniFileToSortedList(v);
        }

        static SortedList<string, string> MergeSortedLists(SortedList<string, string> list1, SortedList<string, string> list2)
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
        static void CleanupSortedListValuesStartWzAlphbt(SortedList<string, string> sortedList)
        {
            // 创建一个列表，存储需要移除的键
            HashSet<string> keysToRemove = new HashSet<string>();

            // 遍历 SortedList，找出以字母开头的值
            foreach (var kvp in sortedList)
            {
                if(string.IsNullOrEmpty(kvp.Value))
                {
                    keysToRemove.Add(kvp.Key);return;

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
        static bool IsLetter(char character)
        {
            return (character >= 'A' && character <= 'Z') || (character >= 'a' && character <= 'z');
        }

        static bool IsEnglishLetter(char character)
        {
            return (character >= 'A' && character <= 'Z') || (character >= 'a' && character <= 'z');
        }
        static void CleanupSortedListKeysLenLessthan3(SortedList<string, string> sortedList)
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
        private static void ticyWdRoot()
        {
            HashSet<string> hs = new HashSet<string>();
            var f = "word7000.json";
            List<string> li = ReadJsonFileToList(f);
            foreach( string wd in li)
            {
                var root = GetRoot(wd);
                hs.Add(root);
            }

            var wds = ConvertAndSortHashSet(hs);
            WriteAllText("wordRoot.json", wds);
        }

        static List<string> ReadJsonFileToList(string filePath)
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
     

    
    
        private static void spdrTest()
        {
            var url = "https://www.khmertimeskh.com/";
            url = "https://www.khmertimeskh.com/category/national/#google_vignette";
            url = "https://www.khmertimeskh.com/category/business/";
            var html=GetHtmlContent(url);
            WriteAllText("downHtmldir/main.htm", html);
            HashSet<string> urls = ExtractHrefAttributes(html);
            WriteAllText("url1119.json", urls);
          //  urls = FilterHtmlUrls(urls);
            foreach_HashSet(urls, (string urlMaybeRltv) =>
            {
                string ext = GetExtension(urlMaybeRltv);
                if (EndsWith(ext, "js css jpg png gif"))
                    return;
               // if (ext.EndsWith("htm") || ext.EndsWith("html"))
                {
                    string url1 = url + urlMaybeRltv;
                    if (urlMaybeRltv.StartsWith("http"))
                        url1 = urlMaybeRltv;
                    Download(url1, "downHtmldir");
                }
                
            });

        }

        private static bool EndsWith(string ext, string extss)
        {
            string[] a = extss.Split(" ");
           foreach(string ex in a)
            {
                if (ext.EndsWith(ex))
                    return true;
            }
            return false;
        }

        static HashSet<string> FilterHtmlUrls(HashSet<string> urls)
        {
            return new HashSet<string>(urls.Where(url => url.EndsWith(".html", StringComparison.OrdinalIgnoreCase) || url.EndsWith(".htm", StringComparison.OrdinalIgnoreCase)));
        }
        private static string GetExtension(string url)
        {
            try
            {
                var ext = Path.GetExtension(url);
                ext = ext.ToLower().Trim();
                return ext;
            }catch(Exception e)
            {
                return "";
            }
           
        }

        private static void Download(string url, string dataDir)
        {// 生成一个新的 UUID
            Guid newUuid = Guid.NewGuid();
            var html = GetHtmlContent(url);
            WriteAllText($"{dataDir}/{newUuid}.htm", html);
        }
        static HashSet<string> ExtractHrefAttributes(string html)
        {
            HashSet<string> hrefs = new HashSet<string>();

            // 创建 HtmlDocument 对象并加载 HTML
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            // 查找所有 a 标签
            var nodes = doc.DocumentNode.SelectNodes("//a[@href]");

            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    // 提取 href 属性值并添加到 HashSet 中
                    string href = node.GetAttributeValue("href", string.Empty);
                    if (!string.IsNullOrEmpty(href))
                    {
                        hrefs.Add(href);
                    }
                }
            }

            return hrefs;
        }
        static HashSet<string> ExtractUrls(string input)
        {
            HashSet<string> urls = new HashSet<string>();

            // 定义匹配 URL 的正则表达式
            string pattern = @"https?://[^\s/$.?#].[^\s]*";
            Regex regex = new Regex(pattern);

            // 使用正则表达式匹配所有 URL
            MatchCollection matches = regex.Matches(input);

            // 将匹配的 URL 添加到 HashSet 中
            foreach (Match match in matches)
            {
                urls.Add(match.Value);
            }

            return urls;
        }

        private static void getwdRoots()
        {
            List<string> liRzt = new List<string>();
            List<string> li = getListFrmFil($"{prjdir}/cfg/wds.txt");
            li = RemoveEmptyElements(li);
            foreach_listStr(li, (string line) =>
            {
                var wd = line.Trim().ToLower();
                string wdOri = GetRoot(wd);
                liRzt.Add(wdOri);
            });
            string rzt = JoinStringsWithNewlines(liRzt);
            print(rzt);
        }
    

        private static void geenBtns()
        { // Create the keyboard object

            // Create the first row of buttons
            var firstRow = new List<InlineKeyboardButton>();
            firstRow.Add(
                new InlineKeyboardButton("代购须知")
                {
                    Text = "代购须知",
                    CallbackData = "daigou"
                }
            );
            firstRow.Add(
                new InlineKeyboardButton("代收须知")
                {
                    Text = "代收须知",
                    CallbackData = "daishou"
                }
            );
            firstRow.Add(
              new InlineKeyboardButton("代付须知")
              {
                  Text = "代付须知",
                  CallbackData = "daifu"
              }
            );

            // Create the second row of buttons
            var secondRow = new List<InlineKeyboardButton>();
            secondRow.Add(
                new InlineKeyboardButton("联系代购/付客服")
                {
                    Text = "联系代购/付客服",
                    Url = "https://t.me/LianXin_ShangWu"
                }
            );




            // Add the rows of buttons to the keyboard
            var lstBtns = new List<List<InlineKeyboardButton>>();
            lstBtns.Add(firstRow);
            lstBtns.Add(secondRow);

            var keyboard = new InlineKeyboardMarkup(lstBtns);
            // Serialize the keyboard to JSON
            var keyboardJson = JsonConvert.SerializeObject(keyboard);
            //    WriteObj("btns.json", keyboard);

            // Print the JSON string to the console
            print(keyboardJson);
        }
        private static async Task<object> asyncF()
        {
            print("enter asyncfun ");
            print("async thrdid:" + Thread.CurrentThread.ManagedThreadId);
            //弹框
            //await botClient.AnswerCallbackQueryAsync(
            //  callbackQueryId: update.CallbackQuery.Id,
            //  text: "这是别人搜索的联系方式,如果你要查看联系方式请自行搜索",
            //  showAlert: true); // 这是显示对话框的关键);
            //return;
            await System.Threading.Tasks.Task.Delay(3000);
            print("...exit from async ");
            return 888;

        }



        private static void rdCateGeneH5list()
        {
            List<SortedList> rws = ormIni.qryV2("cateECns.ini");

            SortedList map = rws[0];
            //  foreach (SortedList item in map)
            foreach (var value in map.Values)
            {
                String s = $" <option value=\"{value}\">";
                print(s);
            }
        }

        private static void testShareCfg()
        {
            try
            {
                //缅甸,老挝
                List<SortedList> rztLi = arr_fltr4readDir("mercht商家数据", "", (SortedList row) =>
                {
                    if (row["商家"].ToString().Contains("理发"))
                        return true;
                    return false;
                });

                print(json_encode(rztLi));

            }
            catch (Exception e)
            {
                print(e.Message);
            }
        }

        private static async System.Threading.Tasks.Task chkTgVld()
        {
            List<SortedList> rsRztInlnKbdBtn = Qe_qryV2("mercht商家数据", "",
              null, null, row => row, storeEngFunRefCls.rnd_next4SqltRf());

            foreach (SortedList map in rsRztInlnKbdBtn)
            {
                try
                {
                    if (map["园区"] == "东风园区" && map["商家"] == "沙县 小吃")
                    {
                        print("dbg");
                    }

                    if (map["id"] == "vekzrqwxkeyuxpcxzkjdnfdsbt")
                    {
                        print("dbg");
                    }
                    string tg = trim_RemoveUnnecessaryCharacters4tgWhtapExt(map["Telegram"].ToString());
                    if (tg == "")
                    {
                        logCls.log(map, "TestTg有效性logDir");
                        //not exist 
                        stfld_addRplsKeyV(map, "TG有效", "N");

                        ormSqlt.save(map, $"mercht商家数据/{map["国家"]}.db");
                        continue;
                    }
                    string t = await http_GetHttpResponseAsync($"https://t.me/{tg}");
                    HashSet<string> lines = splitTxtByChrs(t, "\n\r");
                    if (lines.Count == 4)
                    {
                        logCls.log(map, "TestTg有效性logDir");
                        //not exist 
                        stfld_addRplsKeyV(map, "TG有效", "N");
                        ormSqlt.save(map, $"mercht商家数据/{map["国家"]}.db");

                    }
                    else
                    {//exist tg numb
                        if (ldfld(map, "TG有效", "") == "N")
                        {
                            stfld_addRplsKeyV(map, "TG有效", "Y");
                            ormSqlt.save(map, $"mercht商家数据/{map["国家"]}.db");
                        }
                    }
                }
                catch (Exception e)
                {
                    print(e);
                }

            }
        }

        private static void sim2trand(string v)
        {
            ArrayList li = new ArrayList();
            HashSet<string> wds = filex.ReadWordsFromFile(v);
            int n = 0;

            foreach (string wd in wds)
            {
                SortedList o = new SortedList();
                o.Add(ChineseCharacterConvert.Convert.ToTraditional(wd), wd);
                //   li.Add(o);
                string line = ChineseCharacterConvert.Convert.ToTraditional(wd) + "=" + wd;
                li.Add(line);
                n++;
                print(n);

            }
            // file_put_contents("trd2smpLib.json",json_encode(li));
            file_put_contents("trd2smpLib.ini", string.Join("\r\n", li.ToArray()));

            //   throw new NotImplementedException();
        }

        public static string GetHtmlContent(string url)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.print_call_FunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), url));

            using (HttpClient client = new HttpClient())
            {
                try
                {       // 设置用户代理
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

                    HttpResponseMessage response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();
                    string htmlContent = response.Content.ReadAsStringAsync().Result;
                    dbgCls.print_ret(__METHOD__, htmlContent.Substring(0, 300));
                    return htmlContent;
                }
                catch (HttpRequestException e)
                {
                    print($"Request error: {e.Message}");
                    dbgCls.print_ret(__METHOD__, 0);
                    return null;
                }
            }
        }
        private static void wrtLgTypeDate(string logdir, object o)
        {
            // 创建目录
            System.IO.Directory.CreateDirectory(logdir);
            // 获取当前时间并格式化为文件名
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            string fileName = $"{logdir}/{timestamp}.json";
            file_put_contents(fileName, json_encode(o), false);

        }
 

        private static void getProdSvrWdlib()
        {
            var words = new HashSet<string>();
            //建立商品与服务词库
            List<SortedList> li = ormSqlt.qryV2("D:\\0prj\\mdsj\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据\\缅甸.db");
            foreach (SortedList rw in li)
            {
                string wds = rw["商家"].ToString();
                words = MergeArrayWithHashSet(wds, words);
                words = MergeArrayWithHashSet(rw["关键词"].ToString(), words);
                words = MergeArrayWithHashSet(rw["分类关键词"].ToString(), words);

            }

            var txt = string.Join("\r\n", words);
            System.IO.File.WriteAllText("商品词.txt", txt);
        }

        private static void 增加分类addcate()
        {
            List<SortedList> rws = ormIni.qryV2("cateECns.ini");

            SortedList map = rws[0];


            List<SortedList> li = ormSqlt.qryV2("D:\\0prj\\mdsj\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据\\缅甸.db");
            foreach (SortedList rw in li)
            {
                object? cateE = rw["cateEgls"];
                arrCls.stfld_addRplsKeyV(rw, "分类", map[cateE.ToString()]);
            }


            ormSqlt.saveMltHiPfm(li, "mercht商家数据/缅甸.db");
        }

        private static void json2dbMrcht()
        {
            string f = "mercht商家数据/缅甸.json";
            List<SortedList> li = ormJSonFL.qry(f);
            ormSqlt.saveMltHiPfm(li, "mercht商家数据/缅甸.db");
        }

        private static void exportCftFrmDb()
        {
            List<Dictionary<string, string>> li = ormSqlt._qryV2($"select * from grp_loc_tb ", "grp_loc.db");
            foreach (Dictionary<string, string> dic in li)
            {

                //       List<SortedList> lst = ormJSonFL.qry("grpCfgDir/grpcfg{groupId}.json");
                string groupId = dic["grpid"];

                SortedList hash = DictionaryToSortedList(dic);
                hash["id"] = groupId;
                ormJSonFL.save(hash, $"grpCfgDir/grpcfg{groupId}.json");
            }
        }

        private static string setCtry()
        {
            var sql_dbf = "mrcht.json";
            List<SortedList> lst_hash = ormJSonFL.qrySglFL(sql_dbf);
            foreach (SortedList obj in lst_hash)
            {
                arrCls.stfld_replaceKeyV(obj, "ctry", "缅甸");

                //obj[""ctry = ; // 设置 ctry 属性
                //SortedList sortedList = new SortedList();
                //sortedList.Add(1, obj);
                //sortedLists.Add(sortedList);
            }
            ormJSonFL.saveMltV2(lst_hash, sql_dbf);
            return sql_dbf;
        }

        public static ArrayList kwdSeasrchInGrp(string filePath)
        {
            // 创建一个 ArrayList 来存储所有的单词
            ArrayList wordList = new ArrayList();

            // 读取文件中的所有行
            string[] lines = System.IO.File.ReadAllLines(filePath);

            // 遍历每一行
            foreach (string line in lines)
            {
                // 按空格分割行，得到单词数组
                string[] words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // 将单词添加到 ArrayList 中
                foreach (string word in words)
                {
                    if (word.Trim().Length > 0)
                        wordList.Add(word);
                }
            }
            return wordList;
        }

        private static void exptMrcht()
        {
            HashSet<prj202405.City> _citys = getCitysObj();
            var citys = (from c in _citys select c).ToList();

            foreach (var city in citys)
            {
                System.Collections.SortedList cityMap = corex.ObjectToSortedList(city);
                cityMap.Remove("Address");
                cityMap.Add("cityname", city.Name);
                print(JsonConvert.SerializeObject(cityMap, Newtonsoft.Json.Formatting.Indented));
                var addrS = (from ca in city.Address
                             select ca
                         )
                     .ToList();
                foreach (var addx in addrS)
                {
                    System.Collections.SortedList addMap = corex.ObjectToSortedList(addx);
                    addMap.Remove("Merchant");
                    addMap.Add("parkname", addx.Name);
                    addMap.Add("parkkwd", addx.CityKeywords);
                    print(JsonConvert.SerializeObject(addMap, Newtonsoft.Json.Formatting.Indented));
                    var rws = (from m in addx.Merchant
                               select m
                              )
                          .ToList();
                    foreach (var m in rws)
                    {
                        System.Collections.SortedList mcht = corex.ObjectToSortedList(m);
                        mcht.Add("CityKeywords", city.CityKeywords);
                        mcht.Add("cityname", city.Name);
                        mcht.Add("parkname", addx.Name);
                        mcht.Add("parkkwd", addx.CityKeywords);
                        print(mcht["Category"]);
                        //    mcht.Add("CategoryStr", Program._categoryKeyValue[Convert.ToInt32(mcht["Category"].ToString())]);
                        mcht.Add("CategoryStrKwds", Program._categoryKeyValue[(int)m.Category]);
                        mcht.Add("cateInt", (int)m.Category);
                        mcht.Add("cateEgls", m.Category.ToString());
                        //   mcht

                        print(JsonConvert.SerializeObject(mcht, Newtonsoft.Json.Formatting.Indented));
                        print("..");
                    }

                }
            }



            // orderby am.Views descending
            //  select m,ca
            //count = results.Count;

        }

        private static void findd()
        {
            long timestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            var users_txt = System.IO.File.ReadAllText("db.json");

            showSpanTime(timestamp, "readFile");

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DefaultValueHandling = DefaultValueHandling.Ignore;
            JArray rows = JsonConvert.DeserializeObject<JArray>(users_txt, settings);
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

        private static void addData()
        {

            long timestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;



            List<object> data = new List<object>();
            for (int i = 0; i < 100 * 10000; i++)
            {
                Hashtable ht = new Hashtable();
                ht.Add("key", i);
                ht.Add("value" + i, i);
                data.Add(ht);
            }
            System.IO.File.WriteAllText("db.json", JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented));

            long timestamp_end = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long spantime = (timestamp_end - timestamp);
            print("spatime(ms):" + spantime);

        }
    }


}


//var users = System.IO.File.ReadAllText("Users.json");
//    if (!string.IsNullOrEmpty(users))
//        _users = JsonConvert.DeserializeObject<Dictionary<long, User>>(users)!;
//    var merchants = System.IO.File.ReadAllText("Merchant.json");

//var _citys = (JArray)JsonConvert.DeserializeObject(merchants);
//var arr3 = (JArray)_citys[0].Value<JArray>("Address");

//    foreach (JObject it in arr3)
//    {
//        var shopsarr = it.Value<JArray>("Merchant");
//        foreach (JObject shop in shopsarr)
//        {
//           print(shop.GetValue("KeywordString").ToString());
//        }

//        }



