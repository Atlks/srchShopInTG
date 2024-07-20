using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace prjx.lib
{
    internal class ormTest
    {
        public static void testorm()
        {
            const string DbFileName = "objs2005.db";
            System.Collections.SortedList chtsSesss = new System.Collections.SortedList();
            chtsSesss.Add("id", 1); chtsSesss.Add("nm", "....");
            ormSqlt.save(  chtsSesss, DbFileName);

            System.Collections.SortedList chtsSesss2 = new System.Collections.SortedList();
            chtsSesss2.Add("id", 2); chtsSesss2.Add("nm", "nm222");

            ormSqlt.save(  chtsSesss2, DbFileName);

            var rs = ormSqlt.qryDep( DbFileName);
        }
    }
}
