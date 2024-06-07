using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdsj.lib
{
    internal class exCls
    {

        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("捕获未处理的同步异常：");
            Console.WriteLine(((Exception)e.ExceptionObject).Message);
            // 这里可以记录日志或执行其他处理
            logCls.logErr2025((Exception)e.ExceptionObject, "CurrentDomain_UnhandledException", "errlog");
        }

        public static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Console.WriteLine("捕获未处理的异步异常：");
            Console.WriteLine(e.Exception.Message);
            // 这里可以记录日志或执行其他处理
            e.SetObserved(); // 标记异常已观察到，防止程序崩溃

            logCls.logErr2025((Exception)e.Exception, "TaskScheduler_UnobservedTaskException", "errlog");
        }
    }
}
