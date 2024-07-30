global using static WindowsFormsApp1.libbiz.storeEngFunRefCls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static libx.qryEngrParser;
using static libx.storeEngr4Nodesqlt;
using static mdsj.lib.bscEncdCls;
//using static mdsj.other;
//using static mdsj.clrCls;
using static prjx.lib.corex;
using static prjx.lib.strCls;
using static prjx.lib.arrCls;
using libx;
using prjx.lib;
namespace WindowsFormsApp1.libbiz
{
    internal class storeEngFunRefCls
    {
      
        public static Func<string, List<SortedList>> rnd4nodeSqltRef()
        {
            return (string prtnDbfNoExt) =>
            {
                return rnd_next4nodeSqlt(prtnDbfNoExt);
            };
        }
        public static Func<string, List<SortedList>> rnd4jsonFlRf()
        {
            Func<string, List<SortedList>> rndFun = (dbf) =>
            {
                return rnd4jsonFl(dbf);
            };
            return rndFun;
        }



        public static Func<SortedList, bool> castFltlst2whereFun(List<Filtr> li)
        {
            return (SortedList row) =>
            {


                //li.Add(new Condtn(isLianxifshValid(row)));
                //li.Add(new Condtn(isFldValEq111(row, "城市", whereExprsObj)));
                //li.Add(new Condtn(isFldValEq111(row, "园区", whereExprsObj)));
                //li.Add(new Condtn(isFldValEq111(row, "国家", whereExprsObj)));
                //li.Add(new Condtn(isCotainFuwuci(row, msgCtain)));
                //li.Add(new Condtn(msgHasPostWd && isCotainPostnWd(row, kwds)));
                if (!ChkAllFltrTrueDep(li))
                    return false;
                return true;
            };
        }

        public static Func<SortedList, int> wrt_row4nodeSqltRef(string saveDataDir)
        {
            return (SortedList row) =>
            {
                var dbg = new SortedList();
                string prtnKey = "国家";
                string wrtFile = $"{saveDataDir}\\{row[prtnKey]}.db";
                int strx = storeEngr4Nodesqlt.write_row4nodeSqlt(row, wrtFile, dbg);
                return strx;
            };
        }

       

        public static Func<(SortedList, string), int> del_row4nodeSqltRef  ()
        {
            Func<(SortedList, string), int> del_rowFun = ((SortedList, string) tpl) =>
            {
               Print(tpl);
                //del row dbf retVal
                int strx = storeEngr4Nodesqlt.delete_row4nodeSqlt(tpl.Item1, tpl.Item2);
                return strx;
            };
            return del_rowFun;
        }

        /// <summary>
        ///         Func<string, List<SortedList>>
        /// </summary>
        /// <returns></returns>
        public static Func<string, List<SortedList>> rnd_next4SqltRf()
        {
            Func<string, List<SortedList>> rndFun = (dbf) =>
            {
                return rnd_next4Sqlt(dbf);
            };
            return rndFun;
        }



        //public static SortedList Qe_find4nodesqlt(string id, string dataDir)
        //{
        //    Func<string, List<SortedList>> cfgStrEngr = rndFun4nodeSqlt();
        //    return Qe_find(id, dataDir, null, rndFun4nodeSqlt());
        //}

        //internal static int Qe_saveOrUpdtMrgr4nodeSqlt(SortedList sortedListNew, string saveDataDir)
        //{
        //    Func<SortedList, int> wrt_rowFun = getWrtRowFun(saveDataDir);

        //    Func<string, List<SortedList>> cfgStrEngr4rd = rndFun4nodeSqlt();
        //    return Qe_saveOrUpdtMerge(sortedListNew, saveDataDir, rndFun4nodeSqlt(), wrt_rowFun);
        //}
        //internal static int Qe_del4nodeSqlt(string id, string saveDataDir)
        //{
        //    Func<(SortedList, string), int> del_rowFun = del_rowFun();
        //    return Qe_del(id, saveDataDir, rndFun4nodeSqlt(), queryEngr4nodesqlt.del_rowFun());
        //}
        //public static List<SortedList> Qe_qry4NodeSqltMode(string fromDdataDir, string partns, Func<SortedList, bool> whereFun)
        //{
        //    Func<string, List<SortedList>> cfgStrEngr = rndFun4nodeSqlt(); ;
        //    return Qe_qry(fromDdataDir, partns, whereFun, rndFun: rndFun4nodeSqlt());
        //}

    }
}
