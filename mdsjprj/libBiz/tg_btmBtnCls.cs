global using static mdsj.libBiz.tg_btmBtnCls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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
using prjx.lib;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using prjx.lib;
using prjx.lib;
using prjx.lib;
using JiebaNet.Segmenter;
using System.Xml;
using HtmlAgilityPack;
using Formatting = Newtonsoft.Json.Formatting;
using DocumentFormat.OpenXml;
using mdsj;
using System.Runtime.Intrinsics.Arm;
using Microsoft.Extensions.Primitives;
using System.Runtime.CompilerServices;
using mdsj;
using mdsj.libBiz;

using DocumentFormat.OpenXml.Bibliography;
using mdsj.lib;

using static mdsj.lib.afrmwk;
using static mdsj.lib.util;
using static libx.storeEngr4Nodesqlt;
using static prjx.timerCls;
using static mdsj.biz_other;
using static mdsj.clrCls;
using static libx.qryEngrParser;
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
using static mdsj.lib.afrmwk;

using static SqlParser.Ast.DataType;

using static SqlParser.Ast.CharacterLength;
using static mdsj.lib.avClas;
using static mdsj.lib.dtime;
using static mdsj.lib.fulltxtSrch;

using System.Net.Http.Json;
using DocumentFormat.OpenXml.Spreadsheet;

using System.Security.Policy;
using RG3.PF.Abstractions.Entity;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Windows.UI.Xaml;
using DocumentFormat.OpenXml.Vml.Spreadsheet;

namespace mdsj.libBiz
{
    public class tg_btmBtnCls
    {
        public static void evt_btm_btn_click(Update update)
        {


            Telegram.Bot.Types.Message a;
            var msg = bot_getTxt(update);
            //if (msg.Trim() == "🫂 加入联信")
            //{
            //todo zhege should del
            string f = $"{prjdir}/cfg_btnResp/{msg}.txt";
            if (System.IO.File.Exists(f))
            {
                try
                {
                    var tips2 = ReadAllText(f);
                    IEnumerable<InlineKeyboardButton> inlineKeyboardRow = [InlineKeyboardButton.WithUrl(text: "金娱科技招聘频道", $"https://t.me/JinYuKeJi")];
                    a = botClient.SendTextMessageAsync(
                            update.Message.Chat.Id,
                         tips2,
                            parseMode: ParseMode.Html,
                            replyMarkup: new InlineKeyboardMarkup(inlineKeyboardRow),
                            protectContent: false,
                            replyToMessageId: update.Message.MessageId,
                            disableWebPagePreview: true

                    ).Result;
                    Jmp2end(nameof(evt_btm_btn_click) + ".BLOCKcfg_btnResp");
                }
                catch (jmp2endEx ee)
                {
                    throw ee;
                }
                catch (Exception e)
                {
                    //other ext
                    PrintCatchEx(nameof(evt_btm_btn_click), e);
                    Jmp2end(nameof(evt_btm_btn_click) + ".BLOCKcfg_btnResp.BLKcatchOthEx");
                }






                return;
                //  tglib.bot_DeleteMessageV2(update.Message.Chat.Id, update.Message.MessageId, 9);
                //  tglib.bot_DeleteMessageV2(update.Message.Chat.Id, a.MessageId, 10);

            }

            Callx(btm_btnClk_inCfg, update);
            return;
            //  jmp2end();

        }

        public static InlineKeyboardMarkup daifubtns()
        { // Create the keyboard object

            // Create the first row of buttons
            var firstRow = new List<InlineKeyboardButton>();
            firstRow.Add(
                new InlineKeyboardButton("代购须知")
                {
                    Text = "代购须知",
                    CallbackData = "daigou"
                }
            );
            firstRow.Add(
                new InlineKeyboardButton("代收须知")
                {
                    Text = "代收须知",
                    CallbackData = "代收须知"
                }
            );
            firstRow.Add(
              new InlineKeyboardButton("代付须知")
              {
                  Text = "代付须知",
                  CallbackData = "代付须知"
              }
            );

            // Create the second row of buttons
            var secondRow = new List<InlineKeyboardButton>();
            secondRow.Add(
                new InlineKeyboardButton("联系代购/付客服")
                {
                    Text = "联系代购/付客服",
                    Url = "https://t.me/LianXin_ShangWu"
                }
            );




            // Add the rows of buttons to the keyboard
            var lstBtns = new List<List<InlineKeyboardButton>>();
            lstBtns.Add(firstRow);
            lstBtns.Add(secondRow);

            var keyboard = new InlineKeyboardMarkup(lstBtns);
            // Serialize the keyboard to JSON
            var keyboardJson = JsonConvert.SerializeObject(keyboard);

            // Print the JSON string to the console
            Print(keyboardJson);
            return keyboard;
        }


