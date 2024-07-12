global using static mdsj.lib.util;
using NAudio.Wave;
using Newtonsoft.Json;
using prj202405.lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using static prj202405.lib.corex;
namespace mdsj.lib
{
    internal class util
    {
        public const string botname = "LianXin_BianMinBot";
        public static int GetBatteryPercentage()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Battery");
                foreach (ManagementObject obj in searcher.Get())
                {
                    int estimatedChargeRemaining = Convert.ToInt32(obj["EstimatedChargeRemaining"]);
                    return estimatedChargeRemaining;
                }

                throw new Exception("Battery not found");
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving battery information: " + ex.Message);
            }
        }

        void loopForever()
        {
            while (true)
            {
               print(DateTime.Now);
                Thread.Sleep(5000);
            }
        }
        public static string mp3FilePath_slowSkedu = "C:\\Users\\Administrator\\OneDrive\\mklv song lst\\Nana - Lonely HQ.mp3";

        public static string mp3FilePathEmgcy = "C:\\Users\\Administrator\\OneDrive\\90后非主流的歌曲 v2 w11\\Darin-Be What You Wanna Be HQ.mp3"; // 替换为你的 MP3 文件路径

        public static void tipDayu(string msg2056, Telegram.Bot.Types.Update update)
        {
            try
            {
                if (msg2056.Contains("xxx007") || msg2056.Contains("大鱼") || msg2056.Contains("鱼总"))
                    playMp3(mp3FilePathEmgcy);

            }
            catch (Exception ex)
            {
               print(ex);
                logCls.error_logV2(ex, "err.log");
            }
            try
            {
                if (update.Message.ReplyToMessage.From.FirstName.Contains("大鱼"))
                    playMp3(mp3FilePathEmgcy);
            }
            catch (Exception ex) { }



        }
        public static void ConvertXmlToHtml(string xmlFilePath, string htmlFilePath)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<html>");
            sb.AppendLine("<head><title>XML Documentation</title></head>");
            sb.AppendLine("<body>");
            sb.AppendLine("<h1>api文档</h1>");

            using (var reader = XmlReader.Create(xmlFilePath))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "member")
                    {
                        string name = reader.GetAttribute("name");
                        sb.AppendLine($"<h2>{name}</h2>");

                        while (reader.Read() && !(reader.NodeType == XmlNodeType.EndElement && reader.Name == "member"))
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                string elementName = reader.Name;
                                if (elementName == "example")
                                {
                                    reader.Read(); // Move to the text node or CDATA
                                    if (reader.NodeType == XmlNodeType.CDATA || reader.NodeType == XmlNodeType.Text)
                                    {
                                        string exampleText = reader.Value;
                                        sb.AppendLine($"<p><strong>范例:</strong> {exampleText}</p>");
                                    }
                                }
                                else
                                {//summar
                                    string prmname = reader.GetAttribute("name");
                                    SortedList stlst = new SortedList();
                                    stlst.Add("summary", "功能");

                                    stlst.Add("returns", "返回值");
                                    stlst.Add("param", "----参数");
                                    reader.Read(); // Move to the text node

                                  //  if (reader.NodeType == XmlNodeType.Text)
                                    {
                                        string text = reader.Value;

                                        sb.AppendLine($"<p><strong>{stlst[elementName]+"  " + prmname}:</strong> {text}</p>");
                                    }
                                }

                            }
                        }
                    }
                }
            }

            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            System.IO.File.WriteAllText(htmlFilePath, sb.ToString());
        }
        public static string ConvertXmlToJson(string xmlFilePath)
        {
            XDocument doc = XDocument.Load(xmlFilePath);
            return JsonConvert.SerializeXNode(doc, Newtonsoft.Json.Formatting.Indented, true);
        }
        public static string userDictFile = $"{prjdir}/cfg/user_dict.txt";
        public static void playMp3(string mp3FilePath, int sec)
        {

            // 使用 Task.Run 启动一个新的任务
            callAsync(() => {
                try
                {
                    var __METHOD__ = "playMp3";
                    dbgCls.print_call_FunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), mp3FilePath, sec));

                    using (var audioFile = new AudioFileReader(mp3FilePath))
                    using (var outputDevice = new WaveOutEvent())
                    {
                        outputDevice.Init(audioFile);
                        outputDevice.Play();

                       print("Playing... Press any key to stop.");
                        // Console.ReadKey(); // 按任意键停止播放
                        // 使当前线程休眠5秒钟
                        Thread.Sleep(sec * 1000);
                        //     await Task.Delay(sec*1000);
                        //async maosi nt wk ..only slp wk...maybe same thrd..
                        //yaos ma slp mthis thrd just finish fast..
                        ExecuteAfterDelay(sec * 1000, () =>
                        {
                            outputDevice.Stop();
                        });
                        outputDevice.Stop();

                    }

                    dbgCls.print_ret(__METHOD__, 0);

                }
                catch (Exception ex)
                {
                   print(ex);
                }

            });
          

        }


        public static void playMp3V2(string mp3FilePath)
        {
            try
            {
                var __METHOD__ = MethodBase.GetCurrentMethod().Name;
                dbgCls.print_call_FunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), mp3FilePath));

                using (var audioFile = new AudioFileReader(mp3FilePath))
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();

                   print("Playing... Press any key to stop.");
                    // Console.ReadKey(); // 按任意键停止播放
                    // 使当前线程休眠30秒钟  使得启可以播放audio不会退出
                    Thread.Sleep(15*1000);
                    //async maosi nt wk ..only slp wk...maybe same thrd..
                    //yaos ma slp mthis thrd just finish fast..
                    ExecuteAfterDelay(5000, () =>
                    {
                        outputDevice.Stop();
                    });

                }

                dbgCls.print_ret(__METHOD__, 0);

            }
            catch (Exception ex)
            {
               print(ex);
            }


        }

        /*
         open com ..beri show tips com disable
         <Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>

  <BuiltInComInteropSupport>true</BuiltInComInteropSupport>
  <EnableComHosting>true</EnableComHosting>
         */
        public static void playMp3(string mp3FilePath)
        {
            try
            {
                var __METHOD__ = MethodBase.GetCurrentMethod().Name;
                dbgCls.print_call_FunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), mp3FilePath));

                using (var audioFile = new AudioFileReader(mp3FilePath))
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();

                   print("Playing... Press any key to stop.");
                    // Console.ReadKey(); // 按任意键停止播放
                    // 使当前线程休眠5秒钟
                    Thread.Sleep(60000);
                    //async maosi nt wk ..only slp wk...maybe same thrd..
                    //yaos ma slp mthis thrd just finish fast..
                    ExecuteAfterDelay(5000, () =>
                    {
                        outputDevice.Stop();
                    });

                }

                dbgCls.print_ret(__METHOD__, 0);

            }
            catch (Exception ex)
            {
               print(ex);
            }


        }
    }
}
