﻿global using static prj202405.lib.ormJSonFL;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.Wordprocessing;
using mdsj.lib;
using Microsoft.Data.Sqlite;
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
using static prj202405.lib.arrCls;//  prj202405.lib
using static prj202405.lib.dbgCls;
using static prj202405.lib.ormJSonFL;
namespace prj202405.lib
{
    internal class ormJSonFL
    {
        public static SortedList findOne(string dbfile)
        {
            List<SortedList> sortedLists = ormJSonFL.qry(dbfile);


            SortedList cfg = new SortedList();
            if (sortedLists.Count > 0)
                cfg = sortedLists[0];
            return cfg;
        }
        public static void WriteFileIfNotExist(string filePath, string txt)
        {
            // 获取文件目录
            string dir = System.IO.Path.GetDirectoryName(filePath);

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
 
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.print_call_FunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), dbFileName));

            if (!File.Exists(dbFileName))
                File.WriteAllText(dbFileName, "[]");

            // 将JSON字符串转换为List<Dictionary<string, object>>
            string txt = File.ReadAllText(dbFileName);
            if (txt.Trim().Length == 0)
                txt = "[]";
            var list = JsonConvert.DeserializeObject<List<SortedList>>(txt);
            ArrayList list2 = new ArrayList(list);
          
            dbgCls.print_ret(MethodBase.GetCurrentMethod().Name, array_slice(list2, 0, 3));

 
            return list2;
        }
        public static List<SortedList> qry5829(string dbfS)
        {
            throw new Exception("ex3333");
            return null;
        }
        public static List<SortedList> qry(string dbfS)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.print_call_FunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), dbfS));
            string[] dbArr = dbfS.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            List<SortedList> arr = new List<SortedList>();
            foreach (string dbf in dbArr)
            {
                // 检查文件所在目录是否存在，不存在则创建目录
                string directory = System.IO.Path.GetDirectoryName(dbf);
                if(directory.Length>0)
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
 
 
                if (!File.Exists(dbf))
                {
                    Console.WriteLine("not exist file dbf=>" + dbf);
                    continue;
                }
                List<SortedList> sortedLists = qrySglFL(dbf);
                arr = array_merge(arr, sortedLists);
            }

            print_ret(MethodBase.GetCurrentMethod().Name, array_slice(arr, 0, 1));


            return arr;
        }

        public static List<SortedList> read2list(string dbFileName)
        {


            // setDbgFunEnter(__METHOD__, func_get_args());
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.print_call_FunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), dbFileName));

            if (!File.Exists(dbFileName))
            {
                print_ret(__METHOD__, 0);
                return [];
            }
            //    File.WriteAllText(dbFileName, "[]");

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
            dbgCls.print_ret(MethodBase.GetCurrentMethod().Name, array_slice(list, 0, 1));

            return list;
        }

        public static List<SortedList> qrySglFL(string dbFileName)
        {

     
            // setDbgFunEnter(__METHOD__, func_get_args());
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.print_call_FunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), dbFileName));

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
            dbgCls.print_ret(MethodBase.GetCurrentMethod().Name, array_slice(list, 0, 1));
 
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
        public static void del(string id, string Strfile)
        {
            SortedList d = new SortedList();
            d.Add("id", id);
            delete_row(d, Strfile);

        }
        public static void del(SortedList objSave, string Strfile)
        {
            delete_row(objSave, Strfile);

        }
        public static void delete_row(SortedList objSave, string Strfile)
        {
            List<SortedList> liDel = new List<SortedList>();
            liDel.Add(objSave);
            List<SortedList> li = qrySglFL(Strfile);

            List<SortedList> diff = arr_Difference(li, liDel);

            SortedList listIot = db.lst2IOT(diff);


            ArrayList saveList_hpmod = db.lstFrmIot(listIot);
            wriToDbf(saveList_hpmod, Strfile);
        }


        public static List<SortedList> arr_Difference(
        List<SortedList> list1,
        List<SortedList> list2)
        {
            var difference = new List<SortedList>();
            var set2Ids = new HashSet<string>();

            // 将list2的id存入HashSet
            foreach (var item in list2)
            {
                if (item.ContainsKey("id") )
                {
                    set2Ids.Add(item["id"].ToString());
                }
            }

            // 查找list1中不在list2中的元素
            foreach (var item in list1)
            {
                if (item.ContainsKey("id") && !set2Ids.Contains(item["id"].ToString()))
                {
                    difference.Add(item);
                }
            }

            return difference;
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

        public static void wrt_row(SortedList objSave, string Strfile)
        {
            save(objSave, Strfile);

        }
            public static void save(SortedList SortedList1, string dbfile)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.print_call_FunArgs(__METHOD__, dbgCls.func_get_args(SortedList1, dbfile));

            try
            {
          
                mkdir_forFile(dbfile);
                // 创建目录
                // 使用 Path.GetDirectoryName 方法获取目录路径
                string directoryPath = System.IO.Path.GetDirectoryName(dbfile);

                // 检查目录是否存在
                if (directoryPath.Length > 0)
                    if (!Directory.Exists(directoryPath))
                    {
                        if (directoryPath.Length > 5)  //maybe relt path is empty
                        {
                            // 创建目录及所有上级目录
                            Directory.CreateDirectory(directoryPath);
                            Console.WriteLine($"Created directory: {directoryPath}");
                        }

                    }
                //  Directory.CreateDirectory(logdir);
                // 将JSON字符串转换为List<Dictionary<string, object>>
                ArrayList list = qryDep(dbfile);
                SortedList iotTable = db.lst2IOT(list);

                if (ldfld(SortedList1, "id", "") == "")
                    stfld4447(SortedList1, "id", dtime.uuidYYMMDDhhmmssfff());
                string key = SortedList1["id"].ToString();
                stfld4447(iotTable, key, SortedList1);


                ArrayList saveList_hpmod = db.lstFrmIot(iotTable);
                wriToDbf(saveList_hpmod, dbfile);
               
            }
            catch(Exception e)
            {
             
                print_ex(__METHOD__, e);
            }

            print_ret(__METHOD__, 0);
        }


        //replace insert one row
        //public static void save(object objSave, string Strfile)
        //{

        //    var __METHOD__ = MethodBase.GetCurrentMethod().Name;
        //    dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), Strfile));
        //    // 将JSON字符串转换为List<Dictionary<string, object>>
        //    ArrayList list = qryDep(Strfile);
        //    SortedList listIot = db.lst2IOT(list);

        //    listIot.Add(((SortedList)objSave)["id"], objSave);

        //    ArrayList saveList_hpmod = db.lstFrmIot(listIot);
        //    wriToDbf(saveList_hpmod, Strfile);
        //    dbgCls.setDbgValRtval(MethodBase.GetCurrentMethod().Name, 0);

        //}

        internal static void saveMltV2(List<SortedList> rows, string Strfile)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.print_call_FunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), Strfile));
            ArrayList list = qryDep(Strfile);
            SortedList listIot = db.lst2IOT(list);

            foreach (SortedList objSave in rows)
            {

                arrCls.stfld_replaceKeyV(listIot, ldfld_TryGetValueAsStrDefNull(objSave, "id"), objSave);

            }


            ArrayList saveList_hpmod = db.lstFrmIot(listIot);
            wriToDbf(saveList_hpmod, Strfile);
            dbgCls.print_ret(MethodBase.GetCurrentMethod().Name, 0);
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
                    Console.WriteLine(ex.Message);
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
