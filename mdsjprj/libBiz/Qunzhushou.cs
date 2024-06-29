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
using static mdsj.lib.music;
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
namespace mdsj.libBiz
{
    internal class Qunzhushou
    {
        private const string BotToken = "6312276245:AAF35O3l6TxL0S3UixYuFAec-grd9j0kbog";
        public static TelegramBotClient botClient_QunZzhushou = new(token: BotToken);

        internal static void main1()
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            print_call(__METHOD__, func_get_args());

            //  botClient_QunZzhushou.GetUpdatesAsync().Wait();

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

            //  StartSaveFotoAsync();
            print_ret(__METHOD__, 0);
        }


        public static async Task StartSaveFotoAsync()
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            print_call(__METHOD__, func_get_args());
            var bot = botClient_QunZzhushou;
            string saveDirectory = "savePicDir";
            mkdir(saveDirectory);
            var offset = 0;
            while (true)
            {
                Console.WriteLine(dtime.datetime());
                Thread.Sleep(1000);
                var updates = await bot.GetUpdatesAsync(offset);

                foreach (var update in updates)
                {
                    if (update.Type == UpdateType.Message)
                    {
                        //message.Chat.Id.ToString() == groupId &&
                        var message = update.Message;
                        if (message.Type == MessageType.Photo)
                        {
                            try
                            {
                                var photo = message.Photo[message.Photo.Length - 1];
                                //   photo.
                                var fileId = photo.FileId;
                                var fileName = photo.FileUniqueId;


                                var filePath = System.IO.Path.Combine(saveDirectory, fileName);

                                using var fileStream = System.IO.File.OpenWrite(filePath);
                                await bot.DownloadFileAsync(fileId, fileStream);

                                Console.WriteLine($"Saved image: {fileName}");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }

                        }
                    }
                }

                offset = updates.Last().Id + 1;
            }

