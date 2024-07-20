global using static mdsj.libBiz.strBiz;

using JiebaNet.Segmenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using static prjx.lib.strCls;
namespace mdsj.libBiz
{
    internal class strBiz
    {

        public static int containCalcCntScoreSetfmt(string segments, HashSet<string> set)
        {
            if (segments == null)
                return 0;
            // print(" containCalcCntScoreSetfmt() "+string.Join(' ', segments));
            //  print();
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
                   Print(" containChk2024. kwd=>" + kwd2);
                }


            }
            return n;
        }

        public static int containCalcCntScoreSetfmt(HashSet<string> set, string[] segments)
        {
            // print(" containCalcCntScoreSetfmt() "+string.Join(' ', segments));
            //  print();
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
                       Print(" contain. kwd=>" + kwd2);
                    }

                }
            }
            return n;
        }
        public static string[] removeStopWd4biz(string[] kwds)
        {
            //todo 去除触发词，，只保留 服务次和位置词
            HashSet<string> 搜索触发词 = GetSrchTrgWds();

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

        public static HashSet<string> GetSrchTrgWds()
        {
            return ReadWordsFromFile($"{prjdir}/cfg/搜索触发词.txt");
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
            segmenter.LoadUserDict(userDictFile);
            segmenter.AddWord("会所"); // 可添加一个新词
            segmenter.AddWord("妙瓦底"); // 可添加一个新词
            segmenter.AddWord("御龙湾"); // 可添加一个新词
            HashSet<string> user_dict = GetUser_dict();
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
           print("【搜索引擎模式】：{0}", kdwsJoin);
          //  System.Console.WriteLine("【搜索引擎模式】：{0}", kdwsJoin);
            return kwds;
        }

        

        public static HashSet<string> GetUser_dict()
        {
            return ReadLinesToHashSet($"{prjdir}/cfg/user_dict.txt");
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
