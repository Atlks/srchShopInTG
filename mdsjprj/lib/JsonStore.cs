global using static mdsj.lib.JsonStore;
using Newtonsoft.Json;
using prjx.lib;
using RG3.PF.Abstractions.Entity;
using RG3.PF.Abstractions.Extensions;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace mdsj.lib
{
    internal class JsonStore
    {
        public static ConcurrentDictionary<string, List<SortedList>> dbCache = new ConcurrentDictionary<string, List<SortedList>>();

        /// <summary>
        /// 2. 并行处理
     //   使用并行处理来同时读取多个文件。这可以通过 Parallel.ForEach 或 Task.WhenAll 来实现。
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static async Task<List<SortedList>> ListReadDirJsonsAsync(string dir)
        {
            //asyn to imprv pfm
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.PrintCallFunArgs(__METHOD__, func_get_args(dir));

            List<SortedList> arr = new List<SortedList>();
            if (!dbCache.TryGetValue(dir, out List<SortedList> list144))
            {
                // 获取目录中的所有文件
                string[] filePaths = Directory.GetFiles(dir);
              
                //并行处理  imprv pfm
                await Task.WhenAll(filePaths.Select(async filePath =>
                {
                    // 将JSON字符串转换为List<Dictionary<string, object>>
                    //  string txt = File.ReadAllText(dbf);
                    string txt = await File.ReadAllTextAsync(filePath);
                    if (txt.Trim().Length == 0)
                        txt = "{}";
                    SortedList obj = JsonConvert.DeserializeObject<SortedList>(txt);
                    arr.Add(obj);
                }));
                dbCache.AddIfNotContainsKey(dir, arr);
               
             //   dbCache[dir] = arr;
            }
           
            

            PrintRet(MethodBase.GetCurrentMethod().Name, ArrSlice(arr, 0, 1));


            return dbCache[dir];
        }
      
        
        public static async Task SaveToJsonSngleFileAsync(SortedList SortedList1, string dir)
        {
            if (LoadFieldFrmStlst(SortedList1, "id", "") == "")
                SetField938(SortedList1, "id", dtime.uuidYYMMDDhhmmssfff());
            var dbfile = dir + "/id_" + SortedList1["id"] + ".json";
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(SortedList1, dbfile));

            //------------wrt to cache
            List<SortedList> li127 = await ListReadDirJsonsAsync(dir);
            SetElmt(li127, SortedList1);
          
                

            //-------------wrt to file
            try
            {

                Mkdir4File(dbfile);
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
                            Print($"Created directory: {directoryPath}");
                        }

                    }
                //  Directory.CreateDirectory(logdir);
                // 将JSON字符串转换为List<Dictionary<string, object>>
            
            
                string key = SortedList1["id"].ToString();
              //  SetField938(iotTable, key, SortedList1);
 
                wriToDbf((SortedList1), dbfile);

            }
            catch (Exception e)
            {

                PrintExcept(__METHOD__, e);
            }



            PrintRet(__METHOD__, 0);
        }

      
    
    }
}
