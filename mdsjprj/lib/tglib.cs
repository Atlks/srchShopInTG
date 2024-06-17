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

using static mdsj.libBiz.tgBiz;
using static prj202405.lib.tglib;
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
        public static async Task bot_sendMsg(string imgPath, string msgtxt, List<InlineKeyboardButton[]> results)
        {

            var __METHOD__ = "sendMsg";
            dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args4async(imgPath, msgtxt, results));

            try
            {
                // var  = plchdTxt;
                //  Console.WriteLine(string.Format("{0}-{1}", de.Key, de.Value));
                var Photo = InputFile.FromStream(System.IO.File.OpenRead(imgPath));
                //  Program.botClient.SendPhotoAsync()

                Message message = await Program.botClient.SendPhotoAsync(
                          Program.groupId, Photo, null,
                          msgtxt,
                            parseMode: ParseMode.Html,
                           replyMarkup: new InlineKeyboardMarkup(results),
                           protectContent: false);

                Console.WriteLine(JsonConvert.SerializeObject(message));


                var chtsSess = JsonConvert.DeserializeObject<Hashtable>(System.IO.File.ReadAllText(timerCls.chatSessStrfile))!;
                //遍历方法三：遍历哈希表中的键值
                foreach (DictionaryEntry de in chtsSess)
                {
                    if (Convert.ToInt64(de.Key) == Program.groupId)
                        continue;
                    var key = de.Key;
                    Console.WriteLine(" SendPhotoAsync " + de.Key);

                    //  Program.botClient.send
                    try
                    {
                        var Photo2 = InputFile.FromStream(System.IO.File.OpenRead(imgPath));
                        Message message2 = await Program.botClient.SendPhotoAsync(
                        Convert.ToInt64(de.Key)
                          , Photo2, null,
                          msgtxt,
                            parseMode: ParseMode.Html,
                           replyMarkup: new InlineKeyboardMarkup(results),
                           protectContent: false);
                        Console.WriteLine(JsonConvert.SerializeObject(message2));

                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                logErr2024(e, __METHOD__, "errlog", (meth: __METHOD__, prm: func_get_args4async(imgPath, msgtxt, results)));

            }



            dbgCls.setDbgValRtval(__METHOD__, 0);
            //Program.botClient.SendTextMessageAsync(
            //         Program.groupId,
            //         "活动商家",
            //         parseMode: ParseMode.Html,
            //         replyMarkup: new InlineKeyboardMarkup(results),
            //         protectContent: false,
            //         disableWebPagePreview: true);
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
        public static Task bot_pollingErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            try
            {
                logErr2024(exception, "bot_pollingErrorHandler", "errlog", (bot: botClient, cancellationToken: cancellationToken));
                var ErrorMessage = exception switch
                {
                    ApiRequestException apiRequestException => $"Telegram API 错误:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };
                Console.WriteLine(ErrorMessage);
               
            }catch(Exception e)
            {
                logErr2024(e, "bot_pollingErrorHandler", "errlog", (bot: botClient, cancellationToken: cancellationToken));

            }
            return Task.CompletedTask;
        }
        //删除别人信息
        public static async Task bot_DeleteMessageV2(long chatId, int msgid, int second)

        {
            _ = Task.Run(async () =>
                {
                    await Task.Delay(second * 1000);

                    try
                    {
                        await Program.botClient.DeleteMessageAsync(chatId, msgid);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("删除他人商家搜索信息时出错:" + e.Message);
                    }
                });

        }
        public static void bot_saveChtSesion(object chtid, object frm)
        {
            try
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
            catch (Exception e)
            {

            }

        }
        //新增加入的聊天Id
       public static void bot_AddChatIds(long chatId)
        {
            var id = chatId.ToString();
            if (Program.chatIds.Contains(chatId.ToString()) == false)
            {
                Program. chatIds.Add(id);
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
        public static async Task tg_addChtid(Update update)
        {
            long chatId = -1;
            switch (update!.Type)
            {
                case UpdateType.Message:
                    chatId = update.Message!.Chat.Id;
                    break;
                case UpdateType.EditedMessage:
                    try
                    {
                        await tglib.bot_dltMsgThenSendmsg(update.EditedMessage!.Chat.Id, update.EditedMessage.MessageId, "不可二次编辑搜索信息,只能重新搜索,现对您编辑的信息进行销毁!", 5);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("告知不可二次编辑时出错:" + ex.Message);
                    }
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


        public static void bot_saveGrpInf2db(ChatMemberUpdated myChatMember)
        {
            try
            {
                if (myChatMember.Chat.Type.ToString().ToLower() == "supergroup")
                {
                    SortedList chtsSesss = new SortedList();
                    chtsSesss.Add("id", myChatMember.Chat.Id);
                    chtsSesss.Add("grp", myChatMember.Chat.Title);
                    chtsSesss.Add("loc", "Unk");
                    ormSqlt._saveDep("grpinfo", chtsSesss, "grpinfoDB.db");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }

        public static void bot_iniChtStrfile()
        {
            if (!System.IO.File.Exists(timerCls.chatSessStrfile))
                System.IO.File.WriteAllText(timerCls.chatSessStrfile, "{}");
        }


        public static ChatId bot_getChatid(Update update)
        {
            if (update?.Message?.Chat?.Id != null)
                return update?.Message?.Chat?.Id;
            //  if (update?.CallbackQuery?.Message?.Chat?.Id != null)
            return update?.CallbackQuery?.Message?.Chat?.Id;
        }
        public static async Task bot_dltMsgThenSendmsg(long chatId, int msgid, string text, int second)
        {
            Message? msg = null;
            try
            {
                msg = await Program.botClient.SendTextMessageAsync(chatId, text, parseMode: ParseMode.Html, replyToMessageId: msgid);
            }
            catch (Exception e)
            {
                Console.WriteLine("他人发了不合规的商家搜索信息,告知对方时出错:" + e.Message);
            }

            if (msg == null)
                return;

            _ = Task.Run(async () =>
            {
                await Task.Delay(second * 1000);

                try
                {
                    await Program.botClient.DeleteMessageAsync(chatId, msgid);
                }
                catch (Exception e)
                {
                    Console.WriteLine("删除他人商家搜索信息时出错:" + e.Message);
                }

                try
                {
                    await Program.botClient.DeleteMessageAsync(chatId, msg.MessageId);
                }
                catch (Exception e)
                {
                    Console.WriteLine("他人发了不合规的商家搜索信息,删除信息时出错:" + e.Message);
                }
            });
        }
    }
}
