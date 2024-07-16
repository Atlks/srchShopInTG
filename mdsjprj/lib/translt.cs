global using static mdsj.lib.translt;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace mdsj.lib
{
    internal class translt
    {
        static Hashtable dicWord5k = getHstbFromIniFl("wdlib.enNcn5k.v2.ini");

        public static void transltTest()
        {
            List<string> liRzt = new List<string>();
         //   FolderPath = "downHtmldir";
            List<string> li = ldLstWdsFrmDataDirHtml("spdr/downHtmtTaskQue");
            List<string> li2 = ldLstWdsFrmDataDirHtml("spdr/downHtmldirLog");
            li = array_merge(li, li2);
            //  List<string> li = getListFrmFil($"{prjdir}/cfg/testart/t3.txt");
            li = RemoveEmptyElements(li);
            foreach_listStr(li, (string line) =>
            {
                string transed = transedE2Cn(line);
                liRzt.Add(transed);
            });
            //get misswdFmt.txt
            string rzt = JoinStringsWithNewlines(liRzt);
            WriteAllText("transed.txt", rzt);
            print(rzt);
        }

      //  private const string


        public static HashSet<string> ExtractWordsFromFilesHtml(string folderPath)
        {
            HashSet<string> words = new HashSet<string>();

            if (!Directory.Exists(folderPath))
            {
                print(" ..warning.... fld not exist: fun ExtractWordsFromFilesHtml " + folderPath);

                return words;
            }
              
            // 获取文件夹中的所有文件
            string[] files = Directory.GetFiles(folderPath);

            foreach (string file in files)
            {
                try
                {
                    // 读取文件内容
                    string content = System.IO.File.ReadAllText(file);
                    content = htm_strip_tags(content);
                    // 提取单词
                    var extractedWords = ExtractWords(content);

                    // 将提取的单词添加到 HashSet 中
                    foreach (var word in extractedWords)
                    {
                        var a = word.Split("-");
                        foreach (var wd in a)
                        {
                            var a2 = wd.Split("_");
                            foreach (var w3 in a2)
                            {
                               // if (w3.Length > 3)
                                    words.Add(w3);
                            }
                        }

                    }
                }catch(Exception e)
                {
                    print_catchEx(nameof(ExtractWordsFromFilesHtml),e);
                }
                
            }

            return words;
        }

        public static HashSet<string> ExtractWordsFromFiles(string folderPath)
        {
            HashSet<string> words = new HashSet<string>();

            // 获取文件夹中的所有文件
            string[] files = Directory.GetFiles(folderPath);

            foreach (string file in files)
            {
                // 读取文件内容
                string content = System.IO.File.ReadAllText(file);

                // 提取单词
                var extractedWords = ExtractWords(content);

                // 将提取的单词添加到 HashSet 中
                foreach (var word in extractedWords)
                {
                    var a = word.Split("-");
                    foreach (var wd in a)
                    {
                        var a2 = wd.Split("_");
                        foreach (var w3 in a2)
                        {
                            if (w3.Length > 3)
                                words.Add(w3);
                        }
                    }

                }
            }

            return words;
        }
      
        public static IEnumerable<string> ExtractWords(string text)
        {
            // 使用正则表达式提取单词
            MatchCollection matches = Regex.Matches(text, @"\b\w+\b");

            foreach (Match match in matches)
            {
                yield return match.Value.ToLower(); // 转换为小写以确保唯一性
            }
        }

        private static string transedE2Cn(string line)
        {
            List<string> liRzt = new List<string>();
            string[] wds = Tokenize(line);
            foreach (string wd in wds)
            {
                if (wd.Contains("sibl"))
                    print("dbg");
                if (wd.Contains("ontivero"))
                    print("dbg");
                transE2cn4wd(liRzt, wd);

            }
            // 将 List<string> 转换为用空格隔开的字符串
            string result = String.Join("    ", liRzt);
            return result;

        }

      public  static HashSet<string> hs_mswd = new HashSet<string>();

        private static void transE2cn4wd(List<string> liRzt, string wd)
        {
            if (IsNumeric(wd))
            { //数字
                liRzt.Add(wd);
                return;

            }
            if (!IsWord(wd))
            {
                // Or   标点
                //标点
                liRzt.Add(wd);
            }
            //is word
            if (StartsWithUppercase(wd))
            {
                string wdRoot = GetRoot(wd);
               // string cnWd = transE2cWd(wdRoot);
                liRzt.Add(wd + "");
                return;
            }

            //normal word
            if (wd.Length <= 3)
            {
                liRzt.Add(wd + "");
                return;
            }

            string cnWd = transE2cWd(wd);
            if (cnWd != "")
            {
                //普通单词找到对于翻译词了
                liRzt.Add(wd + "[" + cnWd + "." + wd + "]");
                return;
            }
            else
            {
                //非普通单词，找不到翻译词，尝试 使用词干法
                string wdRoot = GetRoot(wd);
                cnWd = transE2cWd(wdRoot);
                if(cnWd=="")
                {
                    //log miss wd
                    logF($"\n{wd}=\n{wdRoot}=","misswd.log");
                    //for misswd.log then trans fill to wdlib.ini
                    hs_mswd.Add(wd + ""); hs_mswd.Add(wdRoot + "");
                }
                liRzt.Add(wd + "[" + cnWd + "." + wdRoot + "]");
                return;
            }





        }

  
        static string[] Tokenize(string input)
        {
            // 定义正则表达式，匹配单词和标点符号
            string pattern = @"([\w']+|[^\w\s])";
            Regex regex = new Regex(pattern);

            // 使用正则表达式拆分输入字符串
            MatchCollection matches = regex.Matches(input);

            // 构建结果数组
            string[] tokens = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                tokens[i] = matches[i].Value;
            }

            return tokens;
        }
        private static string transE2cWd(string wdOrwdRoot)
        {

            //     WriteAllText("wd5000.json", encodeJson(dic));
            // dic = ToLower(dic);
            return (string)ldfld(dicWord5k, wdOrwdRoot, "");
        }

      

      
    
    }
}
