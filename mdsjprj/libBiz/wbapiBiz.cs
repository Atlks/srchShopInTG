﻿
global using static mdsj.libBiz.wbapiBiz;
using System.Security.Cryptography;
using System.Text;
using libx;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using prjx;
using prjx.lib;
using RG3.PF.Abstractions.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities.Collections;

namespace mdsj.libBiz
{
    internal class wbapiBiz
    {


        //todo  setcity  setpark

        //  http://localhost:5000/setpark?park=东风园区&uid=007


        /// <summary>
        /// 设置园区
        /// </summary>
        /// <param name="qrystr"></param>
        /// <returns></returns>
        public static string WbapiXsetpark(string qrystr)
        {
            //shangjiaID,uid,dafen
            SortedList qrystrMap = GetHashtableFromQrystr(qrystr);
            qrystrMap["id"] = qrystrMap["uid"];
            string dbfile = "parkcfgDir/uid_" + qrystrMap["uid"] + ".json";
            ormJSonFL.SaveJson(qrystrMap, dbfile);
            return "ok";
        }

        ///// <summary>
        ///// wbapi_upldPOST
        ///// </summary>
        ///// <param name="request"></param>
        ///// <param name="response"></param>
        //public static void wbapi_upldPOST(HttpRequest request, HttpResponse response)
        //{
        //    // Check if the request contains a file
        //    if (request.Form.Files.Count > 0)
        //    {
        //        foreach (var file in request.Form.Files)
        //        {
        //            // Get the file content and save it to a desired location
        //            var filePath = Path.Combine("uploads", file.FileName);
        //            Mkdir4File(filePath);
        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                file.CopyToAsync(stream).GetAwaiter().GetResult();
        //            }
        //        }
        //    }

        //    // Handle other form data
        //    //foreach (var key in request.Form.Keys)
        //    //{
        //    //    var value = request.Form[key];
        //    //    ConsoleWriteLine($"Key: {key}, Value: {value}");
        //    //}
        //    SortedList dafenObj = ConvertFormToSortedList(request.Form);
        //    ormJSonFL.save(dafenObj, "uplodData/" + Guid.NewGuid().ToString() + ".json");
        //    SendResp("ok", response);
        //    //    return "ok";
        //}

        ///// <summary>
        /// 打分
        ///  
        /// <example><![CDATA[ http://localhost:5000/dafen?shangjiaID=yourValue11&dafen=3&uid=007]]></example>
        /// </summary>
        /// <param name="shangjiaID">商家id</param>
        /// <param name="dafen">分数</param>
        ///     <param name="uid">用户id</param>
        public static string WbapiXdafen(string qrystr)
        {
            //shangjiaID,uid,dafen
            SortedList dafenObj = GetHashtableFromQrystr(qrystr);
            ormJSonFL.SaveJson(dafenObj, "dafenDt打分数据/" + dafenObj["shangjiaID"] + ".json");
            return "ok";
        }

        //public static string Wbapi_swag()
        //{

        //}



        /// <summary>
        /// 评论商家
        /// 
        /// <example><![CDATA[  http://localhost:5000/pinlun?shangjiaID=avymrhifuyzkfetlnifryraazk&pinlun=465464564646 ]]></example>
        /// </summary>
        /// <param name="shangjiaID">商家id</param>
        ///     <param name="pinlun">评论内容</param>
        public static string WbapiXpinlun(string qrystr)
        {
            //  print("Received getlist: " + callGetlistFromDb);
            //  return Results.Ok("OK");
            SortedList qrystrMap = GetHashtableFromQrystr(qrystr);
            SortedList obj1 = new SortedList();
            CopySortedList(qrystrMap, obj1);
            obj1.Add("id", DateTime.Now.ToString());
            obj1.Add("商家guid", qrystrMap["shangjiaID"]);
            //    obj1.Add("商家", merchant.Name);
            obj1.Add("时间", DateTime.Now.ToString());
            obj1.Add("评论内容", qrystrMap["pinlun"]);
            //    obj1.Add("评论人", update.Message.From.Username);
            obj1.Add("评论人id", qrystrMap["uid"]);
            System.IO.Directory.CreateDirectory("pinlunDir");
            //    ormSqlt.save(obj1, "pinlunDir/" + merchant.Guid + merchant.Name + ".db");
            ormJSonFL.SaveJson(obj1, "pinlunDir评论数据/" + qrystrMap["shangjiaID"] + ".json");

            //    ormJSonFL.save(dafenObj, "dafenDatadir/" + dafenObj["shangjiaID"] + ".json");
            return "ok";
        }

