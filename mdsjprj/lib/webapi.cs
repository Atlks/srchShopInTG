global using static mdsj.lib.webapi;
using FFmpeg.AutoGen;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Nethereum.KeyStore;
using prj202405.lib;
using RG3.PF.Abstractions.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
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
        /// <summary>
        /// /
        /// </summary>
        /// <param name="act"></param>
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
            app.Run((RequestDelegate)(async (HttpContext context) =>
            {
                try
                {
                    HttpContext context2 = context;
                    HttpResponse response = context2.Response;
                    // 获取当前请求的 URL
                    var request = context.Request;
                    var url = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";

                    // 获取查询字符串
                    var queryString = request.QueryString.ToString();
                    string path = request.Path;

                    //static res
                    var staticResDir = $"{prjdir}/webroot";
                    Hashtable ht = new Hashtable();

                    ht.Add("txt   css js", txthtmRespAsync);
                    //txt   text / plain; charset = UTF - 8
                    //pdf word zip file
                    //css  text/css; charset=UTF-8
                    //js   text/javascript
                    ht.Add(" html htm", txthtmRespAsync);
                    ht.Add("json", jsonRespAsync);
                    ht.Add("jpg png", imageRespAsync);
                    //png   image/png
                    //ico  image/x-icon
                    object? audioRespAsync = null;
                    ht.Add("mp3", audioRespAsync);
                    object? videoRespAsync = null;
                    ht.Add("mp4", videoRespAsync);
                    Invk(imageRespAsync);
                    processStaticRes(context, ht);
                    //--------------def json api mode
                    // 设置响应内容类型和编码
                    context.Response.ContentType = "application/json; charset=utf-8";

                    act(context);
                    path = path.Substring(1);
                    object rzt = callxTryx("Wbapi_" + path, Substring(queryString, 1));
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

            }));
            app.Run();
        }

        private static void Invk(Func<HttpContext, System.Threading.Tasks.Task> imageRespAsync)
        {

        }

        static void test(HttpContext context)
        {
            Hashtable ht = new Hashtable();

            ht.Add("jpg png", imageRespAsync2);
            processStaticRes(context, ht);
        }
        public static async System.Threading.Tasks.Task imageRespAsync2(HttpContext context)
        {

        }
        private static void processStaticRes(HttpContext context, Hashtable ht)
        {
            // 获取当前请求的 URL
            var request = context.Request;
            var url = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";

            // 获取查询字符串
            var queryString = request.QueryString.ToString();
            string path = request.Path;

            foreach_hashtable(ht,  (DictionaryEntry de) =>
            {
                string[] exts = de.Key.ToString().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                foreach (string ext in exts)
                {
                    if (path.ToUpper().Trim().EndsWith("." + ext.Trim().ToUpper()))
                    {
                        //   MethodInfo a
                        var typex = de.Value.GetType().ToString();
                        //  Delegate? value = (Delegate)de.Value;
                        //   callx(value, context);


                        var func1 = (Func<HttpContext, System.Threading.Tasks.Task>)de.Value;

                        // 调用异步处理方法，并等待完成
                        //try
                        //{
                            var task = func1(context);
                            task.Wait(); // 或者使用 await task;
                        //}
                        //catch (jmp2endEx e)
                        //{
                        //    throw e;
                        //}
                        //catch (Exception ex)
                        //{
                        //    // 异常处理逻辑
                        //    ConsoleWriteLine($"发生异常：{ex.Message}");
                        //    throw ex;
                        //}

                        //   await task;
                        //   return;
                        jmp2end();

                    }
                }



            });

            //other ext use down mode
            // 获取文件的实际扩展名
            string fileExtension = Path.GetExtension(path);
            if (fileExtension != "")
            {
                downRespAsync(context); return;
            }

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
        public static string staticResDir = $"{prjdir}/webroot";
        public static async System.Threading.Tasks.Task txthtmRespAsync(HttpContext context)
        {
            // 获取当前请求的 URL
            var request = context.Request;
            string path = request.Path;
            // if (path.EndsWith(".htm"))
            {
                // 设置响应内容类型和编码
                context.Response.ContentType = "text/html; charset=utf-8";
                object rzt2 = ReadAllText(staticResDir + path);
                await context.Response.WriteAsync(rzt2.ToString(), Encoding.UTF8);
                jmp2end(); return;
            }
        }
        public static async System.Threading.Tasks.Task downRespAsync(HttpContext context)
        {
            HttpContext context2 = context;
            HttpResponse response = context2.Response;
            // 获取当前请求的 URL
            var request = context.Request;
            string path = request.Path;
            // 设置响应内容类型和编码


            //    HttpListenerResponse response = (HttpListenerResponse)response2;
            string path1 = staticResDir + path;
            if (!existFil(path1))
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                StreamWriter writer = new StreamWriter(response.Body);
                writer.Write("res not found");
                return;
            }
            byte[] buffer = File.ReadAllBytes(path1);
            response.ContentType = "application/octet-stream";
            response.ContentLength = buffer.Length;
            //   await response.WriteAsync(buffer, 0, buffer.Length);
            await response.Body.WriteAsync(buffer, 0, buffer.Length);
            response.Body.Close();
            jmp2end();
            return;


        }


        public static async System.Threading.Tasks.Task imageRespAsync(HttpContext context)
        {
            HttpContext context2 = context;
            HttpResponse response = context2.Response;
            // 获取当前请求的 URL
            var request = context.Request;
            string path = request.Path;
            // 设置响应内容类型和编码


            //    HttpListenerResponse response = (HttpListenerResponse)response2;
            string path1 = staticResDir + path;
            if (!existFil(path1))
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                StreamWriter writer = new StreamWriter(response.Body);
                writer.Write("Image not found");
                jmp2end();
                return;
            }
            byte[] buffer = File.ReadAllBytes(path1);
            response.ContentType = "image/jpeg";
            response.ContentLength = buffer.Length;
            //   await response.WriteAsync(buffer, 0, buffer.Length);
            await response.Body.WriteAsync(buffer, 0, buffer.Length);
            response.Body.Close();
            jmp2end();
            return;


        }

        public static async System.Threading.Tasks.Task jsonRespAsync(HttpContext context)
        {
            // 获取当前请求的 URL
            var request = context.Request;
            string path = request.Path;
            // 设置响应内容类型和编码
            context.Response.ContentType = "application/json; charset=utf-8";
            object rzt2 = ReadAllText(staticResDir + path);
            await context.Response.WriteAsync(rzt2.ToString(), Encoding.UTF8);
            jmp2end();
            return;

        }

        public static string Wbapi_swagApi(string xmlpath, HttpContext context)
        {
            context.Response.ContentType = "text/html; charset=utf-8";
            //shangjiaID,uid,dafen
            //     SortedList dafenObj = getHstbFromQrystr(qrystr);
            //   ormJSonFL.save(dafenObj, "dafenDatadir/" + dafenObj["shangjiaID"] + ".json");
            ConvertXmlToHtml(xmlpath, xmlpath + ".htm");
            return ReadAllText(xmlpath + ".htm");
        }

    }
}
