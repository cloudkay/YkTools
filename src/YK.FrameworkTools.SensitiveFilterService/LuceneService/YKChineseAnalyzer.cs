using System;
using System.IO;
using System.Text;
using System.Collections;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using System.Collections.Generic;

namespace YK.FrameworkTools.SensitiveFilterService
{
    /// <summary>
    /// An <see cref="Analyzer"/> that tokenizes text with <see cref="ChineseTokenizer"/> and
    /// filters with <see cref="ChineseFilter"/>
    /// </summary>
    public class YKChineseAnalyzer : Analyzer
    {
        /// <summary>
        /// ºöÂÔ´ÊÓï
        /// </summary>
        public static readonly ISet<string> CHINESE_ENGLISH_STOP_WORDS = new HashSet<string>();
        private static bool m_IsLoaded = false;

        /// <summary>
        /// Ë÷Òý·ÖÎöÆ÷
        /// </summary>
        public YKChineseAnalyzer()
        {
            if (!m_IsLoaded)
            {
                string filePath = "";
                if (System.Environment.CurrentDirectory.TrimEnd(new char[] { '\\' }) == AppDomain.CurrentDomain.BaseDirectory.TrimEnd(new char[] { '\\' }))
                {
                    filePath = AppDomain.CurrentDomain.BaseDirectory;
                }
                else
                {
                    filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
                }

                StreamReader streamReader = new StreamReader(filePath + @"\Dict\NoiseDict.txt", Encoding.UTF8);
                string text = streamReader.ReadLine();
                int num = 0;
                while (!string.IsNullOrEmpty(text))
                {
                    CHINESE_ENGLISH_STOP_WORDS.Add(text);
                    text = streamReader.ReadLine();
                    num++;
                }

                m_IsLoaded = true;
            }
        }

        /// <summary>
        /// Creates a TokenStream which tokenizes all the text in the provided Reader.
        /// </summary>
        /// <returns>A TokenStream build from a ChineseTokenizer filtered with ChineseFilter.</returns>
        public override sealed TokenStream TokenStream(String fieldName, TextReader reader)
        {
            TokenStream tokenStream = new YKChineseTokenizer(reader);
            tokenStream = new StandardFilter(tokenStream);
            return new StopFilter(true, tokenStream, YKChineseAnalyzer.CHINESE_ENGLISH_STOP_WORDS, true);
        }

        private class SavedStreams
        {
            protected internal Tokenizer source;
            protected internal TokenStream result;
        };

        /// <summary>
        /// Returns a (possibly reused) <see cref="TokenStream"/> which tokenizes all the text in the
        /// provided <see cref="TextReader"/>.
        /// </summary>
        /// <returns>
        ///   A <see cref="TokenStream"/> built from a <see cref="ChineseTokenizer"/> 
        ///   filtered with <see cref="ChineseFilter"/>.
        /// </returns>
        public override TokenStream ReusableTokenStream(String fieldName, TextReader reader)
        {
            /* tokenStream() is final, no back compat issue */
            SavedStreams streams = (SavedStreams)PreviousTokenStream; KeyValuePair<string, string> gg = new KeyValuePair<string, string>();
            if (streams == null)
            {
                streams = new SavedStreams();
                streams.source = new YKChineseTokenizer(reader);
                streams.result = new StandardFilter(streams.source);
                PreviousTokenStream = streams;
            }
            else
            {
                streams.source.Reset(reader);
            }
            return streams.result;
        }
    }
}
