using prj202405.lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//using static mdsj.other;
//using static mdsj.clrCls;
using static mdsj.lib.exCls;
using static prj202405.lib.arrCls;//  prj202405.lib
using static prj202405.lib.dbgCls;
using static mdsj.lib.logCls;
using static prj202405.lib.corex;
using static prj202405.lib.db;
using static prj202405.lib.filex;
using static prj202405.lib.ormJSonFL;
using static prj202405.lib.strCls;
using static mdsj.lib.encdCls;
using static mdsj.lib.net_http;

namespace WindowsFormsApp1
{
    [ComVisible(true)]
    public class ScriptManager
    {
        private Form1 form;

        public ScriptManager(Form1 form)
        {
            this.form = form;
        }

        public string list()
        {
            List<SortedList> li= ormSqlt.qryV2("D:\\0prj\\mdsj\\mdsjprj\\bin\\Debug\\net8.0\\mercht商家数据\\缅甸.db");
           return  json_encode(li);
        }

        public void save()
        {
            MessageBox.Show(" function called from HTML frm scrptmng!");
        }


        public void m1()
        {
            MessageBox.Show(" function called from HTML frm scrptmng!");
        }
    }
}
