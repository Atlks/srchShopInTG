using static prjx.lib.corex;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
//   prj202405.lib.strCls
namespace prjx.lib
{
    internal class strCls
    {

        public static string ExtractTextAfterMarker(string input, string marker)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(marker))
            {
                throw new ArgumentException("Input and marker cannot be null or empty.");
            }

            int markerIndex = input.IndexOf(marker);
            if (markerIndex == -1)
            {
                return string.Empty; // Marker not found, return empty string
            }

            int startIndex = markerIndex + marker.Length;
            if (startIndex >= input.Length)
            {
                return string.Empty; // No text after marker
            }

            return input.Substring(startIndex);
        }

        /// <summary>
        ///  // 示例用法
      //  string result = sprintf("Hello, {0}! You have {1} new messages.", "John", 5);

        /// 类似于 PHP 中的 sprintf 函数，使用格式字符串和参数数组构建格式化字符串。
        /// </summary>
        /// <param name="format">格式字符串</param>
        /// <param name="args">参数数组</param>
        /// <returns>格式化后的字符串</returns>
        public static string sprintf(string format, params object[] args)
        {
            return string.Format(format, args);
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
        internal static bool contain(string caption, string v)
        {
            if (caption == null)
                return false;
            return caption.Contains(v);
        }

     public   static string trim_RemoveUnnecessaryCharacters(string input)
        {
            // Define the characters to be removed
            char[] charsToRemove = new char[] { '\"', '[', ']' };

            // Create a new array to hold the filtered characters
            char[] result = new char[input.Length];
            int resultIndex = 0;

            // Iterate over the input string and copy only the characters that are not in charsToRemove
            foreach (char c in input)
            {
                bool shouldRemove = false;
                foreach (char remove in charsToRemove)
                {
                    if (c == remove)
                    {
                        shouldRemove = true;
                        break;
                    }
                }

                if (!shouldRemove)
                {
                    result[resultIndex++] = c;
                }
            }

            // Return the new string without the unnecessary characters
            string v = new string(result, 0, resultIndex);
            return v.Trim();
        }

        internal static bool containKwds(string text, string trgSearchKwds)
        {
            if (text == null)
                return false;
            string[] kwds = trgSearchKwds.Split(new char[] { ' ' });
            foreach (string kwd in kwds)
            {
                var kwd2 = kwd.Trim();
                if (kwd2.Length > 0)
                {
                    if (text.Contains(kwd2))
                    {
                       print(" str.containKwds() kwd=>" + kwd2);
                        return true;
                    }
                       
                }
            }



            return false;
        }

        //pai除 城市词
        //触发词  城市词  商品词
        internal static int containCalcCntScore(string seasrchKw2ds, IEnumerable<string> segments)
        {
            int n = 0;
            foreach (string kwd in segments)
            {
                var kwd2 = kwd.Trim();
                if (kwd2.Length > 0)
                {
                    if (strBiz_isPostnWord(kwd2))
                        continue;
                    if (seasrchKw2ds.Contains(kwd2))
                    {
                        n++;
                       print(" contain kwd=>" + kwd2);
                    }
                      
                }
            }
            return n;
        }

        private static bool strBiz_isPostnWord(string kwd2)
        {
            HashSet<string> citys = ReadLinesToHashSet("位置词.txt");
            return citys.Contains(kwd2);
        }


        /// <summary>
        /// Reads each line from the specified file and adds them to a HashSet.
        /// </summary>
        /// <param name="filePath">The path to the file to read.</param>
        /// <returns>A HashSet containing the unique lines from the file.</returns>
        public static HashSet<string> ReadLinesToHashSet(string filePath)
        {
            HashSet<string> linesHashSet = new HashSet<string>();

            try
            {
                // Read each line from the file and add it to the HashSet
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        linesHashSet.Add(line);
                    }
                }
            }
            catch (Exception ex)
            {
               print($"An error occurred while reading the file: {ex.Message}");
            }

            return linesHashSet;
        }

        internal static bool eq(object v, string cityName4srch)
        {
            if (v == null) return false;
            return v.Equals(cityName4srch);
        }


        //internal static bool eqV2(object rowVal, Dictionary<string, Microsoft.Extensions.Primitives.StringValues> whereExprsObj, string cityName4srchxx)
        //{

        //    string cityName4srch = arrCls.TryGetValue(whereExprsObj, cityName4srchxx); ;
        //    if (cityName4srch == null)  //if not have this clm in where exprs
        //        return false;
        //    else if (cityName4srch != null)
        //    {
        //        if (rowVal == null)
        //            return false;
        //        return rowVal.Equals(cityName4srch);
        //    }
        //    return false;

        //}

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
        internal static bool StartsWith(string text, string v)
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
