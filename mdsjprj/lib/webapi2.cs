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
    internal class webapi2
    {
        internal static async System.Threading.Tasks.Task StartWbapiAsync()
        {
            TaskRun(async () =>
            {
                System.Net.HttpListener listener = new HttpListener();
                listener.Prefixes.Add("http://localhost:5001/");
                listener.Start();
                ConsoleWriteLine("Listening for requests at http://localhost:5001/");

                while (true)
                {
                    HttpListenerContext context = await listener.GetContextAsync();
                    HttpListenerRequest request = context.Request;
                    HttpListenerResponse response = context.Response;
                    try
                    {
                        httpHdlrV2(request, response);

                        response.OutputStream.Close();
                    }
                    catch (jmp2endEx e)
                    {

                    }
                    catch (Exception e)
                    {
                        PrintCatchEx(nameof(StartWbapiAsync), e);
                    }

                }
            });
          
        }

        //if (request.Url.AbsolutePath == "/image/jpg")
        //{
        //    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "image.jpg");

        //    if (File.Exists(imagePath))
        //    {
        //        byte[] buffer = File.ReadAllBytes(imagePath);
        //        response.ContentType = "image/jpeg";
        //        response.ContentLength64 = buffer.Length;
        //        await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        //    }
        //    else
        //    {
        //        response.StatusCode = (int)HttpStatusCode.NotFound;
        //        using (StreamWriter writer = new StreamWriter(response.OutputStream))
        //        {
        //            writer.Write("Image not found");
        //        }
        //    }
        //}
        //else
        //{
        //    response.StatusCode = (int)HttpStatusCode.NotFound;
        //    using (StreamWriter writer = new StreamWriter(response.OutputStream))
        //    {
        //        writer.Write("Resource not found");
        //    }
        //}

        private static void httpHdlrV2(HttpListenerRequest request, HttpListenerResponse response )
        {

         //   var url = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
            // 获取查询字符串
            var queryString = request.QueryString.ToString();
            string path = request.Url.AbsolutePath;
            //----------------static res
            // 静态资源处理器映射表
            Hashtable extNhdlrChoosrMaplist = new Hashtable();
            extNhdlrChoosrMaplist.Add("txt   css js", Html_httpHdlrfilTxtHtmlV2); ;
            extNhdlrChoosrMaplist.Add("json", jsonfl_httpHdlrFilJsonV2);
           
            httpHdlrFilV2(request, response, extNhdlrChoosrMaplist);
            //-------------------swag doc api
            // 处理特定API  doc api
          //  callx(httpHdlrApiSpecl, request, response);
            //------------httpHdlrApi--def json api mode
            // 设置响应内容类型和编码
            SetRespContentTypeNencodeV2(response, "application/json; charset=utf-8");
            var fn = path.Substring(1);
            var api_prefix = "webapi";
            object rzt = CallxTryx(api_prefix + fn, Substring(queryString, 1));
            // 发送响应
            SendRespV2(rzt, response);
        }
        public static async System.Threading.Tasks.Task Html_httpHdlrfilTxtHtmlV2(HttpListenerRequest request, HttpListenerResponse response)
        {
            // 获取当前请求的 URL

            string path = request.Url.AbsolutePath;
            // if (path.EndsWith(".htm"))
            {
                // 设置响应内容类型和编码
                response.ContentType = "text/html; charset=utf-8";
                string f = webrootDir + DecodeUrl(path);
                object rzt2 = ReadAllText(f);
                SendRespV2(rzt2.ToString(), response);
                Jmp2endDep(); return;
            }
        }
        public static async System.Threading.Tasks.Task jsonfl_httpHdlrFilJsonV2(HttpListenerRequest request, HttpListenerResponse response)
        {
            // 获取当前请求的 URL
           
            string path = request.Url.AbsolutePath;
            // 设置响应内容类型和编码
            response.ContentType = "application/json; charset=utf-8";
            path = DecodeUrl(path);

            object rzt2 = ReadAllText(webrootDir + path);
            SendRespV2(rzt2.ToString(), response);
            Jmp2endDep();
            return;

        }

        private static void SetRespContentTypeNencodeV2(HttpListenerResponse response, string 内容类型和编码)
        {
            response.ContentType = 内容类型和编码;
        }

        private static void SendRespV2(object 输出结果, HttpListenerResponse response)
        {
            using (StreamWriter writer = new StreamWriter(response.OutputStream))
            {
                writer.Write(输出结果.ToString(), Encoding.UTF8) ;

            }

        }

        private static void httpHdlrFilV2(HttpListenerRequest request, HttpListenerResponse response, Hashtable extnameNhdlrChooser)
        {
            // 获取查询字符串
            var queryString = request.QueryString.ToString();
            string path = request.Url.AbsolutePath;
            path = DecodeUrl(path);

            if (path.Contains("analytics"))
                Print("Dbg");

            ForeachHashtable(extnameNhdlrChooser, (DictionaryEntry de) =>
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
                        Jmp2endDep();

                    }
                }



            });

        }
    }
}
