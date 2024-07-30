
global using static mdsj.libBiz.cmdHdlr;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.ExtendedProperties;
using mdsj.lib;
using MusicApiCollection.Sites.MusicBrainz.Data;
using prjx.lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace mdsj.libBiz
{
    internal class cmdHdlr
    {
        private const string tipsSetOK = "设置ok,你已经设置服务区域，如需重新设置区域请点击 /clearArea";
        public static string tipsSelectArea = "请选择服务区域";
        public static string tipsAppendArea = "继续新增区域国家/城市/园区";
        public static string cancelAddArea = "取消新增区域";
        public static string noTrigSrchMsgs = $"{tipsSelectArea},{tipsAppendArea},{cancelAddArea}";
        public static void CmdHdlr设置园区(string cmdFulltxt, Update update, string reqThreadId)
        {
            ///设置园区 东风园区
          //  if (cmdFulltxt.StartsWith("/设置园区"))
            {
                var prm_park = SubstrAfterMarker(cmdFulltxt, "/设置园区");

                SetPark(prm_park, update);

            }

            botClient.SendTextMessageAsync(
                update.Message.Chat.Id,
                "设置ok",
                parseMode: ParseMode.Html,

                protectContent: false,
                disableWebPagePreview: true,
                replyToMessageId: update.Message.MessageId);
            Jmp2endDep();

        }


        //public static void OnCmdPublic(string cmdFulltxt, Update update, string reqThreadId)
        //{
        //    string prjdir = @"../../../";
        //    prjdir = filex.GetAbsolutePath(prjdir);






        //}

        /// <summary>
        ///    string svrPksHtml = "";
        /// </summary>
        /// <param name="ctry">泰国</param>
        /// <param name="update"></param>
        /// <exception cref="NotImplementedException"></exception>
        public static void ConfirmSetCountryBtnClick(string ctry, Update update)
        {
            //public 判断权限先
            var grpid = update.Message.Chat.Id;
            var fromUid = update.Message.From.Id;
            var mybotid = botClient.BotId;
            //   string f = $"botEnterGrpLog/{grpid}.{fromUid}.{util.botname}.json";
            string f = $"{prjdir}/db/botEnterGrpLog/inGrp{grpid}.u{fromUid}.addBot.{util.botname}.json";
            int svrPrks = 0;
            string svrPksHtml = "";
            if (isGrpChat(update))
            {

                if (!IsAdmin(update))
                {
                    //send
                    Print("no auth ");
                    // print("no auth ");
                    //botClient.SendTextMessageAsync(update.Message.Chat.Id,
                    //      "权限不足", replyToMessageId: update.Message.MessageId);
                    bot_DeleteMessageV2(update.Message.Chat.Id, update.Message.MessageId, 3); ;

                    Jmp2end(nameof(ConfirmSetCountryBtnClick));
                }
            }
            // if (isGrpChat(update))
            {
                string dbfile2 = $"{prjdir}/grpCfgDir/grpcfg{grpid}.json";
                SortedList cfg = findOne(dbfile2);
                string pk = ctry.Trim().ToUpper();

                string parks = GetParksByCountry(ctry);
                string newParks = AppendParks(cfg, parks);

                AppendArea(ctry, cfg);
                SetField(cfg, "园区", newParks);
                string allParks = GetFieldAsStr(cfg, "园区");
                svrPrks = allParks.Split(",").Length;
                svrPksHtml = Join(allParks.Split(","), "\n");
                //   "\n"+newParks.Split(",").Join("\n");
                SetField(cfg, "id", grpid.ToString());
                SetField(cfg, "grpid", grpid.ToString());
                SetField(cfg, "chatid", grpid.ToString());
                SetField(cfg, "chat", update.Message.Chat);//here pe know pub prvt

                SetField(cfg, "from", update.Message.From);

                SetField(cfg, "grpinfo", update.Message.Chat);
                if (pk == "不限制")
                {
                    DelField(cfg, "国家", "");
                    DelField(cfg, "城市", "");
                    DelField(cfg, "园区", "");
                    SetField(cfg, "whereExprs", $"");
                }
                ormJSonFL.SaveJson(cfg, dbfile2);
            }
            //prive
            if (!isGrpChat(update))
            {
                string dbfile = $"{prjdir}/cfg_prvtChtPark/{update.Message.From.Id}.json";
                SortedList cfgPrvt = findOne(dbfile);
                string parks2 = GetParksByCountry(ctry);
                string newParks2 = AppendParks(cfgPrvt, parks2);


                SetField(cfgPrvt, "园区", newParks2);
                svrPrks = newParks2.Split(",").Length;
                SetField(cfgPrvt, "id", update.Message.From.Id.ToString());
                SetField(cfgPrvt, "from", update.Message.From);
                //   SetField(cfg, "whereExprs", newParks);
                if (ctry == "不限制")
                {

                    DelField(cfgPrvt, "国家", "");
                    DelField(cfgPrvt, "城市", "");
                    DelField(cfgPrvt, "园区", "");
                    SetField(cfgPrvt, "whereExprs", $"");
                }
                ormJSonFL.SaveJson(cfgPrvt, dbfile);
            }
            Message m = botClient.SendTextMessageAsync(
                update.Message.Chat.Id,
                tipsSetOK + "\n 已经设置园区数量：" + svrPrks + "\n" + svrPksHtml,
                parseMode: ParseMode.Html,

                protectContent: false,
                disableWebPagePreview: true,
                replyToMessageId: update.Message.MessageId).GetAwaiter().GetResult();
            bot_DeleteMessageV2(update.Message.Chat.Id, update.Message.MessageId, 3);
            bot_DeleteMessageV2(update.Message.Chat.Id, m?.MessageId, 3);
            //------------set menu btm
            Message m2 = SetBtmMenu(update);
            bot_DeleteMessageV2(update.Message.Chat.Id, (m2?.MessageId), 3);
            Jmp2endDep();
        }



        public static void BtmEvtSetParkMsgHdlrPre(Update update, string txt307)
        {

            if (!txt307.StartsWith(PreCh))
                return;


            var area = SubStr(txt307, 2);
            if (!IsPark(area))
                return;

            var park149 = SubStr(txt307, 2);
            var pks = LoadHashsetReadFileLinesToHashSet($"{prjdir}/cfg_cmd/园区列表.txt");
            if (txt307.StartsWith(PreCh) && pks.Contains(park149))
            {
                //if (isGrpChat(update))
                //{
                //    //auth chk
                //}
                Callx(SetParkBtnClick, park149, update);
                Jmp2end(nameof(BtmEvtSetParkMsgHdlrPre));
            }
        }

        public static void BtmEvtSetCityMsgHdlr(ITelegramBotClient botClient, Update update, string txt307)
        {
            if (!IsSetAreaBtnname(txt307))
                return;
            var city = SubStr(txt307, 2);
            var ctry = SubStr(txt307, 2);
            if (ctry.StartsWith("确定设置城市为"))
            {
                var ctry158 = SubstrAfterMarker(ctry, "确定设置城市为");
                ctry158 = ctry158.Trim();
                ConfirmSetCityBtnClick(ctry158, update);
                Jmp2endDep();
            }
            if (IsFileExist($"{prjdir}/cfg_cmd/{city}园区.txt"))
            {
                KeyboardButton[][] btns = ConvertFileToKeyboardButtons(
                    $"{prjdir}/cfg_cmd/{city}园区.txt");
                Print(EncodeJson(btns));
                var rplyKbdMkp = new ReplyKeyboardMarkup(btns);
                rplyKbdMkp.ResizeKeyboard = true;

                string imgPath = "今日促销商家.gif";
                var Photo2 = InputFile.FromStream(System.IO.File.OpenRead(imgPath));
                //  Message message2dbg = await 
                var m = botClient.SendTextMessageAsync(
                                update.Message.Chat.Id, "选择园区",
                                parseMode: ParseMode.Html,
                                replyMarkup: rplyKbdMkp,
                                protectContent: false, disableWebPagePreview: true).GetAwaiter().GetResult();

                bot_DeleteMessageV2(update.Message.Chat.Id, update.Message.MessageId, 120);
                bot_DeleteMessageV2(update.Message.Chat.Id, m.MessageId, 120);
                Print(m);
                Jmp2end(nameof(BtmEvtSetCityMsgHdlr));
            }

            if (IsSetAreaBtnname(txt307) && ISCity(city) &&
          IsNotExistFil($"{prjdir}/cfg_cmd/{city}园区.txt"))
            {
                Print("暂无配置");
                //  Message message2dbg = await 
                var m = botClient.SendTextMessageAsync(
                                update.Message.Chat.Id, "暂无配置",
                                parseMode: ParseMode.Html,
                                //  replyMarkup: rplyKbdMkp,
                                protectContent: false, disableWebPagePreview: true).GetAwaiter().GetResult();


                bot_DeleteMessageV2(update.Message.Chat.Id, update.Message.MessageId, 120);
                bot_DeleteMessageV2(update.Message.Chat.Id, m.MessageId, 120);
                Print(m);
                Jmp2end(nameof(BtmEvtSetCityMsgHdlr));


            }

        }


        public static void BtmEvtSetCountry(ITelegramBotClient botClient, Update update, string txt307)
        {
            var __Mthd = nameof(BtmEvtSetCountry);
            //  Jmp2end(nameof(BtmEvtSetCountry));
            jmp2endCurFunInThrd.Value = nameof(BtmEvtSetCountry);
            if (!IsSetAreaBtnname(txt307))
                return;


            var ctry = SubStr(txt307, 2);


            if (ctry.StartsWith("确定设置国家为"))
            {
                var ctry158 = SubstrAfterMarker(ctry, "确定设置国家为");
                ctry158 = ctry158.Trim();
                ConfirmSetCountryBtnClick(ctry158, update);
                Jmp2end(__Mthd);
            }

            try
            {
                //--is not country ,end fun
                ifStrutsThrdloc.Value = NewIFAst();
                iff(Condt(ISCtry, ctry), null, () =>
                {
                    //for prm use ret val mode
                    Jmp2endCurFunFlag.Value = true;
                    // Jmp2endCurFun();
                });
                if (Jmp2endCurFunFlag.Value)
                {
                    //  PrintRet(nameof(BtmEvtSetCountry), "");
                    return;
                }
                //todo      jmp2endCurFunEx also shold uper to catch ,not here..
            }
            catch (Exception jmp2endCurFunEx)
            {
                PrintRet(nameof(BtmEvtSetCountry), "");
                return;
            }



            iff(Condt(IsFileExist, $"{prjdir}/cfg_cmd/{ctry}城市.txt"), () =>
            {

                KeyboardButton[][] btns = ConvertFileToKeyboardButtons($"{prjdir}/cfg_cmd/{ctry}城市.txt");
                Print(EncodeJson(btns));
                var rplyKbdMkp = new ReplyKeyboardMarkup(btns);
                rplyKbdMkp.ResizeKeyboard = true;

                string imgPath = "今日促销商家.gif";
                var Photo2 = InputFile.FromStream(System.IO.File.OpenRead(imgPath));
                //  Message message2dbg = await 
                var m = botClient.SendTextMessageAsync(
                                update.Message.Chat.Id, "选择城市",
                                parseMode: ParseMode.Html,
                                replyMarkup: rplyKbdMkp,
                                protectContent: false, disableWebPagePreview: true).GetAwaiter().GetResult();

                Print(m);
                bot_DeleteMessageV2(update.Message.Chat.Id, update.Message.MessageId, 120);
                bot_DeleteMessageV2(update.Message.Chat.Id, m.MessageId, 120);
                Jmp2end(nameof(BtmEvtSetCountry));
            });

            //iff(Condt(IsSetAreaBtnname, txt307)
            //    && Condt(ISCtry, ctry)
            //       && Condt(IsExistFil, $"{prjdir}/cfg_cmd/{ctry}城市.txt"),
            //    () => {
            //        Print("THEN➡️➡️");


            //    },
            //     () => {
            //         Print("ELSE☑️");
            //         Print("暂无配置");

            //     }
            //);

            iff(Condt(IsSetAreaBtnname, txt307)
               && Condt(ISCtry, ctry)
                   && Condt(IsNotExistFil, $"{prjdir}/cfg_cmd/{ctry}城市.txt"), () =>
                   {
                       Print("暂无配置");
                       //  Message message2dbg = await 
                       var m = botClient.SendTextMessageAsync(
                                       update.Message.Chat.Id, "暂无配置",
                                       parseMode: ParseMode.Html,
                                       //  replyMarkup: rplyKbdMkp,
                                       protectContent: false, disableWebPagePreview: true).GetAwaiter().GetResult();

                       Print(m);
                       bot_DeleteMessageV2(update.Message.Chat.Id, update.Message.MessageId, 120);
                       bot_DeleteMessageV2(update.Message.Chat.Id, m.MessageId, 120);
                       Jmp2endDep();


                   });
        }



        public static void BtmEvtSetAreaHdlrChk(Update? update)
        {


            //--------------------shezhi 国家指令
            string txt307 = GetStr(update?.Message?.Text);
            if (txt307.StartsWith("❌"))
            {
                string park = Substring(txt307, 1);
                DelParkBtmbtnEvt(park, update);
                Jmp2end(nameof(BtmEvtSetAreaHdlrChk));
            }
            if (update.CallbackQuery?.Data != null)
            {
                string cmd = update.CallbackQuery?.Data;
                if (cmd == "testpopbx")
                {
                    botClient.AnswerCallbackQueryAsync(
                 callbackQueryId: update.CallbackQuery.Id,
                 text: ReadAllText("testpopbx.txt"),
                 showAlert: true); // 这是显示对话框的关键);
                    return;
                }

            }
            if (txt307 == "testpopbx")
            {
                botClient.AnswerCallbackQueryAsync(
                callbackQueryId: update.CallbackQuery.Id,
                text: "",
                showAlert: true); // 这是显示对话框的关键);
                return;
            }

            if (txt307.StartsWith("请选择本群所在区域"))
                return;
            if (txt307.StartsWith(tipsSelectArea))
                return;
            if (txt307.StartsWith(tipsAppendArea))
                return;
            if (txt307.StartsWith("取消新增区域"))
                return;
            if (txt307.StartsWith(PreCh + "取消"))
            {
                if (IsSetArea(update))
                {
                    SetBtmBtnMenu("今日促销商家.gif", plchdTxt, update.Message.Chat.Id, update.Message.Chat.Type.ToString());

                }
                else
                {
                    SetBtmBtnMenuClr("", plchdTxt, update.Message.Chat.Id, update.Message.Chat.Type.ToString());
                }
                bot_DeleteMessageV2(update.Message.Chat.Id, update.Message.MessageId, 3); ;

                Jmp2end(nameof(BtmEvtSetAreaHdlrChk));
            }

            if (txt307.StartsWith("/"))
                return;
            if (!IsSetAreaBtnname(txt307))
                return;

            Callx(BtmEvtSetCountry, botClient, update, txt307);
            //-------shezhi 城市指令
            //  string txt307 = GetStr(update?.Message?.Text);
            Callx(BtmEvtSetCityMsgHdlr, botClient, update, txt307);

            //------setpark   BtmEvtSetParkMsgHdlr
            BtmEvtSetParkMsgHdlrPre(update, txt307);
        }

        private static void DelParkBtmbtnEvt(string park, Update? update)
        {
            //public 判断权限先
            var chtid434 = update.Message.Chat.Id;
            var fromUid = update.Message.From.Id;
            var mybotid = botClient.BotId;
            int svrPrks = 0;
            string f = $"{prjdir}/db/botEnterGrpLog/inGrp{chtid434}.u{fromUid}.addBot.{util.botname}.json";

            if (isGrpChat(update))
            {

                if (!IsAdmin(update))
                {
                    //send
                    Print("no auth ");
                    // print("no auth ");
                    //botClient.SendTextMessageAsync(update.Message.Chat.Id,
                    //      "权限不足", replyToMessageId: update.Message.MessageId);
                    bot_DeleteMessageV2(update.Message.Chat.Id, update.Message.MessageId, 3); ;

                    Jmp2end(nameof(ConfirmSetCityBtnClick));
                }
            }
            string svrPksHtml = "";
           // if (isGrpChat(update))
            {
                string dbfile2 = $"{prjdir}/grpCfgDir/grpcfg{chtid434}.json";
                SortedList cfg = findOne(dbfile2);


                string whereExprsFld = "whereExprs";
                string areakey = "城市";
                string parks = GetFieldAsStr(cfg, "园区");

                string newParks = DelElmt(park, parks);

                svrPrks = newParks.Split(",").Length;
                DelField(cfg, "国家");
                DelField(cfg, "城市");
                SetField(cfg, "园区", newParks);
                SetField(cfg, "whereExprs", $"园区="+ newParks);
                SetField(cfg, "id", chtid434.ToString());
                SetField(cfg, "grpid", chtid434.ToString());
                SetField(cfg, "grpinfo", update.Message.Chat);
                //if (pk == "不限制")
                //{
                //    SetField(cfg, "国家", "");
                //    SetField(cfg, "城市", "");
                //    SetField(cfg, "园区", "");
                //    SetField(cfg, "whereExprs", $"");
                //}
                ormJSonFL.SaveJson(cfg, dbfile2);
                svrPksHtml = GetFieldAsStr(cfg, "园区");

            }
            //prive
            if (!isGrpChat(update))
            {
           //     return;
                //string dbfile = $"{prjdir}/cfg_prvtChtPark/{update.Message.From.Id}.json";
                //SortedList cfg = findOne(dbfile);
                //string whereExprsFld = "whereExprs";
                //string parks = GetParksByCity(area);
                //string newParks = AppendParks(cfg, parks);
                //svrPrks = newParks.Split(",").Length;
                //DelField(cfg, "国家");
                //DelField(cfg, "城市");
                ////     SetField(cfg, "园区", whereExprsNew);
                //SetField(cfg, "id", update.Message.From.Id.ToString());
                //SetField(cfg, "from", update.Message.From);
                ////    SetField(cfg, "whereExprs", $"园区={newParks}");
                //if (area == "不限制")
                //{
                //    SetField(cfg, "城市", "");
                //    SetField(cfg, "园区", "");
                //    SetField(cfg, "whereExprs", $"");
                //}
                //ormJSonFL.SaveJson(cfg, dbfile);
            }
            Message m = botClient.SendTextMessageAsync(
                 update.Message.Chat.Id,
                    tipsSetOK + "\n 删除成功,目前园区为\n" + svrPksHtml,
                 parseMode: ParseMode.Html,

                 protectContent: false,
                 disableWebPagePreview: true,
                 replyToMessageId: update.Message.MessageId).GetAwaiter().GetResult();
            bot_DeleteMessageV2(update.Message.Chat.Id, m.MessageId, 300);
            //------------set menu btm
            Message m2 = SetBtmMenu(update);
            bot_DeleteMessageV2(update.Message.Chat.Id, (m2?.MessageId), 3);
            Jmp2end(nameof(DelParkBtmbtnEvt));
        }

        private static void AppendArea(string ctry, SortedList cfg)
        {
            string areaArr = GetFieldAsStrDep(cfg, "area地区");
            string areaArr22 = AppendStrcomma(ctry, areaArr);
            SetField(cfg, "area地区", areaArr22);
        }

        public static string AppendStrcomma(string ctry, string areas)
        {
            if (areas == "")
                areas = ctry;
            else
                areas = areas + "," + ctry;


            return removeDulip(areas);
        }

        public static void ConfirmSetCityBtnClick(string area, Update update)
        {   //public 判断权限先
            var grpid = update.Message.Chat.Id;
            var fromUid = update.Message.From.Id;
            var mybotid = botClient.BotId;
            int svrPrks = 0;
            string f = $"{prjdir}/db/botEnterGrpLog/inGrp{grpid}.u{fromUid}.addBot.{util.botname}.json";

            if (isGrpChat(update))
            {

                if (!IsAdmin(update))
                {
                    //send
                    Print("no auth ");
                    // print("no auth ");
                    //botClient.SendTextMessageAsync(update.Message.Chat.Id,
                    //      "权限不足", replyToMessageId: update.Message.MessageId);
                    bot_DeleteMessageV2(update.Message.Chat.Id, update.Message.MessageId, 3); ;

                    Jmp2end(nameof(ConfirmSetCityBtnClick));
                }
            }
            string allParks = "";
            string svrPksHtml = "";
           // if (isGrpChat(update))
            {
                string dbfile2 = $"{prjdir}/grpCfgDir/grpcfg{grpid}.json";
                SortedList cfg = findOne(dbfile2);
                string pk = area.Trim().ToUpper();

                string whereExprsFld = "whereExprs";
                string areakey = "城市";
                string parks = GetParksByCity(area);
                string newParks = AppendParks(cfg, parks);
                AppendArea(area, cfg);
                svrPrks = newParks.Split(",").Length;
                DelField(cfg, "国家");
                DelField(cfg, "城市");
                SetField(cfg, "园区", newParks);
                SetField(cfg, "id", grpid.ToString());
                SetField(cfg, "grpid", grpid.ToString());
                SetField(cfg, "grpinfo", update.Message.Chat);

                //all same cfg
                SetField(cfg, "chatid", grpid.ToString());
                SetField(cfg, "chat", update.Message.Chat);//here pe know pub prvt
                SetField(cfg, "from", update.Message.From);
                //end allsame cfg

                allParks = GetFieldAsStr(cfg, "园区");
                svrPksHtml = Join(allParks.Split(","), "\n");
                svrPksHtml = AddIdxToElmt(svrPksHtml.Split("\n"), "\n");


                if (pk == "不限制")
                {
                    SetField(cfg, "国家", "");
                    SetField(cfg, "城市", "");
                    SetField(cfg, "园区", "");
                    SetField(cfg, "whereExprs", $"");
                }
                ormJSonFL.SaveJson(cfg, dbfile2);
            }
            //prive
            if (!isGrpChat(update))
            {
                string dbfile = $"{prjdir}/cfg_prvtChtPark/{update.Message.From.Id}.json";
                SortedList cfg = findOne(dbfile);
                string whereExprsFld = "whereExprs";
                string parks = GetParksByCity(area);
                string newParks = AppendParks(cfg, parks);
                svrPrks = newParks.Split(",").Length;
                DelField(cfg, "国家");
                DelField(cfg, "城市");
                //     SetField(cfg, "园区", whereExprsNew);
                SetField(cfg, "id", update.Message.From.Id.ToString());
                SetField(cfg, "from", update.Message.From);
                //    SetField(cfg, "whereExprs", $"园区={newParks}");
                if (area == "不限制")
                {
                    SetField(cfg, "城市", "");
                    SetField(cfg, "园区", "");
                    SetField(cfg, "whereExprs", $"");
                }
                ormJSonFL.SaveJson(cfg, dbfile);
            }
            Message m = botClient.SendTextMessageAsync(
               update.Message.Chat.Id,
                  tipsSetOK + "\n 已经设置园区：" + "\n" + svrPksHtml,

               parseMode: ParseMode.Html,

               protectContent: false,
               disableWebPagePreview: true,
               replyToMessageId: update.Message.MessageId).GetAwaiter().GetResult();
            bot_DeleteMessageV2(update.Message.Chat.Id, update.Message.MessageId, 300);
            bot_DeleteMessageV2(update.Message.Chat.Id, m.MessageId, 300);
            //------------set menu btm
            Message m2 = SetBtmMenu(update);
            bot_DeleteMessageV2(update.Message.Chat.Id, (m2?.MessageId), 3);

            Jmp2endDep();
        }







        private static string AppendParks(SortedList cfg, string parks
        )
        {
            string whereExprs = GetFieldAsStrDep(cfg, "whereExprs");
            SortedList whereExpmap = GetHashtableFromQrystr(whereExprs);
            DelField(whereExpmap, "国家");
            DelField(whereExpmap, "城市");

            string ormParks = GetFieldAsStrDep(whereExpmap, "园区");
            string newParks = ormParks + "," + parks;
            newParks = removeDulip(newParks);

            SetField(whereExpmap, "园区", newParks);
            DelField(cfg, "国家");
            DelField(cfg, "城市");

            string whereExprsNew = CastHashtableToQuerystringNoEncodeurl(whereExpmap);
            SetField(cfg, "whereExprs", whereExprsNew);
            return newParks;
        }



        //private static string SetMapWhereexprsFldOFkv(SortedList cfg, string whereExprsFld, string key2, string v2
        //    )
        //{
        //    string whereExprs = GetFieldAsStr(cfg, whereExprsFld);
        //    SortedList whereExpmap = GetHashtableFromQrystr(whereExprs);
        //    SetField(whereExpmap, "园区", v2);

        //    SetField(whereExpmap, "国家", "");
        //    SetField(whereExpmap, "城市", "");

        //    string whereExprsNew = CastHashtableToQuerystringNoEncodeurl(whereExpmap);
        //    return whereExprsNew;
        //}


        public static void SetParkBtnClick(string park, Update update)
        {
            //public 判断权限先
            var grpid = update.Message.Chat.Id;
            var fromUid = update.Message.From.Id;
            var mybotid = botClient.BotId;


            if (isGrpChat(update))
            {

                if (!IsAdmin(update))
                {
                    //send
                    Print("no auth ");
                    // print("no auth ");
                    //botClient.SendTextMessageAsync(update.Message.Chat.Id,
                    //      "权限不足", replyToMessageId: update.Message.MessageId);
                    bot_DeleteMessageV2(update.Message.Chat.Id, update.Message.MessageId, 3); ;

                    Jmp2end(nameof(SetParkBtnClick));
                }
            }
            //   string f = $"botEnterGrpLog/{grpid}.{fromUid}.{util.botname}.json";
            string allParks = SetPark(park, update);
            string svrPksHtml = Join(allParks.Split(","), "\n");
            svrPksHtml = AddIdxToElmt(svrPksHtml.Split("\n"), "\n");
            Message m = botClient.SendTextMessageAsync(
              update.Message.Chat.Id,
                tipsSetOK + "\n 已经设置园区：" + "\n" + svrPksHtml,
              parseMode: ParseMode.Html,

              protectContent: false,
              disableWebPagePreview: true,
              replyToMessageId: update.Message.MessageId).GetAwaiter().GetResult();
            bot_DeleteMessageV2(update.Message.Chat.Id, update.Message.MessageId, 120);
            bot_DeleteMessageV2(update.Message.Chat.Id, m.MessageId, 120);
            //---------------set menu
            Message m2 = SetBtmMenu(update);
            bot_DeleteMessageV2(update.Message.Chat.Id, (m2?.MessageId), 3);
            Jmp2end(nameof(SetParkBtnClick));

        }

        public static string AddIdxToElmt(string[] items, string spltr)
        {
            int n = 1;
            // 使用索引和元素创建新的字符串数组
            string[] indexedItems = new string[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                indexedItems[i] = $"{i + 1}.{items[i]}";
            }

            // 用回车符连接所有元素
            return string.Join(spltr, indexedItems);
        }

        private static Message SetBtmMenu(Update update)
        {
            ReplyKeyboardMarkup rplyKbdMkp;
            //  Program.botClient.send
            try
            {
                string chtType = update.Message.Chat.Type.ToString();
                //私聊不要助力本群
                if (!chtType.Contains("group"))
                {
                    rplyKbdMkp = tgBiz.tg_btmBtns();
                    KeyboardButton[][] kbtns = (KeyboardButton[][])rplyKbdMkp.Keyboard;
                    RemoveButtonByName(kbtns, juliBencyon);
                }
                else
                {
                    rplyKbdMkp = tgBiz.tg_btmBtns();
                }


                //def is grp btns
                //tgBiz.tg_btmBtns()
                var Photo2 = InputFile.FromStream(System.IO.File.OpenRead("今日促销商家.gif"));
                //  Message message2dbg = await 
                Message m = botClient.SendTextMessageAsync(
                 update.Message.Chat.Id, plchdTxt,
                     parseMode: ParseMode.Html,
                    replyMarkup: rplyKbdMkp,
                    protectContent: false, disableWebPagePreview: true).GetAwaiter().GetResult();
                return m;
                //  print(JsonConvert.SerializeObject(message2));

                //Program.botClient.SendTextMessageAsync(
                //         Program.groupId,
                //         "活动商家",
                //         parseMode: ParseMode.Html,
                //         replyMarkup: new InlineKeyboardMarkup(results),
                //         protectContent: false,
                //         disableWebPagePreview: true);

            }
            catch (Exception ex) { Print(ex.ToString()); return null; }

        }

        public static string SetPark(string park, Update update)
        {
            string allParks = "";
            park = CastToEnglishCharPunctuation(park);
            //public 判断权限先
            var grpid = update.Message.Chat.Id;
            var fromUid = update.Message.From.Id;
            var mybotid = botClient.BotId;
            //   string f = $"botEnterGrpLog/{grpid}.{fromUid}.{util.botname}.json";
            string f = $"{prjdir}/db/botEnterGrpLog/inGrp{grpid}.u{fromUid}.addBot.{util.botname}.json";
            if (isGrpChat(update))
            {
                if (!IsAdmin(update))
                {
                    Print("no auth " + f);
                    // print("no auth ");
                    //       botClient.SendTextMessageAsync(
                    //update.Message.Chat.Id,
                    //"权限不足",
                    //replyToMessageId: update.Message.MessageId);
                    bot_DeleteMessageV2(update.Message.Chat.Id, update.Message.MessageId, 3); ;

                    Jmp2endDep();
                    return "";
                }
            }
            //auth chk ok ,,or  privete 
            string svrPksHtml = "";
                //   string dbfile2 = $"{prjdir}/cfg_grp/{grpid}.json";
                string dbfile2 = $"{prjdir}/grpCfgDir/grpcfg{grpid}.json";
                SortedList cfg = findOne(dbfile2);

                //  string wehreExprs = GetFieldAsStr(cfg, "whereExprs");
                //  SortedList whereMap = GetHashtableFromQrystr(wehreExprs);
                //    string oriparks = GetFieldAsStr(whereMap, "园区");
                string newParks = AppendParks(cfg, park);
                string pk = park.Trim().ToUpper();
                AppendArea(pk, cfg);
                SetField(cfg, "园区", newParks);
                SetField(cfg, "id", grpid.ToString());
                SetField(cfg, "chatid", grpid.ToString());
                SetField(cfg, "chatinfo", update.Message.Chat);
                SetField(cfg, "grpid", grpid.ToString());
                SetField(cfg, "grpinfo", update.Message.Chat);

                //all same cfg
                SetField(cfg, "chatid", grpid.ToString());
                SetField(cfg, "chat", update.Message.Chat);//here pe know pub prvt
                SetField(cfg, "from", update.Message.From);
                //end allsame cfg

                allParks = GetFieldAsStr(cfg, "园区");
                //   svrPrks = allParks.Split(",").Length;
                svrPksHtml = Join(allParks.Split(","), "\n");
                if (pk == "不限制")
                {
                    SetField(cfg, "园区", "");
                    SetField(cfg, "whereExprs", $"");
                }
                ormJSonFL.SaveJson(cfg, dbfile2);
          

            if (!isGrpChat(update))
            {
                SetParkPrvt(park, update);
            }

            return allParks;


        }

        private static void SetParkPrvt(string park, Update update)
        {
            string dbfile = $"{prjdir}/cfg_prvtChtPark/{update.Message.From.Id}.json";
            SortedList cfg = findOne(dbfile);
            string pk = park.Trim().ToUpper();
            string newParks = AppendParks(cfg, park);


            SetField(cfg, "园区", newParks);
            DelField(cfg, "国家");
            //     SetField(cfg, "whereExprs", $"园区={newParks}");
            DelField(cfg, "城市");
            SetField(cfg, "园区", pk);
            SetField(cfg, "id", update.Message.From.Id.ToString());
            SetField(cfg, "from", update.Message.From);
            if (pk == "不限制")
            {
                SetField(cfg, "园区", "");
                SetField(cfg, "whereExprs", $"");
            }
            ormJSonFL.SaveJson(cfg, dbfile);
        }



        //public static void On我是谁Supergroup(Update update, string reqThreadId)
        //{
        //    Print("我是打游戏");
        //}
        //public static void On设置城市supergroup(Update update, string reqThreadId)
        //{
        //    Print("oo617");
        //}

        public static void CmdHdlrclearArea(string fullcmd, Update update, string reqThreadId)
        {

            //public 判断权限先
            var grpid = update.Message.Chat.Id;
            var fromUid = update.Message.From.Id;
            var mybotid = botClient.BotId;

            string f = $"{prjdir}/db/botEnterGrpLog/inGrp{grpid}.u{fromUid}.addBot.{util.botname}.json";

            if (isGrpChat(update))
            {

                if (!IsAdmin(update))
                {
                    //send
                    Print("no auth ");
                    // print("no auth ");
                    //botClient.SendTextMessageAsync(update.Message.Chat.Id,
                    //      "权限不足", replyToMessageId: update.Message.MessageId);
                    bot_DeleteMessageV2(update.Message.Chat.Id, update.Message.MessageId, 3); ;

                    Jmp2end(nameof(CmdHdlrclearArea));
                }
            }

         //   if (isGrpChat(update))
            {
                string dbfile2 = $"{prjdir}/grpCfgDir/grpcfg{grpid}.json";
                SortedList cfg = findOne(dbfile2);
                string whereExprsFld = "whereExprs";
                string areakey = "城市";

                DelField(cfg, "国家", "");
                DelField(cfg, "城市", "");


                SetField(cfg, "园区", "");

                SetField(cfg, "whereExprs", "");
                SetField(cfg, "id", grpid.ToString());
                SetField(cfg, "grpid", grpid.ToString());
                SetField(cfg, "grpinfo", update.Message.Chat);
                ormJSonFL.SaveJson(cfg, dbfile2);
            }


            //prive
            if (!isGrpChat(update))
            {
                string dbfile = $"{prjdir}/cfg_prvtChtPark/{update.Message.From.Id}.json";
                SortedList cfg = findOne(dbfile);
                string whereExprsFld = "whereExprs";

                DelField(cfg, "国家");
                DelField(cfg, "城市");
                DelField(cfg, "园区");
                SetField(cfg, "id", update.Message.From.Id.ToString());
                SetField(cfg, "from", update.Message.From);
                SetField(cfg, "whereExprs", "");

                ormJSonFL.SaveJson(cfg, dbfile);
            }


            Message m = botClient.SendTextMessageAsync(
            update.Message.Chat.Id,
            "清理成功",
            parseMode: ParseMode.Html,

            protectContent: false,
            disableWebPagePreview: true,
            replyToMessageId: update.Message.MessageId).GetAwaiter().GetResult();

            bot_DeleteMessageV2(update.Message.Chat.Id, update.Message.MessageId, 3);
            bot_DeleteMessageV2(update.Message.Chat.Id, update.Message.MessageId, 120);
            Jmp2end(nameof(CmdHdlrclearArea));
        }

        /// <summary>
        ///   string svrPksHtml = "";   svrPksHtml = GetFieldAsStr(cfg, "园区");
        ///     bot_DeleteMessageV2(update.Message.Chat.Id, m.MessageId,20);
        /// </summary>
        /// <param name="fullcmd"></param>
        /// <param name="update"></param>
        /// <param name="reqThreadId"></param>
        public static void CmdHdlrshowArea(string fullcmd, Update update, string reqThreadId)
        {
            //public 判断权限先
            var grpid = update.Message.Chat.Id;
            var fromUid = update.Message.From.Id;
            var mybotid = botClient.BotId;
            string svrPksHtml = "";
            if (isGrpChat(update))
            {
                string dbfile2 = $"{prjdir}/grpCfgDir/grpcfg{grpid}.json";
                SortedList cfg = findOne(dbfile2);



                svrPksHtml = GetFieldAsStr(cfg, "园区");


            }
            //prive
            if (!isGrpChat(update))
            {
                return;
                string dbfile = $"{prjdir}/cfg_prvtChtPark/{update.Message.From.Id}.json";
                SortedList cfg = findOne(dbfile);
                //    string parks = GetParksByCountry(ctry);
                //    string newParks = AppendParks(cfg, parks);



            }
            Message m = botClient.SendTextMessageAsync(
                 update.Message.Chat.Id,
                 tipsSetOK + "\n 已经设置园区" + "\n" + svrPksHtml,
                 parseMode: ParseMode.Html,

                 protectContent: false,
                 disableWebPagePreview: true,
                 replyToMessageId: update.Message.MessageId).GetAwaiter().GetResult();

            //------------set menu btm
            //   SetBtmMenu(update);
            bot_DeleteMessageV2(update.Message.Chat.Id, update.Message.MessageId, 120);
            bot_DeleteMessageV2(update.Message.Chat.Id, m.MessageId, 20);
            Jmp2endDep();

        }
        public static void CmdHdlrdelArea(string fullcmd, Update update, string reqThreadId)
        {

            //public 判断权限先
            var grpid = update.Message.Chat.Id;
            var fromUid = update.Message.From.Id;
            var mybotid = botClient.BotId;


            if (isGrpChat(update))
            {

                if (!IsAdmin(update))
                {
                    //send
                    Print("no auth ");
                    // print("no auth ");
                    //botClient.SendTextMessageAsync(update.Message.Chat.Id,
                    //      "权限不足", replyToMessageId: update.Message.MessageId);
                    bot_DeleteMessageV2(update.Message.Chat.Id, update.Message.MessageId, 3); ;
                    Jmp2end(nameof(CmdHdlrdelArea));
                }
            }
            string svrPksHtml = "";
         //   if (isGrpChat(update))
            {
                string dbfile2 = $"{prjdir}/grpCfgDir/grpcfg{grpid}.json";
                SortedList cfg = findOne(dbfile2);



                string nowPks = GetFieldAsStr(cfg, "园区");
                nowPks = AddCharFrontToElmt("❌", nowPks);


                KeyboardButton[][] btns = ConvertFileToKeyboardButtons(nowPks.Split(","));
                Print(EncodeJson(btns));
                var rplyKbdMkp = new ReplyKeyboardMarkup(btns);
                string imgPath = "今日促销商家.gif";
                var Photo2 = InputFile.FromStream(System.IO.File.OpenRead(imgPath));
                //  Message message2dbg = await 

                var m = botClient.SendTextMessageAsync(
                                update.Message.Chat.Id, "选择要删除的园区",
                                parseMode: ParseMode.Html,
                                replyMarkup: rplyKbdMkp,
                                protectContent: false, disableWebPagePreview: true).GetAwaiter().GetResult();
                bot_DeleteMessageV2(update.Message.Chat.Id, update.Message.MessageId, 120);
                bot_DeleteMessageV2(update.Message.Chat.Id, m.MessageId, 120);
                bot_DeleteMessageV2(update.Message.Chat.Id, update.Message.MessageId, 120);
                Print(m);
                Jmp2end(nameof(CmdHdlrdelArea));
            }
        }


        public static void CmdHdlrarea(string fullcmd, Update update, string reqThreadId)
        {

            if (isGrpChat(update))
            {

                if (!IsAdmin(update))
                {
                    //send
                    Print("no auth ");
                    // print("no auth ");
                    //botClient.SendTextMessageAsync(update.Message.Chat.Id,
                    //      "权限不足", replyToMessageId: update.Message.MessageId);
                    bot_DeleteMessageV2(update.Message.Chat.Id, update.Message.MessageId, 3); ;
                    Jmp2end(nameof(CmdHdlrarea));
                }
            }

            var tipsd = tipsSelectArea;

            if (IsSetArea(update))
            {
                if (isGrpChat(update))
                {
                    var grpid = update.Message.Chat.Id;
                    var fromUid = update.Message.From.Id;
                    var mybotid = botClient.BotId;
                    string dbfile2 = $"{prjdir}/grpCfgDir/grpcfg{grpid}.json";
                    SortedList cfg = findOne(dbfile2);


                    string area = GetFieldAsStrDep(cfg, "area地区");
                    tipsd = $"你已经选择了地区：{area} 。";
                    tipsd += tipsAppendArea;
                }
                else

                    tipsd = tipsAppendArea;
            }



            string cmd = GetCmdFun(update?.Message?.Text);
            //  Print("oo617");
            string filePath = $"{prjdir}/cfg_cmd/{cmd}.txt";
            string[] lines = ReadFileAndRemoveEmptyLines(filePath);
            lines = Replace(lines, "{tipsd}", tipsAppendArea);


            KeyboardButton[][] btns = ConvertFileToKeyboardButtons(lines);
            Print(EncodeJson(btns));
            var rplyKbdMkp = new ReplyKeyboardMarkup(btns);
            string imgPath = "今日促销商家.gif";
            var Photo2 = InputFile.FromStream(System.IO.File.OpenRead(imgPath));
            //  Message message2dbg = await 

            var m = botClient.SendTextMessageAsync(
                            update.Message.Chat.Id, tipsd,
                            parseMode: ParseMode.Html,
                            replyMarkup: rplyKbdMkp,
                            protectContent: false, disableWebPagePreview: true).GetAwaiter().GetResult();

            bot_DeleteMessageV2(update.Message.Chat.Id, update.Message.MessageId, 120);
            bot_DeleteMessageV2(update.Message.Chat.Id, m.MessageId, 120);
            Print(m);
            // PrintRet(nameof(CmdHdlrarea), "");
            //---settimeout 
            NewThrd(() =>
            {
                Thread.Sleep(300 * 1000);
                SetBtmBtnMenu("今日促销商家.gif", plchdTxt, update.Message.Chat.Id, update.Message.Chat.Type.ToString());
            });
            jmp2endCurFunInThrd.Value = nameof(CmdHdlrarea);
            Jmp2end(nameof(CmdHdlrarea));
        }


        public static bool IsSetArea(Update? update)
        {
            var grpid = update.Message.Chat.Id;
            var fromUid = update.Message.From.Id;
            var mybotid = botClient.BotId;
            string dbfile2 = $"{prjdir}/grpCfgDir/grpcfg{grpid}.json";
            SortedList cfg = findOne(dbfile2);


            string whereExprsFld = "whereExprs";
            string areakey = "城市";
            if (IsEmpty(GetFieldAsStrDep(cfg, whereExprsFld)))
                return false;
            return true;

            //    string whereExprsNew = SetMapWhereexprsFldOFkv(cfg, whereExprsFld, areakey, area);

        }






        public static void OnCmdPrvt(string cmdFulltxt, Update update, string reqThreadId)
        {

            string prjdir = @"../../../";
            prjdir = filex.GetAbsolutePath(prjdir);
            string dbfile = $"{prjdir}/cfg_prvtChtPark/{update.Message.From.Id}.json";


            //私聊消息  /start开始
            if (cmdFulltxt == "/start")
            {
                CallUserFunc409(evt_startMsgEvtInPrvtAddBot, update);
                return;
            }

            ///设置园区 东风园区
            if (cmdFulltxt.StartsWith("/设置园区"))
            {
                var park = SubstrAfterMarker(cmdFulltxt, "/设置园区");

                SortedList cfg = findOne(dbfile);
                string pk = park.Trim().ToUpper();
                SetField(cfg, "园区", pk);
                SetField(cfg, "id", update.Message.From.Id.ToString());
                SetField(cfg, "from", update.Message.From);
                if (pk == "不限制")
                {
                    SetField(cfg, "园区", "");
                    SetField(cfg, "whereExprs", $"");
                }
                ormJSonFL.SaveJson(cfg, dbfile);



            }
            /////设置城市 妙瓦底
            //if (cmdFulltxt == "/设置城市")
            //{
            //    var park = SubstrAfterMarker(cmdFulltxt, "/设置城市");
            //    SortedList cfg = findOne(dbfile);
            //    SetField(cfg, "城市", park.Trim().ToUpper());
            //    SetField(cfg, "id", update.Message.From.Id.ToString());
            //    SetField(cfg, "from", update.Message.From);

            //    ormJSonFL.SaveJson(cfg, dbfile);

            //}

            botClient.SendTextMessageAsync(
              update.Message.Chat.Id,
              "设置ok",
              parseMode: ParseMode.Html,

              protectContent: false,
              disableWebPagePreview: true,
              replyToMessageId: update.Message.MessageId);

        }
        public static void evt_startMsgEvtInPrvtAddBot(Update update)
        {
            botClient.SendTextMessageAsync(
                    update.Message.Chat.Id,
                    "请直接搜索园区/城市+商家/菜单即可,比如”金三角 会所”!\n"
                    + "\n可以设置园区方便搜索，指令如下:\n"
                    + "/设置园区 东风园区\n",
                    //  + "/设置园区 不限制\n",
                    parseMode: ParseMode.Html,
                    //   replyMarkup: new InlineKeyboardMarkup([]),
                    protectContent: false,
                    disableWebPagePreview: true);

            tglib.bot_saveChtSesion(update.Message.Chat.Id, update.Message.From);

            //   tmrEvt_sendMsg4keepmenu("今日促销商家.gif", plchdTxt));

            var rplyKbdMkp = tgBiz.tg_btmBtns();
            KeyboardButton[][] kbtns = (KeyboardButton[][])rplyKbdMkp.Keyboard;
            RemoveButtonByName(kbtns, juliBencyon);

            //  var Photo2 = InputFile.FromStream(System.IO.File.OpenRead(imgPath));
            long chatid = update.Message.Chat.Id;
            //  Message message2dbg = await 
            botClient.SendTextMessageAsync(
         Convert.ToInt64(chatid), plchdTxt,
             parseMode: ParseMode.Html,
            replyMarkup: rplyKbdMkp,
            protectContent: false, disableWebPagePreview: true);
        }


    }
}
