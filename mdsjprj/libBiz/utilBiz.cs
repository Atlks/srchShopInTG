global using static mdsj.libBiz.utilBiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace mdsj.libBiz
{
    internal class utilBiz
    {
        public static bool IsSetAreaBtnname(string txt307)
        {
            return txt307.StartsWith(PreCh);
        }
    }
}
