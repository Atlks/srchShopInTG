 
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
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
//  prj202405.lib.db
namespace prj202405.lib
{
    internal class db
    {


        public static List<SortedList> qryV7(List<SortedList> rows,
  Func<SortedList, bool> whereFun,
  Func<SortedList, int> ordFun,
  Func<SortedList, SortedList> selktFun)
        {
            List<SortedList> rows_rzt4srch =new List<SortedList>();
            foreach (SortedList row in rows)
            {
                if (whereFun(row))
                {
                    rows_rzt4srch.Add(row);
                }
              
            }

            List<SortedList> list_ordered = rows_rzt4srch;
            if (ordFun!=null)
            {
                list_ordered = rows_rzt4srch.Cast<SortedList>()
                                .OrderBy(ordFun)
                                .ToList();
            }
          


            List<SortedList> list_Seleced = new List<SortedList>();
            for (int i = 0; i < list_ordered.Count; i++)
            {
                SortedList row = list_ordered[i];
                if(selktFun!=null)
                    list_Seleced.Add(selktFun(row));
                else
                    list_Seleced.Add(row);
            }

            return list_Seleced;

        }


        public static List<t> qryFrmSqlt<t>(string dbfFrom,
  Func<SortedList, bool> whereFun,
  Func<SortedList, int> ordFun,
  Func<SortedList, t> selktFun)
        {
            ArrayList rows_rzt4srch = new ArrayList();
            List<SortedList> rows = ormSqlt.qryV2(dbfFrom);
            foreach (SortedList row in rows)
            {
                if (whereFun(row))
                {
                    rows_rzt4srch.Add(row);
                }
                //  遍历一个大概40ms   case trycat 模式，给为if else 模式，立马变为1ms
                // Console.WriteLine(DateTime.Now.ToString("yyyyMMdd_HHmmss_fff"));  
            }


            List<SortedList> list = rows_rzt4srch.Cast<SortedList>()
                                  .OrderBy(ordFun)
                                  .ToList();


            List<t> rsRztInlnKbdBtn = new List<t>();
            for (int i = 0; i < rows_rzt4srch.Count; i++)
            {
                SortedList row = list[i];
                rsRztInlnKbdBtn.Add(selktFun(row));
            }

            return rsRztInlnKbdBtn;

        }


        public static List<t> qryV6<t>(string dbfFrom,
       Func<SortedList, bool> whereFun,
       Func<SortedList, int> ordFun,
       Func<SortedList, t> selktFun)
        {
            ArrayList rows_rzt4srch = new ArrayList();
            List<SortedList> rows = ormJSonFL.qry(dbfFrom);
            foreach (SortedList row in rows)
            {
                if (whereFun(row))
                {
                    rows_rzt4srch.Add(row);
                }
                //  遍历一个大概40ms   case trycat 模式，给为if else 模式，立马变为1ms
                // Console.WriteLine(DateTime.Now.ToString("yyyyMMdd_HHmmss_fff"));  
            }


            List<SortedList> list = rows_rzt4srch.Cast<SortedList>()
                                  .OrderBy(ordFun)
                                  .ToList();


            List<t> rsRztInlnKbdBtn = new List<t>  ();
            for (int i = 0; i < rows_rzt4srch.Count; i++)
            {
                SortedList row = list[i];
                rsRztInlnKbdBtn.Add(selktFun(row));
            }

            return rsRztInlnKbdBtn;

        }

        public static ArrayList iot2hpLst(SortedList listIot)
        {
            // 创建一个 ArrayList 来存储 SortedList 中的值
            ArrayList arrayList = new ArrayList();

            // 遍历 SortedList 的值并添加到 ArrayList 中
            foreach (var value in listIot.Values)
            {
                arrayList.Add(value);
            }
            return arrayList;
        }
        public static ArrayList lstFrmIot(SortedList listIot)
        {
            // 创建一个 ArrayList 来存储 SortedList 中的值
            ArrayList arrayList = new ArrayList();

            // 遍历 SortedList 的值并添加到 ArrayList 中
            foreach (var value in listIot.Values)
            {
                arrayList.Add(value);
            }
            return arrayList;
        }
        /// <summary>
        /// Converts a SortedList to a List of SortedLists.
        /// </summary>
        /// <param name="listIot">The SortedList to be converted.</param>
        /// <returns>A List of SortedLists containing the values from the input SortedList.</returns>

        public static List<SortedList> iot2list(SortedList listIot)
        {
            // 创建一个 ArrayList 来存储 SortedList 中的值
            List<SortedList> arrayList = new List<SortedList>();

            // 遍历 SortedList 的值并添加到 ArrayList 中
            foreach (var value in listIot.Values)
            {
                arrayList.Add((SortedList)value);
            }
            return arrayList;
        }


