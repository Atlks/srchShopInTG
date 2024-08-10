global using static mdsj.mrcht;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using JiebaNet.Segmenter;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using prjx;
using prjx.lib;
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
using City = prjx.City;
using static prjx.lib.arrCls;//  prj202405.lib
using static prjx.lib.dbgCls;
using static prjx.lib.arrCls;//  prj202405.lib
using static prjx.lib.dbgCls;
using static mdsj.lib.logCls;
using static prjx.lib.corex;
using static prjx.lib.db;
using static prjx.lib.filex;
using static prjx.lib.ormJSonFL;
using static prjx.lib.strCls;
using static mdsj.lib.bscEncdCls;
using static mdsj.lib.net_http;
using static mdsj.libBiz.strBiz;
using static mdsj.libBiz.tgBiz;
using static prjx.lib.strCls;
using static libx.qryEngrParser;
using static libx.storeEngr4Nodesqlt;

using Microsoft.CodeAnalysis.CSharp.Syntax;
using libx;
using System.Drawing.Printing;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Web;
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
        public static List<InlineKeyboardButton[]> qryFromMrcht(string dbFrom, string shareNames, Dictionary<string, StringValues> filters, string msgCtain_msgx_remvTrigWd2)
        {


            msgCtain_msgx_remvTrigWd2 = msgCtain_msgx_remvTrigWd2.ToUpper();
            msgCtain_msgx_remvTrigWd2 = ChineseCharacterConvert.Convert.ToSimple(msgCtain_msgx_remvTrigWd2);
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            PrintCallFunArgs(__METHOD__, func_get_args(dbFrom, shareNames, filters, msgCtain_msgx_remvTrigWd2));

            //  string msgx = whereExprsObj["msgCtain"];
            if (string.IsNullOrEmpty(msgCtain_msgx_remvTrigWd2)) { return []; }
            string[] kwds = strCls.SpltByFenci(ref msgCtain_msgx_remvTrigWd2);
            kwds = RemoveShortWords(kwds);
            //todo 去除触发词，，只保留 服务次和位置词
            //园区
            kwds = removeStopWd4biz(kwds);
            Print(" now kwd for srarch is =>" + string.Join(" ", kwds));


            //c----calc fuwuci 
            HashSet<string> 商品与服务词库 = file_getWords商品与服务词库();
            string fuwuci = substr_getFuwuci(msgCtain_msgx_remvTrigWd2, 商品与服务词库);
            print_varDump(__METHOD__, "fuwuci", fuwuci);
            //   var patns_dbfs = db.calcPatns("mercht商家数据", arrCls.ldfld_TryGetValue(whereExprsObj, "@file"));


            //-----------------calc weizhici-----
            Print("--------------qry weizhici--------");
            HashSet<string> postnKywd位置词set = qry_getPostnWds(dbFrom, shareNames, filters);
            bool msgHasPostWd = isMmsgHasMatchPostWd(postnKywd位置词set, kwds);
            print_varDump(__METHOD__, "msgHasPostWd", msgHasPostWd);
            Print("--------------qry weizhici finish--------");

            // weizhici = guiyihuaWeizhici(weizhici);
            //Dictionary<string, StringValues> whereExprsObj = new Dictionary<string, StringValues>();

            // Func<SortedList, bool> whereFun22 = filtrList2whereFun111();
            Func<SortedList, bool> whereFun = (SortedList row) =>
            {
                if (row["园区"].ToString().Contains("东风"))
                    Print("dbg2445");
                List<Filtr> li = new List<Filtr>();
                li.Add(new Filtr(IsNotEmptyLianxi(row)));
                li.Add(new Filtr(isLianxifshValid(row)));
                li.Add(new Filtr(isFldValEq111(row, "城市", filters)));
                li.Add(new Filtr(isFldValEq111(row, "园区", filters)));
                li.Add(new Filtr(isFldValEq111(row, "国家", filters)));
                //li.Add(new Condtn(isCotainFuwuci(row, msgCtain)));
                //li.Add(new Condtn(msgHasPostWd && isCotainPostnWd(row, kwds)));
                if (!ChkAllFltrTrueDep(li))
                    return false;



                //--------------is lianixfsh empty
                if (isEmptyLianxi(row)) return false;
                if (!isLianxifshValid(row))
                    return false;



                //if have condit n fuhe condit next...beir skip ( dont have cdi or not eq )
                //if (hasCondt(whereExprsObj, "城市"))
                //    if (!strCls.eq(row["城市"], arrCls.ldfld_TryGetValue(whereExprsObj, "城市")))   //  cityname not in (citysss) 
                //        return false;
                //if (hasCondt(whereExprsObj, "园区"))
                //    if (!strCls.eq(row["园区"], arrCls.ldfld_TryGetValue(whereExprsObj, "园区")))   //  cityname not in (citysss) 
                //        return false;
                //if (hasCondt(whereExprsObj, "国家"))
                //    if (!strCls.eq(row["国家"], arrCls.ldfld_TryGetValue(whereExprsObj, "国家")))   //  cityname not in (citysss) 
                //        return false;
                //  if (arrCls.ldFldDefEmpty(row, "cateEgls") == "Property")
                //     return false;



                //-------------fuwuci panduan

                if (!isCotainFuwuci(row, msgCtain_msgx_remvTrigWd2))
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
                "mercht商家数据", shareNames,
                whereFun, (SortedList sl) =>
                {
                    //zheg个用来排序的，放在order上。。
                    //containScore = containCalcCntScoreSetfmt1(row, kwds);
                    //if (containScore > 0)
                    //{
                    //    row["_containCntScore"] = containScore;
                    //}
                    //   (List<SortedList<string, object>>)
                    return 0 - (int)LoadField(sl, "_containCntScore", 0);
                }, (SortedList row) =>
                {
                    string text = LoadFieldDefEmpty(row, "城市") + " • " + LoadFieldDefEmpty(row, "园区") + " • " + LoadFieldDefEmpty(row, "商家");
                    string guid = LoadFieldDefEmpty(row, "Guid编号");
                    InlineKeyboardButton[] btnsInLine = new[] { new InlineKeyboardButton(text) { CallbackData = $"id={guid}&chkuid=y&btn=dtl" } };
                    return btnsInLine;
                }, rnd_next4SqltRf());
            //end fun
            PrintRet(MethodBase.GetCurrentMethod().Name, ArrSlice<InlineKeyboardButton[]>(rsRztInlnKbdBtn, 0, 3));
            return rsRztInlnKbdBtn;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imgPath"></param>
        /// <param name="msgtxtAdmsg"></param>
        /// <param name="wdss">xxx,xxx xxx</param>
        /// <param name="chatid"></param>
        /// <param name="whereExprsObjFltrs"></param>
        /// <param name="partfile区块文件Exprs"></param>

        public static void srchNsendFotoToChatSess(string imgPath, string msgtxtAdmsg, string wdss, long chatid, Dictionary<string, StringValues> whereExprsObjFltrs, string partfile区块文件Exprs)
        {
            //   Dictionary<string, StringValues> whereExprsObj;
            //  string partfile区块文件Exprs;
            // calcPrtExprsNdefWhrCondt4grp(chatid, out whereExprsObj, out partfile区块文件Exprs);
            //  var patns_dbfs = db.calcPatns("mercht商家数据", partfile区块文件Exprs);

            //     string keyword = getRdmKwd(wdss);

            List<InlineKeyboardButton[]> results3 = QryFromMrchtByKwds(  wdss, whereExprsObjFltrs, partfile区块文件Exprs);
            Print(" SendPhotoAsync " + chatid);//  Program.botClient.send
            if (results3.Count > 0)
                sendFoto(imgPath, msgtxtAdmsg, results3, chatid);
        }

        private static List<SortedList> QryFromMrchtByKwdsV2(string wdss, string qrystr, string partfile区块文件Exprs)
        {
            wdss = wdss.Replace(" ", ",");

            //----where
            Func<SortedList, bool> whereFun = (SortedList row) =>
            {

                List<bool> li = new List<bool>();
                li.Add((IsNotEmptyLianxi(row)));
                li.Add((isLianxifshValid(row)));

                var fltrs = GetDicFromQrtstr(qrystr);
                //ConvertToStringDictionary(filters);
                li.Add(IsIn4qrycdt(LoadFieldDfemp(row, "园区"), LoadField232(fltrs, "园区")));
            
                return IsChkfltrOk(li);
                    //   if (LoadFieldDefEmpty(row, "cateEgls") == "Property")
                    //return false;

 

                HashSet<string> curRowKywdSset = new HashSet<string>();
                AddElmts2hashset(curRowKywdSset, LoadFieldDefEmpty(row, "商家"));
                AddElmts2hashset(curRowKywdSset, LoadFieldDefEmpty(row, "关键词"));
                AddElmts2hashset(curRowKywdSset, LoadFieldDefEmpty(row, "分类关键词"));
                if (IsContains(curRowKywdSset, wdss))
                    return true;
                return false;
            };


            //order
            //  List<SortedList> results22 = rdmList<SortedList>(rztLi);
            //  Func<SortedList, int> ordFun = (SortedList) => { return 1; };
            //map select 


          var rztLi = GetListFltrV2("mercht商家数据", partfile区块文件Exprs, whereFun);


          //  var results3 = rztLi.Skip(0 * 10).Take(5).ToList();
            return rztLi;
        }

  
        private static List<InlineKeyboardButton[]> QryFromMrchtByKwds(  string wdss, Dictionary<string, StringValues> whereExprsObjFltrs, string partfile区块文件Exprs)
        {
              wdss = wdss.Replace(" ", ",");

            //----where
            Func<SortedList, bool> whereFun = (SortedList row) =>
            {
                //if (row["园区"].ToString().Contains("龙湾"))
                //    Print("dbg2442");

                if (LoadFieldDefEmpty(row, "TG有效") == "N")
                    return false;
                //if have condit n fuhe condit next...beir skip ( dont have cdi or not eq )
                if (hasCondt(whereExprsObjFltrs, "城市"))
                    if (!strCls.StrEq(row["城市"], LoadFieldTryGetValue(whereExprsObjFltrs, "城市")))   //  cityname not in (citysss) 
                        return false;
                if (hasCondt(whereExprsObjFltrs, "园区"))
                    if (!IsIn4qrycdt(row["园区"], LoadFieldTryGetValue(whereExprsObjFltrs, "园区")))   //  cityname not in (citysss) 
                        return false;
                if (hasCondt(whereExprsObjFltrs, "国家"))
                    if (!strCls.StrEq(row["国家"], LoadFieldTryGetValue(whereExprsObjFltrs, "国家")))   //  cityname not in (citysss) 
                        return false;
                if (LoadFieldDefEmpty(row, "cateEgls") == "Property")
                    return false;


                //--------------is lianixfsh empty
                string lianxifsh = mrcht.getLianxifsh(row);
                if (lianxifsh == "")
                    return false;

                HashSet<string> curRowKywdSset = new HashSet<string>();
                AddElmts2hashset(curRowKywdSset, LoadFieldDefEmpty(row, "商家"));
                AddElmts2hashset(curRowKywdSset, LoadFieldDefEmpty(row, "关键词"));
                AddElmts2hashset(curRowKywdSset, LoadFieldDefEmpty(row, "分类关键词"));
                if (IsContains(curRowKywdSset, wdss))
                    return true;
                return false;
            };


            //order
            //  List<SortedList> results22 = rdmList<SortedList>(rztLi);
            Func<SortedList, int> ordFun = (SortedList) => { return 1; };
            //map select 
            Func<SortedList, InlineKeyboardButton[]> mapFun = (SortedList row) =>
            {
                string text = LoadFieldDefEmpty(row, "城市") + " • " + LoadFieldDefEmpty(row, "园区") + " • " + LoadFieldDefEmpty(row, "商家");
                string guid = LoadFieldDefEmpty(row, "Guid编号");
                InlineKeyboardButton[] btnsInLine = new[] { new InlineKeyboardButton(text) { CallbackData = $"id={guid}&sdr=tmr&btn=dtl&ckuid=n" } };
                return btnsInLine;
            };

            List<InlineKeyboardButton[]> rztLi = Qe_qryV2<InlineKeyboardButton[]>("mercht商家数据", partfile区块文件Exprs, whereFun, ordFun, mapFun, (dbf) =>
            {
                return rnd_next4Sqlt(dbf);
            });



            var results3 = rztLi.Skip(0 * 10).Take(5).ToList();
            return results3;
        }


        /// <summary>
        ///   查询商家
        ///    //先按照 服务词+地域词搜索，如果找不到则排除地域词只使用服务词搜索
        /// </summary>
        /// <param name="dbFrom">数据源</param>
        /// <param name="shareNames">分片</param>
        /// <param name="filters"></param>
        /// <param name="msgCtain_msgx_remvTrigWd2">消息内容</param>
        /// <returns>tg按钮数组</returns>
        public static List<InlineKeyboardButton[]> qryFromMrchtV2(string dbFrom, string shareNames, string filtersExprs, string msgCtain_msgx_remvTrigWd2)
        {

            //qry from mrcht by  where exprs  strFmt
            Dictionary<string, StringValues> filters = QueryHelpers.ParseQuery(filtersExprs);
            msgCtain_msgx_remvTrigWd2 = msgCtain_msgx_remvTrigWd2.ToUpper();
            msgCtain_msgx_remvTrigWd2 = ChineseCharacterConvert.Convert.ToSimple(msgCtain_msgx_remvTrigWd2);
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            PrintCallFunArgs(__METHOD__, func_get_args(dbFrom, shareNames, filters, msgCtain_msgx_remvTrigWd2));

            //  string msgx = whereExprsObj["msgCtain"];
            if (string.IsNullOrEmpty(msgCtain_msgx_remvTrigWd2)) { return []; }
            string[] kwds = strCls.SpltByFenci(ref msgCtain_msgx_remvTrigWd2);
            kwds = RemoveShortWords(kwds);
            //todo 去除触发词，，只保留 服务次和位置词
            //园区
            kwds = removeStopWd4biz(kwds);
            Print(" now kwd for srarch is =>" + string.Join(" ", kwds));




            // weizhici = guiyihuaWeizhici(weizhici);
            //Dictionary<string, StringValues> whereExprsObj = new Dictionary<string, StringValues>();

            // Func<SortedList, bool> whereFun22 = filtrList2whereFun111();
            //先按照 服务词+地域词搜索，如果找不到则排除地域词只使用服务词搜索
            Func<SortedList, bool> whereFun = (SortedList row) =>
            {
                if (row["园区"].ToString().Contains("东风"))
                    Print("dbg2443");
                List<bool> li = new List<bool>();
                li.Add((IsNotEmptyLianxi(row)));
                li.Add((isLianxifshValid(row)));
         
                var fltrs = ConvertToStringDictionary(filters);
                li.Add(IsIn4qrycdt(LoadFieldDfemp(row, "园区"), LoadField232(fltrs, "园区")));
                li.Add(isEq4qrycdt(LoadFieldDfemp(row, "城市"), LoadField232(fltrs, "城市")));
                //   li.Add(isEq4qrycdt(ldfldDfemp(row, "国家"), ldfld(fltrs, "国家")));
                li.Add((isCotainFuwuci(row, msgCtain_msgx_remvTrigWd2)));               
                li.Add((isCotainPostnWd(row, kwds)));
                return IsChkfltrOk(li);
            };
            var list = GetListFltr("mercht商家数据", shareNames, whereFun);
            if (list.Count == 0)
            {
                //qry rmv poston where 
                whereFun = (SortedList row) =>
                  {
                      if (row["园区"].ToString().Contains("东风"))
                          Print("dbg2444");
                      List<bool> li = new List<bool>();
                      //    li.Add(new Filtr(isNotEmptyLianxi(row)));
                      //    li.Add(new Filtr(isLianxifshValid(row)));
                      var fltrs = ConvertToStringDictionary(filters);
                      li.Add(IsIn4qrycdt(LoadFieldDfemp( row, "园区"),LoadField232(fltrs, "园区")));
                      li.Add(isEq4qrycdt(LoadFieldDfemp(row, "城市"), LoadField232(fltrs, "城市")));
                      //   li.Add(isEq4qrycdt(ldfldDfemp(row, "国家"), ldfld(fltrs, "国家")));
                      li.Add((isCotainFuwuci(row, msgCtain_msgx_remvTrigWd2)));

                      return IsChkfltrOk(li);
                  };
                list = GetListFltr("mercht商家数据", shareNames, whereFun);
            }

            //----------paixu 
            ForeachHashtableEs(list, (SortedList row) =>
           {
               var containScore = containCalcCntScoreSetfmt1(row, kwds);
               if (containScore > 0)
               {
                   row["_containCntScore"] = containScore;
               }
           });
            var ordFun = (SortedList sl) =>
            {
                return 0 - (int)LoadField(sl, "_containCntScore", 0);
            };
            var rztLi_afterOrd = list.Cast<SortedList>()
                                   .OrderBy(ordFun)
                                   .ToList();
            //---------------
            var list_trans = foreach_hstbEs(rztLi_afterOrd, (SortedList row) =>
            {
                string text = LoadFieldDefEmpty(row, "城市") + " • " + LoadFieldDefEmpty(row, "园区") + " • " + LoadFieldDefEmpty(row, "商家");
                string guid = LoadFieldDefEmpty(row, "Guid编号");
                InlineKeyboardButton[] btnsInLine = new[] { new InlineKeyboardButton(text) { CallbackData = $"id={guid}&chkuid=y&btn=dtl" } };
                return btnsInLine;
            });


            //end fun
            PrintRet(MethodBase.GetCurrentMethod().Name, ArrSlice<object>(list_trans, 0, 1));
            return ConvertToInlineKeyboardButtons(list_trans);
        }

    

 
        //private static Func<SortedList, bool> filtrList2whereFun111()
        //{

        //    List<Filtr> li = new List<Filtr>();
        //    li.Add(new Filtr(isNotEmptyLianxi(row)));

        //    Func<SortedList, bool> whereFun = castFltlst2whereFun(li);
        //}





        //private static bool isFldValEq(SortedList row, string v1, string v2)
        //{
        //    throw new NotImplementedException();
        //}

        public static bool IsNotEmptyLianxi(SortedList row)
        {
            return !isEmptyLianxi(row);
        }

        public static bool isLianxifshValid(SortedList row)
        {
            // string Fld = ;
            if (getFldLianxifs(row, "Whatsapp") != "")
                return true;
            if (getFldLianxifs(row, "微信") != "")
                return true;
            if (getFldLianxifs(row, "Telegram") != "")
                if (LoadFieldDefEmpty(row, "TG有效") == "N")// only tg and tg not vld
                    return false;
                else
                    return true;
            return false;

        }

        public static string getFldLianxifs(SortedList row, string Fld)
        {
            // const string Fld = "Telegram";
            return TrimRemoveUnnecessaryCharacters4tgWhtapExt(LoadFieldDefEmpty(row, Fld));
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
            arrCls.AddElmts2hashset(fuwuWds_row, LoadFieldDefEmpty(row, "商家"));
            arrCls.AddElmts2hashset(fuwuWds_row, LoadFieldDefEmpty(row, "关键词"));
            arrCls.AddElmts2hashset(fuwuWds_row, LoadFieldDefEmpty(row, "分类关键词"));
            fuwuWds_row.Remove("店");
            fuwuWds_row = ConvertToUpperCase(fuwuWds_row);

            if (ContainKwds(msgCtain, fuwuWds_row))
            {
                SortedList dbg = new SortedList();
                dbg.Add("row", row);
                dbg.Add("msgctain", msgCtain);
                Print(" $$FUN isCotainFuwuci()" + json_encode_noFmt(dbg));
                Print(" isCotainFuwuc() containKwds(msgCtain, fuwuWds_row) true");
                print_varDump("isCotainFuwuci", "fuwuWds_row", fuwuWds_row);
                Print(" $$ENDFUN isCotainFuwuci()");
                return true;
            }

            return false;
        }

        private static int containCalcCntScoreSetfmt1(SortedList row, string[] kwds)
        {
            HashSet<string> curRowKywdSset = new HashSet<string>();

            AddElmts2hashset(curRowKywdSset, LoadFieldDefEmpty(row, "国家"));
            AddElmts2hashset(curRowKywdSset, LoadFieldDefEmpty(row, "城市"));
            AddElmts2hashset(curRowKywdSset, LoadFieldDefEmpty(row, "园区"));
            AddElmts2hashset(curRowKywdSset, LoadFieldDefEmpty(row, "城市关键词"));
            AddElmts2hashset(curRowKywdSset, LoadFieldDefEmpty(row, "园区关键词"));
            return containCalcCntScoreSetfmt(ConvertToUpperCase(curRowKywdSset), kwds);
        }

        private static bool isCotainPostnWd(SortedList row, string[] kwds)
        {
            HashSet<string> curRowKywdSset = new HashSet<string>();

            AddElmts2hashset(curRowKywdSset, LoadFieldDefEmpty(row, "国家"));
            AddElmts2hashset(curRowKywdSset, LoadFieldDefEmpty(row, "城市"));
            AddElmts2hashset(curRowKywdSset, LoadFieldDefEmpty(row, "园区"));
            AddElmts2hashset(curRowKywdSset, LoadFieldDefEmpty(row, "城市关键词"));
            AddElmts2hashset(curRowKywdSset, LoadFieldDefEmpty(row, "园区关键词"));
            curRowKywdSset = ConvertToUpperCase(curRowKywdSset);
            Print(" curRw_posnSet=>" + String.Join(" ", curRowKywdSset));
            //print(" weizhici=>" + weizhici);
            return isCcontainKwds42(curRowKywdSset, kwds);
        }

        //public static List<SortedList> getList(string dbFrom, string shareNames, Dictionary<string, StringValues> whereExprsObj)
        //{

        //    Func<SortedList, bool> whereFun = (SortedList row) =>
        //    {

        //        try
        //        {

        //            //if have condit n fuhe condit next...beir skip ( dont have cdi or not eq )
        //            if (hasCondt(whereExprsObj, "城市"))
        //                if (!strCls.str_eq(row["城市"], arrCls.ldfld_TryGetValue(whereExprsObj, "城市")))   //  cityname not in (citysss) 
        //                    return false;
        //            if (hasCondt(whereExprsObj, "园区"))
        //                if (!strCls.str_eq(row["园区"], arrCls.ldfld_TryGetValue(whereExprsObj, "园区")))   //  cityname not in (citysss) 
        //                    return false;
        //            if (hasCondt(whereExprsObj, "国家"))
        //                if (!strCls.str_eq(row["国家"], arrCls.ldfld_TryGetValue(whereExprsObj, "国家")))   //  cityname not in (citysss) 
        //                    return false;
        //            return true;
        //        }
        //        catch (Exception e)
        //        {
        //            print_catchEx("getPostnWds", e);
        //            //print_varDump("getPostnWds","ex", e);
        //        }
        //        return false;
        //    };

        //    List<SortedList> rsRztInlnKbdBtn = null;//= arr_fltr(dbFrom, shareNames, null);



        //    return rsRztInlnKbdBtn;
        //}


        public static HashSet<string> qry_getPostnWds(string dbFrom, string shareNames, Dictionary<string, StringValues> whereExprsObj)
        {

            Func<SortedList, bool> whereFun = (SortedList row) =>
            {

                try
                {

                    //if have condit n fuhe condit next...beir skip ( dont have cdi or not eq )
                    if (hasCondt(whereExprsObj, "城市"))
                        if (!strCls.StrEq(row["城市"], LoadFieldTryGetValue(whereExprsObj, "城市")))   //  cityname not in (citysss) 
                            return false;
                    if (hasCondt(whereExprsObj, "园区"))
                        if (!strCls.StrEq(row["园区"], LoadFieldTryGetValue(whereExprsObj, "园区")))   //  cityname not in (citysss) 
                            return false;
                    if (hasCondt(whereExprsObj, "国家"))
                        if (!strCls.StrEq(row["国家"], LoadFieldTryGetValue(whereExprsObj, "国家")))   //  cityname not in (citysss) 
                            return false;
                    return true;
                }
                catch (Exception e)
                {
                    PrintCatchEx("getPostnWds", e);
                    //print_varDump("getPostnWds","ex", e);
                }
                return false;
            };

            List<string> rsRztInlnKbdBtn = Qe_qryV2(
                  dbFrom, shareNames,
                  whereFun, null, (SortedList row) =>
                  {
                      HashSet<string> curRowKywdSset = new HashSet<string>();

                      AddElmts2hashset(curRowKywdSset, LoadFieldDefEmpty(row, "国家"));
                      AddElmts2hashset(curRowKywdSset, LoadFieldDefEmpty(row, "城市"));
                      AddElmts2hashset(curRowKywdSset, LoadFieldDefEmpty(row, "园区"));
                      AddElmts2hashset(curRowKywdSset, LoadFieldDefEmpty(row, "城市关键词"));
                      AddElmts2hashset(curRowKywdSset, LoadFieldDefEmpty(row, "园区关键词"));
                      curRowKywdSset = ConvertToUpperCase(curRowKywdSset);
                      return String.Join(" ", curRowKywdSset);
                  }, rnd_next4SqltRf());

            HashSet<string> hs = new HashSet<string>();
            foreach (string pstWds in rsRztInlnKbdBtn)
            {
                string[] wds = pstWds.Split(' ');
                HashSet<string> hsTmp = castArr2set(wds);
                hs = ArrMerge(hs, hsTmp);
            }

            //   var patns_dbfs = db.calcPatns(dbFrom, shareNames);

            HashSet<string> hashSet = ReadLinesToHashSet("位置词.txt");
            HashSet<string> hashSet1 = ArrMerge(hs, hashSet);

            HashSet<string> hashSet2 = ConvertToUpperCase(hashSet1);
            hashSet2 = ArrRemove(hashSet2, "市区 城市 国家 园区名称 城市名称 国家词 ");

            // 使用 LINQ 过滤非空元素并创建新的 HashSet<string>
            HashSet<string> filteredSet = new HashSet<string>(
                hashSet2.Where(s => !string.IsNullOrWhiteSpace(s))
            );
            return filteredSet;
        }







        public static string getLianxifsh(SortedList row)
        {
          string shj=  LoadFieldDefEmpty(row, "商家");
            if (shj.Contains("福满多"))
                Print("dbg415");
            string lianxifsh = LoadFieldDefEmpty(row, "Telegram") + LoadFieldDefEmpty(row, "WhatsApp")

             + LoadFieldDefEmpty(row, "微信") + LoadFieldDefEmpty(row, "Tel")
              + LoadFieldDefEmpty(row, "Line");
            lianxifsh = TrimRemoveUnnecessaryCharacters4tgWhtapExt(lianxifsh);
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
        //        //print(DateTime.Now.ToString("yyyyMMdd_HHmmss_fff"));  
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
        //   print("【搜索引擎模式】：{0}", string.Join("/ ", segments));


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
        //    //   print(updateString);
        //    // 获取当前时间并格式化为文件名
        //    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
        //    string fileName = $"{dbgFl}/{timestamp}.txt";
        //    //     print(fileName);
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
        //        //   print(JsonConvert.SerializeObject(cityMap, Formatting.Indented));
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
        //            //    print(JsonConvert.SerializeObject(addMap, Formatting.Indented));
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
        //                //  print(mcht["Category"]);
        //                //    mcht.Add("CategoryStr", Program._categoryKeyValue[Convert.ToInt32(mcht["Category"].ToString())]);
        //                mcht.Add("CategoryStrKwds", Program._categoryKeyValue[(int)m.Category]);
        //                mcht.Add("cateInt", (int)m.Category);
        //                mcht.Add("cateEgls", m.Category.ToString());
        //                //   mcht

        //                mcht.Add("_parent", city.Name + "_" + addx_park.Name);
        //                a.Add(mcht);
        //                //  print(JsonConvert.SerializeObject(mcht, Formatting.Indented));
        //                // print("..");
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
        //           print(e.Message);
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
        //           print(e.Message);
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

