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

        public string list()
        {
            string str = ExecuteNodeScript("D:\\0prj\\mdsj\\WindowsFormsApp1\\sqltnode\\qry.js", "");
            string marker = "----------qryrzt----------";
            str = ExtractTextAfterMarker(str, marker);
            str= str.Trim();
            string txt=File.ReadAllText(" D:\\0prj\\mdsj\\WindowsFormsApp1\\sqltnode\\tmp\\"+str);
            List<SortedList> li= json_decode(txt);
                //ormSqlt.qryV2("D:\\0prj\\mdsj\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据\\缅甸.db");
           return  json_encode(li);
        }

        public void save(string urlqryStr)
        {
            SortedList sortedList = urlqry2hashtb(urlqryStr);

            if (!sortedList.ContainsKey("id"))
            {
                // 获取当前时间并格式化为文件名
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                sortedList.Add("id", timestamp);
            }

            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            ormSqlt.save(sortedList, "D:\\0prj\\mdsj\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据\\缅甸.db");

            //  Dictionary<string, StringValues> whereExprsObj = QueryHelpers.ParseQuery(urlqryStr);
            MessageBox.Show(" function called from HTML frm scrptmng!");
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
