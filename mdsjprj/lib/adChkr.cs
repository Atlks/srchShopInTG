using prjx.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using City = prjx.City;
using static prjx.lib.arrCls;//  prj202405.lib
using static prjx.lib.dbgCls;
using static prjx.lib.arrCls;//  prj202405.lib
using static prjx.lib.dbgCls;
using static mdsj.lib.logCls;
using static prjx.lib.corex;
using static prjx.lib.db;
using static prjx.lib.filex;
using static prjx.lib.ormJSonFL;
using static prjx.lib.strCls;
using static mdsj.lib.bscEncdCls;
using static mdsj.lib.net_http;
using static mdsj.libBiz.strBiz;
using static mdsj.libBiz.tgBiz;
using static prjx.lib.strCls;
using static prjx.lib.corex;
using static prjx.lib.db;
using static prjx.lib.filex;
using static prjx.lib.ormJSonFL;
using static prjx.lib.strCls;
using static mdsj.lib.bscEncdCls;
using static mdsj.lib.net_http;

using static mdsj.libBiz.tgBiz;
using static prjx.lib.tglib;
using static mdsj.lib.adChkr;
using System.Reflection;
namespace mdsj.lib
{
    internal class adChkr
    {

        public static void logic_chkad(string text, string uid, long grpid, Action act)
        {
            var __METHOD__ = "logic_chkad";
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(  text, uid, grpid));

            try
            {
                string prjdir = @"../../../";
                prjdir = filex.GetAbsolutePath(prjdir);
                string adwdlib = $"{prjdir}/gbwd垃圾关键词词库/ads_word.txt";
                HashSet<string> adwds = SplitFileByChrs(adwdlib, ",\r \n");
                int ctnScr = containCalcCntScoreSetfmt(text, adwds);

               Print("广告词包含分数=》" + ctnScr);


                if (text.Length < 10)
                    return;
                string timestampMM = DateTime.Now.ToString("MM");
                string fnameFrmTxt = ConvertToValidFileName(text);
               Print("fnameFrmTxt=>" + fnameFrmTxt);
                string fname = $"adchkDir/uid{uid}_grp{grpid}_Dt{timestampMM}___" + Sub1109 (fnameFrmTxt,0, 50) + ".txt";
                if (System.IO.File.Exists(fname))
                {
                   Print("是重复消息了" + fname);
                    file_put_contents(fname, "\n\n" + text + "", true);


                    act();


                }
                else
                    file_put_contents(fname, "\n\n" + text, true);
            }
            catch(Exception e)
            {
               Print("catch in ()=>" + __METHOD__ + "()");
               Print(e);
            }
          

            dbgCls.PrintRet(__METHOD__, 0);
        }
    }
}
