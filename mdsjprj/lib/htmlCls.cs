global using static mdsj.lib.htmlCls;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace mdsj.lib
{
    internal class htmlCls
    {
        public static string htm_strip_tags(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return string.Empty;
            }


            // Remove <script> tags and their content
            string noScript = Regex.Replace(html, "<script.*?>.*?</script>", string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            // Regular expression to match HTML tags
            string pattern = "<.*?>";

            // Replace HTML tags with an empty string
            //    string result = Regex.Replace(noScript, pattern, string.Empty);
            // Replace HTML tags with an empty string
            string result = Regex.Replace(noScript, pattern, string.Empty, RegexOptions.Singleline);

            result = RemoveExtraNewlines(result);
            result = RemoveInvisibleCharacters(result);
            result = delEmpltyLines(result);

            result = RemoveExtraNewlines(result); result = RemoveExtraNewlines(result);
            //result = RemoveExtraNewlines(result); result = RemoveExtraNewlines(result);
            return result;
        }

        static void RemoveCustomEmojiRendererElement(string inputFilePath, string outputFilePath)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.Load(inputFilePath);

            // 在这里添加你的代码来去除 custom-emoji-renderer-element 标签
            // 这里提供一个示例来移除所有的 custom-emoji-renderer-element 标签
            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//custom-emoji-renderer-element"))
            {
                node.ParentNode.RemoveChild(node);
            }

            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//custom-emoji-element"))
            {
                node.ParentNode.RemoveChild(node);
            }

            doc.Save(outputFilePath);
        }
    }
}
