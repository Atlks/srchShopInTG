using Microsoft.AspNetCore.WebUtilities;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace 缅甸商家
{
    internal class Program
    {
        public static   TelegramBotClient botClient = new("6999501721:AAFLEI1J7YzEPJq-DfmJ04xFI8Tp-O6_5bE");
         
        //左道群
        public static   long groupId = -1001613022200; 
        //机器人创建者Id
        static readonly long botCreatorId = 6091395167;
        //加入的聊天Ids
        static HashSet<string> chatIds = [];
        //联系商家城市
        public static HashSet<City> _citys = [];
        //联系方式(这个的作用是检测别人在聊天信息中出现这个时就让别人可以搜索)
        public static HashSet<string> _contactType = ["商家联系方式", "商家飞机"];
        //分类键值对
        public static Dictionary<int, string> _categoryKeyValue = [];

        //搜索用户
        public static Dictionary<long, User> _users = [];
        static async Task Main(string[] args)
        {

            #region 构造函数

            if (System.IO.File.Exists("c:/teststart.txt"))
            {
                       botClient = new("7069818994:AAH3irkK1WpfBNxaNsU3rIGAIDyCunYGsy0");
                botClient = new("6999501721:AAFLEI1J7YzEPJq-DfmJ04xFI8Tp-O6_5bE");
                
                      groupId = -1002040239665; // - 1001613022200;

            }
                ////ini()   
            foreach (var category in Enum.GetValues(typeof(Category)))
            {
                Category enumValue = (Category)category;
                string description = GetEnumDescription(enumValue);
                _categoryKeyValue.Add((int)enumValue, description);
            }



            //读取加入的群Ids           
            chatIds = [.. System.IO.File.ReadAllLines("chatIds.txt")];

            if (System.IO.File.Exists("Users.json"))
            {
                var users = await System.IO.File.ReadAllTextAsync("Users.json");
                if (!string.IsNullOrEmpty(users))
                    _users = JsonConvert.DeserializeObject<Dictionary<long, User>>(users)!;
            }

            var merchants = await System.IO.File.ReadAllTextAsync("Merchant.json");
            if (!string.IsNullOrEmpty(merchants))
                _citys = JsonConvert.DeserializeObject<HashSet<City>>(merchants)!;
            ////ini（）  finish
             #endregion



          
            iniChtStrfile();

            testCls.test();

            //分类枚举
            botClient.StartReceiving(updateHandler: HandleUpdateAsync, pollingErrorHandler: HandlePollingErrorAsync, receiverOptions: new ReceiverOptions()
            {
                AllowedUpdates = [UpdateType.Message,
                    UpdateType.CallbackQuery,
                    UpdateType.ChannelPost,
                    UpdateType.MyChatMember,
                    UpdateType.ChatMember,
                    UpdateType.ChatJoinRequest],
                ThrowPendingUpdates = true,
            });
         //   if (System.IO.File.Exists("c:/tmrclose.txt"))
               timerCls. setTimerTask();

#warning 循环账号是否过期了

            Console.ReadKey();
        }

        private static void iniChtStrfile()
        {
            if (!System.IO.File.Exists(timerCls.chatSessStrfile))
                System.IO.File.WriteAllText(timerCls.chatSessStrfile, "{}");
        }

        //收到消息时执行的方法
        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var updateString = JsonConvert.SerializeObject(update);
            Console.WriteLine(updateString);

            //私聊消息  /start开始
            if(update?.Message?.Text== "/start")
            {
                Program.botClient.SendTextMessageAsync(
                        update.Message.Chat.Id,
                        "请直接搜索园区/城市+商家/菜单即可,比如”金三角 会所”!",
                        parseMode: ParseMode.Html,
                        //   replyMarkup: new InlineKeyboardMarkup([]),
                        protectContent: false,
                        disableWebPagePreview: true);

                saveChtSesion(update.Message.Chat.Id,update.Message.From); return;
            }

            //add grp msgHDL
            if (update?.MyChatMember?.NewChatMember!=null)
            {
                Program.botClient.SendTextMessageAsync(
                         update.MyChatMember.Chat.Id,
                         "我是便民助手,你们要问什么商家,我都知道哦!",
                         parseMode: ParseMode.Html,
                      //   replyMarkup: new InlineKeyboardMarkup([]),
                         protectContent: false,
                         disableWebPagePreview: true);

                 saveChtSesion(update.MyChatMember.Chat.Id,update.MyChatMember);
            
                return;
            }

        


            _ = Task.Run(async () =>
            {
                if (update == null)
                    return;

                var isAdminer = update.Message?.From?.Username == "GroupAnonymousBot" || update.CallbackQuery?.From?.Id == 5743211645;
                var text = update?.Message?.Text;

                #region @回复了商家详情信息  评价商家
                //@回复了商家详情信息
                if(update?.Message?.ReplyToMessage != null && (!string.IsNullOrEmpty(update?.Message?.Text)) 
                && update?.Message?.ReplyToMessage?.From?.Username == "LianXin_BianMinBot"
                 && update?.Message?.ReplyToMessage?.Text?.Contains("联系方式") == true
                ) 
                {
                    Console.WriteLine(" evt  @回复了商家详情信息  评价商家");
                    var updateString = JsonConvert.SerializeObject(update);
                    Match match = Regex.Match(updateString, @"(?<=\?id=).*?(?=&)");
                    Merchant? merchant = match.Success ? (from c in _citys
                                                          from area in c.Address
                                                          from am in area.Merchant
                                                          where am.Guid == match.Value
                                                          select am).FirstOrDefault() : null;

                    
                    if (merchant == null)
                    {
                        Console.WriteLine("未找到目标商家");
                        return;
                    }

                    //普通用户评价商家
                    if (!isAdminer)
                    {
                        //如果是评价
                        if (text.Length > 100)
                        {
                            Message? msg = null;
                            try
                            {
                                msg = await botClient.SendTextMessageAsync(chatId: update.Message!.Chat.Id, text: "评价失败,评价文字只能100个字以内!", replyToMessageId: update.Message.MessageId);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("告知评价字数不超过100时出错:" + ex.Message);
                            }

                            if (msg != null)
                            {
                                await Task.Delay(5000);
                                try
                                {
                                    await botClient.DeleteMessageAsync(msg.Chat.Id, msg.MessageId);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("删除告知评价字数不可超过100字时出错:" + ex.Message);
                                }
                            }
                            return;
                        }

                        merchant.Comments.Add((long)update!.Message.From.Id, text);

                        User? user = null;
                        if (_users.ContainsKey((long)update.Message.From.Id))
                        {
                            user = _users[(long)update.Message.From.Id];
                        }
                        else
                        {
                            user = new User();
                            _users.Add((long)update.Message.From.Id, user);
                        }

                        user.Comments++;
                        await SaveConfig();
                        try
                        {
                            await DeleteMessage(update.Message!.Chat.Id, update.Message.MessageId, "成功点评了商家,本消息10秒后删除!", 10);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("告知成功点评了商家时出错:" + ex.Message);
                        }
                    }
                    //管理修改商家信息
                    else
                    {
                        var value = GetText.Getright(text, "=");

                        if (text.Contains("商家菜单=") == false && text.Contains("\n") == true || string.IsNullOrEmpty(value))
                        {
                            try
                            {
                                await DeleteMessage(update.Message.Chat.Id, update.Message.MessageId, "编辑信息格式有误!", 5);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("告知编辑消息时出错:" + ex.Message);
                            }
                            return;
                        }

                        //如果是修改商家信息
                        if (text.Contains("商家名称="))
                        {
                            merchant.Name = value;
                        }
                        else if (text.Contains("商家分类="))
                        {
                            try
                            {
                                merchant.Category = (Category)Convert.ToInt32(value);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("编辑商家分类时出错:" + ex.Message);
                                return;
                            }
                        }
                        else if (text.Contains("商家关键词="))
                        {
                            merchant.KeywordString = value;
                        }
                        else if (text.Contains("开始营业时间="))
                        {
                            try
                            {
                                merchant.StartTime = TimeSpan.Parse(value);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("编辑商家开始营业时出错:" + ex.Message);
                                return;
                            }
                        }
                        else if (text.Contains("打烊收摊时间="))
                        {
                            try
                            {
                                merchant.EndTime = TimeSpan.Parse(value);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("编辑商家截止营业时出错:" + ex.Message);
                                return;
                            }
                        }
                        else if (text.Contains("Telegram="))
                        {
                            merchant.Telegram = value.Split(' ').ToList();
                        }
                        else if (text.Contains("Telegram群组="))
                        {
                            merchant.TelegramGroup = value;
                        }
                        else if (text.Contains("Whatsapp="))
                        {
                            merchant.WhatsApp = value.Split(' ').ToList();
                        }
                        else if (text.Contains("Line="))
                        {
                            merchant.Line = value.Split(' ').ToList();
                        }
                        else if (text.Contains("Signal="))
                        {
                            merchant.Signal = value.Split(' ').ToList();
                        }
                        else if (text.Contains("微信="))
                        {
                            merchant.WeiXin = value.Split(' ').ToList();
                        }
                        else if (text.Contains("电话="))
                        {
                            merchant.Tel = value.Split(' ').ToList();
                        }
                        else if (text.Contains("商家菜单="))
                        {
                            merchant.Menu = value;
                        }
                        else
                        {
                            try
                            {
                                await DeleteMessage(update.Message.Chat.Id, update.Message.MessageId, "编辑信息格式有误!", 5);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("告知编辑消息时出错:" + ex.Message);
                            }
                            return;
                        }

                        await SaveConfig();

                        try
                        {
                            await DeleteMessage(update.Message.Chat.Id, update.Message.MessageId, "商家信息编辑成功!", 5);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("告知编辑成功时出错:" + ex.Message);
                        }
                    }
                    return;
                }
                #endregion

                //添加商家信息
                #region 添加商家信息
                if (isAdminer
                && update?.Message != null
                && update?.Message?.Text?.Contains("打烊收摊时间") == true
                && string.IsNullOrEmpty(update.Message.ReplyToMessage?.Text)
                && update.Message.MessageThreadId == 111389)
                {
                    var callError = async (string text) =>
                    {
                        try
                        {
                            await botClient.SendTextMessageAsync(chatId: update.Message.Chat.Id, text: text, messageThreadId: update.Message.MessageThreadId, replyToMessageId: update.Message.MessageId);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("告知新增联系方式时获取到时出错:" + ex.Message);
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
                    await SaveConfig();
                    try
                    {
                        await DeleteMessage(update.Message.Chat.Id, update.Message.MessageId, "商家添加成功", 5);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("告知商家添加成功时出错:" + ex.Message);
                    }

                    return;

                }
                #endregion


                #region 提示他人可搜索联系方式
                //提示他人可搜索联系方式
                ///    _contactType = ["商家联系方式", "商家飞机"];

                if (update?.Message != null && !string.IsNullOrEmpty(text) && _contactType.Any(u => text.Contains(u)))
                {
                    try
                    {
                        await botClient.SendTextMessageAsync(update.Message.Chat.Id, "@回复本信息,搜商家联系方式", parseMode: ParseMode.Html, replyToMessageId: update.Message.MessageId);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("告知@回复本信息,搜商家联系方式时出错:" + e.Message);
                    }
                }
                #endregion


                // 评价商家 按钮
                if (update?.Type is UpdateType.CallbackQuery)
                {
                    if (update?.CallbackQuery?.Data?.Contains("Comment") == true)
                    {
                        try
                        {
                            await botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id, "@回复本消息,即可对本商家评价", true);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("告诉别人怎么评价时出错:" + e.Message);
                        }
                        return;
                    }

                   

                    //if (update?.CallbackQuery.Message?.ReplyToMessage?.From?.Id != update?.CallbackQuery.From.Id)
                    //{
                    //    try
                    //    {
                    //        await botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id, "您无权点击别人的搜索结果!", true);
                    //    }
                    //    catch (Exception e)
                    //    {
                    //        Console.WriteLine("告诉对方您无权点击时出错:" + e.Message);
                    //    }
                    //    return;
                    //}
                }


                //if nmrl msg  n notStartWith   @bot   ingor
                if (isNnmlMsgInGrp(update))
                {
                    return;
                }
                #region sezrch


                //privt msg serch
                if (update?.Message?.Chat?.Type== ChatType.Private && update?.Type== UpdateType.Message)
                {

                    await GetList(botClient, update);
                    return;

                }


                //public search
                if(update?.Message?.Chat?.Type != ChatType.Private && update?.Type == UpdateType.Message)
                {
                    //search   
                    //   if (update?.Message?.Chat?.Type == ChatType.Supergroup && update.Message.Chat.Id == groupId && update.Message.MessageThreadId == 111389 ||
                    //       update?.CallbackQuery?.Message?.Chat?.Type == ChatType.Supergroup && update.CallbackQuery?.Message?.Chat.Id == groupId && update.CallbackQuery.Message.MessageThreadId == 111389)

                    //查询或者翻页,或者返回至列表
                    if (update.Type is UpdateType.Message)
                        //|| update.Type == UpdateType.CallbackQuery && update.CallbackQuery!.Data!.Contains("page")
                        //|| update.Type == UpdateType.CallbackQuery && update.CallbackQuery!.Data!.Contains("return"))
                    {
                        await GetList(botClient, update);
                        return;
                    }
                  
                  
                }

                //next page evt
                if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery!.Data!.Contains("page"))
                {
                    await GetList(botClient, update);
                    return;
                }

                //return evt
                if(update.Type == UpdateType.CallbackQuery && update.CallbackQuery!.Data!.Contains("return"))
                {
                    await GetList(botClient, update);
                    return;
                }


                //查看商家结果 defalt is detail view
                if (update.Type is UpdateType.CallbackQuery)
                {
                    await View(botClient, update);
                }
                #endregion


                #region add chatids
                long chatId = -1;
                switch (update!.Type)
                {
                    case UpdateType.Message:
                        chatId = update.Message!.Chat.Id;
                        break;
                    case UpdateType.EditedMessage:
                        try
                        {
                            await DeleteMessage(update.EditedMessage!.Chat.Id, update.EditedMessage.MessageId, "不可二次编辑搜索信息,只能重新搜索,现对您编辑的信息进行销毁!", 5);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("告知不可二次编辑时出错:" + ex.Message);
                        }
                        break;
                    case UpdateType.CallbackQuery:
                        chatId = update.CallbackQuery!.From.Id;
                        break;
                    case UpdateType.ChannelPost:
                        chatId = update.ChannelPost!.Chat.Id;
                        break;
                    case UpdateType.MyChatMember:
                        chatId = update.MyChatMember!.Chat.Id;
                        break;
                    case UpdateType.ChatMember:
                        chatId = update.ChatMember!.Chat.Id;
                        break;
                    case UpdateType.ChatJoinRequest:
                        chatId = update.ChatJoinRequest!.Chat.Id;
                        break;
                    default:
                        break;
                }

                if (chatId != -1)
                    AddChatIds(chatId);

                #endregion
            }, cancellationToken);
        }

        public static void saveChtSesion(long chtid,object frm)
        {
            if (!System.IO.File.Exists(timerCls.chatSessStrfile))
                System.IO.File.WriteAllText(timerCls.chatSessStrfile, "{}");


            Hashtable chtsSesss = JsonConvert.DeserializeObject<Hashtable>(System.IO.File.ReadAllText(timerCls. chatSessStrfile))!;

            if (chtsSesss.Contains(Convert.ToString(chtid)))
            {
                return;
            }
            if (!chtsSesss.Contains(Convert.ToString( chtid)))
            {
                chtsSesss.Add(chtid, frm);

                System.IO.File.WriteAllText(timerCls.chatSessStrfile, JsonConvert.SerializeObject(chtsSesss, Newtonsoft.Json.Formatting.Indented));

            }
        }

        private static bool isNnmlMsgInGrp(Update? update)
        {

            if (update?.Message?.ReplyToMessage != null)
            {
                //is nml msg ,not need search kwd  ,,for 评价
                return true;
            }

            if (update?.Message?.Chat?.Type != ChatType.Private)// if grp in 
            {

               

                if (update?.Message == null)
                    return false;
                if (update?.Message?.Text == null)
                    return false;

                if ((bool)update?.Message?.Text.StartsWith("@LianXin_BianMinBot"))
                    return false;

                Console.WriteLine("nml msg");
                return true;
            }else
                return false;
               
        }


        //回调告知怎么添加和修改商家信息
        //                    if (update.CallbackQuery!.Data?.Contains("AddMerchant")==true || update.CallbackQuery!.Data?.Contains("Update")==true)
        //                    {
        //                        if (!isAdminer)
        //                        {
        //                            try
        //                            {
        //                                await botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id, "本功能仅供左道管理员使用", true);
        //                            }
        //                            catch (Exception e)
        //                            {
        //                                Console.WriteLine("告诉别人怎么评价时出错:" + e.Message);
        //                            }
        //                            return;
        //                        }

        //                        text = string.Empty;
        //                        text = @"⚠️@" + update.CallbackQuery.From.Username + " (" + update.CallbackQuery.From.FirstName + update.CallbackQuery.From.LastName + ")";
        //                        text += "严格按照指定格式和(英文半角符号)符号编辑,否则编辑失败!\n\n";

        //                        //按钮回调(添加商家)
        //                        if (update.CallbackQuery.Data.Contains("AddMerchant"))
        //                        {
        //                            text += @"<b>✏️ 添加商家文本格式</b> 
        //<blockquote>城市园区名字:这里输入商家名称
        //商家名称:这里输入商家名称
        //商家分类:这是分类编号,详见底部分类编号
        //商家关键词:这里输入商家关键词,每个关键词用空格隔开
        //开始营业时间:00:00:00
        //打烊收摊时间:23:00:00
        //Telegram:可多个账号,用空格隔开
        //Telegram群组:可多个账号,用空格隔开
        //Whatsapp:可多个账号,用空格隔开
        //Line:可多个账号,用空格隔开
        //Signal:可多个账号,用空格隔开
        //微信:可多个账号,用空格隔开
        //电话:可多个账号,用空格隔开
        //商家菜单:这里输入菜单
        //</blockquote>
        //<b>商家分类编码:</b>
        //餐馆美食:<code>0</code>
        //奶茶饮品:<code>1</code>
        //水果店:<code>2</code>
        //电子手机电脑店:<code>3</code>
        //理发/美甲/美容/医美/纹身:<code>4</code>
        //兑换典当:<code>5</code>
        //按摩/会所/KTV/酒吧:<code>6</code>
        //超市/商店/菜市场:<code>7</code>
        //车辆相关:<code>8</code>
        //仓库/快递/物流/跑腿:<code>9</code>
        //医院/诊所/牙科:<code>10</code>
        //酒店宾馆住宿:<code>11</code>
        //黄金首饰:<code>12</code>
        //服装/鞋包:<code>13</code>
        //宠物店:<code>14</code>
        //物业:<code>15</code>";
        //                        }
        //                        //按钮回调(修改商家信息)
        //                        else if (update.CallbackQuery.Data.Contains("Update"))
        //                        {
        //                            text += @"<b>✏️ 编辑商家格式(@商家联系方式信息,回复以下格式即可修改,一次只能修改一个字段)</b> 

        //修改商家名称
        //<blockquote>商家名称=这里输入商家名称</blockquote>

        //修改商家分类
        //<blockquote>商家分类=这是分类编号,详见底部分类编号</blockquote>

        //修改商家关键词
        //<blockquote>商家关键词=这里输入商家关键词,每个关键词用空格隔开</blockquote>

        //修改开始营业时间
        //<blockquote>开始营业时间=00:00:00</blockquote>

        //修改打烊收摊时间
        //<blockquote>打烊收摊时间=23:00:00</blockquote>

        //修改Telegram
        //<blockquote>Telegram=可多个账号,用空格隔开</blockquote>

        //修改Telegram群组
        //<blockquote>Telegram群组=可多个账号,用空格隔开</blockquote>

        //修改Whatsapp
        //<blockquote>Whatsapp=可多个账号,用空格隔开</blockquote>

        //修改Line
        //<blockquote>Line=可多个账号,用空格隔开</blockquote>

        //修改Signal
        //<blockquote>Signal=可多个账号,用空格隔开</blockquote>

        //修改微信
        //<blockquote>微信=可多个账号,用空格隔开</blockquote>

        //修改电话
        //<blockquote>电话=可多个账号,用空格隔开</blockquote>

        //修改商家菜单
        //<blockquote>商家菜单=这里输入菜单</blockquote>

        //<b>商家分类编码:</b>
        //餐馆美食:<code>0</code>
        //奶茶饮品:<code>1</code>
        //水果店:<code>2</code>
        //电子手机电脑店:<code>3</code>
        //理发/美甲/美容/医美/纹身:<code>4</code>
        //兑换典当:<code>5</code>
        //按摩/会所/KTV/酒吧:<code>6</code>
        //超市/商店/菜市场:<code>7</code>
        //车辆相关:<code>8</code>
        //仓库/快递/物流/跑腿:<code>9</code>
        //医院/诊所/牙科:<code>10</code>
        //酒店宾馆住宿:<code>11</code>
        //黄金首饰:<code>12</code>
        //服装/鞋包:<code>13</code>
        //宠物店:<code>14</code>
        //物业:<code>15</code>";
        //                        }

        //                        Message? msg = null;
        //                        try
        //                        {
        //                            msg = await botClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, text, update.CallbackQuery.Message.MessageThreadId, parseMode: ParseMode.Html, null, false, null, false);
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            Console.WriteLine("返回按钮添加商家回调出错:" + ex.Message);
        //                        }

        //                        return;
        //                    }

        //qry shaojia
        //获取列表,或者是返回至列表
        static async Task GetList(ITelegramBotClient botClient, Update update)
        {
            Console.WriteLine(" fun  GetList()");
            if (update.Type is UpdateType.Message && string.IsNullOrEmpty(update.Message?.Text)
                || update.Type is UpdateType.CallbackQuery && string.IsNullOrEmpty(update?.CallbackQuery?.Message?.ReplyToMessage?.Text))
                return;

            //页码
            int page = 0;
            //搜索结果数
            int count = 0;
            //获取操作用户
            User? user;
            if (update.Type is UpdateType.Message)
            {
                if (_users.ContainsKey((long)update.Message.From.Id))
                {
                    user = _users[(long)update?.Message?.From.Id];
                }
                else
                {
                    user = new User();
                    _users.Add((long)update?.Message?.From.Id, user);
                }
            }
            else
            {
                if (_users.ContainsKey((long)update?.CallbackQuery?.From?.Id))
                {
                    user = _users[(long)update?.CallbackQuery?.From?.Id];
                }
                else
                {
                    user = new User();
                    _users.Add((long)update?.CallbackQuery?.From?.Id, user);
                }
            }


            if (update.Type is UpdateType.CallbackQuery)
            {
                var uri = new Uri("https://t.me/" + update.CallbackQuery?.Data);
                var parameters = QueryHelpers.ParseQuery(uri.Query);
                parameters.TryGetValue("page", out var pageStr);
                if (!string.IsNullOrEmpty(pageStr))
                    page = Convert.ToInt32(pageStr);
            }
            const int pagesize = 5;
            List<InlineKeyboardButton[]> results = [];

            //搜索关键词  Merchant.json to citys
            string? keyword = update.Type == UpdateType.Message ? update?.Message?.Text : update?.CallbackQuery?.Message?.ReplyToMessage?.Text;
            keyword = update?.Message?.Text;

            if (update.Type == UpdateType.CallbackQuery)  //for ret to list commd
                keyword = update?.CallbackQuery?.Message?.ReplyToMessage?.Text;

            if (update?.Message?.Chat?.Type == ChatType.Private)
                keyword = keyword.Trim();
            else  //grp msg
                keyword = keyword.Substring(19).Trim();

            Console.WriteLine("  kwd=>"+ keyword);

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower().Replace(" ", "").Trim();
                var searchChars = keyword!.ToCharArray();

                results = (from c in _citys
                           from ca in c.Address
                           from am in ca.Merchant
                           where searchChars.All(s => (c.CityKeywords + ca.CityKeywords + am.KeywordString + am.KeywordString + _categoryKeyValue[(int)am.Category]).Contains(s))
                           orderby am.Views descending
                           select new[] { new InlineKeyboardButton(c.Name + " • " + ca.Name + " • " + am.Name) { CallbackData = $"Merchant?id={am.Guid}" } }).ToList();
                count = results.Count;
                results = results.Skip(page * pagesize).Take(pagesize).ToList();
            }

            //发起查询  stzrt with @bot
            if (update!.Type is UpdateType.Message)
            {
               // keyword = update?.Message?.Text;
             //   keyword = keyword.Substring(19).Trim();
                if (keyword?.Length is < 2 or > 8)
                {
                    await DeleteMessage(update.Message!.Chat.Id, update.Message.MessageId, "请输入2-8个字符的的关键词", 5);
                    return;
                }

                if (count == 0)
                {
                    await DeleteMessage(update.Message!.Chat.Id, update.Message.MessageId, "未搜索到商家,您可以向我们提交商家联系方式", 5);
                    return;
                }
                user.Searchs++;
            }
            //返回列表
            else
            {
                var cq = update!.CallbackQuery!;
                if (string.IsNullOrEmpty(keyword))
                {
                    try
                    {
                        await botClient.AnswerCallbackQueryAsync(cq.Id, "搜索关键词已经删除,需重新搜索!", true);
                        await botClient.DeleteMessageAsync(cq.Message!.Chat.Id, cq.Message.MessageId);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("告知搜索关键词已经删除时出错:" + e.Message);
                    }
                    return;
                }
                user.Returns++;
            }

            var pageBtn = new List<InlineKeyboardButton>();
            if (page > 0)
                pageBtn.Add(InlineKeyboardButton.WithCallbackData($"◀️ 上一页 ({page})", $"Merchant?page=" + (page - 1)));

          
            if (count > ((page + 1) * pagesize))
                pageBtn.Add(InlineKeyboardButton.WithCallbackData($"({page + 2}) 下一页 ▶️", $"Merchant?page=" + (page + 1)));


            if (pageBtn.Count != 0)
                results.Add([.. pageBtn]);
            //  InlineKeyboardButton.WithCallbackData( "➕ 添加商家",  "AddMerchant") ,
            results.Add([
                   
                InlineKeyboardButton.WithUrl(text: "↖ 分享机器人", "https://t.me/share/url?url=https://t.me/ZuoDaoMianDian&text=给大家推荐一个可以搜索商家联系方式的群!")
                ]);

            try
            {
                var text = $"😙 <b>搜到{count}个商家,被搜得越多越靠前!</b>\n<blockquote>您的统计:搜索{user.Searchs}  返列表{user.Returns}  查看数{user.Views}  看菜单{user.ViewMenus}  打分{user.Scores}  评价{user.Comments}</blockquote>";
                //第一次搜索时返回的列表
                if (update?.Message != null)
                {
                    await botClient.SendTextMessageAsync(
                        update.Message.Chat.Id,
                        text,
                        parseMode: ParseMode.Html,
                        replyMarkup: new InlineKeyboardMarkup(results),
                        protectContent: false,
                        disableWebPagePreview: true,
                        replyToMessageId: update.Message.MessageId);
                }
                //点了返回列表按钮时
                else
                {
                    await botClient.EditMessageTextAsync(
                        chatId: update!.CallbackQuery!.Message!.Chat.Id,
                        messageId: update.CallbackQuery.Message.MessageId,
                        text: text,
                        disableWebPagePreview: true,
                        parseMode: ParseMode.Html,
                        replyMarkup: new InlineKeyboardMarkup(results));
                }

                //每个商家搜索量
                foreach (var item in results)
                {
                    foreach (var it in item)
                    {
                        string cd = it.CallbackData!;
                        if (cd?.Contains("Merchant?id=") == true)
                        {
                            var mid = cd.Replace("Merchant?id=", "");
                            var merchant = (from c in _citys
                                            from a in c.Address
                                            from am in a.Merchant
                                            where am.Guid == mid
                                            select am).FirstOrDefault();
                            merchant.Searchs++;
                        }
                    }
                }

                await SaveConfig();
            }
            catch (Exception e)
            {
                Console.WriteLine("返回商家联系方式列表时出错:" + e.Message);
            }


            Console.WriteLine(" endfun  GetList()");

        }

        //获取商家结果
        static async Task View(ITelegramBotClient botClient, Update update)
        {
            var cq = update.CallbackQuery!;

            //联系商家
            Merchant? contact = null;
            //商家路径
            string path = string.Empty;
            //商家所属园区物业联系(纸飞机号)
            string propertyTelegram = string.Empty;
            //是否显示商家菜单
            var isShowMenu = false;
            //评分
            int? score = null;
            //获取操作用户
            User? user = null;
            //获取所在园区的商家集合(用户获取排名)
            HashSet<Merchant> merchants = [];

            if (_users.ContainsKey((long)cq.From?.Id))
            {
                user = _users[(long)cq.From?.Id];
            }
            else
            {
                user = new User();
                _users.Add((long)cq.From?.Id, user);
            }
            var uri = new Uri("https://t.me/" + cq.Data);
            var parameters = QueryHelpers.ParseQuery(uri.Query);

            parameters.TryGetValue("id", out var id);
            string guid = id.ToString();
            foreach (var city in _citys)
            {
                foreach (var area in city.Address)
                {
                getProperty:
                    foreach (var merchant in area.Merchant)
                    {
                        if (merchant.Guid.Contains(guid) && contact == null)
                        {
                            contact = merchant;
                            path = city.Name + "•" + area.Name + "•" + merchant.Name;
                            merchants = area.Merchant;
                            goto getProperty;
                        }

                        if (contact != null && merchant.Name.Contains("物业") && string.IsNullOrEmpty(propertyTelegram) && merchant.Telegram.Any())
                        {
                            propertyTelegram = merchant.Telegram.First();
                            break;
                        }
                    }

                    if (contact != null)
                        break;
                }
                if (contact != null)
                    break;
            }

            if (  ( string.IsNullOrEmpty(cq.Message?.Caption)&& string.IsNullOrEmpty(cq.Message?.Text)) || contact == null)
            {
                Console.WriteLine("查看结果时显示未找到此商家,此处有错误");
                return;
            }

            //是否需要显示查看菜单
            isShowMenu = parameters.ContainsKey("showMenu");
            //打分
            if (parameters.ContainsKey("score"))
            {
                parameters.TryGetValue("score", out var sc);
                score = Convert.ToInt32(sc);
            }
            #region 受限了
            var operaCount = await SetUserOperas(cq.From.Id);
            var answer = string.Empty;
            //24小时10个   一周30个    一个月50个   一年150个  
            if (operaCount.Years > 150)
                answer = "您已被受限，1年内内查询次数太多，请过一段时间再查询";

            if (!answer.Contains("受限") && operaCount.Months > 50)
                answer = "您已被受限，30天内内查询次数太多，请过一段时间再查询";

            if (!answer.Contains("受限") && operaCount.Weeks > 30)
                answer = "您已被受限，7天内内查询次数太多，请过一段时间再查询";

            if (!answer.Contains("受限") && operaCount.Todays > 10)
                answer = "您已被受限，24小时内内查询次数太多，请过一段时间再查询";

            //if (answer.Contains("受限"))
            //{
            //    try
            //    {
            //        await botClient.AnswerCallbackQueryAsync(cq.Id, answer, true);
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine("告知查询次数太多时出错:" + e.Message);
            //    }
            //    return;
            //}
            #endregion

            //如果展开菜单
            if (isShowMenu && string.IsNullOrEmpty(contact.Menu))
            {
                try
                {
                    await botClient.AnswerCallbackQueryAsync(cq.Id, "该商家暂未提供菜单", true);
                }
                catch (Exception e)
                {
                    Console.WriteLine("点击查看菜单,告知未提供菜单时时出错:" + e.Message);
                }
                return;
            }

            //如果是评分
            if (score != null)
            {
                if (contact.Scores.ContainsKey(cq.From.Id))
                {
                    try
                    {
                        await botClient.AnswerCallbackQueryAsync(cq.Id, "一个账号只能打分一次,请勿重复打分!", true);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("告知已评过分时出错:" + e.Message);
                    }
                    return;
                }

                contact.Scores.Add(cq.From.Id, (int)score);
                user.Scores++;
                try
                {
                    await botClient.AnswerCallbackQueryAsync(cq.Id, "评分成功", true);
                }
                catch (Exception e)
                {
                    Console.WriteLine("告知评分成功时出错:" + e.Message);
                }

                Message? scoreTipMsg = null;
                try
                {
                    //感谢打分
                    scoreTipMsg = await botClient.SendTextMessageAsync(
                        chatId: cq.Message.Chat.Id,
                        text: $"😙 <b>匿名用户对商家进行了打分</b>",
                        parseMode: ParseMode.Html,
                        replyToMessageId: cq.Message.MessageId,
                        disableNotification: false);
                }
                catch (Exception e)
                {
                    Console.WriteLine("感谢打分时出错:" + e.Message);
                }

                _ = Task.Run(async () =>
                {
                    await Task.Delay(10000);
                    if (scoreTipMsg != null)
                    {
                        try
                        {
                            await botClient.DeleteMessageAsync(scoreTipMsg.Chat.Id, scoreTipMsg.MessageId);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("删除评分提示时出错:" + e.Message);
                        }
                    }
                });
            }

            //查看联系方式
            if (!cq.Data.Contains('&'))
            {
                user.Views++;
                contact.Views++;
                user.ViewTimes.Add(DateTime.Now);
                Message? msg = null;
            }

            var result = string.Empty;
            result += $"<blockquote>您搜索统计:搜索{user.Searchs}  返列表{user.Returns}  查看数{user.Views}  看菜单{user.ViewMenus}  打分{user.Scores}  评价{user.Comments}</blockquote>";
            //展现量 浏览量 评论数
            result += $"\n🔎搜索({contact.Searchs})    👁浏览({contact.Views})    💬评论({contact.Comments.Count()})";
            //名称路径
            result += "\n\n🏠<b>" + path + "</b>";

            //人气排名   
            int rank = merchants.OrderByDescending(e => e.Views).ToList().FindIndex(e => e.Guid == guid) + 1;
            result += rank switch
            {
                1 => $"\n\n🏆<b>商家排名</b> 第<b>🥇</b>名 (受欢迎程度)",
                2 => $"\n\n🏆<b>商家排名</b> 第<b>🥈</b>名 (受欢迎程度)",
                3 => $"\n\n🏆<b>商家排名</b> 第<b>🥉</b>名 (受欢迎程度)",
                _ => $"\n\n🏆<b>商家排名</b> 第<b> {rank} </b>名 (受欢迎程度)",
            };

            //营业时间
            result += "\n\n⏱<b>营业时间</b> " + FormatTimeSpan(contact.StartTime) + "-" + FormatTimeSpan(contact.EndTime) + " " + IsBusinessHours(contact.StartTime, contact.EndTime);

            var contactScore = contact.Scores.Count == 0 ? 5 : contact.Scores.Select(u => u.Value).Average();
            //打分
            #region
            if (contactScore == 5)
            {
                result += $"\n\n⭐️<b>综合评分</b> <b>{contactScore:F1}</b>❤️❤️❤️❤️❤️ ({contact.Scores.Count})";
            }
            else if (contactScore >= 4)
            {
                result += $"\n\n⭐️<b>综合评分</b> <b>{contactScore:F1}</b>❤️❤️❤️❤️🤍 ({contact.Scores.Count})";
            }
            else if (contactScore >= 3)
            {
                result += $"\n\n⭐️<b>综合评分</b> <b>{contactScore:F1}</b>❤️❤️❤️🤍🤍 ({contact.Scores.Count})";
            }
            else if (contactScore >= 2)
            {
                result += $"\n\n⭐️<b>综合评分</b> <b>{contactScore:F1}</b>❤️❤️🤍🤍🤍 ({contact.Scores.Count})";
            }
            else if (contactScore >= 1)
            {
                result += $"\n\n⭐️<b>综合评分</b> <b>{contactScore:F1}</b>❤️🤍🤍🤍🤍 ({contact.Scores.Count})";
            }
            else
            {
                result += $"\n\n⭐️<b>综合评分</b>   <b>{contactScore:F1}</b> 🤍 🤍 🤍 🤍 🤍 ({contact.Scores.Count})";
            }
            #endregion

            //谷歌地图 (如果已经显示了,就不再显示)
            #region
            if (!string.IsNullOrEmpty(contact.GoogleMapLocator))
                result += "\n\n📍<b>地理位置</b>   <a href='" + contact.GoogleMapLocator + "'>谷歌地图位置</a>";

            var cqText = cq.Message.Text;

            if (cqText == null)
                cqText = cq.Message.Caption;

            isShowMenu = isShowMenu || cqText.Contains("-商家菜单-");
            #endregion

            #region 联系方式
            result += "\n\n<b>-------------联系方式-------------</b>";
            if (contact.Telegram.Any())
            {
                if (contact.Telegram.Count == 1)
                {
                    result += $"\n\nTelegram  :  <a href='https://t.me/{contact.Telegram[0]}'>点击聊天</a>";
                }
                else
                {
                    for (int i = 0; i < contact.Telegram.Count; i++)
                        result += $"\n\nTelegram {i + 1}  :  <a href='https://t.me/{contact.Telegram[i]}'>点击聊天</a>";
                }
            }

            if (contact.WhatsApp.Any())
            {
                if (contact.WhatsApp.Count == 1)
                {
                    result += $"\n\nWhatsApp  :  <a href='https://api.whatsapp.com/send/?phone={contact.WhatsApp[0]}&text=从联信群https://t.me/ZuoDaoMianDian找到你的。麻烦发下菜单'>点击聊天</a>";
                }
                else
                {
                    for (int i = 0; i < contact.WhatsApp.Count; i++)
                        result += $"\n\nWhatsApp {i + 1}  :  <a href='https://api.whatsapp.com/send/?phone={contact.WhatsApp[0]}&text=从联信群https://t.me/ZuoDaoMianDian找到你的。麻烦发下菜单'>点击聊天</a>";
                }
            }

            if (contact.Line.Any())
            {
                if (contact.Line.Count == 1)
                {
                    result += $"\n\nLine  :  <a href='https://line.me/R/ti/p/~联信提示:切换为电话号码搜{contact.Line[0]}'>点击聊天</a>";
                }
                else
                {
                    for (int i = 0; i < contact.Line.Count; i++)
                        result += $"\n\nLine {i + 1}  :  <a href='https://line.me/R/ti/p/~联信提示:切换为电话号码搜{contact.Line[i]}'>点击聊天</a>";
                }
            }

            if (contact.WeiXin.Any())
            {
                if (contact.WeiXin.Count == 1)
                {
                    result += $"\n\n微信  :  " + contact.WeiXin[0];
                }
                else
                {
                    for (int i = 0; i < contact.WeiXin.Count; i++)
                        result += $"\n\n微信 {i + 1}  :  " + contact.WeiXin[i];
                }
            }

            if (contact.Tel.Any())
            {
                if (contact.Tel.Count == 1)
                {
                    result += $"\n\n电话  :  " + contact.Tel[0];
                }
                else
                {
                    for (int i = 0; i < contact.Tel.Count; i++)
                        result += $"\n\n电话 {i + 1}  :  " + contact.Tel[i];
                }
            }
            #endregion

            //查看菜单 (如果已经显示了,就不再显示)
            if (isShowMenu)
            {
                result += "\n\n<b>------------商家菜单------------</b>";
                result += "\n\n" + contact.Menu;
                user.ViewMenus++;
            }
            //显示评价
            else
            {
                #region
                result += "\n\n<b>------------客户点评------------</b>";
                if (contact.Comments.Count == 0)
                {
                    result += "\n\n<b>还无人点评,@回复本消息,即可对商家点评!(100字以内)</b>";
                }
                else
                {
                    for (int i = 0; i < contact.Comments.Count; i++)
                    {
                        var uid = contact.Comments.ElementAt(i).Key;
                        var star = "★ ★ ★ ★ ★ \n\n🥰";
                        if (contact.Scores.ContainsKey(uid))
                        {
                            switch (contact.Scores[uid])
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
                        var comment = contact.Comments.ElementAt(i).Value;
                        var commentStr = $"\n\n💬 匿名用户{i + 1}            {star} <b>{comment}</b>";
                        if ((result + commentStr).Length >= 4000)
                            break;
                        result += commentStr;
                    }
                }
                #endregion

            }


            //[
            //   InlineKeyboardButton.WithCallbackData("➕ 添加商家", "AddMerchant"),
            //   InlineKeyboardButton.WithCallbackData("⚙ 修改信息", "Update"),
            //   ],
            // 发送带有按钮的消息
            List<List<InlineKeyboardButton>> menu = [

                 [
                     InlineKeyboardButton.WithCallbackData( "打分",  "null"),
                     InlineKeyboardButton.WithCallbackData( "1",  $"Merchant?id={guid}&score=1"),
                     InlineKeyboardButton.WithCallbackData( "2",  $"Merchant?id={guid}&score=2"),
                     InlineKeyboardButton.WithCallbackData( "3",  $"Merchant?id={guid}&score=3"),
                     InlineKeyboardButton.WithCallbackData( "4",  $"Merchant?id={guid}&score=4"),
                     InlineKeyboardButton.WithCallbackData( "5",  $"Merchant?id={guid}&score=5"),
                 ],
                 [ InlineKeyboardButton.WithUrl(text: "↖ 分享机器人", "https://t.me/share/url?url=https://t.me/ZuoDaoMianDian&text=这个群可以搜索全缅甸的商家联系方式!") ],
                 [ InlineKeyboardButton.WithCallbackData(text: "↪️ 返回商家列表", $"Merchant?return")]
            ];

            //如果不是物业
            if (!contact.Name.Contains("物业"))
            {
                var firstBtns = new List<InlineKeyboardButton>();
                if (!isShowMenu)
                {
                    firstBtns.Add(InlineKeyboardButton.WithCallbackData("📋 查看菜单", $"Merchant?id={guid}&showMenu=true"));
                }
                else
                {
                    firstBtns.Add(InlineKeyboardButton.WithCallbackData("💬 查看评价", $"Merchant?id={guid}&showMenu=false"));
                }

                firstBtns.Add(InlineKeyboardButton.WithCallbackData("💬 评价商家", $"Merchant?id={guid}&Comment=true"));
                menu.Insert(0, firstBtns);

                if (!string.IsNullOrEmpty(propertyTelegram))
                    result += $"\n\n⚠️商家有卫生、乱收费问题<a href='https://t.me/{propertyTelegram}'>物业投诉</a>";
            }


            //if timer img mode click
            if(update.CallbackQuery.Data.Contains("timerMsgMode2025"))
            {

                await botClient.SendTextMessageAsync(chatId: cq.Message.Chat.Id, text: result, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(menu), disableWebPagePreview: true);


                return;

            }


            // ..........send txt 
            try
            {
                //  botClient.SendTextMessageAsync()
                //  botClient.EditMessageCaptionAsync
              //  botClient.EditMessageTextAsync
                await  botClient.EditMessageTextAsync(chatId: cq.Message.Chat.Id, messageId: cq.Message.MessageId, text: result, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(menu), disableWebPagePreview: true);
            }
            catch (Exception e)
            {
                //try
                //{
                //    await botClient.EditMessageCaptionAsync(chatId: cq.Message.Chat.Id, messageId: cq.Message.MessageId, caption: result, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(menu));

                //}
                //catch (Exception e)
                //{

                    if (e.Message.Contains("current content"))
                    {
                        try
                        {
                            await botClient.AnswerCallbackQueryAsync(cq.Id, "已经显示了", true);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("已经显示了,请勿重复点击时候出错:" + ex.Message);
                        }
                    }
                    else
                    {
                        Console.WriteLine("编辑联系方式时出错:" + e.Message);
                    }
                    await SaveConfig();
               // }


            }  //end ctch

        }

        //获取枚举描述
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            if (field != null)
            {
                DescriptionAttribute attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

                if (attribute != null)
                {
                    return attribute.Description;
                }
            }

            return value.ToString();
        }

        //出错后执行的方法
        static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API 错误:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        //新增加入的聊天Id
        static void AddChatIds(long chatId)
        {
            var id = chatId.ToString();
            if (chatIds.Contains(chatId.ToString()) == false)
            {
                chatIds.Add(id);
            AddChatIds:
                try
                {
                    System.IO.File.AppendAllLines("chatIds.txt", [id]);
                }
                catch
                {
                    goto AddChatIds;
                }
            }
        }

        static HashSet<string> GetNotepadFilePaths(string rootDirectory, int depth)
        {
            HashSet<string> notepadFilePaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            GetNotepadFilePathsRecursive(rootDirectory, depth, notepadFilePaths);
            return notepadFilePaths;
        }
        static void GetNotepadFilePathsRecursive(string currentDirectory, int remainingDepth, HashSet<string> notepadFilePaths)
        {
            if (remainingDepth == 0)
                return;

            try
            {
                // 获取当前文件夹中的所有文件
                string[] files = Directory.GetFiles(currentDirectory, "*.txt");

                // 将记事本文件的路径添加到HashSet中
                foreach (string file in files)
                {
                    notepadFilePaths.Add(file);
                }

                // 获取当前文件夹中的所有子文件夹
                string[] subdirectories = Directory.GetDirectories(currentDirectory);

                // 递归调用每个子文件夹
                foreach (string subdirectory in subdirectories)
                {
                    GetNotepadFilePathsRecursive(subdirectory, remainingDepth - 1, notepadFilePaths);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // 处理无权访问的文件夹
                Console.WriteLine($"Access to {currentDirectory} is denied.");
            }
            catch (Exception ex)
            {
                // 处理其他异常
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        //删除别人信息
        public static async Task DeleteMessage(long chatId, int messageId, string text, int second)
        {
            Message? msg = null;
            try
            {
                msg = await botClient.SendTextMessageAsync(chatId, text, parseMode: ParseMode.Html, replyToMessageId: messageId);
            }
            catch (Exception e)
            {
                Console.WriteLine("他人发了不合规的商家搜索信息,告知对方时出错:" + e.Message);
            }

            if (msg == null)
                return;

            _ = Task.Run(async () =>
            {
                await Task.Delay(second * 1000);

                try
                {
                    await botClient.DeleteMessageAsync(chatId, messageId);
                }
                catch (Exception e)
                {
                    Console.WriteLine("删除他人商家搜索信息时出错:" + e.Message);
                }

                try
                {
                    await botClient.DeleteMessageAsync(chatId, msg.MessageId);
                }
                catch (Exception e)
                {
                    Console.WriteLine("他人发了不合规的商家搜索信息,删除信息时出错:" + e.Message);
                }
            });
        }

        //设置用户限制
        public static async Task<Operas> SetUserOperas(long userId)
        {
            //操作计数
            var operas = new Operas();

            var member = _users[userId];
            //有此用户
            if (member == null)
            {
                _users.Add(userId, new User { ViewTimes = [DateTime.Now] });
            }
            //无此用户
            else
            {
                member.ViewTimes.Add(DateTime.Now);
                operas.Todays = member.ViewTimes.Count(time => (DateTime.Now - time).TotalHours <= 24);
                operas.Weeks = member.ViewTimes.Count(time => (DateTime.Now - time).TotalDays <= 7);
                operas.Months = member.ViewTimes.Count(time => (DateTime.Now - time).TotalDays <= 30);
                operas.Totals = member.ViewTimes.Count;
            }

            await SaveConfig();
            return operas;
        }

        public static async Task SaveConfig()
        {
        writeUser:
            try
            {
                await System.IO.File.WriteAllTextAsync("Users.json", JsonConvert.SerializeObject(_users));
            }
            catch (Exception e)
            {
                Console.WriteLine("向本地写入限制用户时出错：" + e.Message);
                goto writeUser;
            }

        writeMerchant:
            try
            {
                await System.IO.File.WriteAllTextAsync("Merchant.json", JsonConvert.SerializeObject(_citys));
            }
            catch (Exception e)
            {
                Console.WriteLine("向本地写入商家时出错：" + e.Message);
                goto writeMerchant;
            }
        }

        //转换时间格式
        static string FormatTimeSpan(TimeSpan? timeSpan)
        {
            return string.Format("{0:D2}:{1:D2}", timeSpan?.Hours, timeSpan?.Minutes);
        }

        //是否在营业时间内
        static string IsBusinessHours(TimeSpan startTime, TimeSpan endTime)
        {
            var currentDayTime = DateTime.Now.TimeOfDay;

            // 如果结束时间小于开始时间，说明跨越了午夜
            if (endTime < startTime)
            {
                if (currentDayTime >= startTime || currentDayTime <= endTime)
                {
                    return "(营业中)";
                }
                else
                {
                    return "(已打烊)";
                }
            }
            else
            {
                // 正常情况下比较开始时间和结束时间
                if (currentDayTime >= startTime && currentDayTime <= endTime)
                {
                    return "(营业中)";
                }
                else
                {
                    return "(已打烊)";
                }
            }

        }

        //获取上级目录名称 dep
        static string GetParentDirectory(string filePath)
        {
            // 使用Path.GetDirectoryName方法获取文件路径的上一级目录
            // Path.GetDirectoryName方法返回目录路径字符串，如果路径无效，则返回null。
            string parentDirectory = Path.GetDirectoryName(filePath);

            if (parentDirectory != null)
            {
                // 使用Path.GetFileName获取上一级目录的名称
                string parentDirectoryName = Path.GetFileName(parentDirectory);
                return parentDirectoryName;
            }
            else
            {
                // 处理无效路径的情况
                return "无效路径";
            }
        }
    }
}
