using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdsj.lib
{
    internal class net_http
    {
        /// <summary>
        /// 发出 HTTP GET 请求并返回响应内容
        /// </summary>
        /// <param name="url">请求的 URL</param>
        /// <returns>HTTP 响应内容</returns>
        public static async Task<string> GetHttpResponseAsync(string url)
        {
            // 创建 HttpClient 实例
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // 发出 GET 请求
                    HttpResponseMessage response = await client.GetAsync(url);

                    // 确认响应状态成功
                    response.EnsureSuccessStatusCode();

                    // 读取响应内容
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                catch (HttpRequestException e)
                {
                    // 捕获并处理请求异常
                    Console.WriteLine($"Request exception: {e.Message}");
                    return null;
                }
            }
        }
    }
}
