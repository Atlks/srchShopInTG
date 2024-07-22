using prjx.lib;
using prjx;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace prjx
{
    internal class pinlun
    {
        public static string pinlun_getpinlun(Merchant? contact_Merchant)
        {
            var __METHOD__ = MethodBase.GetCurrentMethod().Name;
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), contact_Merchant));

            string result = "";
            //  ormJSonFL.save(obj1, "pinlunDir/" + merchant.Guid + merchant.Name + ".json");
            List<SortedList> rowsx = ormJSonFL.qry("pinlunDir/" + contact_Merchant.Guid + contact_Merchant.Name + ".json");
            if (rowsx.Count == 0)
            {
              //  result += "\n\n<b>------------客户点评------------</b>";
                // result += "\n\n<b>还无人点评 " ；
               // result += "\n\n@回复本消息,即可对商家点评!(100字以内)";
             
                return result;
            }

            System.IO.Directory.CreateDirectory("pinlunDir");
            //  ormSqlt.save(obj1, "pinlunDir/" + merchant.Guid + merchant.Name + ".db");
            List<SortedList> rows = ormJSonFL.qry("pinlunDir/" + contact_Merchant.Guid + contact_Merchant.Name + ".json");
            for (int i = 0; i < rows.Count; i++)
            {
                SortedList rw = rows[i];
                try
                {
                    if (LoadFieldTryGetValueAsStrDefNull(rw, "评论人id") == null)
                    {
                        continue;
                    }
                    
                    var uid =(long) rw["评论人id"];
                        //contact_Merchant.Comments.ElementAt(i).Key;
                    #region start 
                    var star = "★ ★ ★ ★ ★ \n\n🥰";
                    if (contact_Merchant.Scores.ContainsKey(uid))
                    {
                        switch (contact_Merchant.Scores[uid])
                        {
                            case 1:
                                star = "★ ☆ ☆ ☆ ☆ \n\n🤯";
                                break;
                            case 2:
                                star = "★ ★ ☆ ☆ ☆ \n\n😤";
                                break;
                            case 3:
                                star = "★ ★ ★ ☆ ☆ \n\n😟";
                                break;
                            case 4:
                                star = "★ ★ ★ ★ ☆ \n\n😁";
                                break;
                            case 5:
                                star = "★ ★ ★ ★ ★ \n\n🥰";
                                break;
                        }
                    }
                    #endregion
                    var comment = ((SortedList)rows[i])["评论内容"];
                    var commentStr = $"\n\n💬 匿名用户{i + 1}            {star} <b>{comment}</b>";
                    if ((result + commentStr).Length >= 4000)
                        break;
                    result += commentStr;
                }
                catch (Exception e)
                {
                   Print(e.Message);
                }
            }


            dbgCls.PrintRet( __METHOD__, result);
            return result;
        }

    }
}
