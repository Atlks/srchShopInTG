using DocumentFormat.OpenXml.Spreadsheet;
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
using static mdsj.lib.net_http;
using static mdsj.lib.web3;
using static mdsj.libBiz.tgBiz;
using static prj202405.lib.tglib;
using static prj202405.timerCls;
using static mdsj.lib.util;

using static mdsj.lib.afrmwk;
namespace mdsj.lib
{
    internal class afrmwk
    {
        public static void aop_lgtry(Action act)
        {
            try
            {
                act();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                logErr2024(e, "", "errlog", "");
            }
           
        }
        public static void evt_boot(Action actBiz)
        {
            //捕获未处理的同步异常：使用 AppDomain.CurrentDomain.UnhandledException 事件。
            //捕获未处理的异步异常：使用 TaskScheduler.UnobservedTaskException 事件。
            // 设置全局异常处理
            mdsj.lib.exCls.set_error_handler();
            try
            {
                PrintLogo();
                //start boot music
                // 启动一个新线程，执行匿名函数
                Thread newThread = new Thread(() =>
                {
                    Console.WriteLine("新线程开始执行");
                    playMp3("C:\\Users\\Administrator\\OneDrive\\song cn\\新疆美丽公主组合 - 欢乐地跳吧.mp3", 2);

                    Console.WriteLine("新线程完成工作");
                });

                //动画金字塔logo
                for (int i = 0; i < 40; i++)
                {
                    Thread.Sleep(50);
                    Console.WriteLine(str_repeatV2("=", i) + "=>");
                }
                // 启动新线程
                newThread.Start();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            call(actBiz, []);
           // actBiz();



        }
        public static void evt_exit()


        {
            try
            {
               // PrintPythonLogo();
                playMp3("C:\\Users\\Administrator\\OneDrive\\song cn\\张震岳 - 再见.mp3", 10);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


        }
        static async Task PrintLogo()
        {

            Console.WriteLine(@"
        ,--./,-.
       / #      \
      |          |
       \        /    
        `._,._,'
           ]
        ,--'
        |
        `.___.
        ");
            Console.WriteLine(System.IO.File.ReadAllText("logo.txt"));
        }


    }
}
