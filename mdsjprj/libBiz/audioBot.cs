using prj202405.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

using prj202405.lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Concentus.Oggfile;
using Concentus.Structs;
using NAudio.Wave;
using NAudio.Lame;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using NAudio.Wave;
using NAudio.Lame;
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
using static mdsj.lib.avClas;
using static mdsj.lib.dtime;
using static mdsj.lib.fulltxtSrch;
using static prj202405.lib.tglib;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using libx;
using prj202405;
using Telegram.Bot.Types.ReplyMarkups;
using NReco.VideoConverter;
using Org.BouncyCastle.Crypto.IO;
using mdsj.lib;
using Concentus.Structs;
using RG3.PF.Abstractions.Entity;
using DocumentFormat.OpenXml.Office.Word;
namespace mdsj.libBiz
{
    internal class audioBot
    {
        public static string BotTokenAvBot = "6134198347:AAEdHZUkmYrpm0RHUrzZaKK9d11SiEIhSUk";

        public static TelegramBotClient botClient_Audio = new(token: BotTokenAvBot);


        internal static void main1()
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            print_call_FunArgs(__METHOD__, func_get_args());

            //  botClient_QunZzhushou.GetUpdatesAsync().Wait();

            //  botClient_QunZzhushou.o
            botClient_Audio.StartReceiving(
                updateHandler: OnUpdateHdl4av,
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

            //  StartSaveFotoAsync();
            print_ret(__METHOD__, 0);
        }


        private static async Task OnUpdateHdl4av(ITelegramBotClient client, Update update, CancellationToken token)
        {
            CallAsAsyncTaskRun(() =>
            {
                callxTryJmp(updateHdlSync, update);
                //   callxTryJmp(updateHdlSync, update);

            });


        }

   

        public static void msgHdlr嗨小爱童鞋搜索音乐(Update update)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            prjdir = filex.GetAbsolutePath(prjdir);
            string cmd = null;
            var songName = substr_GetTextAfterKeyword(update.Message.Text.Trim(), "搜索音乐").Trim();
            botClient_QunZzhushou.SendTextMessageAsync(update.Message.Chat.Id, "开始搜索音乐。。。" + songName + "因为要从互联网检索下载，可能需要长达好几分钟去处理，稍等。。", replyToMessageId: update.Message.MessageId);
            string downdir = prjdir + "/downmp3";
            string fname = filex.ConvertToValidFileName2024(songName);
            string mp3path = $"{downdir}/{fname}.mp3";
            Console.WriteLine(mp3path);
            if (!System.IO.File.Exists(mp3path))
                mp3path = DownloadSongAsMp3(songName, downdir);
            if (System.IO.File.Exists(mp3path))
                SendMp3ToGroupAsync(botClient_Audio, mp3path, update.Message.Chat.Id, update.Message.MessageId);
            dbgpad = 0;
            dbgCls.print_ret(__METHOD__, 0);
            jmp2end();
        }
        public static void msgHdlr嗨小爱童鞋(Update update)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            prjdir = filex.GetAbsolutePath(prjdir);

            string path = $"{prjdir}/cfg/所有命令.txt";
            string text = System.IO.File.ReadAllText(path);
            text = text.Replace("%前导提示词%", serchTipsWd);
            botClient_Audio.SendTextMessageAsync(update.Message.Chat.Id, text, replyToMessageId: update.Message.MessageId);
         //   dbgCls.print_ret(__METHOD__, 0);
        }
        private static void msgHdlr嗨小爱童鞋所有命令(Update update)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            print_call_FunArgs(__METHOD__, update);
            prjdir = filex.GetAbsolutePath(prjdir); string path = $"{prjdir}/cfg/所有命令.txt";
            string text = System.IO.File.ReadAllText(path);
            text = text.Replace("%前导提示词%", serchTipsWd);
            botClient_Audio.SendTextMessageAsync(update.Message.Chat.Id, text, replyToMessageId: update.Message.MessageId);
           // dbgCls.print_ret(__METHOD__, 0);
            jmp2end();
        }
        private const string serchTipsWd = "嗨小爱童鞋";
        private static void updateHdlSync(Update update)
        {
            string reqThreadId = geneReqid();

            bot_logRcvMsgV2(update, "msgRcvDir2025");



            //------hi xiaongai txye xxx 
            callTryAll(() =>
            {
                string[] a = splt(update?.Message?.Text);
                a = trimUper(a);
                var cmd = getElmt(a, 1);
                var hdlrname = "msgHdlr" + serchTipsWd + cmd;
                callx(hdlrname, update);
            });
          


            //if (update.Type == UpdateType.ChatMember && update.ChatMember.NewChatMember.Status == ChatMemberStatus.Member)
            //{
            //    var chatId = update.ChatMember.Chat.Id;
            //    var userId = update.ChatMember.NewChatMember.User.Id;

            //    await evt_newUserjoinSngle(chatId, userId, update.ChatMember.NewChatMember.User, update);
            //}
            //if (update.Message?.Type == Telegram.Bot.Types.Enums.MessageType.Voice)  // Adjust this condition based on your voting mechanism
            //{
            //    await SendThankYouMessage(update.Message.Chat.Id);
            //    return;
            //}

            //if (bot_getTxt(update).Trim().StartsWith(serchTipsWd))
            //{
            //    evt_嗨小爱同学Async(update, reqThreadId);
            //    return;
            //}
            if (update.Type == UpdateType.Message)
            {
                OnMsg(update, reqThreadId); return;
            }

            //if (update.Type == UpdateType.CallbackQuery)
            //{
            //    OnCallbk(update, reqThreadId); return;
            //}

            //if (update.Type == UpdateType.MyChatMember)
            //{
            //    OnChatMembr(update, reqThreadId); return;
            //}

            if (isFileMesg(update))
            {
                return;
            }



        }
      

      

        private static bool isFileMesg(Update update)
        {
            return false;

        }

        // private const string serchTipsWd = "嗨小爱童鞋";
        private static void evt_嗨小爱同学Async(Update update, string reqThreadId)
        {
            var __METHOD__ = "evt_嗨小爱同学Async";
            dbgCls.print_call_FunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), update, reqThreadId));
            string prjdir = @"../../../";
            if (update.Message.Text.Trim() == serchTipsWd)
            {


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
                dbgCls.print_ret(__METHOD__, 0);
                return;
            }

            if (cmd.Equals("搜索音乐") || cmd.Equals("搜索歌曲"))
            {


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
                dbgCls.print_ret(__METHOD__, 0);
                return;
            }

            //if (cmd.Equals("记账"))
            //{
            //    evt_cash记账(update);
            //    dbgCls.print_ret(__METHOD__, 0);
            //    return;
            //}


            //if (cmd.Equals("账单清单"))
            //{
            //    evt_cash账单清单账(update);
            //    dbgCls.print_ret(__METHOD__, 0);
            //    return;
            //}

            //if (cmd.Equals("删除"))
            //{
            //    evt_cash删除(update);
            //    dbgCls.print_ret(__METHOD__, 0);
            //    return;
            //}
            //if (cmd.Equals("账单统计"))
            //{
            //    evt_cashflowGrpby账单统计(update);
            //    dbgCls.print_ret(__METHOD__, 0);
            //    return;
            //}
        }


        // 
    }
}
