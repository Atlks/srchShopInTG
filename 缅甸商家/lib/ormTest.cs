using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace prj202405.lib
{
    internal class ormTest
    {
        public static void testorm()
        {
            const string DbFileName = "objs2005.db";
            System.Collections.Hashtable chtsSesss = new System.Collections.Hashtable();
            chtsSesss.Add("id", 1); chtsSesss.Add("nm", "....");
            ormSqlt.save(  chtsSesss, DbFileName);

            System.Collections.Hashtable chtsSesss2 = new System.Collections.Hashtable();
            chtsSesss2.Add("id", 2); chtsSesss2.Add("nm", "nm222");

            ormSqlt.save(  chtsSesss2, DbFileName);

            var rs = ormSqlt.qry( DbFileName);
        }
    }
}
