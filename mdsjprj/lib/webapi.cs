global using static mdsj.lib.webapi;
using FFmpeg.AutoGen;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using MusicApiCollection.Sites.GraceNote.Data;
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
using System.Text.RegularExpressions;
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
        /// startWebapi
        /// </summary>
        /// <param name="httpHdlrSpel"></param>
        /// <param name="api_prefix"></param>
        public static void startWebapi(Action<HttpRequest, HttpResponse> httpHdlrSpel, string api_prefix)
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
                                httpHdlr(context.Request, context.Response, api_prefix, httpHdlrSpel);
                            }
                            catch (jmp2endEx e)
                            {
                            }
                            catch (Exception e)
                            {
                                print_catchEx(nameof(startWebapi), e);
                                logErr2025(e, nameof(startWebapi), "errlog");
                            }

                        };
            app.Run(RequestDelegate1);
            app.Run();
        }
        public static string webrootDir = $"{prjdir}/webroot";


        /*
         
           //txt   text / plain; charset = UTF - 8
            //pdf word zip file
            //css  text/css; charset=UTF-8
            //js   text/javascript
            //png   image/png
            //ico  image/x-icon
            // Invk(img_httpHdlrFilImg);
            // 静态资源处理函数调用
            object? audioRespAsync = null;
            extNhdlrChoosrMaplist.Add("mp3", audioRespAsync);
            object? videoRespAsync = null;
            extNhdlrChoosrMaplist.Add("mp4", videoRespAsync);
        
         
         */
        /// <summary>
        /// httpHdlr
        /// </summary>
        /// <param name="httpHdlrApiSpecl"></param>
        /// <param name="context"></param>
        /// <param name="api_prefix"></param>
        private static void httpHdlr(HttpRequest request, HttpResponse response, string api_prefix, Action<HttpRequest, HttpResponse> httpHdlrApiSpecl)
        {

            var url = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
            // 获取查询字符串
            var queryString = request.QueryString.ToString();
            string path = request.Path;
            //----------------static res
            // 静态资源处理器映射表
            Hashtable extNhdlrChoosrMaplist = new Hashtable();
            extNhdlrChoosrMaplist.Add("txt   css js", Html_httpHdlrfilTxtHtml);
            extNhdlrChoosrMaplist.Add(" html htm", Html_httpHdlrfilTxtHtml);
            extNhdlrChoosrMaplist.Add("json", jsonfl_httpHdlrFilJson);
            extNhdlrChoosrMaplist.Add("jpg png", img_httpHdlrFilImg);
            httpHdlrFil(request, response, extNhdlrChoosrMaplist);
            //-------------------swag doc api
            // 处理特定API
            callx(httpHdlrApiSpecl, request, response);
            //------------httpHdlrApi--def json api mode
            // 设置响应内容类型和编码
            SetRespContentTypeNencode(response, "application/json; charset=utf-8");
            var fn = path.Substring(1);
            object rzt = callxTryx(api_prefix + fn, Substring(queryString, 1));
            // 发送响应
            SendResp(rzt, response);
        }

        private static void Invk(Func<HttpContext, System.Threading.Tasks.Task> imageRespAsync)
        {

        }

        static void test(HttpContext context)
        {
            Hashtable ht = new Hashtable();

            ht.Add("jpg png", imageRespAsync2);
            //   httpHdlrFil(context, ht);
        }
        public static async System.Threading.Tasks.Task imageRespAsync2(HttpContext context)
        {

        }

        /// <summary>
        /// httpHdlrFil
        /// </summary>
        /// <param name="context"></param>
        /// <param name="extnameNhdlrChooser"></param>
        public static void httpHdlrFil(HttpRequest request, HttpResponse response, Hashtable extnameNhdlrChooser)
        {

            var url = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";

            // 获取查询字符串
            var queryString = request.QueryString.ToString();
            string path = request.Path;
            path = decodeUrl(path);

            if (path.Contains("analytics"))
                print("Dbg");

            foreach_hashtable(extnameNhdlrChooser, (DictionaryEntry de) =>
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


                    var func1 = de.Value.ToString();

                    var task = 调用(func1, request, response);
                    // var task = func1(context);
                    //  task.Wait();  
                    Jmp2end();

                }
            }



        });

            //------------------other ext use down mode
            // 获取文件的实际扩展名
            string fileExtension = Path.GetExtension(path);
            string filePath = $"{webrootDir}{path}";
            filePath = castNormalizePath(filePath);
            if (文件有扩展名(filePath) && 文件存在(filePath))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                long fileSize = fileInfo.Length;
                if (fileSize < 1000 * 1000)
                {
                    Html_httpHdlrfilTxtHtml(request, response); ; return;
                }

                else
                {
                    fildown_httpHdlrFilDown(request, response); return;

                }

            }

            if (文件有扩展名(filePath) && 文件不存在(filePath))
            {
                发送响应_资源不存在(response);
                跳转到结束();
            }



            //---------------- rest nt have ext
            //  var filepath = path.Substring(1);
            if (fileExtension == "" && File.Exists($"{prjdir}{path}"))
            {
                Html_httpHdlrfilTxtHtml(request, response);
                Jmp2end();
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
        //   public static string staticResDir = $"{prjdir}/webroot";
        public static async System.Threading.Tasks.Task Html_httpHdlrfilTxtHtml(HttpRequest request, HttpResponse response)
        {
            // 获取当前请求的 URL

            string path = request.Path;
            // if (path.EndsWith(".htm"))
            {
                // 设置响应内容类型和编码
                response.ContentType = "text/html; charset=utf-8";
                string f = webrootDir + decodeUrl(path);
                object rzt2 = ReadAllText(f);
                await response.WriteAsync(rzt2.ToString(), Encoding.UTF8);
                Jmp2end(); return;
            }
        }

        public static async System.Threading.Tasks.Task fildown_httpHdlrFilDown(HttpRequest request, HttpResponse response)
        {


            string path = request.Path;
            // 设置响应内容类型和编码


            //    HttpListenerResponse response = (HttpListenerResponse)response2;
            string path1 = webrootDir + path;
            path1 = decodeUrl(path1);
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
            Jmp2end();
            return;


        }


        public static async System.Threading.Tasks.Task fildown_httpHdlrFilDown(HttpContext context)
        {
            HttpContext context2 = context;
            HttpResponse response = context2.Response;
            // 获取当前请求的 URL
            var request = context.Request;
            string path = request.Path;
            // 设置响应内容类型和编码


            //    HttpListenerResponse response = (HttpListenerResponse)response2;
            string path1 = webrootDir + path;
            path1 = decodeUrl(path1);
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
            Jmp2end();
            return;


        }


        public static async System.Threading.Tasks.Task img_httpHdlrFilImg(HttpContext context)
        {
            HttpContext context2 = context;
            HttpResponse response = context2.Response;
            // 获取当前请求的 URL
            var request = context.Request;
            string path = request.Path;
            // 设置响应内容类型和编码


            //    HttpListenerResponse response = (HttpListenerResponse)response2;
            string path1 = webrootDir + path;
            path1 = decodeUrl(path1);
            if (!existFil(path1))
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                StreamWriter writer = new StreamWriter(response.Body);
                writer.Write("Image not found");
                Jmp2end();
                return;
            }
            byte[] buffer = File.ReadAllBytes(path1);
            response.ContentType = "image/jpeg";
            response.ContentLength = buffer.Length;
            //   await response.WriteAsync(buffer, 0, buffer.Length);
            await response.Body.WriteAsync(buffer, 0, buffer.Length);
            response.Body.Close();
            Jmp2end();
            return;


        }

        /// <summary>
        /// httpHdlrFilJson
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task jsonfl_httpHdlrFilJson(HttpContext context)
        {
            // 获取当前请求的 URL
            var request = context.Request;
            string path = request.Path;
            // 设置响应内容类型和编码
            context.Response.ContentType = "application/json; charset=utf-8";
            path = decodeUrl(path);

            object rzt2 = ReadAllText(webrootDir + path);
            await context.Response.WriteAsync(rzt2.ToString(), Encoding.UTF8);
            Jmp2end();
            return;

        }

        /// <summary>
        /// httpHdlrApiSpelDocapi
        /// </summary>
        /// <param name="xmlpath"></param>
        /// <param name="context"></param>
        /// <returns>doc html</returns>
        public static string docapi_httpHdlrApiSpelDocapi(string xmlpath, HttpResponse context)
        {
            context.ContentType = "text/html; charset=utf-8";
            //shangjiaID,uid,dafen
            //     SortedList dafenObj = getHstbFromQrystr(qrystr);
            //   ormJSonFL.save(dafenObj, "dafenDatadir/" + dafenObj["shangjiaID"] + ".json");
            ConvertXmlToHtml(xmlpath, xmlpath + ".htm");
            return ReadAllText(xmlpath + ".htm");
        }

    }
}
