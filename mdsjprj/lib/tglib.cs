global using static prj202405.lib.tglib;
using prj202405;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Collections;
using static mdsj.biz_other;
using static mdsj.clrCls;
using static mdsj.mrcht;
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
using static prj202405.lib.db;
using static libx.qryEngrParser;
using static mdsj.libBiz.tgBiz;

using static libx.storeEngr4Nodesqlt;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using DocumentFormat.OpenXml;
using System.Reflection;
using mdsj;
using Newtonsoft.Json.Linq;
using mdsj.libBiz;
using mdsj.lib;
namespace prj202405.lib
{
    internal class tglib
    {
        /*
         * 
         * 401错误检测，可能token错误，检测token是ok的。
         https://api.telegram.org/bot6999501721:AAFNqa2YZ-lLZMfN8T2tYscKBi33noXhdJA/getMe
         
         */
        // sendmsg4timrtask
        public static void bot_sendMsgToMlt(string imgPath, string msgtxt, List<InlineKeyboardButton[]> results)
        {

            var __METHOD__ = "sendMsg";
            dbgCls.print_call_FunArgs(__METHOD__, dbgCls.func_get_args4async(imgPath, msgtxt, results));

            try
            {
                // var  = plchdTxt;
                // print(string.Format("{0}-{1}", de.Key, de.Value));
                var Photo = InputFile.FromStream(System.IO.File.OpenRead(imgPath));
                var chtsSess = JsonConvert.DeserializeObject<Hashtable>(System.IO.File.ReadAllText(timerCls.chatSessStrfile))!;
                //遍历方法三：遍历哈希表中的键值
                foreach (DictionaryEntry de in chtsSess)
                {
                    //if (Convert.ToInt64(de.Key) == Program.groupId)
                    //    continue;
                    var chatid = Convert.ToInt64(de.Key);
                   print(" SendPhotoAsync " + chatid);//  Program.botClient.send
                    sendFoto(imgPath, msgtxt, results, chatid);
                }
            }
            catch (Exception e)
            {
               print(e);
                logErr2024(e, __METHOD__, "errlog", (meth: __METHOD__, prm: func_get_args4async(imgPath, msgtxt, results)));

            }
            dbgCls.print_ret(__METHOD__, 0);

        }

