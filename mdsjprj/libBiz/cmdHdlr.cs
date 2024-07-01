global using static mdsj.libBiz.cmdHdlr;
using DocumentFormat.OpenXml;
using prj202405.lib;
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
        public static void OnCmdPrvt(string cmdFulltxt, Update update, string reqThreadId)
        {

            string prjdir = @"../../../";
            prjdir = filex.GetAbsolutePath(prjdir);
            string dbfile = $"{prjdir}/cfg_prvtChtPark/{update.Message.From.Id}.json";


            //私聊消息  /start开始
            if (cmdFulltxt == "/start")
            {
               call_user_func(     evt_startMsgEvtInPrvtAddBot,update);
                return;
            }

            ///设置园区 东风园区
            if (cmdFulltxt.StartsWith("/设置园区"))
            {

                var park = substr_AfterMarker(cmdFulltxt, "/设置园区");
                SortedList cfg = findOne(dbfile);

                setFld(cfg, "园区", park.Trim().ToUpper());
                setFld(cfg, "id", update.Message.From.Id.ToString());
                setFld(cfg, "from", update.Message.From);


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
                    "请直接搜索园区/城市+商家/菜单即可,比如”金三角 会所”!",
                    parseMode: ParseMode.Html,
                    //   replyMarkup: new InlineKeyboardMarkup([]),
                    protectContent: false,
                    disableWebPagePreview: true);

            tglib.bot_saveChtSesion(update.Message.Chat.Id, update.Message.From);

            //   tmrEvt_sendMsg4keepmenu("今日促销商家.gif", plchdTxt));

            var rplyKbdMkp = tgBiz.tg_btmBtns();
            KeyboardButton[][] kbtns = (KeyboardButton[][])rplyKbdMkp.Keyboard;
            RemoveButtonByName(kbtns, "🔥助力本群");

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
