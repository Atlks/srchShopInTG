using JiebaNet.Segmenter;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prj202405.lib
{
    internal class strCls
    {

        public static string[] calcKwdsAsArr(ref string msgx)
        {
            msgx = ChineseCharacterConvert.Convert.ToSimple(msgx);
            var segmenter = new JiebaSegmenter();
            segmenter.LoadUserDict("user_dict.txt");
            segmenter.AddWord("会所"); // 可添加一个新词
            segmenter.AddWord("妙瓦底"); // 可添加一个新词
            IEnumerable<string> enumerable = segmenter.CutForSearch(msgx);
            // 使用 LINQ 的 ToArray 方法进行转换
            string[] kwds = enumerable.ToArray();
          //  string[] kwds = enumerable; // 搜索引擎模式
            string kdwsJoin = string.Join("/", kwds);
            Console.WriteLine("【搜索引擎模式】：{0}", kdwsJoin);
            return kwds;
        }
        public static string calcKwds(ref string msgx)
        {
            msgx = ChineseCharacterConvert.Convert.ToSimple(msgx);
            var segmenter = new JiebaSegmenter();
            segmenter.LoadUserDict("user_dict.txt");
            segmenter.AddWord("会所"); // 可添加一个新词
            segmenter.AddWord("妙瓦底"); // 可添加一个新词
            var kwds = segmenter.CutForSearch(msgx); // 搜索引擎模式
            string kdwsJoin = string.Join("/", kwds);
            Console.WriteLine("【搜索引擎模式】：{0}", kdwsJoin);
            return kdwsJoin;
        }

        /// <summary> 
        /// 繁体转换为简体
        /// </summary> 
        /// <param name="str">繁体字</param> 
        /// <returns>简体字</returns> 
        //public static string GetSimplified(string str)
        //{
        //    return ChineseConverter.Convert(str, ChineseConversionDirection.TraditionalToSimplified);
        //}

        public static string ConvertToSimplifiedChinese(string traditionalChinese)
        {
            Dictionary<char, char> traditionalToSimplifiedMap = new Dictionary<char, char>
        {
            {'繁', '繁'},
            {'體', '体'},
            {'中', '中'},
            {'文', '文'}
            // 这里需要添加所有的繁体到简体的映射
        };

            char[] simplifiedChars = new char[traditionalChinese.Length];
            for (int i = 0; i < traditionalChinese.Length; i++)
            {
                char ch = traditionalChinese[i];
                simplifiedChars[i] = traditionalToSimplifiedMap.ContainsKey(ch) ? traditionalToSimplifiedMap[ch] : ch;
            }

            return new string(simplifiedChars);
        }


        //public static string ConvertToSimplifiedChinese(string traditionalChinese)
        //{
        //    // 使用 Microsoft.VisualBasic 中的 StrConv 方法进行转换
        //    string simplifiedChinese = Strings.StrConv(traditionalChinese, VbStrConv.SimplifiedChinese);
        //    return simplifiedChinese;
        //}
        internal static bool contain(string? caption, string v)
        {
            if (caption == null)
                return false;
            return caption.Contains(v);
        }

        internal static bool containKwds(string? text, string trgSearchKwds)
        {
            string[] kwds = trgSearchKwds.Split(" ");
            foreach (string kwd in kwds)
            {
                var kwd2 = kwd.Trim();
                if (kwd2.Length > 0)
                {
                    if (text.Contains(kwd2))
                    {
                        Console.WriteLine(" str.containKwds() kwd=>" + kwd2);
                        return true;
                    }
                       
                }
            }



            return false;
        }

        internal static int containCalcCntScore(string seasrchKw2ds, IEnumerable<string> segments)
        {
            int n = 0;
            foreach (string kwd in segments)
            {
                var kwd2 = kwd.Trim();
                if (kwd2.Length > 0)
                {
                    if (seasrchKw2ds.Contains(kwd2))
                    {
                        n++;
                        Console.WriteLine(" contain kwd=>" + kwd2);
                    }
                      
                }
            }
            return n;
        }

        internal static bool eq(object? v, string cityName4srch)
        {
            if (v == null) return false;
            return v.Equals(cityName4srch);
        }


        internal static bool eqV2(object? rowVal, Dictionary<string, Microsoft.Extensions.Primitives.StringValues> whereExprsObj, string cityName4srchxx)
        {

            string cityName4srch = arrCls.TryGetValue(whereExprsObj, cityName4srchxx); ;
            if (cityName4srch == null)  //if not have this clm in where exprs
                return false;
            else if (cityName4srch != null)
            {
                if (rowVal == null)
                    return false;
                return rowVal.Equals(cityName4srch);
            }
            return false;

        }

        internal static string JoinHashtbKV(string v, ICollection keys)
        {
            // 获取Hashtable的所有键
            //     ICollection keys = hashtable.Keys;

            // 将键转换为字符串数组
            string[] keyArray = new string[keys.Count];
            keys.CopyTo(keyArray, 0);

            // 将字符串数组中的所有键连接成一个单一的字符串
            string allKeys = string.Join(", ", keyArray);
            return allKeys;
        }

        // ToLower
        internal static bool StartsWith(string? text, string v)
        {
            if (text == null) return false;

            if (text.StartsWith(v))
            {
                return true;
            }
            return false;
        }
    }
}
