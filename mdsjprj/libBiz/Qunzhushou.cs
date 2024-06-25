using prj202405.lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
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
using static WindowsFormsApp1.libbiz.storeEngFunRefCls;
using static SqlParser.Ast.DataType;

using static SqlParser.Ast.CharacterLength;
using static mdsj.lib.music;
using static mdsj.lib.dtime;
using static mdsj.lib.fulltxtSrch;
using static prj202405.lib.tglib;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using libx;
using prj202405;
using Telegram.Bot.Types.ReplyMarkups;
namespace mdsj.libBiz
{
    internal class Qunzhushou
    {
        public static TelegramBotClient botClient_QunZzhushou = new("6312276245:AAF35O3l6TxL0S3UixYuFAec-grd9j0kbog");

        internal static void main1()
        {
            //  botClient_QunZzhushou.o
            botClient_QunZzhushou.StartReceiving(
                updateHandler: OnUpdateHdl,
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
        }

        private static async Task OnUpdateHdl(ITelegramBotClient client, Update update, CancellationToken token)
        {
            string reqThreadId = geneReqid();
            if (update.Type == UpdateType.Message)
            {
                OnMsg(update, reqThreadId);
            }

            if (update.Type == UpdateType.CallbackQuery)
            {
                OnCallbk(update, reqThreadId);
            }

            if (update.Type == UpdateType.MyChatMember)
            {
                OnChatMembr(update, reqThreadId);
            }
        }

        private static void OnChatMembr(Update update, string reqThreadId)
        {
            // throw new NotImplementedException();
        }

        private static void OnCallbk(Update update, string reqThreadId)
        {
            // throw new NotImplementedException();
        }

        private static void OnMsg(Update update, string reqThreadId)
        {
            string DataDir = "fullTxtSrchIdxdataDir";
            wrt_rows4fulltxt(json_encode(update), DataDir);
            if (update.Message.Text.Trim().StartsWith(serchTipsWd))
            {
                evt_嗨小爱同学Async(update, reqThreadId);
                return;
            }
        }

        private const string serchTipsWd = "嗨小爱童鞋";
        private static async Task evt_嗨小爱同学Async(Update update, string reqThreadId)
        {
            var __METHOD__ = "evt_嗨小爱同学Async";
            dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), update, reqThreadId));
            string prjdir = @"../../../";
            if (update.Message.Text.Trim() == serchTipsWd)
            {

                prjdir = filex.GetAbsolutePath(prjdir);

                string path = $"{prjdir}/cfg/所有命令.txt";
                string text = System.IO.File.ReadAllText(path);
                text = text.Replace("%前导提示词%", serchTipsWd);
                botClient_QunZzhushou.SendTextMessageAsync(update.Message.Chat.Id, text, replyToMessageId: update.Message.MessageId);
                dbgCls.setDbgValRtval(__METHOD__, 0);
                return;
            }
            string[] a = update.Message.Text.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var cmd = a[1].Trim().ToUpper();
            if (cmd.Equals("所有命令"))
            {

                prjdir = filex.GetAbsolutePath(prjdir); string path = $"{prjdir}/cfg/所有命令.txt";
                string text = System.IO.File.ReadAllText(path);
                text = text.Replace("%前导提示词%", serchTipsWd);
                botClient_QunZzhushou.SendTextMessageAsync(update.Message.Chat.Id, text, replyToMessageId: update.Message.MessageId);
                dbgCls.setDbgValRtval(__METHOD__, 0);
                return;
            }

            if (cmd.Equals("搜索音乐") || cmd.Equals("搜索歌曲"))
            {

                prjdir = filex.GetAbsolutePath(prjdir);
                var songName = substr_GetTextAfterKeyword(update.Message.Text.Trim(), cmd).Trim();
                botClient_QunZzhushou.SendTextMessageAsync(update.Message.Chat.Id, "开始搜索音乐。。。" + songName + "因为要从互联网检索下载，可能需要长达好几分钟去处理，稍等。。", replyToMessageId: update.Message.MessageId);
                string downdir = prjdir + "/downmp3";
                string mp3path = $"{downdir}/{songName}.mp3";
                Console.WriteLine(mp3path);
                if (!System.IO.File.Exists(mp3path))
                    await DownloadSongAsMp3(songName, downdir);
                SendMp3ToGroupAsync(mp3path, update.Message.Chat.Id, update.Message.MessageId);
                dbgpad = 0;
                dbgCls.setDbgValRtval(__METHOD__, 0);
                return;
            }

