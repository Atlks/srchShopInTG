global using static mdsj.lib.logCls;
global using static prjx.timerCls;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Collections;
using System.Timers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using prjx.lib;
using JiebaNet.Segmenter;
using System.Reflection;
using ChineseCharacterConvert;
using Convert = System.Convert;
using System.Runtime.CompilerServices;
using mdsj.libBiz;
using static mdsj.biz_other;
using static prjx.timerCls;
using System.Collections.Generic;
using DocumentFormat.OpenXml;
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
using static mdsj.lib.net_http;
using static mdsj.lib.web3;
using static mdsj.libBiz.tgBiz;
using static prjx.lib.tglib;

using static mdsj.lib.util;
using NAudio.Wave;
using Newtonsoft.Json.Linq;
namespace prjx
{
    internal class timerCls
    {
        public const string chatSessStrfile = "chtSess.json";


        public static void setTimerTask()
        {
            // return;
            //活动商家(每小时推送)   物业(跟随每个商家推送) 
            //早餐店6点推送   午餐店11点推送推送   下午茶(水果/奶茶)店16点推送   晚餐店18点推送    娱乐消遣/酒店推送21点推送   活动商家(每小时推送)   物业(跟随每个商家推送)    每日人气榜单(每日夜间0:00推送)
            //_ = Task.Run(async () =>
            //{
            //    while (true)
            //    {
            //        var now = DateTime.Now;

            //        await Task.Delay(1000);
            //    }
            //});


            //设置定时间隔(毫秒为单位)
            int interval = 15 * 1000;  //15s 一次，一共四次机会每小时。。
            //因为设施了每小时 01分才触发
            System.Timers.Timer timer = new System.Timers.Timer(interval);
            //设置执行一次（false）还是一直执行(true)
            timer.AutoReset = true;
            //设置是否执行System.Timers.Timer.Elapsed事件
            timer.Enabled = true;
            //绑定Elapsed事件
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timerCls.TimerEvt);
            timer.Start();
        }


        public static void setTimerTask4prs()
        {



            //设置定时间隔(毫秒为单位)
            int interval = 5 * 60 * 1000;  //15s 一次，一共四次机会每小时。。
            //因为设施了每小时 01分才触发
            System.Timers.Timer timer = new System.Timers.Timer(interval);
            //设置执行一次（false）还是一直执行(true)
            timer.AutoReset = true;
            //设置是否执行System.Timers.Timer.Elapsed事件
            timer.Enabled = true;
            //绑定Elapsed事件
            timer.Elapsed += new System.Timers.ElapsedEventHandler((object? sender, ElapsedEventArgs e) =>
            {
                rdCnPrs();
            });
            timer.Start();
        }

        public static void setTimerTask4tmr()
        {
            DateTime now = DateTime.Now;
            if (now.Hour >= 18 || now.Hour < 9)
                return;

            //设置定时间隔(毫秒为单位)
            int interval = 15 * 60 * 1000;
            //2 * 60 * 1000;  //15s 一次，一共四次机会每小时。。
            //因为设施了每小时 01分才触发
            System.Timers.Timer timer = new System.Timers.Timer(interval);
            //设置执行一次（false）还是一直执行(true)
            timer.AutoReset = true;
            //设置是否执行System.Timers.Timer.Elapsed事件
            timer.Enabled = true;
            //绑定Elapsed事件
            timer.Elapsed += new System.Timers.ElapsedEventHandler((object? sender, ElapsedEventArgs e) =>
            {
                //  string mp3FilePath = "C:\\Users\\Administrator\\OneDrive\\90后非主流的歌曲 v2 w11\\Darin-Be What You Wanna Be HQ.mp3"; // 替换为你的 MP3 文件路径

                playMp3(mp3FilePath_slowSkedu);

            });
            timer.Start();
        }





