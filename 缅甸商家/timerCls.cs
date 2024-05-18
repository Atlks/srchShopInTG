using System.Timers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace 缅甸商家
{
    internal class timerCls
    {
        internal static void TimerUp(object? sender, ElapsedEventArgs e)
        {

            System.IO.Directory.CreateDirectory("tmrlg");
            Console.WriteLine("定时任务。。");

            DateTime now = DateTime.Now;

            var zaocanLgF = $"tmrlg/brkfstPushLog{Convert.ToString(now.Month) + now.Day}.json";
            if (now.Hour == 6 && (!System.IO.File.Exists(zaocanLgF )) )  //早餐
            {
                // do something
                System.IO.File.WriteAllText(zaocanLgF, "pushlog");
                // Program.botClient.SendTextMessageAsync(chatId: Program.groupId, text: "早餐时间到了");
                zaocan();
            }
            if (now.Hour == 11 && (!System.IO.File.Exists($"tmrlg/lunchPushLog{Convert.ToString(now.Month)+now.Day}.json")) )  //午餐
               
            {
                Console.WriteLine("push luch time。");
                System.IO.File.WriteAllText($"tmrlg/lunchPushLog{Convert.ToString(now.Month) + now.Day}.json", "pushlog");
                //  Program.botClient.SendTextMessageAsync(chatId: Program.groupId, text: "午餐时间到了");
                wucan();

            }


            var xwcF = $"tmrlg/xiawuchaPushLog{Convert.ToString(now.Month) + now.Day}.json";
            if (now.Hour == 16 && (!System.IO.File.Exists(xwcF)) ) //下午差
            {
                System.IO.File.WriteAllText(xwcF, "pushlog");
                // do something
                xiawucha();
            }

            var ylF = $"tmrlg/yulePushLog{Convert.ToString(now.Month) + now.Day}.json";
            if (now.Hour == 21)  //娱乐
            {
                System.IO.File.WriteAllText(ylF, "pushlog");
                // do something
                yule();
            }

            if (now.Hour == 0)  //人气榜
            {
                // do something
            }
        }
        public static void yule()
        {
            var s = "娱乐 ktv 水疗";
            List<InlineKeyboardButton[]> results = qryFrmShangjiaByKwds(s);

            Program.botClient.SendTextMessageAsync(
                       Program.groupId,
                       "娱乐推荐",
                       parseMode: ParseMode.Html,
                       replyMarkup: new InlineKeyboardMarkup(results),
                       protectContent: false,
                       disableWebPagePreview: true);
        }

        public static void zaocan()
        {
            var s = "早餐 餐饮 鱼肉 牛肉 火锅 炒饭 炒粉";
            List<InlineKeyboardButton[]> results = qryFrmShangjiaByKwds(s);

            Program.botClient.SendTextMessageAsync(
                       Program.groupId,
                       "早餐推荐",
                       parseMode: ParseMode.Html,
                       replyMarkup: new InlineKeyboardMarkup(results),
                       protectContent: false,
                       disableWebPagePreview: true);
        }

        public static void wucan()
        {
            var s = "午餐 餐饮 鱼肉 牛肉 火锅 炒饭 炒粉";
            List<InlineKeyboardButton[]> results = qryFrmShangjiaByKwds(s);

            Program.botClient.SendTextMessageAsync(
                       Program.groupId,
                       "午餐推荐",
                       parseMode: ParseMode.Html,
                       replyMarkup: new InlineKeyboardMarkup(results),
                       protectContent: false,
                       disableWebPagePreview: true);
        }

        public static void xiawucha()
        {
            var s = "下午茶 奶茶 水果茶 水果";
            List<InlineKeyboardButton[]> results = qryFrmShangjiaByKwds(s);

            Program.botClient.SendTextMessageAsync(
                       Program.groupId,
                       "下午茶推荐",
                       parseMode: ParseMode.Html,
                       replyMarkup: new InlineKeyboardMarkup(results),
                       protectContent: false,
                       disableWebPagePreview: true);
        }


        public static List<InlineKeyboardButton[]> qryFrmShangjiaByKwds(string s)
        {
            var arr = s.Split(" ").ToArray();

            var rdm = new Random().Next(1, arr.Length);
            string? keyword = arr[rdm - 1];

            List<InlineKeyboardButton[]> results = [];
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower().Replace(" ", "").Trim();
                var searchChars = keyword!.ToCharArray();

                results = (from c in Program._citys
                           from ca in c.Address
                           from am in ca.Merchant
                           where searchChars.All(s => (c.CityKeywords + ca.CityKeywords + am.KeywordString + am.KeywordString + Program._categoryKeyValue[(int)am.Category]).Contains(s))
                           orderby am.Views descending
                           select new[] { new InlineKeyboardButton(c.Name + " • " + ca.Name + " • " + am.Name) { CallbackData = $"Merchant?id={am.Guid}" } }).ToList();
                //count = results.Count;
                results = results.Skip(0 * 10).Take(10).ToList();
            }

            return results;
        }


    }
}