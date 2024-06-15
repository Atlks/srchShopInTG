using libx;
using prj202405.lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static libx.qryEngrParser;
using static libx.storeEngr;
using static mdsj.lib.encdCls;
//using static mdsj.other;
//using static mdsj.clrCls;
using static prj202405.lib.corex;
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
            var fromDdataDir = $"{soluDir}\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据";
            Func<SortedList, bool> whereFun = (SortedList row) =>
            {
                if (string.IsNullOrEmpty(mrchName))
                    return true;
                if (row["商家"].ToString().ToLower().Contains(mrchName.ToLower()))
                    return true;
                return false;
            };
           
            //from xxx partion(aa,bb) where xxx


            Func<string, List<SortedList>> cfgStrEngr = (string prtnDbfNoExt) =>
            {
                return   rd(prtnDbfNoExt);               
            };
            List<SortedList> rztLi = qryEngrParser.qry(fromDdataDir, partns, whereFun, cfgStrEngr: cfgStrEngr);


            //ormSqlt.qryV2("D:\\0prj\\mdsj\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据\\缅甸.db");
            return json_encode(rztLi);
        }

        public string find(string id)
        {

            string soluDir = @"../../../";
            soluDir = filex.GetAbsolutePath(soluDir);
             var dataDir = $"{soluDir}\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据";
            Func<string, List<SortedList>> cfgStrEngr = (string prtnDbfNoExt) =>
            {
                return rd(prtnDbfNoExt);
            };
            SortedList results = find24614(id, dataDir ,null, cfgStrEngr);



            return json_encode(results);
        }


            public string save_click(string urlqryStr)
        {
            SortedList sortedListNew = urlqry2hashtb(urlqryStr);

            SortedList dbg = new SortedList();
            dbg.Add("urlqryStr", urlqryStr);


            string soluDir = @"../../../";
            soluDir = filex.GetAbsolutePath(soluDir);
            var saveDataDir = $"{soluDir}\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据";


            Dictionary<string, string> prtnCfg = new Dictionary<string, string>();
            prtnCfg.Add("prtnKey", "国家");
            prtnCfg.Add("filetype", "db");  //sqlt
            Func<SortedList, int> callFun_ivkStrEngr = (SortedList row) =>
            {                
                string prtnKey = "国家";
                string wrtFile = $"{saveDataDir}\\{row[prtnKey]}.db";
                int strx =storeEngr. write(row, wrtFile, dbg);
                return strx;
            };
            //prtn cfg also trans into  save24614
            int str = qryEngrParser. save24614(sortedListNew,  saveDataDir, callFun_ivkStrEngr, dbg);
            MessageBox.Show("添加成功!");
            return str.ToString();
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
