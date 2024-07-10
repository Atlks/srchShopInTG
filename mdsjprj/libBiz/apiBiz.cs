global using static mdsj.libBiz.apiBiz;
using libx;
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


        // http://localhost:5000/getlistPinlun?shangjiaID=avymrhifuyzkfetlnifryraazk 
        public static string Wbapi_getlistPinlun(string qrystr)
        {
            SortedList qrystrHstb = getHstbFromQrystr(qrystr);
            var li = ormJSonFL.qrySglFL("pinlunDir/" + qrystrHstb["shangjiaID"] + ".json");
            return encodeJson(li);
        }

        // http://localhost:5000/pinlun?shangjiaID=avymrhifuyzkfetlnifryraazk&pinlun=465464564646

        public static string Wbapi_pinlun(string qrystr)
        {
            //  print("Received getlist: " + callGetlistFromDb);
            //  return Results.Ok("OK");
            SortedList dafenObj = getHstbFromQrystr(qrystr);
            SortedList obj1 = new SortedList();
            CopySortedList(dafenObj, obj1);
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

        //   http://localhost:5000/getlist?id=avymrhifuyzkfetlnifryraazk
        //   http://localhost:5000/getlist?分类=娱乐&page=1
        public static string Wbapi_getlist(string qrystr)
        {
            //  print("Received getlist: " + callGetlistFromDb);
            //  return Results.Ok("OK");
            SortedList dafenObj = getHstbFromQrystr(qrystr);
            SortedList map = new SortedList();
            map.Add("limit", 5);

            Func<SortedList, bool> whereFun = (SortedList row) =>
            {
                if (row["园区"].ToString().Contains("东风"))
                    print("dbg");

                List<Filtr> li = new List<Filtr>();
                //  li.Add(new Filtr(isNotEmptyLianxi(row)));
                //   li.Add(new Filtr(isLianxifshValid(row)));

                Dictionary<string, string> filters = ldDic4qryCdtn(qrystr);
                foreach_DictionaryKeys(filters, (string key) =>
                {
                    li.Add(new Filtr(isFldValEq111(row, key, filters)));
                });

                //li.Add(new Filtr(isFldValEq111(row, "园区", filters)));
                //li.Add(new Filtr(isFldValEq111(row, "国家", filters)));

                if (!ChkAllFltrTrue(li))
                    return false;
                return true;
            };
            var list = getListFltr("mercht商家数据", null, whereFun);
            int start = (page - 1) * 10;
            //if (start < 0)
            //    start = 0;

            var list2 = SliceX(list, start, 10);
            return encodeJson(list2);
        }


    }
}
