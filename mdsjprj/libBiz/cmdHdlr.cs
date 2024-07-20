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
                var park = substr_AfterMarker(cmdFulltxt, "/设置园区");

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

        public static void SetPark(string park, Update update)
        {
            //public 判断权限先
            var grpid = update.Message.Chat.Id;
            var fromUid = update.Message.From.Id;
            var mybotid = botClient.BotId;
            //   string f = $"botEnterGrpLog/{grpid}.{fromUid}.{util.botname}.json";
            string f = $"{prjdir}/db/botEnterGrpLog/inGrp{grpid}.u{fromUid}.addBot.{util.botname}.json";
            if (isGrpChat(update))
                if (!System.IO.File.Exists(f))
                {
                    Print("no auth " + f);
                    // print("no auth ");
                    botClient.SendTextMessageAsync(
             update.Message.Chat.Id,
             "权限不足",
             replyToMessageId: update.Message.MessageId);
                    return;
                }

            //auth chk ok ,,or  privete 

            //   string dbfile2 = $"{prjdir}/cfg_grp/{grpid}.json";
            string dbfile2 = $"{prjdir}/grpCfgDir/grpcfg{grpid}.json";
            SortedList cfg = findOne(dbfile2);
            string pk = park.Trim().ToUpper();

            setFld(cfg, "园区", pk);
            setFld(cfg, "id", grpid.ToString());
            setFld(cfg, "grpid", grpid.ToString());
            setFld(cfg, "whereExprs", $"园区={pk}");
            setFld(cfg, "grpinfo", update.Message.Chat);
            if (pk == "不限制")
            {
                setFld(cfg, "园区", "");
                setFld(cfg, "whereExprs", $"");
            }
            ormJSonFL.save(cfg, dbfile2);
        }

        public static string GetStr(string? v)
        {
            if (string.IsNullOrEmpty(v)) return "";

            return v;
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
            Print(encodeJson(btns));
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
        public static string[] ReadFileAndRemoveEmptyLines(string filePath)
        {
            // 读取文件所有行
            string[] lines = System.IO.File.ReadAllLines(filePath);

            // 使用 LINQ 过滤掉空行
            string[] nonEmptyLines = lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

            return nonEmptyLines;
        }

        public static void OnCmdPrvt(string cmdFulltxt, Update update, string reqThreadId)
        {

            string prjdir = @"../../../";
            prjdir = filex.GetAbsolutePath(prjdir);
            string dbfile = $"{prjdir}/cfg_prvtChtPark/{update.Message.From.Id}.json";


            //私聊消息  /start开始
            if (cmdFulltxt == "/start")
            {
                call_user_func(evt_startMsgEvtInPrvtAddBot, update);
                return;
            }

            ///设置园区 东风园区
            if (cmdFulltxt.StartsWith("/设置园区"))
            {
                var park = substr_AfterMarker(cmdFulltxt, "/设置园区");

                SortedList cfg = findOne(dbfile);
                string pk = park.Trim().ToUpper();
                setFld(cfg, "园区", pk);
                setFld(cfg, "id", update.Message.From.Id.ToString());
                setFld(cfg, "from", update.Message.From);
                if (pk == "不限制")
                {
                    setFld(cfg, "园区", "");
                    setFld(cfg, "whereExprs", $"");
                }
                ormJSonFL.save(cfg, dbfile);



            }
            ///设置城市 妙瓦底
            if (cmdFulltxt == "/设置城市")
            {
                var park = substr_AfterMarker(cmdFulltxt, "/设置城市");
                SortedList cfg = findOne(dbfile);
                setFld(cfg, "城市", park.Trim().ToUpper());
                setFld(cfg, "id", update.Message.From.Id.ToString());
                setFld(cfg, "from", update.Message.From);

                ormJSonFL.save(cfg, dbfile);

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