        public static async void evt_shiboBocai_click(Update? update)
        {
            //   RemoveCustomEmojiRendererElement("shiboRaw.htm", "shiboTrm.htm");

            string plchdTxt1422 = System.IO.File.ReadAllText($"{prjdir}/cfg/shibobc.txt");
            //"💁 联信与世博联盟正式达成长期战略合作，联信为世博联盟旗下所有盘口提供双倍担保，确保100%真实可靠。\r\n\r\n在娱乐过程中，如发现世博联盟存在杀客、不予提现、杀大赔小等违规行为，请立即向联信负责人及运营团队举报。经核实后，联信将对您在世博联盟里因世博盘口违规行为造成的损失给予双倍赔偿！";

            string imgPath = "推荐横幅.jpg";
            var Photo = InputFile.FromStream(System.IO.File.OpenRead(imgPath));


            InlineKeyboardButton[][] btns = ConvertHtmlLinksToTelegramButtons($"{prjdir}/cfg/shiboTrm.htm");
            Telegram.Bot.Types.Message message = await botClient.SendPhotoAsync(
                  update.Message.Chat.Id, Photo, null,
             plchdTxt1422,
             replyToMessageId: update.Message.MessageId,
                    parseMode: ParseMode.Html,
                     replyMarkup: new InlineKeyboardMarkup(btns),
                   protectContent: false); ;

            //ori 64
            //Message message = await Program.botClient.SendPhotoAsync(
            //        update.Message.Chat.Id, Photo, null,
            //      System.IO.File.ReadAllText("shiboTrm.htm"),
            //          parseMode: ParseMode.Html,
            //         //   replyMarkup: new InlineKeyboardMarkup(results),
            //         protectContent: false);
            Print(JsonConvert.SerializeObject(message));
        }

        //dep todo 
        public static void evt_btm_btn_click_inPubgrp(Update update)
        {  //  ,
           //try
           //{
           //  await btm_btnClk_inCfg(update);
            Callx(btm_btnClk_inCfg, update);
            //  await callxAsync(btm_btnClk, update);
            //}
            //catch (jmp2endEx e)
            //{
            //    return;
            //}
            //catch (Exception e)
            //{
            //    print_catchEx("evt_btm_btn_click_inPubgrp", e);
            //    //  return;
            //}



            //brch------other btm btn not in cfg
            //Telegram.Bot.Types.Message msgNew2 =  botClient.SendTextMessageAsync(
            //                   update.Message.Chat.Id,
            //                 "要获取多级菜单，请私聊我",
            //                   replyMarkup: new InlineKeyboardMarkup([InlineKeyboardButton.WithUrl(text: "私聊我", $"https://t.me/{botname}")]),
            //                      replyToMessageId: update.Message.MessageId
            //                   ).Result;



            //dltMsgDelay(update, msgNew2);
            //  jmp2exit();
            //}
            //catch (Exception e)
            //{
            //   print(e);
            //}

            //todo reply
        }
        public static string zhuli_tips = "如果您是Telegram VIP会员,请为本群助力, 提升群功能! ";
        public static string zhuli_btn = "🔥 点击助力本群";
        public static void evt_btm_btn_zhuliBenqunAsync(Update update)
        {

            var grpUsername = update.Message.Chat.Username;

            Message msgNew = sendZhuliGrp(update, zhuli_tips, zhuli_btn, grpUsername);

            dltMsgDelay(update, msgNew);
        }

