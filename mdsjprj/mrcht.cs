global using static mdsj.mrcht;
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
using static prj202405.lib.arrCls;//  prj202405.lib
using static prj202405.lib.dbgCls;
using static mdsj.lib.logCls;
using static prj202405.lib.corex;
using static prj202405.lib.db;
using static prj202405.lib.filex;
using static prj202405.lib.ormJSonFL;
using static prj202405.lib.strCls;
using static mdsj.lib.encdCls;
using static mdsj.lib.net_http;
using static mdsj.libBiz.strBiz;
using static mdsj.libBiz.tgBiz;
using static prj202405.lib.strCls;
using static libx.qryEngrParser;
using static libx.storeEngr4Nodesqlt;

using Microsoft.CodeAnalysis.CSharp.Syntax;
using libx;
namespace mdsj
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
        //todo sharnames
        public static List<InlineKeyboardButton[]> qryFromMrcht(string dbFrom, string shareNames, Dictionary<string, StringValues> whereExprsObj, string msgCtain)
        {


            msgCtain = msgCtain.ToUpper();
            msgCtain = ChineseCharacterConvert.Convert.ToSimple(msgCtain);
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbg_setDbgFunEnter(__METHOD__, func_get_args(dbFrom, shareNames, whereExprsObj, msgCtain));

            //  string msgx = whereExprsObj["msgCtain"];
            if (string.IsNullOrEmpty(msgCtain)) { return []; }
            string[] kwds = strCls.splt_by_fenci(ref msgCtain);
            kwds = RemoveShortWords(kwds);
            //todo 去除触发词，，只保留 服务次和位置词
            //园区
            kwds = removeStopWd4biz(kwds);
            Console.WriteLine(" now kwd for srarch is =>" + string.Join(" ", kwds));


            //c----calc fuwuci 
            HashSet<string> 商品与服务词库 = file_getWords商品与服务词库();
            string fuwuci = substr_getFuwuci(msgCtain, 商品与服务词库);

            //   var patns_dbfs = db.calcPatns("mercht商家数据", arrCls.ldfld_TryGetValue(whereExprsObj, "@file"));

            HashSet<string> postnKywd位置词set = getPostnWds(dbFrom, shareNames, whereExprsObj);
            bool msgHasPostWd = isMmsgHasMatchPostWd(postnKywd位置词set, kwds);
            // weizhici = guiyihuaWeizhici(weizhici);
            //Dictionary<string, StringValues> whereExprsObj = new Dictionary<string, StringValues>();

           // Func<SortedList, bool> whereFun22 = filtrList2whereFun111();
            Func<SortedList, bool> whereFun = (SortedList row) =>
            {

                List<Filtr> li = new List<Filtr>();
                li.Add(new Filtr(isNotEmptyLianxi(row)));
                //li.Add(new Condtn(isLianxifshValid(row)));
                //li.Add(new Condtn(isFldValEq111(row, "城市", whereExprsObj)));
                //li.Add(new Condtn(isFldValEq111(row, "园区", whereExprsObj)));
                //li.Add(new Condtn(isFldValEq111(row, "国家", whereExprsObj)));
                //li.Add(new Condtn(isCotainFuwuci(row, msgCtain)));
                //li.Add(new Condtn(msgHasPostWd && isCotainPostnWd(row, kwds)));
                if (!ChkAllFltrTrue(li))
                    return false;



                //--------------is lianixfsh empty
                if (isEmptyLianxi(row)) return false;
                if (!isLianxifshValid(row))
                    return false;

                //if have condit n fuhe condit next...beir skip ( dont have cdi or not eq )
                if (hasCondt(whereExprsObj, "城市"))
                    if (!strCls.eq(row["城市"], arrCls.ldfld_TryGetValue(whereExprsObj, "城市")))   //  cityname not in (citysss) 
                        return false;
                if (hasCondt(whereExprsObj, "园区"))
                    if (!strCls.eq(row["园区"], arrCls.ldfld_TryGetValue(whereExprsObj, "园区")))   //  cityname not in (citysss) 
                        return false;
                if (hasCondt(whereExprsObj, "国家"))
                    if (!strCls.eq(row["国家"], arrCls.ldfld_TryGetValue(whereExprsObj, "国家")))   //  cityname not in (citysss) 
                        return false;
                //  if (arrCls.ldFldDefEmpty(row, "cateEgls") == "Property")
                //     return false;



                //-------------fuwuci panduan

                if (!isCotainFuwuci(row, msgCtain))
                    return false;
                int containScore = 0;

                //-------------weizhi condt

                //todo use udf
                //if has fuwuWds and weizhici empty
                if (msgHasPostWd)
                {
                    if (!isCotainPostnWd(row, kwds))
                        return false;

                    //zheg个用来排序的，放在order上。。
                    containScore = containCalcCntScoreSetfmt1(row, kwds);
                    if (containScore > 0)
                    {
                        row["_containCntScore"] = containScore;
                    }
                }
                return true;
            };

            //dbFrom sharesss
            var rsRztInlnKbdBtn = Qe_qryV2(
                "mercht商家数据", "",
                whereFun, (SortedList sl) =>
                {
                    //zheg个用来排序的，放在order上。。
                    //containScore = containCalcCntScoreSetfmt1(row, kwds);
                    //if (containScore > 0)
                    //{
                    //    row["_containCntScore"] = containScore;
                    //}
                    //   (List<SortedList<string, object>>)
                    return 0 - (int)ldfld(sl, "_containCntScore", 0);
                }, (SortedList row) =>
                {
                    string text = arrCls.ldFldDefEmpty(row, "城市") + " • " + arrCls.ldFldDefEmpty(row, "园区") + " • " + arrCls.ldFldDefEmpty(row, "商家");
                    string guid = arrCls.ldFldDefEmpty(row, "Guid编号");
                    InlineKeyboardButton[] btnsInLine = new[] { new InlineKeyboardButton(text) { CallbackData = $"id={guid}&chkuid=y&btn=dtl" } };
                    return btnsInLine;
                }, rnd_next4SqltRf());
            //end fun
            dbgCls.dbg_setDbgValRtval(MethodBase.GetCurrentMethod().Name, array_slice<InlineKeyboardButton[]>(rsRztInlnKbdBtn, 0, 3));
            return rsRztInlnKbdBtn;
        }

        //private static Func<SortedList, bool> filtrList2whereFun111()
        //{

        //    List<Filtr> li = new List<Filtr>();
        //    li.Add(new Filtr(isNotEmptyLianxi(row)));

        //    Func<SortedList, bool> whereFun = castFltlst2whereFun(li);
        //}



            public static bool isFldValEq111(SortedList row, string Fld, Dictionary<string, StringValues> whereExprsObj)
            {
                //  string Fld = "城市";
                if (hasCondt(whereExprsObj, Fld))
                    if (!strCls.eq(row[Fld], arrCls.ldfld_TryGetValue(whereExprsObj, Fld)))   //  cityname not in (citysss) 
                        return false;

                return true;
            }

            //private static bool isFldValEq(SortedList row, string v1, string v2)
            //{
            //    throw new NotImplementedException();
            //}

            public static bool isNotEmptyLianxi(SortedList row)
            {
                return !isEmptyLianxi(row);
            }

            private static bool isLianxifshValid(SortedList row)
            {
                // string Fld = ;
                if (getFldLianxifs(row, "Whatsapp") != "")
                    return true;
                if (getFldLianxifs(row, "微信") != "")
                    return true;
                if (getFldLianxifs(row, "Telegram") != "")
                    if (ldFldDefEmpty(row, "TG有效") == "N")// only tg and tg not vld
                        return false;
                    else
                        return true;
                return false;

            }

            private static string getFldLianxifs(SortedList row, string Fld)
            {
                // const string Fld = "Telegram";
                return trim_RemoveUnnecessaryCharacters4tgWhtapExt(ldFldDefEmpty(row, Fld));
            }

            private static bool isEmptyLianxi(SortedList row)
            {
                string lianxifsh = getLianxifsh(row);
                if (lianxifsh == "")
                    return true;
                return false;
            }

            private static bool isCotainFuwuci(SortedList row, string msgCtain)
            {
                HashSet<string> fuwuWds_row = new HashSet<string>();
                arrCls.add_elmts2hsst(fuwuWds_row, arrCls.ldFldDefEmpty(row, "商家"));
                arrCls.add_elmts2hsst(fuwuWds_row, arrCls.ldFldDefEmpty(row, "关键词"));
                arrCls.add_elmts2hsst(fuwuWds_row, arrCls.ldFldDefEmpty(row, "分类关键词"));
                fuwuWds_row.Remove("店");
                fuwuWds_row = ConvertToUpperCase(fuwuWds_row);

                if (strCls.containKwds(msgCtain, fuwuWds_row))
                    return true;
                return false;
            }

            private static int containCalcCntScoreSetfmt1(SortedList row, string[] kwds)
            {
                HashSet<string> curRowKywdSset = new HashSet<string>();

                add_elmts2hsst(curRowKywdSset, ldFldDefEmpty(row, "国家"));
                add_elmts2hsst(curRowKywdSset, ldFldDefEmpty(row, "城市"));
                add_elmts2hsst(curRowKywdSset, ldFldDefEmpty(row, "园区"));
                add_elmts2hsst(curRowKywdSset, ldFldDefEmpty(row, "城市关键词"));
                add_elmts2hsst(curRowKywdSset, ldFldDefEmpty(row, "园区关键词"));
                return containCalcCntScoreSetfmt(ConvertToUpperCase(curRowKywdSset), kwds);
            }

            private static bool isCotainPostnWd(SortedList row, string[] kwds)
            {
                HashSet<string> curRowKywdSset = new HashSet<string>();

                add_elmts2hsst(curRowKywdSset, ldFldDefEmpty(row, "国家"));
                add_elmts2hsst(curRowKywdSset, ldFldDefEmpty(row, "城市"));
                add_elmts2hsst(curRowKywdSset, ldFldDefEmpty(row, "园区"));
                add_elmts2hsst(curRowKywdSset, ldFldDefEmpty(row, "城市关键词"));
                add_elmts2hsst(curRowKywdSset, ldFldDefEmpty(row, "园区关键词"));
                curRowKywdSset = ConvertToUpperCase(curRowKywdSset);
                Console.WriteLine(" curRw_posnSet=>" + String.Join(" ", curRowKywdSset));
                // Console.WriteLine(" weizhici=>" + weizhici);
                return isCcontainKwds42(curRowKywdSset, kwds);
            }



            public static HashSet<string> getPostnWds(string dbFrom, string shareNames, Dictionary<string, StringValues> whereExprsObj)
            {

                Func<SortedList, bool> whereFun = (SortedList row) =>
                {

                    try
                    {

                        //if have condit n fuhe condit next...beir skip ( dont have cdi or not eq )
                        if (hasCondt(whereExprsObj, "城市"))
                            if (!strCls.eq(row["城市"], arrCls.ldfld_TryGetValue(whereExprsObj, "城市")))   //  cityname not in (citysss) 
                                return false;
                        if (hasCondt(whereExprsObj, "园区"))
                            if (!strCls.eq(row["园区"], arrCls.ldfld_TryGetValue(whereExprsObj, "园区")))   //  cityname not in (citysss) 
                                return false;
                        if (hasCondt(whereExprsObj, "国家"))
                            if (!strCls.eq(row["国家"], arrCls.ldfld_TryGetValue(whereExprsObj, "国家")))   //  cityname not in (citysss) 
                                return false;
                        return true;
                    }
                    catch (Exception e)
                    {

                    }
                    return false;
                };

                List<string> rsRztInlnKbdBtn = Qe_qryV2(
                      dbFrom, shareNames,
                      whereFun, null, (SortedList row) =>
                      {
                          HashSet<string> curRowKywdSset = new HashSet<string>();

                          add_elmts2hsst(curRowKywdSset, ldFldDefEmpty(row, "国家"));
                          add_elmts2hsst(curRowKywdSset, ldFldDefEmpty(row, "城市"));
                          add_elmts2hsst(curRowKywdSset, ldFldDefEmpty(row, "园区"));
                          add_elmts2hsst(curRowKywdSset, ldFldDefEmpty(row, "城市关键词"));
                          add_elmts2hsst(curRowKywdSset, ldFldDefEmpty(row, "园区关键词"));
                          curRowKywdSset = ConvertToUpperCase(curRowKywdSset);
                          return String.Join(" ", curRowKywdSset);
                      }, rnd_next4SqltRf());

                HashSet<string> hs = new HashSet<string>();
                foreach (string pstWds in rsRztInlnKbdBtn)
                {
                    string[] wds = pstWds.Split(' ');
                    HashSet<string> hsTmp = castArr2set(wds);
                    hs = array_merge(hs, hsTmp);
                }

                //   var patns_dbfs = db.calcPatns(dbFrom, shareNames);

                HashSet<string> hashSet = ReadLinesToHashSet("位置词.txt");
                HashSet<string> hashSet1 = array_merge(hs, hashSet);

                HashSet<string> hashSet2 = ConvertToUpperCase(hashSet1);
                hashSet2 = arr_remove(hashSet2, "市区 城市 国家 园区名称 城市名称 国家词 ");

                // 使用 LINQ 过滤非空元素并创建新的 HashSet<string>
                HashSet<string> filteredSet = new HashSet<string>(
                    hashSet2.Where(s => !string.IsNullOrWhiteSpace(s))
                );
                return filteredSet;
            }

            private static HashSet<string> arr_remove(HashSet<string> hashSet2, string v)
            {
                string[] a = v.Split(" ");
                foreach (string wd in a)
                {
                    hashSet2.Remove(wd);
                }

                return hashSet2;
            }



            public static string getLianxifsh(SortedList row)
            {
                string lianxifsh = ldFldDefEmpty(row, "Telegram") + ldFldDefEmpty(row, "WhatsApp")

                 + ldFldDefEmpty(row, "微信") + ldFldDefEmpty(row, "Tel")
                  + ldFldDefEmpty(row, "Line");
                lianxifsh = trim_RemoveUnnecessaryCharacters4tgWhtapExt(lianxifsh);
                lianxifsh = lianxifsh.Trim();
                return lianxifsh;
            }

            //private static string guiyihuaWeizhici(string weizhici)
            //{
            //    SortedList<string, string> sortedList = new SortedList<string, string>();
            //    sortedList.Add("kk", "kk园区"); try
            //    {
            //        return sortedList[weizhici].Trim().ToLower();
            //    }
            //    catch (Exception e)
            //    {
            //        return weizhici;
            //    }

            //}

            //、、o 去除触发词，，只保留 服务次和位置词



            //private static string getWeizhici(HashSet<string> postnKywd位置词set, string[] kwds)
            //{
            //    //if (text == null)
            //    //    return null;
            //    string[] spltWds = kwds;
            //    foreach (string wd in spltWds)
            //    {
            //        if (postnKywd位置词set.Contains(wd))
            //            return wd;
            //    }
            //    return null;
            //}

            // seelct from dbfform patn(111) wehre xxxx   sekect  List<InlineKeyboardButton[
            //public static List<InlineKeyboardButton[]> qryByMsgKwdsV3dep(string dbfFrom, Dictionary<string, StringValues> whereExprsObj)
            //{
            //    var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            //    dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), dbfFrom, whereExprsObj));

            //    //----------kwds splt
            //    string msgx = whereExprsObj["msgCtain"];
            //    string[] kwds = strCls.calcKwdsAsArr(ref msgx);

            //    ArrayList rows_rzt4srch = [];
            //    List<SortedList> rows = ormJSonFL.qry(dbfFrom);

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
            //        if (hasCondt(whereExprsObj, "ctry"))
            //            if (!strCls.eq(row["ctry"], arrCls.TryGetValue(whereExprsObj, "ctry")))   //  cityname not in (citysss) 
            //                continue;  //skip
            //        if (arrCls.rowValDefEmpty(row, "cateEgls") == "Property")
            //            continue;   //skip


            //        //if condt  containxx(row,msgSpltKwArr)>0
            //        var seasrchKwds = "__citykwds=> " + arrCls.rowValDefEmpty(row, "CityKeywords") +
            //          "__pkkwds=> " + arrCls.rowValDefEmpty(row, "parkkwd") +
            //           "__mrcht_kwds=> " + arrCls.rowValDefEmpty(row, "KeywordString") +
            //           "__mrcht_CategoryStrKwds=> " + arrCls.rowValDefEmpty(row, "CategoryStrKwds");
            //        row["_seasrchKw2ds"] = seasrchKwds;

            //        int containScore = strCls.containCalcCntScore(seasrchKwds, kwds);
            //        if (containScore > 0)
            //        {
            //            row["_containCntScore"] = containScore;
            //            rows_rzt4srch.Add(row);
            //        }
            //        //  遍历一个大概40ms   case trycat 模式，给为if else 模式，立马变为1ms
            //        // Console.WriteLine(DateTime.Now.ToString("yyyyMMdd_HHmmss_fff"));  
            //    }
            //    const string dbgFlDir = "rows_rzt4srchDirdbg";
            //    dbgooutput(rows_rzt4srch, dbgFlDir);


            //    //--------------------order prcs
            //    // 使用 LINQ 对 ArrayList 进行排序
            //    Func<SortedList, int> keySelector = sl => (int)sl["_containCntScore"];
            //    List<SortedList> list = rows_rzt4srch.Cast<SortedList>()
            //                              .OrderBy(keySelector)
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




            //public static bool hasCondt(Dictionary<string, StringValues> whereExprsObj, string v)
            //{
            //    string park4srch = arrCls.ldfld_TryGetValue(whereExprsObj, v); ;

            //    if (park4srch == null)
            //    {
            //        return false;
            //    }
            //    return true;
            //}

            //private static void dbgooutput(ArrayList rows_rzt4srch, string dbgFl)
            //{
            //    var updateString = JsonConvert.SerializeObject(rows_rzt4srch, Formatting.Indented);

            //    Directory.CreateDirectory(dbgFl);
            //    //    Console.WriteLine(updateString);
            //    // 获取当前时间并格式化为文件名
            //    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            //    string fileName = $"{dbgFl}/{timestamp}.txt";
            //    //      Console.WriteLine(fileName);
            //    System.IO.File.WriteAllText("" + fileName, updateString);
            //}

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

