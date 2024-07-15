global using static mdsj.lib.htmlCls;
using HtmlAgilityPack;
using prj202405.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace mdsj.lib
{
    internal class htmlCls
    {
        public static string GetHtmlContent(string url)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.print_call_FunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), url));

            using (HttpClient client = new HttpClient())
            {
                try
                {       // 设置用户代理
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

                    HttpResponseMessage response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();
                    string htmlContent = response.Content.ReadAsStringAsync().Result;
                    dbgCls.print_ret(__METHOD__, htmlContent.Substring(0, 300));
                    return htmlContent;
                }
                catch (HttpRequestException e)
                {
                    print($"Request error: {e.Message}");
                    dbgCls.print_ret(__METHOD__, 0);
                    return null;
                }
            }
        }
         
        public static string htm_strip_tags(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return string.Empty;
            }


            // Remove <script> tags and their content
            string noScript = Regex.Replace(html, "<script.*?>.*?</script>", string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            // Regular expression to match HTML tags
            string pattern = "<.*?>";

            // Replace HTML tags with an empty string
            //    string result = Regex.Replace(noScript, pattern, string.Empty);
            // Replace HTML tags with an empty string
            string result = Regex.Replace(noScript, pattern, string.Empty, RegexOptions.Singleline);

            result = RemoveExtraNewlines(result);
            result = RemoveInvisibleCharacters(result);
            result = delEmpltyLines(result);

            result = RemoveExtraNewlines(result); result = RemoveExtraNewlines(result);
            //result = RemoveExtraNewlines(result); result = RemoveExtraNewlines(result);
            return result;
        }

        static void RemoveCustomEmojiRendererElement(string inputFilePath, string outputFilePath)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.Load(inputFilePath);

            // 在这里添加你的代码来去除 custom-emoji-renderer-element 标签
            // 这里提供一个示例来移除所有的 custom-emoji-renderer-element 标签
            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//custom-emoji-renderer-element"))
            {
                node.ParentNode.RemoveChild(node);
            }

            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//custom-emoji-element"))
            {
                node.ParentNode.RemoveChild(node);
            }

            doc.Save(outputFilePath);
        }
    }
}
