global using static mdsj.lib.webapi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using prj202405.lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdsj.lib
{
    internal class webapi
    {
        public static void webapiStart()
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



            //拦截所有请求：
            app.Run(async (HttpContext context) =>
            {
                // 获取当前请求的 URL
                var request = context.Request;
                var url = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";

                // 获取查询字符串
                var queryString = request.QueryString.ToString();
                string methd = request.Path;
                methd = methd.Substring(1);
                object rzt = callxTryx("Wbapi_" + methd, queryString.Substring(1));
  
                // 设置响应内容类型和编码
                 context.Response.ContentType = "application/json; charset=utf-8";
             //   context.Response.ContentEncoding = Encoding.UTF8;
                await context.Response.WriteAsync(rzt.ToString(), Encoding.UTF8);
            });
            app.Run();
        }

      
    }
}
