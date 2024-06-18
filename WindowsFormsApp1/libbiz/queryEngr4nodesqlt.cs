using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WindowsFormsApp1.libbiz.queryEngr4nodesqlt;
using static libx.qryEngrParser;
using static libx.storeEngr;
using static mdsj.lib.encdCls;
//using static mdsj.other;
//using static mdsj.clrCls;
using static prj202405.lib.corex;
using static prj202405.lib.strCls;
using static prj202405.lib.arrCls;
using libx;
using prj202405.lib;
namespace WindowsFormsApp1.libbiz
{
    internal class queryEngr4nodesqlt
    {
        public static List<SortedList> Qe_qry4NodeSqltMode(string fromDdataDir, string partns, Func<SortedList, bool> whereFun)
        {
            Func<string, List<SortedList>> cfgStrEngr = rdStrEngr(); ;
            return Qe_qry(fromDdataDir, partns, whereFun, cfgStrEngr: cfgStrEngr);
        }

        private static Func<string, List<SortedList>> rdStrEngr()
        {
            return (string prtnDbfNoExt) =>
            {
                return rnd_next(prtnDbfNoExt);
            };
        }

        public static SortedList Qe_find4nodesqlt(string id, string dataDir)
        {
            Func<string, List<SortedList>> cfgStrEngr = rdStrEngr();
            return Qe_find(id, dataDir, null, cfgStrEngr);
        }

        internal static int Qe_save4nodeSqlt(SortedList sortedListNew, string saveDataDir)
        {
            Func<SortedList, int> caFun_ivkStrEngr = (SortedList row) =>
            {
                var dbg =new SortedList();
                string prtnKey = "国家";
                string wrtFile = $"{saveDataDir}\\{row[prtnKey]}.db";
                int strx = storeEngr.write_row(row, wrtFile, dbg);
                return strx;
            };

            Func<string, List<SortedList>> cfgStrEngr4rd = rdStrEngr();
            return Qe_save(sortedListNew, saveDataDir, cfgStrEngr4rd, caFun_ivkStrEngr);
        }

        internal static int Qe_del4nodeSqlt(string id, string saveDataDir)
        {
            Func<(SortedList, string), int> callFun_ivkStrEngr = ((SortedList, string) tpl) =>
            {
                Console.WriteLine(tpl);
                //del row dbf retVal
                int strx = storeEngr.delete_row(tpl.Item1, tpl.Item2);
                return strx;
            };
          return Qe_del(id, saveDataDir, rdStrEngr(), callFun_ivkStrEngr);   
        }
    }
}
