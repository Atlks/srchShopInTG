global using static mdsj.libBiz.tgBiz;
using prjx.lib;
using prjx;
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
using static prjx.timerCls;
using static mdsj.biz_other;
using static mdsj.clrCls;
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
using static mdsj.lib.util;
using static mdsj.libBiz.tgBiz;
using Telegram.Bot;
using System.Reflection;
using Newtonsoft.Json;
using static mdsj.libBiz.strBiz;
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

using static prjx.lib.strCls;
using static mdsj.lib.adChkr;
namespace mdsj.libBiz
{
    internal class tgBiz
    {
        public static void bot_adChk(Update update)
        {
            try
            {
                if (update?.Type is UpdateType.Message)
                {


                    if (update.Message.Text.Length < 10)
                        return;

                    //  string timecode=
                    string text = update.Message.Text;

                    string uid = update.Message.From.Id.ToString();
                    var grpid = update.Message.Chat.Id;

                    Action act = () =>
                    {

                        SortedList obj = new SortedList();
                        obj.Add("id", uid);
                        obj.Add("user", update.Message.From);
                        ormJSonFL.SaveJson(obj, "aduser.json");
                       Print("可能广告");
                        //  tglib.bot_dltMsgThenSendmsg(update.Message!.Chat.Id, update.Message.MessageId, "检测到此消息为重复性消息,本消息10秒后删除!", 10);

                    };
                    logic_chkad(text, uid, grpid, act);

                    //机器人检测
                    if (update.Message.From.IsBot)
                    {
                        SortedList obj = new SortedList();
                        obj.Add("id", uid);
                        obj.Add("user", update.Message.From);
                        ormJSonFL.SaveJson(obj, "aduser.json");
                    }

                    //广告号检测


                }
            }
            catch (Exception ex)
            {
               Print(ex);
            }


        }

        public static async Task 添加商家信息(ITelegramBotClient botClient, Update update, string? text)
        {
            var callError = async (string text) =>
            {
                try
                {
                    await botClient.SendTextMessageAsync(chatId: update.Message.Chat.Id, text: text, messageThreadId: update.Message.MessageThreadId, replyToMessageId: update.Message.MessageId);
                }
                catch (Exception ex)
                {
                   Print("告知新增联系方式时获取到时出错:" + ex.Message);
                }
            };
            var merchant = new Merchant();
            merchant.Guid = Guid.NewGuid().ToString();

            var chengshiandyuanqu = GetText.GetBetween(text, "城市园区名字:", "\n");
            if (string.IsNullOrEmpty(chengshiandyuanqu))
            {
                await callError("在添加商家联系方式时,城市/园区名字未获取到");
                return;
            }


            var _citys = getCitysObj();
            //园区城市
            Address? address = null;
            foreach (var c in _citys)
            {
                foreach (var a in c.Address)
                {
                    if (a.Name == chengshiandyuanqu)
                    {
                        address = a;
                        break;
                    }
                }
            }
            if (address == null)
            {
                await callError("城市园区不存在");
                return;
            }

            merchant.Name = GetText.GetBetween(text, "商家名称:", "\n");
            if (string.IsNullOrEmpty(merchant.Name))
            {
                await callError("商家名称未获取到");
                return;
            }

            var category = GetText.GetBetween(text, "商家分类:", "\n");
            try
            {
                merchant.Category = (Category)Convert.ToInt32(category);
            }
            catch (Exception)
            {
                await callError("商家分类未获取到");
                return;
            }

            merchant.KeywordString = GetText.GetBetween(text, "商家关键词:", "\n");
            if (string.IsNullOrEmpty(merchant.KeywordString))
            {
                await callError("商家关键词未获取到");
                return;
            }

            var start = GetText.GetBetween(text, "开始营业时间:", "\n");
            try
            {
                merchant.StartTime = TimeSpan.Parse(start);
            }
            catch (Exception)
            {
                await callError("商家开始营业时间未获取到");
                return;
            }

            var end = GetText.GetBetween(text, "打烊收摊时间:", "\n");
            try
            {
                merchant.StartTime = TimeSpan.Parse(end);
            }
            catch (Exception)
            {
                await callError("商家打烊时间未获取到");
                return;
            }

            var telegram = GetText.GetBetween(text, "Telegram:", "\n");
            if (!string.IsNullOrEmpty(telegram))
            {
                merchant.Telegram = telegram.Split(' ').ToList();
            }

            var telegramGroup = GetText.GetBetween(text, "Telegram群组:", "\n");
            if (!string.IsNullOrEmpty(telegramGroup))
            {
                merchant.TelegramGroup = telegramGroup;
            }

            var whatsapp = GetText.GetBetween(text, "Whatsapp:", "\n");
            if (!string.IsNullOrEmpty(whatsapp))
            {
                merchant.WhatsApp = whatsapp.Split(' ').ToList();
            }

            var lines = GetText.GetBetween(text, "Line:", "\n");
            if (!string.IsNullOrEmpty(lines))
            {
                merchant.Line = lines.Split(' ').ToList();
            }

            var signals = GetText.GetBetween(text, "Signal:", "\n");
            if (!string.IsNullOrEmpty(signals))
            {
                merchant.Signal = signals.Split(' ').ToList();
            }

            var weixins = GetText.GetBetween(text, "微信:", "\n");
            if (!string.IsNullOrEmpty(weixins))
            {
                merchant.WeiXin = weixins.Split(' ').ToList();
            }

            var tels = GetText.GetBetween(text, "电话:", "\n");
            if (!string.IsNullOrEmpty(tels))
            {
                merchant.Tel = tels.Split(' ').ToList();
            }

            if (merchant.Telegram.Count == 0 && merchant.WhatsApp.Count == 0 && merchant.Line.Count == 0 && merchant.Signal.Count == 0 && merchant.WeiXin.Count == 0)
            {
                await callError("未获取到任何一个联系方式");
                return;
            }

            merchant.Menu = GetText.GetBetween(text, "商家菜单:", "\n");
            address.Merchant.Add(merchant);
            await biz_other._SaveConfig();
            try
            {
                  tglib.bot_dltMsgThenSendmsg(update.Message.Chat.Id, update.Message.MessageId, "商家添加成功", 5);
            }
            catch (Exception ex)
            {
               Print("告知商家添加成功时出错:" + ex.Message);
            }
        }

