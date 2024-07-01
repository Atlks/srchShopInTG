global using static mdsj.lib.qry_share;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdsj.lib
{
    internal class qry_share
    {
        //
        public static SortedList getShareCfg(string FromdataDir, string shareName)
        {
            SortedList cfg4curDatatype = getShareCfg4table(FromdataDir);
            Console.WriteLine(json_encode(cfg4curDatatype));

          //  SortedList cfg4curDatatype= shareCfgList[]
            SortedList? sortedList = (SortedList)cfg4curDatatype[shareName];
         
            return sortedList;
        }

        public static SortedList getShareCfg4table(string dataType)
        {

            //SortedList shareCfg1 = new SortedList();
            //shareCfg1.Add("name", "缅甸");
            //shareCfg1.Add("fmt", "sqlt");
            //shareCfg1.Add("storeEngr", "rnd_next4SqltRf");
            ////  shareCfg1.Add("storeEngr", rnd_next4SqltRf());
            //SortedList shareCfg2 = new SortedList();
            //shareCfg2.Add("name", "老挝");
            //shareCfg2.Add("fmt", "json");
            //shareCfg2.Add("storeEngr", "rnd4jsonFlRf");
       // public static List<SortedList> rnd_next4Sqlt(string dbf)
            ////     shareCfg2.Add("storeEngr", rnd4jsonFlRf());
            /////-----cfg  mercht商家数据 share
            var mymm4shareCfg = "name=缅甸&fmt=sqlt&rndFun=" + nameof(rnd_next4Sqlt);
            var laos4shareCfg = "name=老挝&fmt=json&rndFun=" + nameof(rnd4jsonFl);
            SortedList shareCfgList4dataDir = new SortedList();
            SortedList valueMM = castUrlQueryString2hashtable(mymm4shareCfg);
            shareCfgList4dataDir.Add("缅甸", valueMM);
            shareCfgList4dataDir.Add("老挝", castUrlQueryString2hashtable(laos4shareCfg));

            SortedList cfgFnal = new SortedList();
            cfgFnal.Add("mercht商家数据",shareCfgList4dataDir);




            return (SortedList)cfgFnal[dataType];
        }
    }
}
