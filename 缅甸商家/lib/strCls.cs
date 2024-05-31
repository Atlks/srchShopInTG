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
        internal static bool contain(string? caption, string v)
        {
          if(caption==null)
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
                        return true;
                }
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
