﻿global using static mdsj.libBiz.strBiz;

using JiebaNet.Segmenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using static prj202405.lib.strCls;
namespace mdsj.libBiz
{
    internal class strBiz
    {

        public static int containCalcCntScoreSetfmt(string segments, HashSet<string> set)
        {
            //  Console.WriteLine(" containCalcCntScoreSetfmt() "+string.Join(' ', segments));
            //   Console.WriteLine();
            set.Remove("店");
            set.Remove("飞机号");

            HashSet<string> blackListWd = new HashSet<string>();

            blackListWd.Add("店");
            blackListWd.Add("飞机号");


            int n = 0;
            foreach (string kwd in set)
            {
                var kwd2 = kwd.Trim();

                if (kwd2.Length == 0)
                    continue;



                if (segments.Contains(kwd2))
                {
                    n++;
                    Console.WriteLine(" containChk2024. kwd=>" + kwd2);
                }


            }
            return n;
        }

        public static int containCalcCntScoreSetfmt(HashSet<string> set, string[] segments)
        {
            //  Console.WriteLine(" containCalcCntScoreSetfmt() "+string.Join(' ', segments));
            //   Console.WriteLine();
            set.Remove("店");
            set.Remove("飞机号");

            HashSet<string> blackListWd = new HashSet<string>();

            blackListWd.Add("店");
            blackListWd.Add("飞机号");


            int n = 0;
            foreach (string kwd in segments)
            {
                var kwd2 = kwd.Trim();

                if (kwd2.Length > 0)
                {
                    if (blackListWd.Contains(kwd2))
                        continue;
                    if (strBiz_isPostnWord(kwd2))
                        continue;
                    if (set.Contains(kwd2))
                    {
                        n++;
                        Console.WriteLine(" contain. kwd=>" + kwd2);
                    }

                }
            }
            return n;
        }
        public static string[] removeStopWd4biz(string[] kwds)
        {
            //todo 去除触发词，，只保留 服务次和位置词
            HashSet<string> 搜索触发词 = ReadWordsFromFile("搜索触发词.txt");

            HashSet<string> stopWdSet = new HashSet<string>();

            //园区
            stopWdSet.Add("店"); stopWdSet.Add("的"); stopWdSet.Add("号");
            stopWdSet.Add("飞机号"); stopWdSet.Add("飞机");
            stopWdSet.Add("园区");


            // 使用HashSet的构造函数将字符串数组转换为HashSet
            HashSet<string> kwdSt = new HashSet<string>(kwds);
            // 移除set2中的元素
            kwdSt.ExceptWith(搜索触发词);
            kwdSt.ExceptWith(stopWdSet);

            return kwdSt.ToArray();

        }
        public static bool strBiz_isPostnWord(string kwd2)
        {
            HashSet<string> citys = ReadLinesToHashSet("位置词.txt");
            return citys.Contains(kwd2);
        }
        public static string[] splt_by_fenci458prj(ref string msgx)
        {
            msgx = ChineseCharacterConvert.Convert.ToSimple(msgx);
            var segmenter = new JiebaSegmenter();
            segmenter.LoadUserDict("user_dict.txt");
            segmenter.AddWord("会所"); // 可添加一个新词
            segmenter.AddWord("妙瓦底"); // 可添加一个新词
            segmenter.AddWord("御龙湾"); // 可添加一个新词
            HashSet<string> user_dict = ReadLinesToHashSet("user_dict.txt");
            foreach (string line in user_dict)
            {
                segmenter.AddWord(line);
            }
            HashSet<string> postnKywd位置词set = ReadLinesToHashSet("位置词.txt");
            foreach (string line in postnKywd位置词set)
            {
                segmenter.AddWord(line);
            }




            IEnumerable<string> enumerable = segmenter.CutForSearch(msgx);
            // 使用 LINQ 的 ToArray 方法进行转换
            string[] kwds = enumerable.ToArray();
            //  string[] kwds = enumerable; // 搜索引擎模式
            string kdwsJoin = string.Join("/", kwds);
            Console.WriteLine("【搜索引擎模式】：{0}", kdwsJoin);
            return kwds;
        }
        public static string substr_getFuwuci(string? text, HashSet<string> 商品与服务词库)
        {
            if (text == null)
                return null;
            string[] spltWds = splt_by_fenci458prj(ref text);
            foreach (string wd in spltWds)
            {
                if (商品与服务词库.Contains(wd))
                    return wd;
            }
            return null;
        }
    }
}
