global using static libx.funCls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static mdsj.lib.exCls;
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
using static prjx.lib.corex;
using static libx.storeEngr4Nodesqlt;
using prjx.lib;
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
               Print($"Type '{typeName}' not found.");
                return false;
            }

            MethodInfo methodInfo = type.GetMethod(methodName);
            if (methodInfo == null)
            {
               Print($"Method '{methodName}' not found in type '{typeName}'.");
                return false;
            }

            return true;
        }
    }
}
