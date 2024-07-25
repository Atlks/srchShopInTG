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
            HashSet<string> 商品与服务词库 = GetHashset商品与服务词库();
            商品与服务词库.Remove("店"); 商品与服务词库.Remove("针");
            商品与服务词库.Remove("处"); 商品与服务词库.Remove("院");


            RemoveSingleCharacterWords(商品与服务词库);
            RemoveWordsFromHashSet(商品与服务词库, " 档 湘");

            HashSet<string> hashSet = RemoveEmptyElements(ConvertToUpperCase(商品与服务词库));

            HashSet<string> hashSetBlklst = ReadFileToHashSet($"{prjdir}/cfg/fuwuciBlklst.txt");
            ConvertToUpperCase(hashSetBlklst);


            return RemoveEmptyElements(removeByHashSets(hashSet, hashSetBlklst));
        }

        public static HashSet<string> GetHashset商品与服务词库()
        {
            return ReadWordsFromFile($"{prjdir}/cfg/商品与服务词库.txt");
        }

     public   static HashSet<string> removeByHashSets(HashSet<string> set1, HashSet<string> set2)
        {
            // 创建一个新的 HashSet 以保留 set1 的副本
            HashSet<string> result = new HashSet<string>(set1);

            // 从 result 中移除 set2 中的所有元素
            result.ExceptWith(set2);

            return result;
        }
        public static List<string> RemoveEmptyElements(List<string> words)
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
