using libx;
using prjx.lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static libx.qryEngrParser;
using static libx.storeEngr4Nodesqlt;
using static mdsj.lib.encdCls;
//using static mdsj.other;
//using static mdsj.clrCls;
using static prjx.lib.corex;
using static prjx.lib.strCls;
using static prjx.lib.arrCls;

using static WindowsFormsApp1.libbiz.strengFunRefCls;
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




            List<SortedList> rztLi =   Qe_qry(fromDdataDir, partns, whereFun, rndFun: rnd4nodeSqltRef());
          //  Qe_qry4NodeSqltMode(fromDdataDir, partns, whereFun);
          foreach( SortedList results in rztLi)
            {
                results["Telegram"] = trim_RemoveUnnecessaryCharacters(TryGetValueAsStrDfEmpty(results, "Telegram"));
                results["WhatsApp"] = trim_RemoveUnnecessaryCharacters(arrCls.TryGetValueAsStrDfEmpty(results, "WhatsApp"));
                results["微信"] = trim_RemoveUnnecessaryCharacters(arrCls.TryGetValueAsStrDfEmpty(results, "微信"));

            }

            //ormSqlt.qryV2("D:\\0prj\\mdsj\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据\\缅甸.db");
            return json_encode(rztLi);
        }

        public int del_click(string id)
        {
            var obj = new SortedList();
            obj.Add("id", id);
            //  { "id":id};
            string soluDir = @"../../../";
            soluDir = filex.GetAbsolutePath(soluDir);
            var dataDir = $"{soluDir}\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据";
            return Qe_delV2(id, dataDir,nameof  (delete_row4nodeSqlt));

        }

            public string find(string id)
        {

            string soluDir = @"../../../";
            soluDir = filex.GetAbsolutePath(soluDir);
            var dataDir = $"{soluDir}\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据";
           
            SortedList results = Qe_find(id, dataDir, null, rnd4nodeSqltRef());// Qe_find4nodesqlt(id, dataDir);
          
            results["Telegram"] = trim_RemoveUnnecessaryCharacters(TryGetValueAsStrDfEmpty(results, "Telegram"));
            results["WhatsApp"] = trim_RemoveUnnecessaryCharacters(arrCls.TryGetValueAsStrDfEmpty(results, "WhatsApp"));
            results["微信"] = trim_RemoveUnnecessaryCharacters(arrCls.TryGetValueAsStrDfEmpty(results, "微信"));


            return json_encode(results);
        }

       

        public string del_clck(string id)
        {

            string soluDir = @"../../../";
            soluDir = filex.GetAbsolutePath(soluDir);
            var saveDataDir = $"{soluDir}\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据";


            

            
            //prtn cfg also trans into  save24614
            int str =    Qe_del(id, saveDataDir, rnd4nodeSqltRef(), del_row4nodeSqltRef());
            return str.ToString();

        }
      
        public string save_click(string urlqryStr)
        {
            SortedList sortedListNew = urlqry2hashtb(urlqryStr);

            SortedList dbg = new SortedList();
            dbg.Add("urlqryStr", urlqryStr);


            string soluDir = @"../../../";
           
            soluDir = filex.GetAbsolutePath(soluDir);
            soluPath = soluDir;
            var saveDataDir = $"{soluDir}\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据";


         
           
            //prtn cfg also trans into  save24614
            int str =Qe_saveOrUpdtMerge(sortedListNew, saveDataDir, rnd4nodeSqltRef(), wrt_row4nodeSqltRef( saveDataDir));
            //Qe_saveOrUpdtMrgr4nodeSqlt(sortedListNew, saveDataDir);
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
