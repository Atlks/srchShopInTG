global using static libx.qryEngrParser;

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

using static libx.storeEngr4Nodesqlt;
using Microsoft.Extensions.Primitives;
using prj202405;
using DocumentFormat.OpenXml.Wordprocessing;
using mdsj;
namespace libx
{
    internal class qryEngrParser
    {
        public static bool hasCondt(Dictionary<string, StringValues> whereExprsObj, string fld)
        {
            return whereExprsObj.ContainsKey(fld);

            //if (park4srch == null)
            //{
            //    return false;
            //}
            //return true;
        }
        public static int Qe_del(string id, string fromDdataDir, Func<string, List<SortedList>> rndFun, Func<(SortedList, string), int> del_row_Fun)
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
                List<SortedList> delneedRowSX = _qryBySnglePart(dbf, whereFun, rndFun);
                //   var tuple = (id: id, dbf: dbf);
                //   var tuple = (id: id, dbf: dbf);
                foreach (SortedList delneedRow in delneedRowSX)
                {
                    n = n + del_row_Fun((rw: delneedRow, dbf: dbf));
                }

            }

            return n;
        }

        //public static List<t> xxxx<t>()
        //{
        //    SortedList row = new SortedList();
        //    List<t> rsRztInlnKbdBtn = new List<t>();
        //    rsRztInlnKbdBtn.Add( row);
        //    return rsRztInlnKbdBtn;
        //}



        public static List<t> Qe_qryV2<t>(string fromDdataDir, string partnsExprs,
                    Func<SortedList, bool> whereFun,
                    Func<SortedList, int> ordFun,
                    Func<SortedList, t> selktFun,
                    Func<string, List<SortedList>> rndFun)
        {

            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.print_call(__METHOD__, dbgCls.func_get_args(fromDdataDir, partnsExprs));


            List<t> rsRztInlnKbdBtn = new List<t>();
            try
            {
                if (rndFun is null)
                {
                    throw new ArgumentNullException(nameof(rndFun));
                }

                List<SortedList> rztLi = new List<SortedList>();
                var patns_dbfs = _calcPatnsV3(fromDdataDir, partnsExprs);
                string[] arr = patns_dbfs.Split(',');
                foreach (string dbf in arr)
                {
                    List<SortedList> li = _qryBySnglePart(dbf, whereFun, rndFun);
                    rztLi = array_merge(rztLi, li);
                }

                if (ordFun != null)
                {
                    rztLi = rztLi.Cast<SortedList>()
                                   .OrderBy(ordFun)
                                   .ToList();
                }


                if (selktFun == null)
                {
                    throw new ArgumentNullException("  need slktfun ,u can use def slktfun");
                    //   dbgCls.setDbgValRtval(__METHOD__, 0);
                    //   return rztLi;

                }


                for (int i = 0; i < rztLi.Count; i++)
                {
                    SortedList row = rztLi[i];

                    rsRztInlnKbdBtn.Add(selktFun(row));


                }


            }
            catch (Exception e)
            {
                Console.WriteLine($"--ex catch---- mtth:{__METHOD__}((( {json_encode_noFmt(func_get_args(fromDdataDir, partnsExprs))}");
                Console.WriteLine(e);
                logErr2025(e, __METHOD__, "errdir");
                //  return rsRztInlnKbdBtn;
            }
            dbgCls.print_ret(__METHOD__, 0);
            return rsRztInlnKbdBtn;
        }

        public static List<SortedList> Qe_qry(string fromDdataDir, string partnsExprs, Func<SortedList, bool> whereFun, Func<string, List<SortedList>> rndFun)
        {
            if (rndFun is null)
            {
                throw new ArgumentNullException(nameof(rndFun));
            }

            List<SortedList> rztLi = new List<SortedList>();
            var patns_dbfs = _calcPatnsV3(fromDdataDir, partnsExprs);
            string[] arr = patns_dbfs.Split(',');
            foreach (string dbf in arr)
            {
                List<SortedList> li = _qryBySnglePart(dbf, whereFun, rndFun);
                rztLi = arrCls.array_merge(rztLi, li);
            }

            return rztLi;
        }

        public static int Qe_save(SortedList sortedListNew, string dataDir, Func<string, List<SortedList>> rndFun, Func<SortedList, int> wrt_rowFun, SortedList dbg = null)
        {
            SortedList mereed = new SortedList();
            if (sortedListNew.ContainsKey("id") && sortedListNew["id"].ToString().Trim().Length > 0)//updt mode
            {
                return qe_merge(sortedListNew, dataDir, rndFun, wrt_rowFun);
            }
            else
            {//new
                return qe_add(sortedListNew, dataDir, wrt_rowFun);

            }
        }

        public static int Qe_saveOrUpdtMerge(SortedList sortedListNew, string dataDir, Func<string, List<SortedList>> rndFun, Func<SortedList, int> wrt_row_fun, SortedList dbg = null)
        {
            SortedList mereed = new SortedList();
            if (sortedListNew.ContainsKey("id") && sortedListNew["id"].ToString().Trim().Length > 0)//updt mode
            {
                return qe_merge(sortedListNew, dataDir, rndFun, wrt_row_fun);
            }
            else
            {//new
                return qe_add(sortedListNew, dataDir, wrt_row_fun);

            }
        }

        public static int Qe_saveOrUpdtReplace(SortedList sortedListNew, string dataDir, Func<SortedList, int> wrt_rowFun, SortedList dbg = null)
        {
            SortedList mereed = new SortedList();
            if (sortedListNew.ContainsKey("id") && sortedListNew["id"].ToString().Trim().Length > 0)//updt mode
            {
                return qe_replace(sortedListNew, dataDir, wrt_rowFun);
            }
            else
            {//new
                return qe_add(sortedListNew, dataDir, wrt_rowFun);

            }
        }

        private static int qe_replace(SortedList sortedListNew, string dataDir, Func<SortedList, int> wrt_rowFun)
        {

            int str = wrt_rowFun(sortedListNew);
            return str;
        }

        private static int qe_merge(SortedList sortedListNew, string dataDir, Func<string, List<SortedList>> rndFun, Func<SortedList, int> wrt_rowFun)
        {

            SortedList old = (Qe_find(sortedListNew["id"].ToString(), dataDir, null, rndFun));
            SortedList mereed = CopyToOldSortedList(sortedListNew, old);
            int str = wrt_rowFun(mereed);
            return str;
        }

        private static int qe_add(SortedList sortedListNew, string dataDir, Func<SortedList, int> wrt_rowFun)
        {
            //  mereed = sortedListNew;
            // 获取当前时间并格式化为文件名
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            sortedListNew["id"] = timestamp;
            int str = wrt_rowFun(sortedListNew);
            return str;
        }

        // str eng is find_current_row
        public static SortedList Qe_find(string id, string dataDir, string partns, Func<string, List<SortedList>> rndFun)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.print_call(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), id, dataDir, partns));

            Func<SortedList, bool> whereFun = (SortedList row) =>
            {
                if (string.IsNullOrEmpty(id))
                    return false;
                if (row["id"].ToString().ToLower() == (id.ToLower()))
                    return true;
                return false;
            };

            //from xxx partion(aa,bb) where xxx
            List<SortedList> rztLi = Qe_qry(dataDir, partns, whereFun, rndFun: rndFun);


            SortedList results = rztLi[0];
            dbgCls.print_ret(__METHOD__, "results.Count=>" + results.Count);
            return results;
        }



        internal static string _calcPatnsV3(string dir, string partfile区块文件)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.print_call(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), dir, partfile区块文件));

            //if (string.IsNullOrEmpty(Extname))
            //    Extname = "txt";
            if (string.IsNullOrEmpty(partfile区块文件))
            {

                string rzt = GetFilePathsCommaSeparated(dir);
                dbgCls.print_ret(__METHOD__, rzt);
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

            dbgCls.print_ret(__METHOD__, result);

            return result;
        }


        //单个分区ony need where ,,,bcs order only need in mergeed...and map_select maybe orderd,and top n ,,then last is need to selectMap op
        public static List<SortedList> _qryBySnglePart(string dbf, Func<SortedList, bool> whereFun, Func<string, List<SortedList>> rndFun)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.print_call(__METHOD__, dbgCls.func_get_args(dbf));

            List<SortedList> li = rndFun(dbf);

            li = db.qryV7(li, whereFun);

            dbgCls.print_ret(__METHOD__, 0);
            return li;
        }



        public static bool ChkAllFltrTrue(List<Filtr> li)
        {
            foreach (Filtr cdt in li)
            {
                if (cdt.left == false)
                    return false;
            }
            return true;
        }
    }


    public class Filtr
    {
        public bool left;
        public bool v2;

        public Filtr(bool left)
        {
            this.left = left;
            this.v2 = true;
        }
    }
}