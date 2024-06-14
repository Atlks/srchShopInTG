using prj202405.lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
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
using static libx.qryParser;
using static libx.storeEngr;
namespace libx
{
    internal class qryParser
    {
        public static List<SortedList> qry(string fromDdataDir, string partnsExprs, Func<SortedList, bool> whereFun, Func<string, List<SortedList>> cfgStrEngr)
        {
            if (cfgStrEngr is null)
            {
                throw new ArgumentNullException(nameof(cfgStrEngr));
            }

            List<SortedList> rztLi = new List<SortedList>();
            var patns_dbfs = _calcPatnsV3(fromDdataDir, partnsExprs);
            string[] arr = patns_dbfs.Split(',');
            foreach (string dbf in arr)
            {
                List<SortedList> li = _qryBySnglePart(dbf, whereFun, cfgStrEngr);
                rztLi = arrCls.array_merge(rztLi, li);
            }

            return rztLi;
        }

        public static int save24614(SortedList sortedListNew, string dataDir, Func<SortedList, int> setStrEngrFun, SortedList dbg = null)
        {
            SortedList mereed = new SortedList();
            if (sortedListNew.ContainsKey("id") && sortedListNew["id"].ToString().Trim().Length > 0)//updt mode
            {
                SortedList old = (find24614(sortedListNew["id"].ToString(), dataDir));
                mereed = CopyToOldSortedList(sortedListNew, old);
            }
            else
            {//new
                mereed = sortedListNew;
                // 获取当前时间并格式化为文件名
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                sortedListNew["id"] = timestamp;
            }

            // string str = save2storeFLByNodejs( mereed, dbg);
            int str = setStrEngrFun(mereed);
            return str;
        }

        public static SortedList find24614(string id, string dataDir, string partns = null, Func<string, List<SortedList>> cfgStrEngrx = null)
        {

            Func<SortedList, bool> whereFun = (SortedList row) =>
            {
                if (string.IsNullOrEmpty(id))
                    return false;
                if (row["id"].ToString().ToLower().Contains(id.ToLower()))
                    return true;
                return false;
            };

            //from xxx partion(aa,bb) where xxx
            List<SortedList> rztLi = qry888(dataDir, partns, whereFun, cfgStrEngr: cfgStrEngrx);


            SortedList results = rztLi[0];
            return results;
        }



        internal static string _calcPatnsV3(string dir, string partfile区块文件)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), dir, partfile区块文件));

            //if (string.IsNullOrEmpty(Extname))
            //    Extname = "txt";
            if (string.IsNullOrEmpty(partfile区块文件))
            {

                string rzt = GetFilePathsCommaSeparated(dir);
                dbgCls.setDbgValRtval(__METHOD__, rzt);
                return rzt;
            }
            ArrayList arrayList = new ArrayList();
            string[] dbArr = partfile区块文件.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var dbf in dbArr)
            {
                string path = dir + "/" + dbf + "";
                //if (!File.Exists(path))
                //{
                //    Console.WriteLine("not exist file dbf=>" + path);
                //    continue;
                //}
                arrayList.Add(path);
            }

            // 使用 ArrayList 的 ToArray 方法将其转换为对象数组
            object[] objectArray = arrayList.ToArray();

            // 使用 String.Join 方法将数组转换为逗号分割的字符串
            string result = string.Join(",", objectArray);

            dbgCls.setDbgValRtval(__METHOD__, result);

            return result;
        }


        //单个分区ony need where ,,,bcs order only need in mergeed...and map_select maybe orderd,and top n ,,then last is need to selectMap op
        public static List<SortedList> _qryBySnglePart(string dbf, Func<SortedList, bool> whereFun, Func<string, List<SortedList>> cfgStrEngr)
        {
            List<SortedList> li = cfgStrEngr(dbf);

            li = db.qryV7(li, whereFun);
            return li;
        }
    }
}