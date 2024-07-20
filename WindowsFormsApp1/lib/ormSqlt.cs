using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
using System.Xml.Linq;
using System.IO;
using Microsoft.Data.Sqlite;
using System.Collections;
 
using SQLitePCL;
using Newtonsoft.Json;
 
using System.Reflection;
 
using static mdsj.lib.exCls;
using static prjx.lib.arrCls;//  prj202405.lib
using static prjx.lib.dbgCls;
using static mdsj.lib.logCls;
using static prjx.lib.corex;
using static prjx.lib.db;
using static prjx.lib.filex;
using static prjx.lib.ormJSonFL;
using static prjx.lib.strCls;
namespace prjx.lib
{
    internal class ormSqlt
    {


        /**
         * @param $dbFileName
         * @param $tabl
         * @param mapx
         * @return array
         */
        public static void crtTable(string tabl, SortedList mapx, string dbFileName)
        {
            // setDbgFunEnter(__METHOD__, func_get_args());

            SqliteConnection cn = new SqliteConnection("data source=" + dbFileName);
             //   SQLitePCL.raw.SetProvider(new SQLitePCL.ISQLite3Provider());

            cn.Open();
            //  cn.Close();


            var sql = $" CREATE TABLE {tabl} ( id text  PRIMARY KEY) ";
            //    setDbgVal(__METHOD__, "", $sql);
            SqliteCommand cmd = new SqliteCommand();
            cmd.Connection = cn;
            cmd.CommandText = sql;
            //cmd.CommandText = "CREATE TABLE IF NOT EXISTS t1(id varchar(4),score int
            cmd_ExecuteNonQuery(cmd);

            //   $typeMapPHP2sqlt = ["integer" => "int", "string" => "text"];
            Hashtable typeMapPHP2sqlt = new Hashtable();
            typeMapPHP2sqlt.Add("integer", "int"); typeMapPHP2sqlt.Add("string", "text");


            // foreach (mapx as $k => $v) {
            //遍历方法三：遍历哈希表中的键值
            foreach (DictionaryEntry de in mapx)
            {
                if ((de.Key).ToString().ToLower() == "id")
                    continue;
                var sqltType = typeMapPHP2sqlt[(de.Key.GetType().Name.ToString().ToLower())];
                if (sqltType.ToString().ToLower() != "integer")
                    sqltType = "text";
                sql = $"alter table {tabl} add column {de.Key}  {sqltType}";

                cmd = new SqliteCommand();
                cmd.Connection = cn;
                cmd.CommandText = sql;
                cmd_ExecuteNonQuery(cmd);
                 
            }


        }

        private static object cmd_ExecuteNonQuery(SqliteCommand cmd)
        {
            try
            {
             return   cmd.ExecuteNonQuery(); //cmd.ExecuteNonQuery();cmd.
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static void _saveDep(string tblx, SortedList mapx, string dbFileName)
        {
            //    setDbgFunEnter(__METHOD__, func_get_args());
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
           dbgCls. setDbgFunEnter(MethodBase.GetCurrentMethod().Name, dbgCls. func_get_args (MethodBase.GetCurrentMethod(),tblx, mapx, dbFileName));

            //--------------------- crt table

            crtTable(tblx, mapx, dbFileName);



            var sql = $"replace into {tblx}" + sqlCls.arr_toSqlPrms4insert(mapx);
            dbgCls.setDbgVal(__METHOD__, "sql", sql);
           //print(sql);
            SqliteConnection cn = new SqliteConnection("data source=" + dbFileName);
            cn.Open();

            SqliteCommand cmd = new SqliteCommand();
            cmd.Connection = cn;
            cmd.CommandText = sql;
            
         
            var ret = cmd_ExecuteNonQuery(cmd);
            dbgCls.setDbgValRtval(__METHOD__, ret);

        }



        public static void delByID(string id, string tabl, string dbFileName)
        {
            //  setdbgfunenter(__method__, func_get_args());


            var sql = "delete from $tabl where id='" + id + "'";
            //  setdbgval(__method__, "sql", $sql);
            SqliteConnection cn = new SqliteConnection("data source=" + dbFileName);
            cn.Open();

            SqliteCommand cmd = new SqliteCommand();
            cmd.Connection = cn;
            cmd.CommandText = sql;
            cmd_ExecuteNonQuery(cmd);
            //   setdbgrtval(__method__, $ret);

        }

        //public static void deldt(array $array, string $tabl, string $dbfilename)
        //{
        //    setdbgfunenter(__method__, func_get_args());


        //$sql = "delete from $tabl where id=". $array['id'];
        //    setdbgval(__method__, "sql", $sql);
        //$db = new sqlite3($dbfilename);
        //$ret = $db->exec($sql);
        //    setdbgrtval(__method__, $ret);

        //}

        public static List<Dictionary<string, object>> qryDep(  string dbFileName)
        {
            return _qry("select * from 表格1", dbFileName);
        }
        public static List<SortedList> qryV2(string dbFileName)
        {
            // 初始化SQLitePCL
            Batteries_V2.Init();
            string querySql = "select * from 表格1";
            // setDbgFunEnter(__METHOD__, func_get_args());
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), querySql, dbFileName));

