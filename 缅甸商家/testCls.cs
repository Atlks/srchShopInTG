
using JiebaNet.Segmenter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using 缅甸商家.lib;
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
            if (System.IO.File.Exists("c:/teststart.txt"))
            {
                Merchant? merchant = new Merchant();
                merchant.Guid = "123456";
                merchant.Name = "shjjj";
                var text = "pinlunxxxx";
              Hashtable pinlunobj = new Hashtable();
                pinlunobj.Add("id", DateTime.Now.ToString());
                pinlunobj.Add("商家guid", merchant.Guid);
                pinlunobj.Add("商家", merchant.Name);
                pinlunobj.Add("时间", DateTime.Now.ToString());
                pinlunobj.Add("评论内容", text);
                ormSqlt.save("商家评论表", pinlunobj, "商家评论表.db");
                System.IO.Directory.CreateDirectory("pinlunDir");
                pinlun.savePinlun(pinlunobj, "pinlunDir/" + merchant.Guid + merchant.Name + ".json");




                const string DbFileName = "objs2005.db";
                Hashtable chtsSesss = new Hashtable();
                chtsSesss.Add("id", 1); chtsSesss.Add("nm", "....");
                ormSqlt.save("tb_memb", chtsSesss, DbFileName);

                Hashtable chtsSesss2 = new Hashtable();
                chtsSesss2.Add("id", 2); chtsSesss2.Add("nm", "nm222");

                ormSqlt.save("tb_memb", chtsSesss2, DbFileName);

                var rs = ormSqlt.qry("select * from tb_memb", DbFileName);



                var segmenter = new JiebaSegmenter();
                segmenter.LoadUserDict("user_dict.txt");
                segmenter.AddWord("会所"); // 可添加一个新词

                //var segments = segmenter.Cut("我来到北京清华大学", cutAll: true);
                //Console.WriteLine("【全模式】：{0}", string.Join("/ ", segments));

                //segments = segmenter.Cut("我来到北京清华大学");  // 默认为精确模式
                //Console.WriteLine("【精确模式】：{0}", string.Join("/ ", segments));

                //segments = segmenter.Cut("他来到了网易杭研大厦");  // 默认为精确模式，同时也使用HMM模型
                //Console.WriteLine("【新词识别】：{0}", string.Join("/ ", segments));

                var segments = segmenter.CutForSearch("谁知道会所联系方式呢"); // 搜索引擎模式
                Console.WriteLine("【搜索引擎模式】：{0}", string.Join("/ ", segments));
                // timerCls.z_actSj();
                //  timerCls.renqi();
                //     timerCls.z21_yule();
                //     timerCls.zaocan();
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
            var results = (from jo in rows
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
            for (int i = 0; i < 100 * 10000; i++)
            {
                Hashtable ht = new Hashtable();
                ht.Add("key", i);
                ht.Add("value" + i, i);
                data.Add(ht);
            }
            System.IO.File.WriteAllText("db.json", JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented));

            long timestamp_end = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long spantime = (timestamp_end - timestamp);
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



