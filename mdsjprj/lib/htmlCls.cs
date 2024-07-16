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
        public static void Download(string url, string dataDir)
        {// 生成一个新的 UUID
            Guid newUuid = Guid.NewGuid();
            var html = GetHtmlContent(url);
            WriteAllText($"{dataDir}/{newUuid}.htm", html);
        }
        public static HashSet<string> ExtractHrefAttributes(string html)
        {
            HashSet<string> hrefs = new HashSet<string>();

            // 创建 HtmlDocument 对象并加载 HTML
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            // 查找所有 a 标签
            var nodes = doc.DocumentNode.SelectNodes("//a[@href]");

            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    // 提取 href 属性值并添加到 HashSet 中
                    string href = node.GetAttributeValue("href", string.Empty);
                    if (!string.IsNullOrEmpty(href))
                    {
                        hrefs.Add(href);
                    }
                }
            }

            return hrefs;
        }
        public static HashSet<string> ExtractUrls(string input)
        {
            HashSet<string> urls = new HashSet<string>();

            // 定义匹配 URL 的正则表达式
            string pattern = @"https?://[^\s/$.?#].[^\s]*";
            Regex regex = new Regex(pattern);

            // 使用正则表达式匹配所有 URL
            MatchCollection matches = regex.Matches(input);

            // 将匹配的 URL 添加到 HashSet 中
            foreach (Match match in matches)
            {
                urls.Add(match.Value);
            }

            return urls;
        }

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
