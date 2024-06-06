using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
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

        public static ArrayList  qry(  string dbFileName)
        {

            //if (!File.Exists(dbFileName))
            //    File.WriteAllText(dbFileName, "[]");
            // setDbgFunEnter(__METHOD__, func_get_args());
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(),  dbFileName));

            if (!File.Exists(dbFileName))
                File.WriteAllText(dbFileName, "[]");

            // 将JSON字符串转换为List<Dictionary<string, object>>
            string txt = File.ReadAllText(dbFileName);
            if (txt.Trim().Length == 0)
                txt = "[]";
            var  list = JsonConvert.DeserializeObject<List<SortedList>>(txt);
            ArrayList list2= new ArrayList(list);
            //   ArrayList list = (ArrayList)JsonConvert.DeserializeObject(File.ReadAllText(dbFileName));

            // 获取当前方法的信息
            //MethodBase method = );

            //// 输出当前方法的名称
            //Console.WriteLine("Current Method Name: " + method.Name);
            dbgCls.setDbgValRtval(MethodBase.GetCurrentMethod().Name, dbgCls.array_slice(list2, 0, 3));


            // 将List转换为ArrayList
          //  ArrayList arrayList = new ArrayList(list);
            return list2;
        }

        //replace insert one row
        public static void save(object objSave, string Strfile)
        {


            // 将JSON字符串转换为List<Dictionary<string, object>>
            ArrayList list = qry(Strfile);
            SortedList listIot =db. lst2IOT(list);

            listIot.Add(((SortedList)objSave)["id"], objSave);

            ArrayList saveList_hpmod = db.lstFrmIot(listIot);
            wriToDbf(saveList_hpmod, Strfile);
        
        }

        internal static void saveMlt(ArrayList rows, string Strfile)
        {
            // 将JSON字符串转换为List<Dictionary<string, object>>
            ArrayList list = qry(Strfile);
            SortedList listIot = db.lst2IOT(list);

            foreach(var objSave in rows )
            {
                try
                {
                    listIot.Add(((SortedList)objSave)["id"], objSave);
                }catch(Exception ex) {
                    Console.WriteLine(ex.Message);
                }
               
            }
          

            ArrayList saveList_hpmod = db.lstFrmIot(listIot);
            wriToDbf(saveList_hpmod, Strfile);
        }

        private static void wriToDbf(object lst, string dbfl)
        {
            File.WriteAllText(dbfl, JsonConvert.SerializeObject(lst, Formatting.Indented));
        }
    }
}
