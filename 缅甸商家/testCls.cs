
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Net.Mime.MediaTypeNames;

namespace 缅甸商家
{
    internal class testCls
    {
        internal static void test()
        {
            //联系商家城市
            //HashSet<City> _citys = [];
            //var merchants = System.IO.File.ReadAllText("Merchant.json");
            //if (!string.IsNullOrEmpty(merchants))
            //    _citys = JsonConvert.DeserializeObject<HashSet<City>>(merchants)!;

            //   午餐餐饮关键词 午餐 餐饮 鱼肉 牛肉 火锅 炒饭 炒粉

            //搜索关键词  Merchant.json to citys

           // wucan();
          // timerCls.  xiawucha();
          if(System.IO.File.Exists("c:/teststart.txt"))
            {
                timerCls.z_actSj();
              //  timerCls.renqi();
             //  timerCls.yule();
              //  timerCls.zaocan();
                //  timerCls.wucan();
                //  timerCls.xiawucha();
                //   timerCls.actSj();
                //   addData();

                //   findd();
            }
           
          // 

        }

        private static void findd()
        {
            long timestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            var users_txt = System.IO.File.ReadAllText("db.json");

            showSpanTime(timestamp, "readFile");

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DefaultValueHandling = DefaultValueHandling.Ignore;
            JArray rows = JsonConvert.DeserializeObject<JArray>(users_txt, settings);
            showSpanTime(timestamp, "delzobj");
            var results = (from   jo in rows
                           where jo.Value<int>("key") == 10
                           select jo).ToList();

           
            Console.WriteLine(JsonConvert.SerializeObject(results));


            string showtitle = "spatime(ms):";
            showSpanTime(timestamp, showtitle);

        }

        private static void showSpanTime(long timestamp, string showtitle)
        {
            long timestamp_end = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long spantime = (timestamp_end - timestamp);

            Console.WriteLine(showtitle + spantime);
        }

        private static void addData()
        {
 
            long timestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;



            List<object> data = new List<object>();
            for (int i = 0;i<100*10000;i++)
            {
                Hashtable ht= new Hashtable();
                ht.Add("key", i);
                ht.Add("value"+i, i);
                data.Add(ht);
            }
            System.IO.File.WriteAllText("db.json", JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented));

            long timestamp_end = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long spantime = (   timestamp_end- timestamp);
            Console.WriteLine("spatime(ms):" + spantime);

        }
    }

}


//var users = System.IO.File.ReadAllText("Users.json");
//    if (!string.IsNullOrEmpty(users))
//        _users = JsonConvert.DeserializeObject<Dictionary<long, User>>(users)!;
//    var merchants = System.IO.File.ReadAllText("Merchant.json");

//var _citys = (JArray)JsonConvert.DeserializeObject(merchants);
//var arr3 = (JArray)_citys[0].Value<JArray>("Address");

//    foreach (JObject it in arr3)
//    {
//        var shopsarr = it.Value<JArray>("Merchant");
//        foreach (JObject shop in shopsarr)
//        {
//            Console.WriteLine(shop.GetValue("KeywordString").ToString());
//        }

//        }



