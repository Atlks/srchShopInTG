using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using JiebaNet.Segmenter;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using prj202405;
using prj202405.lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using City = prj202405.City;
using static prj202405.lib.arrCls;//  prj202405.lib
using static prj202405.lib.dbgCls;

namespace prj202504
{
    internal class mrcht
    {
        //city=妙瓦底&park=世纪新城园区
        //public static HashSet<City> qry4byParknameExprs2Dataobj(string uri_Query, string dbf)
        //{
        //    HashSet<City> rowss = new HashSet<City>();
        //    var merchantsStr = System.IO.File.ReadAllText(dbf);
        //    if (!string.IsNullOrEmpty(merchantsStr))
        //        rowss = JsonConvert.DeserializeObject<HashSet<City>>(merchantsStr)!;
        //    //dataObj
        //    Dictionary<string, StringValues> parameters = QueryHelpers.ParseQuery(uri_Query);
        //    string cityName = parameters["city"]; string park = parameters["park"];
        //    //  Program._citys = 
        //    //trimOtherCity(Program._citys, city);

        //    // 使用LINQ查询来查找指定名称的城市
        //    City city1 = rowss.FirstOrDefault(c => c.Name == cityName);

        //    Address park1 = city1.Address.FirstOrDefault(a => a.Name == park);
        //    city1.Address.Clear();
        //    city1.Address.Add(park1);
        //    return rowss;
        //}



        //public static List<InlineKeyboardButton[]> qryByKwd(string keyword)
        //{
        //    var __METHOD__ = MethodBase.GetCurrentMethod().Name;
        //    dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), keyword));
        //    List<InlineKeyboardButton[]> results = [];

        //    if (string.IsNullOrEmpty(keyword))
        //        return [];

        //    keyword = keyword.ToLower().Replace(" ", "").Trim();
        //    var searchChars = keyword!.ToCharArray();

        //    results = (from c in Program._citys
        //               from ca in c.Address
        //               from am in ca.Merchant
        //               where searchChars.All(s => (c.CityKeywords + ca.CityKeywords + am.KeywordString + am.KeywordString + Program._categoryKeyValue[(int)am.Category]).Contains(s))
        //               orderby am.Views descending
        //               select new[] { new InlineKeyboardButton(c.Name + " • " + ca.Name + " • " + am.Name) { CallbackData = $"Merchant?id={am.Guid}" } }).ToList();
        //    //count = results.Count;
        //    foreach (InlineKeyboardButton[] btn in results)
        //    {

        //    }


        //    //          {
        //    //              "text": "妙瓦底 ? 东风园区 ? 东方名剪",
        //    //  "callback_data": "Merchant?id=dfwlvxcahlzudawgoeqjxafkxv"
        //    //}
        //    if (results.Count > 0)
        //        dbgCls.setDbgValRtval(__METHOD__, results[0]);



        //    return results;
        //}