        public static async Task 获取机器人的信息()
        {
            try
            {
                // 获取机器人的信息
                Telegram.Bot.Types.User me = await botClient.GetMeAsync();
               Print($"Bot ID: {me.Id}");
               Print($"Bot Name: {me.FirstName}");
               Print($"Bot Username: {me.Username}");
            }
            catch (Exception ex)
            {
               Print($"An error occurred: {ex.Message}");
            }
        }

        public static void bot_logRcvMsgV2(Update update, string dir)
        {
            try
            {
                var updateString = JsonConvert.SerializeObject(update, Formatting.Indented);
              //  const string dir = "msgRcvDir";
                Directory.CreateDirectory(dir);
               Print("fun bot_logRcvMsgV2（）");
               Print(updateString);
                // 获取当前时间并格式化为文件名
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                string fileName = $"{dir}/{timestamp}.json";
               Print(fileName);
                filex.Mkdir4File(fileName);
                System.IO.File.WriteAllText("" + fileName, updateString);
               Print("end fun bot_logRcvMsgV2（）");
            }
            catch (Exception e)
            {
               Print(e);
            }

        }

        public static void bot_logRcvMsg(Update update, string dir1)
        {
            try
            {
                var updateString = JsonConvert.SerializeObject(update, Formatting.Indented);
          //      const string dir1 = "msgRcvDir1115";
                Directory.CreateDirectory(dir1);
               Print(updateString);
                // 获取当前时间并格式化为文件名
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                string fileName = $"{dir1}/{timestamp}.json";
               Print(fileName);
                filex.Mkdir4File(fileName);
                System.IO.File.WriteAllText("" + fileName, updateString);
            }
            catch (Exception e)
            {
               Print(e);
            }

        }

        public static void bot_logRcvMsg(Update update)
        {
            try
            {
                var updateString = JsonConvert.SerializeObject(update, Formatting.Indented);
                const string dir1 = "msgRcvDir1115";
                Directory.CreateDirectory(dir1);
               //print(updateString);
                // 获取当前时间并格式化为文件名
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                string fileName = $"{dir1}/{timestamp}.json";
               Print(fileName);
                filex.Mkdir4File(fileName);
                System.IO.File.WriteAllText("" + fileName, updateString);
            }
            catch (Exception e)
            {
               Print(e);
            }

        }
    //    public static TelegramBotClient botClient;

