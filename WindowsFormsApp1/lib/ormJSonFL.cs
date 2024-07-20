
using static prjx.lib.corex;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static prjx.lib.arrCls;//  prj202405.lib
using static prjx.lib.dbgCls;
using static prjx.lib.ormJSonFL;
namespace prjx.lib
{
    internal class ormJSonFL
    {

        public static void WriteFileIfNotExist(string filePath, string txt)
        {
            // 获取文件目录
            string dir = Path.GetDirectoryName(filePath);

            // 检查目录是否存在，如果不存在，则创建目录
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.WriteAllText(filePath, txt);


        }
        //qry just use path as qry dsl  ,,
        public static ArrayList qryDep(string dbFileName)
        {

            //if (!File.Exists(dbFileName))
            //    File.WriteAllText(dbFileName, "[]");
            // setDbgFunEnter(__METHOD__, func_get_args());
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), dbFileName));

            if (!File.Exists(dbFileName))
                File.WriteAllText(dbFileName, "[]");

            // 将JSON字符串转换为List<Dictionary<string, object>>
            string txt = File.ReadAllText(dbFileName);
            if (txt.Trim().Length == 0)
                txt = "[]";
            var list = JsonConvert.DeserializeObject<List<SortedList>>(txt);
            ArrayList list2 = new ArrayList(list);
            //   ArrayList list = (ArrayList)JsonConvert.DeserializeObject(File.ReadAllText(dbFileName));

            // 获取当前方法的信息
            //MethodBase method = );

            //// 输出当前方法的名称
            //Console.WriteLine("Current Method Name: " + method.Name);
            dbgCls.setDbgValRtval(MethodBase.GetCurrentMethod().Name, array_slice(list2, 0, 3));


            // 将List转换为ArrayList
            //  ArrayList arrayList = new ArrayList(list);
            return list2;
        }

        public static List<SortedList> qry(string dbfS)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), dbfS));
            string[] dbArr = dbfS.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            List<SortedList> arr = new List<SortedList>();
            foreach (string dbf in dbArr)
            {
                // 检查文件所在目录是否存在，不存在则创建目录
                string directory = System.IO.Path.GetDirectoryName(dbf);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                //// 检查文件是否存在，不存在则写入内容
                //if (!File.Exists(filePath))
                //{
                //    File.WriteAllText(filePath, content);
                //   print("File created and content written.");
                //}
                if (!File.Exists(dbf))
                {
                   print("not exist file dbf=>" + dbf);
                    continue;
                }
                List<SortedList> sortedLists = qrySglFL(dbf);
                arr = array_merge(arr, sortedLists);
            }

            dbgCls.setDbgValRtval(MethodBase.GetCurrentMethod().Name, array_slice(arr, 0, 2));


            return arr;
        }

        public static List<SortedList> qrySglFL(string dbFileName)
        {

            //if (!File.Exists(dbFileName))
            //    File.WriteAllText(dbFileName, "[]");
            // setDbgFunEnter(__METHOD__, func_get_args());
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), dbFileName));

            if (!File.Exists(dbFileName))
                File.WriteAllText(dbFileName, "[]");

            // 将JSON字符串转换为List<Dictionary<string, object>>
            string txt = File.ReadAllText(dbFileName);
            if (txt.Trim().Length == 0)
                txt = "[]";
            var list = JsonConvert.DeserializeObject<List<SortedList>>(txt);

            //   ArrayList list = (ArrayList)JsonConvert.DeserializeObject(File.ReadAllText(dbFileName));

            // 获取当前方法的信息
            //MethodBase method = );

            //// 输出当前方法的名称
            //Console.WriteLine("Current Method Name: " + method.Name);
            dbgCls.setDbgValRtval(MethodBase.GetCurrentMethod().Name, array_slice(list, 0, 1));


            // 将List转换为ArrayList
            //  ArrayList arrayList = new ArrayList(list);
            return list;
        }

        public static void dec(SortedList objSave, string Strfile)
        {

        }
        public static void inc(SortedList objSave, string Strfile)
        {

        }
        public static void value(SortedList objSave, string Strfile)
        {

        }
        public static void find(SortedList objSave, string Strfile)
        {

        }
        public static void del(SortedList objSave, string Strfile)
        {

        }
        public static void update(SortedList objSave, string Strfile)
        {

        }
        public static void replace(SortedList objSave, string Strfile)
        {

        }
        public static void saveOrUpdate(SortedList objSave, string Strfile)
        {

        }
        public static void save(SortedList objSave, string Strfile)
        {

            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), Strfile));
            // 将JSON字符串转换为List<Dictionary<string, object>>
            ArrayList list = qryDep(Strfile);
            SortedList listIot = db.lst2IOT(list);

            string key = objSave["id"].ToString();
            arrCls.addRplsKeyV(listIot, key, objSave);



            ArrayList saveList_hpmod = db.lstFrmIot(listIot);
            wriToDbf(saveList_hpmod, Strfile);
            dbgCls.setDbgValRtval(MethodBase.GetCurrentMethod().Name, 0);

        }


        //replace insert one row
        public static void save(object objSave, string Strfile)
        {

            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), Strfile));
            // 将JSON字符串转换为List<Dictionary<string, object>>
            ArrayList list = qryDep(Strfile);
            SortedList listIot = db.lst2IOT(list);

            listIot.Add(((SortedList)objSave)["id"], objSave);

            ArrayList saveList_hpmod = db.lstFrmIot(listIot);
            wriToDbf(saveList_hpmod, Strfile);
            dbgCls.setDbgValRtval(MethodBase.GetCurrentMethod().Name, 0);

        }

        internal static void saveMltV2(List<SortedList> rows, string Strfile)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), Strfile));
            ArrayList list = qryDep(Strfile);
            SortedList listIot = db.lst2IOT(list);

            foreach (SortedList objSave in rows)
            {

                arrCls.replaceKeyV(listIot, TryGetValueAsStrDefNull(objSave, "id"), objSave);

            }


            ArrayList saveList_hpmod = db.lstFrmIot(listIot);
            wriToDbf(saveList_hpmod, Strfile);
            dbgCls.setDbgValRtval(MethodBase.GetCurrentMethod().Name, 0);
        }
        internal static void saveMlt(List<SortedList> rows, string Strfile)
        {
            // 将JSON字符串转换为List<Dictionary<string, object>>
            ArrayList list = qryDep(Strfile);
            SortedList listIot = db.lst2IOT(list);

            foreach (SortedList objSave in rows)
            {
                try
                {
                    listIot.Add(objSave["id"], objSave);
                }
                catch (Exception ex)
                {
                   print(ex.Message);
                }
                listIot[objSave["id"]] = objSave;
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
