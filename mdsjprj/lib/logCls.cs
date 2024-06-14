using Newtonsoft.Json;
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
namespace mdsj.lib
{
    internal class logCls
    {
        public static void logErr2024(object e, string funName, string logdir,object othInf)
        {

            try
            {
                // 创建目录
                Directory.CreateDirectory(logdir);
                // 获取当前时间并格式化为文件名
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                string fileName = $"{logdir}/{timestamp}.txt";
                File.AppendAllText(fileName, funName + "()\n e=>" + e.ToString());

                File.AppendAllText(fileName,"\n inf=》" + JsonConvert.SerializeObject(othInf, Formatting.Indented));
                File.AppendAllText(fileName,"\n eFmt=》" + JsonConvert.SerializeObject(e, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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
                Console.WriteLine($"Failed to log error: {ex.Message}");
            }
        }

        public static void error_logV2(object messageObj, string filePath)
        {
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
                Console.WriteLine($"Failed to log error: {ex.Message}");
            }
        }

        public static void logErr2025(object e, string funName, string logdir)
        {

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
                Console.WriteLine(ex.ToString());
            }


        }

        internal static void log(object m, string logdir)
        {
            try
            {
                Directory.CreateDirectory(logdir);
                // 获取当前时间并格式化为文件名
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                string fileName = $"{logdir}/{timestamp}.txt";
                Console.WriteLine(" logdir=>"+logdir);
                Console.WriteLine(" fileName=>" + fileName);
                if (IsString(m))
                {
                    System.IO.File.WriteAllText(  fileName, m.ToString());
                }else
                System.IO.File.WriteAllText( fileName, json_encode(m));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
