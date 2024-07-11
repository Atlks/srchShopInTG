global using static libx.qryEngrParser;
global using static libx.Filtr;
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
        public static bool hasCondt(Dictionary<string, string> whereExprsObj, string fld)
        {
            return whereExprsObj.ContainsKey(fld);

            //if (park4srch == null)
            //{
            //    return false;
            //}
            //return true;
        }
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
        public static List<bool> getLstFltrsFrmQrystr(string qrystr, SortedList row)
        {
            List<bool> li = new List<bool>();


            Dictionary<string, string> filters = ldDic4qryCdtn(qrystr);
            foreach_DictionaryKeys(filters, (string key) =>
            {
                li.Add((isFldValEq111(row, key, filters)));
            });
            return li;
        }
        public static List<Filtr> getLstFltrCdtnsFrmQrystr(string qrystr, SortedList row)
        {
            List<Filtr> li = new List<Filtr>();
          

            Dictionary<string, string> filters = ldDic4qryCdtn(qrystr);
            foreach_DictionaryKeys(filters, (string key) =>
            {
                li.Add(new Filtr(isFldValEq111(row, key, filters)));
            });
            return li;
        }
        public static List<SortedList> getListFltr(string fromDdataDir, string shanrES,
         Func<SortedList, bool> whereFun
       )
        {

            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            print_call_FunArgs(__METHOD__, dbgCls.func_get_args(fromDdataDir, shanrES, "whereFun()"));


            //   SortedList shareCfgList = getShareCfgLst(fromDdataDir);

            //  List<t> rsRztInlnKbdBtn = new List<t>();

            //if (shareCfgList is null)
            //{
            //    throw new ArgumentNullException(nameof(shareCfgList));
            //}

            List<SortedList> rztLi = new List<SortedList>();
            //zhe 这里不要检测物理文件，全逻辑。。物理检测在存储引擎即可。

            // string[] shareArr = shanrES.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //zhe 这里不要检测物理文件，全逻辑。。物理检测在存储引擎即可。
            string shareStr = _calcPatnsV4(fromDdataDir, shanrES);
            string[] shareArr = shareStr.Split(',');
            foreach (var shar in shareArr)
            {
                var CurSharFullpath = fromDdataDir + "/" + shar;
                string rndFun;
                SortedList curShareCfg = shareDetail(fromDdataDir, shar);
                rndFun = (string)curShareCfg["rndFun"];

                //    Func<string, List<SortedList>> rndEng_Fun = (Func<string, List<SortedList>>)GetFunc(); ;
                var dbg = (shar: shar, storeEngr: rndFun, CurSharFullpath: CurSharFullpath);
                List<SortedList> li = arr_fltr4ReadShare(CurSharFullpath, whereFun, rndFun.ToString(), dbg);
                rztLi = array_merge(rztLi, li);
            }
            print_ret(__METHOD__, "rztLi.size:" + rztLi.Count);
            return rztLi;
        }

        //public static List<t> xxxx<t>()
        //{
        //    SortedList row = new SortedList();
        //    List<t> rsRztInlnKbdBtn = new List<t>();
        //    rsRztInlnKbdBtn.Add( row);
        //    return rsRztInlnKbdBtn;
        //}
        public static List<SortedList> arr_fltr4readDir(string fromDdataDir, string shanrES,
            Func<SortedList, bool> whereFun
          )
        {

            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            print_call_FunArgs(__METHOD__, dbgCls.func_get_args(fromDdataDir, shanrES, "whereFun()"));


            //   SortedList shareCfgList = getShareCfgLst(fromDdataDir);

            //  List<t> rsRztInlnKbdBtn = new List<t>();

            //if (shareCfgList is null)
            //{
            //    throw new ArgumentNullException(nameof(shareCfgList));
            //}

            List<SortedList> rztLi = new List<SortedList>();
            //zhe 这里不要检测物理文件，全逻辑。。物理检测在存储引擎即可。

           // string[] shareArr = shanrES.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //zhe 这里不要检测物理文件，全逻辑。。物理检测在存储引擎即可。
            string shareStr = _calcPatnsV4(fromDdataDir, shanrES);
            string[] shareArr = shareStr.Split(',');
            foreach (var shar in shareArr)
            {
                var CurSharFullpath = fromDdataDir + "/" + shar;
                string rndFun;
                SortedList curShareCfg = shareDetail(fromDdataDir, shar);
                rndFun = (string)curShareCfg["rndFun"];

                //    Func<string, List<SortedList>> rndEng_Fun = (Func<string, List<SortedList>>)GetFunc(); ;
                var dbg = (shar: shar, storeEngr: rndFun, CurSharFullpath: CurSharFullpath);
                List<SortedList> li = arr_fltr4ReadShare(CurSharFullpath, whereFun, rndFun.ToString(), dbg);
                rztLi = array_merge(rztLi, li);
            }
            print_ret(__METHOD__, "rztLi.size:" + rztLi.Count);
            return rztLi;
        }
        public static Func<SortedList, bool> castQrystr2FltrCdtFun(string qrystr)
        {
            Func<SortedList, bool> whereFun = (SortedList row) =>
            {
                if (row["园区"].ToString().Contains("东风"))
                    print("dbg");

                List<bool> li = getLstFltrsFrmQrystr(qrystr, row);

                //  li.Add(new Filtr(isNotEmptyLianxi(row)));
                //   li.Add(new Filtr(isLianxifshValid(row)));

                //li.Add(new Filtr(isFldValEq111(row, "园区", filters)));
                //li.Add(new Filtr(isFldValEq111(row, "国家", filters)));

                //string dbfile = "parkcfgDir/uid_" + qrystrMap["uid"] + ".json";
                //SortedList cfg = findOne(dbfile);
                //Dictionary<string, StringValues> cdtMap = CopySortedListToDictionary(cfg);
                //   li.Add(new Filtr(str_eq(row["园区"], ldfldDfempty(cdtMap, "park"))));

                if (!ChkAllFltrTrue(li))
                    return false;
                return true;
            };
            return whereFun;
        }

        private static string getRndFun(string fromDdataDir, string shanrES, string shar)
        {
            string rndFun;
            if (string.IsNullOrEmpty(shanrES))
            {
                if(shar.EndsWith(".db"))
                    return nameof(rnd_next4Sqlt);
                if (shar.EndsWith(".json"))
                    return nameof(rnd4jsonFl);
                if (shar.EndsWith(".ini"))
                    return nameof(rnd4jsonFl);
                if (shar.EndsWith(".xlsx"))
                    return nameof(rnd4jsonFl);
                return nameof(rnd4jsonFl);
            }
            else
            {
                SortedList curShareCfg = shareDetail(fromDdataDir, shar);
                rndFun = (string)curShareCfg["rndFun"];
            }

            return rndFun;
        }

        //arr_fltr4readDir
        public static List<SortedList> arr_fltrV2(string fromDdataDir, string shanrES,
           Func<SortedList, bool> whereFun,
           Func<string, List<SortedList>> rndFun)
        {

            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.print_call_FunArgs(__METHOD__, dbgCls.func_get_args(fromDdataDir, shanrES));


            //  List<t> rsRztInlnKbdBtn = new List<t>();

            if (rndFun is null)
            {
                throw new ArgumentNullException(nameof(rndFun));
            }

            List<SortedList> rztLi = new List<SortedList>();
            //zhe 这里不要检测物理文件，全逻辑。。物理检测在存储引擎即可。
            var share_dbfs = _calcPatnsV3(fromDdataDir, shanrES);
            string[] arr = share_dbfs.Split(',');
            foreach (string dbfCurShar in arr)
            {
                List<SortedList> li = _qryBySnglePart(dbfCurShar, whereFun, rndFun);
                rztLi = array_merge(rztLi, li);
            }

            return rztLi;
        }

        public static List<SortedList> arr_fltrDep2024(string fromDdataDir, string shanrES,
                 Func<SortedList, bool> whereFun,
                 Func<string, List<SortedList>> rndFun)
        {

            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.print_call_FunArgs(__METHOD__, dbgCls.func_get_args(fromDdataDir, shanrES));


            //  List<t> rsRztInlnKbdBtn = new List<t>();

            if (rndFun is null)
            {
                throw new ArgumentNullException(nameof(rndFun));
            }

            List<SortedList> rztLi = new List<SortedList>();
            //zhe 这里不要检测物理文件，全逻辑。。物理检测在存储引擎即可。
            var share_dbfs = _calcPatnsV3(fromDdataDir, shanrES);
            string[] arr = share_dbfs.Split(',');
            foreach (string dbfCurShar in arr)
            {
                List<SortedList> li = _qryBySnglePart(dbfCurShar, whereFun, rndFun);
                rztLi = array_merge(rztLi, li);
            }

            return rztLi;
        }

        public static List<t> Qe_qryV3<t>(string fromDdataDir, string shanrES,
          Func<SortedList, bool> whereFun,
          Func<SortedList, int> ordFun,
          Func<SortedList, t> selktFun,
          Func<string, List<SortedList>> rndFun)
        {

            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.print_call_FunArgs(__METHOD__, dbgCls.func_get_args(fromDdataDir, shanrES));
            List<t> listFnl = new List<t>();
            try
            {


                if (rndFun is null)
                {
                    throw new ArgumentNullException(nameof(rndFun));
                }

                List<SortedList> rztLi = arr_fltrDep2024(fromDdataDir, shanrES, whereFun, rndFun);



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

                listFnl = arr_transfm_map(rztLi, selktFun);
            }

            catch (Exception e)
            {
               print($"--ex catch---- mtth:{__METHOD__}((( {json_encode_noFmt(func_get_args(fromDdataDir, shanrES))}");
               print(e);
                logErr2025(e, __METHOD__, "errdir");
                //  return rsRztInlnKbdBtn;
            }

            print_ret(__METHOD__, 0);
            return listFnl;
        }

        private static List<t> arr_transfm_map<t>(List<SortedList> rztLi, Func<SortedList, t> selktFun)
        {
            List<t> listFnl = new List<t>();
            for (int i = 0; i < rztLi.Count; i++)
            {
                SortedList row = rztLi[i];

                listFnl.Add(selktFun(row));


            }
            return listFnl;
        }

        public static List<t> Qe_qryV2<t>(string fromDdataDir, string shanrES,
            Func<SortedList, bool> whereFun,
            Func<SortedList, int> ordFun,
            Func<SortedList, t> selktFun,
            Func<string, List<SortedList>> rndFun)
        {

            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.print_call_FunArgs(__METHOD__, dbgCls.func_get_args(fromDdataDir, shanrES));


            List<t> rsRztInlnKbdBtn = new List<t>();
            try
            {
                if (rndFun is null)
                {
                    throw new ArgumentNullException(nameof(rndFun));
                }

                List<SortedList> rztLi = new List<SortedList>();
                //zhe 这里不要检测物理文件，全逻辑。。物理检测在存储引擎即可。
                var share_dbfs = _calcPatnsV3(fromDdataDir, shanrES);
                string[] arr = share_dbfs.Split(',');
                foreach (string dbfCurShar in arr)
                {
                    List<SortedList> li = _qryBySnglePart(dbfCurShar, whereFun, rndFun);
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
               print($"--ex catch---- mtth:{__METHOD__}((( {json_encode_noFmt(func_get_args(fromDdataDir, shanrES))}");
               print(e);
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
                try
                {
                    List<SortedList> li = _qryBySnglePart(dbf, whereFun, rndFun);
                    rztLi = arrCls.array_merge(rztLi, li);
                }catch(Exception e)
                {
                    print_ex("Qe_qry",e);
                }
              
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
            dbgCls.print_call_FunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), id, dataDir, partns));

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

        internal static string _calcPatnsV4(string FromdataDir, string shareFiles)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.print_call_FunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), FromdataDir, shareFiles));

            string result = shareFiles;

            if( string.IsNullOrEmpty(shareFiles))
            {
                SortedList sharecfg = shareList(FromdataDir);
                result = GetKeysCommaSeparated(sharecfg);
            }
        


            dbgCls.print_ret(__METHOD__, result);

            return result;
        }


        internal static string _calcPatnsV3(string dir, string shareFiles)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.print_call_FunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), dir, shareFiles));

            //if (string.IsNullOrEmpty(Extname))
            //    Extname = "txt";
            if (string.IsNullOrEmpty(shareFiles))
            {

                string rzt = GetFilePathsCommaSeparated(dir);
                dbgCls.print_ret(__METHOD__, rzt);
                return rzt;
            }
            ArrayList arrayList = new ArrayList();
            string[] dbArr = shareFiles.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var dbf in dbArr)
            {
                string path = dir + "/" + dbf + "";
                //if (!File.Exists(path))
                //{
                //   print("not exist file dbf=>" + path);
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
        public static List<SortedList> arr_fltr4ReadShare(string shareName, Func<SortedList, bool> whereFun, string rnd, object dbg)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            print_call_FunArgs(__METHOD__, func_get_args(shareName, "whreFun()", rnd, dbg));


            List<SortedList> li = (List<SortedList>)callx(rnd, shareName);
            if (li.Count > 0 && whereFun != null)
                li = db.arr_fltr330(li, whereFun);

            dbgCls.print_ret(__METHOD__, li.Count);
            return li;
        }

        public static List<SortedList> _qryByShare(string shareName, Func<SortedList, bool> whereFun, Func<string, List<SortedList>> rndFun, object dbg)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            print_call_FunArgs(__METHOD__, dbgCls.func_get_args(shareName, dbg));

            List<SortedList> li = rndFun(shareName);

            li = db.arr_fltr330(li, whereFun);

            dbgCls.print_ret(__METHOD__, li.Count);
            return li;
        }
        //单个分区ony need where ,,,bcs order only need in mergeed...and map_select maybe orderd,and top n ,,then last is need to selectMap op
        public static List<SortedList> _qryBySnglePart(string dbfName, Func<SortedList, bool> whereFun, Func<string, List<SortedList>> rndFun)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            print_call_FunArgs(__METHOD__, dbgCls.func_get_args(dbfName, "rndFun"));

            List<SortedList> li = rndFun(dbfName);

            li = db.arr_fltr330(li, whereFun);

            dbgCls.print_ret(__METHOD__, li.Count);
            return li;
        }

      
        public static bool ChkAllFltrTrue(List<bool> li)
        {
            foreach (bool cdt in li)
            {
                if (cdt == false)
                    return false;
            }
            return true;
        }
        //dep
        public static bool ChkAllFltrTrueDep(List<Filtr> li)
        {
            foreach (Filtr cdt in li)
            {
                if (cdt.left == false)
                    return false;
            }
            return true;
        }
    }


    //dep
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