global using static mdsj.lib.translt;
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
        public static void transltTest()
        {
            List<string> liRzt = new List<string>();
            List<string> li = getListFrmFil($"{prjdir}/cfg/testart/t3.txt");
            li = RemoveEmptyElements(li);
            foreach_listStr(li, (string line) =>
            {
                string transed = transedE2Cn(line);
                liRzt.Add(transed);
            });
            string rzt = JoinStringsWithNewlines(liRzt);
            WriteAllText("transed.txt", rzt);
            print(rzt);
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
                if (IsWord(wd))
                {
                    if (StartsWithUppercase(wd))
                    {
                        string wdRoot = GetRoot(wd);
                        string cnWd = transE2cWd(wdRoot);
                        liRzt.Add(wd + " [" + cnWd + "]");
                    }
                    else
                    {//normal 
                        string wdRoot = GetRoot(wd);
                        string cnWd = transE2cWd(wdRoot);
                        liRzt.Add(wd + " [" + cnWd + " " + wdRoot + "]");
                    }

                }
                else
                {
                    //标点
                    liRzt.Add(wd);
                }

            }
            // 将 List<string> 转换为用空格隔开的字符串
            string result = String.Join(" ", liRzt);
            return result;

        }
        static bool StartsWithUppercase(string input)
        {
            // 判断字符串是否为空或为null
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            // 检查第一个字符是否为大写字母
            return char.IsUpper(input[0]);
        }
        static bool IsWord(string input)
        {
            // 使用 Char 类的方法判断是否是字母或数字
            return input.All(c => char.IsLetterOrDigit(c));
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
        static Hashtable dicWord5k = getHstbFromIniFl($"{prjdir}/cfg/word5000.ini");
        private static string transE2cWd(string wdRoot)
        {

            //     WriteAllText("wd5000.json", encodeJson(dic));
            // dic = ToLower(dic);
            return (string)ldfld(dicWord5k, wdRoot, wdRoot);
        }

        private static Hashtable ToLower(Hashtable dic)
        {
            ConvertKeysToLowercase(dic);
            return dic;
        }

        static void ConvertKeysToLowercase(Hashtable hashtable)
        {
            Hashtable newHashtable = new Hashtable();

            foreach (DictionaryEntry entry in hashtable)
            {
                string lowercaseKey = entry.Key.ToString().ToLower(); // 将键转换为小写
                newHashtable[lowercaseKey] = entry.Value; // 保持值不变，添加到新的 Hashtable 中
            }

            // 清空原始 Hashtable 并将新的键值对复制回去
            hashtable.Clear();
            foreach (DictionaryEntry entry in newHashtable)
            {
                hashtable[entry.Key] = entry.Value;
            }
        }

        private static Hashtable getHstbFromIniFl(string v)
        {
            return ReadIniFile(v);
        }

        static Hashtable ReadIniFile(string filePath)
        {
            Hashtable iniData = new Hashtable();

            // 按行读取 INI 文件内容
            string[] lines = ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();

                // 忽略空行和注释行
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith(";") || trimmedLine.StartsWith("#"))
                    continue;

                // 处理键值对行
                int equalIndex = trimmedLine.IndexOf('=');
                if (equalIndex > 0)
                {
                    string key = trimmedLine.Substring(0, equalIndex).Trim();
                    string value = trimmedLine.Substring(equalIndex + 1).Trim();
                    iniData[key] = value;
                }
            }

            return iniData;
        }

        public static string[] ReadAllLines(string filePath)
        {
            return System.IO.File.ReadAllLines(filePath);
        }

        public static void foreach_listStr(List<string> li, Action<string> value)
        {
            foreach (string line in li)
            {
                value(line);
            }
        }

        public static List<string> getListFrmFil(string v)
        {
            return ReadFileToLines(v);
        }

        public static List<string> ReadFileToLines(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines;
        }

        public static string JoinStringsWithNewlines(List<string> stringList)
        {
            if (stringList == null || stringList.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();
            foreach (string item in stringList)
            {
                builder.Append(item);
                builder.AppendLine();
            }

            // Remove the last newline character
            builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }
    }
}
