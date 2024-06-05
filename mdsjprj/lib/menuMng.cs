using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace mdsj.lib
{
    internal class menuMng
    {
        public static InlineKeyboardButton[][] wdsFromFileRendrToBtnmenu(string filePath)
        {
            // 创建一个 ArrayList 来存储所有的单词
            ArrayList wordList = new ArrayList();

            // 读取文件中的所有行
            string[] lines = System.IO.File.ReadAllLines(filePath);

            List<InlineKeyboardButton[]> btnTable = [];
            // 遍历每一行
            foreach (string line in lines)
            {
                // 按空格分割行，得到单词数组
                string[] words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                List<InlineKeyboardButton> lineBtnArr = [];
                // 将单词添加到 ArrayList 中
                foreach (string word in words)
                {
                    if (word.Trim().Length > 0)
                    {
                        wordList.Add(word);
                        // { CallbackData = $"Merchant?id={guid}" }
                        InlineKeyboardButton btn = new InlineKeyboardButton(word) { CallbackData = $"cmd={word}" };
                        lineBtnArr.Add(btn);
                    }

                }

                btnTable.Add(lineBtnArr.ToArray());
            }
            return btnTable.ToArray();
        }

        public static KeyboardButton[][] wdsFromFileRendrToTgBtmBtnmenuBycomma(string filePath)
        {
            // 创建一个 ArrayList 来存储所有的单词
            ArrayList wordList = new ArrayList();

            // 读取文件中的所有行
            string[] lines = System.IO.File.ReadAllLines(filePath);

            List<KeyboardButton[]> btnTable = [];
            // 遍历每一行
            foreach (string line in lines)
            {
                // 按空格分割行，得到单词数组
                string[] words = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                List<KeyboardButton> lineBtnArr = [];
                // 将单词添加到 ArrayList 中
                foreach (string word in words)
                {
                    if (word.Trim().Length > 0)
                    {
                        wordList.Add(word);
                        KeyboardButton btn = new KeyboardButton(word);
                        lineBtnArr.Add(btn);
                    }

                }

                btnTable.Add(lineBtnArr.ToArray());
            }
            return btnTable.ToArray();
        }

        public static KeyboardButton[][] wdsFromFileRendrToTgBtmBtnmenu(string filePath)
        {
            // 创建一个 ArrayList 来存储所有的单词
            ArrayList wordList = new ArrayList();

            // 读取文件中的所有行
            string[] lines = System.IO.File.ReadAllLines(filePath);

            List<KeyboardButton[]> btnTable = [];
            // 遍历每一行
            foreach (string line in lines)
            {
                // 按空格分割行，得到单词数组
                string[] words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                List<KeyboardButton> lineBtnArr = [];
                // 将单词添加到 ArrayList 中
                foreach (string word in words)
                {
                    if (word.Trim().Length > 0)
                    {
                        wordList.Add(word);
                        KeyboardButton btn = new KeyboardButton(word);
                        lineBtnArr.Add(btn);
                    }

                }

                btnTable.Add(lineBtnArr.ToArray());
            }
            return btnTable.ToArray();
        }
        public static InlineKeyboardButton[][] ConvertHtmlLinksToTelegramButtons(string filePath)
        {
            // 读取HTML文件内容
            var htmlContent = System.IO.File.ReadAllText(filePath);

            // 使用HtmlAgilityPack解析HTML内容
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);

            // 查找所有的<a>标签
            var linkNodes = htmlDoc.DocumentNode.SelectNodes("//a[@href]");

            if (linkNodes == null || !linkNodes.Any())
            {
                return Array.Empty<InlineKeyboardButton[]>();
            }

            // 将每个<a>标签转换为InlineKeyboardButton
            var buttons = linkNodes
                .Select(linkNode =>
                {
                    var url = linkNode.GetAttributeValue("href", string.Empty);
                    var text = linkNode.InnerText;
                    return InlineKeyboardButton.WithUrl(text, url);
                })
                .ToList();

            // 将按钮组织成一个二维数组，这里假设每行只有一个按钮
            //var buttonRows = buttons
            //    .Select(button => new[] { button })
            //    .ToArray();

            // 将按钮组织成一个二维数组，每行3个按钮
            var buttonRows = buttons
                .Select((button, index) => new { button, index })
                .GroupBy(x => x.index / 3)
                .Select(g => g.Select(x => x.button).ToArray())
                .ToArray();
            return buttonRows;
        }

        public static InlineKeyboardButton[] ConvertHtmlLinksToTelegramButtonsSnglRow(string filePath)
        {
            // 读取HTML文件内容
            var htmlContent = System.IO.File.ReadAllText(filePath);

            // 使用HtmlAgilityPack解析HTML内容
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);

            // 查找所有的<a>标签
            var linkNodes = htmlDoc.DocumentNode.SelectNodes("//a[@href]");

            if (linkNodes == null || !linkNodes.Any())
            {
                return Array.Empty<InlineKeyboardButton>();
            }

            // 将每个<a>标签转换为InlineKeyboardButton
            var buttons = linkNodes
                .Select(linkNode =>
                {
                    var url = linkNode.GetAttributeValue("href", string.Empty);
                    var text = linkNode.InnerText;
                    return InlineKeyboardButton.WithUrl(text, url);
                })
                .ToArray();

            return buttons;
        }


    }
}
