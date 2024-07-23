global using static mdsj.libBiz.cmdHdlr;
using DocumentFormat.OpenXml;
using mdsj.lib;
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

        public static void OnCmdPublic(string cmdFulltxt, Update update, string reqThreadId)
        {
            string prjdir = @"../../../";
            prjdir = filex.GetAbsolutePath(prjdir);




            ///设置园区 东风园区
            if (cmdFulltxt.StartsWith("/设置园区"))
            {
                var park = SubstrAfterMarker(cmdFulltxt, "/设置园区");

                SetPark(park, update);

            }

            botClient.SendTextMessageAsync(
                update.Message.Chat.Id,
                "设置ok",
                parseMode: ParseMode.Html,

                protectContent: false,
                disableWebPagePreview: true,
                replyToMessageId: update.Message.MessageId);

        }

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
                if (!System.IO.File.Exists(f))
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
            }

            if (isGrpChat(update))
            {
                string dbfile2 = $"{prjdir}/grpCfgDir/grpcfg{grpid}.json";
                SortedList cfg = findOne(dbfile2);
                string pk = ctry.Trim().ToUpper();

                string whereExprsFld = "whereExprs";
                string coutryKey = "国家";
                string whereExprsNew = SetMapWhereexprsFldOFkv(cfg, whereExprsFld,  coutryKey, pk);

                SetFld(cfg, "国家", pk);
                SetFld(cfg, "城市", "");
                SetFld(cfg, "园区", "");
                SetFld(cfg, "id", grpid.ToString());
                SetFld(cfg, "grpid", grpid.ToString());
                SetFld(cfg, "whereExprs", $"国家={ctry}");
                SetFld(cfg, "grpinfo", update.Message.Chat);
                if (pk == "不限制")
                {
                    SetFld(cfg, "国家", "");
                    SetFld(cfg, "城市", "");
                    SetFld(cfg, "园区", "");
                    SetFld(cfg, "whereExprs", $"");
                }
                ormJSonFL.SaveJson(cfg, dbfile2);
            }
            //prive
            if (!isGrpChat(update))
            {
                string dbfile = $"{prjdir}/cfg_prvtChtPark/{update.Message.From.Id}.json";
                SortedList cfg = findOne(dbfile);
                string whereExprsFld = "whereExprs";
                string coutryKey = "国家";
                string whereExprsNew = SetMapWhereexprsFldOFkv(cfg, whereExprsFld, coutryKey, ctry);

                SetFld(cfg, "国家", ctry);
                SetFld(cfg, "城市", "");
                SetFld(cfg, "园区", "");
                SetFld(cfg, "id", update.Message.From.Id.ToString());
                SetFld(cfg, "from", update.Message.From);
                SetFld(cfg, "whereExprs", $"国家={ctry}");
                if (ctry == "不限制")
                {
                  
                    SetFld(cfg, "国家", "");
                    SetFld(cfg, "城市", "");
                    SetFld(cfg, "园区", "");
                    SetFld(cfg, "whereExprs", $"");
                }
                ormJSonFL.SaveJson(cfg, dbfile);
            }
            botClient.SendTextMessageAsync(
              update.Message.Chat.Id,
              "设置ok",
              parseMode: ParseMode.Html,

              protectContent: false,
              disableWebPagePreview: true,
              replyToMessageId: update.Message.MessageId);

            //------------set menu btm
            SetBtmMenu(update);
            Jmp2end();
        }

        public static void ConfirmSetCityBtnClick(string area, Update update)
        {   //public 判断权限先
            var grpid = update.Message.Chat.Id;
            var fromUid = update.Message.From.Id;
            var mybotid = botClient.BotId;

            string f = $"{prjdir}/db/botEnterGrpLog/inGrp{grpid}.u{fromUid}.addBot.{util.botname}.json";
            if (isGrpChat(update))
            {
                if (!System.IO.File.Exists(f))
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
            }

            if (isGrpChat(update))
            {
                string dbfile2 = $"{prjdir}/grpCfgDir/grpcfg{grpid}.json";
                SortedList cfg = findOne(dbfile2);
                string pk = area.Trim().ToUpper();

                string whereExprsFld = "whereExprs";
                string areakey = "城市";
                string whereExprsNew = SetMapWhereexprsFldOFkv(cfg, whereExprsFld, areakey, area);

               
                SetFld(cfg, "城市", area);
                SetFld(cfg, "园区", "");
                SetFld(cfg, "id", grpid.ToString());
                SetFld(cfg, "grpid", grpid.ToString());
                SetFld(cfg, "whereExprs", whereExprsNew);
                SetFld(cfg, "grpinfo", update.Message.Chat);
                if (pk == "不限制")
                {
                   
                    SetFld(cfg, "城市", "");
                    SetFld(cfg, "园区", "");
                    SetFld(cfg, "whereExprs", $"");
                }
                ormJSonFL.SaveJson(cfg, dbfile2);
            }
            //prive
            if (!isGrpChat(update))
            {
                string dbfile = $"{prjdir}/cfg_prvtChtPark/{update.Message.From.Id}.json";
                SortedList cfg = findOne(dbfile);
                string whereExprsFld = "whereExprs";
                string coutryKey = "城市";
                string whereExprsNew = SetMapWhereexprsFldOFkv(cfg, whereExprsFld, coutryKey, area);

                SetFld(cfg, "城市", area);
           
                SetFld(cfg, "园区", "");
                SetFld(cfg, "id", update.Message.From.Id.ToString());
                SetFld(cfg, "from", update.Message.From);
                SetFld(cfg, "whereExprs", whereExprsNew);
                if (area == "不限制")
                {                  
                    SetFld(cfg, "城市", "");
                    SetFld(cfg, "园区", "");
                    SetFld(cfg, "whereExprs", $"");
                }
                ormJSonFL.SaveJson(cfg, dbfile);
            }
            botClient.SendTextMessageAsync(
              update.Message.Chat.Id,
              "设置ok",
              parseMode: ParseMode.Html,

              protectContent: false,
              disableWebPagePreview: true,
              replyToMessageId: update.Message.MessageId);

            //------------set menu btm
            SetBtmMenu(update); Jmp2end();
        }

        private static string SetMapWhereexprsFldOFkv(SortedList cfg, string whereExprsFld, string key2, string v2
            )
        {
            string whereExprs = GetFieldAsStr(cfg, whereExprsFld);
            SortedList whereExpmap = GetHashtableFromQrystr(whereExprs);
            SetFld(whereExpmap, key2, v2);
            string whereExprsNew = CastHashtableToQuerystringNoEncodeurl(whereExpmap);
            return whereExprsNew;
        }

     
        public static void SetParkFrmMsg(string park, Update update)
        {
            //public 判断权限先
            var grpid = update.Message.Chat.Id;
            var fromUid = update.Message.From.Id;
            var mybotid = botClient.BotId;
            //   string f = $"botEnterGrpLog/{grpid}.{fromUid}.{util.botname}.json";
            SetPark(park, update);


            botClient.SendTextMessageAsync(
              update.Message.Chat.Id,
              "设置ok",
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
            //public 判断权限先
            var grpid = update.Message.Chat.Id;
            var fromUid = update.Message.From.Id;
            var mybotid = botClient.BotId;
            //   string f = $"botEnterGrpLog/{grpid}.{fromUid}.{util.botname}.json";
            string f = $"{prjdir}/db/botEnterGrpLog/inGrp{grpid}.u{fromUid}.addBot.{util.botname}.json";
            if (isGrpChat(update))
            {
                if (!System.IO.File.Exists(f))
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
                string pk = park.Trim().ToUpper();

                SetFld(cfg, "园区", pk);
                SetFld(cfg, "id", grpid.ToString());
                SetFld(cfg, "grpid", grpid.ToString());
                SetFld(cfg, "whereExprs", $"园区={pk}");
                SetFld(cfg, "grpinfo", update.Message.Chat);
                if (pk == "不限制")
                {
                    SetFld(cfg, "园区", "");
                    SetFld(cfg, "whereExprs", $"");
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
            SetFld(cfg, "园区", pk);
            SetFld(cfg, "id", update.Message.From.Id.ToString());
            SetFld(cfg, "from", update.Message.From);
            if (pk == "不限制")
            {
                SetFld(cfg, "园区", "");
                SetFld(cfg, "whereExprs", $"");
            }
            ormJSonFL.SaveJson(cfg, dbfile);
        }

        
        public static string getCmdFun(string? v)
        {
            if (string.IsNullOrEmpty(v)) return "";
            if (v.StartsWith("/"))
            {
                v = v.Replace("@" + botname, "");
                return v.ToString().Substring(1);
            }

            else
                return "";
        }
        public static void On我是谁Supergroup(Update update, string reqThreadId)
        {
            Print("我是打游戏");
        }
        public static void On设置城市supergroup(Update update, string reqThreadId)
        {
            Print("oo617");
        }
        public static void CmdHdlrarea(Update update, string reqThreadId)
        {
            string cmd = getCmdFun(update?.Message?.Text);
            //  Print("oo617");
            KeyboardButton[][] btns = ConvertFileToKeyboardButtons($"{prjdir}/cfg_cmd/{cmd}.txt");
            Print(EncodeJson(btns));
            var rplyKbdMkp = new ReplyKeyboardMarkup(btns);
            string imgPath = "今日促销商家.gif";
            var Photo2 = InputFile.FromStream(System.IO.File.OpenRead(imgPath));
            //  Message message2dbg = await 
            var m = botClient.SendTextMessageAsync(
                            update.Message.Chat.Id, "请选择本群所在区域",
                            parseMode: ParseMode.Html,
                            replyMarkup: rplyKbdMkp,
                            protectContent: false, disableWebPagePreview: true).GetAwaiter().GetResult();

            Print(m);
            // PrintRet(nameof(CmdHdlrarea), "");
            Jmp2end();
        }

        public static KeyboardButton[][] ConvertFileToKeyboardButtons(string filePath)
        {
            // 读取文件所有行
            string[] lines = ReadFileAndRemoveEmptyLines(filePath);

            // 初始化 KeyboardButton[][] 数组
            KeyboardButton[][] keyboardButtons = new KeyboardButton[lines.Length][];

            for (int i = 0; i < lines.Length; i++)
            {
                // 按空格分割每行
                string lin = lines[i];
                if (lin.Trim().Length == 0)
                    continue;
                string[] words = lin.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                // 将每个单词转换为 KeyboardButton
                KeyboardButton[] buttonsRow = new KeyboardButton[words.Length];
                for (int j = 0; j < words.Length; j++)
                {
                    buttonsRow[j] = new KeyboardButton(words[j]);
                }

                // 将 KeyboardButton[] 添加到 KeyboardButton[][]
                keyboardButtons[i] = buttonsRow;
            }

            return keyboardButtons;

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
                SetFld(cfg, "园区", pk);
                SetFld(cfg, "id", update.Message.From.Id.ToString());
                SetFld(cfg, "from", update.Message.From);
                if (pk == "不限制")
                {
                    SetFld(cfg, "园区", "");
                    SetFld(cfg, "whereExprs", $"");
                }
                ormJSonFL.SaveJson(cfg, dbfile);



            }
            ///设置城市 妙瓦底
            if (cmdFulltxt == "/设置城市")
            {
                var park = SubstrAfterMarker(cmdFulltxt, "/设置城市");
                SortedList cfg = findOne(dbfile);
                SetFld(cfg, "城市", park.Trim().ToUpper());
                SetFld(cfg, "id", update.Message.From.Id.ToString());
                SetFld(cfg, "from", update.Message.From);

                ormJSonFL.SaveJson(cfg, dbfile);

            }

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