        public static Message sendZhuliGrp(Update update, string tips, string btn, string? grpUsername)
        {
            var url = $"https://t.me/boost/{grpUsername}";
            InlineKeyboardMarkup InlineKeyboardMarkup1 = null;

            InlineKeyboardButton btn2 = InlineKeyboardButton.WithCallbackData(text: btn, "testpopbx");
            IEnumerable<InlineKeyboardButton> inlineKeyboardRow1 = [InlineKeyboardButton.WithUrl(text: btn, url)];
            Print(EncodeJson(inlineKeyboardRow1));
            InlineKeyboardMarkup1 = new InlineKeyboardMarkup(inlineKeyboardRow1);
            var msgNew = botClient.SendTextMessageAsync(
                                  update.Message.Chat.Id, tips,
                                  replyMarkup: InlineKeyboardMarkup1,
                                  replyToMessageId: update.Message.MessageId

                          ).Result;
            return msgNew;
        }
        public static Message sendZhuliGrp(object ChatId, string tips, string btn, string? grpUsername)
        {
            long ChatId2 = long.Parse(ChatId.ToString());
            var url = $"https://t.me/boost/{grpUsername}";
            InlineKeyboardMarkup InlineKeyboardMarkup1 = null;

            IEnumerable<InlineKeyboardButton> inlineKeyboardRow1 = [InlineKeyboardButton.WithUrl(text: btn, url)];
            Print(EncodeJson(inlineKeyboardRow1));
            InlineKeyboardMarkup1 = new InlineKeyboardMarkup(inlineKeyboardRow1);
            var msgNew = botClient.SendTextMessageAsync(
                                   ChatId2, tips,
                                  replyMarkup: InlineKeyboardMarkup1

                          ).Result;
            return msgNew;
        }
        private static SortedList ldHstbFromIni(string f)
        {
            List<SortedList> li = ormIni.qryV2(f);
            return li[0];
        }
        public static object convertExtWd2btnname(string extWd)
        {
            SortedList st = ldHstbFromIni($"{prjdir}/cfg/底部按钮扩展词.ini");
            SortedList st2 = ArrReverseSortedList(st);
            return LoadFieldFrmStlst(st2, extWd, "");
        }

        public static string getBtnExtWdFromTxt(string? text)
        {
            HashSet<string> st = LoadHashstWordsFromFile($"{prjdir}/cfg/底部按钮扩展词.ini");
            return (ContainRetMatchWd(text, st));
        }

        public static object getBtnnameFromTxt(string? text)
        {
            HashSet<string> st = LoadHashstWordsFromFile($"{prjdir}/menu/底部公共菜单.txt");
            return (ContainRetMatchWd(text, st));

        }

        public static void dltMsgDelay(Update update, Message msgNew)
        {
            tglib.bot_DeleteMessageV2(update.Message.Chat.Id, update.Message.MessageId, 121);
            tglib.bot_DeleteMessageV2(update.Message.Chat.Id, msgNew.MessageId, 120);
        }
        public static async System.Threading.Tasks.Task btm_btnClk(Update update)
        {
            //if (update?.Message?.Text == "代付")
            //{
            //    var tips = "联信为您提供各类暗网或网站代付、各个网购平台代购、各国话费充值服务!\n代购/收低于500人民币的，收50元，高于500的收取10%. 如需丢失保价需交产品价值的5%（只保丢失，不保快递破损摔坏）";
            //    Message msgNew = await Program.botClient.SendTextMessageAsync(
            //                 update.Message.Chat.Id, tips,
            //                 replyMarkup: daifubtns(),
            //                 replyToMessageId: update.Message.MessageId,
            //                  parseMode: ParseMode.Html
            //         );

            //    dltMsgDelay(update, msgNew);
            //    jmp2exit();

            //    return;
            //}
        }
        public const string juliBencyon = "🔥 助力本群";
        public static void btm_btnClk_inCfg(Update update)
        {

            var msg = bot_getTxt(update);
            //  btm_btnClk_inCfgV2(msg);
            Callx(BtmBtnClkinCfgByMsg, update, msg);
        }



