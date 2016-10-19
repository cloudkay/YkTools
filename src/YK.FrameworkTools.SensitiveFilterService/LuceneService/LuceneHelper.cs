using Lucene.Net.Analysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.FrameworkTools.SensitiveFilterService
{
    /// <summary>
    /// 分词辅助类
    /// </summary>
    public class LuceneHelper
    {
        internal static WordTreeHelper WordTree= new FSWordTreeHelper("");

        /// <summary>
        /// 根据句子分词
        /// </summary>
        /// <param name="strVocabulary">句子</param>
        /// <param name="fieldName">字段名称，可为空</param>
        /// <returns></returns>
        public static List<string> AnalysisVocabulary(string strVocabulary, string fieldName = "LuceneVocabulary")
        {
            if (string.IsNullOrEmpty(strVocabulary))
            {
                return null;
            }
            else
            {
                List<string> result = new List<string>();
                Analyzer analyzer = new YKChineseAnalyzer();

                TokenStream tokenStream = analyzer.TokenStream(fieldName, new System.IO.StringReader(ReplaceFH(strVocabulary)));
                var termAttr = tokenStream.GetAttribute<Lucene.Net.Analysis.Tokenattributes.ITermAttribute>();


                while (tokenStream.IncrementToken())
                {
                    result.Add(termAttr.Term);
                }

                return result;
            }
        }

        /// <summary>
        /// 多个句子分词
        /// </summary>
        /// <param name="dictVocabularys">句子集合</param>
        /// <returns>分词结果</returns>
        public static List<string> AnalysisVocabulary(Dictionary<string,string> dictVocabularys)
        {
            if (dictVocabularys == null || dictVocabularys.Count == 0)
            {
                return null;
            }
            else
            {
                List<string> result = new List<string>();
                Analyzer analyzer = new YKChineseAnalyzer();

                foreach(var dictVocabulary in dictVocabularys)
                {
                    TokenStream tokenStream = analyzer.TokenStream(dictVocabulary.Key, new System.IO.StringReader(ReplaceFH(dictVocabulary.Value)));
                    var termAttr = tokenStream.GetAttribute<Lucene.Net.Analysis.Tokenattributes.ITermAttribute>();

                    while (tokenStream.IncrementToken())
                    {
                        result.Add(termAttr.Term);
                    }
                }                

                return result;
            }
        }

        private static string ReplaceFH(string value)
        {
            value = value.Replace(",", "").Replace(".", "").Replace("。", "").Replace("，", "").Replace("!", "").Replace("！", "").Replace("?", "").Replace("？", "").Replace(" ", "").Trim();
            return value;
        }
    }
}
