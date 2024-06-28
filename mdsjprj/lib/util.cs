using NAudio.Wave;
using prj202405.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static mdsj.lib.util;
using static prj202405.lib.corex;
namespace mdsj.lib
{
    internal class util
    {

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
                Console.WriteLine(DateTime.Now);
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
                Console.WriteLine(ex);
                logCls.error_logV2(ex, "err.log");
            }
            try
            {
                if (update.Message.ReplyToMessage.From.FirstName.Contains("大鱼"))
                    playMp3(mp3FilePathEmgcy);
            }
            catch (Exception ex) { }



        }
        public static async Task playMp3(string mp3FilePath, int sec)
        {
            try
            {
                var __METHOD__ = "playMp3";
                dbgCls.print_call(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), mp3FilePath, sec));

                using (var audioFile = new AudioFileReader(mp3FilePath))
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();

                    Console.WriteLine("Playing... Press any key to stop.");
                    // Console.ReadKey(); // 按任意键停止播放
                    // 使当前线程休眠5秒钟
                    Thread.Sleep(sec * 1000);
                    //async maosi nt wk ..only slp wk...maybe same thrd..
                    //yaos ma slp mthis thrd just finish fast..
                    ExecuteAfterDelay(sec * 1000, () =>
                    {
                        outputDevice.Stop();
                    });

                }

                dbgCls.print_ret(__METHOD__, 0);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


        }
        /*
         open com ..beri show tips com disable
         <Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>

  <BuiltInComInteropSupport>true</BuiltInComInteropSupport>
  <EnableComHosting>true</EnableComHosting>
         */
        public static async Task playMp3(string mp3FilePath)
        {
            try
            {
                var __METHOD__ = MethodBase.GetCurrentMethod().Name;
                dbgCls.print_call(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), mp3FilePath));

                using (var audioFile = new AudioFileReader(mp3FilePath))
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();

                    Console.WriteLine("Playing... Press any key to stop.");
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
                Console.WriteLine(ex);
            }


        }
    }
}
