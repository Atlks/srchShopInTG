using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

        public static void startWebapiV2(Delegate httpHdlrSpel, string api_prefix)
        {
            var builder = WebApplication.CreateBuilder();
            // Configure Kestrel to listen on a specific port
            ConfigureWebHostBuilder webHost = builder.WebHost;
            webHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.ListenAnyIP(5000); // 自定义端口号，例如5001
            });
            var app = builder.Build();
            //http://localhost:5000/dafen?callGetlistFromDb=yourValue11
            //拦截请求：
            RequestDelegate RequestDelegate1 = async (HttpContext context) =>
            {
                try
                {
                    http请求处理器(context.Request,context.Response, api_prefix, httpHdlrSpel);
                }
                catch (jmp2endEx e)
                {
                }
                catch (Exception e)
                {
                    PrintCatchEx(nameof(StartWebapi), e);
                    logErr2025(e, nameof(StartWebapi), "errlog");
                }

            };
            app.Run(RequestDelegate1);
            app.Run();
        }

        /// <summary>
        ///    //txt   text / plain; charset = UTF - 8
        //pdf word zip file
        //css  text/css; charset=UTF-8
        //js   text/javascript
        //png   image/png
        //ico  image/x-icon
        /// </summary>
        /// <param name="特定api"></param>
        /// <param name="http上下文"></param>
        /// <param name="api前缀"></param>
        private static void http请求处理器(HttpRequest http请求对象, HttpResponse HTTP响应对象,   string api前缀, Delegate 特定api)
        {
          //  var url = $"{http请求对象.Scheme}://{HTTP请求对象.Host}{HTTP请求对象.Path}{HTTP请求对象.QueryString}";
            string 路径 = http请求对象.Path;
            //----------------static res
            Hashtable 扩展名与处理器对应表 = 新建哈希表hashtb();
            扩展名与处理器对应表.Add("txt   css js", nameof(html文件处理器));         
            扩展名与处理器对应表.Add(" html htm", nameof(html文件处理器));
        //    扩展名与处理器对应表.Add("json", jsonfl_httpHdlrFilJson);
          //  扩展名与处理器对应表.Add("jpg png", img_httpHdlrFilImg);  
            文件响应处理( 扩展名与处理器对应表, http请求对象,HTTP响应对象);
            //-------------------swag doc api
            调用(特定api, http请求对象, HTTP响应对象);
          
            //------------httpHdlrApi--def json api mode
            设置响应内容类型和编码(HTTP响应对象, "application/json; charset=utf-8");
            var 函数名称 = 子文本截取(路径, 1);
            var 查询字符串 = http请求对象.QueryString.ToString();
            object 输出结果 = 调用(api前缀 + 函数名称, 子文本截取(查询字符串, 1));
            //   context.Response.ContentEncoding = Encoding.UTF8;
            发送响应(输出结果, HTTP响应对象);
        }

        public static void 文件响应处理(  Hashtable 扩展名与处理器对应表, HttpRequest http请求对象, HttpResponse HTTP响应对象)
        { 
          //  var url = $"{HTTP请求对象.Scheme}://{HTTP请求对象.Host}{HTTP请求对象.Path}{HTTP请求对象.QueryString}";
            string 路径 = 解码URL(http请求对象.Path);       
            if (路径.Contains("analytics"))
                调试输出("Dbg");
            遍历哈希表(扩展名与处理器对应表, (DictionaryEntry de) =>
            {
                string[] 扩展名数组 = 拆分(de.Key);
                遍历数组(扩展名数组, ( 扩展名) =>
                {
                    if (路径包含扩展名结尾(路径, 扩展名))
                    {
                        var 处理函数 = de.Value.ToString();
                         var 结果task = 调用(处理函数, http请求对象, HTTP响应对象);
                    //    等待异步任务执行完毕(结果task);
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
                    html文件处理器(http请求对象, HTTP响应对象); 跳转到结束();
                }
                else
                {
                    FildownHttpHdlrFilDown(http请求对象, HTTP响应对象); 跳转到结束(); ;
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
                html文件处理器(http请求对象, HTTP响应对象);
                跳转到结束();
            }
        }

        public static void html文件处理器(HttpRequest http请求对象, HttpResponse HTTP响应对象)
        {
            // 获取当前请求的 URL
          
            string 路径 = http请求对象.Path;

            设置响应内容类型和编码(HTTP响应对象, "text/html; charset=utf-8");
            string f = web根目录 + 解码URL(路径);
            object 内容 = 读入文本(f);
            发送响应(内容, HTTP响应对象);
            跳转到结束();
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
