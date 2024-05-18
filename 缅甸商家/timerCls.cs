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

            // //早餐
            var zaocanLgF = $"tmrlg/brkfstPushLog{Convert.ToString(now.Month) + now.Day}.json";
            if (now.Hour == 6 && (!System.IO.File.Exists(zaocanLgF )) )  
            {
                // do something
                System.IO.File.WriteAllText(zaocanLgF, "pushlog");
                // Program.botClient.SendTextMessageAsync(chatId: Program.groupId, text: "早餐时间到了");
                zaocan();
            }


            //午餐
            if (now.Hour == 11 && (!System.IO.File.Exists($"tmrlg/lunchPushLog{Convert.ToString(now.Month)+now.Day}.json")) )  
               
            {
                Console.WriteLine("push luch time。");
                System.IO.File.WriteAllText($"tmrlg/lunchPushLog{Convert.ToString(now.Month) + now.Day}.json", "pushlog");
                //  Program.botClient.SendTextMessageAsync(chatId: Program.groupId, text: "午餐时间到了");
                wucan();

            }


            //下午差
            var xwcF = $"tmrlg/xiawuchaPushLog{Convert.ToString(now.Month) + now.Day}.json";
            if (now.Hour == 16 && (!System.IO.File.Exists(xwcF)) )
            {
                System.IO.File.WriteAllText(xwcF, "pushlog");
                // do something
                xiawucha();
            }


            //娱乐
            var ylF = $"tmrlg/yulePushLog{Convert.ToString(now.Month) + now.Day}.json";
            if (now.Hour == 21 && (!System.IO.File.Exists(xwcF) ) )
            {
                System.IO.File.WriteAllText(ylF, "pushlog");
                // do something
                yule();
            }



            //人气榜
            var rqF = $"tmrlg/renqiPushLog{Convert.ToString(now.Month) + now.Day}.json";
            if (now.Hour == 0 && (!System.IO.File.Exists(xwcF)) ) 
            {
                System.IO.File.WriteAllText(rqF, "pushlog");
                // do something
                renqi();
            }
        }

        public static void renqi()
        {
            var s = "";
            List<InlineKeyboardButton[]> results = [];
 
            results = (from c in Program._citys
                       from ca in c.Address
                       from am in ca.Merchant
                           //   where searchChars.All(s => (c.CityKeywords + ca.CityKeywords + am.KeywordString + am.KeywordString + Program._categoryKeyValue[(int)am.Category]).Contains(s))
                       orderby am.Views descending
                       select new[] { new InlineKeyboardButton(c.Name + " • " + ca.Name + " • " + am.Name) { CallbackData = $"Merchant?id={am.Guid}" } }).ToList();
            //count = results.Count;
            results = results.Skip(0 * 10).Take(5).ToList();

            Program.botClient.SendTextMessageAsync(
                       Program.groupId,
                       "人气榜",
                       parseMode: ParseMode.Html,
                       replyMarkup: new InlineKeyboardMarkup(results),
                       protectContent: false,
                       disableWebPagePreview: true);
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
                       "懂得享受下午茶时光。点一杯咖啡，点一杯奶茶 ，亦或自己静静思考，生活再忙碌，也要记得给自己喘口气",
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
            List<InlineKeyboardButton[]> results22 = new List<InlineKeyboardButton[]>();
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
                foreach(InlineKeyboardButton[] btn in results)
                {

                }

                Random rng = new Random();

                results22 = results.OrderBy(x => rng.Next()).ToList();




                results22 = results22.Skip(0 * 10).Take(5).ToList();
            }

            return results22;
        }


        public static List<InlineKeyboardButton[]> qryFrmShangjiaOrdbyViewDesc()
        {

            List<InlineKeyboardButton[]> results = [];
            results = (from c in Program._citys
                           from ca in c.Address
                           from am in ca.Merchant
                        //   where searchChars.All(s => (c.CityKeywords + ca.CityKeywords + am.KeywordString + am.KeywordString + Program._categoryKeyValue[(int)am.Category]).Contains(s))
                           orderby am.Views descending
                           select new[] { new InlineKeyboardButton(c.Name + " • " + ca.Name + " • " + am.Name) { CallbackData = $"Merchant?id={am.Guid}" } }).ToList();
                //count = results.Count;
                results = results.Skip(0 * 10).Take(5).ToList();
           

            return results;
        }


    }
}