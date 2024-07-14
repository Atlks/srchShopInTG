﻿global using static mdsj.lib.tmrTaskMng;
using NCrontab;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace mdsj.lib
{
    internal class tmrTaskMng
    {
        public static void CallTmrTasks()
        {
            string tmrtaskDir = $"{prjdir}/cfg/tmrtask";
            List<Hashtable> list = getListFrmDir(tmrtaskDir);
            foreach_listHstb(list, (Hashtable hs) =>
            {
                DateTime now = DateTime.Now;

                string[] times = splt(hs["time"]);
                var zhuliLog = $"tmrlg/{hs["basename"]}{Convert.ToString(now.Month) + now.Day + Convert.ToString(now.Hour)}.json";
                if (isIn(now.Hour, times) && now.Minute == 1 && (!System.IO.File.Exists(zhuliLog)))
                {
                    System.IO.File.WriteAllText(zhuliLog, "pushlog");
                    var txtkeepBtnMenu = "";// "美好的心情从现在开始\n";

                    callx(hs["fun"].ToString());
                }
            });
        }
        public static void task222()
        {
            print("\n\n\n\n$$$$$$$$$$$$$$$$$$$$$$$$$$4task2222");
            print("$$$$$$$$$$$$$$$$$$$$$$$$$$4task2222");
            print("$$$$$$$$$$$$$$$$$$$$$$$$$$4task2222");
        }
        public static void RunTmrTasksCron()
        {

            // 创建一个定时器，每分钟检查一次是否需要执行任务
            var timer = new System.Threading.Timer(
                callback: _ =>
                {
                    callxTryJmp(tmrTask1start);
                },
                state: null,
                dueTime: TimeSpan.Zero,
                period: TimeSpan.FromMinutes(1)); // 每分钟检查一次

        }

        public static void tmrTask1start()
        {
            string tmrtaskDir = $"{prjdir}/cfg/tmrtask";
            List<Hashtable> list = getListFrmDir(tmrtaskDir);
            foreach_listHstb(list, (Hashtable hs) =>
            {// 获取当前时间
                DateTime now = DateTime.Now;
                string cronExpression = hs["cron"].ToString();
                // 解析 Cron 表达式
                CrontabSchedule schedule = CrontabSchedule.Parse(cronExpression);
                // 检查是否需要执行任务
                DateTime Run_dateTime = schedule.GetNextOccurrence(now);
                if (Run_dateTime.Hour == now.Hour && Run_dateTime.Minute == now.Minute + 1)
                {
                  //  DateTime now = DateTime.Now;
                    var zhuliLog = $"tmrlg/{hs["basename"]}{Convert.ToString(now.Month) + now.Day + Convert.ToString(now.Hour)}.json";
                    if (!System.IO.File.Exists(zhuliLog))
                    {
                        System.IO.File.WriteAllText(zhuliLog, "pushlog");
                        // 调用需要执行的函数
                        //     fun1();
                        callx(hs["fun"].ToString());
                    }
                }
            });
        }


        public static void tmrTask1startNow()
        {
            string tmrtaskDir = $"{prjdir}/cfg/tmrtask";
            List<Hashtable> list = getListFrmDir(tmrtaskDir);
            foreach_listHstb(list, (Hashtable hs) =>
            {// 获取当前时间
                DateTime now = DateTime.Now;
                var zhuliLog = $"tmrlg/{hs["basename"]}{Convert.ToString(now.Month) + now.Day }_19.json";
                print(zhuliLog);
                if (!System.IO.File.Exists(zhuliLog))
                {
                    System.IO.File.WriteAllText(zhuliLog, "pushlog");
                    // 调用需要执行的函数
                    //     fun1();
                    callx(hs["fun"].ToString());
                }

            });
        }


        //end fun


    }
}
