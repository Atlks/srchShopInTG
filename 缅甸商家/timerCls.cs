using System.Timers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace 缅甸商家
{
    internal class timerCls
    {


        public static void setTimerTask()
        {
            //活动商家(每小时推送)   物业(跟随每个商家推送) 
            //早餐店6点推送   午餐店11点推送推送   下午茶(水果/奶茶)店16点推送   晚餐店18点推送    娱乐消遣/酒店推送21点推送   活动商家(每小时推送)   物业(跟随每个商家推送)    每日人气榜单(每日夜间0:00推送)
            //_ = Task.Run(async () =>
            //{
            //    while (true)
            //    {
            //        var now = DateTime.Now;

            //        await Task.Delay(1000);
            //    }
            //});


            //设置定时间隔(毫秒为单位)
            int interval = 5000;
            System.Timers.Timer timer = new System.Timers.Timer(interval);
            //设置执行一次（false）还是一直执行(true)
            timer.AutoReset = true;
            //设置是否执行System.Timers.Timer.Elapsed事件
            timer.Enabled = true;
            //绑定Elapsed事件
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timerCls.TimerUp);
            timer.Start();
        }

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


            //下午差
            //18,wecan,wancan()
              xwcF = $"tmrlg/wecanPushLog{Convert.ToString(now.Month) + now.Day}.json";
            if (now.Hour == 18 && (!System.IO.File.Exists(xwcF)))
            {
                System.IO.File.WriteAllText(xwcF, "pushlog");
                // do something
                wucan();
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

            //#huodong 商家

            rqF = $"tmrlg/actShjPushLog{Convert.ToString(now.Month) + now.Day+ Convert.ToString(now.Hour)}.json";
            if (  (!System.IO.File.Exists(rqF)))
            {
                System.IO.File.WriteAllText(rqF, "pushlog");
                // do something
                actSj();
            }
        }

        public static void actSj()
        {
            List<InlineKeyboardButton[]> results = [];
            results = (from c in Program._citys
                       from ca in c.Address
                       from am in ca.Merchant
                       orderby am.Views descending
                       select new[] { new InlineKeyboardButton(c.Name + " • " + ca.Name + " • " + am.Name) { CallbackData = $"Merchant?id={am.Guid}" } }).ToList();
            //count = results.Count;


            results = rdmList(results);

            results = results.Skip(0 * 10).Take(5).ToList();
            Program.botClient.SendTextMessageAsync(
                     Program.groupId,
                     "活动商家",
                     parseMode: ParseMode.Html,
                     replyMarkup: new InlineKeyboardMarkup(results),
                     protectContent: false,
                     disableWebPagePreview: true);
        }

        private static void wancan()
        {
            throw new NotImplementedException();
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
                       "激动的心，颤抖的手,又到了娱乐时间啦",
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
                       "美好的一天从早上开始，当然美丽的心情从早餐开始，别忘了吃早餐哦",
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
                foreach (InlineKeyboardButton[] btn in results)
                {

                }

                results22 = rdmList(results);

                results22 = results22.Skip(0 * 10).Take(5).ToList();
            }

            return results22;
        }

        private static List<InlineKeyboardButton[]> rdmList(List<InlineKeyboardButton[]> results)
        {
            List<InlineKeyboardButton[]> results22;
            Random rng = new Random();

            results22 = results.OrderBy(x => rng.Next()).ToList();
            return results22;
        }

        //dep
        public static List<InlineKeyboardButton[]> qryFrmShangjiaOrdbyViewDesc__DEP()
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