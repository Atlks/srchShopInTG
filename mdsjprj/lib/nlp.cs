global using static mdsj.lib.nlp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Porter2Stemmer;

//using PorterStemmerAlgorithm;
namespace mdsj.lib
{
    internal class nlp
    {
        //static string GetStem(string word)
        //{
        //    PorterStemmer stemmer = new PorterStemmer();
        //    return stemmer.StemWord(word);
        //}

     public   static string GetRoot(string word)
        {
            // 创建一个 Porter2Stemmer 实例
            var stemmer = new EnglishPorter2Stemmer();

           //  stemme
            // 获取单词的原始形式（词干）
            var stem = stemmer.Stem(word) ;
            return stem.Value.ToString();
          //  stemmer.
           // return stemmer.FindRoot(word);
        }
    }
}
