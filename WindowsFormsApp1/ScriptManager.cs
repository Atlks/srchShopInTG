using prj202405.lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//using static mdsj.other;
//using static mdsj.clrCls;
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
using System.IO;
using Microsoft.Extensions.Primitives;
using Mono.Web;
using System.Collections.Specialized;
using System.Security.Policy;
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

        public string list(string mrchName = "",  string partns = "")
        {

            string soluDir = @"../../../";
            soluDir = filex.GetAbsolutePath(soluDir);
            var dataDir =$"{soluDir}\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据";
            Func<SortedList, bool> whereFun = (SortedList row) =>
            {
                if (string.IsNullOrEmpty(mrchName))
                    return true;
                if (row["商家"].ToString().ToLower().Contains(mrchName.ToLower()))
                    return true;
                return false;
            };

            //from xxx partion(aa,bb) where xxx
            List<SortedList> rztLi = qry(dataDir, partns, whereFun);


            //ormSqlt.qryV2("D:\\0prj\\mdsj\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据\\缅甸.db");
            return json_encode(rztLi);
        }

        public string find(string id = "", string partns = "")
        {

            string soluDir = @"../../../";
            soluDir = filex.GetAbsolutePath(soluDir);
            var dataDir = $"{soluDir}\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据";
            Func<SortedList, bool> whereFun = (SortedList row) =>
            {
                if (string.IsNullOrEmpty(id))
                    return false;
                if (row["id"].ToString().ToLower().Contains(id.ToLower()))
                    return true;
                return false;
            };

            //from xxx partion(aa,bb) where xxx
            List<SortedList> rztLi = qry(dataDir, partns, whereFun);

           // return rztLi[0];
            //ormSqlt.qryV2("D:\\0prj\\mdsj\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据\\缅甸.db");
           return json_encode(rztLi[0]);
        }

        //parti spt
        private static List<SortedList> qry(string dataDir, string partnsExprs,
            Func<SortedList, bool> whereFun, Func<SortedList, int> ordFun=null,
                Func<SortedList, SortedList> selktFun= null)
        {
            List<SortedList> rztLi = new List<SortedList>();
            var patns_dbfs = db.calcPatnsV2(dataDir, partnsExprs,"db");
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
        public static SortedList CopyToOldSortedList( SortedList newList, SortedList oldList)
        {
            // 创建一个新的 SortedList
           // SortedList newList = new SortedList();

            // 遍历旧的 SortedList 并将每个键值对复制到新的 SortedList
            foreach (DictionaryEntry entry in newList)
            {
                arrCls.addRplsKeyV(oldList, entry.Key.ToString(), entry.Value);
             //   newList.Add(entry.Key, entry.Value);
            }

            return newList;
        }
        public string save(string urlqryStr)
        {
            SortedList sortedListNew = urlqry2hashtb(urlqryStr);

            SortedList mereed=new SortedList();
            if (sortedListNew.ContainsKey("id"))//updt mode
            {
                SortedList old = json_decode< SortedList>( find(sortedListNew["id"].ToString()));
                mereed = CopyToOldSortedList(sortedListNew,old );
            }
            else
            {
                mereed = sortedListNew;
                // 获取当前时间并格式化为文件名
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                sortedListNew.Add("id", timestamp);
            }

            SortedList prm = new SortedList();
            prm.Add("urlqryStr", urlqryStr);
            prm.Add("saveobj", mereed);
            string soluDir = @"../../../";
            soluDir = filex.GetAbsolutePath(soluDir);
            var mrchtDir = $"{soluDir}\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据";
           // var mrchtDir = "D:\\0prj\\mdsj\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据";
            prm.Add("dbf", ($"{mrchtDir}\\{mereed["国家"]}.db"));

            string timestamp2 = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            Directory.CreateDirectory("prmDir");
            File.WriteAllText($"prmDir/prm{timestamp2}.txt", json_encode(prm));
            string prm_fileAbs = GetAbsolutePath($"prmDir/prm{timestamp2}.txt");

            string prjDir = @"../../";
            string str = ExecuteNodeScript($"{prjDir}\\sqltnode\\save.js", prm_fileAbs);

            string marker = "----------marker----------";
            str = ExtractTextAfterMarker(str, marker);
            str = str.Trim();
            MessageBox.Show("添加成功!");
            return str;
            //  SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            //ormSqlt.save(sortedList, "D:\\0prj\\mdsj\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据\\缅甸.db");

            //  Dictionary<string, StringValues> whereExprsObj = QueryHelpers.ParseQuery(urlqryStr);
            //   MessageBox.Show(" function called from HTML frm scrptmng!");
        }

        public static SortedList urlqry2hashtb(string urlqryStr)
        {

            // 解析查询字符串为字典
            NameValueCollection queryString = HttpUtility.ParseQueryString(urlqryStr);
            var QueryHashtb = new System.Collections.Generic.Dictionary<string, string>();

            // 将解析结果存入字典
            foreach (string key in queryString.AllKeys)
            {
                QueryHashtb.Add(key, queryString[key]);
            }



            // 创建一个 SortedList 并初始化大小
            SortedList sortedList = new SortedList(QueryHashtb.Count);

            // 将 Dictionary 中的项添加到 SortedList 中
            foreach (var pair in QueryHashtb)
            {
                sortedList.Add(pair.Key, pair.Value.ToString());
            }

            return sortedList;
        }

        public void m1()
        {
            MessageBox.Show(" function called from HTML frm scrptmng!");
        }
    }
}
