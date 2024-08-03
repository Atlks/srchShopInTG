global using static mdsj.lib.kvSqlt;

using Microsoft.Data.Sqlite;
using Newtonsoft.Json.Linq;
using prjx.lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace mdsj.lib
{
    internal class kvSqlt
    {

        public static List<SortedList> GetListFrmSqltKvV2(string dbFileName)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            PrintTimestamp(" start   "+ __METHOD__);
            string querySql = "select * from t";
            // setDbgFunEnter(__METHOD__, func_get_args());
        
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), dbFileName));
            var results = new List<SortedList>();
            try
            {
                SqliteConnection cn = new SqliteConnection("data source=" + dbFileName);
                cn.Open();
                using (var cmd = new SqliteCommand(querySql, cn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {


                        while (reader.Read())
                        {
                            SortedList  row = new SortedList (); ;
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string key = reader.GetName(i);
                                object objSave = reader.GetValue(i);
                                if (key == "id")
                                    continue;
                                if (key == "v")
                                {
                                    if (objSave.ToString() == "61e81427-1bca-4493-bc9f-2003f2cbcb56")
                                        Print("dbg1135");
                                    //   
                                    JObject objSave1 = DecodeJson(objSave);
                                    row = CastJObjectToSortedList(objSave1);
                                    break;
                                }


                                // SetFieldAddRplsKeyV(row, key, objSave);
                                //  row[reader.GetName(i)] = reader.GetValue(i);
                            }
                            results.Add(row);
                        }


                    }
                }


            }
            catch (Exception ex)
            {
                Print(ex);
            }
            PrintRet(MethodBase.GetCurrentMethod().Name, ArrSlice(results, 0, 1));
            PrintTimestamp(" end qryV2() ");
            return results;
        }

        /// <summary>
        /// qry
        /// </summary>
        /// <param name="dbFileName"></param>
        /// <returns></returns>
        public static List<SortedList<string, object>> GetListFromSqltKv(string dbFileName)
        {
            PrintTimestamp(" start GetListFromSqltKv() ");
            string querySql = "select * from t";
            // setDbgFunEnter(__METHOD__, func_get_args());
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), dbFileName));
            var results = new List<SortedList<string, object>>();
            try
            {
                SqliteConnection cn = new SqliteConnection("data source=" + dbFileName);
                cn.Open();
                using (var cmd = new SqliteCommand(querySql, cn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {


                        while (reader.Read())
                        {
                            SortedList<string, object> row = new SortedList<string, object>(); ;
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string key = reader.GetName(i);
                                object objSave = reader.GetValue(i);
                                if (key == "id")
                                    continue;
                                if (key == "v")
                                {
                                    if (objSave.ToString() == "61e81427-1bca-4493-bc9f-2003f2cbcb56")
                                        Print("dbg1135");
                                    //   
                                    JObject objSave1 =  DecodeJson(objSave);
                                    row = ConvertJObjectToSortedList(objSave1);
                                    break;
                                }

                              
                               // SetFieldAddRplsKeyV(row, key, objSave);
                                //  row[reader.GetName(i)] = reader.GetValue(i);
                            }
                            results.Add(row);
                        }


                    }
                }


            }
            catch (Exception ex)
            {
                Print(ex);
            }
            PrintRet(MethodBase.GetCurrentMethod().Name, ArrSlice(results, 0, 1));
            PrintTimestamp(" end qryV2() ");
            return results;
        }

        public static SortedList<string, object> CastObj2Stlst(object objSave)
        {
            //  throw new NotImplementedException();
            return null;
        }


        public static void Save2SqltKvMd(SortedList  SortedList1, string dbFileName)
        {



            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            PrintCallFunArgs(MethodBase.GetCurrentMethod().Name, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), SortedList1, dbFileName));

            //--------------------- crt table
          //  if (IsFileNotExist(dbFileName))
            {
                CreatTableKv(dbFileName);
            }


            var vVal = EncodeJson(SortedList1);
            var id = GetField(SortedList1, "id", GetUuid());
            var sql = $"replace into t (id,v)values( '{id}','{vVal}')";
            print_varDump(__METHOD__, "sql", sql);
            //print(sql);
            SqliteConnection SqliteConnection1 = new SqliteConnection("data source=" + dbFileName);
            SqliteConnection1.Open();

            SqliteCommand SqliteCommand1 = new SqliteCommand();
            SqliteCommand1.Connection = SqliteConnection1;
            SqliteCommand1.CommandText = sql;

            var ret = ormSqlt.cmd_ExecuteNonQuery(SqliteCommand1);
            PrintRet(__METHOD__, ret);
        }

        public static void Save2SqltKvMd(SortedList<string, object> SortedList1, string dbFileName)
        {



            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            PrintCallFunArgs(MethodBase.GetCurrentMethod().Name, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), SortedList1, dbFileName));

            //--------------------- crt table
            if (IsFileNotExist(dbFileName))
            {
                CreatTableKv(dbFileName);
            }


            var vVal = EncodeJson(SortedList1);
            var id = GetField(SortedList1, "id", GetUuid());
            var sql = $"replace into t (id,v)values( '{id}','{vVal}')";
            print_varDump(__METHOD__, "sql", sql);
            //print(sql);
            SqliteConnection SqliteConnection1 = new SqliteConnection("data source=" + dbFileName);
            SqliteConnection1.Open();

            SqliteCommand SqliteCommand1 = new SqliteCommand();
            SqliteCommand1.Connection = SqliteConnection1;
            SqliteCommand1.CommandText = sql;

            var ret = ormSqlt.cmd_ExecuteNonQuery(SqliteCommand1);
            PrintRet(__METHOD__, ret);
        }


        public static void CreatTableKv(string dbFileName)
        {
            var tabl = "t";
            // setDbgFunEnter(__METHOD__, func_get_args());

            SqliteConnection SqliteConnection1 = new SqliteConnection("data source=" + dbFileName);
            //     SQLitePCL.raw.SetProvider(new SQLitePCL.ISQLite3Provider());
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            SqliteConnection1.Open();
            //  cn.Close();


            var sql = $" CREATE TABLE {tabl} ( id text  PRIMARY KEY,v text) ";
            //    setDbgVal(__METHOD__, "", $sql);
            SqliteCommand SqliteCommand1 = new SqliteCommand();
            SqliteCommand1.Connection = SqliteConnection1;
            SqliteCommand1.CommandText = sql;
            //cmd.CommandText = "CREATE TABLE IF NOT EXISTS t1(id varchar(4),score int
            ormSqlt.cmd_ExecuteNonQuery(SqliteCommand1);


            //sql = $"alter table {tabl} add column v  text";

            //SqliteCommand1 = new SqliteCommand();
            //SqliteCommand1.Connection = SqliteConnection1;
            //SqliteCommand1.CommandText = sql;
            //ormSqlt.cmd_ExecuteNonQuery(SqliteCommand1);




        }

    }
}
