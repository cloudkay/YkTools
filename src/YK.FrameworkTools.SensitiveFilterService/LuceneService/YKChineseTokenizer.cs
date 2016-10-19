using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Tokenattributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace YK.FrameworkTools.SensitiveFilterService
{
    /// <summary>
    /// 中文分词Tokenier
    /// </summary>
    public class YKChineseTokenizer : Tokenizer
    {
        private int bufferIndex = 0;
        private int dataLen = 0;
        private int start;
        private string text;

        private WordTreeHelper m_WordTreeHelper;

        private ITermAttribute termAtt;
        private IOffsetAttribute offsetAtt;

        /// <summary>
        /// 中文分词Tokenier构造函数
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="pathDic"></param>
        /// <param name="isLoadExtra"></param>
        public YKChineseTokenizer(TextReader reader, string pathDic = "", bool isLoadExtra = true)
        {
            this.input = reader;
            this.text = this.input.ReadToEnd();
            this.dataLen = this.text.Length;
            m_WordTreeHelper = new FSWordTreeHelper(pathDic, isLoadExtra);
            Init();
        }

        private void Init()
        {
            termAtt = AddAttribute<ITermAttribute>();
            offsetAtt = AddAttribute<IOffsetAttribute>();
        }

        private bool Flush(string termText)
        {
            if (!string.IsNullOrEmpty(termText))
            {
                termAtt.SetTermBuffer(termText.ToArray(), 0, termText.Length);
                offsetAtt.SetOffset(CorrectOffset(start - termText.Length), CorrectOffset(start + termText.Length));
                return true;
            }

            return true;
        }

        public override bool IncrementToken()
        {
            ClearAttributes();

            Hashtable hashtable = m_WordTreeHelper.HashtableWordDict;

            string text = string.Empty;
            this.bufferIndex = this.start;
            int num = this.start;
            int num2 = this.bufferIndex;
            string text2 = string.Empty;
            while (this.start < this.dataLen)
            {
                string text3 = this.text.Substring(this.start, 1);
                if (!string.IsNullOrEmpty(text3.Trim()))
                {
                    if (!hashtable.Contains(text3))
                    {
                        if (text == string.Empty)
                        {
                            int i = this.start + 1;
                            switch (WordTreeHelper.GetCharType(text3))
                            {
                                case 0:
                                    text += text3;
                                    break;
                                case 1:
                                    while (i < this.dataLen)
                                    {
                                        if (WordTreeHelper.GetCharType(this.text.Substring(i, 1)) != 1)
                                        {
                                            break;
                                        }
                                        i++;
                                    }
                                    text += this.text.Substring(this.start, i - this.start).ToLower();
                                    break;
                                case 2:
                                    while (i < this.dataLen)
                                    {
                                        if (WordTreeHelper.GetCharType(this.text.Substring(i, 1)) != 2)
                                        {
                                            break;
                                        }
                                        i++;
                                    }
                                    text += this.text.Substring(this.start, i - this.start);
                                    break;
                                default:
                                    this.start++;
                                    this.bufferIndex = this.start;
                                    continue;
                            }
                            this.start = i;
                        }
                        else if (WordTreeHelper.GetCharType(text3) == -1)
                        {
                            this.start++;
                        }
                        if (hashtable.Contains("word"))
                        {
                            return Flush(text);
                            //result = new Token(text, this.bufferIndex, this.bufferIndex + text.Length);
                        }
                        else
                        {
                            this.start = num + 1;
                            return Flush(text2);
                            //result = new Token(text2, num2, num2 + text2.Length);
                        }
                    }
                    else
                    {
                        text += text3;
                        hashtable = (Hashtable)hashtable[text3];
                        if (hashtable.Contains("word") || text.Length == 1)
                        {
                            text2 = text;
                            num = this.start;
                            num2 = this.bufferIndex;
                        }
                        this.start++;
                        if (this.start != this.dataLen)
                        {
                            continue;
                        }
                        if (hashtable.Contains("word") || text.Length == 1)
                        {
                            return Flush(text);
                            //result = new Token(text, this.bufferIndex, this.bufferIndex + text.Length);
                        }
                        else
                        {
                            this.start = num + 1;
                            return Flush(text2);
                            //result = new Token(text2, num2, num2 + text2.Length);
                        }
                    }
                    //return true;
                }
                this.start++;
                this.bufferIndex = this.start;
            }
            return false;
        }

        public override sealed void End()
        {
            // set final offset
            int finalOffset = CorrectOffset(start);
            this.offsetAtt.SetOffset(finalOffset, finalOffset);
        }

        public override void Reset()
        {
            base.Reset();
            start = bufferIndex = 0;
        }

        public override void Reset(TextReader input)
        {
            base.Reset(input);
            this.text = this.input.ReadToEnd();
            this.dataLen = this.text.Length;
            Reset();
        }
    }
}
