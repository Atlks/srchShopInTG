
using System.Management;
using JiebaNet.Segmenter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using prjx.lib;
using static System.Net.Mime.MediaTypeNames;
using prjx.lib;
using ClosedXML.Excel;
using prjx.lib;
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
using static prjx.lib.arrCls;//  prj202405.lib
using static prjx.lib.dbgCls;
using static mdsj.lib.logCls;
using static prjx.lib.corex;
using static prjx.lib.db;
using static prjx.lib.filex;
using static prjx.lib.ormJSonFL;
using static prjx.lib.strCls;
using static mdsj.lib.bscEncdCls;

using static mdsj.lib.CallFun;
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
using static prjx.lib.tglib;
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
using Windows.Storage.Search;
using System.Runtime.InteropServices;
using Nustache.Core;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
namespace prjx
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

     

        internal static async System.Threading.Tasks.Task testAsync()
        {
            try
            {
                Print("----test add sqlt");
                SortedList a = new SortedList();
                a.Add("kk", 11);
                ormSqlt.Save4Sqlt(a, "test_sqlt.db");
                Print("---- end test add sqlt");
                await main1148();
            }
            catch (Exception e)
            {
                PrintCatchEx("test main()", e);
            }


        }




        public static void geneCert( )
        {
            string domain = "lianxin.co";
            string certificatePath = "lianxin.co.pfx";  // Path where the certificate will be saved
            string certificatePassword = "";  // Password for the certificate

            GenerateAndSaveCertificate(domain, certificatePath, certificatePassword);

            Console.WriteLine($"Certificate generated and saved to {certificatePath}");
        }

       public static SortedList<string, object> ConvertToStringObjectSortedList(SortedList  originalList)
        {
            var result = new SortedList<string, object>();

            foreach (DictionaryEntry entry in originalList)
            {
                string key = entry.Key?.ToString();  // 转换键为字符串
                object value = entry.Value;          // 保持值不变

                if (key != null)
                {
                    result.Add(key, value);
                }
            }

            return result;
        }

    public    static List<SortedList<string, object>> ConvertDicListToSortedList(List<Dictionary<string, string>> listOfDictionaries)
        {
            var listOfSortedLists = new List<SortedList<string, object>>();

            foreach (var dictionary in listOfDictionaries)
            {
                var sortedList = new SortedList<string, object>();

                foreach (var kvp in dictionary)
                {
                    sortedList[kvp.Key] = kvp.Value;
                }

                listOfSortedLists.Add(sortedList);
            }

            return listOfSortedLists;

         
        }

        private static async System.Threading.Tasks.Task main1148()
        {
            // db store cmpr  zip troub
            // json just ok....sngle json imprv pfm..
            //sqlt kv mode
            //   saveZip();

            //  geneCert();
          //  List<Dictionary<string, string>> st = GetListFrmIniFL("mrcht.ini");
            List< SortedList > li2 = GetListDicFrmIniFL("mrcht.ini");
            foreach(SortedList  itm in li2)
            {
              //  Save2SqltKvMd(itm, "mchtKv.db");
            }
            //for (int i = 1; i < 5; i++)
            //{
            //    SortedList<string, object> o = new SortedList<string, object>();
            //    o.Add("id", i);
            //    o.Add("name", 888);
            //    Save2SqltKvMd(o, "db1056.db");
            // //   AppendSortedListToZip(o, "db1share2.zip", $"id_{i}.json");
            //}
            PrintTimestamp("bf qry kv db");
            Print(EncodeJsonFmt(GetListFrmSqltKvV2("mchtKvs.db")));
            PrintTimestamp("end  qry kv db");

            Print("\n----------------\n");
            // RenderTableToConsole
            string markdownTable = ConvertHtmlTableToMarkdown(ReadAllText($"{prjdir}/webroot/htm2csl.htm"));

            Print(markdownTable);

            Print("\n----------------\n");
            RenderMarkdownTableToConsole(markdownTable);
            Print("\n----------------\n");
         //   RenderMstch(“);

            var f = $"{prjdir}/webroot/tmplt.htm";
            string rztlist511 = RendHtm(f);
            Print(rztlist511);
            //  List<string> exprss = ExtractExpressions(f);
            //GetListExprsFromHtm(f);

            PrintTimestamp("bef rd ini");
            Print(GetListFrmIniFL("mrcht.ini").Count);
            PrintTimestamp("end rd ini");


            PrintTimestamp("bef rd json");
            Print(GetListHashtableFromJsonFil("mmr.json").Count);
            PrintTimestamp("end rd json");

            PrintTimestamp("bef to dic");
            Print(ormSqlt.qryToDic("mercht商家数据/缅甸.db").Count);
            PrintTimestamp("end rd dic");


            PrintTimestamp("bef rd to sortedlist");
            Print(ormSqlt.qryV2("mercht商家数据/缅甸.db").Count);
            PrintTimestamp("end rd sortedlist");

            //   qryToDic
            Print("os ver:" + GetOSVersion());//os ver:OS: Win32NT, Version: 10.0.22631
            //for cache
            var listFlrted = GetListFltrByQrystr("mercht商家数据", null, "");
            Print(listFlrted.Count);
            //username:password@hostname/resource
            string url136 = "administrator:gy5NLU0MJ4yv@206.119.166.120:3389";
            Dictionary<string, string> dic = GetDicFromUrl(url136);
            //  TransferFileByRdpWmi("mdsj.exe", "d:/upldir", dic["host"], dic["u"], dic["pwd"]);

            return;
            var orilen140 = "879006550_2d2481f9f76818ff6a54083de36ff7ed98593a9ef5871a5b98c676590fd8a345c084ed8554f4c52132ffe8b4de67c7fe9b6b3f360048011c1d70febb66e31608";
            var newlen = CompressString(orilen140);
            Print("newlen.Length::" + newlen.Length);//newlen.Length::148

            //   z_wucan();
            Print(DecryptAes("fea6fe56297b3ff650d928182f8caad06beb07c587251cf5294d1ce6b0fcfc6b8e94b0735f18579f1d13e78de98f158e24a73a57dc27ee6bfe12a9d15b61dcce"));
            Print(newToken("00799988", 3600 * 24 * 7));
            var encStr = EncryptAes("202411");
            Print(DecryptAes(encStr));

            Print(newToken("0079999", 3600 * 240));
            string htmlf = $"{prjdir}/cfg_btmbtn/好奇.htm";
            string html = ReadAllText(htmlf);
            string v = ConvertHtmlToJson4tg(html);
            WriteAllText("haocy.json", v);
            Print(v);
            Print("\a\a\a\a");
            //   add30xiezhi();
            Print(AddElmts("aaa", "a,b"));
            Print(DelElmts("a", "a,b,c"));
            HashSet<string> hs11 = GetHashsetEmojiCmn();
            //   💰💰💰();

            try
            {
                var lst458 = ormExcel.QryExcel("C:\\Users\\Administrator\\Documents\\sumdoc 2405\\xx国家商家数据 v3.xlsx");
                Print(EncodeJsonFmt(lst458));

                //----------------trans cn2en form--------------
                string TransFfilePath = $"{prjdir}/cfg字段翻译表/导入商家字段对应表.ini";
                List<SortedList> list_rzt_fmt = TransltKey(lst458, TransFfilePath);

                Print(EncodeJsonFmt(list_rzt_fmt));
            }
            catch (Exception e)
            {
                Print(e);
            }

            //    tmrEvt_sendMsg4keepmenu("今日促销商家.gif", plchdTxt);
            //HashSet<string> downedUrlss = newSet("downedUrlss2024.json");
            //downedUrlss.Add("111");
            //downedUrlss.Add("222");
            //    mergeTransWdlib();
            //    tmrTask1startNow();
            //  ticyWdRoot();
            //   ticyuWEdsTest();
            //   TaskRun(() => { new spdr(). spdrTest(); });
            //
            // getwdRoots();
            var root = GetRoot("running");
            //    transltTest();
            arr_cut();
            var sss = string.Join("\n", hs_mswd);
            WriteAllText("misswdFmt.txt", sss);
            // tmrTask1start();
            tmrTask1startNow();
            //  CallTmrTasks();

            ConvertXmlToHtml("mdsj.xml", "mdsj.htm");
            WriteAllText("mdsj.xml.json", ConvertXmlToJson("mdsj.xml"));

            //var set = ProcessFilesDep("D:\\0prj\\mdsj");
            //WriteAllText("wds.json", set);
            Print("Column1\tColumn2\tColumn3");
            Print("Data1\tData2\tData3");
            Print("  thrdid:" + Thread.CurrentThread.ManagedThreadId);

            // 使用 Task.Run 启动一个新的任务
            //Task newTask = Task.Run(() =>{
            //    asyncF();
            //});
            Print("sync  log");
            //  tts("此消息来了11");
            geenBtns();
            try
            {
                int x = 1;
                var y = 2;
            }
            catch (Exception ex)
            {
                Print("除法错误：" + ex.Message);
            }
            //for(int i=0;i<100;i++)
            //{
            //    Thread.Sleep(50);
            //   print(str_repeatV2("=", i) + "=>");
            //}
            var mymm4shareCfg = "name=缅甸&fmt=sqlt&storeEngr=rnd_next4SqltRf";
            SortedList valueMM = castUrlQueryString2hashtable(mymm4shareCfg);
            //  testShareCfg();







            //var s222 = "C:\\Users\\Administrator\\OneDrive\\song cn\\龙梅子 - 离别的眼泪.mp3";
            //var rzt = await RecognizeMusic(s222);

            //  await   AaveCollateralInfo.GetCollateralInfo("0xc54931775f7b9f2f9648c38c52b96ccb828bf8af");
            //  chkTgVld();

            //  call_user_func(qry5829,  "xxx.json" );

            using (var engine = new V8ScriptEngine())
            {
                engine.Execute("a= 555");
                var result = engine.Script.a;
                Print(result); // 输出 8
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
                Print(json_encode(prices));
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
                ormJSonFL.SaveJson(pinlunobj, "pinlunDir/" + merchant.Guid + merchant.Name + ".json");


                ormSqlt.Save4Sqlt(pinlunobj, "pinlunDir/" + merchant.Guid + merchant.Name + ".db");

                ormExcel.save(pinlunobj, "pinlunDir/" + merchant.Guid + merchant.Name + ".xlsx");
                ormIni.save(pinlunobj, "pinlunDir/" + merchant.Guid + merchant.Name + ".ini");
                Print("line1633");

                Print(JsonConvert.SerializeObject(ormIni.qry("pinlunDir/" + merchant.Guid + merchant.Name + ".ini")));



                Print(JsonConvert.SerializeObject(ormExcel.QryExcel("pinlunDir/" + merchant.Guid + merchant.Name + ".xlsx")));


                Print(JsonConvert.SerializeObject(ormJSonFL.qryDep("pinlunDir/ziluxwubxeaktvrvcmsrryfzrmH13 红楼 一楼 按摩.json")));

                Print(JsonConvert.SerializeObject(ormSqlt.qryDep("pinlunDir/ziluxwubxeaktvrvcmsrryfzrmH13 红楼 一楼 按摩商家评论表.db")));
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

        public static void saveZip()
        {
            for(int i = 1; i < 5; i++)
            {
                SortedList<string, object> o = new SortedList<string, object>();
                o.Add("id", i);
                o.Add("name", 888);
                AppendSortedListToZip(o, "db1share2.zip", $"id_{i}.json");
            }
           
        }

        /*
         创建临时文件: 使用临时文件来创建一个新的 ZIP 文件，该文件包含现有的条目和新的条目。
读取现有 ZIP 文件内容: 从现有 ZIP 文件中读取条目并将它们复制到新的 ZIP 文件中。
追加新条目: 将新的数据作为新的条目追加到 ZIP 文件中。
替换原始文件: 删除原始 ZIP 文件并将临时文件重命名为原始文件名。
         */
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortedList"></param>
        /// <param name="zipFilePath"></param>
        /// <param name="entryName"></param>
        public static void AppendSortedListToZip(SortedList<string, object> sortedList, string zipFilePath, string entryName)
        {
            // Create a temporary file to hold the new ZIP archive
            var tempFilePath = Path.GetTempFileName();

            try
            {
                // Create a memory stream to hold the serialized SortedList
                using (var memoryStream = new MemoryStream())
                {
                    // Serialize the SortedList to JSON and write it to the memory stream
                    using (var streamWriter = new StreamWriter(memoryStream, leaveOpen: true))
                    using (var jsonWriter = new JsonTextWriter(streamWriter))
                    {
                        var jsonSerializer = new Newtonsoft.Json.JsonSerializer();
                        jsonSerializer.Serialize(jsonWriter, sortedList);
                        jsonWriter.Flush();
                    }

                    // Reset the memory stream position to the beginning
                    memoryStream.Position = 0;

                    // Create or open the existing ZIP file and append to it
                    using (var zipFileStream = new FileStream(tempFilePath, FileMode.Create))
                    using (var zipArchive = new ZipArchive(zipFileStream, ZipArchiveMode.Create))
                    {
                        // Copy existing entries from the original ZIP file to the new ZIP file
                        if (System.IO.File.Exists(zipFilePath))
                        {
                            using (var originalZipFileStream = new FileStream(zipFilePath, FileMode.Open))
                            using (var originalZipArchive = new ZipArchive(originalZipFileStream, ZipArchiveMode.Read))
                            {
                                foreach (var entry in originalZipArchive.Entries)
                                {
                                    // Copy existing entries to the new archive
                                    var newEntry = zipArchive.CreateEntry(entry.FullName);
                                    using (var entryStream = entry.Open())
                                    using (var newEntryStream = newEntry.Open())
                                    {
                                        entryStream.CopyTo(newEntryStream);
                                    }
                                }
                            }
                        }

                        // Create a new entry in the new ZIP file
                        var zipEntry = zipArchive.CreateEntry(entryName);

                        // Write the serialized data to the new entry
                        using (var zipEntryStream = zipEntry.Open())
                        {
                            memoryStream.CopyTo(zipEntryStream);
                        }
                    }

                    // Replace the original ZIP file with the updated ZIP file
                    System.IO.File.Delete(zipFilePath);
                    System.IO.File.Move(tempFilePath, zipFilePath);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions if necessary
                Console.WriteLine($"An error occurred: {ex.Message}");
                if (System.IO.File.Exists(tempFilePath))
                {
                    System.IO.File.Delete(tempFilePath);
                }
            }
        }
        public static void SaveSortedListToZip (SortedList<string, object> sortedList, string zipFilePath, string entryName)
        {
            // Create a memory stream to hold the serialized SortedList
            // Create a memory stream to hold the serialized SortedList
            using (var memoryStream = new MemoryStream())
            {
                // Serialize the SortedList to JSON and write it to the memory stream
                using (var streamWriter = new StreamWriter(memoryStream, leaveOpen: true))
                using (var jsonWriter = new JsonTextWriter(streamWriter))
                {
                    var jsonSerializer = new Newtonsoft.Json.JsonSerializer();
                    jsonSerializer.Serialize(jsonWriter, sortedList);
                    jsonWriter.Flush();
                }

                // Reset the memory stream position to the beginning
                memoryStream.Position = 0;

                // Create or overwrite the zip file
                using (var zipFileStream = new FileStream(zipFilePath, FileMode.Create))
                using (var zipArchive = new ZipArchive(zipFileStream, ZipArchiveMode.Create))
                {
                    // Create a new entry in the ZIP file
                    var zipEntry = zipArchive.CreateEntry(entryName);

                    // Write the serialized data to the entry
                    using (var zipEntryStream = zipEntry.Open())
                    {
                        memoryStream.CopyTo(zipEntryStream);
                    }
                }
            }

        }



        private static void add30xiezhi()
        {
            for (int i = 0; i < 30; i++)
            {
                List<string> filess = new List<string>();
                filess.Add("uploads1016/FB_IMG_16042416836456873.jpg");
                //  SortedList o = new SortedList();
                SortedList saveOBJ = new SortedList();
                // saveOBJ.Add("照片或视频", fil);
                saveOBJ.Add("Files", (filess));
                saveOBJ.Add("Cate", "闲置");
                saveOBJ.Add("Title", "标题111");
                saveOBJ.Add("Txt", "内容222");
                saveOBJ.Add("Poster", "fadfa");

                // 获取当前时间（本地时间）
                DateTime now = DateTime.Now;

                // 格式化为可读性较强的字符串，精确到毫秒
                string formattedDate = now.ToString("yyyy-MM-dd HH:mm:ss");
                saveOBJ.Add("Time", formattedDate);
                ormJSonFL.SaveJson(saveOBJ, $"{prjdir}/db/{saveOBJ["Cate"]}.json");
            }

        }

        public static void ticyWdRoot()
        {
            HashSet<string> hs = new HashSet<string>();
            var f = "word7000.json";
            List<string> li = ReadJsonFileToList(f);
            foreach (string wd in li)
            {
                var root = GetRoot(wd);
                hs.Add(root);
            }

            var wds = ConvertAndSortHashSet(hs);
            WriteAllText("wordRoot.json", wds);
        }







        private static void getwdRoots()
        {
            List<string> liRzt = new List<string>();
            List<string> li = GetListFrmFil($"{prjdir}/cfg/wds.txt");
            li = RemoveEmptyElements(li);
            foreach_listStr(li, (string line) =>
            {
                var wd = line.Trim().ToLower();
                string wdOri = GetRoot(wd);
                liRzt.Add(wdOri);
            });
            string rzt = JoinStringsWithNewlines(liRzt);
            Print(rzt);
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
            Print(keyboardJson);
        }
        private static async Task<object> asyncF()
        {
            Print("enter asyncfun ");
            Print("async thrdid:" + Thread.CurrentThread.ManagedThreadId);
            //弹框
            //await botClient.AnswerCallbackQueryAsync(
            //  callbackQueryId: update.CallbackQuery.Id,
            //  text: "这是别人搜索的联系方式,如果你要查看联系方式请自行搜索",
            //  showAlert: true); // 这是显示对话框的关键);
            //return;
            await System.Threading.Tasks.Task.Delay(3000);
            Print("...exit from async ");
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
                Print(s);
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

                Print(json_encode(rztLi));

            }
            catch (Exception e)
            {
                Print(e.Message);
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
                        Print("dbg1431");
                    }

                    if (map["id"] == "vekzrqwxkeyuxpcxzkjdnfdsbt")
                    {
                        Print("dbg2432");
                    }
                    string tg = TrimRemoveUnnecessaryCharacters4tgWhtapExt(map["Telegram"].ToString());
                    if (tg == "")
                    {
                        logCls.log(map, "TestTg有效性logDir");
                        //not exist 
                        SetFieldAddRplsKeyV(map, "TG有效", "N");

                        ormSqlt.Save4Sqlt(map, $"mercht商家数据/{map["国家"]}.db");
                        continue;
                    }
                    string t = await http_GetHttpResponseAsync($"https://t.me/{tg}");
                    HashSet<string> lines = SplitTxtByChrs(t, "\n\r");
                    if (lines.Count == 4)
                    {
                        logCls.log(map, "TestTg有效性logDir");
                        //not exist 
                        SetFieldAddRplsKeyV(map, "TG有效", "N");
                        ormSqlt.Save4Sqlt(map, $"mercht商家数据/{map["国家"]}.db");

                    }
                    else
                    {//exist tg numb
                        if (LoadFieldFrmStlst(map, "TG有效", "") == "N")
                        {
                            SetFieldAddRplsKeyV(map, "TG有效", "Y");
                            ormSqlt.Save4Sqlt(map, $"mercht商家数据/{map["国家"]}.db");
                        }
                    }
                }
                catch (Exception e)
                {
                    Print(e);
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
                Print(n);

            }
            // file_put_contents("trd2smpLib.json",json_encode(li));
            file_put_contents("trd2smpLib.ini", string.Join("\r\n", li.ToArray()));

            //   throw new NotImplementedException();
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


            const string DbFileName = "mercht商家数据/缅甸.db";
            List<SortedList> li = ormSqlt.qryV2(DbFileName);
            foreach (SortedList rw in li)
            {
                NewMethod(map, rw);
            }

            ormSqlt.saveMltHiPfm(li, DbFileName);
        }


  

        private static void NewMethod(SortedList map, SortedList rw)
        {
            object? cateE = rw["cateEgls"];
            SetFieldAddRplsKeyV(rw, "分类", map[cateE.ToString()]);
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
                ormJSonFL.SaveJson(hash, $"grpCfgDir/grpcfg{groupId}.json");
            }
        }

        private static string setCtry()
        {
            var sql_dbf = "mrcht.json";
            List<SortedList> lst_hash = ormJSonFL.QrySglFL(sql_dbf);
            foreach (SortedList obj in lst_hash)
            {
                SetFieldReplaceKeyV(obj, "ctry", "缅甸");

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
            HashSet<prjx.City> _citys = getCitysObj();
            var citys = (from c in _citys select c).ToList();

            foreach (var city in citys)
            {
                System.Collections.SortedList cityMap = corex.ObjectToSortedList(city);
                cityMap.Remove("Address");
                cityMap.Add("cityname", city.Name);
                Print(JsonConvert.SerializeObject(cityMap, Newtonsoft.Json.Formatting.Indented));
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
                    Print(JsonConvert.SerializeObject(addMap, Newtonsoft.Json.Formatting.Indented));
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
                        Print(mcht["Category"]);
                        //    mcht.Add("CategoryStr", Program._categoryKeyValue[Convert.ToInt32(mcht["Category"].ToString())]);
                        mcht.Add("CategoryStrKwds", Program._categoryKeyValue[(int)m.Category]);
                        mcht.Add("cateInt", (int)m.Category);
                        mcht.Add("cateEgls", m.Category.ToString());
                        //   mcht

                        Print(JsonConvert.SerializeObject(mcht, Newtonsoft.Json.Formatting.Indented));
                        Print("..");
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


            Print(JsonConvert.SerializeObject(results));


            string showtitle = "spatime(ms):";
            showSpanTime(timestamp, showtitle);

        }

        private static void showSpanTime(long timestamp, string showtitle)
        {
            long timestamp_end = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long spantime = (timestamp_end - timestamp);

            Print(showtitle + spantime);
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
            Print("spatime(ms):" + spantime);

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



