using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static prjx.lib.corex;
namespace mdsj.lib
{
    public class exCls
    {

        /// <summary>
        /// 设置全局异常处理程序。
        /// </summary>
        /// <param name="handler">异常处理程序。</param>
        public static void set_exception_handler(Action<Exception> handler)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                handler(e.ExceptionObject as Exception);
            };

            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                handler(e.Exception);
                e.SetObserved();
            };
        }

        /// <summary>
        /// 异常处理程序。
        /// </summary>
        /// <param name="ex">捕获的异常。</param>
        public static void HandleException(Exception ex)
        {
            // 在这里处理异常，例如记录日志或显示错误信息
           print("捕获到未处理的异常:");
           print($"消息: {ex.Message}");
           print($"堆栈跟踪: {ex.StackTrace}");
        }

        public static void set_error_handler()
        {
            AppDomain.CurrentDomain.UnhandledException += exCls.CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += exCls.TaskScheduler_UnobservedTaskException;
        }

        // set_error_handler
        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
           print("捕获未处理的同步异常：");
           print(((Exception)e.ExceptionObject).Message);
            // 这里可以记录日志或执行其他处理
            logCls.logErr2025((Exception)e.ExceptionObject, "CurrentDomain_UnhandledException", "errlog");
        }

        public static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
           print("捕获未处理的异步异常：");
           print(e.Exception.Message);
            // 这里可以记录日志或执行其他处理
            e.SetObserved(); // 标记异常已观察到，防止程序崩溃

            logCls.logErr2025((Exception)e.Exception, "TaskScheduler_UnobservedTaskException", "errlog");
        }
    }
}
