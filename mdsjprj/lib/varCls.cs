﻿global using static mdsj.lib.varCls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 

namespace mdsj.lib
{
    internal class varCls
    {
        public static string gettype(object obj)
        {
            if (obj == null)
            {
                return "null";
            }

            return obj.GetType().Name;
        }

        public static void varCastType(object obj,string t1,string t2)
        {
            
        }
    }
}
