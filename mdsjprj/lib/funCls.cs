global using static libx.funCls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public bool MethodExists(string typeName, string methodName)
        {
            Type type = Type.GetType(typeName);
            if (type == null)
            {
                Console.WriteLine($"Type '{typeName}' not found.");
                return false;
            }

            MethodInfo methodInfo = type.GetMethod(methodName);
            if (methodInfo == null)
            {
                Console.WriteLine($"Method '{methodName}' not found in type '{typeName}'.");
                return false;
            }

            return true;
        }
    }
}
