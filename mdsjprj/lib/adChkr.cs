using prj202405.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using City = prj202405.City;
using static prj202405.lib.arrCls;//  prj202405.lib
using static prj202405.lib.dbgCls;
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
using static mdsj.libBiz.strBiz;
using static mdsj.libBiz.tgBiz;
using static prj202405.lib.strCls;
using static prj202405.lib.corex;
using static prj202405.lib.db;
using static prj202405.lib.filex;
using static prj202405.lib.ormJSonFL;
using static prj202405.lib.strCls;
using static mdsj.lib.encdCls;
using static mdsj.lib.net_http;

using static mdsj.libBiz.tgBiz;
using static prj202405.lib.tglib;
using static mdsj.lib.adChkr;
using System.Reflection;
namespace mdsj.lib
{
    internal class adChkr
    {

        public static async Task logic_chkad(string text, string uid, long grpid, Action act)
        {
            var __METHOD__ = "logic_chkad";
            dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(  text, uid, grpid));

            try
            {
                string prjdir = @"../../../";
                prjdir = filex.GetAbsolutePath(prjdir);
                string adwdlib = $"{prjdir}/gbwd垃圾关键词词库/ads_word.txt";
                HashSet<string> adwds = splitFileByChrs(adwdlib, ",\r \n");
                int ctnScr = containCalcCntScoreSetfmt(text, adwds);

                Console.WriteLine("广告词包含分数=》" + ctnScr);


                if (text.Length < 10)
                    return;
                string timestampMM = DateTime.Now.ToString("MM");
                string fnameFrmTxt = ConvertToValidFileName(text);
                Console.WriteLine("fnameFrmTxt=>" + fnameFrmTxt);
                string fname = $"adchkDir/uid{uid}_grp{grpid}_Dt{timestampMM}___" + fnameFrmTxt.Substring(0, 50) + ".txt";
                if (System.IO.File.Exists(fname))
                {
                    Console.WriteLine("是重复消息了" + fname);
                    file_put_contents(fname, "\n\n" + text + "", true);


                    act();


                }
                else
                    file_put_contents(fname, "\n\n" + text, true);
            }
            catch(Exception e)
            {
                Console.WriteLine("catch in ()=>" + __METHOD__ + "()");
                Console.WriteLine(e);
            }
          

            dbgCls.setDbgValRtval(__METHOD__, 0);
        }
    }
}
