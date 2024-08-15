
global using static prjx.lib.tglib;
global using static prjx.Program;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using prjx.lib;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using prjx.lib;
using prjx.lib;
using prjx.lib;
using JiebaNet.Segmenter;
using System.Xml;
using HtmlAgilityPack;
using Formatting = Newtonsoft.Json.Formatting;

using mdsj;
using System.Runtime.Intrinsics.Arm;

using System.Runtime.CompilerServices;
using mdsj;
using mdsj.libBiz;

using mdsj.lib;

using static mdsj.lib.afrmwk;
using static mdsj.lib.util;
using static libx.storeEngr4Nodesqlt;
using static prjx.timerCls;
using static mdsj.biz_other;
using static mdsj.clrCls;
using static libx.qryEngrParser;
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
using static mdsj.lib.util;
using static mdsj.libBiz.tgBiz;
using static mdsj.lib.afrmwk;


using static mdsj.lib.avClas;
using static mdsj.lib.dtime;
using static mdsj.lib.fulltxtSrch;

using System.Net.Http.Json;


using System.Security.Policy;

using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;


using System.Text;

using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Primitives;
using Org.BouncyCastle.Utilities.Collections;

namespace prjx
{
    internal class Program
    {
        //prech 4 set msg
        public const string PreCh = "📍";// ch4selectLocation

        //  https://api.telegram.org/bot6999501721:AAFNqa2YZ-lLZMfN8T2tYscKBi33noXhdJA/getMe
        // public const string botname = "LianXin_BianMinBot";

        public static TelegramBotClient botClient;
        //  @LianXin_QunBot

        // task grp
        //  public static long groupId = -1002206103554;
        //机器人创建者Id
        //     static readonly long botCreatorId = 6091395167;
        //加入的聊天Ids
        public static HashSet<string> chatIds = [];
        //联系商家城市
        //public static HashSet<City> _citys = [];
        //联系方式(这个的作用是检测别人在聊天信息中出现这个时就让别人可以搜索)
        public static HashSet<string> _contactType = ["商家联系方式", "商家飞机"];
        //分类键值对
        public static Dictionary<int, string> _categoryKeyValue = [];

        //搜索用户
        public static Dictionary<long, User> _users = [];

        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddControllers();
        //    services.AddSwaggerGen();
        //}

        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        //{
        //    app.UseSwagger();
        //    app.UseSwaggerUI(c =>
        //    {
        //        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        //    });

        //    app.UseRouting();
        //    app.UseEndpoints(endpoints =>
        //    {
        //        endpoints.MapControllers();
        //    });
        //}


     


        public static async Task Main(string[] args)
        {
       //     rnd4jsonFl
            if (System.IO.File.Exists("c:/teststart.txt"))
                await main10test1030();
            //  http://localhost:5000;
            //   List<string> ExtractProxyPassUrls111 = ExtractProxyPassUrls(nginccfg);
            //    var nnn=  JsonConvert.DeserializeObject<object>("adfaf");
            GetMethInfo("echo");
            // 设置控制台编码为 UTF-8
            Console.OutputEncoding = Encoding.UTF8;
            Callx("aaa", "prm1");
            prjdir = filex.GetAbsolutePath(prjdir);

            userDictFile = $"{prjdir}/cfg/user_dict.txt";

            var cfgf = $"{prjdir}/cfg/cfg.ini";
            Hashtable cfgDic = GetHashtabFromIniFl(cfgf);
            botClient = new(cfgDic["bottoken"].ToString());
            util.botname = cfgDic["botname"].ToString();
            int botEnable = GetFieldAsInt147(cfgDic, "bot", 1);
            if (botEnable == 1)
            {


                Evtboot(() =>
                {
                    //   botClient = botClient;
                    //todo here should wrt to rot ini therre..not here
                    获取机器人的信息();


                });

                System.IO.Directory.CreateDirectory("pinlunDir");
                #region 构造函数


                // set test grp bot
                if (System.IO.File.Exists("c:/teststart.txt"))
                {
                    //                mg MR.HAN, [20 / 5 / 2024 下午 1:25]
                    //6999501721:AAFLEI1J7YzEPJq - DfmJ04xFI8Tp - O6_5bE

                    //mg MR.HAN, [20 / 5 / 2024 下午 1:25]
                    //便民助手的APITOKEN

                    //mg MR.HAN, [20 / 5 / 2024 下午 1:25]
                    //@LianXin_BianMinBot
                    // botClient = new("7069818994:AAH3irkK1WpfBNxaNsU3rIGAIDyCunYGsy0"); ///lianxin_2025bot.
                    //  botClient = new("6999501721:AAFLEI1J7YzEPJq-DfmJ04xFI8Tp-O6_5bE");   //@LianXin_BianMinBot

                    //groupId = -1002206103554; //taskgrp

                }
                ////ini()   
                var vls = System.Enum.GetValues(typeof(Category));//  food drink ....
                foreach (var category in System.Enum.GetValues(typeof(Category)))
                {
                    Category enumValue = (Category)category;
                    string description = biz_other._GetEnumDescription(enumValue);
                    _categoryKeyValue.Add((int)enumValue, description);
                }


                #region 读取商家信息
                //  读取加入的群Ids           
                biz_other._readMerInfo();
                #endregion
                #endregion



                tglib.bot_iniChtStrfile();

                testCls.testAsync();
              //  botClient.on
                //   botClient.OnApiResponseReceived
                //botClient.OnMessage += Bot_OnMessage;
                //   botClient. += Bot_OnCallbackQuery;  jeig api outtime
                //分类枚举
                botClient.StartReceiving(updateHandler: EvtUpdateHdlrAsyncSafe,
                    pollingErrorHandler: tglib.bot_pollingErrorHandler,
                    receiverOptions: new ReceiverOptions()
                    {
                        AllowedUpdates = System.Array.Empty<UpdateType>(),
                        // 接收所有类型的更新
                        //AllowedUpdates = [UpdateType.Message,
                        //    UpdateType.CallbackQuery,
                        //    UpdateType.ChannelPost,
                        //    UpdateType.MyChatMember,
                        //    UpdateType.ChatMember,
                        //    UpdateType.ChatJoinRequest],
                        ThrowPendingUpdates = true,
                    });
                //在 Telegram.Bot 库中，ThrowPendingUpdates 是一个参数，用于指定在机器人启动时是否丢弃所有挂起的更新。换句话说，如果在启动机器人之前已经有一些未处理的更新（消息、命令等），设置 ThrowPendingUpdates 可以决定是否忽略这些未处理的更新。
                //   if (System.IO.File.Exists("c:/tmrclose.txt"))
                timerCls.setTimerTask();
                setTimerTask4prs();
                setTimerTask4tmr();
                //#warning 循环账号是否过期了
                RunTmrTasksCron();

            }



            //     Qunzhushou.main1();
            //    audioBot.main1();

            if (GetFieldAsInt(cfgDic, "wbsvs", 1) == 1)
            {
                webapi2.StartWbapiAsync();
                Action<HttpRequest, HttpResponse> value = (HttpRequest request, HttpResponse response) =>
                {

                    string methd = request.Path;
                    ////  methd = methd.Substring(1);
                    if (methd == "/swag33")
                    {
                        response.ContentType = "text/html; charset=utf-8";
                        var rzt = DocapiHttpHdlrApiSpelDocapi("mdsj.xml", response);
                        response.WriteAsync(rzt.ToString(), Encoding.UTF8).GetAwaiter().GetResult();
                        Jmp2endDep();
                    }

                };
                StartWebapi(value, "WbapiX");
            }



            //  Console.ReadKey();
            LoopForever();

        }

        private static async Task main10test1030()
        {
            const string FromDdataDir = "mercht商家数据"; ;
            var qrystr1005 = "园区(kk园区) 分类(兑换)";
            List<SortedList> listMered = GetListByQrystrNmldsl(FromDdataDir, qrystr1005);
            Print(listMered);
            //  string lxfsTmplt = @"[["Telegram","whveie123"],["微信","1233339"],["WhatsApp","093383"],["Signal","2349jhe"],["电话","1838383939"]]"
            //rewrt park cdt
            string url = "园区=KK园区,东方园区,缅甸,妙瓦底";
            string pkrPrm = "KK园区,东方园区,缅甸,妙瓦底";

            string rzt = ExtParks(pkrPrm);
            rzt = ToSqlPrmMode(rzt);

            Print("rzt=>" + rzt);
            //     Thread.Sleep(7000);

            string[] a237 = url.Split(",");

            string f119 = $"{prjdir}/webroot/国家.json";

            Print("GetParkPath=>" + GetParkPath("金州园区", ReadAllText(f119)));
            string qrystr = "aaa=111&园区=KK园区,东方园区";
            Dictionary<string, string> qrystrDic = LoadDic4qryCdtn(qrystr);
            Oss.testOss();
            string str = "KK园区,东方园区,金州园区,世纪新城园区";
            string path1 = "缅甸/妙瓦底/KK园区";

            string originalString = castToJsonArrstr(path1);
            Print(Encodeurl(originalString));
            string bbb = DecodeUrl("%5b%22KK%e5%9b%ad%e5%8c%ba%22%2c%22%e4%b8%9c%e6%96%b9%e5%9b%ad%e5%8c%ba%22%2c%22%e9%87%91%e5%b7%9e%e5%9b%ad%e5%8c%ba%22%2c%22%e4%b8%96%e7%ba%aa%e6%96%b0%e5%9f%8e%e5%9b%ad%e5%8c%ba%22%5d");

            //   string bbb = DecodeUrl("%5b%22KK%e5%9b%ad%e5%8c%ba%22%2c%22%e4%b8%9c%e6%96%b9%e5%9b%ad%e5%8c%ba%22%2c%22%e9%87%91%e5%b7%9e%e5%9b%ad%e5%8c%ba%22%2c%22%e4%b8%96%e7%ba%aa%e6%96%b0%e5%9f%8e%e5%9b%ad%e5%8c%ba%22%5d");

            string lxfs = "D:\\0prj\\mdsj\\mdsjprj\\cfg\\lxfs.txt";
            string jsonString = "[[\"Line\",\"123321\"],[\"电话\",\"231231231\"]]";
            jsonString = "[[\"Telegram\",\"123456\"],[\"Telegram\",\"8888\"]]";
            Hashtable ht = new Hashtable();
            ht.Add("Telegram", "4546");
            string v1006 = GetFieldAsStr(ht, "Telegram");
            Hashtable hashtable = CastToHashtbFrmparseLxfs(jsonString);
            Print(EncodeJsonFmt(hashtable));

            GenerateImageFromHtml("D:\\0prj\\mdsj\\mdsjprj\\cfg\\btns.htm", "btns405.jpg");
            for (int i = 0; i < 5; i++)
            {
                SortedList st = new SortedList();
                st.Add("id", "id" + i);
                st.Add("nm", "name" + i);
                SaveToJsonSngleFile(st, "datadir127");

            }
            List<SortedList> li127 = await ListFromDirJsonsAsync("datadir127");
            Print(EncodeJsonFmt(li127));

            //   ParseCertificate($"{prjdir}/cfg/certificate.crt");
            //   ParseCertificate("");
            var gg_apiky = "AIzaSyD3e-K8bH7-_vt7BYWXlyaAiGe_cIUpWnU";
            var gmlOauthKeyFl = "C:\\Intel\\Wireless\\client_secret_635470856727-rl5bi02li1aebf0ln04hdm1jpd67j3cs.apps.googleusercontent.com.json";
            NewThrd(() =>
            {
                //     gglML(gmlOauthKeyFl, "账单", "EmlDirBill");
            });

            //   GetAddr("EmlDir2");
            var nginccfg = "D:\\nginx-1.27.0\\conf\\nginx.conf";
            List<Hashtable> li = ParseNginxConfigV2(ReadAllText(nginccfg));
        }

     










        static async System.Threading.Tasks.Task EvtUpdateHdlrAsyncSafe(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {


            //  throw new Exception("myex");
            //   call_user_func(evt_aHandleUpdateAsync, botClient, update, cancellationToken, reqThreadId)

            //try todo map evt
            callAsyncNewThrdx(() =>
            {
                try
                {
                    string reqThreadId = geneReqid();
                    EvtUpdateHdlr(botClient, update, cancellationToken, reqThreadId);
                    //     throw new InvalidOperationException("An error occurred in the task.");

                }
                catch (jmp2endEx e22)
                {
                    Print("jmp2exitEx"); Print(e22.Message); ;
                }
                catch (Exception e)
                {
                    logErr2024(e, "evt_aHandleUpdateAsyncSafe", "errlogDir", null);
                }
                return 0;


            });
            //     Task.Run(async );
            //  int reqThreadId = Thread.CurrentThread.ManagedThreadId;


        }

        private static async void Bot_OnUpdate(object sender, UpdateEventArgs e)
        {
            var __METHOD__ = "Bot_OnUpdate";
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), e));


