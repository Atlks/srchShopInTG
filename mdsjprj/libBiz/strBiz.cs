using JiebaNet.Segmenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static mdsj.libBiz.strBiz;

using static prj202405.lib.strCls;
namespace mdsj.libBiz
{
    internal class strBiz
    {
        public static string[] calcKwdsAsArr(ref string msgx)
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
        public static string getFuwuci(string? text, HashSet<string> 商品与服务词库)
        {
            if (text == null)
                return null;
            string[] spltWds = calcKwdsAsArr(ref text);
            foreach (string wd in spltWds)
            {
                if (商品与服务词库.Contains(wd))
                    return wd;
            }
            return null;
        }
    }
}
