global using static mdsj.libBiz.apiBiz;
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
    internal class apiBiz
    {
        //   http://localhost:5000/dafen?shangjiaID=yourValue11&dafen=3&uid=007
        public static string Wbapi_dafen(string qrystr)
        {
            //shangjiaID,uid,dafen
            SortedList dafenObj = getHstbFromQrystr(qrystr);
            ormJSonFL.save(dafenObj, "dafenDatadir/" + dafenObj["shangjiaID"] + ".json");
            return "ok";
        }
        public static string Wbapi_pinlun(string qrystr)
        {
            //  print("Received getlist: " + callGetlistFromDb);
            //  return Results.Ok("OK");
            SortedList dafenObj = getHstbFromQrystr(qrystr);
            SortedList obj1 = new SortedList();
            obj1.Add("id", DateTime.Now.ToString());
            obj1.Add("商家guid", dafenObj["shangjiaID"]);
            //    obj1.Add("商家", merchant.Name);
            obj1.Add("时间", DateTime.Now.ToString());
            obj1.Add("评论内容", dafenObj["pinlun"]);
            //    obj1.Add("评论人", update.Message.From.Username);
            obj1.Add("评论人id", dafenObj["uid"]);
            System.IO.Directory.CreateDirectory("pinlunDir");
            //    ormSqlt.save(obj1, "pinlunDir/" + merchant.Guid + merchant.Name + ".db");
            ormJSonFL.save(obj1, "pinlunDir/" + dafenObj["shangjiaID"] + ".json");

            //    ormJSonFL.save(dafenObj, "dafenDatadir/" + dafenObj["shangjiaID"] + ".json");
            return "ok";
        }


        public static string Wbapi_getlist(string qrystr)
        {
            //  print("Received getlist: " + callGetlistFromDb);
            //  return Results.Ok("OK");
            SortedList dafenObj = getHstbFromQrystr(qrystr);
            int page = gtfldInt(dafenObj, "page", 0);
            SortedList map = new SortedList();
            map.Add("limit", 5);
            var list = getListFltr("mercht商家数据", null, null);
            int start = (page - 1) * 10;

            list = list.Slice(start, 10);
            return encodeJson(list);
        }

    }
}
