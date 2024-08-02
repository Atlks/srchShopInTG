global using static mdsj.lib.util;
using NAudio.Wave;
using Newtonsoft.Json;
using prjx.lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using static prjx.lib.corex;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using System.Runtime.InteropServices;
namespace mdsj.lib
{
    internal class util
    {
        public static MemoryCache cache2024 = new MemoryCache(new MemoryCacheOptions());
        public const string pageprm251 = "token page pages pagesize limit page limit pagesize from ";
        //  public static bool jmp2exitFlag;
        public static ThreadLocal<bool> jmp2exitFlagInThrd = new ThreadLocal<bool>(() =>
        {
            // 初始化每个线程的值为 false
            return false;
        });
        public static ThreadLocal<bool> Jmp2endCurFunFlag = new ThreadLocal<bool>(() =>
        {
            // 初始化每个线程的值为 false
            return false;
        });

        public static ThreadLocal<SortedList> ifStrutsThrdloc = new ThreadLocal<SortedList>(() =>
        {
            return NewIFAst();
        });

        public static ThreadLocal<object> lastSendMsg = new ThreadLocal<object>(() =>
        {
            
            return null;
        });

        public static SortedList NewIFAst()
        {
            SortedList ifx = new SortedList();

          
            ifx.Add("cdts", new ArrayList());
            ifx.Add("cdtsRzt", false);
            ifx.Add("choose", "Then");
            return ifx;
        }

        public static ThreadLocal<string> jmp2endCurFunInThrd = new ThreadLocal<string>(() =>
        {
            // 初始化每个线程的值为 false
            return "";
        });
        public static string botname = "LianXin_BianMinBot";

        public static void TransferFileByRdpWmi(string sourceFilePath, string destinationFilePath, string targetHost, string username, string password)
        {
            Print(" start TransferFileByRdpWmi()" + sourceFilePath + $"  {destinationFilePath} {targetHost} {username} {password} ");
            ManagementScope scope = null;

            //    在本地计算机上尝试连接到本地 WMI 服务，以确保 WMI 本身工作正常：
            //   如果本地连接成功，但远程连接失败，问题可能与网络配置或远程设置有关。



            scope = new ManagementScope(@"\\.\root\cimv2");
            scope.Connect();

            try
            {
                ConnectionOptions options = new ConnectionOptions
                {
                    Username = username,
                    Password = password
                };

                scope = new ManagementScope($@"\\{targetHost}\root\cimv2", options);
                scope.Connect();
            }
            catch (COMException ex)
            {
                Console.WriteLine($"COMException: {ex.Message}");
                Console.WriteLine($"Error Code: {ex.ErrorCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            // Create the management object for file transfer
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, new ObjectQuery("SELECT * FROM Win32_Process"));
            foreach (ManagementObject obj in searcher.Get())
            {
                // Create process to copy the file
                ManagementClass processClass = new ManagementClass(scope, new ManagementPath("Win32_Process"), new ObjectGetOptions());
                ManagementBaseObject inParams = processClass.GetMethodParameters("Create");
                inParams["CommandLine"] = $"cmd.exe /c copy \"{sourceFilePath}\" \"{destinationFilePath}\"";
                ManagementBaseObject outParams = processClass.InvokeMethod("Create", inParams, null);

                Console.WriteLine($"Process created with ID {outParams["ProcessId"]}");
            }
        }

        /// <summary>
        /// 通过 HTTP POST 请求将文件发送到指定的 URL
        /// </summary>
        /// <param name="filePath">要发送的文件路径</param>
        /// <param name="url">目标 URL</param>
        /// <returns>异步任务</returns>
        public static void UploadFileAsync(string filePath, string url)
        {
            Print("UploadFileAsync() " + filePath + " " + url);
            try
            {
                using (HttpClient client = new HttpClient())
                using (MultipartFormDataContent content = new MultipartFormDataContent())
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    // 创建文件内容
                    var fileContent = new StreamContent(fileStream);
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");

                    // 添加文件内容到请求
                    content.Add(fileContent, "file", Path.GetFileName(filePath));

                    // 发送 POST 请求
                    HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult(); ;

                    // 检查响应状态
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("File uploaded successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to upload file. Status code: {response.StatusCode}");
                    }
                }
            }catch(Exception e)
            {
                PrintExcept("UploadFileAsync", e);
            }
            
          //  return 1;
        }



        public static void PrintTimestamp(string msg)
        {
            // 获取当前时间（本地时间）
            DateTime now = DateTime.Now;

            // 格式化为可读性较强的字符串，精确到毫秒
            string formattedDate = now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            // 打印结果
            Console.WriteLine($"⏱️⏱️ {msg} milliseconds: " + formattedDate);
        }
        public static void PrintTimestamp()
        {
            // 获取当前时间（本地时间）
            DateTime now = DateTime.Now;

            // 格式化为可读性较强的字符串，精确到毫秒
            string formattedDate = now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            // 打印结果
            Console.WriteLine("⏱️⏱️Current time with milliseconds: " + formattedDate);
        }
        public static int CalculateTotalPages(int pageSize, int totalRecords)
        {
            if (pageSize <= 0)
            {
                pageSize = 10;
            }

            if (totalRecords < 0)
            {
                totalRecords = 0;
            }

            return (int)Math.Ceiling((double)totalRecords / pageSize);
        }
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
               Print(ex);
                logCls.error_logV2(ex, "err.log");
            }
            try
            {
                if (update.Message.ReplyToMessage.From.FirstName.Contains("大鱼"))
                    playMp3(mp3FilePathEmgcy);
            }
            catch (Exception ex) { }



        }
     
        public static string userDictFile = $"{prjdir}/cfg/user_dict.txt";
        public static void playMp3(string mp3FilePath, int sec)
        {

            // 使用 Task.Run 启动一个新的任务
            CallAsyncNewThrd(() => {
                try
                {
                    var __METHOD__ = "playMp3";
                    dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), mp3FilePath, sec));

                    using (var audioFile = new AudioFileReader(mp3FilePath))
                    using (var outputDevice = new WaveOutEvent())
                    {
                        outputDevice.Init(audioFile);
                        outputDevice.Play();

                       Print("Playing... Press any key to stop.");
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

                    dbgCls.PrintRet(__METHOD__, 0);

                }
                catch (Exception ex)
                {
                   Print(ex);
                }

            });
          

        }


        public static void playMp3V2(string mp3FilePath)
        {
            try
            {
                var __METHOD__ = MethodBase.GetCurrentMethod().Name;
                dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), mp3FilePath));

                using (var audioFile = new AudioFileReader(mp3FilePath))
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();

                   Print("Playing... Press any key to stop.");
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

                dbgCls.PrintRet(__METHOD__, 0);

            }
            catch (Exception ex)
            {
               Print(ex);
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
                dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), mp3FilePath));

                using (var audioFile = new AudioFileReader(mp3FilePath))
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();

                   Print("Playing... Press any key to stop.");
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

                dbgCls.PrintRet(__METHOD__, 0);

            }
            catch (Exception ex)
            {
               Print(ex);
            }


        }
    }
}
