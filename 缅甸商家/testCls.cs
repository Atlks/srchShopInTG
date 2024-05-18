
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
           timerCls.  xiawucha();

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



