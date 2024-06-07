using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdsj.lib
{
    internal class logCls
    {


        public static void logErr2025(Exception e, string funName, string logdir)
        {
            // 创建目录
            Directory.CreateDirectory(logdir);
            // 获取当前时间并格式化为文件名
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            string fileName = $"{logdir}/{timestamp}.txt";
            File.WriteAllText(fileName, funName+"()\n"+ e.ToString());

            try
            {
                File.AppendAllText(JsonConvert.SerializeObject(e, Formatting.Indented),fileName);
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
           
           
        }
    }
}
