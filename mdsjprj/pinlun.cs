using prj202405.lib;
using prj202405;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prj202405
{
    internal class pinlun
    {
        public static string pinlun_getpinlun(Merchant? contact_Merchant, string result)
        {
            result += "\n\n<b>------------客户点评------------</b>";
            if (contact_Merchant.Comments.Count == 0)
            {
                result += "\n\n<b>还无人点评,@回复本消息,即可对商家点评!(100字以内)</b>";
                result += "\n\n" + timerCls.plchdTxt;
                return result;
            }

            System.IO.Directory.CreateDirectory("pinlunDir");
            //  ormSqlt.save(obj1, "pinlunDir/" + merchant.Guid + merchant.Name + ".db");
            ArrayList rows = ormJSonFL.qry("pinlunDir/" + contact_Merchant.Guid + contact_Merchant.Name + ".json");
            for (int i = 0; i < rows.Count; i++)
            {
                try
                {
                    var uid = contact_Merchant.Comments.ElementAt(i).Key;
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
                    Console.WriteLine(e.Message);
                }
            }



            return result;
        }

    }
}