        internal static void TimerEvt(object? sender, ElapsedEventArgs e)
        {

            System.IO.Directory.CreateDirectory("tmrlg");
            Print("定时任务。。");

            DateTime now = DateTime.Now;

            早餐(now);

            Chk_launch午餐(now);

            Chk_aftnTea(now);

            Chktrg_wecan(now);
            chktrg_yule(now);

            renciNshangj(now);
            keepBtnMenu(now);

            int btr = GetBatteryPercentage();
            if (btr < 90)
            {
                // playMp3(mp3FilePathEmgcy);
            }

            static void 早餐(DateTime now)
            {
                // //早餐
                string zaocanLgF = $"tmrlg/brkfstPushLog{Convert.ToString(now.Month) + now.Day}.json";

                if (now.Hour == 6 && now.Minute == 1 && (!System.IO.File.Exists(zaocanLgF)))
                {
                    // do something
                    System.IO.File.WriteAllText(zaocanLgF, "pushlog");
                    // Program.botClient.SendTextMessageAsync(chatId: Program.groupId, text: "早餐时间到了");
                    zaocan();
                }
            }

            static void Chk_launch午餐(DateTime now)
            {
                //午餐
                string lauch = $"tmrlg/lunchPushLog{Convert.ToString(now.Month) + now.Day}.json";
                if (now.Hour == 11 && now.Minute == 1 && (!System.IO.File.Exists(lauch)))

                {
                    Print("push luch time。");
                    System.IO.File.WriteAllText($"tmrlg/lunchPushLog{Convert.ToString(now.Month) + now.Day}.json", "pushlog");
                    //  Program.botClient.SendTextMessageAsync(chatId: Program.groupId, text: "午餐时间到了");
                    z_wucan();

                }
            }

            static void Chk_aftnTea(DateTime now)
            {
                //下午差
                var xwcF = $"tmrlg/xiawuchaPushLog{Convert.ToString(now.Month) + now.Day}.json";
                if (now.Hour == 16 && now.Minute == 1 && (!System.IO.File.Exists(xwcF)))
                {
                    System.IO.File.WriteAllText(xwcF, "pushlog");
                    // do something
                    z_xiawucha();
                }
            }

            static void Chktrg_wecan(DateTime now)
            {
                //晚餐
                //18,wecan,wancan()
                var vecan = $"tmrlg/wecanPushLog{Convert.ToString(now.Month) + now.Day}.json";
                if (now.Hour == 18 && now.Minute == 1 && (!System.IO.File.Exists(vecan)))
                {
                    System.IO.File.WriteAllText(vecan, "pushlog");
                    // do something
                    tmEvt_z18_wancan();
                }
            }

            static void renciNshangj(DateTime now)
            {

                //人气榜
                var rqF = $"tmrlg/renqiPushLog{Convert.ToString(now.Month) + now.Day}.json";
                if (now.Hour == 0 && now.Minute == 1 && (!System.IO.File.Exists(rqF)))
                {
                    System.IO.File.WriteAllText(rqF, "pushlog");
                    // do something
                    z_renqi();
                }

                //#huodong 商家
                var hour = "8";
                var huodonMrcht = $"tmrlg/actShjPushLog{Convert.ToString(now.Month) + now.Day + Convert.ToString(now.Hour)}.json";
                if (now.Hour == 8 && now.Minute == 1 && (!System.IO.File.Exists(huodonMrcht)))
                {
                    System.IO.File.WriteAllText(huodonMrcht, "pushlog");
                    // do something
                    //    z_actSj();
                }
            }

            static void keepBtnMenu(DateTime now)
            {
                var tsoxiaoShjk = $"tmrlg/actMenuPushLog{Convert.ToString(now.Month) + now.Day + Convert.ToString(now.Hour)}.json";
                if ((now.Hour == 10 || now.Hour == 16) && now.Minute == 1 && (!System.IO.File.Exists(tsoxiaoShjk)))
                {
                    System.IO.File.WriteAllText(tsoxiaoShjk, "pushlog");
                    var txtkeepBtnMenu = "";// "美好的心情从现在开始\n";

                    tmrEvt_sendMsg4keepmenu("今日促销商家.gif", txtkeepBtnMenu + plchdTxt);
                }
            }
        }

        //static void tmrEvtLLLzhuligrp(DateTime now)
        //{
        //    var zhuliLog = $"tmrlg/zhuliLog{Convert.ToString(now.Month) + now.Day + Convert.ToString(now.Hour)}.json";
        //    if ((now.Hour == 10 || now.Hour == 16) && now.Minute == 1 && (!System.IO.File.Exists(zhuliLog)))
        //    {
        //        System.IO.File.WriteAllText(zhuliLog, "pushlog");
        //        var txtkeepBtnMenu = "";// "美好的心情从现在开始\n";

