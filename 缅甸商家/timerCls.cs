using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Collections;
using System.Timers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using prj202405.lib;

namespace prj202405
{
    internal class timerCls
    {
        public const string chatSessStrfile = "chtSess.json";


        public static void setTimerTask()
        {
            return;
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
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timerCls.TimerEvt);
            timer.Start();
        }

        internal static void TimerEvt(object? sender, ElapsedEventArgs e)
        {

            System.IO.Directory.CreateDirectory("tmrlg");
            Console.WriteLine("定时任务。。");

            DateTime now = DateTime.Now;

            // //早餐
            var zaocanLgF = $"tmrlg/brkfstPushLog{Convert.ToString(now.Month) + now.Day}.json";
            if (now.Hour == 6 && (!System.IO.File.Exists(zaocanLgF)))
            {
                // do something
                System.IO.File.WriteAllText(zaocanLgF, "pushlog");
                // Program.botClient.SendTextMessageAsync(chatId: Program.groupId, text: "早餐时间到了");
                zaocan();
            }


            //午餐
            if (now.Hour == 11 && (!System.IO.File.Exists($"tmrlg/lunchPushLog{Convert.ToString(now.Month) + now.Day}.json")))

            {
                Console.WriteLine("push luch time。");
                System.IO.File.WriteAllText($"tmrlg/lunchPushLog{Convert.ToString(now.Month) + now.Day}.json", "pushlog");
                //  Program.botClient.SendTextMessageAsync(chatId: Program.groupId, text: "午餐时间到了");
                z_wucan();

            }


            //下午差
            var xwcF = $"tmrlg/xiawuchaPushLog{Convert.ToString(now.Month) + now.Day}.json";
            if (now.Hour == 16 && (!System.IO.File.Exists(xwcF)))
            {
                System.IO.File.WriteAllText(xwcF, "pushlog");
                // do something
                z_xiawucha();
            }


            //下午差
            //18,wecan,wancan()
            xwcF = $"tmrlg/wecanPushLog{Convert.ToString(now.Month) + now.Day}.json";
            if (now.Hour == 18 && (!System.IO.File.Exists(xwcF)))
            {
                System.IO.File.WriteAllText(xwcF, "pushlog");
                // do something
                z18_wancan();
            }


            //娱乐
            var ylF = $"tmrlg/yulePushLog{Convert.ToString(now.Month) + now.Day}.json";
            if (now.Hour == 21 && (!System.IO.File.Exists(xwcF)))
            {
                System.IO.File.WriteAllText(ylF, "pushlog");
                // do something
                z21_yule();
            }



            //人气榜
            var rqF = $"tmrlg/renqiPushLog{Convert.ToString(now.Month) + now.Day}.json";
            if (now.Hour == 0 && (!System.IO.File.Exists(rqF)))
            {
                System.IO.File.WriteAllText(rqF, "pushlog");
                // do something
                z_renqi();
            }

            //#huodong 商家
            var hour = "8";
            rqF = $"tmrlg/actShjPushLog{Convert.ToString(now.Month) + now.Day + Convert.ToString(now.Hour)}.json";
            if (now.Hour == 8 && (!System.IO.File.Exists(rqF)))
            {
                System.IO.File.WriteAllText(rqF, "pushlog");
                // do something
                z_actSj();
            }


            rqF = $"tmrlg/actMenuPushLog{Convert.ToString(now.Month) + now.Day + Convert.ToString(now.Hour)}.json";
            if ((now.Hour == 10 || now.Hour == 16) && (!System.IO.File.Exists(rqF)))
            {
                System.IO.File.WriteAllText(rqF, "pushlog");
                // do something
                //var Keyboard =
                //  new KeyboardButton[][]
                //  {
                //            new KeyboardButton[]
                //            {
                //                new KeyboardButton("美食"),
                //                new KeyboardButton("会所")
                //            },

                //            new KeyboardButton[]
                //            {
                //                new KeyboardButton("酒吧")
                //            },

                //            new KeyboardButton[]
                //            {
                //                new KeyboardButton("咖啡"),
                //                new KeyboardButton("ktv"),
                //                new KeyboardButton("医院")
                //            }
                //  };
                //var rkm = new ReplyKeyboardMarkup(Keyboard);
                sendMsg4keepmenu("今日促销商家.gif", plchdTxt,Program. _btmBtns());
            }


        }
        public static string plchdTxt = "💁博彩信誉盘推荐：  <a href='https://t.me/shibolianmeng'>世博联盟 </a>";
        //static string   plchdTxt = "💸 信誉博彩盘推荐 :  世博联盟飞投博彩 (https://t.me/shibolianmeng) 💸";
        public static async void z_actSj()
        {
            List<InlineKeyboardButton[]> results = [];
            results = (from c in Program._citys
                       from ca in c.Address
                       from am in ca.Merchant
                       orderby am.Views descending
                       select new[] { new InlineKeyboardButton(c.Name + " • " + ca.Name + " • " + am.Name) { CallbackData = $"Merchant?id={am.Guid}&timerMsgMode2025" } }).ToList();
            //count = results.Count;


            results = arrCls.rdmList<InlineKeyboardButton[]>(results);

            results = results.Skip(0 * 10).Take(5).ToList();



            string Path = "今日促销商家.gif";
            await sendMsg(Path, plchdTxt, results);
        }