        /// <summary>
        /// 底部按钮处理事件
        /// </summary>
        /// <param name="update"></param>
        /// <param name="msg"></param>
        public static void BtmBtnClkinCfgByMsg
            (Update update, string msg)
        {
            Telegram.Bot.Types.Message msgNew = null;

            //----商家在这里
            string f1 = $"{prjdir}/cfg_btmbtn/{msg}.json";
            //  print_varDump(, " Exists(f1)"+f1)
            if (System.IO.File.Exists(f1))
            {
                print_varDump(nameof(BtmBtnClkinCfgByMsg), " Exists f", f1);
                SortedList table = ReadAsHashtable(f1);
                var tips = table["tips"].ToString();
                if (tips.StartsWith("$"))
                {
                    string fl = $"{prjdir}/cfg_btmbtn/" + SubStr(tips, 1);
                    tips = ReadAllText(fl);
                }

                tips = tips + $"\n\n{plchdTxt}";
                InlineKeyboardMarkup InlineKeyboardMarkup1 = null;
                //  ReplyKeyboardMarkup
                if (table.ContainsKey("url"))
                {
                    IEnumerable<InlineKeyboardButton> inlineKeyboardRow1 = [InlineKeyboardButton.WithUrl(text: "打开目录", table["url"].ToString())];
                    InlineKeyboardMarkup1 = new InlineKeyboardMarkup(inlineKeyboardRow1);
                }
                else
                {
                    JArray btns = (JArray)table["btns"];
                    ProcessAppendParksQrystr(update, btns);
                    InlineKeyboardMarkup1 = castJsonAarrToInlineKeyboardButtons(btns);
                }

                try
                {
                    msgNew = SendPhotoV("推荐横幅.jpg", tips, InlineKeyboardMarkup1, update.Message.Chat.Id, replyToMessageId: update.Message.MessageId);
                    //msgNew = botClient.SendTextMessageAsync(
                    //                          update.Message.Chat.Id, tips,
                    //                          replyMarkup: InlineKeyboardMarkup1,
                    //                          replyToMessageId: update.Message.MessageId,
                    //                           parseMode: ParseMode.Html
                    //                  ).Result;
                }
                catch (Exception e)
                {
                    PrintCatchEx(nameof(BtmBtnClkinCfgByMsg), e);
                    Jmp2endDep();

                    return;
                }

                //aop  auth where exprs
                if (update?.Message?.Chat?.Type != ChatType.Private)
                    dltMsgDelay(update, msgNew);
                Jmp2endDep();

                return;

            }

            //商家
            string htmlf = $"{prjdir}/cfg_btmbtn/{msg}.htm";
            if (System.IO.File.Exists(htmlf))
            {
                print_varDump(nameof(BtmBtnClkinCfgByMsg), " Exists f", htmlf);
                string html = ReadAllText(htmlf);
                string jsonstr = ConvertHtmlToJson4tg(html);
                SortedList table = GetDicFromJson(jsonstr);
                var Img = table["Img"].ToString();
                var tips = table["Text"].ToString();
                if (tips.StartsWith("$"))
                {
                    string fl = $"{prjdir}/cfg_btmbtn/" + SubStr(tips, 1);
                    tips = ReadAllText(fl);
                }

                tips = tips + $"\n\n{plchdTxt}";
                InlineKeyboardMarkup InlineKeyboardMarkup1 = null;

                {
                    JArray btns = (JArray)table["btns"];
                    InlineKeyboardMarkup1 = castJsonAarrToInlineKeyboardButtonsV2(btns);
                }

                try
                {
                    msgNew = SendPhotoV("推荐横幅.jpg", tips, InlineKeyboardMarkup1, update.Message.Chat.Id, replyToMessageId: update.Message.MessageId);
                    //msgNew = botClient.SendTextMessageAsync(
                    //                          update.Message.Chat.Id, tips,
                    //                          replyMarkup: InlineKeyboardMarkup1,
                    //                          replyToMessageId: update.Message.MessageId,
                    //                           parseMode: ParseMode.Html
                    //                  ).Result;
                }
                catch (Exception e)
                {
                    PrintCatchEx(nameof(BtmBtnClkinCfgByMsg), e);
                    Jmp2endDep();

                    return;
                }

                //aop  auth where exprs
                if (update?.Message?.Chat?.Type != ChatType.Private)
                    dltMsgDelay(update, msgNew);
                Jmp2end(nameof(BtmBtnClkinCfgByMsg));

                return;

            }
        }
        public static string GetParkPath(string parkName, string jsonArrayStr)
        {
            // 解析 JSON 数据
            JArray jsonArray = JArray.Parse(jsonArrayStr);
            foreach (var item in jsonArray)
            {
                string path = FindParkPath(parkName, item);
                if (!string.IsNullOrEmpty(path))
                {
                    return path;
                }
            }
            return string.Empty;
        }