        public static List<InlineKeyboardButton[]> lstFrmIot4inlnKbdBtn(SortedList listIot)
        {
            // 创建一个 ArrayList 来存储 SortedList 中的值
            List<InlineKeyboardButton[]>  arrayList =new List<InlineKeyboardButton[]>();

            // 遍历 SortedList 的值并添加到 ArrayList 中
            foreach (var value in listIot.Values)
            {
                arrayList.Add((InlineKeyboardButton[])value);
            }
            return arrayList;
        }

        public static SortedList lst2IOT(ArrayList arrayList)
        {
            SortedList hash = new SortedList();


            foreach (var item in arrayList)
            {
                SortedList itemx = (SortedList)item;
                hash.Add(itemx["id"], item);
            }

            return hash;
        }


        public static SortedList lst2IOT(List<SortedList> arrayList)
        {
            SortedList hash = new SortedList();


            foreach (SortedList item in arrayList)
            {
                
                hash.Add(item["id"], item);
            }

            return hash;
        }
        public static SortedList lst2IOT4inlKbdBtnArr(List<InlineKeyboardButton[]> arrayList, string idColmName)
        {
            SortedList obj = new SortedList();


            foreach (InlineKeyboardButton[] item in arrayList)
            {
               
                //   SortedList itemx = (SortedList)item;
                try {
                    InlineKeyboardButton btn = item[0];
                    map_add(obj, btn.CallbackData, item);
                   
                }
                catch(Exception e) {
                    Console.WriteLine(e.Message);

                }
                
            }

            return obj;
        }

        private static void map_add(SortedList obj, string callbackData, object item)
        {
            try { obj.Add(callbackData, item); }
            catch(Exception e)
            {

            }
           
        }

        public static SortedList lst2IOT(ArrayList arrayList,string idColmName)
        {
            SortedList obj = new SortedList();


            foreach (var item in arrayList)
            {
                SortedList itemx = (SortedList)item;
                obj.Add(itemx[idColmName], item);
            }

            return obj;
        }
        public static object getRowVal(List<SortedList > lst, string fld, string v2)
        {
            if (lst.Count > 0)
            {
                SortedList d = lst[0];
                if (d.ContainsKey(fld))
                {
                    object v = d[fld];
                    return v;
                }
                else
                    return v2;


            }
            return v2;
        }

        public static object getRowVal(List<Dictionary<string, string>> lst, string fld, string v2)
        {
            if (lst.Count > 0)
            {
                Dictionary<string, string> d = lst[0];
                if (d.ContainsKey(fld))
                {
                    object v = d[fld];
                    return v;
                }
                else
                    return v2;


            }
            return v2;
        }

        public static object getRowVal(List<Dictionary<string, object>> lst, string fld, string v2)
        {
            if (lst.Count > 0)
            {
                Dictionary<string, object> d = lst[0];
                if (d.ContainsKey(fld))
                {
                    object v = d[fld];
                    return v;
                }
                else
                    return v2;


            }
            return v2;
        }


        //parti spt
        public static List<SortedList> qry888(string dataDir, string partnsExprs,
            Func<SortedList, bool> whereFun, Func<SortedList, int> ordFun = null,
                Func<SortedList, SortedList> selktFun = null, string prtnFileExt = "ini")
        {
            List<SortedList> rztLi = new List<SortedList>();
            var patns_dbfs = db.calcPatnsV2(dataDir, partnsExprs, prtnFileExt);
            string[] arr = patns_dbfs.Split(',');
            foreach (string dbf in arr)
            {
                List<SortedList> li = _qryBySnglePart(dbf, whereFun);
                rztLi = arrCls.array_merge(rztLi, li);
            }

            return rztLi;
        }

        //单个分区ony need where ,,,bcs order only need in mergeed...and map_select maybe orderd,and top n ,,then last is need to selectMap op
        public static List<SortedList> _qryBySnglePart(string dbf, Func<SortedList, bool> whereFun)
        {
            List<SortedList> li = rdFrmStoreEngr(dbf);

            li = db.qryV7(li, whereFun, null, null);
            return li;
        }

        public static string find(string id ,string dataDir,string partns = "")
        {

            string soluDir = @"../../../";
            soluDir = filex.GetAbsolutePath(soluDir);
          //  var dataDir = $"{soluDir}\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据";
            Func<SortedList, bool> whereFun = (SortedList row) =>
            {
                if (string.IsNullOrEmpty(id))
                    return false;
                if (row["id"].ToString().ToLower().Contains(id.ToLower()))
                    return true;
                return false;
            };

            //from xxx partion(aa,bb) where xxx
            List<SortedList> rztLi = qry888(dataDir, partns, whereFun,null,null);

            // return rztLi[0];
            //ormSqlt.qryV2("D:\\0prj\\mdsj\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据\\缅甸.db");
            return json_encode(rztLi[0]);
        }


