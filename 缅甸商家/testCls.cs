
using JiebaNet.Segmenter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using prj202405.lib;
using static System.Net.Mime.MediaTypeNames;
using prj202405.lib;
using ClosedXML.Excel;
using 缅甸商家.lib;

namespace prj202405
{
    internal class testCls
    {
        internal static void test()
        {

            //export 
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

                //export mercht
                exptMrcht();
                Merchant? merchant = new Merchant();
                merchant.Guid = "123456";
                merchant.Name = "shjjj";
                var text = "pinlunxxxx";
                SortedList pinlunobj = new SortedList();
                pinlunobj.Add("id", DateTime.Now.ToString());
                pinlunobj.Add("商家guid", merchant.Guid);
                pinlunobj.Add("商家", merchant.Name);
                pinlunobj.Add("时间", DateTime.Now.ToString());
                pinlunobj.Add("评论内容", text);

                System.IO.Directory.CreateDirectory("pinlunDir");
                ormSqlt.save(  pinlunobj, "pinlunDir/" + merchant.Guid + merchant.Name + ".db");             
                ormJSonFL.save(pinlunobj, "pinlunDir/" + merchant.Guid + merchant.Name + ".json");
                ormExcel.save(pinlunobj, "pinlunDir/" + merchant.Guid + merchant.Name + ".xlsx");
                ormIni.save(pinlunobj, "pinlunDir/" + merchant.Guid + merchant.Name + ".ini");
                Console.WriteLine("line1633");

                Console.WriteLine(JsonConvert.SerializeObject(ormIni.qry("pinlunDir/" + merchant.Guid + merchant.Name + ".ini")));



                Console.WriteLine(JsonConvert.SerializeObject(ormExcel.qry("pinlunDir/" + merchant.Guid + merchant.Name + ".xlsx")));


                Console.WriteLine(JsonConvert.SerializeObject(ormJSonFL.qry("pinlunDir/ziluxwubxeaktvrvcmsrryfzrmH13 红楼 一楼 按摩.json")));

            Console.WriteLine(JsonConvert.SerializeObject(ormSqlt.qry("pinlunDir/ziluxwubxeaktvrvcmsrryfzrmH13 红楼 一楼 按摩商家评论表.db")));
                //    ormTest.   testorm();

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

        private static void exptMrcht()
        {

            var citys = (from c in Program._citys select c).ToList();

            foreach (var city in citys)
            {
                System.Collections.SortedList cityMap = corex.ObjectToSortedList(city);
                cityMap.Remove("Address");
                cityMap.Add("cityname", city.Name);
                Console.WriteLine(JsonConvert.SerializeObject(cityMap, Formatting.Indented));
                var addrS = (  from ca in city.Address
                               select ca
                         )
                     .ToList();
                foreach (var addx in addrS)
                {
                    System.Collections.SortedList addMap = corex.ObjectToSortedList(addx);
                    addMap.Remove("Merchant");
                    addMap.Add("parkname", addx.Name);
                    addMap.Add("parkkwd", addx.CityKeywords);
                    Console.WriteLine(JsonConvert.SerializeObject(addMap, Formatting.Indented));
                    var rws = (from m in addx.Merchant
                               select m
                              )
                          .ToList();
                    foreach (var m in rws)
                    {
                        System.Collections.SortedList mcht = corex.ObjectToSortedList(m);
                        mcht.Add("CityKeywords", city.CityKeywords);
                        mcht.Add("cityname", city.Name);
                        mcht.Add("parkname", addx.Name);
                        mcht.Add("parkkwd", addx.CityKeywords);
                        Console.WriteLine(mcht["Category"]);
                        //    mcht.Add("CategoryStr", Program._categoryKeyValue[Convert.ToInt32(mcht["Category"].ToString())]);
                        mcht.Add("CategoryStrKwds", Program._categoryKeyValue[ (int)m.Category]);
                        mcht.Add("cateInt", (int)m.Category);
                        mcht.Add("cateEgls",  m.Category.ToString());
                        //   mcht

                        Console.WriteLine(JsonConvert.SerializeObject(mcht, Formatting.Indented));
                        Console.WriteLine("..");
                    }

                }
            }
              

           
                // orderby am.Views descending
          //  select m,ca
            //count = results.Count;
          
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