        //   where searchChars.All(s => (
        //   c.CityKeywords + add.CityKeywords +
        //   am.KeywordString + am.KeywordString +
        //   am.Program._categoryKeyValue[(int)am.Category]).Contains(s))
        public static List<InlineKeyboardButton[]> qryFromMrcht(string dbfFrom, Dictionary<string, StringValues> whereExprsObj)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), dbfFrom, whereExprsObj));

            string msgx = whereExprsObj["msgCtain"];
            if (string.IsNullOrEmpty(msgx)) { return []; }
            string[] kwds = strCls.calcKwdsAsArr(ref msgx);
            //Dictionary<string, StringValues> whereExprsObj = new Dictionary<string, StringValues>();
            var rsRztInlnKbdBtn = db.qryV6(dbfFrom, (SortedList row) =>
                 {
                     //if have condit n fuhe condit next...beir skip ( dont have cdi or not eq )
                     if (hasCondt(whereExprsObj, "city"))
                         if (!strCls.eq(row["cityname"], arrCls.TryGetValue(whereExprsObj, "city")))   //  cityname not in (citysss) 
                             return false;
                     if (hasCondt(whereExprsObj, "park"))
                         if (!strCls.eq(row["parkname"], arrCls.TryGetValue(whereExprsObj, "park")))   //  cityname not in (citysss) 
                             return false;
                     if (hasCondt(whereExprsObj, "ctry"))
                         if (!strCls.eq(row["ctry"], arrCls.TryGetValue(whereExprsObj, "ctry")))   //  cityname not in (citysss) 
                             return false;
                     if (arrCls.rowValDefEmpty(row, "cateEgls") == "Property")
                         return false;

                     //if condt  containxx(row,msgSpltKwArr)>0
                     var seasrchKwds = "__citykwds=> " + arrCls.rowValDefEmpty(row, "CityKeywords") +
                       "__pkkwds=> " + arrCls.rowValDefEmpty(row, "parkkwd") +
                        "__mrcht_kwds=> " + arrCls.rowValDefEmpty(row, "KeywordString") +
                        "__mrcht_CategoryStrKwds=> " + arrCls.rowValDefEmpty(row, "CategoryStrKwds");
                     row["_seasrchKw2ds"] = seasrchKwds;

                     int containScore = strCls.containCalcCntScore(seasrchKwds, kwds);
                     if (containScore > 0)
                     {
                         row["_containCntScore"] = containScore;
                         return true;
                     }
                     return false;
                 },
                 (SortedList sl) =>
                 {
                  //   (List<SortedList<string, object>>)
                     return 0 - (int)getHashtableKv( sl, "_containCntScore", 0);                    
                 },
                     (SortedList row) =>
                     {
                         string text = arrCls.rowValDefEmpty(row, "cityname") + " • " + arrCls.rowValDefEmpty(row, "parkname") + " • " + arrCls.rowValDefEmpty(row, "Name");
                         string guid = arrCls.rowValDefEmpty(row, "Guid");
                         InlineKeyboardButton[] btnsInLine = new[] { new InlineKeyboardButton(text) { CallbackData = $"Merchant?id={guid}" } };
                         return btnsInLine;
                     }
                );
            //end fun
            dbgCls.setDbgValRtval(MethodBase.GetCurrentMethod().Name, array_slice<InlineKeyboardButton[]>(rsRztInlnKbdBtn, 0, 3));
            return rsRztInlnKbdBtn;
        }

        // seelct from dbfform patn(111) wehre xxxx   sekect  List<InlineKeyboardButton[
        public static List<InlineKeyboardButton[]> qryByMsgKwdsV3(string dbfFrom, Dictionary<string, StringValues> whereExprsObj)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), dbfFrom, whereExprsObj));

            //----------kwds splt
            string msgx = whereExprsObj["msgCtain"];
            string[] kwds = strCls.calcKwdsAsArr(ref msgx);

            ArrayList rows_rzt4srch = [];
            List<SortedList> rows = ormJSonFL.qry(dbfFrom);

            //------------------------------- from xx where city=xx and park=xx and  containxx(row,msgSpltKwArr)>0
            foreach (SortedList row in rows)
            {

                //if have condit n fuhe condit next...beir skip ( dont have cdi or not eq )
                if (hasCondt(whereExprsObj, "city"))
                    if (!strCls.eq(row["cityname"], arrCls.TryGetValue(whereExprsObj, "city")))   //  cityname not in (citysss) 
                        continue;  //skip
                if (hasCondt(whereExprsObj, "park"))
                    if (!strCls.eq(row["parkname"], arrCls.TryGetValue(whereExprsObj, "park")))   //  cityname not in (citysss) 
                        continue;  //skip
                if (hasCondt(whereExprsObj, "ctry"))
                    if (!strCls.eq(row["ctry"], arrCls.TryGetValue(whereExprsObj, "ctry")))   //  cityname not in (citysss) 
                        continue;  //skip
                if (arrCls.rowValDefEmpty(row, "cateEgls") == "Property")
                    continue;   //skip


                //if condt  containxx(row,msgSpltKwArr)>0
                var seasrchKwds = "__citykwds=> " + arrCls.rowValDefEmpty(row, "CityKeywords") +
                  "__pkkwds=> " + arrCls.rowValDefEmpty(row, "parkkwd") +
                   "__mrcht_kwds=> " + arrCls.rowValDefEmpty(row, "KeywordString") +
                   "__mrcht_CategoryStrKwds=> " + arrCls.rowValDefEmpty(row, "CategoryStrKwds");
                row["_seasrchKw2ds"] = seasrchKwds;

                int containScore = strCls.containCalcCntScore(seasrchKwds, kwds);
                if (containScore > 0)
                {
                    row["_containCntScore"] = containScore;
                    rows_rzt4srch.Add(row);
                }
                //  遍历一个大概40ms   case trycat 模式，给为if else 模式，立马变为1ms
                // Console.WriteLine(DateTime.Now.ToString("yyyyMMdd_HHmmss_fff"));  
            }
            const string dbgFlDir = "rows_rzt4srchDirdbg";
            dbgooutput(rows_rzt4srch, dbgFlDir);


            //--------------------order prcs
            // 使用 LINQ 对 ArrayList 进行排序
            Func<SortedList, int> keySelector = sl => (int)sl["_containCntScore"];
            List<SortedList> list = rows_rzt4srch.Cast<SortedList>()
                                      .OrderBy(keySelector)
                                      .ToList();


            //-------------------------map select prcs
            List<InlineKeyboardButton[]> rsRztInlnKbdBtn = [];
            for (int i = 0; i < rows_rzt4srch.Count; i++)
            {
                SortedList row = list[i];
                string text = arrCls.rowValDefEmpty(row, "cityname") + " • " + arrCls.rowValDefEmpty(row, "parkname") + " • " + arrCls.rowValDefEmpty(row, "Name");
                string guid = arrCls.rowValDefEmpty(row, "Guid");
                InlineKeyboardButton[] btnsInLine = new[] { new InlineKeyboardButton(text) { CallbackData = $"Merchant?id={guid}" } };
                rsRztInlnKbdBtn.Add(btnsInLine);
            }
            //count = re
            dbgCls.setDbgValRtval(MethodBase.GetCurrentMethod().Name, array_slice<InlineKeyboardButton[]>(rsRztInlnKbdBtn, 0, 3));



            return rsRztInlnKbdBtn;
            //    List<InlineKeyboardButton[]> results22 = arrCls.rdmList<InlineKeyboardButton[]>(results);
            //  results22 = results22.Skip(0 * 10).Take(5).ToList();
        }


        //public static List<InlineKeyboardButton[]> qryByMsgKwdsV2(string msg, string whereExprs, string dbf)
        //{
        //    var __METHOD__ = MethodBase.GetCurrentMethod().Name;
        //    dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), msg, whereExprs, dbf));

        //    //   where searchChars.All(s => (
        //    //   c.CityKeywords + add.CityKeywords +
        //    //   am.KeywordString + am.KeywordString +
        //    //   am.Program._categoryKeyValue[(int)am.Category]).Contains(s))

        //    //联系商家城市
        //    HashSet<City> _citys = [];
        //    var merchants = System.IO.File.ReadAllText(dbf);
        //    if (!string.IsNullOrEmpty(merchants))
        //        _citys = JsonConvert.DeserializeObject<HashSet<City>>(merchants)!;


        //    //----------kwds splt
        //    msg = ChineseCharacterConvert.Convert.ToSimple(msg);
        //    var segmenter = new JiebaSegmenter();
        //    segmenter.LoadUserDict("user_dict.txt");
        //    segmenter.AddWord("会所"); // 可添加一个新词
        //    segmenter.AddWord("妙瓦底"); // 可添加一个新词
        //    var segments = segmenter.CutForSearch(msg); // 搜索引擎模式
        //    Console.WriteLine("【搜索引擎模式】：{0}", string.Join("/ ", segments));


        //    ArrayList rows_rzt4srch = [];
        //    ArrayList rows = cvt2IniRowMode(_citys);

        //    //SetIdProperties(rows);
        //    //ormJSonFL.saveMlt(rows,"mrcht.json");
        //    //dataObj
        //    Dictionary<string, StringValues> whereExprsObj = QueryHelpers.ParseQuery(whereExprs);

        //    //------------------------------- from xx where city=xx and park=xx and  containxx(row,msgSpltKwArr)>0
        //    foreach (SortedList row in rows)
        //    {

        //        //if have condit n fuhe condit next...beir skip ( dont have cdi or not eq )
        //        if (hasCondt(whereExprsObj, "city"))
        //            if (!strCls.eq(row["cityname"], arrCls.TryGetValue(whereExprsObj, "city")))   //  cityname not in (citysss) 
        //                continue;  //skip
        //        if (hasCondt(whereExprsObj, "park"))
        //            if (!strCls.eq(row["parkname"], arrCls.TryGetValue(whereExprsObj, "park")))   //  cityname not in (citysss) 
        //                continue;  //skip
        //        if (arrCls.rowValDefEmpty(row, "cateEgls") == "Property")
        //            continue;   //skip


        //        //if condt  containxx(row,msgSpltKwArr)>0
        //        var seasrchKwds = "__citykwds=> " + arrCls.rowValDefEmpty(row, "CityKeywords") +
        //          "__pkkwds=> " + arrCls.rowValDefEmpty(row, "parkkwd") +
        //           "__mrcht_kwds=> " + arrCls.rowValDefEmpty(row, "KeywordString") +
        //           "__mrcht_CategoryStrKwds=> " + arrCls.rowValDefEmpty(row, "CategoryStrKwds");
        //        row["_seasrchKw2ds"] = seasrchKwds;

        //        int containScore = strCls.containCalcCntScore(seasrchKwds, segments);
        //        row["_containCntScore"] = containScore;
        //        if (containScore > 0)
        //            rows_rzt4srch.Add(row);
        //    }
        //    const string dbgFl = "rows_rzt4srchDirdbg";
        //    dbgooutput(rows_rzt4srch, dbgFl);


        //    //--------------------order prcs
        //    // 使用 LINQ 对 ArrayList 进行排序
        //    List<SortedList> list = rows_rzt4srch.Cast<SortedList>()
        //                              .OrderByDescending(sl => (int)sl["_containCntScore"])
        //                              .ToList();


        //    //-------------------------map select prcs
        //    List<InlineKeyboardButton[]> rsRztInlnKbdBtn = [];
        //    for (int i = 0; i < rows_rzt4srch.Count; i++)
        //    {
        //        SortedList row = list[i];
        //        string text = arrCls.rowValDefEmpty(row, "cityname") + " • " + arrCls.rowValDefEmpty(row, "parkname") + " • " + arrCls.rowValDefEmpty(row, "Name");
        //        string guid = arrCls.rowValDefEmpty(row, "Guid");
        //        InlineKeyboardButton[] btnsInLine = new[] { new InlineKeyboardButton(text) { CallbackData = $"Merchant?id={guid}" } };
        //        rsRztInlnKbdBtn.Add(btnsInLine);
        //    }
        //    //count = re
        //    dbgCls.setDbgValRtval(MethodBase.GetCurrentMethod().Name, array_slice<InlineKeyboardButton[]>(rsRztInlnKbdBtn, 0, 3));



        //    return rsRztInlnKbdBtn;
        //    //    List<InlineKeyboardButton[]> results22 = arrCls.rdmList<InlineKeyboardButton[]>(results);

        //    //  results22 = results22.Skip(0 * 10).Take(5).ToList();
        //}


        static void SetIdProperties(ArrayList arrayList)
        {
            foreach (var item in arrayList)
            {
                SortedList sortedList1 = (SortedList)item;
                sortedList1.Add("id", sortedList1["Guid"]);

            }
        }

        private static bool hasCondt(Dictionary<string, StringValues> whereExprsObj, string v)
        {
            string park4srch = arrCls.TryGetValue(whereExprsObj, v); ;

            if (park4srch == null)
            {
                return false;
            }
            return true;
        }

        private static void dbgooutput(ArrayList rows_rzt4srch, string dbgFl)
        {
            var updateString = JsonConvert.SerializeObject(rows_rzt4srch, Formatting.Indented);

            Directory.CreateDirectory(dbgFl);
            //    Console.WriteLine(updateString);
            // 获取当前时间并格式化为文件名
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            string fileName = $"{dbgFl}/{timestamp}.txt";
            //      Console.WriteLine(fileName);
            System.IO.File.WriteAllText("" + fileName, updateString);
        }

        //private static ArrayList cvt2IniRowMode(HashSet<City> citysobj)
        //{
        //    ArrayList a = new System.Collections.ArrayList();
        //    var citys = (from c in citysobj select c).ToList();

        //    foreach (var city in citys)
        //    {
        //        System.Collections.SortedList cityMap = corex.ObjectToSortedList(city);
        //        cityMap.Remove("Address");
        //        cityMap.Add("cityname", city.Name);
        //        //    Console.WriteLine(JsonConvert.SerializeObject(cityMap, Formatting.Indented));
        //        var addrS = (from ca in city.Address
        //                     select ca
        //                 )
        //             .ToList();
        //        foreach (var addx_park in addrS)
        //        {
        //            System.Collections.SortedList addMap = corex.ObjectToSortedList(addx_park);
        //            addMap.Remove("Merchant");
        //            addMap.Add("parkname", addx_park.Name);
        //            addMap.Add("parkkwd", addx_park.CityKeywords);
        //            //     Console.WriteLine(JsonConvert.SerializeObject(addMap, Formatting.Indented));
        //            var rws = (from m in addx_park.Merchant
        //                       select m
        //                      )
        //                  .ToList();
        //            foreach (var m in rws)
        //            {
        //                System.Collections.SortedList mcht = corex.ObjectToSortedList(m);
        //                mcht.Add("CityKeywords", city.CityKeywords);
        //                mcht.Add("cityname", city.Name);
        //                mcht.Add("parkname", addx_park.Name);
        //                mcht.Add("parkkwd", addx_park.CityKeywords);
        //                //   Console.WriteLine(mcht["Category"]);
        //                //    mcht.Add("CategoryStr", Program._categoryKeyValue[Convert.ToInt32(mcht["Category"].ToString())]);
        //                mcht.Add("CategoryStrKwds", Program._categoryKeyValue[(int)m.Category]);
        //                mcht.Add("cateInt", (int)m.Category);
        //                mcht.Add("cateEgls", m.Category.ToString());
        //                //   mcht

        //                mcht.Add("_parent", city.Name + "_" + addx_park.Name);
        //                a.Add(mcht);
        //                //   Console.WriteLine(JsonConvert.SerializeObject(mcht, Formatting.Indented));
        //                //  Console.WriteLine("..");
        //            }

        //        }
        //    }


        //    return a;
        //    // orderby am.Views descending
        //    //  select m,ca
        //    //count = results.Count;

        //}


        //public static List<InlineKeyboardButton[]> ordRztByOrdtbl(List<InlineKeyboardButton[]> rows_rzt, SortedList<string, int> ordermap)
        //{

        //    // 对 List<InlineKeyboardButton[]> 进行排序
        //    rows_rzt.Sort((a, b) =>
        //    {
        //        try
        //        {
        //            // 获取每个数组的第一个按钮的 Text 属性
        //            InlineKeyboardButton abtn = a[0];
        //            int ord1 = ordermap[abtn.CallbackData];

        //            InlineKeyboardButton bbtn = b[0];
        //            int ord2 = ordermap[bbtn.CallbackData];

        //            // 按 Text 属性进行排序
        //            return ord2.CompareTo(ord1);
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.Message);
        //            return 0;
        //        }

        //    });
        //    return rows_rzt;
        //}


        //public static SortedList<string, int> calcOrderMap(List<InlineKeyboardButton[]> rows_rzt)
        //{
        //    SortedList<string, int> ordMap = new SortedList<string, int>();
        //    foreach (InlineKeyboardButton[] row in rows_rzt)
        //    {
        //        try
        //        {
        //            InlineKeyboardButton btn = row[0];
        //            arrCls.saveIncrs(ordMap, btn.CallbackData);
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.Message);
        //        }

        //    }
        //    return ordMap;
        //}
        //private static HashSet<City> qryByPknm(HashSet<City> citys, string? city, string? park)
        //{
        //    City city1 = FindCityByName(citys, city);
        //    Address park1 = city1.Address.FirstOrDefault(a => a.Name == park);
        //    city1.Address.Clear();
        //    city1.Address.Add(park1);
        //    return citys;
        //}

        //static City FindCityByName(HashSet<City> cities, string name)
        //{
        //    // 使用LINQ查询来查找指定名称的城市
        //    return cities.FirstOrDefault(c => c.Name == name);
        //}

        //private static HashSet<City> trimOtherCity(HashSet<City> citys, string? city)
        //{
        //    HashSet<City> rmvCitys = new HashSet<City>();
        //    foreach (City city1 in citys)
        //    {
        //        if (city1.Name != city)
        //            rmvCitys.Add(city1);
        //    }

        //    // 迭代并移除已知的值
        //    foreach (City city1 in rmvCitys)
        //    {
        //        citys.Remove(city1);
        //    }
        //    return citys;

        //}
    }
}