        //        tmrEvtLLLzhuligrpSendmsg();
        //            }
        //}

        //public static void tmrEvtLLLzhuligrpSendmsg()
        //{
        //    var chtsSess = JsonConvert.DeserializeObject<Hashtable>(System.IO.File.ReadAllText(timerCls.chatSessStrfile))!;
        //    foreach_hashtable(chtsSess, (de) =>
        //   {
        //       var chatid = de.Key;
        //       print(" SendPhotoAsync " + de.Key);
        //       var map = de.Value;
        //       JObject jo = (JObject)map;
        //       string chtType = getFld(jo, "chat.type", "");
        //       string grpusername = getFld(jo, "chat.username", "");
        //       ReplyKeyboardMarkup rplyKbdMkp;
        //       //私聊不要助力本群
        //       if (chtType.Contains("group"))
        //       {
        //           sendZhuliGrp(chatid, zhuli_tips, zhuli_btn, grpusername);
        //       }
        //   });
        //}


        private static void chktrg_yule(DateTime now)
        {
            //娱乐
            var ylF = $"tmrlg/yulePushLog{Convert.ToString(now.Month) + now.Day}.json";
            if (now.Hour == 21 && now.Minute == 1 && (!System.IO.File.Exists(ylF)))
            {
                System.IO.File.WriteAllText(ylF, "pushlog"); //if wrt lg err.not next send
                // do something
                z21_yule();
            }
        }

        /// <summary>
        /// /、、https://t.me/shibolianmeng
        /// </summary>
        public static string plchdTxt = "💁博彩盘推荐：<a href='https://sb.game'><b>世博联盟</b></a>";

