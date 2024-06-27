global using static mdsj.libBiz.filBiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdsj.libBiz
{
    internal class filBiz
    {

        public static HashSet<string> file_getWords商品与服务词库()
        {
            HashSet<string> 商品与服务词库 = ReadWordsFromFile("商品与服务词库.txt");
            商品与服务词库.Remove("店");


            return ConvertToUpperCase(商品与服务词库);
        }
    }
}
