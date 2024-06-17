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


            Func<string, List<SortedList>> cfgStrEngr = getRdStrEngr();
            //return (string prtnDbfNoExt) =>
            //{
            //    return rnd_next(prtnDbfNoExt);
            //};

            List<SortedList> rztLi = qryEngrParser.Qe_qry(fromDdataDir, partns, whereFun, cfgStrEngr: cfgStrEngr);


            //ormSqlt.qryV2("D:\\0prj\\mdsj\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据\\缅甸.db");
            return json_encode(rztLi);
        }

        private Func<string, List<SortedList>> getRdStrEngr()
        {
            return (string prtnDbfNoExt) =>
            {
                return rnd_next(prtnDbfNoExt);
            };
        }

        public string find(string id)
        {

            string soluDir = @"../../../";
            soluDir = filex.GetAbsolutePath(soluDir);
             var dataDir = $"{soluDir}\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据";
            Func<string, List<SortedList>> cfgStrEngr = (string prtnDbfNoExt) =>
            {
                return rnd_next(prtnDbfNoExt);
            };
            SortedList results = Qe_find(id, dataDir ,null, cfgStrEngr);
            results["Telegram"] = trim_RemoveUnnecessaryCharacters(TryGetValueAsStrDfEmpty( results,"Telegram"));
                 results["WhatsApp"] = trim_RemoveUnnecessaryCharacters(arrCls.TryGetValueAsStrDfEmpty(results, "WhatsApp"));


            return json_encode(results);
        }

        public string del_clck(string id)
        {

            string soluDir = @"../../../";
            soluDir = filex.GetAbsolutePath(soluDir);
            var saveDataDir = $"{soluDir}\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据";
         

            Func<string, List<SortedList>> cfgStrEngr4rd = (string prtnDbfNoExt) =>
            {
                return rnd_next(prtnDbfNoExt);
            };


            Func<(SortedList, string), int> callFun_ivkStrEngr = ((SortedList, string) tpl) =>
            {
                Console.WriteLine(tpl);
                //del row dbf retVal
                int strx = storeEngr.delete_row(tpl.Item1,tpl.Item2);
                return strx;
            };
            //prtn cfg also trans into  save24614
            int str = qryEngrParser.Qe_del(id, saveDataDir, cfgStrEngr4rd, callFun_ivkStrEngr);
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


            Dictionary<string, string> prtnCfg = new Dictionary<string, string>();
            prtnCfg.Add("prtnKey", "国家");
            prtnCfg.Add("filetype", "db");  //sqlt
            Func<SortedList, int> callFun_ivkStrEngr = (SortedList row) =>
            {                
                string prtnKey = "国家";
                string wrtFile = $"{saveDataDir}\\{row[prtnKey]}.db";
                int strx =storeEngr. write_row(row, wrtFile, dbg);
                return strx;
            };

            Func<string, List<SortedList>> cfgStrEngr4rd = (string prtnDbfNoExt) =>
            {
                return rnd_next(prtnDbfNoExt);
            };
            //prtn cfg also trans into  save24614
            int str = qryEngrParser. Qe_save(sortedListNew,  saveDataDir, cfgStrEngr4rd, callFun_ivkStrEngr, dbg);
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
