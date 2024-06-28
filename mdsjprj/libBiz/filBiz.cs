global using static mdsj.libBiz.filBiz;
using DocumentFormat.OpenXml.ExtendedProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdsj.libBiz
{
    internal class filBiz
    {

        public static HashSet<string> file_getWords商品与服务词库()
        {
            HashSet<string> 商品与服务词库 = ReadWordsFromFile("商品与服务词库.txt");
            商品与服务词库.Remove("店"); 商品与服务词库.Remove("针");
            商品与服务词库.Remove("处"); 商品与服务词库.Remove("院");


            RemoveSingleCharacterWords(商品与服务词库);
            RemoveWordsFromHashSet(商品与服务词库, " 档 湘");
            return RemoveEmptyElements(ConvertToUpperCase(商品与服务词库));
        }

        static void RemoveWordsFromHashSet(HashSet<string> words, string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return;

            string[] wordsToRemove = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in wordsToRemove)
            {
                if (word.Trim().Length > 0)
                    words.Remove(word);
            }
        }
        public static HashSet<string> RemoveEmptyElements(HashSet<string> words)
        {
            var wordsToRemove = new List<string>();

            foreach (var word in words)
            {
                if (string.IsNullOrWhiteSpace(word))
                {
                    wordsToRemove.Add(word);
                }
            }

            foreach (var word in wordsToRemove)
            {
                words.Remove(word);
            }
            return words;
        }
        public static void RemoveSingleCharacterWords(HashSet<string> words)
        {
            var wordsToRemove = new List<string>();

            foreach (var word in words)
            {
                if (word.Length == 1)
                {
                    wordsToRemove.Add(word);
                }
            }

            foreach (var word in wordsToRemove)
            {
                words.Remove(word);
            }
        }
    }
}
