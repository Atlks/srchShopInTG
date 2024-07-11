global using static mdsj.lib.webapi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
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

        /*
             webapiStart((context) =>
            {
                var request = context.Request;
                string methd = request.Path;
              ////  methd = methd.Substring(1);
                if (methd == "/swag")
                {
                    context.Response.ContentType = "text/html; charset=utf-8";
                     var rzt= Wbapi_swagApi("mdsj.xml");
                    context.Response.WriteAsync(rzt.ToString(), Encoding.UTF8).GetAwaiter().GetResult();
                    jmp2end();
                }
                  
            });
         
         */
        public static void webapiStart(Action<HttpContext> act)
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
            app.Run(async (HttpContext context) =>
            {
                try
                {
                    // 获取当前请求的 URL
                    var request = context.Request;
                    var url = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";

                    // 获取查询字符串
                    var queryString = request.QueryString.ToString();
                    string methd = request.Path;

                    // 设置响应内容类型和编码
                    context.Response.ContentType = "application/json; charset=utf-8";

                    act(context);
                    methd = methd.Substring(1);
                    object rzt = callxTryx("Wbapi_" + methd, Substring(queryString, 1));
                    //   context.Response.ContentEncoding = Encoding.UTF8;
                    await context.Response.WriteAsync(rzt.ToString(), Encoding.UTF8);
                }
                catch (jmp2endEx e)
                {
                }
                catch (Exception e)
                {
                    print_catchEx(nameof(webapiStart), e);
                    logErr2025(e, nameof(webapiStart), "errlog");
                }

            });
            app.Run();
        }
        //----------------------------------swag
        // Add services to the container.
        //  builder.Services.AddSwagger();

        //   var app = builder.Build();

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
        //    app.UseDeveloperExceptionPage();
        //    app.UseSwagger();
        //    app.UseSwaggerUI(c =>
        //    {
        //        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mini API V1");
        //    });
        //}
        //--------------------end swag----

        public static string Wbapi_swagApi(string xmlpath)
        {
            //shangjiaID,uid,dafen
            //     SortedList dafenObj = getHstbFromQrystr(qrystr);
            //   ormJSonFL.save(dafenObj, "dafenDatadir/" + dafenObj["shangjiaID"] + ".json");
            ConvertXmlToHtml(xmlpath, xmlpath + ".htm");
            return ReadAllText(xmlpath + ".htm");
        }

    }
}
