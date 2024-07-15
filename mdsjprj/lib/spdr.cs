using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace mdsj.lib
{
    internal class spdr
    {


        private static void Download(string url, string dataDir)
        {// 生成一个新的 UUID
            Guid newUuid = Guid.NewGuid();
            var html = GetHtmlContent(url);
            WriteAllText($"{dataDir}/{newUuid}.htm", html);
        }
        static HashSet<string> ExtractHrefAttributes(string html)
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
        static HashSet<string> ExtractUrls(string input)
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

        public static void spdrTest()
        {
            var url = "https://www.khmertimeskh.com/";
            url = "https://www.khmertimeskh.com/category/national/#google_vignette";
            url = "https://www.khmertimeskh.com/category/business/";
            url = "https://e.vnexpress.net/news/business";
    
            url = "https://e.vnexpress.net/news/travel";
            url = "https://e.vnexpress.net/news/life";
            url = "https://e.vnexpress.net/news/world";
            url = "https://e.vnexpress.net/news/perspectives";
            url = "https://e.vnexpress.net/";
            url = "https://e.vnexpress.net/news/news";
            url = "https://laotiantimes.com/";
            url = "https://laotiantimes.com/category/economy/";
            url = "https://laotiantimes.com/category/business/";


            var html = GetHtmlContent(url);
            WriteAllText("downHtmldir/main.htm", html);
            HashSet<string> urls = ExtractHrefAttributes(html);
            WriteAllText("url1119.json", urls);
         //   urls = FilterUrlsEndwithHtm(urls);
            foreach_HashSet(urls, (string urlMaybeRltv) =>
            {
                string ext = GetExtension(urlMaybeRltv);
                if (EndsWith(ext, "js css jpg png gif"))
                    return;
                // if (ext.EndsWith("htm") || ext.EndsWith("html"))
                {
                    string url1 = url + urlMaybeRltv;
                    if (urlMaybeRltv.StartsWith("http"))
                        url1 = urlMaybeRltv;
                    Download(url1, "downHtmldir");
                }

            });

        }

    }
}
