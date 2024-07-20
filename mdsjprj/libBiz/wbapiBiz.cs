﻿global using static mdsj.libBiz.wbapiBiz;
using libx;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using prjx;
using prjx.lib;
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
            SortedList qrystrMap = getHstbFromQrystr(qrystr);
            qrystrMap["id"] = qrystrMap["uid"];
            string dbfile = "parkcfgDir/uid_" + qrystrMap["uid"] + ".json";
            ormJSonFL.save(qrystrMap, dbfile);
            return "ok";
        }

        /// <summary>
        /// wbapi_upldPOST
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public static void wbapi_upldPOST(HttpRequest request, HttpResponse response)
        {
            // Check if the request contains a file
            if (request.Form.Files.Count > 0)
            {
                foreach (var file in request.Form.Files)
                {
                    // Get the file content and save it to a desired location
                    var filePath = Path.Combine("uploads", file.FileName);
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
            SortedList dafenObj = ConvertFormToSortedList(request.Form);
            ormJSonFL.save(dafenObj, "uplodData/" + Guid.NewGuid().ToString() + ".json");
            SendResp("ok", response);
            //    return "ok";
        }

        /// <summary>
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
            SortedList dafenObj = getHstbFromQrystr(qrystr);
            ormJSonFL.save(dafenObj, "dafenDatadir/" + dafenObj["shangjiaID"] + ".json");
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
            SortedList qrystrMap = getHstbFromQrystr(qrystr);
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
            ormJSonFL.save(obj1, "pinlunDir/" + qrystrMap["shangjiaID"] + ".json");

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
            SortedList qrystrHstb = getHstbFromQrystr(qrystr);
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
            SortedList dafenObj = getHstbFromQrystr(qrystr);
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


            SortedList<string, string> transmap = LoadSortedListFromIni($"{prjdir}/cfg字段翻译表/字段表.ini");
            // 循环 List<SortedList<string, int>> 并设置每个 SortedList 的 "aaa" 键的值为 1
            foreach (var sortedList in list_rzt)
            {
              //  if (sortedList.ContainsKey("aaa"))
                {
                    SetField938(sortedList, "Scores", 3);
                    SetField938(sortedList, "pages", CalculateTotalPages(pagesize, list_aftFltr.Count));
                      //  sortedList["aaa"] = 1;
                }
            }

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
                    var Cnkey = key;
                    var val = sortedList[key];
                    SetField938(map3, Cnkey.ToString(), val);


                    var keyEng = LoadFieldDefEmpty(transmap, Cnkey);
                    if (keyEng == "")
                        keyEng = Cnkey.ToString();


                    SetField938(map3, keyEng, val);

                    if (IsNumeric((val)))
                    {
                        double objSave = ConvertStringToNumber(val);
                        SetField938(map3, keyEng, objSave);
                    }
                     
                    //   Console.WriteLine($"Key: {key}, Value: {sortedList[key]}");
                  
                }
                list_rzt_fmt.Add(map3);
            }
            return encodeJson(list_rzt_fmt);
        }
        static double ConvertStringToNumber(object str2)
        {
            try
            {
                string str = ToStr(str2);

                return (double.Parse(str));
            }catch(Exception e)
            {
                return 0;
            }
         
           
        }
        private static string ToStr(object val)
        {
            if (val == null)
                return "";
            else
                return val.ToString();
        }

        public static SortedList<string, string> LoadSortedListFromIni(string filePath)
        {
            var result = new SortedList<string, string>(StringComparer.OrdinalIgnoreCase);

            // 读取文件所有行
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();

                // 跳过空行和注释行
                if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith(";") || trimmedLine.StartsWith("#"))
                {
                    continue;
                }

                // 处理键值对
                int equalsIndex = trimmedLine.IndexOf('=');
                if (equalsIndex > 0)
                {
                    string key = trimmedLine.Substring(0, equalsIndex).Trim();
                    string value = trimmedLine.Substring(equalsIndex + 1).Trim();
                    result[key] = value;
                }
            }

            return result;
        }

        public static int CalculateTotalPages(int pageSize, int totalRecords)
        {
            if (pageSize <= 0)
            {
                pageSize = 10;
            }

            if (totalRecords < 0)
            {
                totalRecords = 0;
            }

            return (int)Math.Ceiling((double)totalRecords / pageSize);
        }


        //  http://localhost:5000/getDetail?id=avymrhifuyzkfetlnifryraazk
        public static string Wbapi_getDetail(string qrystr)
        {
            //  print("Received getlist: " + callGetlistFromDb);
            //  return Results.Ok("OK");
            SortedList qrystrMap = getHstbFromQrystr(qrystr);
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
