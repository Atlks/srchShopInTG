using HtmlAgilityPack;
using Newtonsoft.Json;
using RG3.PF.Abstractions.Entity;
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

          HashSet<string> urlsDownWait = new HashSet<string>();
      //  static HashSet<string> rmvs3 = new HashSet<string>();
          HashSet<string> downedUrlss = newSet("downedUrlss.json");
        public   void spdrTest()
        {
            var starturl = "https://www.khmertimeskh.com/";
            starturl = "https://www.khmertimeskh.com/category/national/#google_vignette";
            starturl = "https://www.khmertimeskh.com/category/business/";
            starturl = "https://e.vnexpress.net/news/business";

            starturl = "https://e.vnexpress.net/news/travel";
            starturl = "https://e.vnexpress.net/news/life";
            starturl = "https://e.vnexpress.net/news/world";
            starturl = "https://e.vnexpress.net/news/perspectives";
            starturl = "https://e.vnexpress.net/";
            starturl = "https://e.vnexpress.net/news/news";
            starturl = "https://laotiantimes.com/";
            starturl = "https://laotiantimes.com/category/economy/";
            starturl = "https://laotiantimes.com/category/business/";
            starturl = "https://www.bangkokpost.com/";
            starturl = "https://myanmar-now.org/en/";
            urlsDownWait.Add(starturl);
          //  rmvs3 = newSet("rmvs3.json");
         
            TaskRun(() =>
            {
                while (true)
                {
                    Thread.Sleep(3000);
                    downloadTask(starturl);
                }

            });//download

            //-----------------
            //parser thrd
            TaskRun(() => {

                while (true)
                {
                    Thread.Sleep(3000);

                    parseHtmlFileTask(starturl);

                }


            });//parser
            //var html = GetHtmlContent(starturl);
            //WriteAllText("downHtmldir/main.htm", html);
           
        }

        private  void downloadTask(string starturl)
        {
            //定时持久化downedUrl
            // setHsstToF(rmvs3, "rmvs3.json");
            setHsstToF(downedUrlss, "downedUrlss.json");
            try
            {
                //排除已经下载的任务
                urlsDownWait.ExceptWith(downedUrlss);
                WriteAllText("urlsDownWait.json", urlsDownWait);

                foreach (string urlMaybeRltv in urlsDownWait)
                {
                    string url1 = starturl + urlMaybeRltv;
                    if (urlMaybeRltv.StartsWith("http"))
                        url1 = urlMaybeRltv;
                    Download(url1, "downHtmtTaskQue");
                    Download(url1, "downHtmldirLog");

                    downedUrlss.Add(urlMaybeRltv);
                    downedUrlss.Add(url1);

                }


                urlsDownWait.ExceptWith(downedUrlss);
            }
            catch (Exception e)
            {
                print_catchEx("down thrd", e);
            }

        }

        private  void parseHtmlFileTask(string starturl)
        {
            // 获取文件夹中的所有文件
            string[] files = Directory.GetFiles("downHtmtTaskQue");

            // 遍历文件列表
            foreach (string file in files)
            {
                try
                {
                    // 读取每个文件的内容
                    string html = File.ReadAllText(file);

                    HashSet<string> urls = ExtractHrefAttributes(html);
                    WriteAllText("url1119.json", urls);
                    //   urls = FilterUrlsEndwithHtm(urls);
                    foreach_HashSet(urls, (string urlMaybeRltv) =>
                    {
                        string ext = GetExtension(urlMaybeRltv);
                        if (EndsWith(ext, "js css jpg png gif ico jpeg mp3 mp4"))
                            return;
                        // if (ext.EndsWith("htm") || ext.EndsWith("html"))
                        {
                            string url1 = starturl + urlMaybeRltv;
                            if (urlMaybeRltv.StartsWith("http"))
                                url1 = urlMaybeRltv;
                            urlsDownWait.Add(url1);
                        }

                    });

                    File.Delete(file);
                }catch(Exception e)
                {
                    print_catchEx(nameof(parseHtmlFileTask),e);
                }
               
            }
        }

        private static void setHsstToF(HashSet<string> downedUrl, string v)
        {
            WriteAllText(v, downedUrl);
        }

        private static HashSet<string> newSet(string v)
        {
            try
            {
                if(!isFileExist(v))
                    return new HashSet<string>();
                string json = File.ReadAllText(v);
                HashSet<string> hashSet = JsonConvert.DeserializeObject<HashSet<string>>(json);
                return hashSet;
            }
            catch (Exception ex)
            {
                ConsoleWriteLine($"An error occurred: {ex.Message}");
                return new HashSet<string>();
            }
        }

        private static HashSet<string> LdHsstFrmFJsonDecd(string v)
        {
            return (ReadFileToHashSet(v));
        }
        static HashSet<string> ReadFileToHashSet(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                HashSet<string> hashSet = JsonConvert.DeserializeObject<HashSet<string>>(json);
                return hashSet;
            }
            catch (Exception ex)
            {
                ConsoleWriteLine($"An error occurred: {ex.Message}");
                return new HashSet<string>();
            }
        }
    }
}
