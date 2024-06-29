
global using static prj202405.lib.tglib;
global using static prj202405.Program;
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
using prj202405.lib;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using prj202405.lib;
using prj202405.lib;
using prj202405.lib;
using JiebaNet.Segmenter;
using System.Xml;
using HtmlAgilityPack;
using Formatting = Newtonsoft.Json.Formatting;
using DocumentFormat.OpenXml;
using mdsj;
using System.Runtime.Intrinsics.Arm;
using Microsoft.Extensions.Primitives;
using System.Runtime.CompilerServices;
using mdsj;
using mdsj.libBiz;

using DocumentFormat.OpenXml.Bibliography;
using mdsj.lib;

using static mdsj.lib.afrmwk;
using static mdsj.lib.util;
using static libx.storeEngr4Nodesqlt;
using static prj202405.timerCls;
using static mdsj.biz_other;
using static mdsj.clrCls;
using static libx.qryEngrParser;
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
using static mdsj.lib.util;
using static mdsj.libBiz.tgBiz;
using static mdsj.lib.afrmwk;

using static SqlParser.Ast.DataType;

using static SqlParser.Ast.CharacterLength;
using static mdsj.lib.music;
using static mdsj.lib.dtime;
using static mdsj.lib.fulltxtSrch;

using System.Net.Http.Json;
using DocumentFormat.OpenXml.Spreadsheet;

using System.Security.Policy;
using RG3.PF.Abstractions.Entity;
using System.Security.Cryptography;


namespace prj202405
{
    internal class Program
    {
        //  https://api.telegram.org/bot6999501721:AAFNqa2YZ-lLZMfN8T2tYscKBi33noXhdJA/getMe
        public const string botname = "LianXin_BianMinBot";

        public static TelegramBotClient botClient = new("6999501721:AAFNqa2YZ-lLZMfN8T2tYscKBi33noXhdJA");
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



        public static async Task Main(string[] args)
        {

           prjdir = filex.GetAbsolutePath(prjdir);



            evt_boot(() =>
            {
             //   botClient = botClient;
                获取机器人的信息();
            });

            //aop_lgtry(() =>
            //{
            //    tgBiz.botClient = botClient;
            //    获取机器人的信息();
            //});

            // throw new Exception("000");
            //boot evt

            //    Console.WriteLine("botClient uname=>"+ botClient.)


            //            C# 中捕获全局异常和全局异步异常，可以通过以下方式实现：



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
            await biz_other._readMerInfo();
            #endregion
            #endregion



            tglib.bot_iniChtStrfile();

            testCls.testAsync();

            //   botClient.OnApiResponseReceived
            //botClient.OnMessage += Bot_OnMessage;
            //   botClient. += Bot_OnCallbackQuery;  jeig api outtime
            //分类枚举
            botClient.StartReceiving(updateHandler: evt_aHandleUpdateAsyncSafe,
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
#warning 循环账号是否过期了

            Qunzhushou.main1();

            //  Console.ReadKey();
            //loopForever();
            while (true)
            {
                Thread.Sleep(100);
            }
        }



        static async Task evt_aHandleUpdateAsyncSafe(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            string reqThreadId = geneReqid();
            //  throw new Exception("myex");
            try
            {
                //  int reqThreadId = Thread.CurrentThread.ManagedThreadId;
                evt_aHandleUpdateAsync(botClient, update, cancellationToken, reqThreadId);
            }
            catch (Exception e)
            {
                logErr2024(e, "evt_aHandleUpdateAsyncSafe", "errlogDir", null);
            }

        }

        private static async void Bot_OnUpdate(object sender, UpdateEventArgs e)
        {
            var __METHOD__ = "Bot_OnUpdate";
            dbgCls.print_call(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), e));


            dbgCls.print_ret(__METHOD__, 0);
        }


