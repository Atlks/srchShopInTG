using prj202405.lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//using static mdsj.other;
//using static mdsj.clrCls;
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
using System.IO;
using Microsoft.Extensions.Primitives;
using Mono.Web;
using System.Collections.Specialized;
using System.Security.Policy;
namespace WindowsFormsApp1
{
    [ComVisible(true)]
    public class ScriptManager
    {
        private Form1 form;

        public ScriptManager(Form1 form)
        {
            this.form = form;
        }

        public string list_click(string mrchName = "", string partns = "")
        {

            string soluDir = @"../../../";
            soluDir = filex.GetAbsolutePath(soluDir);
            var dataDir = $"{soluDir}\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据";
            Func<SortedList, bool> whereFun = (SortedList row) =>
            {
                if (string.IsNullOrEmpty(mrchName))
                    return true;
                if (row["商家"].ToString().ToLower().Contains(mrchName.ToLower()))
                    return true;
                return false;
            };
            string prtnFileExt = "db";
            //from xxx partion(aa,bb) where xxx
            List<SortedList> rztLi = qry888(dataDir, partns, whereFun, null,null,prtnFileExt);


            //ormSqlt.qryV2("D:\\0prj\\mdsj\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据\\缅甸.db");
            return json_encode(rztLi);
        }


        public string save_click(string urlqryStr)
        {
            SortedList sortedListNew = urlqry2hashtb(urlqryStr);

            SortedList dbg = new SortedList();
            dbg.Add("urlqryStr", urlqryStr);


            string soluDir = @"../../../";
            soluDir = filex.GetAbsolutePath(soluDir);
            var mrchtDir = $"{soluDir}\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据";
            string prtnFileExt = "db";
            Func<SortedList, string> setStrEngr = (SortedList row) =>
            {
              
                const string prtnKey = "国家";
           
                string strx = save2storeFLByNodejs(row, mrchtDir, prtnKey, prtnFileExt, dbg);
                return strx;
            };
            string str = db.save(sortedListNew, setStrEngr, mrchtDir, prtnFileExt,dbg);
            MessageBox.Show("添加成功!");
            return str;
            //  SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            //ormSqlt.save(sortedList, "D:\\0prj\\mdsj\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据\\缅甸.db");

            //  Dictionary<string, StringValues> whereExprsObj = QueryHelpers.ParseQuery(urlqryStr);
            //   MessageBox.Show(" function called from HTML frm scrptmng!");
        }



        public void m1()
        {
            MessageBox.Show(" function called from HTML frm scrptmng!");
        }
    }
}
