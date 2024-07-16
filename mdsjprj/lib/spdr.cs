﻿using HtmlAgilityPack;
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


        HashSet<string> urlsDownWait_hashset =newSet($"spdr/urlsDownWait{DateTime.Now.ToString("dd_HHmmss")}.json");
        HashSet<string> downedUrlssHashset = newSet("spdr/downedUrlss.json");
        string parserUrlQue = "spdr/downHtmTaskQue";
        // downHtmldirLog
        public void SpdrTest()
        {
            string timestamp2 = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            var starturl = "https://www.khmertimeskh.com/";
  

            starturl = "https://laotiantimes.com/";
            starturl = "https://laotiantimes.com/category/economy/";
            starturl = "https://laotiantimes.com/category/business/";
            starturl = "https://www.bangkokpost.com/";
            starturl = "https://myanmar-now.org/en/";
            starturl = "https://e.vnexpress.net/";
            starturl = "https://www.bangkokpost.com/";
            urlsDownWait_hashset.Add(starturl);
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
            TaskRun(() =>
            {

                while (true)
                {
                    Thread.Sleep(3000);
                    parseHtmlFileTask(starturl);

                }


            });//parser
               //var html = GetHtmlContent(starturl);
               //WriteAllText("downHtmldir/main.htm", html);

        }

        private void downloadTask(string starturl)
        {

            try
            {
                //排除已经下载的任务
                urlsDownWait_hashset.ExceptWith(downedUrlssHashset);
              //  WriteAllText("urlsDownWait.json", urlsDownWait);

                foreach (string urlMaybeRltv in urlsDownWait_hashset)
                {
                    string url1 = starturl + urlMaybeRltv;
                    if (urlMaybeRltv.StartsWith("http"))
                        url1 = urlMaybeRltv;
                    TaskRun(() =>
                    {
                        Download(url1, parserUrlQue);
                    });
                        

                    downedUrlssHashset.Add(urlMaybeRltv);
                    downedUrlssHashset.Add(url1);

                }


                urlsDownWait_hashset.ExceptWith(downedUrlssHashset);
            }
            catch (Exception e)
            {
                print_catchEx("down thrd", e);
            }

        }

        private void parseHtmlFileTask(string starturl)
        {
            // 获取文件夹中的所有文件
            string[] files = Directory.GetFiles(parserUrlQue);

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
                            urlsDownWait_hashset.Add(url1);
                        }

                    });
                    MoveFileToDirectory(file, "spdr/downHtmldirLog");
                    //  File.Delete(file);
                }
                catch (Exception e)
                {
                    print_catchEx(nameof(parseHtmlFileTask), e);
                }

            }
        }

    }
}