        /// <summary>
        /// 查询评论
        /// 
        /// <example><![CDATA[http://localhost:5000/getlistPinlun?shangjiaID=avymrhifuyzkfetlnifryraazk]]></example>
        /// </summary>
        /// <param name="shangjiaID">商家id</param>
        ///  <returns>返回json数组.</returns>
        public static string WbapiXgetlistPinlun(string qrystr)
        {
            SortedList qrystrHstb = GetHashtableFromQrystr(qrystr);
            var li = ormJSonFL.QrySglFL("pinlunDir评论数据/" + qrystrHstb["shangjiaID"] + ".json");
            return EncodeJson(li);
        }




        /// <summary>
        /// 查询商家
        ///  
        /// <example><![CDATA[http://localhost:5000/getlist?id=avymrhifuyzkfetlnifryraazk]]></example>
        /// <example><![CDATA[http://localhost:5000/getlist?分类=娱乐&page=1]]></example>
        /// </summary>
        /// 
        ///          <param name="id">商家id</param>
        /// <param name="分类">商家分类</param>
        ///  <param name="page">页数</param>
        ///        <param name="pagesize">每页数量</param>
        ///  <returns>返回json数组.</returns>
        public static string WbapiXgetlistPost(string qrystr)
        {
            return "";

        }
        public static Hashtable CastToHashtbFrmparseLxfs(string jsonString)
        {
            // 将 JSON 字符串解析为数组
            var array = JsonConvert.DeserializeObject<string[][]>(jsonString);

            // 将数组转换为 Hashtable
            var hashtable = ConvertArrayToHashtable(array);
            return hashtable;
        }
        /*
         
               //    GetQryStr4srch
            //rmv pagePrm token
            //SortedList qryClrMap = RemoveKeys(qryMap, "商家 token page pages pagesize limit page limit pagesize from");
            //string qrtStr4Srch = CastHashtableToQuerystringNoEncodeurl(qryClrMap);

            //Func<SortedList, bool> whereFun = CastQrystr2FltrCdtFun(qrtStr4Srch);
            //var list = GetListFltr(FromDdataDir, null, whereFun);
         
         */
        /// <summary>
        ///   dep   ,,,(req,repos) is bettr
        /// </summary>
        /// <param name="qrystr"></param>
        /// <returns></returns>
        public static string WbapiXgetlist(string qrystr)
        {
            // todo 优化分页处理 cache qry rzt 10 min,
            //parse qrystr ,del page prm..just ok as cacheKey
            //todo  使用 Dictionary 替代 SortedList，pfm
            PrintTimestamp(" start fun WbapiXgetlist()");
            SortedList qrystrMap = GetHashtableFromQrystr(qrystr);
            const string FromDdataDir = "mercht商家数据"; ;
            //todo v2   here qry need abt 50ms
            string qrtStr4Srch2 = DelKeys("商家 国家 城市 " + pageprm251, qrystr);
            //----- qrystr  rwrt  parks
            string pkrPrm = string.Join(",",
                   GetFieldAsStr(qrystrMap, "园区"),
                   GetFieldAsStr(qrystrMap, "国家"),
                   GetFieldAsStr(qrystrMap, "城市")
           );
            //    string pkrPrm = GetFieldAsStr(qrystrMap, "园区")+","+ GetFieldAsStr(qrystrMap, "国家")+"," + GetFieldAsStr(qrystrMap, "城市");    ;// "KK园区,东方园区,缅甸,妙瓦底";
            pkrPrm = ClrCommaStr(pkrPrm);
            string rzt = ExtParks(pkrPrm);
            rzt = ToSqlPrmMode(rzt);  // "KK园区,东方园区,缅甸

            string qrtStr4Srch525 = qrtStr4Srch2;
            if (rzt != "")
                qrtStr4Srch525 = SetField4qrystr(qrtStr4Srch2, "园区", rzt);

            //  qrtStr4Srch525 = SetField4qrystr(qrtStr4Srch2, "园区", "");
            //----end rwrt

            PrintLog("⚠️⚠️true qrtStr4Srch525  => " + qrtStr4Srch525);
            var listFlrted = GetListFltrByQrystr(FromDdataDir, null, qrtStr4Srch525);
            // --------------------  flt 
            SortedList qryMap = GetHashtableFromQrystr(qrystr);
            var list_aftFltr2 = ArrFltrV2(listFlrted, (SortedList row) =>
            {
                List<bool> li = new List<bool>();
                string mrtKwd = GetFieldAsStr1037(qryMap, "商家").ToUpper();
                if (mrtKwd.Length > 0)
                    li.Add(GetFieldAsStr1037(row, "商家").ToUpper().Contains(mrtKwd));
                //   li.Add((IsNotEmptyLianxi(row)));
                //   li.Add((isLianxifshValid(row)));
                return IsChkfltrOk(li);
            });

            //-----------todo block page smp
            int page = GetFieldAsInt(qryMap, "page", 0);
            int pagesize = GetFieldAsInt(qryMap, "pagesize", 10);
            int start = (page - 1) * pagesize;
            //todo 
            //  var list_rzt2 = SliceByPageByQrystr(list_aftFltr2, qrystr);
            List<SortedList> list_rzt = SliceX(list_aftFltr2, start, pagesize);


            //------------block add col
            PrintTimestamp(" start add col");
            //todo binxin for
            ForList("BlkAddCol", list_rzt, (sortedList) =>
            {
                var pinlunDtDir = "pinlunDir评论数据/" + sortedList["id"] + ".json";
                var list11 = GetListHashtableFromJsonFil(pinlunDtDir);
                SetField938(sortedList, "NumberOfComments", list11.Count);
                SetField938(sortedList, "Comments", list11);

                var df = "dafenDt打分数据/" + sortedList["id"] + ".json";
                var list12 = GetListHashtableFromJsonFil(df);


                SetField938(sortedList, "Scores", Avg(list12, "dafen"));
                SetField938(sortedList, "pages", CalculateTotalPages(pagesize, list_aftFltr2.Count));

            });

            PrintTimestamp(" end add col");
            //----------------block trans cn2en form--------------
            PrintTimestamp(" start trans cn2en");
            SortedList<string, string> transmap = LoadSortedListFromIni($"{prjdir}/cfg字段翻译表/字段表.ini");
            //trans key  todo binxin trans

            List<SortedList> list_rzt_fmt = new List<SortedList>();

            //var list_rzt_fmt = list_rzt.AsParallel().Select(sortedList =>
            //{
            //    if (sortedList == null)
            //        return null;
            //    return castKeyToEnName(sortedList, transmap);
            //}).ToList();
            ForList("Blk.transKey", list_rzt, (sortedList) =>
            {
                //todo fun  transkey
                //对于 大数据集 或 计算密集型操作，PLINQ 可能会带来显著的性能提升
                if (sortedList == null)
                    return;
                SortedList map3 = castKeyToEnName(sortedList, transmap);
                list_rzt_fmt.Add(map3);
            });
            PrintTimestamp(" endblock trans cn2en");
            //--------end block trans fmt chg int fmt
            //chg int fmt
            string rsstr = EncodeJson(list_rzt_fmt);
            PrintTimestamp(" end fun WbapiXgetlist()");
            return rsstr;
        }



