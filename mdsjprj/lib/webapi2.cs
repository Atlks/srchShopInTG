using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace mdsj.lib
{
    internal class webapi2
    {
        internal static async System.Threading.Tasks.Task mainWbstartAsync()
        {
            System.Net. HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:5001/");
            listener.Start();
            ConsoleWriteLine("Listening for requests at http://localhost:5001/");

            while (true)
            {
                HttpListenerContext context = await listener.GetContextAsync();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                if (request.Url.AbsolutePath == "/image/jpg")
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "image.jpg");

                    if (File.Exists(imagePath))
                    {
                        byte[] buffer = File.ReadAllBytes(imagePath);
                        response.ContentType = "image/jpeg";
                        response.ContentLength64 = buffer.Length;
                        await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                    }
                    else
                    {
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        using (StreamWriter writer = new StreamWriter(response.OutputStream))
                        {
                            writer.Write("Image not found");
                        }
                    }
                }
                else
                {
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    using (StreamWriter writer = new StreamWriter(response.OutputStream))
                    {
                        writer.Write("Resource not found");
                    }
                }

                response.OutputStream.Close();
            }
        }

       
    }
}