        private static string FindParkPath(string parkName, JToken token)
        {

         
            // 检查当前节点是否是目标园区
            if (token["name"]?.ToString() == parkName)
            {
                return token["id"]?.ToString();
            }

            // 递归查找子节点
            if (token["children"] != null)
            {
                foreach (var child in token["children"])
                {
                    string childPath = FindParkPath(parkName, child);
                    if (!string.IsNullOrEmpty(childPath))
                    {
                        return $"{token["id"]?.ToString()}_{childPath}";
                    }
                }
            }

            return string.Empty;
        }
        private static string GetParkPath(string jsonArrayStr)
        {
            // 解析 JSON 数据
            JArray jsonArray = JArray.Parse(jsonArrayStr);
            foreach (var item in jsonArray)
            {
                string path = Traverse(item);
                if (!string.IsNullOrEmpty(path))
                {
                    return path;
                }
            }
            return string.Empty;
        }

        private static string Traverse(JToken token)
        {
            // 基本检查
            if (token == null) return string.Empty;

            // 获取当前节点的 ID 和名称
            string id = token["id"]?.ToString();
            string name = token["name"]?.ToString();

            // 如果当前节点没有子节点，返回 ID
            if (token["children"] == null || !token["children"].HasValues)
            {
                return id;
            }

            // 递归遍历子节点
            List<string> paths = new List<string>();
            foreach (var child in token["children"])
            {
                string childPath = Traverse(child);
                if (!string.IsNullOrEmpty(childPath))
                {
                    paths.Add(childPath);
                }
            }

            // 选择路径最深的一个
            if (paths.Count > 0)
            {
                return $"{id}_{string.Join("_", paths)}";
            }
            return string.Empty;
        }
        private static void ProcessAppendParksQrystr(Update update, JArray btns)
        {
            foreach (JObject jo in btns)
            {
                try
                {
                    // 获取 URL 并转换为字符串
                    string url = jo.GetValue("url")?.ToString();
                    if (!url.Contains("startapp"))
                        break;
                    if (url == null)
                        break;

                    var grpid = update.Message.Chat.Id;
                    string dbfile2 = $"{prjdir}/grpCfgDir/grpcfg{grpid}.json";
                    SortedList cfg = findOne(dbfile2);
                    string pk = GetFieldAsStr(cfg, "园区");
                    string pks = pk.Replace(",", " ");
                    string[] pksa = pks.Split(" ");
                    HashSet<string> hs = NewHashsetStr();
                    string fnlPk = "";
                    foreach (string p in pksa)
                    {
                        Hashtable ht = GetHashtableFromJsonFl($"{prjdir}/webroot/parkcode.json"); ;
                        Hashtable ht2 = ReverseHashtable(ht);
                        string pkcode = ht2[p].ToString();
                        fnlPk = pkcode;
                        hs.Add(pkcode);
                    }
                    string pks938 = string.Join("_", hs);
                    string Fst = hs.Count > 0 ? hs.First() : string.Empty;

                    Print("pks938ss=>"+ pks938);
                    //   string token = newToken(ToStr(update.Message.From.Id), 3600 * 24 * 7);
                    // 替换占位符
                    // url = url.Replace("pks9381148", pks938);
                    url = url + pks938;
                    Print("url=>"+url);
                    // 更新 JObject
                    jo["url"] = url;

                }
                catch (Exception e)
                {
                    PrintCatchEx("BtmBtnClkinCfgByMsg", e);
                }

            }
            PrintObj(btns);
        }

        private static Hashtable ReverseHashtable(Hashtable hashtable)
        {
            Hashtable reversed = new Hashtable();

            foreach (DictionaryEntry entry in hashtable)
            {
                // 确保值在反转后唯一
                if (reversed.ContainsKey(entry.Value))
                {
                    throw new InvalidOperationException("Cannot reverse hashtable: Duplicate values found.");
                }

                reversed[entry.Value] = entry.Key;
            }

            return reversed;
        }
        private static Hashtable GetHashtableFromJsonFl(string jsonFilePath)
        {
            // 读取 JSON 文件内容
            string jsonContent = System.IO.File.ReadAllText(jsonFilePath);

            // 解析 JSON 为 Dictionary
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonContent);

            // 将 Dictionary 转换为 Hashtable
            Hashtable hashtable = new Hashtable();
            foreach (var kvp in dictionary)
            {
                hashtable[kvp.Key] = kvp.Value;
            }

            return hashtable;
        }

        private static HashSet<string> NewHashsetStr()
        {
            return new HashSet<string>();
        }
    }
}