            if (cmd.Equals("搜索聊天记录"))
            {

                prjdir = filex.GetAbsolutePath(prjdir);
                var kwds = substr_GetTextAfterKeyword(update.Message.Text.Trim(), cmd).Trim();

                List<SortedList> li = qry_ContainMatch("fullTxtSrchIdxdataDir", kwds);
                // 使用 LINQ 查询语法提取 txt 属性值
                var txtValues = li.Select(dict =>
                {
                    return ldfld(dict, "txt", null);
                })
                                    .Where(txt => txt != null)
                                    .ToArray();
                botClient_QunZzhushou.SendTextMessageAsync(update.Message.Chat.Id, json_encode(txtValues), replyToMessageId: update.Message.MessageId);
                dbgCls.setDbgValRtval(__METHOD__, 0);
                return;
            }

            if (cmd.Equals("记账"))
            {
                evt_记账(update);
                dbgCls.setDbgValRtval(__METHOD__, 0);
                return;
            }


            if (cmd.Equals("账单清单"))
            {
                evt_账单清单账(update);
                dbgCls.setDbgValRtval(__METHOD__, 0);
                return;
            }

            if (cmd.Equals("删除"))
            {
                evt_删除(update);
                dbgCls.setDbgValRtval(__METHOD__, 0);
                return;
            }

        }

        private static void evt_删除(Update update)
        {
            string[] a = update.Message.Text.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var cmd = ldElmt(a, 1);

            var id = ldElmt(a, 2);


            long uid = update.Message.From.Id;
            ormJSonFL.del(id,$"blshtDir/blsht{uid}.json");


            botClient_QunZzhushou.SendTextMessageAsync(update.Message.Chat.Id, "删除ok", replyToMessageId: update.Message.MessageId);

        }

        private static void evt_账单清单账(Update update)
        {
            string[] a = update.Message.Text.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var cmd = ldElmt(a, 1);

            var month = ldElmt(a, 2);


            //   Func<SortedList, bool> whereFun = ;


            List<string> li = Qe_qryV2<string>("blshtDir", null, 
                (SortedList row) =>
                {
                    if (row["month"].ToString().Equals(month))
                        return true;
                    return false;
                }, null,


                (SortedList row) =>
                {
                    return $"{row["date"]} {row["cate"]} {row["amt"]} ";
                }
                , rnd4jsonFlRf());

            string msg=string.Join("\n", li);
            if (msg.Trim() == "")
                msg = "@没有结果为空";

            botClient_QunZzhushou.SendTextMessageAsync(update.Message.Chat.Id, msg, replyToMessageId: update.Message.MessageId);



        }

        private static void evt_记账(Update update)
        {
            string[] a = update.Message.Text.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var cmd = ldElmt(a, 1);

            var date = ldElmt(a, 2);
            var amt = toNumber(ldElmt(a, 3));
            var cate = ldElmt(a, 4);
            var demo = substr_AfterMarker(update.Message.Text.Trim(), cate);
            SortedList map = new SortedList();
            map.Add("date", date);
            map.Add("amt", amt);
            map.Add("month", DateTime.Now.Year + left(date, 2));
            map.Add("cate", cate);
            map.Add("demo", demo);
            string recID = $"{date}{cate}{new Random().Next()}";
            map.Add("id", recID);

            long uid = update.Message.From.Id;
            ormJSonFL.save(map, $"blshtDir/blsht{uid}.json");
            botClient_QunZzhushou.SendTextMessageAsync(update.Message.Chat.Id, "ok..\n" + recID, replyToMessageId: update.Message.MessageId);


        }

        private static double toNumber(string str)
        {

            if (string.IsNullOrWhiteSpace(str))
            {
                Console.WriteLine("Input string cannot be null or whitespace.");
                return 0;
                //    throw new ArgumentNullException(nameof(str), "Input string cannot be null or whitespace.");
            }

            if (double.TryParse(str, out double result))
            {
                return result;
            }
            else
            {
                Console.WriteLine("Input string is not in the correct format for a double.");
                //  throw new FormatException("Input string is not in the correct format for a double.");
                return 0;
            }

        }

        public static string ldElmt(string[] array, int index)
        {
            if (index < 0 || index >= array.Length)
            {
                return "";
            }

            string element = array[index];

            return element?.Trim().ToUpper();
            //   return   array[index].Trim().ToUpper();
        }
    }
}