            print_ret(__METHOD__, 0);
        }

        private static async Task OnUpdateHdl(ITelegramBotClient client, Update update, CancellationToken token)
        {

            string reqThreadId = geneReqid();

            bot_logRcvMsgV2(update, "msgRcvDir2025");

            try
            {

                if (update.Message?.Type == MessageType.Voice)
                {
                    Bot_OnVoiceAsync(update, reqThreadId);
                    return;
                }
                if (update.Type == UpdateType.ChatMember && update.ChatMember.NewChatMember.Status == ChatMemberStatus.Member)
                {
                    var chatId = update.ChatMember.Chat.Id;
                    var userId = update.ChatMember.NewChatMember.User.Id;

                    await evt_newUserjoinSngle(chatId, userId, update.ChatMember.NewChatMember.User);
                }
                if (update.Message?.Type == Telegram.Bot.Types.Enums.MessageType.Voice)  // Adjust this condition based on your voting mechanism
                {
                    await SendThankYouMessage(update.Message.Chat.Id);
                    return;
                }
                if (update.Message?.Type == MessageType.Video)
                {
                    Bot_OnVideo(update, reqThreadId);
                    return;
                }

                if (update.Message?.Type == MessageType.Audio)
                {
                    Bot_OnAudioAsync(update, reqThreadId);
                    return;
                }

                //  MessageType.Animation
                //MessageType.contact
                //   MessageType.Voice

                if (update.Message?.Type == MessageType.Contact)
                {
                    Bot_OnContactAsync(update, reqThreadId);
                    return;
                }
                if (update.Message?.Type == MessageType.VideoNote)
                {
                    Bot_OnVideoNoteAsync(update, reqThreadId);
                    return;
                }

             

                if (update.Message?.Type == MessageType.Document)
                {
                    Bot_OnDocAsync(update, reqThreadId);
                    return;
                }
                if (update.Type == UpdateType.Message)
                {
                    OnMsg(update, reqThreadId); return;
                }

                if (update.Type == UpdateType.CallbackQuery)
                {
                    OnCallbk(update, reqThreadId); return;
                }

                if (update.Type == UpdateType.MyChatMember)
                {
                    OnChatMembr(update, reqThreadId); return;
                }

            }
            catch (
            Exception ex)
            {
                Console.WriteLine(ex);
            }



        }

        private static void Bot_OnContactAsync(Update update, string reqThreadId)
        {
            throw new NotImplementedException();
        }

        private static async Task Bot_OnVideoNoteAsync(Update update, string reqThreadId)
        {
            var __METHOD__ = "Bot_OnVideoNoteAsync";
            print_call(__METHOD__, func_get_args(update, reqThreadId));

            var videoFileId = update.Message.Voice.FileId;
            var file = await botClient_QunZzhushou.GetFileAsync(videoFileId);
            var filePathInTg = file.FilePath;

            // 下载视频
            string basname = filenameBydtme();
            //  string songname = $"{update.Message.Audio.FileName}";
            //  string fileName1 = InsertCurrentTimeToFileName($"{update.Message.Audio.FileName}");//file_name.mp3
            string fileName1 = $"{basname}.mp4";
            string saveDirectory = "saveVideoNoteDir";
            string fullfilepath = $"{saveDirectory}/{fileName1}";
            mkdir_forFile(fullfilepath);
            var videoFilePath = await DownloadFile2localThruTgApi(filePathInTg, fullfilepath);
            Console.WriteLine($"{videoFilePath}");


            saveDirectory = "saveVideoNoteDirMeta";
            SortedList sortedList = ConvertToSortedList(update.Message.Audio);
            sortedList.Add("filenameLoc", fileName1);
            ormJSonFL.save(sortedList, $"{saveDirectory}/{basname}.json");

            print_ret(__METHOD__, 0);
        }


        public static void ConvertOggToMp3(string inputFilePath, string outputFilePath)
        {
            if (string.IsNullOrEmpty(inputFilePath))
                throw new ArgumentException("Input file path cannot be null or empty", nameof(inputFilePath));

            if (string.IsNullOrEmpty(outputFilePath))
                throw new ArgumentException("Output file path cannot be null or empty", nameof(outputFilePath));

            // Ensure the input file exists
            if (!System.IO.File.Exists(inputFilePath))
                throw new FileNotFoundException("Input file not found", inputFilePath);

            try
            {
                // Initialize the Opus decoder
                OpusDecoder decoder = new OpusDecoder(48000, 2);

                // Open the Ogg file
                 FileStream oggFile = new FileStream(inputFilePath, FileMode.Open);
                 OpusOggReadStream oggStream = new OpusOggReadStream(decoder, oggFile);
                using (WaveFileWriter waveWriter = new WaveFileWriter(System.IO.Path.ChangeExtension(outputFilePath, ".wav"), new WaveFormat(48000, 16, 2)))
                {
                    // Read the Ogg file and write to a WAV file
                    while (oggStream.HasNextPacket)
                    {
                        short[] packet = oggStream.DecodeNextPacket();
                        if (packet != null)
                        {
                            byte[] buffer = new byte[packet.Length * sizeof(short)];
                            Buffer.BlockCopy(packet, 0, buffer, 0, buffer.Length);
                            waveWriter.Write(buffer, 0, buffer.Length);
                        }
                    }
                }

                // Convert WAV file to MP3
                using (var reader = new AudioFileReader(System.IO.Path.ChangeExtension(outputFilePath, ".wav")))
                using (var mp3Writer = new LameMP3FileWriter(outputFilePath, reader.WaveFormat, LAMEPreset.VBR_90))
                {
                    reader.CopyTo(mp3Writer);
                }

                // Delete the intermediate WAV file
             //   File.Delete(System.IO.Path.ChangeExtension(outputFilePath, ".wav"));

                Console.WriteLine($"Successfully converted {inputFilePath} to {outputFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during conversion: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }
        public static void ConvertOggToMp3_dep(string inputFilePath, string outputFilePath)
        {
            var __METHOD__ = "ConvertOggToMp3";
            print_call(__METHOD__, func_get_args(inputFilePath, outputFilePath));
            if (string.IsNullOrEmpty(inputFilePath))
                throw new ArgumentException("Input file path cannot be null or empty", nameof(inputFilePath));

            if (string.IsNullOrEmpty(outputFilePath))
                throw new ArgumentException("Output file path cannot be null or empty", nameof(outputFilePath));

            // Ensure the input file exists
            if (!System.IO.File.Exists(inputFilePath))
                throw new FileNotFoundException("Input file not found", inputFilePath);
            try
            {
                inputFilePath=GetAbsolutePath(inputFilePath);
                using (var vorbis = new NAudio.Vorbis.VorbisWaveReader(inputFilePath))
                using (var mp3FileWriter = new LameMP3FileWriter(outputFilePath, vorbis.WaveFormat, LAMEPreset.VBR_90))
                {
                    vorbis.CopyTo(mp3FileWriter);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
          
        }
        private static async Task Bot_OnVoiceAsync(Update update, string reqThreadId)
        {
            var __METHOD__ = "Bot_OnVoiceAsync";
            print_call(__METHOD__, func_get_args(update, reqThreadId));

            var videoFileId = update.Message.Voice.FileId;
            var file = await botClient_QunZzhushou.GetFileAsync(videoFileId);
            var filePathInTg = file.FilePath;

            // 下载视频
            string basname = filenameBydtme();
            //  string songname = $"{update.Message.Audio.FileName}";
            //  string fileName1 = InsertCurrentTimeToFileName($"{update.Message.Audio.FileName}");//file_name.mp3
            string fileName1 = $"{basname}.ogg";
            string saveDirectory = "saveVoiceDir";
            string fullfilepath = $"{saveDirectory}/{fileName1}";
            mkdir_forFile(fullfilepath);
            var videoFilePath = await DownloadFile2localThruTgApi(filePathInTg, fullfilepath);
            Console.WriteLine($"{videoFilePath}");

            string outputFilePathMp3 = fullfilepath + ".mp3";
            ConvertOggToMp3(fullfilepath, outputFilePathMp3);


            saveDirectory = "saveVoiceDirMeta";
            SortedList sortedList = ConvertToSortedList(update.Message.Voice);
            sortedList.Add("filenameLoc", fileName1);
            ormJSonFL.save(sortedList, $"{saveDirectory}/{basname}.json");

            var mp3Stream = System.IO.File.Open(outputFilePathMp3, FileMode.Open);
            var inputOnlineFile = InputFile.FromStream(mp3Stream);

            await botClient_QunZzhushou.SendAudioAsync(caption: "搜索结果", title: "录音", chatId: update.Message.Chat.Id, audio: inputOnlineFile, replyToMessageId: update.Message.MessageId);


            print_ret(__METHOD__, 0);
        }

        private static async Task Bot_OnDocAsync(Update update, string reqThreadId)
        {
            var __METHOD__ = "Bot_OnDoc";
            print_call(__METHOD__, func_get_args(update, reqThreadId));

            var videoFileId = update.Message.Audio.FileId;
            var file = await botClient_QunZzhushou.GetFileAsync(videoFileId);
            var filePathInTg = file.FilePath;

            // 下载视频
            string basname = filenameBydtme();
            string fnameOri = $"{update.Message.Audio.FileName}";
            string fileName1 = InsertCurrentTimeToFileName($"{update.Message.Audio.FileName}");//file_name.mp3

            string saveDirectory = "saveFileDir";
            string fullfilepath = $"{saveDirectory}/{fileName1}";
            mkdir_forFile(fullfilepath);
            var videoFilePath = await DownloadFile2localThruTgApi(filePathInTg, fullfilepath);
            Console.WriteLine($"{videoFilePath}");


            saveDirectory = "fileData";
            SortedList sortedList = ConvertToSortedList(update.Message.Audio);
            sortedList.Add("filenameLoc", fileName1);
            ormJSonFL.save(sortedList, $"{saveDirectory}/{fnameOri}.json");

            print_ret(__METHOD__, 0);
        }

        private static async Task Bot_OnAudioAsync(Update update, string reqThreadId)
        {
            var __METHOD__ = "Bot_OnAudioAsync";
            print_call(__METHOD__, func_get_args(update, reqThreadId));

            var videoFileId = update.Message.Audio.FileId;
            var file = await botClient_QunZzhushou.GetFileAsync(videoFileId);
            var filePathInTg = file.FilePath;

            // 下载视频
            string basname = filenameBydtme();
            string songname = $"{update.Message.Audio.FileName}";
            string fileName1 = InsertCurrentTimeToFileName($"{update.Message.Audio.FileName}");//file_name.mp3

            string saveDirectory = "saveAudioDir";
            string fullfilepath = $"{saveDirectory}/{fileName1}";
            mkdir_forFile(fullfilepath);
            var videoFilePath = await DownloadFile2localThruTgApi(filePathInTg, fullfilepath);
            Console.WriteLine($"{videoFilePath}");


            saveDirectory = "saveAudioMetaDir";
            SortedList sortedList = ConvertToSortedList(update.Message.Audio);
            sortedList.Add("filenameLoc", fileName1);
            ormJSonFL.save(sortedList,$"musicData/{songname}.json");

            print_ret(__METHOD__, 0);
        }


        public static string InsertCurrentTimeToFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name cannot be null or empty", nameof(fileName));

            // 获取文件名和扩展名
            string nameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fileName);
            string extension = System.IO.Path.GetExtension(fileName);

            // 获取当前时间并格式化
            string formattedTime = DateTime.Now.ToString("yyMMdd_HHmmss_fff");

            // 构造新的文件名
            string newFileName = $"{nameWithoutExtension}_{formattedTime}{extension}";

            return newFileName;
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
           

            // 这是一个示例的异步任务
              Task.Run(() =>
            {
                string DataDir = "fullTxtSrchIdxdataDir";
                Thread.Sleep(7000);
                Console.WriteLine("-----------------------------fulltxt index create thred----------");
                wrt_rows4fulltxt(json_encode(update), DataDir);
                Console.WriteLine("----------------fulltxt index create thred---- finish....");
            });
   

            if (bot_getTxt(update).Trim().StartsWith(serchTipsWd))
            {
                evt_嗨小爱同学Async(update, reqThreadId);
                return;
            }

            if (update.Type == UpdateType.Message)
            {
                //message.Chat.Id.ToString() == groupId &&
                var message = update.Message;
                if (message.Type == MessageType.Photo)
                {
                    onFotoAsync(message);

                }
            }

        }

        private static async Task onFotoAsync(Message message)
        {
            try
            {
                var photo = message.Photo[message.Photo.Length - 1];
                //   photo.
                var fileId = photo.FileId;
                var fileName = photo.FileUniqueId;

                //  var videoFileId = update.Message.Video.FileId;
                var file = await botClient_QunZzhushou.GetFileAsync(fileId);
                var filePathInTg = file.FilePath;


                // 下载视频
                string basname = filenameBydtme();
                string fileName1 = $"savepic{basname}.jpg";
                string saveDirectory = "saveFotoDir";
                string fullfilepath = $"{saveDirectory}/{fileName1}";
                mkdir_forFile(fullfilepath);
                 //   var fullfilepath = await DownloadFile2localThruTgApi(filePathInTg, fullfilepath);

                //   var filePath = System.IO.Path.Combine(saveDirectory, fileName);
                using var fileStream = System.IO.File.OpenWrite(fullfilepath);
               await botClient_QunZzhushou.DownloadFileAsync(filePathInTg, fileStream);



                Console.WriteLine($"Saved image: {fullfilepath}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static async Task SendThankYouMessage(long chatId)
        {
            try
            {
                await botClient.SendTextMessageAsync(chatId, "感谢投票");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
        }

        private static async void Bot_OnVideo(Update update, string reqThreadId)
        {
            var __METHOD__ = "Bot_OnVideo";
            print_call(__METHOD__, func_get_args(update, reqThreadId));

            var videoFileId = update.Message.Video.FileId;
            var file = await botClient_QunZzhushou.GetFileAsync(videoFileId);
            var filePathInTg = file.FilePath;

            // 下载视频
            string basname = filenameBydtme();
            string fileName1 = $"tg3055video{basname}.mp4";
            string saveDirectory = "saveVideoDir";
            string fullfilepath = $"{saveDirectory}/{fileName1}";
            mkdir_forFile(fullfilepath);
            var videoFilePath = await DownloadFile2localThruTgApi(filePathInTg, fullfilepath);

            // 转换视频为 MP3
            var YYMMmp3FilePath = $"d:/newmp3/{basname}.mp3";
            ConvertVideoToMp3(videoFilePath, YYMMmp3FilePath);

            MusicMetadata MusicMetadata1 = await RecognizeMusic(YYMMmp3FilePath);
            string mp3title = MusicMetadata1.Title + "-" + MusicMetadata1.Artist;
            string mp3titleFname = filex.ConvertToValidFileName2024(mp3title);

            string finaMp3Fullpath = "d:/newmp3copy/" + mp3titleFname + ".mp3";
            Copy2024(YYMMmp3FilePath, finaMp3Fullpath);
            // 发送 MP3 文件回群
            // using (var fileStream = new FileStream(mp3FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {

                var mp3Stream = System.IO.File.Open(finaMp3Fullpath, FileMode.Open);
                var inputOnlineFile = InputFile.FromStream(mp3Stream);

                await botClient_QunZzhushou.SendAudioAsync(caption: "搜索结果", title: mp3titleFname, chatId: update.Message.Chat.Id, audio: inputOnlineFile, replyToMessageId: update.Message.MessageId);
            }

            // 删除临时文件
            ////  System.IO.File.Delete(videoFilePath);
            //   System.IO.File.Delete(mp3FilePath);
            print_ret(__METHOD__, 0);

        }
        public static void Copy2024(string sourceFilePath, string destination_newFileName)
        {
            filex.mkdir_forFile(destination_newFileName);

            // 构造目标文件的完整路径
            // string destinationFilePath = System.IO.Path.Combine(destinationFolderPath, newFileName);

            // 复制并重命名文件
            System.IO.File.Copy(sourceFilePath, destination_newFileName, true);
        }
        public static void CopyAndRenameFile(string sourceFilePath, string destinationFolderPath, string newFileName)
        {
            // 如果目标文件夹不存在，则创建它
            Directory.CreateDirectory(destinationFolderPath);

            // 构造目标文件的完整路径
            string destinationFilePath = System.IO.Path.Combine(destinationFolderPath, newFileName);

            // 复制并重命名文件
            System.IO.File.Copy(sourceFilePath, destinationFilePath, true);
        }

        public static void CopyFileToFolder(string sourceFilePath, string targetFolderPath)
        {
            // 检查目标文件夹是否存在，如果不存在则创建
            if (!Directory.Exists(targetFolderPath))
            {
                Directory.CreateDirectory(targetFolderPath);
                Console.WriteLine($"Created directory: {targetFolderPath}");
            }

            // 获取源文件名
            string fileName = System.IO.Path.GetFileName(sourceFilePath);

            // 构建目标文件路径
            string destinationFilePath = System.IO.Path.Combine(targetFolderPath, fileName);

            // 复制文件
            System.IO.File.Copy(sourceFilePath, destinationFilePath, overwrite: true);
        }
        private static async Task<string> DownloadFile2localThruTgApi(string filePath, string fileFullPath)
        {
            var __METHOD__ = "DownloadFile2localThruTgApi";
            print_call(__METHOD__, func_get_args( filePath, fileFullPath));

            var fileUrl = $"https://api.telegram.org/file/bot{BotToken}/{filePath}";
            //     var fileFullPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), fileName);

            using (var httpClient = new HttpClient())
            {
                // 设置超时时间为30秒
                httpClient.Timeout = TimeSpan.FromSeconds(300);
                var response = await httpClient.GetAsync(fileUrl);
                // 检查响应是否成功
                response.EnsureSuccessStatusCode();
                await using var fileStream = new FileStream(fileFullPath, FileMode.Create, FileAccess.Write, FileShare.None);
                await response.Content.CopyToAsync(fileStream);
            }

            return fileFullPath;
        }
        private static async Task<string> DownloadFileThruTgApi(string filePath, string fileName)
        {
            var fileUrl = $"https://api.telegram.org/file/bot{BotToken}/{filePath}";
            var fileFullPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), fileName);

            using (var httpClient = new HttpClient())
            {
                // 设置超时时间为30秒
                httpClient.Timeout = TimeSpan.FromSeconds(200);
                var response = await httpClient.GetAsync(fileUrl);
                // 检查响应是否成功
                response.EnsureSuccessStatusCode();
                await using var fileStream = new FileStream(fileFullPath, FileMode.Create, FileAccess.Write, FileShare.None);
                await response.Content.CopyToAsync(fileStream);
            }

            return fileFullPath;
        }

        private static void ConvertVideoToMp3(string videoFilePath, string mp3FilePath)
        {
            var __METHOD__ = "ConvertVideoToMp3";
            print_call(__METHOD__, func_get_args(videoFilePath, mp3FilePath));

            // var mp3FilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"{basename}.mp3");
            var ffMpeg = new FFMpegConverter();
            //   ffMpeg.ConvertMedia(videoFilePath, mp3FilePath, "mp3");
            // Convert video to MP3
            // Convert video to MP3
            ffMpeg.ConvertMedia(videoFilePath, mp3FilePath, "mp3");
            //return mp3FilePath;
            Console.WriteLine($"Conversion completed: {mp3FilePath}");
        }
        private const string serchTipsWd = "嗨小爱童鞋";
        private static async Task evt_嗨小爱同学Async(Update update, string reqThreadId)
        {
            var __METHOD__ = "evt_嗨小爱同学Async";
            dbgCls.print_call(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), update, reqThreadId));
            string prjdir = @"../../../";
            if (update.Message.Text.Trim() == serchTipsWd)
            {

                prjdir = filex.GetAbsolutePath(prjdir);

                string path = $"{prjdir}/cfg/所有命令.txt";
                string text = System.IO.File.ReadAllText(path);
                text = text.Replace("%前导提示词%", serchTipsWd);
                botClient_QunZzhushou.SendTextMessageAsync(update.Message.Chat.Id, text, replyToMessageId: update.Message.MessageId);
                dbgCls.print_ret(__METHOD__, 0);
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

                prjdir = filex.GetAbsolutePath(prjdir);
                var songName = substr_GetTextAfterKeyword(update.Message.Text.Trim(), cmd).Trim();
                botClient_QunZzhushou.SendTextMessageAsync(update.Message.Chat.Id, "开始搜索音乐。。。" + songName + "因为要从互联网检索下载，可能需要长达好几分钟去处理，稍等。。", replyToMessageId: update.Message.MessageId);
                string downdir = prjdir + "/downmp3";
                string fname = filex.ConvertToValidFileName2024(songName);
                string mp3path = $"{downdir}/{fname}.mp3";
                Console.WriteLine(mp3path);
                if (!System.IO.File.Exists(mp3path))
                    await DownloadSongAsMp3(songName, downdir);
                SendMp3ToGroupAsync(mp3path, update.Message.Chat.Id, update.Message.MessageId);
                dbgpad = 0;
                dbgCls.print_ret(__METHOD__, 0);
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

            if (cmd.Equals("记账"))
            {
                evt_记账(update);
                dbgCls.print_ret(__METHOD__, 0);
                return;
            }


            if (cmd.Equals("账单清单"))
            {
                evt_账单清单账(update);
                dbgCls.print_ret(__METHOD__, 0);
                return;
            }

            if (cmd.Equals("删除"))
            {
                evt_删除(update);
                dbgCls.print_ret(__METHOD__, 0);
                return;
            }
            if (cmd.Equals("账单统计"))
            {
                evt_cashflowGrpby账单统计(update);
                dbgCls.print_ret(__METHOD__, 0);
                return;
            }
        }

        private static void evt_cashflowGrpby账单统计(Update update)
        {
            string[] a = update.Message.Text.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var cmd = ldElmt(a, 1);

            var month = ldElmt(a, 2);
            long uid = update.Message.From.Id;

            List<SortedList> li = Qe_qryV2<SortedList>("blshtDir", "blsht" + uid.ToString() + ".json",

               (SortedList row) =>
               {
                   if (row["month"].ToString().Equals(month))
                       return true;
                   return false;
               }, null,
               (SortedList row) =>
               {
                   return row;
               }
               , rnd4jsonFlRf());

            var rzt = SummarizeByCategory(li);
            var msg = json_encode(rzt);
            botClient_QunZzhushou.SendTextMessageAsync(update.Message.Chat.Id, msg, replyToMessageId: update.Message.MessageId);

        }


        public static Dictionary<string, decimal> SummarizeByCategory(List<SortedList> dataList)
        {

            const string amt = "amt";
            const string cate = "cate";
            var categoryAmountMap = new Dictionary<string, decimal>();

            foreach (var data in dataList)
            {

                if (data.ContainsKey(cate) && data.ContainsKey(amt))
                {
                    string category = data[cate].ToString();

                    decimal amount = Convert.ToDecimal(data[amt]);

                    if (categoryAmountMap.ContainsKey(category))
                    {
                        categoryAmountMap[category] += amount;
                    }
                    else
                    {
                        categoryAmountMap[category] = amount;
                    }
                }
            }

            return categoryAmountMap;
        }

        private static void evt_删除(Update update)
        {
            string[] a = update.Message.Text.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var cmd = ldElmt(a, 1);

            var id = ldElmt(a, 2);


            long uid = update.Message.From.Id;
            ormJSonFL.del(id, $"blshtDir/blsht{uid}.json");


            botClient_QunZzhushou.SendTextMessageAsync(update.Message.Chat.Id, "删除ok", replyToMessageId: update.Message.MessageId);

        }

        private static void evt_账单清单账(Update update)
        {
            string[] a = update.Message.Text.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var cmd = ldElmt(a, 1);

            var month = ldElmt(a, 2);
            long uid = update.Message.From.Id;

            //   Func<SortedList, bool> whereFun = ;


            List<string> li = Qe_qryV2<string>("blshtDir", "blsht" + uid.ToString() + ".json",

                (SortedList row) =>
                {
                    if (row["month"].ToString().Equals(month))
                        return true;
                    return false;
                }, (SortedList row) =>
                {
                    return int.Parse(row["date"].ToString());
                },


                (SortedList row) =>
                {
                    return $"{row["date"]} {row["cate"]} {row["amt"]} {row["demo"]}";
                }
                , rnd4jsonFlRf());

            string msg = string.Join("\n", li);
            if (msg.Trim() == "")
                msg = "@没有结果为空";

            botClient_QunZzhushou.SendTextMessageAsync(update.Message.Chat.Id, msg, replyToMessageId: update.Message.MessageId);



        }

        private static void evt_记账(Update update)
        {

            long uid = update.Message.From.Id;
            string? text = update.Message.Text;

            string recID = logic_addCashflow(uid, text);
            botClient_QunZzhushou.SendTextMessageAsync(update.Message.Chat.Id, "ok..\n" + recID, replyToMessageId: update.Message.MessageId);


        }

        public static string logic_addCashflow(long uid, string? text)
        {
            string[] a = text.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var cmd = ldElmt(a, 1);

            var date = ldElmt(a, 2);
            var amt = toNumber(ldElmt(a, 3));
            var cate = ldElmt(a, 4);
            var demo = substr_AfterMarker(text.Trim(), cate);
            SortedList map = new SortedList();
            map.Add("date", date);
            map.Add("amt", amt);
            map.Add("month", DateTime.Now.Year + left(date, 2));
            map.Add("cate", cate);
            map.Add("demo", demo);
            string recID = $"{date}{cate}{new Random().Next()}";
            map.Add("id", recID);


            ormJSonFL.save(map, $"blshtDir/blsht{uid}.json");
            return recID;
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
