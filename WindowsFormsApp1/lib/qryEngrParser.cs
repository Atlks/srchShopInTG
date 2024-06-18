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
using static libx.qryEngrParser;
using static libx.storeEngr;
namespace libx
{
    internal class qryEngrParser
    {

        public static int Qe_del(string id, string fromDdataDir, Func<string, List<SortedList>> strE4rd, Func<(SortedList, string), int> callFunStrEngr)
        {

            var patns_dbfs = _calcPatnsV3(fromDdataDir, "");
            string[] arr = patns_dbfs.Split(',');

            int n = 0;
            foreach (string dbf in arr)
            {
                Func<SortedList, bool> whereFun = (SortedList row) =>
                {
                    if (string.IsNullOrEmpty(id))
                        return false;
                    if (row["id"].ToString().ToLower() == (id.ToLower()))
                        return true;
                    return false;
                };

                //= _qryBySnglePart(dbf, whereFun, cfgStrEngr);
                //rztLi = arrCls.array_merge(rztLi, li);
                List<SortedList> delneedRowSX = _qryBySnglePart(dbf, whereFun, strE4rd);
                //   var tuple = (id: id, dbf: dbf);
                //   var tuple = (id: id, dbf: dbf);
                foreach (SortedList delneedRow in delneedRowSX)
                {
                    n = n + callFunStrEngr((rw: delneedRow, dbf: dbf));
                }

            }

            return n;
        }
        public static List<SortedList> Qe_qry(string fromDdataDir, string partnsExprs, Func<SortedList, bool> whereFun, Func<string, List<SortedList>> cfgStrEngr)
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

        public static int Qe_save(SortedList sortedListNew, string dataDir, Func<string, List<SortedList>> cfgStrEngr4rd, Func<SortedList, int> callFunStrEngr, SortedList dbg = null)
        {
            SortedList mereed = new SortedList();
            if (sortedListNew.ContainsKey("id") && sortedListNew["id"].ToString().Trim().Length > 0)//updt mode
            {
                return qe_merge(sortedListNew, dataDir, cfgStrEngr4rd, callFunStrEngr);
            }
            else
            {//new
                return qe_add(sortedListNew, dataDir, callFunStrEngr);

            }
        }

        public static int Qe_saveOrUpdtMerge(SortedList sortedListNew, string dataDir, Func<string, List<SortedList>> cfgStrEngr4rd, Func<SortedList, int> callFunStrEngr, SortedList dbg = null)
        {
            SortedList mereed = new SortedList();
            if (sortedListNew.ContainsKey("id") && sortedListNew["id"].ToString().Trim().Length > 0)//updt mode
            {
                return qe_merge(sortedListNew, dataDir, cfgStrEngr4rd, callFunStrEngr);
            }
            else
            {//new
                return qe_add(sortedListNew, dataDir, callFunStrEngr);

            }
        }

        public static int Qe_saveOrUpdtReplace(SortedList sortedListNew, string dataDir, Func<string, List<SortedList>> cfgStrEngr4rd, Func<SortedList, int> callFunStrEngr, SortedList dbg = null)
        {
            SortedList mereed = new SortedList();
            if (sortedListNew.ContainsKey("id") && sortedListNew["id"].ToString().Trim().Length > 0)//updt mode
            {
                return qe_replace(sortedListNew, dataDir,  callFunStrEngr);
            }
            else
            {//new
                return qe_add(sortedListNew, dataDir, callFunStrEngr);

            }
        }

        private static int qe_replace(SortedList sortedListNew, string dataDir, Func<SortedList, int> callFunStrEngr)
        {   
           
            int str = callFunStrEngr(sortedListNew);
            return str;
        }

        private static int qe_merge(SortedList sortedListNew, string dataDir, Func<string, List<SortedList>> cfgStrEngr4rd, Func<SortedList, int> callFunStrEngr)
        {

            SortedList old = (Qe_find(sortedListNew["id"].ToString(), dataDir, null, cfgStrEngr4rd));
            SortedList mereed = CopyToOldSortedList(sortedListNew, old);
            int str = callFunStrEngr(mereed);
            return str;
        }

        private static int qe_add(SortedList sortedListNew, string dataDir, Func<SortedList, int> callFunStrEngr)
        {
            //  mereed = sortedListNew;
            // 获取当前时间并格式化为文件名
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            sortedListNew["id"] = timestamp;
            int str = callFunStrEngr(sortedListNew);
            return str;
        }

        // str eng is find_current_row
        public static SortedList Qe_find(string id, string dataDir, string partns, Func<string, List<SortedList>> cfgStrEngrx)
        {

            Func<SortedList, bool> whereFun = (SortedList row) =>
            {
                if (string.IsNullOrEmpty(id))
                    return false;
                if (row["id"].ToString().ToLower() == (id.ToLower()))
                    return true;
                return false;
            };

            //from xxx partion(aa,bb) where xxx
            List<SortedList> rztLi = Qe_qry(dataDir, partns, whereFun, cfgStrEngr: cfgStrEngrx);


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