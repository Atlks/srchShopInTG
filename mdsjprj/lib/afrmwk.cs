using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static mdsj.biz_other;
using static mdsj.clrCls;
using static mdsj.lib.exCls;
using static prjx.lib.arrCls;//  prj202405.lib
using static prjx.lib.dbgCls;
using static mdsj.lib.logCls;
using static prjx.lib.corex;
using static prjx.lib.db;
using static prjx.lib.filex;
using static prjx.lib.ormJSonFL;
using static prjx.lib.strCls;
using static mdsj.lib.bscEncdCls;
using static mdsj.lib.net_http;
using static mdsj.lib.web3;
using static mdsj.libBiz.tgBiz;
using static prjx.lib.tglib;
using static prjx.timerCls;
using static mdsj.lib.util;

using static mdsj.lib.afrmwk;
using System.Collections;
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
            catch (Exception e)
            {
                Print(e);
                logErr2024(e, "", "errlog", "");
            }

        }
        public static void Evtboot(Action actBiz)
        {
            Print("!!!!****⚠️⚠️⚠️⚠️⚠️⚠️⚠️ver8888882❣❣");
            PrintLog("ttt");
            SetConsoleQuickEditMode(false);
            Boot4StbltSetting();
            //-----------------log

            RunSetRollLogFileV2();
            //-----------end log
            //add all cache db 
    
            //------------------ 设置全局异常处理
            mdsj.lib.exCls.set_error_handler();
            //捕获未处理的同步异常：使用 AppDomain.CurrentDomain.UnhandledException 事件。
            //捕获未处理的异步异常：使用 TaskScheduler.UnobservedTaskException 事件。

            //-----------------start music
            callTryAll(() =>
            {

                Thread.Sleep(3000);
                PrintLogo();
                //-------------start boot music
                // 启动一个新线程，执行匿名函数
                Thread newThread = new Thread(() =>
                {
                    Print("新线程开始执行");
                    playMp3($"{prjdir}/libres/start.mp3", 2);

                    Print("新线程完成工作");
                });
                // 启动新线程
                newThread.Start();


                //------------动画金字塔logo
                for (int i = 0; i < 40; i++)
                {
                    Thread.Sleep(50);
                    Print(str_repeatV2("=", i) + "=>");
                }


            });


            //-----------sync prgrm to svr
            TaskRunNewThrd(() =>
            {
                var cfgf = $"{prjdir}/cfg/cfg.ini";
                Hashtable cfgDic = GetHashtabFromIniFl(cfgf);
                //  var  localOsKwd = GetFieldAsStr10(cfgDic, "localOsKwd"); 
                var os = GetOSVersion();//os ver:OS: Win32NT, Version: 10.0.22631
                var localOsKwd = GetFieldAsStr10(cfgDic, "localOsKwd");
                if (os.Contains("Win32NT") && os.Contains("10.0."))
                {
                    Thread.Sleep(10000);

                    string url = GetFieldAsStr10(cfgDic, "syncUpldUrl");

                    for (int i = 1; i < 10; i++)
                    {
                        string fl = GetFieldAsStr10(cfgDic, "syncUpldFile" + i);
                        if (fl.Length > 0)
                            UploadFileAsync(fl, url);
                    }

                }

            });

            Call(actBiz, []);
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
                Print(e);
            }


        }
        static void PrintLogo()
        {

            Print(@"
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
            Print(System.IO.File.ReadAllText("logo.txt"));
        }


    }
}
