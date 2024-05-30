using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace prj202405.lib
{
    internal class ormJSonFL
    {

        public static List<Dictionary<string, object>> qry(  string dbFileName)
        {
            // setDbgFunEnter(__METHOD__, func_get_args());
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(),  dbFileName));

            if (!File.Exists(dbFileName))
                File.WriteAllText(dbFileName, "[]");

            // 将JSON字符串转换为List<Dictionary<string, object>>
            var list = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(File.ReadAllText(dbFileName));

            // 获取当前方法的信息
            //MethodBase method = );

            //// 输出当前方法的名称
            //Console.WriteLine("Current Method Name: " + method.Name);
            dbgCls.setDbgValRtval(MethodBase.GetCurrentMethod().Name, dbgCls.array_slice(list, 0, 3));


            return list;
        }

        public static void save(object frm, string Strfile)
        {
            if (!File.Exists(Strfile))
                File.WriteAllText(Strfile, "[]");

            // 将JSON字符串转换为List<Dictionary<string, object>>
            var list = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(File.ReadAllText(Strfile));

            // 将List转换为ArrayList
            ArrayList arrayList = new ArrayList(list);
            //    ArrayList chtsSesss =(ArrayList) JsonConvert.DeserializeObject(System.IO.File.ReadAllText(Strfile))!;

            //if (chtsSesss.Contains(Convert.ToString(chtid)))
            //{
            //    return;
            //}


            arrayList.Add(frm);

            File.WriteAllText(Strfile, JsonConvert.SerializeObject(arrayList, Formatting.Indented));


        }
    }
}
