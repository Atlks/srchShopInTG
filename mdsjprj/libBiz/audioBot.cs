using prjx.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

using prjx.lib;
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
using static WindowsFormsApp1.libbiz.storeEngFunRefCls;
using static SqlParser.Ast.DataType;

using static SqlParser.Ast.CharacterLength;
using static mdsj.lib.avClas;
using static mdsj.lib.dtime;
using static mdsj.lib.fulltxtSrch;
using static prjx.lib.tglib;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using libx;
using prjx;
using Telegram.Bot.Types.ReplyMarkups;
using NReco.VideoConverter;
using Org.BouncyCastle.Crypto.IO;
using mdsj.lib;
using Concentus.Structs;
using RG3.PF.Abstractions.Entity;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Wordprocessing;
namespace mdsj.libBiz
{
    internal class audioBot
    {
        public static string BotTokenAvBot = "6134198347:AAEdHZUkmYrpm0RHUrzZaKK9d11SiEIhSUk";

        public static TelegramBotClient botClient_Audio = new(token: BotTokenAvBot);


        internal static void main1()
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            PrintCallFunArgs(__METHOD__, func_get_args());

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
            PrintRet(__METHOD__, 0);
        }


        private static async System.Threading.Tasks.Task OnUpdateHdl4av(ITelegramBotClient client, Update update, CancellationToken token)
        {
            CallAsAsyncTaskRun(() =>
            {
                CallxTryJmp(updateHdlSync, update);
                //   callxTryJmp(updateHdlSync, update);

            });


        }

   

