﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static libx.funCls;
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
using static prj202405.lib.corex;
using static libx.storeEngr4Nodesqlt;
using prj202405.lib;
using System.Reflection;
namespace libx
{
    internal class funCls
    {

        public static string call_exe_retStr(string exePath,string scriptPath, SortedList prm)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), scriptPath, prm));

            string timestamp2 = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            Directory.CreateDirectory("prmDir");
            File.WriteAllText($"prmDir/prm{timestamp2}.txt", json_encode(prm));
            string prm_fileAbs = GetAbsolutePath($"prmDir/prm{timestamp2}.txt");

            
            string str = call_exe_Pstr(exePath,scriptPath, prm_fileAbs);

            dbgCls.setDbgVal(__METHOD__, "callNodePstr.ret", str);
            string marker = "----------marker----------";
            str = substr_AfterMarker(str, marker);
            str = str.Trim();
            dbgCls.setDbgValRtval(__METHOD__, str);
            return str;
        }


        public static string call_exec_RetList(string execpath,string scriptPath, SortedList prm,string outputDir)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.setDbgFunEnter(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), scriptPath, prm));

            string timestamp2 = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            Directory.CreateDirectory("prmDir");
            File.WriteAllText($"prmDir/prm{timestamp2}.txt", json_encode(prm));

            string prm_fileAbs = GetAbsolutePath($"prmDir/prm{timestamp2}.txt");


            string str = call_exe_Pstr(execpath,scriptPath, prm_fileAbs);
            string marker = "----------qryrzt----------";
        string    strAft = substr_AfterMarker(str, marker);
            strAft = strAft.Trim();
            string prjDir = @"../../";
            string txt = File.ReadAllText(outputDir +"/"+ strAft);
            dbgCls.setDbgValRtval(__METHOD__, txt);
            return txt;
        }

        internal static string callPhp(string scriptPath, SortedList prm)
        {
            throw new NotImplementedException();
        }
    }
}
