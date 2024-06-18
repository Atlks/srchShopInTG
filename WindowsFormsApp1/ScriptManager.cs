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
using static prj202405.lib.strCls;
using static prj202405.lib.arrCls;

using static WindowsFormsApp1.libbiz.queryEngr4nodesqlt;
using WindowsFormsApp1.libbiz;
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




            List<SortedList> rztLi = Qe_qry4NodeSqltMode(fromDdataDir, partns, whereFun);


            //ormSqlt.qryV2("D:\\0prj\\mdsj\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据\\缅甸.db");
            return json_encode(rztLi);
        }

     

        //private Func<string, List<SortedList>> getRdStrEngr()
        //{
        //    return (string prtnDbfNoExt) =>
        //    {
        //        return rnd_next(prtnDbfNoExt);
        //    };
        //}

        public string find(string id)
        {

            string soluDir = @"../../../";
            soluDir = filex.GetAbsolutePath(soluDir);
            var dataDir = $"{soluDir}\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据";
           
            SortedList results = Qe_find4nodesqlt(id, dataDir);
            results["Telegram"] = trim_RemoveUnnecessaryCharacters(TryGetValueAsStrDfEmpty(results, "Telegram"));
            results["WhatsApp"] = trim_RemoveUnnecessaryCharacters(arrCls.TryGetValueAsStrDfEmpty(results, "WhatsApp"));


            return json_encode(results);
        }

       

        public string del_clck(string id)
        {

            string soluDir = @"../../../";
            soluDir = filex.GetAbsolutePath(soluDir);
            var saveDataDir = $"{soluDir}\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据";


            

            
            //prtn cfg also trans into  save24614
            int str = queryEngr4nodesqlt.Qe_del4nodeSqlt(id, saveDataDir);
            return str.ToString();

        }

        public string save_click(string urlqryStr)
        {
            SortedList sortedListNew = urlqry2hashtb(urlqryStr);

            SortedList dbg = new SortedList();
            dbg.Add("urlqryStr", urlqryStr);


            string soluDir = @"../../../";
            soluDir = filex.GetAbsolutePath(soluDir);
            var saveDataDir = $"{soluDir}\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据";


         
           
            //prtn cfg also trans into  save24614
            int str =queryEngr4nodesqlt. Qe_save4nodeSqlt(sortedListNew, saveDataDir);
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