        //收到消息时执行的方法
        static async Task evt_aHandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string reqThreadId)
        {
            //  throw new Exception("myex");

            var __METHOD__ = "evt_aHandleUpdateAsync";
            dbgCls.print_call(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod()));
            logCls.log("fun " + __METHOD__, func_get_args(update), null, "logDir", reqThreadId);
            Console.WriteLine(update?.Message?.Text);
            Console.WriteLine(json_encode(update));
            bot_logRcvMsg(update);


            //----------if new user join
            if (update?.Message?.NewChatMembers != null)
            {

                evt_newUserJoin2024(update.Message.Chat.Id, update?.Message?.NewChatMembers);

                return;
            }

            if (update.Type == UpdateType.Message)
            {
                if (update.Message.Type == MessageType.Text)
                {
                    if (update.Message.Text.Trim().StartsWith("/"))
                        if (update.Message.Chat.Type == ChatType.Private)
                        {
                            OnCmdPrvt(update.Message.Text.Trim(), update, reqThreadId);
                            return;
                        }
                }
            }


            if (update.Type == UpdateType.Message)
            {
                OnMsg(update, reqThreadId);
            }

            if (update.Type == UpdateType.CallbackQuery)
            {
                OnCallbk(update, reqThreadId);
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
                if (ldfld2str(parse_str1, "ckuid") == "y") //def is not
                {
                    if (!str_eq(update.CallbackQuery?.From?.Username, update.CallbackQuery?.Message?.ReplyToMessage?.From?.Username))
                    {
                        await botClient.AnswerCallbackQueryAsync(
                           callbackQueryId: update.CallbackQuery.Id,
                           text: "这是别人搜索的联系方式,如果你要查看联系方式请自行搜索",
                           showAlert: true); // 这是显示对话框的关键);
                        return;
                    }
                }

                if (ldfld2str(parse_str1, "btn") == "lkmenu") //def is not
                {
                    evt_lookmenu(update.CallbackQuery);
                    return;
                }






            }

            if (update.Type == UpdateType.CallbackQuery)
            {
                Dictionary<string, string> parse_str1 = parse_str(update.CallbackQuery.Data);
                if (ldfld2str(parse_str1, "btn") == "解除禁言")
                    canSendBtn_click(update);

            }

            if (update.Type == UpdateType.CallbackQuery)
            {
                Dictionary<string, string> parse_str1 = parse_str(update.CallbackQuery.Data);
                if (ldfld2str(parse_str1, "btn") == "dafenTips")
                    return;

            }


            if (update.Type == UpdateType.CallbackQuery)
            {
                Dictionary<string, string> parse_str1 = parse_str(update.CallbackQuery.Data);
                string btnname = ldfld2str(parse_str1, "btn");
                if (btnname.StartsWith("df") && btnname != "dafenTips")
                {
                    await evtDafen(botClient, update, parse_str1);
                    return;
                }


            }


            await biz_other._readMerInfo();

            // if (update.Message != null)
            if (update?.Type is UpdateType.Message)
            {
                Console.WriteLine(update.Message?.Type);
                if (update.Message?.Type == MessageType.Text)
                {
                    Console.WriteLine(update.Message?.Type);
                    bot_adChk(update);
                }

            }

            //auto add cht sess
            if (update?.Message != null)
            {
                tglib.bot_saveChtSesion(update?.Message?.Chat?.Id, update?.Message?.From);
            }


            //menu proces   evt_btmBtnclick
            if (tgBiz.tg_isBtm_btnClink_in_pubGrp(update))
            {
                evt_btm_btn_click_inPubgrp(update);
                return;
            }
            string msgx2024 = tglib.bot_getTxtMsgDep(update);
            string msg2056 = str_trim_tolower(msgx2024);
            tipDayu(msg2056, update);
            if (System.IO.File.Exists("menu/" + msgx2024 + ".txt"))
            {
                logCls.log(__METHOD__, func_get_args(), "Exists " + "menu/" + msgx2024 + ".txt", "logDir", reqThreadId);
                // var Keyboard = filex.wdsFromFileRendrToBtnmenu("menu/" + msgx2024 + ".txt");
                // var rkm = new InlineKeyboardMarkup(Keyboard);
                // KeyboardButton[][] kybd
                var Keyboard = filex.wdsFromFileRendrToTgBtmBtnmenuBycomma("menu/" + msgx2024 + ".txt");
                var rkm = new ReplyKeyboardMarkup(Keyboard);
                rkm.ResizeKeyboard = true;
                evt_btm_menuitem_clickV2(update?.Message?.Chat?.Id, "今日促销商家.gif", timerCls.plchdTxt, rkm, update);

                //botClient.SendTextMessageAsync()

                return;
            }

            if (msgx2024 == "↩️ 返回主菜单")
            {
                timerCls.evt_ret_mainmenu_sendMsg4keepmenu4btmMenu(update?.Message?.Chat?.Id, "今日促销商家.gif", timerCls.plchdTxt, tgBiz.tg_btmBtns());
                return;
            }

            if (msgx2024 == "↩️ 返回商家菜单")
            {
                evt_retMchrtBtn_click(update);
                //     await evt_btmBtnclick(botClient, update);
                return;
            }
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


            if (update?.Message?.ReplyToMessage?.From?.Username == botname &&
               strCls.contain(update?.Message?.Text, "世博博彩")
                )
            {
                evt_shiboBocai_click(update);
                return;
            }

            if (strCls.contain(update?.Message?.Text, "世博博彩")
              )
            {
                evt_shiboBocai_click(update);
                return;
            }


            //私聊消息  /start开始
            if (update?.Message?.Text == "/start")
            {
                evt_startMsgEvtInPrvtAddBot(update);
                return;
            }

            //add grp msgHDL
            if (update?.MyChatMember?.NewChatMember != null)
            {
                evt_botAddtoGrpEvtHdlr(update);
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
                await evt_pinlunShangjia(botClient, update, isAdminer, text);
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
                await 添加商家信息(botClient, update, text);

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
                    await botClient.SendTextMessageAsync(update.Message.Chat.Id, "@回复本信息,搜商家联系方式", parseMode: ParseMode.Html, replyToMessageId: update.Message.MessageId);
                }
                catch (Exception e)
                {
                    Console.WriteLine("告知@回复本信息,搜商家联系方式时出错:" + e.Message);
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
                        await botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id, "@回复本消息,即可对本商家评价 !(100字以内)", true);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("告诉别人怎么评价时出错:" + e.Message);
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
                //        Console.WriteLine("告诉对方您无权点击时出错:" + e.Message);
                //    }
                //    return;
                //}
            }


            //if nmrl msg  n notStartWith   @bot   ingor
            if (tgBiz.bot_isNnmlMsgInGrp(update))
            {
                Console.WriteLine(" bot_isNnmlMsgInGrp():ret=>true");
                return;
            }



            // if (msgx2024=="商家")

            #region sezrch

            HashSet<string> 商品与服务词库2 = ReadWordsFromFile("商品与服务词库.txt");
            string fuwuci = getFuwuci(update?.Message?.Text, 商品与服务词库2);
            SortedList whereMap2 = new SortedList();
            whereMap2.Add("fuwuci", fuwuci);

            //privt msg serch
            if (update?.Message?.Chat?.Type == ChatType.Private && update?.Type == UpdateType.Message)
            {
                HashSet<string> 商品与服务词库 = file_getWords商品与服务词库();
                if (!strCls.containKwds(update?.Message?.Text, string.Join(" ", 商品与服务词库)))
                {
                    Console.WriteLine(" 不包含商品服务词，ret");

                    Program.botClient.SendTextMessageAsync(update.Message!.Chat.Id, "未搜索到商家,您可以向我们提交商家联系方式", parseMode: ParseMode.Html, replyToMessageId: update.Message.MessageId);

                    //tglib.bot_dltMsgThenSendmsg(update.Message!.Chat.Id, update.Message.MessageId, "未搜索到商家,您可以向我们提交商家联系方式", 5);
                    return;
                    //  return;
                }
                string fuwuWd = getFuwuci(update?.Message?.Text, 商品与服务词库);
                // logCls.log(__METHOD__, func_get_args(),null,"logDir", reqThreadId);
                evt_msgTrgSrch(botClient, update, fuwuWd, reqThreadId);
                return;
            }


            //public search jude
            if (isGrpChat(update?.Message?.Chat?.Type) && update?.Type == UpdateType.Message)
            {
                string? msgx = tglib.bot_getTxt(update);
                if (msgx == null || msgx.Length > 25)
                {
                    Console.WriteLine(" msgx == null || msgx.Length > 25 ");
                    return;
                }
                msgx = msgx.Trim();
                if (msgx.Trim().StartsWith("@" + botname)) //goto seasrch
                    msgx = msgx.Substring(botname.Length + 1).Trim();
                msgx = msgx.Trim();

                HashSet<string> trgWdSt = ReadWordsFromFile("搜索触发词.txt");
                var trgWd = string.Join(" ", trgWdSt);
                Console.WriteLine(" 触发词 chk");
                if (!strCls.containKwds(update?.Message?.Text, trgWd))
                {
                    Console.WriteLine(" 不包含触发词，ret");
                    return;
                }

                //bao含触发词，进一步判断

                //去除搜索触发词，比如哪里有
                msgx = msgx.Replace("联系方式", " ");
                HashSet<string> hs = ReadWordsFromFile("搜索触发词.txt");
                msgx = replace_RemoveWords(msgx, hs);

                //是否包含搜索词 商品或服务关键词
                Console.WriteLine(" 商品或服务关键词 srch");
                HashSet<string> 商品与服务词库 = file_getWords商品与服务词库();
                if (!strCls.containKwds(update?.Message?.Text, string.Join(" ", 商品与服务词库)))
                {
                    Console.WriteLine(" 不包含商品服务词，ret");

                    return;
                }
                string fuwuWd = getFuwuci(update?.Message?.Text, 商品与服务词库);
                if (getFuwuci == null)
                {
                    Console.WriteLine(" 不包含商品服务词，ret");
                    return;
                }



                evt_msgTrgSrch(botClient, update, fuwuWd, reqThreadId);
                dbgCls.print_ret(__METHOD__, 0);
                return;
            }

            //pre page evt???  todo
            //next page evt,,,
            if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery!.Data!.Contains("page"))
            {
                await evt_nextPrePage(botClient, update, whereMap2, reqThreadId);
                return;
            }

            //return evt
            if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery!.Data!.Contains("return"))
            {
                await evt_ret_mchrt_list(botClient, update, whereMap2, reqThreadId);
                return;
            }


            //查看商家结果 defalt is detail view
            //         if (update.CallbackQuery.Data.StartsWith("Merchant?id="))
            if (update.Type is UpdateType.CallbackQuery)
            {
                Dictionary<string, string> parse_str1 = parse_str(update.CallbackQuery.Data);
                if (ldfld2str(parse_str1, "btn") == "dtl")
                {
                    evt_View(botClient, update, reqThreadId);
                }
                // logCls.log("FUN evt_msgTrgSrch", func_get_args(fuwuWd, reqThreadId), null, "logDir", reqThreadId);

            }
            #endregion


            #region add chatids
            await tglib.tg_addChtid(update);

            #endregion
            //}, cancellationToken);
            #endregion

            //}
            //catch (Exception e)
            //{
            //    logCls.logErr2025(e, "evt_msg_rcv", "errlog");
            //}


        }

     

       

        private static void OnCallbk(Update update, string reqThreadId)
        {
            // throw new NotImplementedException();
        }

        private static void OnMsg(Update update, string reqThreadId)
        {



        }

        private static void evt_lookmenu(CallbackQuery? callbackQuery)
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
                    Console.WriteLine("点击查看菜单,告知未提供菜单时时出错:" + e.Message);
                }
                return;
            }
        }

        private static async Task evtDafen(ITelegramBotClient botClient, Update update, Dictionary<string, string> parse_str1)
        {
            //evet dafen 
            if (ldfld2str(parse_str1, "ckuid") == "y")
            {
                if (!str_eq(update.CallbackQuery?.From?.Username, update.CallbackQuery?.Message?.ReplyToMessage?.From?.Username))
                {
                    await botClient.AnswerCallbackQueryAsync(
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

        public static void canSendBtn_click(Update e)
        {
            Dictionary<string, string> parse_str1 = parse_str(e.CallbackQuery.Data);
            string uid = ldfld2str(parse_str1, "uid");
            if (uid != e.CallbackQuery.From.Id.ToString())
            {
                botClient.AnswerCallbackQueryAsync(
                          callbackQueryId: e.CallbackQuery.Id,
                          text: "只能本人解除",
                          showAlert: true); // 这是显示对话框的关键);
                return;
            }


            botClient.RestrictChatMemberAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.From.Id, permissions: new Telegram.Bot.Types.ChatPermissions
            {
                CanSendMessages = true,
                // CanSendMediaMessages = true,
                CanSendOtherMessages = true,
                CanAddWebPagePreviews = true,
                CanSendDocuments = true,
                CanSendPhotos = false,
                CanSendPolls = true,
                CanSendVideoNotes = true,
                CanSendVideos = true,
                CanSendVoiceNotes = true,
                CanSendAudios = true

            });

            botClient.AnswerCallbackQueryAsync(e.CallbackQuery.Id, "已解除禁言！");

        }

        private static void evt_newUserJoin2024(long chatId, Telegram.Bot.Types.User[]? newChatMembers)
        {
            foreach (Telegram.Bot.Types.User u in newChatMembers)
            {
                evt_newUserjoinSngle(chatId, u.Id, u);
            }
        }



        private static string getFuwuci(string? text, HashSet<string> 商品与服务词库)
        {
            if (text == null)
                return null;
            string[] spltWds = splt_by_fenci(ref text);
            foreach (string wd in spltWds)
            {
                if (商品与服务词库.Contains(wd))
                    return wd;
            }
            return null;
        }



        private static async Task evt_msgTrgSrch(ITelegramBotClient botClient, Update update, string fuwuWd, string reqThreadId)
        {
            logCls.log("FUN evt_msgTrgSrch", func_get_args(fuwuWd, reqThreadId), null, "logDir", reqThreadId);
            SortedList whereMap = new SortedList();
            whereMap.Add("fuwuci", fuwuWd);
            var __METHOD__ = "evt_msgTrgSrch";
            dbgCls.print_call(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod()));

            string? msgx = tglib.bot_getTxtMsgDep(update);
            if (msgx.Trim().StartsWith("@" + botname))
                msgx = msgx.Substring(botname.Length + 1).Trim();
            msgx = msgx.Trim();

            msgx = msgx.Replace("联系方式", " ");
            //去除搜索触发词，比如哪里有
            HashSet<string> hs = ReadWordsFromFile("搜索触发词.txt");
            msgx = replace_RemoveWords(msgx, hs);
            // 搜索触发词





            if (msgx != null && msgx.Length < 25)
            {
                await GetList_qryV2(msgx, 1, 5, botClient, update, whereMap, reqThreadId);
                dbgCls.print_ret(__METHOD__, 0);

                return;
            }
            else
            {
                Console.WriteLine(" msg is null or leng>25");
                dbgCls.print_ret(__METHOD__, 0);
                return;
            }


        }


        private static async Task evt_nextPrePage(ITelegramBotClient botClient, Update update, SortedList whereMap2, string reqThreadId)
        {
            string? msgx = tglib.bot_getTxtMsgDep(update);

            if (msgx != null)
            {
                if (msgx.Trim().StartsWith("@" + Program.botname))
                    msgx = msgx.Substring(19).Trim();
                msgx = msgx.Trim();
                await GetList_qryV2(msgx, 1, 5, botClient, update, whereMap2, reqThreadId);
                return;
            }
        }

        private static async Task evt_ret_mchrt_list(ITelegramBotClient botClient, Update update, SortedList fuwuci, string reqThreadId)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.print_call(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), fuwuci, reqThreadId));

            logCls.log("fun evt_ret_mchrt_list", func_get_args(fuwuci), "", "logDir", reqThreadId);
            string? msgx = tglib.bot_getTxtMsgDep(update);
            // if msg==null ..just from timer send msg..ret no op
            if (msgx != null)
            {
                if (msgx.Trim().StartsWith("@" + Program.botname))
                    msgx = msgx.Substring(19).Trim();
                msgx = msgx.Trim();
                await GetList_qryV2(msgx, 1, 5, botClient, update, fuwuci, reqThreadId);
                return;
            }

            dbgCls.print_ret(__METHOD__, 0);
        }


        private static async void evt_btm_btn_click_inPubgrp(Update update)
        {

            //  ,
            try
            {
                Telegram.Bot.Types.Message a = await Program.botClient.SendTextMessageAsync(
                 update.Message.Chat.Id,
               "要获取多级菜单，请私聊我",
                 parseMode: ParseMode.Html,
                 replyMarkup: new InlineKeyboardMarkup([InlineKeyboardButton.WithUrl(text: "私聊我", $"https://t.me/{botname}")]),
                 protectContent: false,
                 replyToMessageId: update.Message.MessageId,
                 disableWebPagePreview: true

                 );
                tglib.bot_DeleteMessageV2(update.Message.Chat.Id, update.Message.MessageId, 9);
                tglib.bot_DeleteMessageV2(update.Message.Chat.Id, a.MessageId, 10);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            //todo reply
        }

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

        private static void evt_retMchrtBtn_click(Update update)
        {
            var Keyboard = filex.wdsFromFileRendrToTgBtmBtnmenuBycomma("menu/商家.txt");

            var rkm = new ReplyKeyboardMarkup(Keyboard);
             rkm.ResizeKeyboard = true;
            evt_btm_menuitem_clickV2(update?.Message?.Chat?.Id, "今日促销商家.gif", timerCls.plchdTxt, rkm, update);

            //  timerCls.evt_inline_menuitem_click_showSubmenu(update?.CallbackQuery?.Message?.Chat?.Id, "今日促销商家.gif", timerCls.plchdTxt, rkm, update);
            return;
        }


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

        private static async void evt_shiboBocai_click(Update? update)
        {
            //   RemoveCustomEmojiRendererElement("shiboRaw.htm", "shiboTrm.htm");

            string plchdTxt1422 = "💁 联信与世博联盟正式达成长期战略合作，联信为世博联盟旗下所有盘口提供双倍担保，确保100%真实可靠。\r\n\r\n在娱乐过程中，如发现世博联盟存在杀客、不予提现、杀大赔小等违规行为，请立即向联信负责人及运营团队举报。经核实后，联信将对您在世博联盟里因世博盘口违规行为造成的损失给予双倍赔偿！";

            string imgPath = "推荐横幅.gif";
            var Photo = InputFile.FromStream(System.IO.File.OpenRead(imgPath));


            InlineKeyboardButton[][] btns = tglib.ConvertHtmlLinksToTelegramButtons("shiboTrm.htm");
            Telegram.Bot.Types.Message message = await Program.botClient.SendPhotoAsync(
                  update.Message.Chat.Id, Photo, null,
             plchdTxt1422,
                    parseMode: ParseMode.Html,
                     replyMarkup: new InlineKeyboardMarkup(btns),
                   protectContent: false);

            //ori 64
            //Message message = await Program.botClient.SendPhotoAsync(
            //        update.Message.Chat.Id, Photo, null,
            //      System.IO.File.ReadAllText("shiboTrm.htm"),
            //          parseMode: ParseMode.Html,
            //         //   replyMarkup: new InlineKeyboardMarkup(results),
            //         protectContent: false);
            Console.WriteLine(JsonConvert.SerializeObject(message));
        }


        private static async Task evt_pinlunShangjia(ITelegramBotClient botClient, Update update, bool isAdminer, string? text)
        {
            if (text.StartsWith("@xxx007"))
                return;
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.print_call(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), isAdminer, text));

            HashSet<prj202405.City> _citys = getCitysObj();
            Console.WriteLine(" evt  @回复了商家详情信息  评价商家");
            var updateString = JsonConvert.SerializeObject(update);
            Match match = Regex.Match(updateString, @"(?<=\?id=).*?(?=&)");
            Merchant? merchant = match.Success ? (from c in _citys
                                                  from area in c.Address
                                                  from am in area.Merchant
                                                  where am.Guid == match.Value
                                                  select am).FirstOrDefault() : null;


            if (merchant == null)
            {
                Console.WriteLine("未找到目标商家");
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
                        msg = await botClient.SendTextMessageAsync(chatId: update.Message!.Chat.Id, text: "评价失败,评价文字只能100个字以内!", replyToMessageId: update.Message.MessageId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("告知评价字数不超过100时出错:" + ex.Message);
                    }

                    if (msg != null)
                    {
                        await Task.Delay(5000);
                        try
                        {
                            await botClient.DeleteMessageAsync(msg.Chat.Id, msg.MessageId);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("删除告知评价字数不可超过100字时出错:" + ex.Message);
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
                    Console.WriteLine(e);
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
                ormSqlt.save(obj1, "pinlunDir/" + merchant.Guid + merchant.Name + ".db");
                ormJSonFL.save(obj1, "pinlunDir/" + merchant.Guid + merchant.Name + ".json");

                user.Comments++;
                await biz_other._SaveConfig();
                try
                {
                    await tglib.bot_dltMsgThenSendmsg(update.Message!.Chat.Id, update.Message.MessageId, "成功点评了商家,本消息10秒后删除!", 10);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("告知成功点评了商家时出错:" + ex.Message);
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
                        await tglib.bot_dltMsgThenSendmsg(update.Message.Chat.Id, update.Message.MessageId, "编辑信息格式有误!", 5);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("告知编辑消息时出错:" + ex.Message);
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
                        Console.WriteLine("编辑商家分类时出错:" + ex.Message);
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
                        Console.WriteLine("编辑商家开始营业时出错:" + ex.Message);
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
                        Console.WriteLine("编辑商家截止营业时出错:" + ex.Message);
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
                        await tglib.bot_dltMsgThenSendmsg(update.Message.Chat.Id, update.Message.MessageId, "编辑信息格式有误!", 5);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("告知编辑消息时出错:" + ex.Message);
                    }
                    return;
                }

                await biz_other._SaveConfig();

                try
                {
                    await tglib.bot_dltMsgThenSendmsg(update.Message.Chat.Id, update.Message.MessageId, "商家信息编辑成功!", 5);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("告知编辑成功时出错:" + ex.Message);
                }
            }

            dbgCls.print_ret(__METHOD__, 0);

        }

        private static void evt_startMsgEvtInPrvtAddBot(Update update)
        {
            Program.botClient.SendTextMessageAsync(
                    update.Message.Chat.Id,
                    "请直接搜索园区/城市+商家/菜单即可,比如”金三角 会所”!",
                    parseMode: ParseMode.Html,
                    //   replyMarkup: new InlineKeyboardMarkup([]),
                    protectContent: false,
                    disableWebPagePreview: true);

            tglib.bot_saveChtSesion(update.Message.Chat.Id, update.Message.From);
        }

        private static void evt_botAddtoGrpEvtHdlr(Update update)
        {
            ReplyKeyboardMarkup rkm = tgBiz.tg_btmBtns();
            Program.botClient.SendTextMessageAsync(
                     update.MyChatMember.Chat.Id,
                     "我是便民助手,你们要问什么商家,我都知道哦!",
                     parseMode: ParseMode.Html,
                      replyMarkup: rkm,
                     protectContent: false,
                     disableWebPagePreview: true);
            tglib.bot_saveGrpInf2db(update.MyChatMember);
            tglib.bot_saveChtSesion(update.MyChatMember.Chat.Id, update.MyChatMember);
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
        //                                Console.WriteLine("告诉别人怎么评价时出错:" + e.Message);
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
        //                            Console.WriteLine("返回按钮添加商家回调出错:" + ex.Message);
        //                        }

        //                        return;
        //                    }

        //qry shaojia
        //获取列表,或者是返回至列表
        static async Task GetList_qryV2(string msgx, int pagex, int pagesizex, ITelegramBotClient botClient, Update update, SortedList whereMapDep, string reqThreadId)
        {
            var __METHOD__ = "GetList_qryV2";  //bcs in task so cant get currentmethod
            print_call(__METHOD__, func_get_args(__METHOD__, msgx));
            logCls.log("fun GetList_qryV2", func_get_args(msgx, pagex, pagesizex, whereMapDep), "", "logDir", reqThreadId);
            if (msgx == null || msgx.Length == 0)
                return;
            //  Console.WriteLine(" fun  GetList()");
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
                msgx = update?.CallbackQuery?.Message?.ReplyToMessage?.Text;

            msgx = msgx.Trim();

            //kwd if ret list btn cmd cmd
            if (update.Type == UpdateType.CallbackQuery)
            {
                if (msgx.Trim().StartsWith("@" + Program.botname))
                    msgx = msgx.Substring(19).Trim();
                else
                    msgx = msgx.Trim();
            }


            Console.WriteLine("  msg=>" + msgx);

            if (!string.IsNullOrEmpty(msgx))

            {
                //    List<InlineKeyboardButton[]> results = [];  &park=世纪新城园区
                if (isGrpChat(update))
                {
                    // update.Message.Chat.Id;
                    string chatid2249 = tglib.bot_getChatid(update).ToString();

                    //  List<Dictionary<string, string>> lst = ormSqlt._qryV2($"select * from grp_loc_tb where grpid='{groupId}'", "grp_loc.db");

                    List<SortedList> lst = ormJSonFL.qry($"{prjdir}/grpCfgDir/grpcfg{chatid2249}.json");
                    string whereExprs = (string)db.getRowVal(lst, "whereExprs", "");
                    //    city = "

                    //qry from mrcht by  where exprs  strFmt
                    Dictionary<string, StringValues> whereExprsObjFiltrs = QueryHelpers.ParseQuery(whereExprs);
                    // whereExprsObj.Add("fuwuci", ldfld_TryGetValueAsStrDefNull(whereMap, "fuwuci"));
                    //here only one db so no mlt ,todo need updt
                    // results = mrcht.qryByMsgKwdsV3(patns_dbfs, whereExprsObj);
                    string sharNames = ldfld_TryGetValue(whereExprsObjFiltrs, "@share");
                    results = mrcht.qryFromMrcht("mercht商家数据", sharNames, whereExprsObjFiltrs, msgx);

                }
                else
                { //privet serach
                  // update.Message.Chat.Id;
                    string chatid2249 = tglib.bot_getChatid(update).ToString();
                  
                    string dbfile = $"{prjdir}/cfg_prvtChtPark/{chatid2249}.json";

                    SortedList cfg = findOne(dbfile);

                    Dictionary<string, StringValues> whereExprsObj = CopySortedListToDictionary(cfg);
                    //todo set    limit  cdt into 
                    results = mrcht.qryFromMrcht("mercht商家数据", null, whereExprsObj, msgx);

                }

                //  results = arrCls.rdmList<InlineKeyboardButton[]>(results);
                count = results.Count;

                //GetList_qryV2 
                if (count == 0 && (update?.Message?.Chat?.Type == ChatType.Private))
                {

                    await tglib.bot_dltMsgThenSendmsg(update.Message!.Chat.Id, update.Message.MessageId, "未搜索到商家,您可以向我们提交商家联系方式", 5);
                    return;
                }

                if (count == 0)   //in pubgrp
                {
                    Console.WriteLine(" evt serch.  in public grp. srch rzt cnt =0,so ret");
                    return;
                }

                results = results.Skip(page * pagesize).Take(pagesize).ToList();
            }

            //发起查询  stzrt with @bot
            if (update!.Type is UpdateType.Message)
            {
                // keyword = update?.Message?.Text;
                //   keyword = keyword.Substring(19).Trim();
                //if (msgx?.Length is < 2 or > 8)
                //{
                //    await bot_DeleteMessage(update.Message!.Chat.Id, update.Message.MessageId, "请输入2-8个字符的的关键词", 5);
                //    return;
                //}

                if (count == 0)
                {

                    //await botapi.bot_DeleteMessage(update.Message!.Chat.Id, update.Message.MessageId, "未搜索到商家,您可以向我们提交商家联系方式", 5);
                    return;
                }
                user.Searchs++;
            }
            //返回列表
            else
            {
                var cq = update!.CallbackQuery!;
                if (string.IsNullOrEmpty(msgx))
                {
                    try
                    {
                        await botClient.AnswerCallbackQueryAsync(cq.Id, "搜索关键词已经删除,需重新搜索!", true);
                        await botClient.DeleteMessageAsync(cq.Message!.Chat.Id, cq.Message.MessageId);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("告知搜索关键词已经删除时出错:" + e.Message);
                    }
                    return;
                }
                user.Returns++;
            }



            // pagebtns
            evt_search_pt3_btns翻页(page, count, pagesize, results);

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
                    //  Console.WriteLine(string.Format("{0}-{1}", de.Key, de.Value));
                    var Photo = InputFile.FromStream(System.IO.File.OpenRead(Path));
                    await botClient.SendPhotoAsync(
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

                    await botClient.EditMessageCaptionAsync(
                     update.CallbackQuery.Message.Chat.Id,
                   caption: text,

                     replyMarkup: new InlineKeyboardMarkup(results),
                   messageId: update.CallbackQuery.Message.MessageId,
                    parseMode: ParseMode.Html
                    );
                    //await botClient.EditMessageTextAsync(
                    //    chatId: update!.CallbackQuery!.Message!.Chat.Id,
                    //    messageId: update.CallbackQuery.Message.MessageId,
                    //    text: text,
                    //    disableWebPagePreview: true,
                    //    parseMode: ParseMode.Html,
                    //    replyMarkup: new InlineKeyboardMarkup(results));
                }

                HashSet<prj202405.City> _citys = getCitysObj();
                //每个商家搜索量
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

                await biz_other._SaveConfig();
            }
            catch (Exception e)
            {
                Console.WriteLine("返回商家联系方式列表时出错:" + e.Message);
            }


            //   Console.WriteLine(" endfun  GetList()");
            print_ret(__METHOD__, "");

        }

        static Dictionary<string, StringValues> CopySortedListToDictionary(SortedList sortedList)
        {
            Dictionary<string, StringValues> dictionary = new Dictionary<string, StringValues>();

            foreach (DictionaryEntry entry in sortedList)
            {
                string key = entry.Key as string;
                string value = entry.Value as string;

                if (key != null && value != null)
                {
                    dictionary[key] = new StringValues(value);
                }
            }

            return dictionary;
        }


        //static async Task evt_btnclick_Pt2_qryByKwd(string msgx, int pagex, int pagesizex, ITelegramBotClient botClient, Update update)
        //{
        //    var __METHOD__ = "evt_btnclick_Pt2_qryByKwd";  //bcs in task so cant get currentmethod
        //    dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), msgx));

        //    if (msgx == null || msgx.Length == 0)
        //        return;
        //    Console.WriteLine(" fun  GetList()");


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


        //    Console.WriteLine("  msg=>" + msgx);

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
        //    //            Console.WriteLine("告知搜索关键词已经删除时出错:" + e.Message);
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
        //        //  Console.WriteLine(string.Format("{0}-{1}", de.Key, de.Value));
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
        //        Console.WriteLine("返回商家联系方式列表时出错:" + e.Message);
        //    }


        //  //  Console.WriteLine(" endfun  GetList()");
        //    dbgCls.setDbgValRtval(__METHOD__, "");
        //}


        private static void evt_search_pt3_btns翻页(int page, int count, int pagesize, List<InlineKeyboardButton[]> results)
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
        //    Console.WriteLine(" fun  GetList()");
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


        //    Console.WriteLine("  kwd=>" + keyword);

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
        //                Console.WriteLine("告知搜索关键词已经删除时出错:" + e.Message);
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
        //            //  Console.WriteLine(string.Format("{0}-{1}", de.Key, de.Value));
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
        //        Console.WriteLine("返回商家联系方式列表时出错:" + e.Message);
        //    }


        //    Console.WriteLine(" endfun  GetList()");

        //}

        //获取商家结果 detail click
        static async Task evt_View(ITelegramBotClient botClient, Update update, string reqThreadId)
        {
            var __METHOD__ = "evt_View listitem_click()";
            dbgCls.print_call(__METHOD__, dbgCls.func_get_args(update, reqThreadId));
            logCls.log("FUN " + __METHOD__, func_get_args(reqThreadId, update), null, "logDir", reqThreadId);

            Dictionary<string, string> parse_str1 = parse_str(update.CallbackQuery.Data);
            if (ldfld2str(parse_str1, "btn") == "dtl") //def is not  
                if (ldfld2str(parse_str1, "ckuid") == "y")
                    if (!str_eq(update.CallbackQuery?.From?.Username, update.CallbackQuery?.Message?.ReplyToMessage?.From?.Username))
                    {


                        Console.WriteLine("not same user...ret");
                        await botClient.AnswerCallbackQueryAsync(
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
            if (ldfld2str(parse_str2, "btn") == "detail") //def is not   
            {
                //need chk
            }


            var cq = update.CallbackQuery!;


            Dictionary<string, StringValues> whereExprsObj = ParseQuery2024(update.CallbackQuery.Data);
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
            //    Console.WriteLine("查看结果时显示未找到此商家,此处有错误");
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
            var operaCount = await biz_other._SetUserOperas(cq.From.Id);
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
            //        Console.WriteLine("告知查询次数太多时出错:" + e.Message);
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
                        await botClient.AnswerCallbackQueryAsync(cq.Id, "一个账号只能打分一次,请勿重复打分!", true);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("告知已评过分时出错:" + e.Message);
                    }
                    return;
                }

                contact_Merchant.Scores.Add(cq.From.Id, (int)score);
                user.Scores++;
                try
                {
                    await botClient.AnswerCallbackQueryAsync(cq.Id, "评分成功", true);
                }
                catch (Exception e)
                {
                    Console.WriteLine("告知评分成功时出错:" + e.Message);
                }

                Telegram.Bot.Types.Message scoreTipMsg = null;
                try
                {
                    //感谢打分
                    scoreTipMsg = await botClient.SendTextMessageAsync(
                        chatId: cq.Message.Chat.Id,
                        text: $"😙 <b>匿名用户对商家进行了打分</b>",
                        parseMode: ParseMode.Html,
                        replyToMessageId: cq.Message.MessageId,
                        disableNotification: false);
                }
                catch (Exception e)
                {
                    Console.WriteLine("感谢打分时出错:" + e.Message);
                }

                _ = Task.Run(async () =>
                {
                    await Task.Delay(10000);
                    if (scoreTipMsg == null)
                    {
                        try
                        {
                            await botClient.DeleteMessageAsync(scoreTipMsg.Chat.Id, scoreTipMsg.MessageId);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("删除评分提示时出错:" + e.Message);
                        }
                    }
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

            Console.WriteLine(result);
            //人气排名   
            //int rank = merchants.OrderByDescending(e => e.Views).ToList().FindIndex(e => e.Guid == guid) + 1;
            //result += rank switch
            //{
            //    1 => $"\n\n🏆<b>商家排名</b> 第<b>🥇</b>名 (受欢迎程度)",
            //    2 => $"\n\n🏆<b>商家排名</b> 第<b>🥈</b>名 (受欢迎程度)",
            //    3 => $"\n\n🏆<b>商家排名</b> 第<b>🥉</b>名 (受欢迎程度)",
            //    _ => $"\n\n🏆<b>商家排名</b> 第<b> {rank} </b>名 (受欢迎程度)",
            //};

            copyPropSortedListToMerchant(Merchant1, contact_Merchant);
            //营业时间
            try
            {
                TimeSpan StartTime = TimeSpan.Parse(Merchant1["开始时间"].ToString());
                TimeSpan EndTime = TimeSpan.Parse(Merchant1["结束时间"].ToString());
                result += "\n\n⏱<b>营业时间</b> " + timeCls.FormatTimeSpan(TimeSpan.Parse(Merchant1["开始时间"].ToString())) + "-" + timeCls.FormatTimeSpan(TimeSpan.Parse(Merchant1["结束时间"].ToString())) + " " + biz_other._IsBusinessHours(StartTime, EndTime);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
            result = evt_detail_rendLianxiFosh(contact_Merchant, result);
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
            Console.WriteLine(result);
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
            List<List<InlineKeyboardButton>> menu = GetMenuDafen(guid, chkUidEq, ldfld2str(parse_str1, "sdr"));

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
            if (ldfld2str(parse_str1, "sdr") == "tmr") //def is not

            {
                // await botClient.SendTextMessageAsync(chatId: cq.Message.Chat.Id, text: result, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(menu), disableWebPagePreview: true);
                string imgPath = "搜索横幅.gif";
                var Photo2 = InputFile.FromStream(System.IO.File.OpenRead(imgPath));
                Telegram.Bot.Types.Message message2 = await Program.botClient.SendPhotoAsync(
              chatId: cq.Message.Chat.Id
                  , Photo2, null,
                 caption: result,
                    parseMode: ParseMode.Html,
                   replyMarkup: new InlineKeyboardMarkup(menu),
                   protectContent: false);
                dbgCls.print_ret(__METHOD__, 0);
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
                    Telegram.Bot.Types.Message m = await botClient.EditMessageCaptionAsync(chatId: cq.Message.Chat.Id, messageId: cq.Message.MessageId, caption: result, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(menu));

                    logCls.log(m, "detailClickLogDir");
                }
                catch (Exception e)
                {
                    logCls.logErr2025(e, "detal click()", "errlog");
                }


                dbgCls.print_ret(__METHOD__, 0);

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
                await botClient.EditMessageTextAsync(chatId: cq.Message.Chat.Id, messageId: cq.Message.MessageId, text: result, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(menu));
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
                        await botClient.AnswerCallbackQueryAsync(cq.Id, "已经显示了", true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("已经显示了,请勿重复点击时候出错:" + ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("编辑联系方式时出错:" + e.Message);
                }
                await biz_other._SaveConfig();
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
        }

        private static List<string> cvt2list(SortedList merchant1, string v)
        {
            List<string> li = new List<string>();
            try
            {
                li.Add(trim_RemoveUnnecessaryCharacters4tgWhtapExt(ldfld(merchant1, v, "").ToString()));

            }
            catch (Exception e)
            {

            }

            return li;
        }

        private static string evt_detail_rendLianxiFosh(Merchant? contact_Merchant, string result)
        {

            #region 联系方式
            result += "\n\n<b>-------------联系方式-------------</b>";
            Console.WriteLine(result);
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
