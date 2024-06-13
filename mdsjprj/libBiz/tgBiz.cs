using prj202405.lib;
using prj202405;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static mdsj.libBiz.tgBiz;
namespace mdsj.libBiz
{
    internal class tgBiz
    {


        public static bool tg_isBtm_btnClink_in_pubGrp(Update update)
        {
            if(  update.Type != UpdateType.Message)
            {
                return false;
            }
            string msgx2024 = tglib.bot_getTxt(update);

            if (update?.Message?.Chat?.Type != ChatType.Private)
            {
                ArrayList a = filex.rdWdsFromFile("menu/底部公共菜单.txt");
                return a.Contains(msgx2024);
            }
            return false;

        }
        //if  is nml msg ,not search
        public static bool bot_isNnmlMsgInGrp(Update? update)
        {
            if (update?.Message == null)  //maybe cmd call
            {
                return false;
            }

            if (update?.Message?.Chat?.Type == ChatType.Private)
            {
                return false;
            }

            if (update?.Message?.Text == null || update?.Message?.Text.Trim() == "")
                return false;


            //--------------here myst msg in grp mode 




            //if rply n frmuser is bot n textContain(我是便民助手
            if (update?.Message?.ReplyToMessage != null
                && update.Message.ReplyToMessage.From.Username ==Program. botname
                && strCls.StartsWith(update.Message?.ReplyToMessage?.Text, "我是便民助手")
                )
            {
                return false;  // not nml msg ,start search;
            }

            //pingjia 内容，不要进行反馈搜索
            if (update?.Message?.ReplyToMessage != null &&
                strCls.contain(update?.Message?.ReplyToMessage?.Caption, "---联系方式---"))
            {
                //is nml msg ,not need search kwd  ,,for 评价
                return true;
            }


            //grp spec kwd... btm btn seqrch wds

            ArrayList lst = testCls.kwdSeasrchInGrp("kwdSearchINGrp.txt");
            if (lst.Contains(update?.Message?.Text))
            {
                return false;
            }



            if (update?.Message?.ReplyToMessage?.From?.Username == botname
                && update?.Message?.ReplyToMessage?.Caption == "??博彩盘推荐：世博联盟")
            {
                return false;
            }


            if ( isGrpChat (update?.Message?.Chat?.Type) )// if grp in 
            {  

                if (update?.Message == null)
                    return false;
                if (update?.Message?.Text == null)
                    return false;

                if ((bool)update?.Message?.Text.StartsWith("@" + botname))
                    return false;

                // 
                Console.WriteLine("搜索触发词 in isNumlMsgInGrp()");
                var trgSearchKwds = " ";
                var trgWd = biz_other.getTrgwdHash("搜索触发词.txt");
                trgSearchKwds = trgSearchKwds + trgWd;
                if (strCls.containKwds(update?.Message?.Text, trgSearchKwds))
                {
                    //if  is nml msg ,not search
                    return false;   //not nml msg,need search
                }

                if (update?.Message?.ReplyToMessage?.From?.Username == botname
                    && update?.Message?.ReplyToMessage?.Caption == "??博彩盘推荐：世博联盟")
                {
                    return false;
                }

                ArrayList lst2 = testCls.kwdSeasrchInGrp("kwdSearchINGrp.txt");
                if (lst2.Contains(update?.Message?.Text))
                {
                    return false;
                }

                Console.WriteLine("nml msg");
                return true;
            }
            else  //prvt mode  ,,,not nml msg
                return false;

        }

        public static bool isGrpChat(ChatType? type)
        {
            if (type == ChatType.Private)
                return false;
            if (type == ChatType.Group || type == ChatType.Supergroup || type == ChatType.Channel)
                return true;
            return false;
        }

        public const string botname =Program.botname;

        public static ReplyKeyboardMarkup tg_btmBtns()
        {
            var Keyboard =
                new KeyboardButton[][]
                {
                            new KeyboardButton[]
                            {
                                new KeyboardButton("💸💸💸 世博博彩 💸💸💸")
                            },
                            new KeyboardButton[]
                            {
                                new KeyboardButton("商家"),
                                       new KeyboardButton("猎艳"),
                                              new KeyboardButton("猎奇"),
                                                new KeyboardButton("买号")

                            },

                            new KeyboardButton[]
                            {

                                 new KeyboardButton("接码")
                                ,   new KeyboardButton("闲置")

                                 ,   new KeyboardButton("资源"),   new KeyboardButton("招聘")


                                // new KeyboardButton("话术") , new KeyboardButton("搜群"),

                                //   new KeyboardButton("卖号") ,   new KeyboardButton("工作")
                                //,   new KeyboardButton("代理")

                                 
                                   
                            },
                              new KeyboardButton[]
                            {
                                 new KeyboardButton("跑腿") ,
                                   new KeyboardButton("代购") ,
                                    new KeyboardButton("优品") ,
                                     new KeyboardButton("兑换")
                            } ,

                               new KeyboardButton[]
                            {
                                //new KeyboardButton("兑换"),
                                 new KeyboardButton("洗资"),
                                new KeyboardButton("担保"),
                                new KeyboardButton("租房"),

                                new KeyboardButton("行程"),
                            }
                            ,

                            new KeyboardButton[]
                            {

                                 new KeyboardButton("搜群"),

                                  new KeyboardButton("文案"),
                                   new KeyboardButton("话术"),
                                new KeyboardButton("办证")


                            }





                };
            var rkm = new ReplyKeyboardMarkup(Keyboard);
            return rkm;
        }


    }
}