            SqliteConnection cn = new SqliteConnection("data source=" + dbFileName);
            cn.Open();
            //    SqliteCommand cmd = new SqliteCommand();
            //cmd.Connection = cn;
            //    cmd.CommandText = sql;  Install-Package Microsoft.Data.Sqlite
            //    cmd.ExecuteNonQuery


            var results = new List<SortedList>();
            using (var cmd = new SqliteCommand(querySql, cn))
            {
                using (var reader = cmd.ExecuteReader())
                {


                    while (reader.Read())
                    {
                        var row = new SortedList();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            arrCls.addRplsKeyV(row,reader.GetName(i), reader.GetValue(i));
                          //  row[reader.GetName(i)] = reader.GetValue(i);
                        }
                        results.Add(row);
                    }


                }
            }

            // 获取当前方法的信息
            //MethodBase method = );

            //// 输出当前方法的名称
            //Console.WriteLine("Current Method Name: " + method.Name);
            dbgCls.setDbgValRtval(MethodBase.GetCurrentMethod().Name, array_slice(results, 0, 3));


            return results;
        }

        /**
         * @param SQLite3 $db
         * @param string $querySql
         * @return array
         */
        public static List<Dictionary<string, object>> _qry(string querySql, string dbFileName)
        {
            // setDbgFunEnter(__METHOD__, func_get_args());
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), querySql, dbFileName));

            SqliteConnection cn = new SqliteConnection("data source=" + dbFileName);
            cn.Open();
            //    SqliteCommand cmd = new SqliteCommand();
            //cmd.Connection = cn;
            //    cmd.CommandText = sql;
            //    cmd.ExecuteNonQuery


            var results = new List<Dictionary<string, object>>();
            using (var cmd = new SqliteCommand(querySql, cn))
            {
                using (var reader = cmd.ExecuteReader())
                {


                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[reader.GetName(i)] = reader.GetValue(i);
                        }
                        results.Add(row);
                    }


                }
            }

            // 获取当前方法的信息
            //MethodBase method = );

            //// 输出当前方法的名称
            //Console.WriteLine("Current Method Name: " + method.Name);
          dbgCls.  setDbgValRtval(MethodBase.GetCurrentMethod().Name,  array_slice(results, 0, 3));


            return results;
        }

        public static List<Dictionary<string, string>> _qryV2(string querySql, string dbFileName)
        {
            // setDbgFunEnter(__METHOD__, func_get_args());
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), querySql, dbFileName));

            SqliteConnection cn = new SqliteConnection("data source=" + dbFileName);
            cn.Open();
            //    SqliteCommand cmd = new SqliteCommand();
            //cmd.Connection = cn;
            //    cmd.CommandText = sql;
            //    cmd.ExecuteNonQuery


            var results = new List<Dictionary<string, string>>();
            using (var cmd = new SqliteCommand(querySql, cn))
            {
                using (var reader = cmd.ExecuteReader())
                {


                    while (reader.Read())
                    {
                        var row = new Dictionary<string, string>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            object v = reader.GetValue(i);
                            if (v == null)
                                row[reader.GetName(i)] = null;
                            else
                               row[reader.GetName(i)] = v.ToString();
                        }
                        results.Add(row);
                    }


                }
            }

            // 获取当前方法的信息
            //MethodBase method = );

            //// 输出当前方法的名称
            //Console.WriteLine("Current Method Name: " + method.Name);
            dbgCls.setDbgValRtval(MethodBase.GetCurrentMethod().Name, array_slice(results, 0, 3));


            return results;
        }

        internal static void save(SortedList mapx, string dbFileName)
        {
            var tblx = "表格1";
        //    _save("tabx", chtsSesss, strFL);

            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.setDbgFunEnter(MethodBase.GetCurrentMethod().Name, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), tblx, mapx, dbFileName));

            //--------------------- crt table

            crtTable(tblx, mapx, dbFileName);



            var sql = $"replace into {tblx}" + sqlCls.arr_toSqlPrms4insert(mapx);
            dbgCls.setDbgVal(__METHOD__, "sql", sql);
            //print(sql);
            SqliteConnection cn = new SqliteConnection("data source=" + dbFileName);
            cn.Open();

            SqliteCommand cmd = new SqliteCommand();
            cmd.Connection = cn;
            cmd.CommandText = sql;

            var ret = cmd_ExecuteNonQuery(cmd);
            dbgCls.setDbgValRtval(__METHOD__, ret);
        }


        internal static void saveHipfm(SortedList mapx, string dbFileName, SqliteConnection cn)
        {
            var tblx = "表格1";
            //    _save("tabx", chtsSesss, strFL);

            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.setDbgFunEnter(MethodBase.GetCurrentMethod().Name, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), tblx, mapx, dbFileName));

   



            var sql = $"replace into {tblx}" + sqlCls.arr_toSqlPrms4insert(mapx);
            dbgCls.setDbgVal(__METHOD__, "sql", sql);
            //print(sql);
            //SqliteConnection cn = new SqliteConnection("data source=" + dbFileName);
            //cn.Open();

            SqliteCommand cmd = new SqliteCommand();
            cmd.Connection = cn;
            cmd.CommandText = sql;

            var ret = cmd_ExecuteNonQuery(cmd);
            dbgCls.setDbgValRtval(__METHOD__, ret);
        }

        internal static void saveMltHiPfm(List<SortedList> rows, string dbFileName)
        {
            var tblx = "表格1";
            //    _save("tabx", chtsSesss, strFL);

            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.setDbgFunEnter(MethodBase.GetCurrentMethod().Name, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), dbFileName));
            int n = 0;

            SqliteConnection cn = new SqliteConnection("data source=" + dbFileName);
            cn.Open();

            //start trans

            SqliteCommand cmd = new SqliteCommand();
            cmd.Connection = cn;
            cmd.CommandText = "BEGIN TRANSACTION";
            var ret = cmd_ExecuteNonQuery(cmd);
            foreach (SortedList objSave in rows)
            {
                //--------------------- crt table
                if(n==0)
                crtTable(tblx, objSave, dbFileName);

                n++;

                try
                {
                    saveHipfm(objSave, dbFileName, cn);
                }
                catch (Exception e)
                {
                   print(e);
                }


            }


            SqliteCommand cmd_cmt = new SqliteCommand();
            cmd_cmt.Connection = cn;
            cmd_cmt.CommandText = "commit;";
              ret = cmd_ExecuteNonQuery(cmd_cmt);

            dbgCls.setDbgValRtval(__METHOD__, ret);
        }



        internal static void saveMlt(List<SortedList> rows, string dbFileName)
        {
           // var tblx = "tabx";
            //    _save("tabx", chtsSesss, strFL);

            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.setDbgFunEnter(MethodBase.GetCurrentMethod().Name, dbgCls.func_get_args(MethodBase.GetCurrentMethod(),  dbFileName));

            foreach (SortedList objSave in rows)
            {

                try
                {
                    save(objSave, dbFileName);
                }catch (Exception e)
                {
                   print(e);
                }
              

            }


 
        
            dbgCls.setDbgValRtval(__METHOD__, 0);
        }




        //private static void setDbgRtVal(object mETHOD__, object results)
        //{
        //    //string jsonString = JsonConvert.SerializeObject(results, Formatting.Indented);
        //    //Console.WriteLine(jsonString);

        //    //    if ($GLOBALS['dbg_show'] == false)
        //    //return;
        //    // ENDFUN
        //    var msglog = str_repeat(" ", dbgpad) + "" + mETHOD__ + ":: ret=>" + json_encode(results);
        //   print(msglog + "\n");
        //    //    array_push($GLOBALS['dbg'], $msglog);
        //    dbgpad = dbgpad - 4;
        //}

        //private static string json_encode(object results)
        //{
        //    string jsonString = JsonConvert.SerializeObject(results, Formatting.Indented);
        //    //print(jsonString);
        //    return jsonString;
        //}

        //private static string str_repeat(string v, int count)
        //{
        //    return new string(' ', count);
        //}
    }
}