        public static string save(SortedList sortedListNew, Func<SortedList, string> setStrEngrFun, string dataDir, string prtnFileExt, SortedList dbg)
        {
            SortedList mereed = new SortedList();
            if (sortedListNew.ContainsKey("id") && sortedListNew["id"].ToString().Trim().Length > 0)//updt mode
            {
                SortedList old = json_decode<SortedList>(find(sortedListNew["id"].ToString(),dataDir, partns:""));
                mereed = CopyToOldSortedList(sortedListNew, old);
            }
            else
            {
                mereed = sortedListNew;
                // 获取当前时间并格式化为文件名
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                sortedListNew.Add("id", timestamp);
            }

            // string str = save2storeFLByNodejs( mereed, dbg);
            string str = setStrEngrFun(mereed);
            return str;
        }

        public static string save2storeFLByNodejs(SortedList mereed, string saveDataDir, string prtnKey, string prtnFileExt, SortedList dbg)
        {
            SortedList prm = new SortedList();
            prm.Add("dbg", dbg);
            prm.Add("saveobj", mereed);


          
            prm.Add("dbf", ($"{saveDataDir}\\{mereed[prtnKey]}."+ prtnFileExt));

            string timestamp2 = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            Directory.CreateDirectory("prmDir");
            File.WriteAllText($"prmDir/prm{timestamp2}.txt", json_encode(prm));
            string prm_fileAbs = GetAbsolutePath($"prmDir/prm{timestamp2}.txt");

            string prjDir = @"../../";
            string str = ExecuteNodeScript($"{prjDir}\\sqltnode\\save.js", prm_fileAbs);

            string marker = "----------marker----------";
            str = ExtractTextAfterMarker(str, marker);
            str = str.Trim();
            return str;
        }


        private static List<SortedList> rdFrmStoreEngr(string dbf)
        {
            SortedList prm = new SortedList();

            //   prm.Add("partns", ($"{mrchtDir}\\{partns}"));
            prm.Add("dbf", ($"{dbf}"));

            string timestamp2 = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            Directory.CreateDirectory("prmDir");
            File.WriteAllText($"prmDir/prm{timestamp2}.txt", json_encode(prm));

            string prm_fileAbs = GetAbsolutePath($"prmDir/prm{timestamp2}.txt");

            string prjDir = @"../../";
            string str = ExecuteNodeScript($"{prjDir}\\sqltnode\\qry.js", prm_fileAbs);
            string marker = "----------qryrzt----------";
            str = ExtractTextAfterMarker(str, marker);
            str = str.Trim();
            string txt = File.ReadAllText($"{prjDir}\\sqltnode\\tmp\\" + str);
            List<SortedList> li = json_decode(txt);
            return li;
        }
        internal static string calcPatnsV2(string dir, string partfile区块文件,string Extname="txt")
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), dir, partfile区块文件));

            if (string.IsNullOrEmpty(Extname))
                Extname = "txt";
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
                string path = dir + "/" + dbf + "."+ Extname;
                if (!File.Exists(path))
                {
                    Console.WriteLine("not exist file dbf=>" + path);
                    continue;
                }
                arrayList.Add(path);
            }

            // 使用 ArrayList 的 ToArray 方法将其转换为对象数组
            object[] objectArray = arrayList.ToArray();

            // 使用 String.Join 方法将数组转换为逗号分割的字符串
            string result = string.Join(",", objectArray);

            dbgCls.setDbgValRtval(__METHOD__, result);

            return result;
        }

        //only for db sdqlt
        internal static string calcPatns(string dir, string partfile区块文件)
        {  
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), dir, partfile区块文件));

            if (string.IsNullOrEmpty(   partfile区块文件))
            {
              
                string rzt = GetFilePathsCommaSeparated(dir);
                dbgCls.setDbgValRtval(__METHOD__, rzt);
                return rzt;
            }
             ArrayList arrayList = new ArrayList();
            string[] dbArr = partfile区块文件.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach(var dbf in dbArr)
            {
                string path = dir + "/" + dbf+".db";
                if (!File.Exists(path))
                {
                    Console.WriteLine("not exist file dbf=>" + path);
                    continue;
                }
                arrayList.Add(path);
            }

            // 使用 ArrayList 的 ToArray 方法将其转换为对象数组
            object[] objectArray = arrayList.ToArray();

            // 使用 String.Join 方法将数组转换为逗号分割的字符串
            string result = string.Join(",", objectArray);

            dbgCls.setDbgValRtval(__METHOD__, result);
            
            return result;
        }

        static string GetFilePathsCommaSeparated(string directoryPath)
        {
            // 获取目录下的所有文件路径
            string[] filePaths = Directory.GetFiles(directoryPath);

            // 将文件路径数组转换为逗号分割的字符串
            string result = string.Join(",", filePaths);

            return result;
        }
    }
}