            dbgCls.PrintRet(__METHOD__, 0);
        }


        //收到消息时执行的方法
        static void EvtUpdateHdlr(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string reqThreadId)
        {
            //  throw new Exception("myex");

            var __METHOD__ = nameof(EvtUpdateHdlr);
            PrintCallFunArgs(__METHOD__, func_get_args(update));
            logCls.log("fun " + __METHOD__, func_get_args(update), null, "logDir", reqThreadId);
            Print(update?.Message?.Text);
            //    tts(update?.Message?.Text);
            // print(json_encode(update));
            Print("tag4520");



            CallAsyncNewThrd(() =>
            {
                Thread.Sleep(1500);
                bot_logRcvMsg(update);
                Thread.Sleep(6000);
                dbgpad = 0;
            });



            if (update?.Type == UpdateType.MyChatMember)
            {

                EvtnewUserJoin2024(update?.Message?.Chat?.Id, update?.Message?.NewChatMembers, update);

                return;
            }


            //======================设置地区==============

            BtmEvtSetAreaHdlrChk(update);

            //======================END 设置地区==============

            //-----------/cmd process
            CmdHdlrChk(update);

            //------------end cmd prcs--------

            if (update.Type == UpdateType.Message)
            {
                // 使用 Task.Run 启动一个新的任务
                callAsyncNewThrdx(() =>
                {

                    return CallxTryJmp(OnMsg, update, reqThreadId);

                });

            }

            if (update.Type == UpdateType.CallbackQuery)
            {
                try
                {
                    OnCallbk(update, reqThreadId);
                }
                catch (jmp2endEx e)
                {
                    return;
                }

            }

            if (update.Type == UpdateType.ChatMember)
            {
                UpdateEventArgs uea = new UpdateEventArgs();
                uea.Update = update;
                Bot_OnUpdate(null, uea);
            }

            if (update.Type == UpdateType.CallbackQuery)
            {
                //callback evt
                Dictionary<string, string> parse_str1 = parse_str(update.CallbackQuery.Data);
                if (LoadFieldAsStr(parse_str1, "ckuid") == "y") //def is not
                {
                    if (!StrEq(update.CallbackQuery?.From?.Username, update.CallbackQuery?.Message?.ReplyToMessage?.From?.Username))
                    {
                        botClient.AnswerCallbackQueryAsync(
                         callbackQueryId: update.CallbackQuery.Id,
                         text: "这是别人搜索的联系方式,如果你要查看联系方式请自行搜索",
                         showAlert: true); // 这是显示对话框的关键);
                        return;
                    }
                }

                if (LoadFieldAsStr(parse_str1, "btn") == "lkmenu") //def is not
                {
                    btnHdl_lookmenu(update.CallbackQuery);
                    return;
                }






            }



            if (update.Type == UpdateType.CallbackQuery)
            {
                Dictionary<string, string> parse_str1 = parse_str(update.CallbackQuery.Data);
                if (LoadFieldAsStr(parse_str1, "btn") == "dafenTips")
                    return;

            }


            if (update.Type == UpdateType.CallbackQuery)
            {
                Dictionary<string, string> parse_str1 = parse_str(update.CallbackQuery.Data);
                string btnname = LoadFieldAsStr(parse_str1, "btn");
                if (btnname.StartsWith("df") && btnname != "dafenTips")
                {
                    btnHdl_evtDafen(botClient, update, parse_str1);
                    return;
                }


            }


            biz_other._readMerInfo();



            //auto add cht sess
            if (update?.Message != null)
            {
                tglib.bot_saveChtSesion(update?.Message?.Chat?.Id, update?.Message?.From);
            }



            string msgx2024 = tglib.bot_getTxtMsgDep(update);
            string msg2056 = str_trim_tolower(msgx2024);
            //       tipDayu(msg2056, update);
            //if (System.IO.File.Exists("menu/" + msgx2024 + ".txt"))
            //{
            //    logCls.log(__METHOD__, func_get_args(), "Exists " + "menu/" + msgx2024 + ".txt", "logDir", reqThreadId);
            //    // var Keyboard = filex.wdsFromFileRendrToBtnmenu("menu/" + msgx2024 + ".txt");
            //    // var rkm = new InlineKeyboardMarkup(Keyboard);
            //    // KeyboardButton[][] kybd
            //    var Keyboard = filex.wdsFromFileRendrToTgBtmBtnmenuBycomma("menu/" + msgx2024 + ".txt");
            //    var rkm = new ReplyKeyboardMarkup(Keyboard);
            //    rkm.ResizeKeyboard = true;
            //    var msg = $"已为您切换至{msgx2024}菜单";
            //    evt_btm_menuitem_clickV2(update?.Message?.Chat?.Id, "今日促销商家.gif", msg, rkm, update);

            //    //botClient.SendTextMessageAsync()

            //    return;
            //}

            //if (msgx2024 == "↩️ 返回主菜单")
            //{
            //    timerCls.evt_ret_mainmenu_sendMsg4keepmenu4btmMenu(update?.Message?.Chat?.Id, "今日促销商家.gif", timerCls.plchdTxt, tgBiz.tg_btmBtnsV2(cast_toString(update?.Message?.Chat?.Type)));
            //    return;
            //}

            //if (msgx2024 == "↩️ 返回商家菜单")
            //{
            //    evt_retMchrtBtn_click(update);
            //    //     await evt_btmBtnclick(botClient, update);
            //    return;
            //}
            //endFUN  evt_btmBtnclick

            //if (msgx2024 == "↩️ 返回资源菜单")
            //{
            //    await evt_btmBtnclick(botClient, update);
            //    return;
            //}


            //LOOK DEP
            //if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery!.Data!.Contains("cmd="))
            //{
            //    await evt_btnclick(botClient, update);
            //    return;
            //}







            //add grp msgHDL
            if (update?.MyChatMember?.NewChatMember != null)
            {
                Callx(EvtBotEnterGrpEvtHdlr, update);
                return;
            }



            //   logCls.log(__METHOD__, func_get_args(),null,"logDir", reqThreadId);
            #region taskregn
            //_ = Task.Run(async (reqThreadId) =>
            //{
            if (update == null)
                return;

            var isAdminer = update.Message?.From?.Username == "GroupAnonymousBot" || update.CallbackQuery?.From?.Id == 5743211645;
            var text = update?.Message?.Text;

            #region @回复了商家详情信息  评价商家
            //@回复了商家详情信息
            if (update?.Message?.ReplyToMessage != null && (!string.IsNullOrEmpty(update?.Message?.Text))
            && update?.Message?.ReplyToMessage?.From?.Username == botname
             && update?.Message?.ReplyToMessage?.Caption?.Contains("--联系方式--") == true
              && update?.Message?.ReplyToMessage?.Caption?.Contains("商家排名") == true
                 && update?.Message?.ReplyToMessage?.Caption?.Contains("营业时间") == true
            //    && update?.Message?.ReplyToMessage?.Caption?.Contains("联系方式") == true
            )
            {
                msgHdl_evt_pinlunShangjia(botClient, update, isAdminer, text);
                return;
            }
            #endregion

            //添加商家信息
            #region 添加商家信息
            if (isAdminer
            && update?.Message != null
            && update?.Message?.Text?.Contains("打烊收摊时间") == true
            && string.IsNullOrEmpty(update.Message.ReplyToMessage?.Text)
            && update.Message.MessageThreadId == 111389)
            {
                添加商家信息(botClient, update, text);

                return;

            }
            #endregion


            #region 提示他人可搜索联系方式
            //提示他人可搜索联系方式
            ///    _contactType = ["商家联系方式", "商家飞机"];

            if (update?.Message != null && !string.IsNullOrEmpty(text) && _contactType.Any(u => text.Contains(u)))
            {
                try
                {
                    botClient.SendTextMessageAsync(update.Message.Chat.Id, "@回复本信息,搜商家联系方式", parseMode: ParseMode.Html, replyToMessageId: update.Message.MessageId);
                }
                catch (Exception e)
                {
                    Print("告知@回复本信息,搜商家联系方式时出错:" + e.Message);
                }
            }
            #endregion


            // 评价商家 按钮
            if (update?.Type is UpdateType.CallbackQuery)
            {
                if (update?.CallbackQuery?.Data?.Contains("Comment") == true)
                {
                    try
                    {
                        botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id, "@回复本消息,即可对本商家评价 !(100字以内)", true);
                    }
                    catch (Exception e)
                    {
                        Print("告诉别人怎么评价时出错:" + e.Message);
                    }
                    return;
                }



                //if (update?.CallbackQuery.Message?.ReplyToMessage?.From?.Id != update?.CallbackQuery.From.Id)
                //{
                //    try
                //    {
                //        await botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id, "您无权点击别人的搜索结果!", true);
                //    }
                //    catch (Exception e)
                //    {
                //       print("告诉对方您无权点击时出错:" + e.Message);
                //    }
                //    return;
                //}
            }


            //if nmrl msg  n notStartWith   @bot   ingor
            //if (tgBiz.bot_isNnmlMsgInGrp(update))
            //{
            //   print(" bot_isNnmlMsgInGrp():ret=>true");
            //    return;
            //}



            // if (msgx2024=="商家")

            //   SortedList whereMap2;
            //    msgHdlr4searchPrejude(botClient, update, reqThreadId);
            //pre page evt???  todo
            //next page evt,,,
            if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery!.Data!.Contains("page"))
            {
                btnHdl_evt_nextPrePage(botClient, update, reqThreadId);
                return;
            }

            //return evt
            if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery!.Data!.Contains("return"))
            {
                btnHdl_evt_ret_mchrt_list(botClient, update, reqThreadId);
                return;
            }


            //查看商家结果 defalt is detail view
            //         if (update.CallbackQuery.Data.StartsWith("Merchant?id="))
            if (update.Type is UpdateType.CallbackQuery)
            {
                Dictionary<string, string> parse_str1 = parse_str(update.CallbackQuery.Data);
                if (LoadFieldAsStr(parse_str1, "btn") == "dtl")
                {
                    BtnHdlEvtView(botClient, update, reqThreadId);
                }
                // logCls.log("FUN evt_msgTrgSrch", func_get_args(fuwuWd, reqThreadId), null, "logDir", reqThreadId);

            }



            #region add chatids
            tglib.tg_addChtid(update);

            #endregion
            //}, cancellationToken);
            #endregion

            //}
            //catch (Exception e)
            //{
            //    logCls.logErr2025(e, "evt_msg_rcv", "errlog");
            //}


        }


        public static void CmdHdlrChk(Update update)
        {
            if (!IsStartsWith(update?.Message?.Text, "/"))
                return;
            string reqThreadId = geneReqid();
            string cmd = GetCmdV2(update?.Message?.Text?.Trim());
            if (!string.IsNullOrEmpty(cmd) && cmd.Length < 100)
            {
                //+ update?.Message?.Chat?.Type ?? "" + ""
                //CmdXXHdlr
                string methodName = "CmdHdlr" + cmd;
                Callx(methodName, update?.Message?.Text, update, reqThreadId);
            }

            if (update.Type == UpdateType.Message)
            {
                if (update.Message.Type == MessageType.Text)
                {
                    if (update.Message.Text.Trim().StartsWith("/"))
                    {
                        if (update.Message.Chat.Type == ChatType.Private)
                        {
                            OnCmdPrvt(update.Message.Text.Trim(), update, reqThreadId);
                            Jmp2endDep();
                            return;
                        }
                        else
                        {
                            //    OnCmdPublic(update.Message.Text.Trim(), update, reqThreadId);
                            Jmp2endDep(); return;
                        }
                    }

                }
            }
        }






        public static void MsgHdlr4searchPrejude(ITelegramBotClient botClient, Update update, string reqThreadId)
        {
            string __METHOD__ = MethodBase.GetCurrentMethod().Name;
            #region sezrch
            string msgx2024 = tglib.bot_getTxtMsgDep(update);
            string msg2056 = str_trim_tolower(msgx2024);
            if (msg2056.StartsWith("/"))
                return;
            if (msg2056.StartsWith(PreCh))
                return;
            HashSet<string> hs11 = GetHashsetEmojiCmn();
            hs11.Add("/");
            AddElemtStrcomma(noTrigSrchMsgs, hs11);
            if (IsStartsWith(msg2056, hs11))
                return;
            if (IsStartsWithArrcomma(msg2056, noTrigSrchMsgs))
                return;
            HashSet<string> 商品与服务词库2 = GetHashset商品与服务词库();
            string fuwuci = GetFuwuci(update?.Message?.Text, 商品与服务词库2);
            //whereMap2 = new SortedList();
            //whereMap2.Add("fuwuci", fuwuci);

            //privt msg serch
            if (update?.Message?.Chat?.Type == ChatType.Private && update?.Type == UpdateType.Message)
            {
                HashSet<string> 商品与服务词库 = file_getWords商品与服务词库();
                if (!strCls.ContainKwds(update?.Message?.Text, string.Join(" ", 商品与服务词库)))
                {
                    Print(" 不包含商品服务词，ret");


                    ArrayList a = filex.rdWdsFromFile($"{prjdir}/menu/底部公共菜单.txt");
                    if (a.Contains(msg2056))
                        return;
                    botClient.SendTextMessageAsync(update.Message!.Chat.Id, "未搜索到商家,您可以向我们提交商家联系方式", parseMode: ParseMode.Html, replyToMessageId: update.Message.MessageId);

                    //tglib.bot_dltMsgThenSendmsg(update.Message!.Chat.Id, update.Message.MessageId, "未搜索到商家,您可以向我们提交商家联系方式", 5);
                    return;
                    //  return;
                }
                string fuwuWd = GetFuwuci(update?.Message?.Text, 商品与服务词库);
                // logCls.log(__METHOD__, func_get_args(),null,"logDir", reqThreadId);
                MsgHdlr4srch(botClient, update, update?.Message?.Text, fuwuWd, reqThreadId);
                return;
            }


            //public search jude
            if (isGrpChat(update?.Message?.Chat?.Type) && update?.Type == UpdateType.Message)
            {
                string? msgx = tglib.bot_getTxt(update);
                if (msgx == null || msgx.Length > 25)
                {
                    Print(" msgx == null || msgx.Length > 25 ");
                    return;
                }
                msgx = msgx.Trim();
                if (msgx.Trim().StartsWith("@" + botname)) //goto seasrch
                    msgx = msgx.Substring(botname.Length + 1).Trim();
                msgx = msgx.Trim();

                //--------------chke trig wd----------
                HashSet<string> trgWdSt = ReadWordsFromFile($"{prjdir}/cfg/搜索触发词.txt");
                var trgWd = string.Join(" ", trgWdSt);
                Print(" 触发词 chk");
                if (!strCls.ContainKwds(update?.Message?.Text, trgWd))
                {
                    Print(" 不包含触发词，ret");
                    return;
                }

                //bao含触发词，进一步判断


                //---------------chk fuwuci -------------

                //去除搜索触发词，比如哪里有
                msgx = msgx.Replace("联系方式", " ");
                HashSet<string> hs = GetSrchTrgWds();
                string msgx_remvTrigWd = ReplaceRemoveWords(msgx, hs);

                //是否包含搜索词 商品或服务关键词
                Print(" 商品或服务关键词 srch");
                HashSet<string> 商品与服务词库 = file_getWords商品与服务词库();
                if (!strCls.ContainKwds(msgx_remvTrigWd, string.Join(" ", 商品与服务词库)))
                {
                    Print(" 不包含商品服务词，ret");
                    return;
                }
                string fuwuWd = GetFuwuci(msgx_remvTrigWd, 商品与服务词库);
                if (fuwuWd == null)
                {
                    Print(" 不包含商品服务词，ret");
                    return;
                }



                MsgHdlr4srch(botClient, update, msgx_remvTrigWd, fuwuWd, reqThreadId);
                dbgCls.PrintRet(__METHOD__, 0);
                return;
            }


            #endregion
        }








        //private static void callx(Func<Update, Task> evt_btm_btn_click_inPrivtAsync, Update update)
        //{
        //    throw new NotImplementedException();
        //}



        private static void OnCallbk(Update update, string reqThreadId)
        {
            //daifu fun
            // throw new NotImplementedException();
            //string path = $"{prjdir}/cfg/{update.CallbackQuery.Data}.txt";
            //if (System.IO.File.Exists(path))
            //{ // Create the chat ID
            //    var chatId = 123456789;
            //    var txt = ReadAllText(path);
            //    //   botClient.SendChatActionAsync(chatId, ChatAction.al);
            //    try
            //    {
            //        await botClient.AnswerCallbackQueryAsync(
            //               callbackQueryId: update.CallbackQuery.Id,
            //               text: txt,
            //               showAlert: true); // 这是显示对话框的关键);
            //    }catch(Exception e)
            //    {
            //       print(e);
            //    }

            //    jmp2exit();
            //    return ;
            //}
            //// if(update.CallbackQuery.Data)
        }

        public static void OnMsg(Update update, string reqThreadId)
        {
            int n = containCalcCntScoreSetfmt(update.Message.Text, LoadHashset(" 盘口 博彩 菠菜 玩家 赔率 世博 杀大赔小 赔率"));
            if (n > 1)
            {
                Callx(evt_shiboBocai_click, update);
                return;
            }
            if (update?.Message?.ReplyToMessage?.From?.Username == botname &&
            strCls.Contain(update?.Message?.Text, "世博博彩")
             )
            {
                Callx(evt_shiboBocai_click, update);
                return;
            }

            if (strCls.Contain(update?.Message?.Text, "世博博彩")
              )
            {
                Callx(evt_shiboBocai_click, update);
                return;
            }
            if (update?.Message?.NewChatMembers != null)
            {
                Callx(EvtBotEnterGrpEvtHdlr, update);
                return;
            }
            //todo 
            //当检测到用户再聊菠菜相关话题时,也要提示:

            //callxTryJmp(xxcc, update); 

            callTryAll(() =>
            {
                //排除指令  设置地区
                HashSet<string> hs11 = new HashSet<string>();
                hs11.Add("/");   //排除im指令
                hs11.Add(PreCh);   //排除底部命令 设置地区
                //  //排除指令提示   请选择xxx
                AddElemtStrcomma(noTrigSrchMsgs, hs11);
                if (IsStartsWith(update?.Message?.Text, hs11))
                    return;
                Callx(msgTrgBtmbtnEvtHdlr11, update);
                Callx(msgxTrigBtmbtnEvtHdlr, update);

                Callx(MsgHdlr4searchPrejude, botClient, update, "111");
            });



            //ad chk

            Print(update.Message?.Type);
            if (update.Message?.Type == MessageType.Text)
            {
                Print(update.Message?.Type);
                bot_adChk(update);
            }
            string msgx2024 = tglib.bot_getTxtMsgDep(update);
            string msg2056 = str_trim_tolower(msgx2024);
            tipDayu(msg2056, update);

        }



        public static void msgTrgBtmbtnEvtHdlr11(Update update)
        {
            if (string.IsNullOrEmpty(update.Message?.Text))
            {
                PrintRetx(nameof(msgTrgBtmbtnEvtHdlr11), "txt is empty");
                return;
            }

            // ----------btm btn hdlr 
            //if (update?.Message?.Text == "\U0001fac2 加入联信")
            //{
            //    //if (update.Message.Chat.Type == ChatType.Private)
            //    {
            //        callx(evt_btm_btn_click_inPrivtAsync, update);
            //        return;
            //    }
            //}



            // if (tg_isBtm_btnClink_in_prvt(update))
            // {
            //加入联系和   btnCfgForeach
            Callx(evt_btm_btn_click, update);
            //   return;
            //  }
            //menu proces   evt_btmBtnclick
            //if (tgBiz.tg_isBtm_btnClink_in_pubGrp(update))
            //{
            //    callx(evt_btm_btn_click_inPubgrp, update);
            //    return;
            //}

            if (update?.Message?.Text == juliBencyon)
            {
                if (update.Message.Chat.Type != ChatType.Private)
                {
                    Callx(evt_btm_btn_zhuliBenqunAsync, update);
                    return;
                }
            }
            //   callx(update?.Message?.Text == juliBencyon&& update.Message.Chat.Type != ChatType.Private, evt_btm_btn_zhuliBenqunAsync, update);
            //---------------end btm btn
        }


        private static void msgxTrigBtmbtnEvtHdlr(Update update)
        {
            const string METHOD__ = nameof(msgxTrigBtmbtnEvtHdlr);

            Print("--------btm btn trig start...----------");
            HashSet<string> hs = GetSrchTrgWds();
            if (!ContainKwdsV2(update?.Message?.Text, hs))
            {
                Print(" 不包含触发词，ret");
                return;
            }
            var btnName = getBtnnameFromTxt(update.Message.Text);

            print_varDump(METHOD__, "包含btnName", btnName);
            if (btnName == "")
            {
                Print(" 不包含btnName，ret");
                //  return;
            }
            else
            {
                Callx(BtmBtnClkinCfgByMsg, update, btnName);
                return;
            }


            var extWd = getBtnExtWdFromTxt(update.Message.Text);
            print_varDump(METHOD__, "get extWd", extWd);
            if (extWd == "")
            {
                Print(" 不包含extWd，ret");
                return;
            }
            btnName = convertExtWd2btnname(extWd);

            // if (tg_isBtm_btnClink_in_pubGrp(update))
            {

                Callx(BtmBtnClkinCfgByMsg, update, btnName);
                //  await callxAsync(btm_btnClk, update);


            }
            Print("-------- end btm btn trig start...----------");

        }



        //todo btnHdl_lookmenu
        private static void btnHdl_lookmenu(CallbackQuery? callbackQuery)
        {
            //如果展开菜单
            // if ( string.IsNullOrEmpty(contact_Merchant.Menu))
            {
                try
                {
                    botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "该商家暂未提供菜单", true);
                }
                catch (Exception e)
                {
                    Print("点击查看菜单,告知未提供菜单时时出错:" + e.Message);
                }
                return;
            }
        }

        private static void btnHdl_evtDafen(ITelegramBotClient botClient, Update update, Dictionary<string, string> parse_str1)
        {
            //evet dafen 
            if (LoadFieldAsStr(parse_str1, "ckuid") == "y")
            {
                if (!StrEq(update.CallbackQuery?.From?.Username, update.CallbackQuery?.Message?.ReplyToMessage?.From?.Username))
                {
                    botClient.AnswerCallbackQueryAsync(
                callbackQueryId: update.CallbackQuery.Id,
                text: "这是别人搜索的联系方式,如果你要查看联系方式请自行搜索",
                showAlert: true); // 这是显示对话框的关键);
                    return;
                }

            }
            //not need chk uid
            botClient.AnswerCallbackQueryAsync(
            callbackQueryId: update.CallbackQuery.Id,
            text: "打分成功",
            showAlert: true); // 这是显示对话框的关键);
            return;
        }


        private static void EvtnewUserJoin2024(long? chatId, Telegram.Bot.Types.User[]? newChatMembers, Update? update)
        {
            if (newChatMembers != null)
            {
                foreach (Telegram.Bot.Types.User u in newChatMembers)
                {
                    if (u != null)
                        evt_newUserjoinSngle(chatId, u?.Id, u, update);
                }

            }

            if (newChatMembers == null)
                evt_newUserjoinSngle(chatId, update.MyChatMember.NewChatMember.User.Id, update.MyChatMember.NewChatMember.User, update);
        }







        private static void MsgHdlr4srch(ITelegramBotClient botClient, Update update, string msgx_remvTrigWd, string fuwuWd, string reqThreadId)
        {
            logCls.log("FUN evt_msgTrgSrch", func_get_args(fuwuWd, reqThreadId), null, "logDir", reqThreadId);
            SortedList whereMap = new SortedList();
            whereMap.Add("fuwuci", fuwuWd);
            var __METHOD__ = "evt_msgTrgSrch";
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod()));

            string? msgx = msgx_remvTrigWd;
            //tglib.bot_getTxtMsgDep(update);
            if (msgx.Trim().StartsWith("@" + botname))
                msgx = msgx.Substring(botname.Length + 1).Trim();
            msgx = msgx.Trim();

            msgx = msgx.Replace("联系方式", " ");
            //去除搜索触发词，比如哪里有
            HashSet<string> hs = GetSrchTrgWds();
            msgx = ReplaceRemoveWords(msgx, hs);
            // 搜索触发词
            string msgx_remvTrigWd2 = msgx;





            if (msgx != null && msgx.Length < 25)
            {
                GetList_qryV2(msgx_remvTrigWd2, 1, 5, botClient, update, reqThreadId);
                dbgCls.PrintRet(__METHOD__, 0);

                return;
            }
            else
            {
                Print(" msg is null or leng>25");
                dbgCls.PrintRet(__METHOD__, 0);
                return;
            }


        }


        private static void btnHdl_evt_nextPrePage(ITelegramBotClient botClient, Update update, string reqThreadId)
        {
            string? msgx = tglib.bot_getTxtMsgDep(update);

            if (msgx != null)
            {
                if (msgx.Trim().StartsWith("@" + botname))
                    msgx = msgx.Substring(19).Trim();
                msgx = msgx.Trim();
                GetList_qryV2(msgx, 1, 5, botClient, update, reqThreadId);
                return;
            }
        }

        private static void btnHdl_evt_ret_mchrt_list(ITelegramBotClient botClient, Update update, string reqThreadId)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(update, reqThreadId));

            logCls.log("fun evt_ret_mchrt_list", func_get_args(update, reqThreadId), "", "logDir", reqThreadId);
            string? msgx = tglib.bot_getTxtMsgDep(update);
            // if msg==null ..just from timer send msg..ret no op
            if (msgx != null)
            {
                if (msgx.Trim().StartsWith("@" + botname))
                    msgx = msgx.Substring(19).Trim();
                msgx = msgx.Trim();
                GetList_qryV2(msgx, 1, 5, botClient, update, reqThreadId);
                return;
            }

            dbgCls.PrintRet(__METHOD__, 0);
        }



        //private static async Task<Message> evt_btmbtn_clk_mltBtns(Update update, Message? msgNew, string f1)
        //{

        //    return msgNew;
        //}




        //private static async Task evt_btmBtnclick(ITelegramBotClient botClient, Update update)
        //{
        //    //evt_inline_menuitem_click
        //    //dataObj
        //    //Dictionary<string, StringValues> whereExprsObj = QueryHelpers.ParseQuery(update.CallbackQuery!.Data);
        //    //var msgx = arrCls.TryGetValueDfEmpy(whereExprsObj, "cmd");
        //    string msgx = tglib.bot_getTxtMsg(update);
        //    msgx = msgx.Trim();
        //    var msgx2024 = msgx;


        //    if (msgx == "返回商家菜单")
        //    {
        //        evt_retMchrtBtn_click(update);
        //    }


        //    if (System.IO.File.Exists("menu/" + msgx2024 + ".txt"))
        //    {
        //        // var Keyboard = filex.wdsFromFileRendrToBtnmenu("menu/" + msgx2024 + ".txt");
        //        // var rkm = new InlineKeyboardMarkup(Keyboard);
        //        var Keyboard = filex.wdsFromFileRendrToTgBtmBtnmenu("menu/" + msgx2024 + ".txt");
        //        var rkm = new ReplyKeyboardMarkup(Keyboard);
        //        timerCls.evt_btm_menuitem_clickV2(update?.Message?.Chat?.Id, "今日促销商家.gif", timerCls.plchdTxt, rkm, update);

        //        //botClient.SendTextMessageAsync()

        //        return;
        //    }

        //    //if (System.IO.File.Exists("menu/" + msgx + ".txt"))
        //    //{
        //    //    InlineKeyboardButton[][] Keyboard = filex.wdsFromFileRendrToBtnmenu("menu/" + msgx + ".txt");
        //    //    var rkm = new InlineKeyboardMarkup(Keyboard);
        //    //    timerCls.evt_inline_menuitem_click_showSubmenu(update?.CallbackQuery?.Message?.Chat?.Id, "今日促销商家.gif", timerCls.plchdTxt, rkm, update);
        //    //    return;
        //    //}

        //    //evt msg trg 
        //    //if cant find menu,,search
        //    //await evt_btnclick_Pt2_qryByKwd(msgx, 1, 5, botClient, update);
        //    await evt_msgTrgSrch(botClient, update);
        //    return;
        //}

        //private static void evt_retMchrtBtn_click(Update update)
        //{
        //    var Keyboard = filex.wdsFromFileRendrToTgBtmBtnmenuBycomma("menu/商家.txt");

        //    var rkm = new ReplyKeyboardMarkup(Keyboard);
        //    rkm.ResizeKeyboard = true;
        //    evt_btm_menuitem_clickV2(update?.Message?.Chat?.Id, "今日促销商家.gif", timerCls.plchdTxt, rkm, update);

        //    //  timerCls.evt_inline_menuitem_click_showSubmenu(update?.CallbackQuery?.Message?.Chat?.Id, "今日促销商家.gif", timerCls.plchdTxt, rkm, update);
        //    return;
        //}


        //LOOK DEP    INLINE BTN CLICK
        //private static async Task evt_btnclick(ITelegramBotClient botClient, Update update)
        //{
        //    //evt_inline_menuitem_click
        //    //dataObj
        //    Dictionary<string, StringValues> whereExprsObj = QueryHelpers.ParseQuery(update.CallbackQuery!.Data);
        //    var msgx = arrCls.TryGetValueDfEmpy(whereExprsObj, "cmd");
        //    msgx = msgx.Trim();


        //    if (msgx == "返回商家菜单")
        //    {
        //        InlineKeyboardButton[][] Keyboard = filex.wdsFromFileRendrToBtnmenu("menu/商家.txt");
        //        var rkm = new InlineKeyboardMarkup(Keyboard);
        //        timerCls.evt_inline_menuitem_click_showSubmenu(update?.CallbackQuery?.Message?.Chat?.Id, "今日促销商家.gif", timerCls.plchdTxt, rkm, update);
        //        return;
        //    }

        //    if (System.IO.File.Exists("menu/" + msgx + ".txt"))
        //    {
        //        InlineKeyboardButton[][] Keyboard = filex.wdsFromFileRendrToBtnmenu("menu/" + msgx + ".txt");
        //        var rkm = new InlineKeyboardMarkup(Keyboard);
        //        timerCls.evt_inline_menuitem_click_showSubmenu(update?.CallbackQuery?.Message?.Chat?.Id, "今日促销商家.gif", timerCls.plchdTxt, rkm, update);
        //        return;
        //    }

        //    //if cant find menu,,search
        //    await evt_btnclick_Pt2_qryByKwd(msgx, 1, 5, botClient, update);
        //    return;
        //}

        private static void msgHdl_evt_pinlunShangjia(ITelegramBotClient botClient, Update update, bool isAdminer, string? text)
        {
            if (text.StartsWith("@xxx007"))
                return;
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), isAdminer, text));

            HashSet<prjx.City> _citys = getCitysObj();
            Print(" evt  @回复了商家详情信息  评价商家");
            var updateString = JsonConvert.SerializeObject(update);
            Match match = Regex.Match(updateString, @"(?<=\?id=).*?(?=&)");
            Merchant? merchant = match.Success ? (from c in _citys
                                                  from area in c.Address
                                                  from am in area.Merchant
                                                  where am.Guid == match.Value
                                                  select am).FirstOrDefault() : null;


            if (merchant == null)
            {
                Print("未找到目标商家");
                return;
            }

            //普通用户评价商家
            if (!isAdminer)
            {
                //如果是评价
                if (text.Length > 100)
                {
                    Telegram.Bot.Types.Message msg = null;
                    try
                    {
                        msg = botClient.SendTextMessageAsync(chatId: update.Message!.Chat.Id, text: "评价失败,评价文字只能100个字以内!", replyToMessageId: update.Message.MessageId).Result;
                    }
                    catch (Exception ex)
                    {
                        Print("告知评价字数不超过100时出错:" + ex.Message);
                    }

                    if (msg != null)
                    {
                        System.Threading.Tasks.Task.Delay(5000);
                        try
                        {
                            botClient.DeleteMessageAsync(msg.Chat.Id, msg.MessageId).GetAwaiter().GetResult();
                        }
                        catch (Exception ex)
                        {
                            Print("删除告知评价字数不可超过100字时出错:" + ex.Message);
                        }
                    }
                    return;
                }

                try
                {
                    merchant.Comments.Add((long)update!.Message.From.Id, text);
                }
                catch (Exception e)
                {
                    Print(e);
                }


                User? user = null;
                if (_users.ContainsKey((long)update.Message.From.Id))
                {
                    user = _users[(long)update.Message.From.Id];
                }
                else
                {
                    user = new User();
                    _users.Add((long)update.Message.From.Id, user);
                }

                SortedList obj1 = new SortedList();
                obj1.Add("id", DateTime.Now.ToString());
                obj1.Add("商家guid", merchant.Guid);
                obj1.Add("商家", merchant.Name);
                obj1.Add("时间", DateTime.Now.ToString());
                obj1.Add("评论内容", text);
                obj1.Add("评论人", update.Message.From.Username);
                obj1.Add("评论人id", update.Message.From.Id);
                System.IO.Directory.CreateDirectory("pinlunDir");
                ormSqlt.Save4Sqlt(obj1, "pinlunDir/" + merchant.Guid + merchant.Name + ".db");
                ormJSonFL.SaveJson(obj1, "pinlunDir/" + merchant.Guid + merchant.Name + ".json");

                user.Comments++;
                biz_other._SaveConfig();
                try
                {
                    tglib.bot_dltMsgThenSendmsg(update.Message!.Chat.Id, update.Message.MessageId, "成功点评了商家,本消息10秒后删除!", 10);
                }
                catch (Exception ex)
                {
                    Print("告知成功点评了商家时出错:" + ex.Message);
                }
            }
            //管理修改商家信息
            else
            {
                var value = GetText.Getright(text, "=");

                if (text.Contains("商家菜单=") == false && text.Contains("\n") == true || string.IsNullOrEmpty(value))
                {
                    try
                    {
                        tglib.bot_dltMsgThenSendmsg(update.Message.Chat.Id, update.Message.MessageId, "编辑信息格式有误!", 5);
                    }
                    catch (Exception ex)
                    {
                        Print("告知编辑消息时出错:" + ex.Message);
                    }
                    return;
                }

                //如果是修改商家信息
                if (text.Contains("商家名称="))
                {
                    merchant.Name = value;
                }
                else if (text.Contains("商家分类="))
                {
                    try
                    {
                        merchant.Category = (Category)Convert.ToInt32(value);
                    }
                    catch (Exception ex)
                    {
                        Print("编辑商家分类时出错:" + ex.Message);
                        return;
                    }
                }
                else if (text.Contains("商家关键词="))
                {
                    merchant.KeywordString = value;
                }
                else if (text.Contains("开始营业时间="))
                {
                    try
                    {
                        merchant.StartTime = TimeSpan.Parse(value);
                    }
                    catch (Exception ex)
                    {
                        Print("编辑商家开始营业时出错:" + ex.Message);
                        return;
                    }
                }
                else if (text.Contains("打烊收摊时间="))
                {
                    try
                    {
                        merchant.EndTime = TimeSpan.Parse(value);
                    }
                    catch (Exception ex)
                    {
                        Print("编辑商家截止营业时出错:" + ex.Message);
                        return;
                    }
                }
                else if (text.Contains("Telegram="))
                {
                    merchant.Telegram = value.Split(' ').ToList();
                }
                else if (text.Contains("Telegram群组="))
                {
                    merchant.TelegramGroup = value;
                }
                else if (text.Contains("Whatsapp="))
                {
                    merchant.WhatsApp = value.Split(' ').ToList();
                }
                else if (text.Contains("Line="))
                {
                    merchant.Line = value.Split(' ').ToList();
                }
                else if (text.Contains("Signal="))
                {
                    merchant.Signal = value.Split(' ').ToList();
                }
                else if (text.Contains("微信="))
                {
                    merchant.WeiXin = value.Split(' ').ToList();
                }
                else if (text.Contains("电话="))
                {
                    merchant.Tel = value.Split(' ').ToList();
                }
                else if (text.Contains("商家菜单="))
                {
                    merchant.Menu = value;
                }
                else
                {
                    try
                    {
                        tglib.bot_dltMsgThenSendmsg(update.Message.Chat.Id, update.Message.MessageId, "编辑信息格式有误!", 5);
                    }
                    catch (Exception ex)
                    {
                        Print("告知编辑消息时出错:" + ex.Message);
                    }
                    return;
                }

                biz_other._SaveConfig();

                try
                {
                    tglib.bot_dltMsgThenSendmsg(update.Message.Chat.Id, update.Message.MessageId, "商家信息编辑成功!", 5);
                }
                catch (Exception ex)
                {
                    Print("告知编辑成功时出错:" + ex.Message);
                }
            }

            dbgCls.PrintRet(__METHOD__, 0);

        }


        /// <summary>
        /// ，指令如下:\n"
           //         + "/设置园区 东风园区\n",
        /// </summary>
        /// <param name="update"></param>
        public static void EvtBotEnterGrpEvtHdlr(Update update)
        {
            var chatid = update?.MyChatMember?.Chat?.Id;
            if (chatid == null)
                chatid = update?.Message?.Chat?.Id;
            ReplyKeyboardMarkup rkm = tgBiz.tg_btmBtnsV2(update?.Message?.Chat?.Type);
            botClient.SendTextMessageAsync(
                    chatid,
                     "我是联信便民助手,你们要问什么商家我都知道.联信是一个集纵网观察、信息搜集、资源整合，旨在为大家解决信息不透明和资源不可信的权威便民助手."
                        + "\n可以设置园区方便搜索",
                     parseMode: ParseMode.Html,
                      replyMarkup: rkm,
                     protectContent: false,
                     disableWebPagePreview: true);
            Callx(bot_saveGrpInf2db, update.MyChatMember);
            Callx(bot_saveChtSesion, chatid, update.MyChatMember);

            SetBtmBtnMenuClr("", plchdTxt, update.Message.Chat.Id, update.Message.Chat.Type.ToString());

        }




        //回调告知怎么添加和修改商家信息
        //                    if (update.CallbackQuery!.Data?.Contains("AddMerchant")==true || update.CallbackQuery!.Data?.Contains("Update")==true)
        //                    {
        //                        if (!isAdminer)
        //                        {
        //                            try
        //                            {
        //                                await botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id, "本功能仅供左道管理员使用", true);
        //                            }
        //                            catch (Exception e)
        //                            {
        //                               print("告诉别人怎么评价时出错:" + e.Message);
        //                            }
        //                            return;
        //                        }

        //                        text = string.Empty;
        //                        text = @"⚠️@" + update.CallbackQuery.From.Username + " (" + update.CallbackQuery.From.FirstName + update.CallbackQuery.From.LastName + ")";
        //                        text += "严格按照指定格式和(英文半角符号)符号编辑,否则编辑失败!\n\n";

        //                        //按钮回调(添加商家)
        //                        if (update.CallbackQuery.Data.Contains("AddMerchant"))
        //                        {
        //                            text += @"<b>✏️ 添加商家文本格式</b> 
        //<blockquote>城市园区名字:这里输入商家名称
        //商家名称:这里输入商家名称
        //商家分类:这是分类编号,详见底部分类编号
        //商家关键词:这里输入商家关键词,每个关键词用空格隔开
        //开始营业时间:00:00:00
        //打烊收摊时间:23:00:00
        //Telegram:可多个账号,用空格隔开
        //Telegram群组:可多个账号,用空格隔开
        //Whatsapp:可多个账号,用空格隔开
        //Line:可多个账号,用空格隔开
        //Signal:可多个账号,用空格隔开
        //微信:可多个账号,用空格隔开
        //电话:可多个账号,用空格隔开
        //商家菜单:这里输入菜单
        //</blockquote>
        //<b>商家分类编码:</b>
        //餐馆美食:<code>0</code>
        //奶茶饮品:<code>1</code>
        //水果店:<code>2</code>
        //电子手机电脑店:<code>3</code>
        //理发/美甲/美容/医美/纹身:<code>4</code>
        //兑换典当:<code>5</code>
        //按摩/会所/KTV/酒吧:<code>6</code>
        //超市/商店/菜市场:<code>7</code>
        //车辆相关:<code>8</code>
        //仓库/快递/物流/跑腿:<code>9</code>
        //医院/诊所/牙科:<code>10</code>
        //酒店宾馆住宿:<code>11</code>
        //黄金首饰:<code>12</code>
        //服装/鞋包:<code>13</code>
        //宠物店:<code>14</code>
        //物业:<code>15</code>";
        //                        }
        //                        //按钮回调(修改商家信息)
        //                        else if (update.CallbackQuery.Data.Contains("Update"))
        //                        {
        //                            text += @"<b>✏️ 编辑商家格式(@商家联系方式信息,回复以下格式即可修改,一次只能修改一个字段)</b> 

        //修改商家名称
        //<blockquote>商家名称=这里输入商家名称</blockquote>

        //修改商家分类
        //<blockquote>商家分类=这是分类编号,详见底部分类编号</blockquote>

        //修改商家关键词
        //<blockquote>商家关键词=这里输入商家关键词,每个关键词用空格隔开</blockquote>

        //修改开始营业时间
        //<blockquote>开始营业时间=00:00:00</blockquote>

        //修改打烊收摊时间
        //<blockquote>打烊收摊时间=23:00:00</blockquote>

        //修改Telegram
        //<blockquote>Telegram=可多个账号,用空格隔开</blockquote>

        //修改Telegram群组
        //<blockquote>Telegram群组=可多个账号,用空格隔开</blockquote>

        //修改Whatsapp
        //<blockquote>Whatsapp=可多个账号,用空格隔开</blockquote>

        //修改Line
        //<blockquote>Line=可多个账号,用空格隔开</blockquote>

        //修改Signal
        //<blockquote>Signal=可多个账号,用空格隔开</blockquote>

        //修改微信
        //<blockquote>微信=可多个账号,用空格隔开</blockquote>

        //修改电话
        //<blockquote>电话=可多个账号,用空格隔开</blockquote>

        //修改商家菜单
        //<blockquote>商家菜单=这里输入菜单</blockquote>

        //<b>商家分类编码:</b>
        //餐馆美食:<code>0</code>
        //奶茶饮品:<code>1</code>
        //水果店:<code>2</code>
        //电子手机电脑店:<code>3</code>
        //理发/美甲/美容/医美/纹身:<code>4</code>
        //兑换典当:<code>5</code>
        //按摩/会所/KTV/酒吧:<code>6</code>
        //超市/商店/菜市场:<code>7</code>
        //车辆相关:<code>8</code>
        //仓库/快递/物流/跑腿:<code>9</code>
        //医院/诊所/牙科:<code>10</code>
        //酒店宾馆住宿:<code>11</code>
        //黄金首饰:<code>12</code>
        //服装/鞋包:<code>13</code>
        //宠物店:<code>14</code>
        //物业:<code>15</code>";
        //                        }

        //                        Message? msg = null;
        //                        try
        //                        {
        //                            msg = await botClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, text, update.CallbackQuery.Message.MessageThreadId, parseMode: ParseMode.Html, null, false, null, false);
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                           print("返回按钮添加商家回调出错:" + ex.Message);
        //                        }

        //                        return;
        //                    }

        //qry shaojia
        //获取列表,或者是返回至列表
        static void GetList_qryV2(string msgx_remvTrigWd2, int pagex, int pagesizex, ITelegramBotClient botClient, Update update, string reqThreadId)
        {
            var __METHOD__ = "GetList_qryV2";  //bcs in task so cant get currentmethod
            PrintCallFunArgs(__METHOD__, func_get_args(__METHOD__, msgx_remvTrigWd2));
            logCls.log("fun GetList_qryV2", func_get_args(msgx_remvTrigWd2, pagex, pagesizex), "", "logDir", reqThreadId);
            if (msgx_remvTrigWd2 == null || msgx_remvTrigWd2.Length == 0)
                return;
            // print(" fun  GetList()");
            if (update.Type is UpdateType.Message && string.IsNullOrEmpty(update.Message?.Text)
                || update.Type is UpdateType.CallbackQuery && string.IsNullOrEmpty(update?.CallbackQuery?.Message?.ReplyToMessage?.Text))
                return;

            //页码
            int page = 0;
            //搜索结果数
            int count = 0;
            //获取操作用户
            User? user;
            if (update.Type is UpdateType.Message)
            {
                if (_users.ContainsKey((long)update.Message.From.Id))
                {
                    user = _users[(long)update?.Message?.From.Id];
                }
                else
                {
                    user = new User();
                    _users.Add((long)update?.Message?.From.Id, user);
                }
            }
            else
            {
                if (_users.ContainsKey((long)update?.CallbackQuery?.From?.Id))
                {
                    user = _users[(long)update?.CallbackQuery?.From?.Id];
                }
                else
                {
                    user = new User();
                    _users.Add((long)update?.CallbackQuery?.From?.Id, user);
                }
            }


            if (update.Type is UpdateType.CallbackQuery)
            {
                var uri = new Uri("https://t.me/" + update.CallbackQuery?.Data);
                var parameters = QueryHelpers.ParseQuery(uri.Query);
                parameters.TryGetValue("page", out var pageStr);
                if (!string.IsNullOrEmpty(pageStr))
                    page = Convert.ToInt32(pageStr);
            }
            const int pagesize = 5;
            List<InlineKeyboardButton[]> results = [];

            //搜索关键词  Merchant.json to citys



            if (update.Type == UpdateType.CallbackQuery)  //for ret to list commd
                msgx_remvTrigWd2 = update?.CallbackQuery?.Message?.ReplyToMessage?.Text;

            msgx_remvTrigWd2 = msgx_remvTrigWd2.Trim();

            //kwd if ret list btn cmd cmd
            if (update.Type == UpdateType.CallbackQuery)
            {
                if (msgx_remvTrigWd2.Trim().StartsWith("@" + botname))
                    msgx_remvTrigWd2 = msgx_remvTrigWd2.Substring(19).Trim();
                else
                    msgx_remvTrigWd2 = msgx_remvTrigWd2.Trim();
            }


            Print("  msg=>" + msgx_remvTrigWd2);

            if (string.IsNullOrEmpty(msgx_remvTrigWd2))
            {
                Print("IsNullOrEmpty(msgx_remvTrigWd2)");
                return;
            }


            //    List<InlineKeyboardButton[]> results = [];  &park=世纪新城园区
            if (tg_isGrpChat(update))
            {
                // update.Message.Chat.Id;
                string chatid2249 = tglib.bot_getChatid(update).ToString();

                //  List<Dictionary<string, string>> lst = ormSqlt._qryV2($"select * from grp_loc_tb where grpid='{groupId}'", "grp_loc.db");

                string grpcfg = $"{prjdir}/grpCfgDir/grpcfg{chatid2249}.json";
                List<SortedList> lst = ormJSonFL.qry(grpcfg);
                string whereExprs = (string)db.getRowVal(lst, "whereExprs", "");
                //    city = "


                // whereExprsObj.Add("fuwuci", ldfld_TryGetValueAsStrDefNull(whereMap, "fuwuci"));
                //here only one db so no mlt ,todo need updt
                // results = mrcht.qryByMsgKwdsV3(patns_dbfs, whereExprsObj);
                //qry from mrcht by  where exprs  strFmt
                Dictionary<string, StringValues> whereExprsObjFiltrs = QueryHelpers.ParseQuery(whereExprs);
                string sharNames = LoadFieldTryGetValue(whereExprsObjFiltrs, "@share");
                results = mrcht.qryFromMrchtV2("mercht商家数据", sharNames, whereExprs, msgx_remvTrigWd2);

            }
            else
            { //privet serach
              // update.Message.Chat.Id;
                string chatid2249 = tglib.bot_getChatid(update).ToString();

                string dbfile = $"{prjdir}/cfg_prvtChtPark/{chatid2249}.json";

                SortedList cfg = findOne(dbfile);

                Dictionary<string, StringValues> whereExprsObj = CopySortedListToDictionary(cfg);
                var whereExprsObjExprs = CastHashtableToQuerystringNoEncodeurl(whereExprsObj);
                //todo set    limit  cdt into 
                results = mrcht.qryFromMrchtV2("mercht商家数据", null, whereExprsObjExprs, msgx_remvTrigWd2);

            }

            //  results = arrCls.rdmList<InlineKeyboardButton[]>(results);
            count = results.Count;

            //GetList_qryV2 
            if (count == 0 && (update?.Message?.Chat?.Type == ChatType.Private))
            {
                ArrayList a = filex.rdWdsFromFile($"{prjdir}/menu/底部公共菜单.txt");
                if (a.Contains(update?.Message?.Text))
                    return;
                tglib.bot_dltMsgThenSendmsg(update.Message!.Chat.Id, update.Message.MessageId, "未搜索到商家,您可以向我们提交商家联系方式", 5);
                return;
            }

            if (count == 0)   //in pubgrp
            {
                Print(" evt serch.  in public grp. srch rzt cnt =0,so ret");
                return;
            }

            results = results.Skip(page * pagesize).Take(pagesize).ToList();


            //发起查询  stzrt with @bot
            //if (update!.Type is UpdateType.Message)
            //{
            //    // keyword = update?.Message?.Text;
            //    //   keyword = keyword.Substring(19).Trim();
            //    //if (msgx?.Length is < 2 or > 8)
            //    //{
            //    //    await bot_DeleteMessage(update.Message!.Chat.Id, update.Message.MessageId, "请输入2-8个字符的的关键词", 5);
            //    //    return;
            //    //}

            //    if (count == 0)
            //    {

            //        //await botapi.bot_DeleteMessage(update.Message!.Chat.Id, update.Message.MessageId, "未搜索到商家,您可以向我们提交商家联系方式", 5);
            //        return;
            //    }
            //    user.Searchs++;
            //}
            ////返回列表
            //else
            //{
            //    var cq = update!.CallbackQuery!;
            //    if (string.IsNullOrEmpty(msgx_remvTrigWd2))
            //    {
            //        try
            //        {
            //            await botClient.AnswerCallbackQueryAsync(cq.Id, "搜索关键词已经删除,需重新搜索!", true);
            //            await botClient.DeleteMessageAsync(cq.Message!.Chat.Id, cq.Message.MessageId);
            //        }
            //        catch (Exception e)
            //        {
            //           print("告知搜索关键词已经删除时出错:" + e.Message);
            //        }
            //        return;
            //    }
            //    user.Returns++;
            //}



            // pagebtns
            EvtsearchPt3btnsPagging(page, count, pagesize, results);

            try
            {
                var text = "";// $"😙 <b>搜到{count}个商家,被搜得越多越靠前!</b>\n";// +
                              //$"<blockquote>您的统计:搜索{user.Searchs}  返列表{user.Returns}  查看数{user.Views}" +
                              //$"  看菜单{user.ViewMenus}  打分{user.Scores}  评价{user.Comments}</blockquote>";
                text += " \n " + timerCls.plchdTxt;
                //第一次搜索时返回的列表
                if (update?.Message != null)
                {


                    string Path = "搜索横幅.gif";
                    //     var text = "——————————————";
                    // print(string.Format("{0}-{1}", de.Key, de.Value));
                    var Photo = InputFile.FromStream(System.IO.File.OpenRead(Path));
                    botClient.SendPhotoAsync(
                      update.Message.Chat.Id,
                      Photo, null, text,
                      parseMode: ParseMode.Html,
                      replyMarkup: new InlineKeyboardMarkup(results),
                      protectContent: false,

                      replyToMessageId: update.Message.MessageId);


                    //await botClient.SendTextMessageAsync(
                    //    update.Message.Chat.Id,
                    //    text,
                    //    parseMode: ParseMode.Html,
                    //    replyMarkup: new InlineKeyboardMarkup(results),
                    //    protectContent: false,
                    //    disableWebPagePreview: true,
                    //    replyToMessageId: update.Message.MessageId);
                }
                //点了返回列表按钮时
                else
                {
                    //  botClient.send

                    string Path = "搜索横幅.gif";

                    var Photo = InputFile.FromStream(System.IO.File.OpenRead(Path));
                    //   botClient.edit

                    _ = botClient.EditMessageCaptionAsync(
                   update.CallbackQuery.Message.Chat.Id,
                 caption: text,

                   replyMarkup: new InlineKeyboardMarkup(results),
                 messageId: update.CallbackQuery.Message.MessageId,
                  parseMode: ParseMode.Html
                  ).Result;
                    //await botClient.EditMessageTextAsync(
                    //    chatId: update!.CallbackQuery!.Message!.Chat.Id,
                    //    messageId: update.CallbackQuery.Message.MessageId,
                    //    text: text,
                    //    disableWebPagePreview: true,
                    //    parseMode: ParseMode.Html,
                    //    replyMarkup: new InlineKeyboardMarkup(results));
                }

                HashSet<prjx.City> _citys = getCitysObj();
                //每个商家搜索量
                setPerMerchtSerchCnt(results, _citys);

                biz_other._SaveConfig();
            }
            catch (Exception e)
            {
                Print("返回商家联系方式列表时出错:" + e.Message);
            }


            //  print(" endfun  GetList()");
            PrintRet(__METHOD__, "");

        }

        private static void setPerMerchtSerchCnt(List<InlineKeyboardButton[]> results, HashSet<City> _citys)
        {
            foreach (var item in results)
            {
                foreach (var it in item)
                {
                    string cd = it.CallbackData!;
                    if (cd?.Contains("Merchant?id=") == true)
                    {
                        var mid = cd.Replace("Merchant?id=", "");
                        var merchant = (from c in _citys
                                        from a in c.Address
                                        from am in a.Merchant
                                        where am.Guid == mid
                                        select am).FirstOrDefault();
                        if (merchant != null)
                            merchant.Searchs++;
                    }
                }
            }
        }



        //static async Task evt_btnclick_Pt2_qryByKwd(string msgx, int pagex, int pagesizex, ITelegramBotClient botClient, Update update)
        //{
        //    var __METHOD__ = "evt_btnclick_Pt2_qryByKwd";  //bcs in task so cant get currentmethod
        //    dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), msgx));

        //    if (msgx == null || msgx.Length == 0)
        //        return;
        //   print(" fun  GetList()");


        //    //页码
        //    int page = 0;
        //    //搜索结果数
        //    int count = 0;
        //    //获取操作用户
        //    User? user;
        //    if (update.Type is UpdateType.Message)
        //    {
        //        if (_users.ContainsKey((long)update.Message.From.Id))
        //        {
        //            user = _users[(long)update?.Message?.From.Id];
        //        }
        //        else
        //        {
        //            user = new User();
        //            _users.Add((long)update?.Message?.From.Id, user);
        //        }
        //    }
        //    else
        //    {
        //        if (_users.ContainsKey((long)update?.CallbackQuery?.From?.Id))
        //        {
        //            user = _users[(long)update?.CallbackQuery?.From?.Id];
        //        }
        //        else
        //        {
        //            user = new User();
        //            _users.Add((long)update?.CallbackQuery?.From?.Id, user);
        //        }
        //    }


        //    if (update.Type is UpdateType.CallbackQuery)
        //    {
        //        var uri = new Uri("https://t.me/" + update.CallbackQuery?.Data);
        //        var parameters = QueryHelpers.ParseQuery(uri.Query);
        //        parameters.TryGetValue("page", out var pageStr);
        //        if (!string.IsNullOrEmpty(pageStr))
        //            page = Convert.ToInt32(pageStr);
        //    }
        //    const int pagesize = 5;
        //    List<InlineKeyboardButton[]> results = [];

        //    //搜索关键词  Merchant.json to citys





        //    //kwd if ret list btn cmd cmd
        //    if (update.Type == UpdateType.CallbackQuery)
        //    {
        //        if (msgx.Trim().StartsWith("@" + Program.botname))
        //            msgx = msgx.Substring(19).Trim();
        //        else
        //            msgx = msgx.Trim();
        //    }


        //   print("  msg=>" + msgx);

        //    if (!string.IsNullOrEmpty(msgx))
        //    {
        //        //    List<InlineKeyboardButton[]> results = [];  &park=世纪新城园区
        //        results = mrcht.qryByMsgKwdsV2(msgx, "city=妙瓦底", other._shangjiaFL(Program.groupId.ToString()));
        //        //  results = arrCls.rdmList<InlineKeyboardButton[]>(results);
        //        count = results.Count;
        //        results = results.Skip(page * pagesize).Take(pagesize).ToList();
        //    }

        //    //发起查询  stzrt with @bot
        //    if (update!.Type is UpdateType.Message)
        //    {
        //        // keyword = update?.Message?.Text;
        //        //   keyword = keyword.Substring(19).Trim();
        //        //if (msgx?.Length is < 2 or > 8)
        //        //{
        //        //    await bot_DeleteMessage(update.Message!.Chat.Id, update.Message.MessageId, "请输入2-8个字符的的关键词", 5);
        //        //    return;
        //        //}

        //        if (count == 0)
        //        {

        //            //await botapi.bot_DeleteMessage(update.Message!.Chat.Id, update.Message.MessageId, "未搜索到商家,您可以向我们提交商家联系方式", 5);
        //            return;
        //        }
        //        user.Searchs++;
        //    }
        //    ////返回列表
        //    //else
        //    //{
        //    //    var cq = update!.CallbackQuery!;
        //    //    if (string.IsNullOrEmpty(msgx))
        //    //    {
        //    //        try
        //    //        {
        //    //            await botClient.AnswerCallbackQueryAsync(cq.Id, "搜索关键词已经删除,需重新搜索!", true);
        //    //            await botClient.DeleteMessageAsync(cq.Message!.Chat.Id, cq.Message.MessageId);
        //    //        }
        //    //        catch (Exception e)
        //    //        {
        //    //           print("告知搜索关键词已经删除时出错:" + e.Message);
        //    //        }
        //    //        return;
        //    //    }
        //    //    user.Returns++;
        //    //}



        //    // pagebtns
        //    evt_search_pt3_btns翻页(page, count, pagesize, results);

        //    try
        //    {
        //        var text = "";// $"😙 <b>搜到{count}个商家,被搜得越多越靠前!</b>\n";// +
        //                      //$"<blockquote>您的统计:搜索{user.Searchs}  返列表{user.Returns}  查看数{user.Views}" +
        //                      //$"  看菜单{user.ViewMenus}  打分{user.Scores}  评价{user.Comments}</blockquote>";
        //        text += " \n " + timerCls.plchdTxt;
        //        //第一次搜索时返回的列表



        //        string Path = "搜索横幅.gif";
        //        //     var text = "——————————————";
        //        // print(string.Format("{0}-{1}", de.Key, de.Value));
        //        var Photo = InputFile.FromStream(System.IO.File.OpenRead(Path));
        //        await botClient.SendPhotoAsync(
        //           tglib.bot_getChatid(update),
        //            Photo, null, text,
        //            parseMode: ParseMode.Html,
        //            replyMarkup: new InlineKeyboardMarkup(results),
        //            protectContent: false);


        //        //await botClient.SendTextMessageAsync(
        //        //    update.Message.Chat.Id,
        //        //    text,
        //        //    parseMode: ParseMode.Html,
        //        //    replyMarkup: new InlineKeyboardMarkup(results),
        //        //    protectContent: false,
        //        //    disableWebPagePreview: true,
        //        //    replyToMessageId: update.Message.MessageId);
        //        //}
        //        ////点了返回列表按钮时
        //        //else
        //        //{

        //        //    string Path = "搜索横幅.gif";

        //        //    var Photo = InputFile.FromStream(System.IO.File.OpenRead(Path));
        //        //    //   botClient.edit

        //        //    await botClient.EditMessageCaptionAsync(
        //        //     update.CallbackQuery.Message.Chat.Id,
        //        //   caption: text,

        //        //     replyMarkup: new InlineKeyboardMarkup(results),
        //        //   messageId: update.CallbackQuery.Message.MessageId,
        //        //    parseMode: ParseMode.Html
        //        //    );
        //        //    //await botClient.EditMessageTextAsync(
        //        //    //    chatId: update!.CallbackQuery!.Message!.Chat.Id,
        //        //    //    messageId: update.CallbackQuery.Message.MessageId,
        //        //    //    text: text,
        //        //    //    disableWebPagePreview: true,
        //        //    //    parseMode: ParseMode.Html,
        //        //    //    replyMarkup: new InlineKeyboardMarkup(results));
        //        //}

        //        //每个商家搜索量
        //        //foreach (var item in results)
        //        //{
        //        //    foreach (var it in item)
        //        //    {
        //        //        string cd = it.CallbackData!;
        //        //        if (cd?.Contains("Merchant?id=") == true)
        //        //        {
        //        //            var mid = cd.Replace("Merchant?id=", "");
        //        //            var merchant = (from c in _citys
        //        //                            from a in c.Address
        //        //                            from am in a.Merchant
        //        //                            where am.Guid == mid
        //        //                            select am).FirstOrDefault();
        //        //            if (merchant != null)
        //        //                merchant.Searchs++;
        //        //        }
        //        //    }
        //        //}

        //        //await _SaveConfig();
        //    }
        //    catch (Exception e)
        //    {
        //       print("返回商家联系方式列表时出错:" + e.Message);
        //    }


        //  // print(" endfun  GetList()");
        //    dbgCls.setDbgValRtval(__METHOD__, "");
        //}


        private static void EvtsearchPt3btnsPagging(int page, int count, int pagesize, List<InlineKeyboardButton[]> results)
        {
            var pageBtn = new List<InlineKeyboardButton>();
            if (page > 0)
                pageBtn.Add(InlineKeyboardButton.WithCallbackData($"◀️ 上一页 ({page})", $"Merchant?page=" + (page - 1)));


            if (count > ((page + 1) * pagesize))
                pageBtn.Add(InlineKeyboardButton.WithCallbackData($"({page + 2}) 下一页 ▶️", $"Merchant?page=" + (page + 1)));


            if (pageBtn.Count != 0)
                results.Add([.. pageBtn]);
            //  InlineKeyboardButton.WithCallbackData( "➕ 添加商家",  "AddMerchant") ,
            string txt = "这个机器人简直是神了，啥都有 !";
            //给大家推荐一个什么信息资源都有的机器人!    detail click 里面also shar bot one need chg sync
            //results.Add([

            //    InlineKeyboardButton.WithUrl(text: "↖ 分享机器人", $"https://t.me/share/url?url=https://t.me/{botname}&text={txt}")
            //    ]);
        }



        ////dep
        //static async Task evt_GetList_qry(ITelegramBotClient botClient, Update update)
        //{
        //   print(" fun  GetList()");
        //    if (update.Type is UpdateType.Message && string.IsNullOrEmpty(update.Message?.Text)
        //        || update.Type is UpdateType.CallbackQuery && string.IsNullOrEmpty(update?.CallbackQuery?.Message?.ReplyToMessage?.Text))
        //        return;

        //    //页码
        //    int page = 0;
        //    //搜索结果数
        //    int count = 0;
        //    //获取操作用户
        //    User? user;
        //    if (update.Type is UpdateType.Message)
        //    {
        //        if (_users.ContainsKey((long)update.Message.From.Id))
        //        {
        //            user = _users[(long)update?.Message?.From.Id];
        //        }
        //        else
        //        {
        //            user = new User();
        //            _users.Add((long)update?.Message?.From.Id, user);
        //        }
        //    }
        //    else
        //    {
        //        if (_users.ContainsKey((long)update?.CallbackQuery?.From?.Id))
        //        {
        //            user = _users[(long)update?.CallbackQuery?.From?.Id];
        //        }
        //        else
        //        {
        //            user = new User();
        //            _users.Add((long)update?.CallbackQuery?.From?.Id, user);
        //        }
        //    }


        //    if (update.Type is UpdateType.CallbackQuery)
        //    {
        //        var uri = new Uri("https://t.me/" + update.CallbackQuery?.Data);
        //        var parameters = QueryHelpers.ParseQuery(uri.Query);
        //        parameters.TryGetValue("page", out var pageStr);
        //        if (!string.IsNullOrEmpty(pageStr))
        //            page = Convert.ToInt32(pageStr);
        //    }
        //    const int pagesize = 5;
        //    List<InlineKeyboardButton[]> results = [];

        //    //搜索关键词  Merchant.json to citys
        //    string? keyword = update.Type == UpdateType.Message ? update?.Message?.Text : update?.CallbackQuery?.Message?.ReplyToMessage?.Text;
        //    keyword = update?.Message?.Text;

        //    if (update.Type == UpdateType.CallbackQuery)  //for ret to list commd
        //        keyword = update?.CallbackQuery?.Message?.ReplyToMessage?.Text;

        //    if (update?.Message?.Chat?.Type == ChatType.Private)
        //        keyword = keyword.Trim();
        //    else  //grp msg
        //    {
        //        if (keyword.Trim().StartsWith("@LianXin_BianMinBot"))
        //            keyword = keyword.Substring(19).Trim();
        //        else
        //            keyword = keyword.Trim();
        //    }

        //    //kwd if ret list btn cmd cmd
        //    if (update.Type == UpdateType.CallbackQuery)
        //    {
        //        if (keyword.Trim().StartsWith("@LianXin_BianMinBot"))
        //            keyword = keyword.Substring(19).Trim();
        //        else
        //            keyword = keyword.Trim();
        //    }


        //   print("  kwd=>" + keyword);

        //    if (!string.IsNullOrEmpty(keyword))
        //    {
        //        keyword = keyword.ToLower().Replace(" ", "").Trim();
        //        var searchChars = keyword!.ToCharArray();

        //        results = (from c in _citys
        //                   from ca in c.Address
        //                   from am in ca.Merchant
        //                   where searchChars.All(s => (c.CityKeywords + ca.CityKeywords + am.KeywordString + am.KeywordString + _categoryKeyValue[(int)am.Category]).Contains(s))
        //                   orderby am.Views descending
        //                   select new[] { new InlineKeyboardButton(c.Name + " • " + ca.Name + " • " + am.Name) { CallbackData = $"Merchant?id={am.Guid}" } }).ToList();
        //        count = results.Count;
        //        results = results.Skip(page * pagesize).Take(pagesize).ToList();
        //    }

        //    //发起查询  stzrt with @bot
        //    if (update!.Type is UpdateType.Message)
        //    {
        //        // keyword = update?.Message?.Text;
        //        //   keyword = keyword.Substring(19).Trim();
        //        if (keyword?.Length is < 2 or > 8)
        //        {
        //            await botapi.bot_DeleteMessage(update.Message!.Chat.Id, update.Message.MessageId, "请输入2-8个字符的的关键词", 5);
        //            return;
        //        }

        //        if (count == 0)
        //        {
        //           // await botapi.bot_DeleteMessage(update.Message!.Chat.Id, update.Message.MessageId, "未搜索到商家,您可以向我们提交商家联系方式", 5);
        //            return;
        //        }
        //        user.Searchs++;
        //    }
        //    //返回列表
        //    else
        //    {
        //        var cq = update!.CallbackQuery!;
        //        if (string.IsNullOrEmpty(keyword))
        //        {
        //            try
        //            {
        //                await botClient.AnswerCallbackQueryAsync(cq.Id, "搜索关键词已经删除,需重新搜索!", true);
        //                await botClient.DeleteMessageAsync(cq.Message!.Chat.Id, cq.Message.MessageId);
        //            }
        //            catch (Exception e)
        //            {
        //               print("告知搜索关键词已经删除时出错:" + e.Message);
        //            }
        //            return;
        //        }
        //        user.Returns++;
        //    }


        //    // pagebtns
        //    var pageBtn = new List<InlineKeyboardButton>();
        //    if (page > 0)
        //        pageBtn.Add(InlineKeyboardButton.WithCallbackData($"◀️ 上一页 ({page})", $"Merchant?page=" + (page - 1)));


        //    if (count > ((page + 1) * pagesize))
        //        pageBtn.Add(InlineKeyboardButton.WithCallbackData($"({page + 2}) 下一页 ▶️", $"Merchant?page=" + (page + 1)));


        //    if (pageBtn.Count != 0)
        //        results.Add([.. pageBtn]);
        //    //  InlineKeyboardButton.WithCallbackData( "➕ 添加商家",  "AddMerchant") ,
        //    results.Add([

        //        InlineKeyboardButton.WithUrl(text: "↖ 分享机器人", "https://t.me/share/url?url=https://t.me/ZuoDaoMianDian&text=给大家推荐一个可以搜索商家联系方式的群!")
        //        ]);

        //    try
        //    {
        //        var text = "";// $"😙 <b>搜到{count}个商家,被搜得越多越靠前!</b>\n";
        //        //+
        //        //  嫖娼还是谈恋爱、 $"<blockquote>您的统计:搜索{user.Searchs}  返列表{user.Returns}  查看数{user.Views}" +
        //        //    $"  看菜单{user.ViewMenus}  打分{user.Scores}  评价{user.Comments}</blockquote>";
        //        text += " \n " + timerCls.plchdTxt;
        //        //第一次搜索时返回的列表
        //        if (update?.Message != null)
        //        {


        //            string Path = "搜索横幅.gif";
        //            //     var text = "——————————————";
        //            // print(string.Format("{0}-{1}", de.Key, de.Value));
        //            var Photo = InputFile.FromStream(System.IO.File.OpenRead(Path));
        //            await botClient.SendPhotoAsync(
        //                update.Message.Chat.Id,
        //                Photo, null, text,
        //                parseMode: ParseMode.Html,
        //                replyMarkup: new InlineKeyboardMarkup(results),
        //                protectContent: false,

        //                replyToMessageId: update.Message.MessageId);


        //            //await botClient.SendTextMessageAsync(
        //            //    update.Message.Chat.Id,
        //            //    text,
        //            //    parseMode: ParseMode.Html,
        //            //    replyMarkup: new InlineKeyboardMarkup(results),
        //            //    protectContent: false,
        //            //    disableWebPagePreview: true,
        //            //    replyToMessageId: update.Message.MessageId);
        //        }
        //        //点了返回列表按钮时
        //        else
        //        {

        //            string Path = "搜索横幅.gif";

        //            var Photo = InputFile.FromStream(System.IO.File.OpenRead(Path));
        //            //   botClient.edit

        //            await botClient.EditMessageCaptionAsync(
        //             update.CallbackQuery.Message.Chat.Id,
        //           caption: text,

        //             replyMarkup: new InlineKeyboardMarkup(results),
        //           messageId: update.CallbackQuery.Message.MessageId,
        //            parseMode: ParseMode.Html
        //            );
        //            //await botClient.EditMessageTextAsync(
        //            //    chatId: update!.CallbackQuery!.Message!.Chat.Id,
        //            //    messageId: update.CallbackQuery.Message.MessageId,
        //            //    text: text,
        //            //    disableWebPagePreview: true,
        //            //    parseMode: ParseMode.Html,
        //            //    replyMarkup: new InlineKeyboardMarkup(results));
        //        }

        //        //每个商家搜索量
        //        foreach (var item in results)
        //        {
        //            foreach (var it in item)
        //            {
        //                string cd = it.CallbackData!;
        //                if (cd?.Contains("Merchant?id=") == true)
        //                {
        //                    var mid = cd.Replace("Merchant?id=", "");
        //                    var merchant = (from c in _citys
        //                                    from a in c.Address
        //                                    from am in a.Merchant
        //                                    where am.Guid == mid
        //                                    select am).FirstOrDefault();
        //                    merchant.Searchs++;
        //                }
        //            }
        //        }

        //        await _SaveConfig();
        //    }
        //    catch (Exception e)
        //    {
        //       print("返回商家联系方式列表时出错:" + e.Message);
        //    }


        //   print(" endfun  GetList()");

        //}

        //获取商家结果 detail click
        static void BtnHdlEvtView(ITelegramBotClient botClient, Update update, string reqThreadId)
        {
            var __METHOD__ = "evt_View listitem_click()";
            dbgCls.PrintCallFunArgs(__METHOD__, func_get_args(update, reqThreadId));
            logCls.log("FUN " + __METHOD__, func_get_args(reqThreadId, update), null, "logDir", reqThreadId);

            Dictionary<string, string> parse_str1 = parse_str(update.CallbackQuery.Data);
            if (LoadFieldAsStr(parse_str1, "btn") == "dtl") //def is not  
                if (LoadFieldAsStr(parse_str1, "ckuid") == "y")
                    if (!StrEq(update.CallbackQuery?.From?.Username, update.CallbackQuery?.Message?.ReplyToMessage?.From?.Username))
                    {


                        Print("not same user...ret");
                        botClient.AnswerCallbackQueryAsync(
                                callbackQueryId: update.CallbackQuery.Id,
                                text: "这是别人搜索的联系方式,如果你要查看联系方式请自行搜索",
                                showAlert: true); // 这是显示对话框的关键);
                        return;

                    }




            ////if dafen lookmenu ing{
            //Dictionary<string, string> parse_str1 = parse_str(update.CallbackQuery.Data);
            //if (ldfld2str(parse_str1, "score") != "") //def is not                        
            //{

            //}
            //else if (ldfld2str(parse_str1, "showMenu") != "") //def is not                        
            //{

            //}
            //else
            //{
            //    if (ldfld2str(parse_str1, "chkUidEq") == "y")


            Dictionary<string, string> parse_str2 = parse_str(update.CallbackQuery.Data);
            if (LoadFieldAsStr(parse_str2, "btn") == "detail") //def is not   
            {
                //need chk
            }


            var cq = update.CallbackQuery!;


            Dictionary<string, StringValues> whereExprsObj = ToDic941(update.CallbackQuery.Data);
            SortedList Merchant1 = Qe_find(whereExprsObj["id"], "mercht商家数据", null, (dbf) =>
            {
                return rnd_next4Sqlt(dbf);
            });

            //联系商家
            Merchant? contact_Merchant = new Merchant();
            //商家路径
            string mrchtpath = string.Empty;
            mrchtpath = Merchant1["城市"] + "•" + Merchant1["园区"] + "•" + Merchant1["商家"];
            //商家所属园区物业联系(纸飞机号)
            string propertyTelegram = string.Empty;
            //是否显示商家菜单
            var isShowMenu = false;
            //评分
            int? score = null;
            //获取操作用户
            User? user = null;
            //获取所在园区的商家集合(用户获取排名)
            HashSet<Merchant> merchants = [];

            if (_users.ContainsKey((long)cq.From?.Id))
            {
                user = _users[(long)cq.From?.Id];
            }
            else
            {
                user = new User();
                _users.Add((long)cq.From?.Id, user);
            }
            var uri = new Uri("https://t.me/" + cq.Data);

            var parameters = QueryHelpers.ParseQuery(uri.Query);

            parameters.TryGetValue("id", out var id);
            string guid = id.ToString(); var _citys = getCitysObj();
            foreach (var city in _citys)
            {
                foreach (var area in city.Address)
                {
                getProperty:
                    foreach (var merchant in area.Merchant)
                    {
                        if (merchant.Guid.Contains(guid) && contact_Merchant == null)
                        {
                            contact_Merchant = merchant;
                            mrchtpath = city.Name + "•" + area.Name + "•" + merchant.Name;
                            merchants = area.Merchant;
                            goto getProperty;
                        }

                        if (contact_Merchant != null && merchant.Name.Contains("物业") && string.IsNullOrEmpty(propertyTelegram) && merchant.Telegram.Any())
                        {
                            propertyTelegram = merchant.Telegram.First();
                            break;
                        }
                    }

                    if (contact_Merchant != null)
                        break;
                }
                if (contact_Merchant != null)
                    break;
            }

            //if ((string.IsNullOrEmpty(cq.Message?.Caption) && string.IsNullOrEmpty(cq.Message?.Text)) || contact_Merchant == null)
            //{
            //   print("查看结果时显示未找到此商家,此处有错误");
            //    return;
            //}

            //是否需要显示查看菜单
            isShowMenu = parameters.ContainsKey("showMenu");
            //打分
            if (parameters.ContainsKey("score"))
            {
                parameters.TryGetValue("score", out var sc);
                score = Convert.ToInt32(sc);
            }
            #region 受限了
            var operaCount = biz_other._SetUserOperas(cq.From.Id).Result;
            var answer = string.Empty;
            //24小时10个   一周30个    一个月50个   一年150个  
            if (operaCount.Years > 150)
                answer = "您已被受限，1年内内查询次数太多，请过一段时间再查询";

            if (!answer.Contains("受限") && operaCount.Months > 50)
                answer = "您已被受限，30天内内查询次数太多，请过一段时间再查询";

            if (!answer.Contains("受限") && operaCount.Weeks > 30)
                answer = "您已被受限，7天内内查询次数太多，请过一段时间再查询";

            if (!answer.Contains("受限") && operaCount.Todays > 10)
                answer = "您已被受限，24小时内内查询次数太多，请过一段时间再查询";

            //if (answer.Contains("受限"))
            //{
            //    try
            //    {
            //        await botClient.AnswerCallbackQueryAsync(cq.Id, answer, true);
            //    }
            //    catch (Exception e)
            //    {
            //       print("告知查询次数太多时出错:" + e.Message);
            //    }
            //    return;
            //}
            #endregion



            //如果是评分
            if (score != null)
            {
                if (contact_Merchant.Scores.ContainsKey(cq.From.Id))
                {
                    try
                    {
                        botClient.AnswerCallbackQueryAsync(cq.Id, "一个账号只能打分一次,请勿重复打分!", true);
                    }
                    catch (Exception e)
                    {
                        Print("告知已评过分时出错:" + e.Message);
                    }
                    return;
                }

                contact_Merchant.Scores.Add(cq.From.Id, (int)score);
                user.Scores++;
                try
                {
                    botClient.AnswerCallbackQueryAsync(cq.Id, "评分成功", true);
                }
                catch (Exception e)
                {
                    Print("告知评分成功时出错:" + e.Message);
                }

                Telegram.Bot.Types.Message scoreTipMsg = null;
                try
                {
                    //感谢打分
                    scoreTipMsg = botClient.SendTextMessageAsync(
                        chatId: cq.Message.Chat.Id,
                        text: $"😙 <b>匿名用户对商家进行了打分</b>",
                        parseMode: ParseMode.Html,
                        replyToMessageId: cq.Message.MessageId,
                        disableNotification: false).Result;
                }
                catch (Exception e)
                {
                    Print("感谢打分时出错:" + e.Message);
                }

                TaskRun(async () =>
               {
                   await System.Threading.Tasks.Task.Delay(10000);
                   if (scoreTipMsg == null)
                   {
                       try
                       {
                           await botClient.DeleteMessageAsync(scoreTipMsg.Chat.Id, scoreTipMsg.MessageId);
                       }
                       catch (Exception e)
                       {
                           Print("删除评分提示时出错:" + e.Message);
                       }
                   }
                   //   return 0;
               });
            }

            //查看联系方式
            if (!cq.Data.Contains('&'))
            {
                user.Views++;
                contact_Merchant.Views++;
                user.ViewTimes.Add(DateTime.Now);
                Telegram.Bot.Types.Message? msg = null;
            }

            var result = string.Empty;
            result += "\n\n" + timerCls.plchdTxt;
            // result += $"<blockquote>您搜索统计:搜索{user.Searchs}  返列表{user.Returns}  查看数{user.Views}  看菜单{user.ViewMenus}  打分{user.Scores}  评价{user.Comments}</blockquote>";
            //展现量 浏览量 评论数
            // result += $"\n🔎{contact_Merchant.Searchs}    👁{contact_Merchant.Views}    💬{contact_Merchant.Comments.Count()}";
            //名称路径
            result += "\n\n🏠<b>" + mrchtpath + "</b>";

            Print(result);
            //人气排名   
            //int rank = merchants.OrderByDescending(e => e.Views).ToList().FindIndex(e => e.Guid == guid) + 1;
            //result += rank switch
            //{
            //    1 => $"\n\n🏆<b>商家排名</b> 第<b>🥇</b>名 (受欢迎程度)",
            //    2 => $"\n\n🏆<b>商家排名</b> 第<b>🥈</b>名 (受欢迎程度)",
            //    3 => $"\n\n🏆<b>商家排名</b> 第<b>🥉</b>名 (受欢迎程度)",
            //    _ => $"\n\n🏆<b>商家排名</b> 第<b> {rank} </b>名 (受欢迎程度)",
            //};

            CopyPropSortedListToMerchant(Merchant1, contact_Merchant);
            //营业时间
            try
            {
                TimeSpan StartTime = TimeSpan.Parse(Merchant1["开始时间"].ToString());
                TimeSpan EndTime = TimeSpan.Parse(Merchant1["结束时间"].ToString());
                result += "\n\n⏱<b>营业时间</b> " + timeCls.FormatTimeSpan(TimeSpan.Parse(Merchant1["开始时间"].ToString())) + "-" + timeCls.FormatTimeSpan(TimeSpan.Parse(Merchant1["结束时间"].ToString())) + " " + biz_other._IsBusinessHours(StartTime, EndTime);

            }
            catch (Exception e)
            {
                Print(e);
            }


            var contactScore = contact_Merchant.Scores.Count == 0 ? 5 : contact_Merchant.Scores.Select(u => u.Value).Average();
            //打分
            #region  

            if (contactScore == 5)
            {
                result += $"\n\n⭐️<b>综合评分</b> <b>{contactScore:F1}</b>❤️❤️❤️❤️❤️ ({contact_Merchant.Scores.Count})";
            }
            else if (contactScore >= 4)
            {
                result += $"\n\n⭐️<b>综合评分</b> <b>{contactScore:F1}</b>❤️❤️❤️❤️🤍 ({contact_Merchant.Scores.Count})";
            }
            else if (contactScore >= 3)
            {
                result += $"\n\n⭐️<b>综合评分</b> <b>{contactScore:F1}</b>❤️❤️❤️🤍🤍 ({contact_Merchant.Scores.Count})";
            }
            else if (contactScore >= 2)
            {
                result += $"\n\n⭐️<b>综合评分</b> <b>{contactScore:F1}</b>❤️❤️🤍🤍🤍 ({contact_Merchant.Scores.Count})";
            }
            else if (contactScore >= 1)
            {
                result += $"\n\n⭐️<b>综合评分</b> <b>{contactScore:F1}</b>❤️🤍🤍🤍🤍 ({contact_Merchant.Scores.Count})";
            }
            else
            {
                result += $"\n\n⭐️<b>综合评分</b>   <b>{contactScore:F1}</b> 🤍 🤍 🤍 🤍 🤍 ({contact_Merchant.Scores.Count})";
            }
            #endregion

            //谷歌地图 (如果已经显示了,就不再显示)
            #region
            if (!string.IsNullOrEmpty(contact_Merchant.GoogleMapLocator))
                result += "\n\n📍<b>地理位置</b>   <a href='" + contact_Merchant.GoogleMapLocator + "'>谷歌地图位置</a>";

            var cqText = cq.Message.Text;

            if (cqText == null)
                cqText = cq.Message.Caption;

            isShowMenu = isShowMenu || cqText.Contains("-商家菜单-");
            #endregion

            contact_Merchant.Telegram = cvt2list(Merchant1, "Telegram");
            contact_Merchant.WhatsApp = cvt2list(Merchant1, "WhatsApp");
            contact_Merchant.WeiXin = cvt2list(Merchant1, "微信");
            contact_Merchant.Tel = cvt2list(Merchant1, "电话");
            result = btnHdl_detail_rendLianxiFosh(contact_Merchant, result);
            //查看菜单 (如果已经显示了,就不再显示)
            if (isShowMenu)
            {
                result += "\n\n<b>------------商家菜单------------</b>";
                result += "\n\n" + contact_Merchant.Menu;
                user.ViewMenus++;
            }
            //显示评价
            else
            {


            }
            #region 显示评价
            string pinlunRzt = pinlun.pinlun_getpinlun(contact_Merchant);
            result = result + pinlunRzt;
            Print(result);
            #endregion

            //[
            //   InlineKeyboardButton.WithCallbackData("➕ 添加商家", "AddMerchant"),
            //   InlineKeyboardButton.WithCallbackData("⚙ 修改信息", "Update"),
            //   ],
            var chkUidEq = "y";
            //if (update.CallbackQuery.Data.Contains("timerMsgMode2025"))
            chkUidEq = "n";
            // 发送带有按钮的消息

            parse_str1 = parse_str(update.CallbackQuery.Data);
            //  if (ldfld2str(parse_str1, "sdr") == "tmr") //def is not
            List<List<InlineKeyboardButton>> menu = GetMenuDafen(guid, chkUidEq, LoadFieldAsStr(parse_str1, "sdr"));

            contact_Merchant.Name = Merchant1["商家"].ToString();
            //如果不是物业
            if (!contact_Merchant.Name.Contains("物业"))
            {
                var firstBtns = new List<InlineKeyboardButton>();
                if (!isShowMenu)
                {
                    firstBtns.Add(InlineKeyboardButton.WithCallbackData("📋 查看菜单", $"id={guid}&btn=lkmenu&ckuid={chkUidEq}"));
                }
                else
                {
                    firstBtns.Add(InlineKeyboardButton.WithCallbackData("💬 查看评价", $"Merchant?id={guid}&showMenu=false"));
                }

                firstBtns.Add(InlineKeyboardButton.WithCallbackData("💬 评价商家", $"Merchant?id={guid}&Comment=true"));
                menu.Insert(0, firstBtns);

                if (!string.IsNullOrEmpty(propertyTelegram))
                    result += $"\n\n⚠️商家有卫生、乱收费问题<a href='https://t.me/{propertyTelegram}'>物业投诉</a>";
            }



            //detail show
            //  if timer img mode click,,new send msg..def is edit msg
            parse_str1 = parse_str(update.CallbackQuery.Data);
            if (LoadFieldAsStr(parse_str1, "sdr") == "tmr") //def is not

            {
                string tailmsg = "\n提示:本消息将在20秒后销毁";
                // await botClient.SendTextMessageAsync(chatId: cq.Message.Chat.Id, text: result, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(menu), disableWebPagePreview: true);
                string imgPath = "搜索横幅.gif";
                var Photo2 = InputFile.FromStream(System.IO.File.OpenRead(imgPath));
                Telegram.Bot.Types.Message message2 = Program.botClient.SendPhotoAsync(
              chatId: cq.Message.Chat.Id
                  , Photo2, null,
                 caption: result + tailmsg,
                    parseMode: ParseMode.Html,
                   replyMarkup: new InlineKeyboardMarkup(menu),
                   protectContent: false).Result;
                bot_DeleteMessageV2(cq.Message.Chat.Id, message2.MessageId, 30);
                dbgCls.PrintRet(__METHOD__, 0);
                return;
            }
            else//   (update.CallbackQuery.Data.StartsWith("Merchant?id="))
            {
                try
                {
                    logCls.log(result, "detailClickLogDir");
                    //  result = "ttt";
                    SortedList obj = new SortedList();
                    obj.Add("txt", result);
                    obj.Add("menu", menu);
                    logCls.log(obj, "detailClickDir");
                    Telegram.Bot.Types.Message m = botClient.EditMessageCaptionAsync(chatId: cq.Message.Chat.Id, messageId: cq.Message.MessageId, caption: result, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(menu)).Result;

                    logCls.log(m, "detailClickLogDir");
                }
                catch (Exception e)
                {
                    logCls.logErr2025(e, "detal click()", "errlog");
                }


                dbgCls.PrintRet(__METHOD__, 0);

                return;
            }
            //end detail


            //---------fowlow maybe dep...
            // ..........send txt 
            try
            {
                //  botClient.SendTextMessageAsync()
                //  botClient.EditMessageCaptionAsync
                //  botClient.EditMessageTextAsync
                botClient.EditMessageTextAsync(chatId: cq.Message.Chat.Id, messageId: cq.Message.MessageId, text: result, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(menu));
            }
            catch (Exception e)
            {
                //try
                //{
                //    await botClient.EditMessageCaptionAsync(chatId: cq.Message.Chat.Id, messageId: cq.Message.MessageId, caption: result, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(menu));

                //}
                //catch (Exception e)
                //{

                if (e.Message.Contains("current content"))
                {
                    try
                    {
                        botClient.AnswerCallbackQueryAsync(cq.Id, "已经显示了", true);
                    }
                    catch (Exception ex)
                    {
                        Print("已经显示了,请勿重复点击时候出错:" + ex.Message);
                    }
                }
                else
                {
                    Print("编辑联系方式时出错:" + e.Message);
                }
                biz_other._SaveConfig();
                // }


            }  //end ctch

            static List<List<InlineKeyboardButton>> GetMenuDafen(string guid, string chkUidEq, string sndr)
            {

                List<InlineKeyboardButton> dafenmenu = [
                     InlineKeyboardButton.WithCallbackData( "打分",  $"btn=dafenTips"),
                     InlineKeyboardButton.WithCallbackData( "1",  $"id={guid}&ckuid={chkUidEq}&btn=df1"),
                     InlineKeyboardButton.WithCallbackData( "2",  $"id={guid}&ckuid={chkUidEq}&btn=df2"),
                     InlineKeyboardButton.WithCallbackData( "3",  $"id={guid}&ckuid={chkUidEq}&btn=df3"),
                     InlineKeyboardButton.WithCallbackData( "4",  $"id={guid}&ckuid={chkUidEq}&btn=df4"),
                     InlineKeyboardButton.WithCallbackData( "5",  $"id={guid}&ckuid={chkUidEq}&btn=df5"),
                 ];
                if (sndr == "tmr")
                    return [
                        dafenmenu
                         ,
               //  [ InlineKeyboardButton.WithUrl(text: "↖ 分享机器人", $"https://t.me/share/url?url=https://t.me/{botname}&text=这个机器人简直是神了，啥都有 !") ],
             //    [ InlineKeyboardButton.WithCallbackData(text: "↪️ 返回商家列表", $"Merchant?return")]
                           ];
                else
                    return [
                        dafenmenu
                                 ,
               //  [ InlineKeyboardButton.WithUrl(text: "↖ 分享机器人", $"https://t.me/share/url?url=https://t.me/{botname}&text=这个机器人简直是神了，啥都有 !") ],
                 [ InlineKeyboardButton.WithCallbackData(text: "↪️ 返回商家列表", $"Merchant?return")]
                                ];
            }

            //end eve detal click()
        }



        //----------------------

        private static string btnHdl_detail_rendLianxiFosh(Merchant? contact_Merchant, string result)
        {

            #region 联系方式
            result += "\n\n<b>-------------联系方式-------------</b>";
            Print(result);
            if (contact_Merchant.Telegram.Any())
            {
                if (contact_Merchant.Telegram.Count == 1)
                {
                    string tlgrm = contact_Merchant.Telegram[0];
                    if (tlgrm.Trim().Length > 3)
                        result += $"\n\nTelegram  :  <a href='https://t.me/{tlgrm}'>点击聊天</a>";
                }
                else
                {
                    for (int i = 0; i < contact_Merchant.Telegram.Count; i++)
                        result += $"\n\nTelegram {i + 1}  :  <a href='https://t.me/{contact_Merchant.Telegram[i]}'>点击聊天</a>";
                }
            }

            if (contact_Merchant.WhatsApp.Any())
            {
                string tmpleTxt = $"你好，从telegrame 的 https://t.me/{botname} 联信便民助手找到你的。麻烦发下菜单，谢谢";
                if (contact_Merchant.WhatsApp.Count == 1)
                {
                    string wtap = contact_Merchant.WhatsApp[0];
                    if (wtap.Trim().Length > 3)
                        result += $"\n\nWhatsApp  :  <a href='https://api.whatsapp.com/send/?phone={wtap}&text={tmpleTxt}'>点击聊天</a>";
                }
                else
                {
                    for (int i = 0; i < contact_Merchant.WhatsApp.Count; i++)
                        result += $"\n\nWhatsApp {i + 1}  :  <a href='https://api.whatsapp.com/send/?phone={contact_Merchant.WhatsApp[0]}&text={tmpleTxt}'>点击聊天</a>";
                }
            }

            if (contact_Merchant.Line.Any())
            {
                if (contact_Merchant.Line.Count == 1)
                {
                    string ctct = contact_Merchant.Line[0];
                    if (ctct.Trim().Length > 3)
                        result += $"\n\nLine  :  <a href='https://line.me/R/ti/p/~联信提示:切换为电话号码搜{ctct}'>点击聊天</a>";
                }
                else
                {
                    for (int i = 0; i < contact_Merchant.Line.Count; i++)
                        result += $"\n\nLine {i + 1}  :  <a href='https://line.me/R/ti/p/~联信提示:切换为电话号码搜{contact_Merchant.Line[i]}'>点击聊天</a>";
                }
            }

            if (contact_Merchant.WeiXin.Any())
            {
                if (contact_Merchant.WeiXin.Count == 1)
                {
                    string ctct = contact_Merchant.WeiXin[0];
                    if (ctct.Trim().Length > 3)
                        result += $"\n\n微信  :  " + contact_Merchant.WeiXin[0];
                }
                else
                {
                    for (int i = 0; i < contact_Merchant.WeiXin.Count; i++)
                        result += $"\n\n微信 {i + 1}  :  " + contact_Merchant.WeiXin[i];
                }
            }

            if (contact_Merchant.Tel.Any())
            {
                if (contact_Merchant.Tel.Count == 1)
                {

                    string ctct = contact_Merchant.Tel[0];
                    if (ctct.Trim().Length > 3)
                        result += $"\n\n电话  :  " + contact_Merchant.Tel[0];
                }
                else
                {
                    for (int i = 0; i < contact_Merchant.Tel.Count; i++)
                        result += $"\n\n电话 {i + 1}  :  " + contact_Merchant.Tel[i];
                }
            }
            #endregion
            return result;
        }













        //获取上级目录名称 dep

    }


}
