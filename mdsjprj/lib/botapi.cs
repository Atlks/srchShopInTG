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

namespace prj202405.lib
{
    internal class botapi
    {

        /**
       * 
       * 
       * 很多时候msg 可能没有text，可能是个evt msg，，enter grp，join grp等
       */
        public static string bot_getTxtMsg(Update update)
        {
            if (update.Type == UpdateType.Message && update?.Message?.Text != null)
                return update?.Message?.Text;
            if (update.Type == UpdateType.Message && update?.Message?.Caption != null)
                return update?.Message?.Caption;
            if (update.Type == UpdateType.CallbackQuery)
                return update?.CallbackQuery?.Message?.ReplyToMessage?.Text;

            return "";

        }

        //出错后执行的方法
        public static Task bot_pollingErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API 错误:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
        //删除别人信息
        public static async Task bot_DeleteMessage(long chatId, int messageId, string text, int second)
        {
            Message? msg = null;
            try
            {
                msg = await Program. botClient.SendTextMessageAsync(chatId, text, parseMode: ParseMode.Html, replyToMessageId: messageId);
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
                    await Program.botClient.DeleteMessageAsync(chatId, messageId);
                }
                catch (Exception e)
                {
                    Console.WriteLine("删除他人商家搜索信息时出错:" + e.Message);
                }

                try
                {
                    await Program. botClient.DeleteMessageAsync(chatId, msg.MessageId);
                }
                catch (Exception e)
                {
                    Console.WriteLine("他人发了不合规的商家搜索信息,删除信息时出错:" + e.Message);
                }
            });
        }
    }
}
