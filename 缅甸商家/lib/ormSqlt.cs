using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using System.IO;
using Microsoft.Data.Sqlite;
using System.Collections;

namespace 缅甸商家.lib
{
    internal class ormSqlt
    {


        /**
         * @param $dbFileName
         * @param $tabl
         * @param mapx
         * @return array
         */
        public static void crtTable(string tabl, Hashtable mapx, string dbFileName)
        {
            // setDbgFunEnter(__METHOD__, func_get_args());

            SqliteConnection cn = new SqliteConnection("data source=" + dbFileName);
            //     SQLitePCL.raw.SetProvider(new SQLitePCL.ISQLite3Provider());
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
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
                if (sqltType.ToString().ToLower()!= "integer")
                     sqltType = "text";
                sql = $"alter table {tabl} add column {de.Key}  {sqltType}";

                cmd = new SqliteCommand();
                cmd.Connection = cn;
                cmd.CommandText = sql;
                cmd_ExecuteNonQuery(cmd);
                // setDbgVal(__METHOD__, "", $crtColm);

                //   setDbgVal(__METHOD__, "sql_ret", $db->exec($crtColm));


                //$idxname =$k."Idx2024";
                //$sql_idx = "  CREATE INDEX $idxname ON  $tabl (  $k ); ";
                //        setDbgVal(__METHOD__, "", $sql_idx);

                //        setDbgVal(__METHOD__, "sql_ret", $db->exec($sql_idx));
                //    }
                //    setDbgRtVal(__METHOD__, "");
            }


        }

        private static void cmd_ExecuteNonQuery(SqliteCommand cmd)
        {
            try
            {
                cmd.ExecuteNonQuery(); //cmd.ExecuteNonQuery();cmd.
            }
            catch(Exception ex)
            {

            }
          
        }

        public static void save(string tblx, Hashtable mapx, string dbFileName)
        {
            //    setDbgFunEnter(__METHOD__, func_get_args());

            //--------------------- crt table

            crtTable(tblx, mapx, dbFileName);


           
           var sql = $"replace into {tblx}" + sqlCls.arr_toSqlPrms4insert(mapx);

            Console.WriteLine(sql);
            SqliteConnection cn = new SqliteConnection("data source=" + dbFileName);
            cn.Open();
           
            SqliteCommand cmd = new SqliteCommand();
            cmd.Connection = cn;
            cmd.CommandText = sql;
            cmd_ExecuteNonQuery(cmd);
            //        setDbgVal(__METHOD__, "sql", sql);
            //ret = $db->exec($sql);
            //        setDbgRtVal(__METHOD__, $ret);

        }



    //    public static void delDt(array $array, string $tabl, string $dbFileName)
    //    {
    //        setDbgFunEnter(__METHOD__, func_get_args());


    //$sql = "delete from $tabl where id=". $array['id'];
    //        setDbgVal(__METHOD__, "sql", $sql);
    //$db = new SQLite3($dbFileName);
    //$ret = $db->exec($sql);
    //        setDbgRtVal(__METHOD__, $ret);

    //    }




        /**
         * @param SQLite3 $db
         * @param string $querySql
         * @return array
         */
//        public static void qry(string $querySql, $dbFileName): array
//{
//    setDbgFunEnter(__METHOD__, func_get_args());
//    $db = new SQLite3($dbFileName);
//    $SQLite3Result = $db->query($querySql);
//    $arr_rzt = [];
//    while ($row = $SQLite3Result->fetchArray(SQLITE3_ASSOC)) {
//        $arr_rzt[] = ($row);
//    }
//    setDbgRtVal(__METHOD__, array_slice($arr_rzt, 0, 3));
//    return $arr_rzt;
//}


    }
}
