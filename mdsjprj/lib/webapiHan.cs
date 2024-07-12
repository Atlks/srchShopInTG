using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace mdsj.lib
{
    internal class webapiHan
    {
        private static void http请求处理器(Action<HttpContext> 特定api, HttpContext http上下文, string api前缀)
        {
            HttpContext context2 = http上下文;
            HttpResponse HTTP响应对象 = context2.Response;
            // 获取当前请求的 URL
            var HTTP请求对象 = http上下文.Request;
            var url = $"{HTTP请求对象.Scheme}://{HTTP请求对象.Host}{HTTP请求对象.Path}{HTTP请求对象.QueryString}";



            string 路径 = HTTP请求对象.Path;


            //----------------static res

            Hashtable 扩展名与处理器对应表 = 新建哈希表hashtb();

            扩展名与处理器对应表.Add("txt   css js", html文件处理器);
            //txt   text / plain; charset = UTF - 8
            //pdf word zip file
            //css  text/css; charset=UTF-8
            //js   text/javascript
            扩展名与处理器对应表.Add(" html htm", html文件处理器);
            扩展名与处理器对应表.Add("json", jsonfl_httpHdlrFilJson);
            扩展名与处理器对应表.Add("jpg png", img_httpHdlrFilImg);
            //png   image/png
            //ico  image/x-icon
            object? audioRespAsync = null;
            扩展名与处理器对应表.Add("mp3", audioRespAsync);
            object? videoRespAsync = null;
            扩展名与处理器对应表.Add("mp4", videoRespAsync);
            //     Invk(img_httpHdlrFilImg);
            文件响应处理(http上下文, 扩展名与处理器对应表);



            //-------------------swag doc api
            特定api(http上下文);

            //------------httpHdlrApi--def json api mode
            设置响应内容类型和编码(http上下文, "application/json; charset=utf-8");

            var 函数名称 = 子文本截取(路径, 1);
            var 查询字符串 = HTTP请求对象.QueryString.ToString();
            object 输出结果 = 调用(api前缀 + 函数名称, 子文本截取(查询字符串, 1));
            //   context.Response.ContentEncoding = Encoding.UTF8;
            发送响应(输出结果, http上下文);
        }

        public static void 文件响应处理(HttpContext http上下文, Hashtable 扩展名与处理器对应表)
        {
            // 获取当前请求的 URL
            var HTTP请求对象 = http上下文.Request;
            var HTTP响应对象 = http上下文.Response;
            var url = $"{HTTP请求对象.Scheme}://{HTTP请求对象.Host}{HTTP请求对象.Path}{HTTP请求对象.QueryString}";
            string 路径 = 解码URL( HTTP请求对象.Path);       
            if (路径.Contains("analytics"))
                调试输出("Dbg");
            遍历哈希表(扩展名与处理器对应表, (DictionaryEntry de) =>
            {
                string[] 扩展名数组 = 拆分(de.Key);
                遍历数组(扩展名数组, ( 扩展名) =>
                {
                    if (路径包含扩展名结尾(路径, 扩展名))
                    {
                        var 处理函数 = (Func<HttpContext, System.Threading.Tasks.Task>)de.Value;
                        var 结果task = 处理函数(http上下文);
                        等待异步任务执行完毕(结果task);
                        跳转到结束();
                    }
                });
            });
            //------------------other ext use down mode
            string 文件路径 = $"{web根目录}{路径}";
            文件路径 = 格式化路径(文件路径);
            if (文件有扩展名(文件路径) && 文件存在(文件路径))
            {
                FileInfo 文件信息 = new FileInfo(文件路径);
                long 文件体积 = 文件信息.Length;
                if (文件体积 < 1000 * 1000)
                {
                    html文件处理器(http上下文); 跳转到结束();
                }
                else
                {
                    fildown_httpHdlrFilDown(http上下文); 跳转到结束(); ;
                }
            }
            if (文件有扩展名(文件路径) && 文件不存在(文件路径))
            {             
                发送响应_资源不存在(HTTP响应对象);
                跳转到结束();
            }
            //---------------- rest nt have ext
            //  var filepath = path.Substring(1);
            if (!文件有扩展名(文件路径) && 文件存在(文件路径))
            {
                html文件处理器(http上下文);
                跳转到结束();
            }
        }

     

        public static async System.Threading.Tasks.Task html文件处理器(HttpContext http上下文)
        {
            // 获取当前请求的 URL
            var http请求对象 = http上下文.Request;
            string 路径 = http请求对象.Path;

            设置响应内容类型和编码(http上下文, "text/html; charset=utf-8");
            string f = web根目录 + 解码URL(路径);
            object 内容 = 读入文本(f);
            发送响应(内容, http上下文);
            跳转到结束();

        }


        public static string web根目录 = $"{prjdir}/webroot";





    }
}