        public static void  SendMp3ToGroupAsync(ITelegramBotClient botClient, string mp3FilePath, long ChatId, int messageId)
        {
            var __METHOD__ = "SendMp3ToGroupAsync";
            dbgCls.print_call_FunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), mp3FilePath, ChatId));

            try
            {
                //   var botClient = new TelegramBotClient(BotToken);

                using (var mp3Stream = System.IO.File.Open(mp3FilePath, FileMode.Open))
                {
                    //  var mp3InputFile = new InputOnlineMedia(mp3Stream, "file.mp3");
                    //  await using Stream stream = System.IO.File.OpenRead("/path/to/voice-nfl_commentary.ogg");
                    object value =   botClient.SendAudioAsync(ChatId, replyToMessageId: messageId, audio: InputFile.FromStream(mp3Stream), caption: "搜索结果", title: GetBaseFileName(mp3FilePath)).Result;
                }

               print("MP3 文件已发送到群组！");
            }
            catch (Exception ex)
            {
               print($"发送 MP3 文件时出错：{ex.Message}");
            }
        }


        public static List<InlineKeyboardButton[]> ConvertToInlineKeyboardButtons(List<object> objects)
        {
            List<InlineKeyboardButton[]> result = new List<InlineKeyboardButton[]>();

            foreach (var obj in objects)
            {
                // 假设 obj 是一个动态对象，并具有 Text 和 CallbackData 属性

                InlineKeyboardButton[] button = (InlineKeyboardButton[])obj;
                result.Add(button);

            }

            return result;
        }

        public static void SendThankYouMessage(long chatId)
        {
            try
            {
                  botClient_QunZzhushou.SendTextMessageAsync(chatId, "感谢投票");
            }
            catch (Exception ex)
            {
               print($"Error sending message: {ex.Message}");
            }
        }

        public static async Task<string> DownloadFile2localThruTgApi(string filePath, string fileFullPath)
        {
            var __METHOD__ = "DownloadFile2localThruTgApi";
            print_call_FunArgs(__METHOD__, func_get_args(filePath, fileFullPath));

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

        public static void bot_sendMsgToMltV2(string imgPath, string msgtxt, string wdss)
        {
            bot_sendMsgToMltV3(imgPath, msgtxt, wdss);
            //var __METHOD__ = "sendMsg";
            //dbgCls.print_call(__METHOD__, dbgCls.func_get_args4async(imgPath, msgtxt, wdss));

            //try
            //{
            //    // var  = plchdTxt;
            //    // print(string.Format("{0}-{1}", de.Key, de.Value));
            //    var Photo = InputFile.FromStream(System.IO.File.OpenRead(imgPath));
            //    var chtsSess = JsonConvert.DeserializeObject<Hashtable>(System.IO.File.ReadAllText(timerCls.chatSessStrfile))!;
            //    //遍历方法三：遍历哈希表中的键值
            //    foreach (DictionaryEntry de in chtsSess)
            //    {
            //        //if (Convert.ToInt64(de.Key) == Program.groupId)
            //        //    continue;
            //        var chatid = Convert.ToInt64(de.Key);
            //        try
            //        {
            //            //  if(chatid== -1002206103554)
            //            srchNsendFotoToGrp(imgPath, msgtxt, wdss, chatid);
            //        }
            //        catch (Exception e)
            //        {
            //           print(e);
            //        }
            //    }

            //}
            //catch (Exception e)
            //{
            //   print(e);
            //    logErr2024(e, __METHOD__, "errlog", (meth: __METHOD__, prm: func_get_args4async(imgPath, msgtxt, wdss)));

            //}
            //dbgCls.print_ret(__METHOD__, 0);
        }


        public static void bot_sendMsgToMltV3(string imgPath, string msgtxt, string wdss4srch)
        {

            var __METHOD__ = "sendMsg";
            dbgCls.print_call_FunArgs(__METHOD__, dbgCls.func_get_args4async(imgPath, msgtxt, wdss4srch));


            // var  = plchdTxt;
            // print(string.Format("{0}-{1}", de.Key, de.Value));
            var Photo = InputFile.FromStream(System.IO.File.OpenRead(imgPath));
            var chtsSess = JsonConvert.DeserializeObject<Hashtable>(System.IO.File.ReadAllText(timerCls.chatSessStrfile))!;
            //遍历方法三：遍历哈希表中的键值
            foreach_hashtable(chtsSess, (de) =>
            {
                var chatid = Convert.ToInt64(de.Key);
                var map = de.Value;
                JObject jo = (JObject)map;
                string chtType = getFld(jo, "chat.type", "");
                //私聊不要助力本群
                if (!chtType.Contains("group"))
                {
                    string chatid2249 = chatid.ToString();

                    string dbfile = $"{prjdir}/cfg_prvtChtPark/{chatid2249}.json";

                    SortedList cfg = findOne(dbfile);
                    Dictionary<string, StringValues> whereExprsObjFltrs = CopySortedListToDictionary(cfg);
                    //todo set    limit  cdt into 
                    //   results = mrcht.qryFromMrcht("mercht商家数据", null, whereExprsObj, msgx);
                    srchNsendFotoToChatSess(imgPath, msgtxt, wdss4srch, chatid, whereExprsObjFltrs, null);
                }
                else
                {

                    var groupId = chatid;
                    List<SortedList> grpcfgObj = ormJSonFL.qry($"{prjdir}/grpCfgDir/grpcfg{groupId}.json");
                    string whereExprs = (string)db.getRowVal(grpcfgObj, "whereExprs", "");
                    //    city = "

                    //qry from mrcht by  where exprs  strFmt
                    var whereExprsObj = QueryHelpers.ParseQuery(whereExprs);
                    var shareNamess = arrCls.ldfld_TryGetValue(whereExprsObj, "@share");
                    srchNsendFotoToChatSess(imgPath, msgtxt, wdss4srch, chatid, whereExprsObj, shareNamess);
                }
                return 0;
            });




            dbgCls.print_ret(__METHOD__, 0);
        }



        private static void srchNsendFotoToChatSess(string imgPath, string msgtxt, string wdss, long chatid, Dictionary<string, StringValues> whereExprsObjFltrs, string partfile区块文件Exprs)
        {
            //   Dictionary<string, StringValues> whereExprsObj;
            //  string partfile区块文件Exprs;
            // calcPrtExprsNdefWhrCondt4grp(chatid, out whereExprsObj, out partfile区块文件Exprs);
            //  var patns_dbfs = db.calcPatns("mercht商家数据", partfile区块文件Exprs);

            string keyword = getRdmKwd(wdss);

            //----where
            Func<SortedList, bool> whereFun = (SortedList row) =>
            {

                if (ldFldDefEmpty(row, "TG有效") == "N")
                    return false;
                //if have condit n fuhe condit next...beir skip ( dont have cdi or not eq )
                if (hasCondt(whereExprsObjFltrs, "城市"))
                    if (!strCls.str_eq(row["城市"], arrCls.ldfld_TryGetValue(whereExprsObjFltrs, "城市")))   //  cityname not in (citysss) 
                        return false;
                if (hasCondt(whereExprsObjFltrs, "园区"))
                    if (!strCls.str_eq(row["园区"], arrCls.ldfld_TryGetValue(whereExprsObjFltrs, "园区")))   //  cityname not in (citysss) 
                        return false;
                if (hasCondt(whereExprsObjFltrs, "国家"))
                    if (!strCls.str_eq(row["国家"], arrCls.ldfld_TryGetValue(whereExprsObjFltrs, "国家")))   //  cityname not in (citysss) 
                        return false;
                if (arrCls.ldFldDefEmpty(row, "cateEgls") == "Property")
                    return false;


                //--------------is lianixfsh empty
                string lianxifsh = mrcht.getLianxifsh(row);
                if (lianxifsh == "")
                    return false;

                HashSet<string> curRowKywdSset = new HashSet<string>();
                arrCls.add_elmts2hsst(curRowKywdSset, arrCls.ldFldDefEmpty(row, "商家"));
                arrCls.add_elmts2hsst(curRowKywdSset, arrCls.ldFldDefEmpty(row, "关键词"));
                arrCls.add_elmts2hsst(curRowKywdSset, arrCls.ldFldDefEmpty(row, "分类关键词"));
                if (curRowKywdSset.Contains(keyword))
                    return true;
                return false;
            };


            //order
            //  List<SortedList> results22 = rdmList<SortedList>(rztLi);
            Func<SortedList, int> ordFun = (SortedList) => { return 1; };
            //map select 
            Func<SortedList, InlineKeyboardButton[]> mapFun = (SortedList row) =>
            {
                string text = arrCls.ldFldDefEmpty(row, "城市") + " • " + arrCls.ldFldDefEmpty(row, "园区") + " • " + arrCls.ldFldDefEmpty(row, "商家");
                string guid = arrCls.ldFldDefEmpty(row, "Guid编号");
                InlineKeyboardButton[] btnsInLine = new[] { new InlineKeyboardButton(text) { CallbackData = $"id={guid}&sdr=tmr&btn=dtl&ckuid=n" } };
                return btnsInLine;
            };

            List<InlineKeyboardButton[]> rztLi = Qe_qryV2<InlineKeyboardButton[]>("mercht商家数据", partfile区块文件Exprs, whereFun, ordFun, mapFun, (dbf) =>
            {
                return rnd_next4Sqlt(dbf);
            });



            var results3 = rztLi.Skip(0 * 10).Take(5).ToList();
           print(" SendPhotoAsync " + chatid);//  Program.botClient.send
            if (results3.Count > 0)
                sendFoto(imgPath, msgtxt, results3, chatid);
        }

        private static string getRdmKwd(string wdss)
        {
            var arr = wdss.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray();
            var rdm = new Random().Next(1, arr.Length);

            string? keyword = arr[rdm - 1];
            return keyword;
        }

        private static void calcPrtExprsNdefWhrCondt4grp(long chatid, out Dictionary<string, StringValues> whereExprsObj, out string partfile区块文件Exprs)
        {
            var groupId = chatid;
            List<SortedList> grpcfgObj = ormJSonFL.qry($"{prjdir}/grpCfgDir/grpcfg{groupId}.json");
            string whereExprs = (string)db.getRowVal(grpcfgObj, "whereExprs", "");
            //    city = "

            //qry from mrcht by  where exprs  strFmt
            whereExprsObj = QueryHelpers.ParseQuery(whereExprs);
            partfile区块文件Exprs = arrCls.ldfld_TryGetValue(whereExprsObj, "@share");
        }


        //Program.botClient.SendTextMessageAsync(
        //         Program.groupId,
        //         "活动商家",
        //         parseMode: ParseMode.Html,
        //         replyMarkup: new InlineKeyboardMarkup(results),
        //         protectContent: false,
        //         disableWebPagePreview: true);

        private static void sendFoto(string imgPath, string msgtxt, List<InlineKeyboardButton[]> results, long chatid)
        {
            try
            {
                var Photo2 = InputFile.FromStream(System.IO.File.OpenRead(imgPath));
                Message message2 =   Program.botClient.SendPhotoAsync(
               chatid
                  , Photo2, null,
                  msgtxt,
                    parseMode: ParseMode.Html,
                   replyMarkup: new InlineKeyboardMarkup(results),
                   protectContent: false).Result;
               print(JsonConvert.SerializeObject(message2));
            }
            catch (Exception ex) {print(ex.ToString()); }
        }

        public static InlineKeyboardButton[][] ConvertHtmlLinksToTelegramButtons(string filePath)
        {
            // 读取HTML文件内容
            var htmlContent = System.IO.File.ReadAllText(filePath);

            // 使用HtmlAgilityPack解析HTML内容
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);

            // 查找所有的<a>标签
            var linkNodes = htmlDoc.DocumentNode.SelectNodes("//a[@href]");

            if (linkNodes == null || !linkNodes.Any())
            {
                return Array.Empty<InlineKeyboardButton[]>();
            }

            // 将每个<a>标签转换为InlineKeyboardButton
            var buttons = linkNodes
                .Select(linkNode =>
                {
                    var url = linkNode.GetAttributeValue("href", string.Empty);
                    var text = linkNode.InnerText;
                    return InlineKeyboardButton.WithUrl(text, url);
                })
                .ToList();

            // 将按钮组织成一个二维数组，这里假设每行只有一个按钮
            //var buttonRows = buttons
            //    .Select(button => new[] { button })
            //    .ToArray();

            // 将按钮组织成一个二维数组，每行3个按钮
            var buttonRows = buttons
                .Select((button, index) => new { button, index })
                .GroupBy(x => x.index / 3)
                .Select(g => g.Select(x => x.button).ToArray())
                .ToArray();
            return buttonRows;
        }

        public static InlineKeyboardButton[] ConvertHtmlLinksToTelegramButtonsSnglRow(string filePath)
        {
            // 读取HTML文件内容
            var htmlContent = System.IO.File.ReadAllText(filePath);

            // 使用HtmlAgilityPack解析HTML内容
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);

            // 查找所有的<a>标签
            var linkNodes = htmlDoc.DocumentNode.SelectNodes("//a[@href]");

            if (linkNodes == null || !linkNodes.Any())
            {
                return Array.Empty<InlineKeyboardButton>();
            }

            // 将每个<a>标签转换为InlineKeyboardButton
            var buttons = linkNodes
                .Select(linkNode =>
                {
                    var url = linkNode.GetAttributeValue("href", string.Empty);
                    var text = linkNode.InnerText;
                    return InlineKeyboardButton.WithUrl(text, url);
                })
                .ToArray();

            return buttons;
        }

        /**
       * 
       * dep
       * 很多时候msg 可能没有text，可能是个evt msg，，enter grp，join grp等
       */
        public static string bot_getTxtMsgDep(Update update)
        {
            if (update.Type == UpdateType.Message && update?.Message?.Text != null)
                return update?.Message?.Text;
            if (update.Type == UpdateType.Message && update?.Message?.Caption != null)
                return update?.Message?.Caption;
            if (update.Type == UpdateType.CallbackQuery)
                return update?.CallbackQuery?.Message?.ReplyToMessage?.Text;

            return "";

        }

        public static string bot_getTxt(Update update)
        {
            if (update.Type == UpdateType.Message && update?.Message?.Text != null)
                return update?.Message?.Text;
            if (update.Type == UpdateType.Message && update?.Message?.Caption != null)
                return update?.Message?.Caption;


            return "";

        }
        //出错后执行的方法
        public static System.Threading.Tasks.Task bot_pollingErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
           print("FUN bot_pollingErrorHandler()");
            try
            {
                logErr2024(exception, "bot_pollingErrorHandler", "errlog", (bot: botClient, cancellationToken: cancellationToken));
                var ErrorMessage = exception switch
                {
                    ApiRequestException apiRequestException => $"Telegram API 错误:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };
               print(ErrorMessage);

            }
            catch (Exception e)
            {
                logErr2024(e, "bot_pollingErrorHandler", "errlog", (bot: botClient, cancellationToken: cancellationToken));

            }
           print("END FUN bot_pollingErrorHandler()");
            return System.Threading.Tasks.Task.CompletedTask;
        }
        //删除别人信息
        public static void bot_DeleteMessageV2(long chatId, int msgid, int second)

        {
           callAsync(async () =>
                {
                    await System.Threading.Tasks.Task.Delay(second * 1000);

                    try
                    {
                        await Program.botClient.DeleteMessageAsync(chatId, msgid);
                    }
                    catch (Exception e)
                    {
                       print("删除他人商家搜索信息时出错:" + e.Message);
                    }
                    
                });

        }
        public static void bot_saveChtSesion(object chtid, object frm)
        { 
                if (!System.IO.File.Exists(timerCls.chatSessStrfile))
                    System.IO.File.WriteAllText(timerCls.chatSessStrfile, "{}");


                Hashtable chtsSesss = JsonConvert.DeserializeObject<Hashtable>(System.IO.File.ReadAllText(timerCls.chatSessStrfile))!;

                if (chtsSesss.Contains(Convert.ToString(chtid)))
                {
                    return;
                }
                if (!chtsSesss.Contains(Convert.ToString(chtid)))
                {
                    chtsSesss.Add(chtid, frm);

                    System.IO.File.WriteAllText(timerCls.chatSessStrfile, JsonConvert.SerializeObject(chtsSesss, Newtonsoft.Json.Formatting.Indented));

                }
            

        }
        //新增加入的聊天Id
        public static void bot_AddChatIds(long chatId)
        {
            var id = chatId.ToString();
            if (Program.chatIds.Contains(chatId.ToString()) == false)
            {
                Program.chatIds.Add(id);
            AddChatIds:
                try
                {
                    System.IO.File.AppendAllLines("chatIds.txt", [id]);
                }
                catch
                {
                    goto AddChatIds;
                }
            }
        }
        public static void tg_addChtid(Update update)
        {
            try
            {
                long chatId = -1;
                switch (update!.Type)
                {
                    case UpdateType.Message:
                        chatId = update.Message!.Chat.Id;
                        break;
                    case UpdateType.EditedMessage:
                        //try
                        //{
                        //    await tglib.bot_dltMsgThenSendmsg(update.EditedMessage!.Chat.Id, update.EditedMessage.MessageId, "不可二次编辑搜索信息,只能重新搜索,现对您编辑的信息进行销毁!", 5);
                        //}
                        //catch (Exception ex)
                        //{
                        //   print("告知不可二次编辑时出错:" + ex.Message);
                        //}
                        break;
                    case UpdateType.CallbackQuery:
                        chatId = update.CallbackQuery!.From.Id;
                        break;
                    case UpdateType.ChannelPost:
                        chatId = update.ChannelPost!.Chat.Id;
                        break;
                    case UpdateType.MyChatMember:
                        chatId = update.MyChatMember!.Chat.Id;
                        break;
                    case UpdateType.ChatMember:
                        chatId = update.ChatMember!.Chat.Id;
                        break;
                    case UpdateType.ChatJoinRequest:
                        chatId = update.ChatJoinRequest!.Chat.Id;
                        break;
                    default:
                        break;
                }

                if (chatId != -1)
                    tglib.bot_AddChatIds(chatId);
            }
            catch (Exception e)
            {
               print(e);
            }

        }


        public static void bot_saveGrpInf2db(ChatMemberUpdated myChatMember)
        {
            if (myChatMember == null)
                return;
            
                if (myChatMember.Chat.Type.ToString().ToLower() == "supergroup")
                {
                    SortedList chtsSesss = new SortedList();
                    chtsSesss.Add("id", myChatMember.Chat.Id);
                    chtsSesss.Add("grp", myChatMember.Chat.Title);
                    chtsSesss.Add("loc", "Unk");
                    ormSqlt._saveDep("grpinfo", chtsSesss, "grpinfoDB.db");
                }
           


        }

        public static void bot_iniChtStrfile()
        {
            if (!System.IO.File.Exists(timerCls.chatSessStrfile))
                System.IO.File.WriteAllText(timerCls.chatSessStrfile, "{}");
        }
        public static ReplyKeyboardMarkup ConvertTextToReplyKeyboardMarkup(string text)
        {
            var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var keyboard = new List<List<KeyboardButton>>();

            foreach (var line in lines)
            {
                List<KeyboardButton> buttonRow = castLine2ListKybdBtn(line);

                keyboard.Add(buttonRow);
            }
            ReplyKeyboardMarkup mkup = new ReplyKeyboardMarkup(keyboard);
            mkup.ResizeKeyboard = true;
            return mkup;
            //new ReplyKeyboardMarkup { Keyboard = keyboard, ResizeKeyboard = true, OneTimeKeyboard = true };
        }

        //public static void MsgHdlrProcess(Delegate callback, Update update)
        //{
            
             

           

        //}

      
        public static List<KeyboardButton> castLine2ListKybdBtn(string line)
        {
            var words = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var buttonRow = new List<KeyboardButton>();

            foreach (var word in words)
            {
                var button = new KeyboardButton(word);

                buttonRow.Add(button);
            }

            return buttonRow;
        }

        public static List<InlineKeyboardButton> castLine2ListInlineKybdBtn(string line)
        {
            var words = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var buttonRow = new List<InlineKeyboardButton>();

            foreach (var word in words)
            {
                var button = new InlineKeyboardButton(word);

                buttonRow.Add(button);
            }

            return buttonRow;
        }

        public static InlineKeyboardMarkup ConvertHtmlToinlineKeyboard(string html)
        {
            string json = ConvertHtmlToJson(html);
            return ConvertJsonToInlineKeyboardMarkup(json);
        }

        public static string ConvertHtmlToJson(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var rows = doc.DocumentNode.SelectNodes("//tr");
            var inlineKeyboard = new List<List<Dictionary<string, string>>>();

            foreach (var row in rows)
            {
                var buttons = row.SelectNodes(".//button");
                var buttonList = new List<Dictionary<string, string>>();

                foreach (var button in buttons)
                {
                    var buttonData = new Dictionary<string, string>();
                    var textNode = button.InnerText.Trim();

                    buttonData["text"] = textNode;

                    if (button.Attributes["data-callback_data"] != null)
                    {
                        buttonData["callback_data"] = button.Attributes["data-callback_data"].Value;
                    }

                    if (button.Attributes["data-url"] != null)
                    {
                        buttonData["url"] = button.Attributes["data-url"].Value;
                    }

                    buttonList.Add(buttonData);
                }

                inlineKeyboard.Add(buttonList);
            }

            var result = new
            {
                inline_keyboard = inlineKeyboard
            };

            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }

        public static ChatId bot_getChatid(Update update)
        {
            if (update?.Message?.Chat?.Id != null)
                return update?.Message?.Chat?.Id;
            //  if (update?.CallbackQuery?.Message?.Chat?.Id != null)
            return update?.CallbackQuery?.Message?.Chat?.Id;
        }
        public static void bot_dltMsgThenSendmsg(long chatId, int msgid, string text, int second)
        {
            Message? msg = null;
            try
            {
                msg =   Program.botClient.SendTextMessageAsync(chatId, text, parseMode: ParseMode.Html, replyToMessageId: msgid).Result;
            }
            catch (Exception e)
            {
               print("他人发了不合规的商家搜索信息,告知对方时出错:" + e.Message);
            }

            if (msg == null)
                return;

            callAsync(async () =>
            {
                await System.Threading.Tasks.Task.Delay(second * 1000);

                try
                {
                    await Program.botClient.DeleteMessageAsync(chatId, msgid);
                }
                catch (Exception e)
                {
                   print("删除他人商家搜索信息时出错:" + e.Message);
                }

                try
                {
                    await Program.botClient.DeleteMessageAsync(chatId, msg.MessageId);
                }
                catch (Exception e)
                {
                   print("他人发了不合规的商家搜索信息,删除信息时出错:" + e.Message);
                }
            });
        }
    }


    //public class KeyboardButton
    //{
    //    [JsonProperty("text")]
    //    public string Text { get; set; }
    //}

    //public class ReplyKeyboardMarkup
    //{
    //    [JsonProperty("keyboard")]
    //    public List<List<KeyboardButton>> Keyboard { get; set; }

    //    [JsonProperty("resize_keyboard")]
    //    public bool ResizeKeyboard { get; set; }

    //    [JsonProperty("one_time_keyboard")]
    //    public bool OneTimeKeyboard { get; set; }
    //}
}
