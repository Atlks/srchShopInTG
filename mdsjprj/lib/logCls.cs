﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static mdsj.biz_other;
using static mdsj.clrCls;
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
using RG3.PF.Abstractions.Entity;
namespace mdsj.lib
{
    internal class logCls
    {
        public static void logErr2024(object e, string funName, string logdir,object othInf)
        {
            if (e is jmp2endEx)
                return;
            try
            {
                // 创建目录
                Directory.CreateDirectory(logdir);
                // 获取当前时间并格式化为文件名
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                string fileName = $"{logdir}/{timestamp}.txt";
                File.AppendAllText(fileName, funName + "()\n eStr=>" + e.ToString());

                File.AppendAllText(fileName,"\n inf=》" + JsonConvert.SerializeObject(othInf, Formatting.Indented));
                File.AppendAllText(fileName,"\n eFmt=》" + JsonConvert.SerializeObject(e, Formatting.Indented));
            }
            catch (Exception ex)
            {
               print(ex.ToString());
            }


        }

        /// <summary>
        /// 记录错误信息到指定的日志文件。
        /// </summary>
        /// <param name="message">错误信息。</param>
        /// <param name="filePath">日志文件路径。</param>
        public static void error_log(string message, string filePath)
        {
            try
            {
                // 确保目录存在
                string directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // 记录错误信息到文件
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"{DateTime.Now}: {message}");
                }
            }
            catch (Exception ex)
            {
                // 处理记录日志时可能出现的异常
               print($"Failed to log error: {ex.Message}");
            }
        }

        public static void error_logV2(object messageObj, string filePath)
        {
            if (messageObj is jmp2endEx)
                return;
            try
            {
                string message=json_encode(messageObj);
                // 确保目录存在
                string directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // 记录错误信息到文件
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"{DateTime.Now}: {message}");
                }
            }
            catch (Exception ex)
            {
                // 处理记录日志时可能出现的异常
               print($"Failed to log error: {ex.Message}");
            }
        }

        public static void logErr2025(object e, string funName, string logdir)
        {
            if (e is jmp2endEx)
                return;
            try
            {
                // 创建目录
                Directory.CreateDirectory(logdir);
                // 获取当前时间并格式化为文件名
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                string fileName = $"{logdir}/{timestamp}.txt";
                File.AppendAllText( fileName, funName + "()\n e=>" + e.ToString());


                File.AppendAllText(fileName,"\n eFmt=》" +JsonConvert.SerializeObject(e, Formatting.Indented));
            }
            catch (Exception ex)
            {
                //Newtonsoft.Json.JsonSerializationException:
               print(ex.Message.ToString());
            }


        }

        internal static void log(object m, string logdir)
        {
            try
            {
                Directory.CreateDirectory(logdir);
                // 获取当前时间并格式化为文件名
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                string fileName = $"{logdir}/{timestamp}.json";
               print(" logdir=>"+logdir);
               print(" fileName=>" + fileName);
                if (IsString(m))
                {
                    System.IO.File.WriteAllText(  fileName, m.ToString());
                }else
                System.IO.File.WriteAllText( fileName, json_encode(m));
            }
            catch (Exception ex)
            {
               print(ex.ToString());
            }
        }

        internal static void log(string mETHOD__, object prm, object val, string logdir, string reqThreadId)
        {
            try
            {
                string timestampPx = DateTime.Now.ToString("dd_HHmmss");
                Directory.CreateDirectory(logdir);
                // 获取当前时间并格式化为文件名
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmm");
                string fileName = $"{logdir}/{timestamp}.{reqThreadId}.txt";
               print(" logdir=>" + logdir);
               print(" fileName=>" + fileName);
                string msg = $"{timestampPx} {mETHOD__}({json_encode_noFmt(prm)})::{json_encode_noFmt(val)} \n"; 
                if (IsString(msg))
                {
                    // 将字符串追加到文件
                    File.AppendAllText(fileName, ToString(msg));
                }
                else
                    System.IO.File.AppendAllText(fileName, json_encode(msg));
            }
            catch (Exception ex)
            {
               print(ex.ToString());
            }
            
        }

        private static string? ToString(object val)
        {
            if (val == null)
                return "";
            return val.ToString();
        }
    }
}
