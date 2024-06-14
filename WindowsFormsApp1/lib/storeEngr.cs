using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static mdsj.lib.exCls;
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
using static prj202405.lib.corex;
using static libx.storeEngr;
using static libx.funCls;
namespace libx
{
    internal class storeEngr
    {

        public static int save2storeFLByNodejs(SortedList mereed, string saveDataDir, Dictionary<string, string> prtnKeyCfg, SortedList dbg)
        {
            SortedList prm = new SortedList();

            string prjDir = @"../../";
         
         //   prm.Add("fun", scriptPath);

          
            prm.Add("saveobj", mereed);



            string prtnKey = prtnKeyCfg["prtnKey"];


            prm.Add("dbf", ($"{saveDataDir}\\{mereed[prtnKey]}.db"));
            prm.Add("dbg", dbg);

            string scriptPath = $"{prjDir}\\sqltnode\\save.js";
            string str = funCls.call(scriptPath, prm);
            return int.Parse(str);
        }

      

        public static List<SortedList> rdFrmStoreEngrFrmNodejsRdSqlt(string dbf)
        {
            if (!dbf.EndsWith(".db"))
            {
                string ext = ".db";
                dbf = dbf + ext;
            }

            SortedList prm = new SortedList();

            //   prm.Add("partns", ($"{mrchtDir}\\{partns}"));


            prm.Add("dbf", ($"{dbf}"));

            string prjDir = @"../../";
            string scriptPath = $"{prjDir}\\sqltnode\\qry.js";
            string txt = callRetList(scriptPath,prm);
            List<SortedList> li = json_decode(txt);
            return li;
        }

       
    }
}
