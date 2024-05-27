using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 缅甸商家.lib
{
    internal class strCls
    {
        internal static bool StartsWith(string? text, string v)
        {
             if(text == null) return false;

            if (text.StartsWith(v))
            {
                return true;
            }
            return false;
        }
    }
}