        public static void msgHdlr嗨小爱童鞋搜索音乐(Update update)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            prjdir = filex.GetAbsolutePath(prjdir);
            string cmd = null;
            var songName = SubstrGetTextAfterKeyword(update.Message.Text.Trim(), "搜索音乐").Trim();
            botClient_QunZzhushou.SendTextMessageAsync(update.Message.Chat.Id, "开始搜索音乐。。。" + songName + "因为要从互联网检索下载，可能需要长达好几分钟去处理，稍等。。", replyToMessageId: update.Message.MessageId);
            string downdir = prjdir + "/downmp3";
            string fname = ConvertToValidFileName2024(songName);
            string mp3path = $"{downdir}/{fname}.mp3";
           Print(mp3path);
            if (!System.IO.File.Exists(mp3path))
                mp3path = DownloadSongAsMp3(songName, downdir);
            if (System.IO.File.Exists(mp3path))
                SendMp3ToGroupAsync(botClient_Audio, mp3path, update.Message.Chat.Id, update.Message.MessageId);
            dbgpad = 0;
            dbgCls.PrintRet(__METHOD__, 0);
            Jmp2endDep();
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
            PrintCallFunArgs(__METHOD__, update);
            prjdir = filex.GetAbsolutePath(prjdir); string path = $"{prjdir}/cfg/所有命令.txt";
            string text = System.IO.File.ReadAllText(path);
            text = text.Replace("%前导提示词%", serchTipsWd);
            botClient_Audio.SendTextMessageAsync(update.Message.Chat.Id, text, replyToMessageId: update.Message.MessageId);
           // dbgCls.print_ret(__METHOD__, 0);
            Jmp2endDep();
        }
        private const string serchTipsWd = "嗨小爱童鞋";
        private static   void updateHdlSync(Update update)
        {
            string reqThreadId = geneReqid();

            bot_logRcvMsgV2(update, "msgRcvDir2025");



            //------hi xiaongai txye xxx 
            callTryAll(() =>
            {
                if (string.IsNullOrEmpty(update?.Message?.Text))
                    return;
                string[] a = Splt(update?.Message?.Text);
                a = TrimUper(a);
                var preTrigwd= GetElmt(a, 0);
                if (preTrigwd != serchTipsWd)
                    return;
                var cmd = GetElmt(a, 1);
                var hdlrname = "msgHdlr" + serchTipsWd + cmd;
                //todo also should calltryx
                Callx(hdlrname, update);
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
              //  OnMsg(update, reqThreadId); return;
            }


            if (update.Message?.Type == MessageType.Voice)
            {
                  Bot_OnVoiceAsync(update, reqThreadId).GetAwaiter().GetResult();
                return;
            }

            if (update.Message?.Type == MessageType.VideoNote)
            {
                Bot_OnVideoNoteAsync(update, reqThreadId);
                return;
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
        private static async System.Threading.Tasks.Task Bot_OnVideoNoteAsync(Update update, string reqThreadId)
        {
            var __METHOD__ = "Bot_OnVideoNoteAsync";
            PrintCallFunArgs(__METHOD__, func_get_args(update, reqThreadId));

            var videoFileId = update.Message.VideoNote.FileId;
            var file = await botClient_Audio.GetFileAsync(videoFileId);
            var filePathInTg = file.FilePath;

            // 下载视频
            string basname = FilenameBydtme();
            //  string songname = $"{update.Message.Audio.FileName}";
            //  string fileName1 = InsertCurrentTimeToFileName($"{update.Message.Audio.FileName}");//file_name.mp3
            string fileName1 = $"{basname}.mp4";
            string saveDirectory = "filedt/VideoNoteDir";
            string fullfilepath = $"{saveDirectory}/{fileName1}";
            Mkdir4File(fullfilepath);
            var videoFilePath = await DownloadFile2localThruTgApiV2(filePathInTg, fullfilepath, BotTokenAvBot);
            Print($"{videoFilePath}");


            saveDirectory = "metadt/VideoNoteDirMeta";
            SortedList sortedList = ConvertToSortedList(update.Message.VideoNote);
            sortedList.Add("filenameLoc", fileName1);
            ormJSonFL.SaveJson(sortedList, $"{saveDirectory}/{basname}.json");

            InputFileStream inputOnlineFile = ToFilStrm(videoFilePath);
            await botClient_QunZzhushou.SendVideoAsync(caption: "转码结果", chatId: update.Message.Chat.Id, video: inputOnlineFile, replyToMessageId: update.Message.MessageId);

            PrintRet(__METHOD__, 0);
        }

      

        private static async System.Threading.Tasks.Task Bot_OnVoiceAsync(Update update, string reqThreadId)
        {
            var __METHOD__ = "Bot_OnVoiceAsync";
            PrintCallFunArgs(__METHOD__, func_get_args(update, reqThreadId));

            var videoFileId = update.Message.Voice.FileId;
            var file = await botClient_Audio.GetFileAsync(videoFileId);
            var filePathInTg = file.FilePath;

            // 下载视频
            string basname = FilenameBydtme();
            //  string songname = $"{update.Message.Audio.FileName}";
            //  string fileName1 = InsertCurrentTimeToFileName($"{update.Message.Audio.FileName}");//file_name.mp3
            string fileName1 = $"{basname}.ogg";
            string saveDirectory = "filedt/VoiceFile";
            string fullfilepath = $"{saveDirectory}/{fileName1}";
            Mkdir4File(fullfilepath);
            var videoFilePath = await DownloadFile2localThruTgApiV2(filePathInTg, fullfilepath, BotTokenAvBot);
            Print($"{videoFilePath}");

            string outputFilePathMp3 = fullfilepath + ".mp3";
            ConvertOggToMp3(fullfilepath, outputFilePathMp3);


            saveDirectory = "metadt/VoiceMtdt";
            SortedList sortedList = ConvertToSortedList(update.Message.Voice);
            sortedList.Add("filenameLoc", fileName1);
            ormJSonFL.SaveJson(sortedList, $"{saveDirectory}/{basname}.json");

            var mp3Stream = System.IO.File.Open(outputFilePathMp3, FileMode.Open);
            var inputOnlineFile = InputFile.FromStream(mp3Stream);

            await botClient_QunZzhushou.SendAudioAsync(caption: "转码结果", title: "录音", chatId: update.Message.Chat.Id, audio: inputOnlineFile, replyToMessageId: update.Message.MessageId);


            PrintRet(__METHOD__, 0);
        }



        private static bool isFileMesg(Update update)
        {
            return false;

        }

        // private const string serchTipsWd = "嗨小爱童鞋";
        private static void evt_嗨小爱同学Async(Update update, string reqThreadId)
        {
            var __METHOD__ = "evt_嗨小爱同学Async";
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), update, reqThreadId));
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
                dbgCls.PrintRet(__METHOD__, 0);
                return;
            }

            if (cmd.Equals("搜索音乐") || cmd.Equals("搜索歌曲"))
            {


                return;
            }

            if (cmd.Equals("搜索聊天记录"))
            {

                prjdir = filex.GetAbsolutePath(prjdir);
                var kwds = SubstrGetTextAfterKeyword(update.Message.Text.Trim(), cmd).Trim();

                List<SortedList> li = qry_ContainMatch("fullTxtSrchIdxdataDir", kwds);
                // 使用 LINQ 查询语法提取 txt 属性值
                var txtValues = li.Select(dict =>
                {
                    return LoadFieldFrmStlst(dict, "txt", null);
                })
                                    .Where(txt => txt != null)
                                    .ToArray();
                botClient_QunZzhushou.SendTextMessageAsync(update.Message.Chat.Id, json_encode(txtValues), replyToMessageId: update.Message.MessageId);
                dbgCls.PrintRet(__METHOD__, 0);
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