        // sendmsg4timrtask
        private static async Task sendMsg(string imgPath, string msgtxt, List<InlineKeyboardButton[]> results)
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



            //Program.botClient.SendTextMessageAsync(
            //         Program.groupId,
            //         "活动商家",
            //         parseMode: ParseMode.Html,
            //         replyMarkup: new InlineKeyboardMarkup(results),
            //         protectContent: false,
            //         disableWebPagePreview: true);
        }


        private static async Task sendMsg4keepmenu(string imgPath, string msgtxt, ReplyKeyboardMarkup rplyKbdMkp)
        {
            // var  = plchdTxt;
            //  Console.WriteLine(string.Format("{0}-{1}", de.Key, de.Value));
            var Photo = InputFile.FromStream(System.IO.File.OpenRead(imgPath));
            //  Program.botClient.SendPhotoAsync()

            Message message = await Program.botClient.SendPhotoAsync(
                      Program.groupId, Photo, null,
                      msgtxt,
                        parseMode: ParseMode.Html,
                       replyMarkup: rplyKbdMkp,
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
                       replyMarkup: rplyKbdMkp,
                       protectContent: false);
                    Console.WriteLine(JsonConvert.SerializeObject(message2));

                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }

            }



        }

        //private static void wancan()
        //{
        //    throw new NotImplementedException();
        //}

        public static async void z_renqi()
        {



            string Path = "今日商家人气榜.gif";




            var s = "";
            List<InlineKeyboardButton[]> results = [];

            results = (from c in Program._citys
                       from ca in c.Address
                       from am in ca.Merchant
                           //   where searchChars.All(s => (c.CityKeywords + ca.CityKeywords + am.KeywordString + am.KeywordString + Program._categoryKeyValue[(int)am.Category]).Contains(s))
                       orderby am.Views descending
                       select new[] { new InlineKeyboardButton(c.Name + " • " + ca.Name + " • " + am.Name) { CallbackData = $"Merchant?id={am.Guid}&timerMsgMode2025" } }).ToList();
            //count = results.Count;
            results = results.Skip(0 * 10).Take(5).ToList();

            await sendMsg("今日商家人气榜.gif", plchdTxt, results);
        }

        public static async void z21_yule()
        {
            var s = "娱乐 ktv 水疗 会所 嫖娼 酒吧 足疗 spa 马杀鸡 按摩 咖啡爆 gogobar 啤酒吧 帝王浴 泡泡浴 nuru 咬吧";
            List<InlineKeyboardButton[]> results = qryFrmShangjiaByKwds(s);


            string Path = "娱乐消遣.gif";
            var CaptionTxt = "美好的一天从晚上开始，激动的心，颤抖的手,又到了娱乐时间啦";
            await sendMsg("娱乐消遣.gif", plchdTxt, results);

        }

        public static async void zaocan()
        {
            var s = "早餐 餐饮 鱼肉 牛肉 火锅 炒饭 炒粉";
            List<InlineKeyboardButton[]> results = qryFrmShangjiaByKwds(s);



            string Path = "早餐商家推荐.gif";
            var CaptionTxt = "美好的一天从早上开始，当然美丽的心情从早餐开始，别忘了吃早餐哦";

            await sendMsg("早餐商家推荐.gif", plchdTxt, results);
        }


        public static async void z18_wancan()
        {
            var s = "餐饮 米饭 牛肉 火锅 炒饭 炒粉";
            List<InlineKeyboardButton[]> results = qryFrmShangjiaByKwds(s);
            string CaptionTxt = "晚餐时间到了！让我们一起享受美食和愉快的时光吧！！";


            await sendMsg("晚餐商家推荐.gif", plchdTxt, results);

        }
        public static async void z_wucan()
        {
            var s = "餐饮 米饭 牛肉 火锅 炒饭 炒粉";
            List<InlineKeyboardButton[]> results = qryFrmShangjiaByKwds(s);
            var msgtxt = "午餐时间到了！让我们一起享受美食和愉快的时光吧！希望你的午后充满欢乐和满满的正能量！";

            await sendMsg("午餐商家推荐.gif", plchdTxt, results);


        }

        public static async void z_xiawucha()
        {
            var s = "下午茶 奶茶 水果茶 水果";
            var msgtxt = "懂得享受下午茶时光。点一杯咖啡，点一杯奶茶 ，亦或自己静静思考，生活再忙碌，也要记得给自己喘口气";
            List<InlineKeyboardButton[]> results = qryFrmShangjiaByKwds(s);


            await sendMsg("下午茶商家推荐.gif", plchdTxt, results);




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
                           select new[] { new InlineKeyboardButton(c.Name + " • " + ca.Name + " • " + am.Name) { CallbackData = $"Merchant?id={am.Guid}&timerMsgMode2025" } }).ToList();
                //count = results.Count;
                foreach (InlineKeyboardButton[] btn in results)
                {

                }

                results22 = arrCls.rdmList<InlineKeyboardButton[]>(results);

                results22 = results22.Skip(0 * 10).Take(5).ToList();
            }

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