global using static mdsj.libBiz.cmdHdlr;
using DocumentFormat.OpenXml;
using mdsj.lib;
using MusicApiCollection.Sites.MusicBrainz.Data;
using prjx.lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace mdsj.libBiz
{
    internal class cmdHdlr
    {
        private const string tipsSetOK = "设置ok,你已经设置服务区域，如需重新设置区域请点击 /clearArea";
        public static string tipsSelectArea = "请选择服务区域";
        public static string tipsAppendArea = "继续新增区域国家/城市/园区";
        public static string cancelAddArea = "取消新增区域";
        public static string noTrigSrchMsgs = $"{tipsSelectArea},{tipsAppendArea},{cancelAddArea}";
        public static void CmdHdlr设置园区(string cmdFulltxt, Update update, string reqThreadId)
        {
            ///设置园区 东风园区
          //  if (cmdFulltxt.StartsWith("/设置园区"))
            {
                var prm_park = SubstrAfterMarker(cmdFulltxt, "/设置园区");

                SetPark(prm_park, update);

            }

            botClient.SendTextMessageAsync(
                update.Message.Chat.Id,
                "设置ok",
                parseMode: ParseMode.Html,

                protectContent: false,
                disableWebPagePreview: true,
                replyToMessageId: update.Message.MessageId);
            Jmp2end();

        }


        //public static void OnCmdPublic(string cmdFulltxt, Update update, string reqThreadId)
        //{
        //    string prjdir = @"../../../";
        //    prjdir = filex.GetAbsolutePath(prjdir);




          

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctry">泰国</param>
        /// <param name="update"></param>
        /// <exception cref="NotImplementedException"></exception>
        public static void ConfirmSetCountryBtnClick(string ctry, Update update)
        {
            //public 判断权限先
            var grpid = update.Message.Chat.Id;
            var fromUid = update.Message.From.Id;
            var mybotid = botClient.BotId;
            //   string f = $"botEnterGrpLog/{grpid}.{fromUid}.{util.botname}.json";
            string f = $"{prjdir}/db/botEnterGrpLog/inGrp{grpid}.u{fromUid}.addBot.{util.botname}.json";

            if (isGrpChat(update))
            {

                if (!IsAdmin(update))
                {
                    //send
                    Print("no auth ");
                    // print("no auth ");
                    botClient.SendTextMessageAsync(update.Message.Chat.Id,
                          "权限不足", replyToMessageId: update.Message.MessageId);
                    Jmp2end();
                }
            }
            if (isGrpChat(update))
            {
                string dbfile2 = $"{prjdir}/grpCfgDir/grpcfg{grpid}.json";
                SortedList cfg = findOne(dbfile2);
                string pk = ctry.Trim().ToUpper();

                string parks = GetParksByCountry(ctry);
                string newParks = AppendParks(cfg, parks);

                AppendArea(ctry, cfg);
                SetField(cfg, "园区", newParks);

                SetField(cfg, "id", grpid.ToString());
                SetField(cfg, "grpid", grpid.ToString());

                SetField(cfg, "grpinfo", update.Message.Chat);
                if (pk == "不限制")
                {
                    DelField(cfg, "国家", "");
                    DelField(cfg, "城市", "");
                    DelField(cfg, "园区", "");
                    SetField(cfg, "whereExprs", $"");
                }
                ormJSonFL.SaveJson(cfg, dbfile2);
            }
            //prive
            if (!isGrpChat(update))
            {
                string dbfile = $"{prjdir}/cfg_prvtChtPark/{update.Message.From.Id}.json";
                SortedList cfg = findOne(dbfile);
                string parks = GetParksByCountry(ctry);
                string newParks = AppendParks(cfg, parks);


                SetField(cfg, "园区", newParks);
             
                SetField(cfg, "id", update.Message.From.Id.ToString());
                SetField(cfg, "from", update.Message.From);
             //   SetField(cfg, "whereExprs", newParks);
                if (ctry == "不限制")
                {

                    DelField(cfg, "国家", "");
                    DelField(cfg, "城市", "");
                    DelField(cfg, "园区", "");
                    SetField(cfg, "whereExprs", $"");
                }
                ormJSonFL.SaveJson(cfg, dbfile);
            }
            botClient.SendTextMessageAsync(
              update.Message.Chat.Id,
              tipsSetOK,
              parseMode: ParseMode.Html,

              protectContent: false,
              disableWebPagePreview: true,
              replyToMessageId: update.Message.MessageId);

            //------------set menu btm
            SetBtmMenu(update);
            Jmp2end();
        }

        private static void AppendArea(string ctry, SortedList cfg)
        {
            string areaArr = GetFieldAsStr(cfg, "area地区");
       string     areaArr22 = AppendStrcomma(ctry, areaArr);
            SetField(cfg, "area地区", areaArr22);
        }

        public static string AppendStrcomma(string ctry, string areas)
        {
            if (areas == "")
                areas = ctry;
            else
                areas = areas + "," + ctry;


            return removeDulip(areas);
        }

        public static void ConfirmSetCityBtnClick(string area, Update update)
        {   //public 判断权限先
            var grpid = update.Message.Chat.Id;
            var fromUid = update.Message.From.Id;
            var mybotid = botClient.BotId;

            string f = $"{prjdir}/db/botEnterGrpLog/inGrp{grpid}.u{fromUid}.addBot.{util.botname}.json";

            if (isGrpChat(update))
            {

                if (!IsAdmin(update))
                {
                    //send
                    Print("no auth ");
                    // print("no auth ");
                    botClient.SendTextMessageAsync(update.Message.Chat.Id,
                          "权限不足", replyToMessageId: update.Message.MessageId);
                    Jmp2end();
                }
            }

            if (isGrpChat(update))
            {
                string dbfile2 = $"{prjdir}/grpCfgDir/grpcfg{grpid}.json";
                SortedList cfg = findOne(dbfile2);
                string pk = area.Trim().ToUpper();

                string whereExprsFld = "whereExprs";
                string areakey = "城市";
                string parks = GetParksByCity(area);
             string newParks=   AppendParks(cfg,  parks);
                AppendArea(area, cfg);

                DelField(cfg, "国家");
                DelField(cfg, "城市");
                SetField(cfg, "园区", newParks);
                SetField(cfg, "id", grpid.ToString());
                SetField(cfg, "grpid", grpid.ToString());            
                SetField(cfg, "grpinfo", update.Message.Chat);
                if (pk == "不限制")
                {
                    SetField(cfg, "国家", "");
                    SetField(cfg, "城市", "");
                    SetField(cfg, "园区", "");
                    SetField(cfg, "whereExprs", $"");
                }
                ormJSonFL.SaveJson(cfg, dbfile2);
            }
            //prive
            if (!isGrpChat(update))
            {
                string dbfile = $"{prjdir}/cfg_prvtChtPark/{update.Message.From.Id}.json";
                SortedList cfg = findOne(dbfile);
                string whereExprsFld = "whereExprs";
                string parks = GetParksByCity(area);
                string newParks = AppendParks(cfg, parks);
                DelField(cfg, "国家");
                DelField(cfg, "城市");
           //     SetField(cfg, "园区", whereExprsNew);
                SetField(cfg, "id", update.Message.From.Id.ToString());
                SetField(cfg, "from", update.Message.From);
            //    SetField(cfg, "whereExprs", $"园区={newParks}");
                if (area == "不限制")
                {
                    SetField(cfg, "城市", "");
                    SetField(cfg, "园区", "");
                    SetField(cfg, "whereExprs", $"");
                }
                ormJSonFL.SaveJson(cfg, dbfile);
            }
            botClient.SendTextMessageAsync(
              update.Message.Chat.Id,
              tipsSetOK,
              parseMode: ParseMode.Html,

              protectContent: false,
              disableWebPagePreview: true,
              replyToMessageId: update.Message.MessageId);

            //------------set menu btm
            SetBtmMenu(update); Jmp2end();
        }
   

  

  


        private static string AppendParks(SortedList cfg,   string parks
        )
        {
            string whereExprs = GetFieldAsStr(cfg, "whereExprs");
            SortedList whereExpmap = GetHashtableFromQrystr(whereExprs);
            DelField(whereExpmap, "国家");
            DelField(whereExpmap, "城市");

            string ormParks = GetFieldAsStr(whereExpmap, "园区");
            string newParks = ormParks + "," + parks;
            newParks = removeDulip(newParks);

            SetField(whereExpmap, "园区", newParks);
            DelField(cfg, "国家");
            DelField(cfg, "城市");

            string whereExprsNew = CastHashtableToQuerystringNoEncodeurl(whereExpmap);
            SetField(cfg, "whereExprs", whereExprsNew);
               return newParks;
        }

  

        //private static string SetMapWhereexprsFldOFkv(SortedList cfg, string whereExprsFld, string key2, string v2
        //    )
        //{
        //    string whereExprs = GetFieldAsStr(cfg, whereExprsFld);
        //    SortedList whereExpmap = GetHashtableFromQrystr(whereExprs);
        //    SetField(whereExpmap, "园区", v2);

        //    SetField(whereExpmap, "国家", "");
        //    SetField(whereExpmap, "城市", "");
           
        //    string whereExprsNew = CastHashtableToQuerystringNoEncodeurl(whereExpmap);
        //    return whereExprsNew;
        //}


        public static void SetParkBtnClick(string park, Update update)
        {
            //public 判断权限先
            var grpid = update.Message.Chat.Id;
            var fromUid = update.Message.From.Id;
            var mybotid = botClient.BotId;


            if (isGrpChat(update))
            {

                if (!IsAdmin(update))
                {
                    //send
                    Print("no auth ");
                    // print("no auth ");
                    botClient.SendTextMessageAsync(update.Message.Chat.Id,
                          "权限不足", replyToMessageId: update.Message.MessageId);
                    Jmp2end();
                }
            }
            //   string f = $"botEnterGrpLog/{grpid}.{fromUid}.{util.botname}.json";
            SetPark(park, update);


            botClient.SendTextMessageAsync(
              update.Message.Chat.Id,
               tipsSetOK,
              parseMode: ParseMode.Html,

              protectContent: false,
              disableWebPagePreview: true,
              replyToMessageId: update.Message.MessageId);

            //---------------set menu
            SetBtmMenu(update);

        }

        private static void SetBtmMenu(Update update)
        {
            ReplyKeyboardMarkup rplyKbdMkp;
            //  Program.botClient.send
            try
            {
                string chtType = update.Message.Chat.Type.ToString();
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
                var Photo2 = InputFile.FromStream(System.IO.File.OpenRead("今日促销商家.gif"));
                //  Message message2dbg = await 
                botClient.SendTextMessageAsync(
             update.Message.Chat.Id, plchdTxt,
                 parseMode: ParseMode.Html,
                replyMarkup: rplyKbdMkp,
                protectContent: false, disableWebPagePreview: true);
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

        public static void SetPark(string park, Update update)
        {
            park = CastToEnglishCharPunctuation(park);
            //public 判断权限先
            var grpid = update.Message.Chat.Id;
            var fromUid = update.Message.From.Id;
            var mybotid = botClient.BotId;
            //   string f = $"botEnterGrpLog/{grpid}.{fromUid}.{util.botname}.json";
            string f = $"{prjdir}/db/botEnterGrpLog/inGrp{grpid}.u{fromUid}.addBot.{util.botname}.json";
            if (isGrpChat(update))
            {
                if (!IsAdmin(update))
                {
                    Print("no auth " + f);
                    // print("no auth ");
                    botClient.SendTextMessageAsync(
             update.Message.Chat.Id,
             "权限不足",
             replyToMessageId: update.Message.MessageId);
                    Jmp2end();
                    return;
                }

                //auth chk ok ,,or  privete 

                //   string dbfile2 = $"{prjdir}/cfg_grp/{grpid}.json";
                string dbfile2 = $"{prjdir}/grpCfgDir/grpcfg{grpid}.json";
                SortedList cfg = findOne(dbfile2);

              //  string wehreExprs = GetFieldAsStr(cfg, "whereExprs");
              //  SortedList whereMap = GetHashtableFromQrystr(wehreExprs);
            //    string oriparks = GetFieldAsStr(whereMap, "园区");
                string newParks = AppendParks(cfg, park);
                string pk = park.Trim().ToUpper();
                AppendArea(pk, cfg);
                SetField(cfg, "园区", newParks);
                SetField(cfg, "id", grpid.ToString());
                SetField(cfg, "grpid", grpid.ToString());
                SetField(cfg, "grpinfo", update.Message.Chat);
                if (pk == "不限制")
                {
                    SetField(cfg, "园区", "");
                    SetField(cfg, "whereExprs", $"");
                }
                ormJSonFL.SaveJson(cfg, dbfile2);
            }

            if (!isGrpChat(update))
            {
                SetParkPrvt(park, update);
            }




        }

        private static void SetParkPrvt(string park, Update update)
        {
            string dbfile = $"{prjdir}/cfg_prvtChtPark/{update.Message.From.Id}.json";
            SortedList cfg = findOne(dbfile);
            string pk = park.Trim().ToUpper();
            string newParks = AppendParks(cfg, park);
        

            SetField(cfg, "园区", newParks);
            DelField(cfg, "国家");
       //     SetField(cfg, "whereExprs", $"园区={newParks}");
            DelField(cfg, "城市");
            SetField(cfg, "园区", pk);
            SetField(cfg, "id", update.Message.From.Id.ToString());
            SetField(cfg, "from", update.Message.From);
            if (pk == "不限制")
            {
                SetField(cfg, "园区", "");
                SetField(cfg, "whereExprs", $"");
            }
            ormJSonFL.SaveJson(cfg, dbfile);
        }

        
        
        //public static void On我是谁Supergroup(Update update, string reqThreadId)
        //{
        //    Print("我是打游戏");
        //}
        //public static void On设置城市supergroup(Update update, string reqThreadId)
        //{
        //    Print("oo617");
        //}

        public static void CmdHdlrclearArea(string fullcmd, Update update, string reqThreadId)
        {

            //public 判断权限先
            var grpid = update.Message.Chat.Id;
            var fromUid = update.Message.From.Id;
            var mybotid = botClient.BotId;

            string f = $"{prjdir}/db/botEnterGrpLog/inGrp{grpid}.u{fromUid}.addBot.{util.botname}.json";

            if (isGrpChat(update))
            {

                if (!IsAdmin(update))
                {
                    //send
                    Print("no auth ");
                    // print("no auth ");
                    botClient.SendTextMessageAsync(update.Message.Chat.Id,
                          "权限不足", replyToMessageId: update.Message.MessageId);
                    Jmp2end();
                }
            }

            if (isGrpChat(update))
            {
                string dbfile2 = $"{prjdir}/grpCfgDir/grpcfg{grpid}.json";
                SortedList cfg = findOne(dbfile2); 
                string whereExprsFld = "whereExprs";
                string areakey = "城市";

                DelField(cfg, "国家", "");
                DelField(cfg, "城市", "");


                SetField(cfg, "园区", "");
           
                SetField(cfg, "whereExprs", "");
                SetField(cfg, "id", grpid.ToString());
                SetField(cfg, "grpid", grpid.ToString());
                SetField(cfg, "grpinfo", update.Message.Chat);  
                ormJSonFL.SaveJson(cfg, dbfile2);
            }


            //prive
            if (!isGrpChat(update))
            {
                string dbfile = $"{prjdir}/cfg_prvtChtPark/{update.Message.From.Id}.json";
                SortedList cfg = findOne(dbfile);
                string whereExprsFld = "whereExprs";
               
                DelField(cfg, "国家");
                DelField(cfg, "城市");
                DelField(cfg, "园区");
                SetField(cfg, "id", update.Message.From.Id.ToString());
                SetField(cfg, "from", update.Message.From);
                SetField(cfg, "whereExprs", "");
               
                ormJSonFL.SaveJson(cfg, dbfile);
            }


            botClient.SendTextMessageAsync(
           update.Message.Chat.Id,
           "清理成功",
           parseMode: ParseMode.Html,

           protectContent: false,
           disableWebPagePreview: true,
           replyToMessageId: update.Message.MessageId);
            Jmp2end();
        }
            public static void CmdHdlrarea(string fullcmd,Update update, string reqThreadId)
        {

            if (isGrpChat(update))
            {

                if (!IsAdmin(update))
                {
                    //send
                    Print("no auth " );
                    // print("no auth ");
                    botClient.SendTextMessageAsync(update.Message.Chat.Id,
                          "权限不足", replyToMessageId: update.Message.MessageId);
                    Jmp2end();
                }
            }

            var tipsd = tipsSelectArea;

            if (IsSetArea(update))
            {
                if(isGrpChat(update))
                {
                    var grpid = update.Message.Chat.Id;
                    var fromUid = update.Message.From.Id;
                    var mybotid = botClient.BotId;
                    string dbfile2 = $"{prjdir}/grpCfgDir/grpcfg{grpid}.json";
                    SortedList cfg = findOne(dbfile2);
                 

                    string area = GetFieldAsStr(cfg, "area地区");
                    tipsd = $"你已经选择了地区：{area} 。";
                    tipsd += tipsAppendArea;
                }else

                tipsd = tipsAppendArea;
            } 

             

            string cmd = GetCmdFun(update?.Message?.Text);
            //  Print("oo617");
            string filePath = $"{prjdir}/cfg_cmd/{cmd}.txt";
            string[] lines = ReadFileAndRemoveEmptyLines(filePath);
            lines = Replace(lines, "{tipsd}", tipsAppendArea);
 

            KeyboardButton[][] btns = ConvertFileToKeyboardButtons(lines);
            Print(EncodeJson(btns));
            var rplyKbdMkp = new ReplyKeyboardMarkup(btns);
            string imgPath = "今日促销商家.gif";
            var Photo2 = InputFile.FromStream(System.IO.File.OpenRead(imgPath));
            //  Message message2dbg = await 
         
            var m = botClient.SendTextMessageAsync(
                            update.Message.Chat.Id, tipsd,
                            parseMode: ParseMode.Html,
                            replyMarkup: rplyKbdMkp,
                            protectContent: false, disableWebPagePreview: true).GetAwaiter().GetResult();

            Print(m);
            // PrintRet(nameof(CmdHdlrarea), "");
            Jmp2end();
        }

     
        public static bool IsSetArea(Update? update)
        {
            var grpid = update.Message.Chat.Id;
            var fromUid = update.Message.From.Id;
            var mybotid = botClient.BotId;
            string dbfile2 = $"{prjdir}/grpCfgDir/grpcfg{grpid}.json";
            SortedList cfg = findOne(dbfile2);
           

            string whereExprsFld = "whereExprs";
            string areakey = "城市";
            if(IsEmpty(GetFieldAsStr(cfg, whereExprsFld)))
                return false;
            return true;
                    
        //    string whereExprsNew = SetMapWhereexprsFldOFkv(cfg, whereExprsFld, areakey, area);

        }

      


     

        public static void OnCmdPrvt(string cmdFulltxt, Update update, string reqThreadId)
        {

            string prjdir = @"../../../";
            prjdir = filex.GetAbsolutePath(prjdir);
            string dbfile = $"{prjdir}/cfg_prvtChtPark/{update.Message.From.Id}.json";


            //私聊消息  /start开始
            if (cmdFulltxt == "/start")
            {
                CallUserFunc409(evt_startMsgEvtInPrvtAddBot, update);
                return;
            }

            ///设置园区 东风园区
            if (cmdFulltxt.StartsWith("/设置园区"))
            {
                var park = SubstrAfterMarker(cmdFulltxt, "/设置园区");

                SortedList cfg = findOne(dbfile);
                string pk = park.Trim().ToUpper();
                SetField(cfg, "园区", pk);
                SetField(cfg, "id", update.Message.From.Id.ToString());
                SetField(cfg, "from", update.Message.From);
                if (pk == "不限制")
                {
                    SetField(cfg, "园区", "");
                    SetField(cfg, "whereExprs", $"");
                }
                ormJSonFL.SaveJson(cfg, dbfile);



            }
            /////设置城市 妙瓦底
            //if (cmdFulltxt == "/设置城市")
            //{
            //    var park = SubstrAfterMarker(cmdFulltxt, "/设置城市");
            //    SortedList cfg = findOne(dbfile);
            //    SetField(cfg, "城市", park.Trim().ToUpper());
            //    SetField(cfg, "id", update.Message.From.Id.ToString());
            //    SetField(cfg, "from", update.Message.From);

            //    ormJSonFL.SaveJson(cfg, dbfile);

            //}

            botClient.SendTextMessageAsync(
              update.Message.Chat.Id,
              "设置ok",
              parseMode: ParseMode.Html,

              protectContent: false,
              disableWebPagePreview: true,
              replyToMessageId: update.Message.MessageId);

        }
        public static void evt_startMsgEvtInPrvtAddBot(Update update)
        {
            botClient.SendTextMessageAsync(
                    update.Message.Chat.Id,
                    "请直接搜索园区/城市+商家/菜单即可,比如”金三角 会所”!\n"
                    + "\n可以设置园区方便搜索，指令如下:\n"
                    + "/设置园区 东风园区\n",
                    //  + "/设置园区 不限制\n",
                    parseMode: ParseMode.Html,
                    //   replyMarkup: new InlineKeyboardMarkup([]),
                    protectContent: false,
                    disableWebPagePreview: true);

            tglib.bot_saveChtSesion(update.Message.Chat.Id, update.Message.From);

            //   tmrEvt_sendMsg4keepmenu("今日促销商家.gif", plchdTxt));

            var rplyKbdMkp = tgBiz.tg_btmBtns();
            KeyboardButton[][] kbtns = (KeyboardButton[][])rplyKbdMkp.Keyboard;
            RemoveButtonByName(kbtns, juliBencyon);

            //  var Photo2 = InputFile.FromStream(System.IO.File.OpenRead(imgPath));
            long chatid = update.Message.Chat.Id;
            //  Message message2dbg = await 
            botClient.SendTextMessageAsync(
         Convert.ToInt64(chatid), plchdTxt,
             parseMode: ParseMode.Html,
            replyMarkup: rplyKbdMkp,
            protectContent: false, disableWebPagePreview: true);
        }


    }
}
