global using static mdsj.libBiz.cmdHdlr;
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

namespace mdsj.libBiz
{
    internal class cmdHdlr
    {
        public static void OnCmdPrvt(string cmdFulltxt, Update update, string reqThreadId)
        {

            string prjdir = @"../../../";
            prjdir = filex.GetAbsolutePath(prjdir);
            string dbfile = $"{prjdir}/cfg_prvtChtPark/{update.Message.From.Id}.json";


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

        private static void setFld(SortedList cfg, string f, object v)
        {
            if (cfg.ContainsKey(f))
                cfg.Remove(f);

            cfg.Add(f, v);
        }
    }
}