        public static async Task evt_newUserjoinSngle(long? chatId, long? userId, Telegram.Bot.Types.User user, Update? update)
        {
            var __METHOD__ = "evt_newUserjoinSngle";
            dbgCls.PrintCallFunArgs(__METHOD__, dbgCls.func_get_args(MethodBase.GetCurrentMethod(), chatId, userId, user));


            //记录拉如机器人记录，谁拉到哪个群了。。未来权限判断
            try
            {
                var grpid = update.MyChatMember.Chat.Id;
                var uid = update.MyChatMember.From.Id;
                var botNme = update.MyChatMember.NewChatMember.User.Username;
                string f = $"{prjdir}/db/botEnterGrpLog/inGrp{grpid}.u{uid}.addBot.{botNme}.json";
                WriteAllText( f, update);
            }catch(Exception e)
            {
                PrintCatchEx(__METHOD__,e);
            }
           

            try
            {
                if (user.Username.ToLower().StartsWith("shibo"))
                    return;
                if (user.Username.ToLower().StartsWith("lianxin_"))
                {
                    dbgCls.PrintRet(__METHOD__, 0); return;
                }


                // 禁言用户
                await Program.botClient.RestrictChatMemberAsync(chatId, userId ?? 0, permissions: new Telegram.Bot.Types.ChatPermissions
                {
                    CanSendDocuments = false,
                    CanSendPhotos = false,
                    CanSendPolls = false,
                    CanSendVideoNotes = false,
                    CanSendVideos = false,
                    CanSendVoiceNotes = false,
                    CanSendAudios = false,
                    CanSendMessages = false,
                    //    CanSendMediaMessages = false,
                    CanSendOtherMessages = false,
                    CanAddWebPagePreviews = false
                });
            }
            catch (Exception e)
            {
               Print(e);
            }


            // 发送欢迎消息和按钮
            try
            {
                var inlineKeyboard = new InlineKeyboardMarkup(new[]
                 {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("解除禁言", $"btn=解除禁言&uid={userId}")
                }
                 });

                await Program.botClient.SendTextMessageAsync(chatId, $"@{user.Username} 欢迎来到群组，请点击按钮解除禁言状态。", replyMarkup: inlineKeyboard);

            }
            catch (Exception e)
            {
               Print(e);
            }
            dbgCls.PrintRet(__METHOD__, 0);

        }