        /// <summary>
        /// 添加文章 （ post提交 ）
        ///  提交路径 /AddPost
        /// </summary>
        ///    <param name="Cate">分类 (资源 招聘等 ）</param>
        /// <param name="Title">标题</param>
        /// <param name="Txt">内容</param>

        /// <param name="Poster">发布人</param>
        /// <param name="File">相关文件 图片 视频等 资源</param>
        /// 
        /// 
        public static void AddPostPOSTWbapi(HttpRequest request, HttpResponse response)
        {
            //   if (request.Method == HttpMethods.Post)

            // Check if the request contains a file
            var fil = "";
            List<string> filess = new List<string>();
            if (request.Form.Files.Count > 0)
            {
                Print("🔄♻️✅");
                foreach (var file in request.Form.Files)
                {
                    // Get the file content and save it to a desired location
                    var filePath = Path.Combine($"{prjdir}/webroot/uploads1016", file.FileName);
                    fil = filePath;
                    filess.Add("uploads1016/" + file.FileName);
                    Mkdir4File(filePath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyToAsync(stream).GetAwaiter().GetResult();
                    }
                }
            }

            // Handle other form data
            //foreach (var key in request.Form.Keys)
            //{
            //    var value = request.Form[key];
            //    ConsoleWriteLine($"Key: {key}, Value: {value}");
            //}

            // Call the specific API handler
            //    httpHdlrApiSpecl(request, response);

            SortedList saveOBJ = ConvertFormToSortedList(request.Form);
            // saveOBJ.Add("照片或视频", fil);
            saveOBJ.Add("Files", (filess));

            // 获取当前时间（本地时间）
            DateTime now = DateTime.Now;

            // 格式化为可读性较强的字符串，精确到毫秒
            string formattedDate = now.ToString("yyyy-MM-dd HH:mm:ss");
            saveOBJ.Add("Time", formattedDate);
            ormJSonFL.SaveJson(saveOBJ, $"{prjdir}/db/{saveOBJ["Cate"]}.json");
            SendResp("ok", response);

            Jmp2end(nameof(AddPostPOSTWbapi));

        }