        //static string   plchdTxt = "💸 信誉博彩盘推荐 :  世博联盟飞投博彩 (https://t.me/shibolianmeng) 💸";
        public static async void z_actSj()
        {
            var __METHOD__ = "z_actSj";
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod()));

            HashSet<prjx.City> _citys = getCitysObj();
            List<InlineKeyboardButton[]> results = [];
            results = (from c in _citys
                       from ca in c.Address
                       from am in ca.Merchant
                       orderby am.Views descending
                       select new[] { new InlineKeyboardButton(c.Name + " • " + ca.Name + " • " + am.Name) { CallbackData = $"Merchant?id={am.Guid}&timerMsgMode2025" } }).ToList();
            //count = results.Count;


            results = GetRdmList<InlineKeyboardButton[]>(results);

            results = results.Skip(0 * 10).Take(5).ToList();



            string Path = "今日促销商家.gif";
            if (results.Count > 0)
                bot_sendMsgToMlt(Path, plchdTxt, results);
            dbgCls.PrintRet(__METHOD__, 0);
        }



        public static async Task evt_inline_menuitem_click_showSubmenu(long? chat_id, string imgPath, string msgtxt, InlineKeyboardMarkup rplyKbdMkp, Update? update)
        {
            // [CallerMemberName] string methodName = ""
            //  CallerMemberName 只能获取上一级的调用方法，不能本级别的，只好手工赋值了
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            //  __METHOD__ = methodName;
            __METHOD__ = "evt_menuitem_click";  //bcs in task so cant get currentmethod
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), chat_id, rplyKbdMkp));


            //  Program.botClient.send
            try
            {
                var Photo2 = InputFile.FromStream(System.IO.File.OpenRead(imgPath));
                //   Message message2 =   await Program.botClient.EditMessageReplyMarkupAsync(chat_id,(int)update?.Message?.MessageId, rplyKbdMkp);
                Message message2 = await Program.botClient.SendTextMessageAsync(
                chat_id, msgtxt,
                    parseMode: ParseMode.Html,
                   replyMarkup: rplyKbdMkp,
                   protectContent: false, disableWebPagePreview: true);
                Print(JsonConvert.SerializeObject(message2));



            }
            catch (Exception ex)
            {
                Print(ex.ToString());
            }

            dbgCls.PrintRet(__METHOD__, 0);



        }


        public static async Task evt_ret_mainmenu_sendMsg4keepmenu4btmMenu(long? chat_id, string imgPath, string msgtxt, ReplyKeyboardMarkup rplyKbdMkp)
        {


            //  Program.botClient.send
            try
            {
                var Photo2 = InputFile.FromStream(System.IO.File.OpenRead(imgPath));
                Message message2 = await Program.botClient.SendTextMessageAsync(
               chat_id, msgtxt,
                    parseMode: ParseMode.Html,
                   replyMarkup: rplyKbdMkp,
                   protectContent: false, disableWebPagePreview: true);
                Print(JsonConvert.SerializeObject(message2));

                //Program.botClient.SendTextMessageAsync(
                //         Program.groupId,
                //         "活动商家",
                //         parseMode: ParseMode.Html,
                //         replyMarkup: new InlineKeyboardMarkup(results),
                //         protectContent: false,
                //         disableWebPagePreview: true);

            }
            catch (Exception ex) { Print(ex.ToString()); }





        }


        public static async Task tmrEvt_sendMsg4keepmenu(string imgPath, string msgtxt)
        {
            var chtsSess = JsonConvert.DeserializeObject<Hashtable>(System.IO.File.ReadAllText(timerCls.chatSessStrfile))!;
            //    chtsSess.Add(Program.groupId, "");

            //遍历方法三：遍历哈希表中的键值
            foreach (DictionaryEntry de in chtsSess)
            {
                //if (Convert.ToInt64(de.Key) == Program.groupId)
                //    continue;
                var chatid = de.Key;
                Print(" SendPhotoAsync " + de.Key);
                var map = de.Value;
                JObject jo = (JObject)map;
                string chtType = GetFld(jo, "chat.type", "");

                ReplyKeyboardMarkup rplyKbdMkp;
                //  Program.botClient.send
                try
                {
                    rplyKbdMkp = SetBtmBtnMenu(imgPath, msgtxt, chatid, chtType);
                    //  print(JsonConvert.SerializeObject(message2));

                    //Program.botClient.SendTextMessageAsync(
                    //         Program.groupId,
                    //         "活动商家",
                    //         parseMode: ParseMode.Html,
                    //         replyMarkup: new InlineKeyboardMarkup(results),
                    //         protectContent: false,
                    //         disableWebPagePreview: true);

                }
                catch (Exception ex) { Print(ex.ToString()); }

            }



        }

        public static ReplyKeyboardMarkup SetBtmBtnMenuClr(string imgPath, string msgtxt, object chatid, string chtType)
        {
            Print("SetBtmBtnMenuClr ()");
            ReplyKeyboardMarkup rplyKbdMkp;


            // 创建一个空的 KeyboardButton[][] 数组
            KeyboardButton[][] kbtns = new KeyboardButton[0][];
            rplyKbdMkp = new ReplyKeyboardMarkup(kbtns);

            // Create an empty ReplyKeyboardRemove to clear the keyboard
            var rplyKbdMkp2 = new ReplyKeyboardRemove();

            //def is grp btns
            //tgBiz.tg_btmBtns()
            //   var Photo2 = InputFile.FromStream(System.IO.File.OpenRead(imgPath));
            //  Message message2dbg = await 
            Message m = Program.botClient.SendTextMessageAsync(
    Convert.ToInt64(chatid), msgtxt,
        parseMode: ParseMode.Html,
       replyMarkup: rplyKbdMkp2,
       protectContent: false, disableWebPagePreview: true).GetAwaiter().GetResult();
            lastSendMsg.Value = m;
            return rplyKbdMkp;
        }


        public static ReplyKeyboardMarkup SetBtmBtnMenu(string imgPath, string msgtxt, object chatid, string chtType)
        {
            lastSendMsg.Value=null;
           ReplyKeyboardMarkup rplyKbdMkp;
            //私聊不要助力本群
            if (!chtType.Contains("group"))
            {
                rplyKbdMkp = tgBiz.tg_btmBtns();
                KeyboardButton[][] kbtns = (KeyboardButton[][])rplyKbdMkp.Keyboard;
                RemoveButtonByName(kbtns, juliBencyon);
            }
            else
            {
                rplyKbdMkp = tgBiz.tg_btmBtns();
            }


            //def is grp btns
            //tgBiz.tg_btmBtns()
            var Photo2 = InputFile.FromStream(System.IO.File.OpenRead(imgPath));
            //  Message message2dbg = await 
        Message m=    Program.botClient.SendTextMessageAsync(
         Convert.ToInt64(chatid), msgtxt,
             parseMode: ParseMode.Html,
            replyMarkup: rplyKbdMkp,
            protectContent: false, disableWebPagePreview: true).GetAwaiter().GetResult();
            lastSendMsg.Value = m;
            return rplyKbdMkp;
        }


        //private static void wancan()
        //{
        //    throw new NotImplementedException();
        //}

        public static async void z_renqi()
        {



            string Path = "今日商家人气榜.gif";




            var s = "";
            List<InlineKeyboardButton[]> results = [];
            HashSet<prjx.City> _citys = getCitysObj();
            results = (from c in _citys
                       from ca in c.Address
                       from am in ca.Merchant
                           //   where searchChars.All(s => (c.CityKeywords + ca.CityKeywords + am.KeywordString + am.KeywordString + Program._categoryKeyValue[(int)am.Category]).Contains(s))
                       orderby am.Views descending
                       select new[] { new InlineKeyboardButton(c.Name + " • " + ca.Name + " • " + am.Name) { CallbackData = $"Merchant?id={am.Guid}&timerMsgMode2025" } }).ToList();
            //count = results.Count;
            results = results.Skip(0 * 10).Take(5).ToList();
            if (results.Count > 0)
                bot_sendMsgToMltV2("今日商家人气榜.gif", plchdTxt, "");
        }

        string CaptionTxt = "美好的一天从晚上开始，激动的心，颤抖的手,又到了娱乐时间啦";

        public static async void z21_yule()
        {
            //咖啡爆 gogobar 啤酒吧 帝王浴 泡泡浴 nuru 咬吧 马杀鸡
            var s = "ktv 水疗 会所 嫖娼 酒吧 足疗 spa  按摩 ";
            //   List<InlineKeyboardButton[]> results = qry_ByKwds_OrderbyRdm_Timermode_lmt5(s);


            string Path = "娱乐消遣.gif";

            //  if (results.Count > 0)
            bot_sendMsgToMltV2("娱乐消遣.gif", plchdTxt, s);

        }

        public static async void zaocan()
        {
            var s = "早餐 餐饮 早点 牛肉 火锅  炒粉";
            //    List<InlineKeyboardButton[]> results = qry_ByKwds_OrderbyRdm_Timermode_lmt5(s);



            string Path = "早餐商家推荐.gif";
            //   var CaptionTxt = "美好的一天从早上开始，当然美丽的心情从早餐开始，别忘了吃早餐哦";

            //   if(results.Count>0)
            bot_sendMsgToMltV2("早餐商家推荐.gif", plchdTxt, s);
        }


        public static async void tmEvt_z18_wancan()
        {

            var s = "餐饮 牛肉 火锅 炒粉";
            List<InlineKeyboardButton[]> results = new List<InlineKeyboardButton[]>();
            //qry_ByKwds_OrderbyRdm_Timermode_lmt5(s);
            string CaptionTxt = "晚餐时间到了！让我们一起享受美食和愉快的时光吧！！";

            //   if (results.Count > 0)
            bot_sendMsgToMltV2("晚餐商家推荐.gif", plchdTxt, s);

        }
        public static async void z_wucan()
        {
            var wdss = "餐饮 牛肉 火锅 炒粉";

            //   List<InlineKeyboardButton[]> results = qry_ByKwds_OrderbyRdm_Timermode_lmt5(wdss);
            var msgtxt = "午餐时间到了！让我们一起享受美食和愉快的时光吧！希望你的午后充满欢乐和满满的正能量！";
            // if (results.Count > 0)
            bot_sendMsgToMltV3("午餐商家推荐.gif", plchdTxt, wdss);


        }

        public static async void z_xiawucha()
        {
            var s = "奶茶 水果茶 水果";
            var msgtxt = "懂得享受下午茶时光。点一杯咖啡，点一杯奶茶 ，亦或自己静静思考，生活再忙碌，也要记得给自己喘口气";
            //   List<InlineKeyboardButton[]> results = qry_ByKwds_OrderbyRdm_Timermode_lmt5(s);

            //   if (results.Count > 0)
            bot_sendMsgToMltV2("下午茶商家推荐.gif", plchdTxt, s);




        }


        //public static List<InlineKeyboardButton[]> qry_ByKwds_OrderbyRdm_Timermode_lmt5(string wdss)
        //{
        //    var __METHOD__ = MethodBase.GetCurrentMethod().Name;
        //    dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), wdss));

        //    var arr = wdss.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray();
        //    var rdm = new Random().Next(1, arr.Length);

        //    string? keyword = arr[rdm - 1];
        //    dbgCls.setDbgVal(__METHOD__, "kwd", keyword);
        //    List<InlineKeyboardButton[]> results = qry_ByKwd_TmrMsgmode(keyword);
        //    List<InlineKeyboardButton[]> results22 = arrCls.rdmList<InlineKeyboardButton[]>(results);
        //    results22 = results22.Skip(0 * 10).Take(5).ToList();
        //    return results22;
        //}

        //new msg mode
        //public static List<InlineKeyboardButton[]> qry_ByKwd_TmrMsgmode(string keyword)
        //{
        //    var __METHOD__ = MethodBase.GetCurrentMethod().Name;
        //    dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), keyword));

        //    List<InlineKeyboardButton[]> results = [];

        //    if (string.IsNullOrEmpty(keyword))
        //        return [];

        //    keyword = keyword.ToLower().Replace(" ", "").Trim();
        //    var searchChars = keyword!.ToCharArray();
        //    HashSet<prj202405.City> _citys = getCitysObj();
        //    results = (from c in _citys
        //               from ca in c.Address
        //               from am in ca.Merchant
        //               where searchChars.All(s => (c.CityKeywords + ca.CityKeywords + am.KeywordString + am.KeywordString + Program._categoryKeyValue[(int)am.Category]).Contains(s))
        //               orderby am.Views descending
        //               select new[] { new InlineKeyboardButton(c.Name + " • " + ca.Name + " • " + am.Name) { CallbackData = $"Merchant?id={am.Guid}&timerMsgMode2025" } }).ToList();
        //    //count = results.Count;
        //    foreach (InlineKeyboardButton[] btn in results)
        //    {

        //    }



        //    return results;
        //}

        //public static List<InlineKeyboardButton[]> qryByMsgKwds(string msg)
        //{
        //    var segmenter = new JiebaSegmenter();
        //    segmenter.LoadUserDict("user_dict.txt");
        //    segmenter.AddWord("会所"); // 可添加一个新词
        //    var segments = segmenter.CutForSearch(msg); // 搜索引擎模式
        //   print("【搜索引擎模式】：{0}", string.Join("/ ", segments));


        //    List<InlineKeyboardButton[]> rows_rzt = [];
        //    foreach (string kwd in segments)
        //    {
        //        if (kwd.Length < 2)
        //            continue;
        //        var rows = qryByKwd(kwd);
        //       print("kwd=>" + kwd);
        //       print("qryByKwd(kwd) cnt=>" + rows.Count);
        //        rows_rzt = arrCls.MergeLists(rows_rzt, rows);

        //    }

        //    // ArrayList rzt = new ArrayList(rows_rzt);
        //    rows_rzt = (List<InlineKeyboardButton[]>)arrCls.dedulip4inlnKbdBtnArr(rows_rzt, "callback_data");
        //    return rows_rzt;
        //    //    List<InlineKeyboardButton[]> results22 = arrCls.rdmList<InlineKeyboardButton[]>(results);

        //    //  results22 = results22.Skip(0 * 10).Take(5).ToList();
        //}


        //dep
        public static List<InlineKeyboardButton[]> qryFrmShangjiaOrdbyViewDesc__DEP()
        {
            HashSet<prjx.City> _citys = getCitysObj();
            List<InlineKeyboardButton[]> results = [];
            results = (from c in _citys
                       from ca in c.Address
                       from am in ca.Merchant
                           //   where searchChars.All(s => (c.CityKeywords + ca.CityKeywords + am.KeywordString + am.KeywordString + Program._categoryKeyValue[(int)am.Category]).Contains(s))
                       orderby am.Views descending
                       select new[] { new InlineKeyboardButton(c.Name + " • " + ca.Name + " • " + am.Name) { CallbackData = $"Merchant?id={am.Guid}" } }).ToList();
            //count = results.Count;
            results = results.Skip(0 * 10).Take(5).ToList();


            return results;
        }


    }
}