        public static bool IsBtm_btnClink_in_prvt(Update update)
        {
            if (update.Type != UpdateType.Message)
            {
                return false;
            }
            string msgx2024 = tglib.bot_getTxt(update);

            if (update?.Message?.Chat?.Type == ChatType.Private)
            {
                ArrayList a = filex.rdWdsFromFile($"{prjdir}/menu/底部公共菜单.txt");
                return a.Contains(msgx2024);
            }
            return false;

        }
        public static bool tg_isBtm_btnClink_in_pubGrp(Update update)
        {
            if (update.Type != UpdateType.Message)
            {
                return false;
            }
            string msgx2024 = tglib.bot_getTxt(update);

            if (update?.Message?.Chat?.Type != ChatType.Private)
            {
                ArrayList a = filex.rdWdsFromFile($"{prjdir}/menu/底部公共菜单.txt");
                return a.Contains(msgx2024);
            }
            return false;

        }
        //if  is nml msg ,not search
        public static bool IsNnmlMsgInGrp(Update? update)
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
                && update.Message.ReplyToMessage.From.Username == botname
                && strCls.isStartsWith(update.Message?.ReplyToMessage?.Text, "我是便民助手")
                )
            {
                return false;  // not nml msg ,start search;
            }

            //pingjia 内容，不要进行反馈搜索
            if (update?.Message?.ReplyToMessage != null &&
                strCls.Contain(update?.Message?.ReplyToMessage?.Caption, "---联系方式---"))
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


            if (isGrpChat(update?.Message?.Chat?.Type))// if grp in 
            {

                if (update?.Message == null)
                    return false;
                if (update?.Message?.Text == null)
                    return false;

                if ((bool)update?.Message?.Text.StartsWith("@" + botname))
                    return false;

                // 
               Print("搜索触发词 in isNumlMsgInGrp()");
                var trgSearchKwds = " ";
                var trgWd = biz_other.getTrgwdHash($"{prjdir}/cfg/搜索触发词.txt");
                trgSearchKwds = trgSearchKwds + trgWd;
                if (strCls.ContainKwds(update?.Message?.Text, trgSearchKwds))
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

               Print("nml msg");
                return true;
            }
            else  //prvt mode  ,,,not nml msg
                return false;

        }

        public static bool tg_isGrpChat(Update update)
        {
            if(update.Type==UpdateType.CallbackQuery)
                return isGrpChat(update?.CallbackQuery.Message?.Chat?.Type);
            return isGrpChat(update?.Message?.Chat?.Type);

        }
        public static bool isGrpChat(Update update)
        {
            return isGrpChat(update?.Message?.Chat?.Type);
        }
       
      //  isGrpChat(update?.Message?.Chat?.Type)
        public static bool isGrpChat(ChatType? type)
        {
            if (type == ChatType.Private)
                return false;
            if (type == ChatType.Group || type == ChatType.Supergroup || type == ChatType.Channel)
                return true;
            return false;
        }
        public static KeyboardButton[][] RemoveButtonByName(KeyboardButton[][] keyboard, string buttonName)
        {
            for (int i = 0; i < keyboard.Length; i++)
            {
                List<KeyboardButton> buttons = new List<KeyboardButton>(keyboard[i]);
                buttons.RemoveAll(button => button.Text == buttonName);
                keyboard[i] = buttons.ToArray();
            }

            return keyboard;
        }
      
    

        public static ReplyKeyboardMarkup tg_btmBtnsV2(object chattype1)
        {
         string   chattype = cast_toString(chattype1);
            ReplyKeyboardMarkup rplyKbdMkp;
            if (chattype.Trim().ToLower()=="private")
            {  
                rplyKbdMkp = tgBiz.tg_btmBtns();
                KeyboardButton[][] kbtns = (KeyboardButton[][])rplyKbdMkp.Keyboard;
                RemoveButtonByName(kbtns, juliBencyon);
            }
            else
            {
                rplyKbdMkp = tgBiz.tg_btmBtns();
            }
            return rplyKbdMkp;
        }


        /// <summary>
        ///      //            mg MR.HAN, [18 / 7 / 2024 下午 12:00]
        //
        //   招募代理             授权加盟  分销连锁
        //合伙合营 盘口租赁 众筹众营
        //mg MR.HAN, [18 / 7 / 2024 下午 12:00]
        //分别这三个图标
        /// </summary>
        /// <returns></returns>
        public static ReplyKeyboardMarkup tg_btmBtns()
        {
       
            string line = "🌐%20招募代理    🏢%20授权加盟   🏪%20分销连锁";
            var kbdBtnArr = castString2kbdBtnArr(line);
            var kbdBtnArr2 = castString2kbdBtnArr("🤝%20合伙合营    📄%20盘口租赁    👨‍👩‍👧‍👦%20全民众营");

            var Keyboard =
                new KeyboardButton[][]
                {
                            new KeyboardButton[]
                            {
                                new KeyboardButton("💵💵💵 世博博彩 💵💵💵")
                            },
                            kbdBtnArr,kbdBtnArr2,
                            new KeyboardButton[]
                            {
                                new KeyboardButton("商家"),
                                       new KeyboardButton("猎艳"),
                                              new KeyboardButton("好奇"),
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
                                   new KeyboardButton("代付") ,
                                    new KeyboardButton("商城") ,
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


                            },
                             new KeyboardButton[]
                            {

                                 new KeyboardButton(juliBencyon),

                                  new KeyboardButton("🫂 加入联信"),
                                   new KeyboardButton("📝 合作洽谈")  


                            }



                                  




                };
            var rkm = new ReplyKeyboardMarkup(Keyboard);
            
            return rkm;
        }

        public static KeyboardButton[] castString2kbdBtnArr(string line)
        {
            // 将字符串按空格分割为数组
            string[] buttonTexts = line.Split(' ');

            // 将数组元素转换为 KeyboardButton 对象
            KeyboardButton[] keyboardButtons = buttonTexts
                .Select(text => {
                    text = DecodeUrl(text);
                    return new KeyboardButton(text);
                    })
                .ToArray();
            return keyboardButtons;
        }
    }

    public class UpdateEventArgs
    {
        public Update Update { get; internal set; }
    }

}