        /// <summary>
        ///  商家删除        /DelMercht?id=111
        ///  
        /// </summary>
        /// <param name="商家"></param>
        /// <param name="营业时间">12:00-22:00</param>
        /// <param name="位置"></param>
        /// <param name="照片或视频">h5文件表单上传文件</param>
        public static void DelMerchtGETWbapi(HttpRequest request, HttpResponse response)
        {
            var queryString = request.QueryString.ToString();

            SortedList saveOBJ = GetHashtableFromQrystr(queryString);

            string token = GetFieldAsStrDep(saveOBJ, "token");
            //string[] tka = token.Split("_");
            //string uid = GetElmt(tka, 0);
            //string exprt = GetElmt(tka, 1);
            //string token_uidDotExprtMode = DecryptAes(exprt);

            //if (IsValidToken(token))
            //{
            //    SendResp("token无效", response);
            //    Jmp2end(nameof(AddMerchtPOSTWbapi));
            //}


            ormSqlt.delByID(saveOBJ["id"].ToString(), "mercht商家数据", "mercht商家数据/缅甸.db");
            cache2024.Remove("mercht商家数据/缅甸");
            cache2024.Remove("mercht商家数据/缅甸.db");

            SendResp("ok", response);

            Jmp2end(nameof(DelMerchtGETWbapi));
        }

        // Handle other form data
        //foreach (var key in request.Form.Keys)
        //{
        //    var value = request.Form[key];
        //    ConsoleWriteLine($"Key: {key}, Value: {value}");
        //}

        // Call the specific API handler
        //    httpHdlrApiSpecl(request, response);

        public static void AddMerchtPOSTWbapi(HttpRequest request, HttpResponse response)
        {
            //   if (request.Method == HttpMethods.Post)

            // Check if the request contains a file
            PrintLog(" start fun AddMerchtPOSTWbapi()");
            PrintLog(" start fun request.Form.Files.Count=>" + request.Form.Files.Count);
            var fil = "";
            if (request.Form.Files.Count > 0)
            {
                foreach (var file in request.Form.Files)
                {
                    try
                    {
                        // Get the file content and save it to a desired location
                        var filePath = Path.Combine($"{prjdir}/webroot/uploads1016", file.FileName);
                        fil = filePath;
                        fil = "uploads1016/" + file.FileName;
                        Mkdir4File(filePath);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyToAsync(stream).GetAwaiter().GetResult();
                        }
                    }
                    catch (Exception e)
                    {
                        PrintExcept("add mrcht", e);
                    }

                }
            }


            SortedList saveOBJ = ConvertFormToSortedList(request.Form);
            saveOBJ.Add("照片或视频", fil);

            Hashtable hashtable = CastToHashtbFrmparseLxfs(GetFieldAsStr(saveOBJ, "联系方式"));
            CopyHashtableToSortedList(hashtable, saveOBJ);

            string token = GetFieldAsStrDep(saveOBJ, "token");


            //if (IsValidToken(token))
            // {
            //     SendResp("token无效", response);
            //     Jmp2end(nameof(AddMerchtPOSTWbapi));
            // }
            //string[] tka = token.Split("_");
            //string uid = GetElmt(tka, 0);
            //SetField(saveOBJ, "uid", uid);
            ormJSonFL.SaveJson(saveOBJ, $"{prjdir}/db/mrchtDt商家数据/" + Guid.NewGuid().ToString() + ".json");
            ormSqlt.Save4Sqlt(saveOBJ, "mercht商家数据/缅甸.db");
            cache2024.Remove("mercht商家数据/缅甸");//here move to save4sqlt
            cache2024.Remove("mercht商家数据/缅甸.db");
            SendResp("ok", response);

            Jmp2end(nameof(AddMerchtPOSTWbapi));
        }




        //  http://localhost:5000/getDetail?id=avymrhifuyzkfetlnifryraazk
        public static string Wbapi_getDetail(string qrystr)
        {
            //  print("Received getlist: " + callGetlistFromDb);
            //  return Results.Ok("OK");
            SortedList qrystrMap = GetHashtableFromQrystr(qrystr);
            int page = GetFieldAsInt(qrystrMap, "page", 0);
            int pagesize = GetFieldAsInt(qrystrMap, "pagesize", 10);
            SortedList map = new SortedList();
            map.Add("limit", 5);

            Func<SortedList, bool> whereFun = CastQrystr2FltrCdtFun(qrystr);
            var list = GetListFltr("mercht商家数据", null, whereFun);
            var list3 = ArrFltr(list, (SortedList row) =>
            {
                List<bool> li = new List<bool>();

                //  li.Add((isNotEmptyLianxi(row)));
                //   li.Add((isLianxifshValid(row)));
                if (!ChkAllFltrTrue(li))
                    return false;
                return true;

            });
            int start = (page - 1) * pagesize;
            //if (start < 0)
            //    start = 0;

            var list2 = SliceX(list, start, pagesize);
            //    foreach_hashtable
            //Func<DictionaryEntry, object> fun
            ForeachHashtableEs(list2, (SortedList rw) =>
            {
                rw.Add("pinlun", ormJSonFL.QrySglFL("pinlunDir/" + qrystrMap["id"] + ".json"));
                rw.Add("dafen", "555");
            });
            return EncodeJson(list2);
        }


    }
}
