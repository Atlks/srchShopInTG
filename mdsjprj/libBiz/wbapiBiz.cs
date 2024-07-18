global using static mdsj.libBiz.wbapiBiz;
using libx;
using Microsoft.Extensions.Primitives;
using prj202405;
using prj202405.lib;
using System;
using System.Collections;
using System.Collections.Generic;
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
            var list_aftFltr = arr_fltr(list, (SortedList row) =>
            {
                List<bool> li = new List<bool>();
                li.Add((isNotEmptyLianxi(row)));
                //   li.Add((isLianxifshValid(row)));
                return isChkfltrOk(li);

            });
            int start = (page - 1) * pagesize;
            //if (start < 0)
            //    start = 0;

            var list_rzt = SliceX(list_aftFltr, start, pagesize);
            return encodeJson(list_rzt);
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
            var list3 = arr_fltr(list, (SortedList row) =>
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
