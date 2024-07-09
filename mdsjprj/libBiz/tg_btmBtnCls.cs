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
using prj202405.lib;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using prj202405.lib;
using prj202405.lib;
using prj202405.lib;
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
using static prj202405.timerCls;
using static mdsj.biz_other;
using static mdsj.clrCls;
using static libx.qryEngrParser;
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
                    jmp2end();
                }
                catch (Exception e)
                {
                    print_catchEx(nameof(evt_btm_btn_click), e); jmp2end();
                }

                return;
                //  tglib.bot_DeleteMessageV2(update.Message.Chat.Id, update.Message.MessageId, 9);
                //  tglib.bot_DeleteMessageV2(update.Message.Chat.Id, a.MessageId, 10);

            }

            callx(btm_btnClk_inCfg, update);
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
            Console.WriteLine(keyboardJson);
            return keyboard;
        }


        public static InlineKeyboardMarkup castJsonAarrToInlineKeyboardButtons(JArray ja)
        {

            List<List<InlineKeyboardButton>> lst = new List<List<InlineKeyboardButton>>();
            foreach (JObject jo1 in ja)
            {
                //  InlineKeyboardButton btn= InlineKeyboardButton.WithUrl(jo1..GetValue("btnname"), jo1.GetValue("url"));
                InlineKeyboardButton btn = InlineKeyboardButton.WithUrl(jo1["btnname"].ToString(), jo1["url"].ToString().Trim());
                lst.Add(new List<InlineKeyboardButton> { btn });


            }


            return new InlineKeyboardMarkup(lst);
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
            Console.WriteLine(JsonConvert.SerializeObject(message));
        }


        public static void evt_btm_btn_click_inPubgrp(Update update)
        {  //  ,
           //try
           //{
           //  await btm_btnClk_inCfg(update);
            callx(btm_btnClk_inCfg, update);
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
            //    Console.WriteLine(e);
            //}

            //todo reply
        }

        public static void evt_btm_btn_zhuliBenqunAsync(Update update)
        {
            string tips = "如果您是Telegram VIP会员,请为本群助力, 提升群功能! ";
            var btn = "🔥 点击助力本群";
            var grp = update.Message.Chat.Username;

            var url = $"https://t.me/boost/{grp}";
            InlineKeyboardMarkup InlineKeyboardMarkup1 = null;

            IEnumerable<InlineKeyboardButton> inlineKeyboardRow1 = [InlineKeyboardButton.WithUrl(text: btn, url)];
            Console.WriteLine(encodeJson(inlineKeyboardRow1));
            InlineKeyboardMarkup1 = new InlineKeyboardMarkup(inlineKeyboardRow1);
            var msgNew =   botClient.SendTextMessageAsync(
                                  update.Message.Chat.Id, tips,
                                  replyMarkup: InlineKeyboardMarkup1,
                                  replyToMessageId: update.Message.MessageId

                          ).Result;

            dltMsgDelay(update, msgNew);
        }


        private static SortedList ldHstbFromIni(string f)
        {
            List<SortedList> li = ormIni.qryV2(f);
            return li[0];
        }
        public static object convertExtWd2btnname(string extWd)
        {
            SortedList st = ldHstbFromIni($"{prjdir}/cfg/底部按钮扩展词.ini");
            SortedList st2 = arr_ReverseSortedList(st);
            return ldfld(st2, extWd, "");
        }

        public static string getBtnExtWdFromTxt(string? text)
        {
            HashSet<string> st = LdHsstWordsFromFile($"{prjdir}/cfg/底部按钮扩展词.ini");
            return (containRetMatchWd(text, st));
        }

        public static object getBtnnameFromTxt(string? text)
        {
            HashSet<string> st = LdHsstWordsFromFile($"{prjdir}/menu/底部公共菜单.txt");
            return (containRetMatchWd(text, st));

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
            callx(btm_btnClk_inCfgByMsg, update, msg);
        }




        public static void btm_btnClk_inCfgByMsg
            (Update update, string msg)
        {
            Telegram.Bot.Types.Message msgNew = null;
            string f1 = $"{prjdir}/cfg_btmbtn/{msg}.json";
            //  print_varDump(, " Exists(f1)"+f1)
            if (System.IO.File.Exists(f1))
            {
                print_varDump(nameof(btm_btnClk_inCfgByMsg), " Exists f", f1);
                SortedList table = ReadAsHashtable(f1);


                var tips = table["tips"].ToString() + $"\n\n{plchdTxt}";
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
                    InlineKeyboardMarkup1 = castJsonAarrToInlineKeyboardButtons(btns);
                }

                try
                {
                    msgNew = botClient.SendTextMessageAsync(
                                              update.Message.Chat.Id, tips,
                                              replyMarkup: InlineKeyboardMarkup1,
                                              replyToMessageId: update.Message.MessageId,
                                               parseMode: ParseMode.Html
                                      ).Result;
                }
                catch (Exception e)
                {
                    print_catchEx(nameof(btm_btnClk_inCfgByMsg), e);
                    jmp2end();

                    return;
                }

                //aop  auth where exprs
                if(update?.Message?.Chat?.Type!=ChatType.Private)
                     dltMsgDelay(update, msgNew);
                jmp2end();

                return;

            }

        }
    }
}
