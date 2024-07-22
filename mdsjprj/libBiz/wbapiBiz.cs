global using static mdsj.libBiz.wbapiBiz;
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
using System.Text;
using System.Threading.Tasks;

namespace mdsj.libBiz
{
    internal class wbapiBiz
    {
        //todo  setcity  setpark

        //  http://localhost:5000/setpark?park=东风园区&uid=007
        public static string Wbapi_setpark(string qrystr)
        {
            //shangjiaID,uid,dafen
            SortedList qrystrMap = GetHashtableFromQrystr(qrystr);
            qrystrMap["id"] = qrystrMap["uid"];
            string dbfile = "parkcfgDir/uid_" + qrystrMap["uid"] + ".json";
            ormJSonFL.save(qrystrMap, dbfile);
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
        public static string Wbapi_dafen(string qrystr)
        {
            //shangjiaID,uid,dafen
            SortedList dafenObj = GetHashtableFromQrystr(qrystr);
            ormJSonFL.save(dafenObj, "dafenDt打分数据/" + dafenObj["shangjiaID"] + ".json");
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
        public static string Wbapi_pinlun(string qrystr)
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
            ormJSonFL.save(obj1, "pinlunDir评论数据/" + qrystrMap["shangjiaID"] + ".json");

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
        public static string Wbapi_getlistPinlun(string qrystr)
        {
            SortedList qrystrHstb = GetHashtableFromQrystr(qrystr);
            var li = ormJSonFL.qrySglFL("pinlunDir/" + qrystrHstb["shangjiaID"] + ".json");
            return encodeJson(li);
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

        public static string Wbapi_getlist(string qrystr)
        {
            //  print("Received getlist: " + callGetlistFromDb);
            //  return Results.Ok("OK");
            SortedList dafenObj = GetHashtableFromQrystr(qrystr);
            int page = gtfldInt(dafenObj, "page", 0);
            int pagesize = gtfldInt(dafenObj, "pagesize", 10);
            SortedList map = new SortedList();
            map.Add("limit", 5);

            Func<SortedList, bool> whereFun = castQrystr2FltrCdtFun(qrystr);
            var list = getListFltr("mercht商家数据", null, whereFun);
            var list_aftFltr = ArrFltr(list, (SortedList row) =>
            {
                List<bool> li = new List<bool>();
                li.Add((isNotEmptyLianxi(row)));
                //   li.Add((isLianxifshValid(row)));
                return isChkfltrOk(li);

            });
            int start = (page - 1) * pagesize;
            //if (start < 0)
            //    start = 0;

            List<SortedList> list_rzt = SliceX(list_aftFltr, start, pagesize);


            //------------add col

            foreach (var sortedList in list_rzt)
            {
                var pinlunDtDir = "pinlunDir评论数据/" + sortedList["id"] + ".json";
                var list11 = GetListHashtableFromJsonFil(pinlunDtDir);
                SetField938(sortedList, "NumberOfComments", list11.Count);

                var df = "dafenDt打分数据/" + sortedList["id"] + ".json";
                var list12= GetListHashtableFromJsonFil(df);


                SetField938(sortedList, "Scores", Avg(list12,"dafen"));
                SetField938(sortedList, "pages", CalculateTotalPages(pagesize, list_aftFltr.Count));

            }

            //----------------trans cn2en form--------------
            SortedList<string, string> transmap = LoadSortedListFromIni($"{prjdir}/cfg字段翻译表/字段表.ini");


            //trans key
            List<SortedList> list_rzt_fmt = new List<SortedList>();
            foreach (var sortedList in list_rzt)
            {
                SortedList map3 = new SortedList();
                // 循环遍历每一个键
                foreach (object key in sortedList.Keys)
                {
                    if (key.ToString() == "Searchs")
                        Print("dbg");
                    //add all cn key
                    var Cnkey = key;
                    var val = sortedList[key];
                    SetField938(map3, Cnkey.ToString(), val);

                    //add all eng key
                    var keyEng = LoadFieldDefEmpty(transmap, Cnkey);
                    if (keyEng == "")
                        keyEng = Cnkey.ToString();
                    SetField938(map3, keyEng, val);
                    //chg int fmt
                    if (IsNumeric((val)))
                    {
                        double objSave = ConvertStringToNumber(val);
                        SetField938(map3, keyEng, objSave);
                    }

                    //   Console.WriteLine($"Key: {key}, Value: {sortedList[key]}");

                }
                list_rzt_fmt.Add(map3);
            }

            //--------trans fmt chg int fmt
            //chg int fmt
            return encodeJson(list_rzt_fmt);
        }


        /// <summary>
        ///  商家入住    （ post提交 ）
        ///  提交路径 /AddMercht
        /// </summary>
        /// <param name="商家"></param>
        /// <param name="营业时间">12:00-22:00</param>
        /// <param name="位置"></param>
        /// <param name="照片或视频">h5文件表单上传文件</param>
        public static void AddMerchtPOSTWbapi(HttpRequest request, HttpResponse response)
        {
            //   if (request.Method == HttpMethods.Post)

            // Check if the request contains a file
            var fil = "";
            if (request.Form.Files.Count > 0)
            {
                foreach (var file in request.Form.Files)
                {
                    // Get the file content and save it to a desired location
                    var filePath = Path.Combine("uploads1016", file.FileName);
                    fil = filePath;
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
            saveOBJ.Add("照片或视频", fil);
            ormJSonFL.save(saveOBJ, $"{prjdir}/db/mrchtDt商家数据/" + Guid.NewGuid().ToString() + ".json");
            SendResp("ok", response);

            ormSqlt.save(saveOBJ, "mercht商家数据/缅甸.db");

        }

  
   


        //  http://localhost:5000/getDetail?id=avymrhifuyzkfetlnifryraazk
        public static string Wbapi_getDetail(string qrystr)
        {
            //  print("Received getlist: " + callGetlistFromDb);
            //  return Results.Ok("OK");
            SortedList qrystrMap = GetHashtableFromQrystr(qrystr);
            int page = gtfldInt(qrystrMap, "page", 0);
            int pagesize = gtfldInt(qrystrMap, "pagesize", 10);
            SortedList map = new SortedList();
            map.Add("limit", 5);

            Func<SortedList, bool> whereFun = castQrystr2FltrCdtFun(qrystr);
            var list = getListFltr("mercht商家数据", null, whereFun);
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
            foreach_hstbEs(list2, (SortedList rw) =>
            {
                rw.Add("pinlun", ormJSonFL.qrySglFL("pinlunDir/" + qrystrMap["id"] + ".json"));
                rw.Add("dafen", "555");
            });
            return encodeJson(list2);
        }


    }
}
