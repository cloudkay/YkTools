using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.FrameworkTools.SensitiveFilterService
{
    internal abstract class SensitiveWordComponent
    {
        private SensitiveWordEntity m_SensitiveWordEntity { get; set; }
        private SensitiveWordResult m_SensitiveWordResult { get; set; }
        private SensitiveWordComponent m_SensitiveWordComponent { get; set; }
        protected string m_Text { get; set; }

        internal SensitiveWordComponent(SensitiveWordEntity sensitiveWordEntity, SensitiveWordResult sensitiveWordResult, SensitiveWordComponent sensitiveWordComponent)
        {
            m_SensitiveWordEntity = sensitiveWordEntity;
            m_SensitiveWordResult = sensitiveWordResult;
            m_SensitiveWordComponent = sensitiveWordComponent;
        }

        private void MakeSensitiveWord()
        {
            var words = LuceneHelper.AnalysisVocabulary(m_Text);
            foreach (var word in words)
            {
                if (LuceneHelper.WordTree.ExtraWordDict.Contains(word))
                {
                    if (m_SensitiveWordResult.SensitiveWordResultType == ESensitiveWordResultType.OnlyOne)
                    {
                        m_SensitiveWordResult.IsSensitiveWord = true;
                        m_SensitiveWordResult.OnlyOneDictErrorWord = new KeyValuePair<string, string>(m_SensitiveWordEntity.Field, word);
                    }
                    else
                    {
                        m_SensitiveWordResult.PerOneDictErrorWord.Add(m_SensitiveWordEntity.Field, word);
                    }

                    return;
                }
            }

        }

        private void MakeSensitiveWords()
        {
            List<string> wordResult = new List<string>();
            var words = LuceneHelper.AnalysisVocabulary(m_Text);
            foreach (var word in words)
            {
                if (LuceneHelper.WordTree.ExtraWordDict.Contains(word))
                {
                    wordResult.Add(word);
                }
            }

            m_SensitiveWordResult.IsSensitiveWord = true;
            m_SensitiveWordResult.AllDictErrorWord.Add(m_SensitiveWordEntity.Field, wordResult);
        }

        internal void CheckSensitive()
        {
            switch (m_SensitiveWordResult.SensitiveWordResultType)
            {
                case ESensitiveWordResultType.OnlyOne:
                    if (m_SensitiveWordResult.IsSensitiveWord)
                    {
                        return;
                    }
                    else
                    {
                        MakeSensitiveWord();
                    }
                    break;
                case ESensitiveWordResultType.PerOne:
                    MakeSensitiveWord();
                    break;
                case ESensitiveWordResultType.All:
                    MakeSensitiveWords();
                    break;
                default:
                    if (m_SensitiveWordResult.IsSensitiveWord)
                    {
                        return;
                    }
                    else
                    {
                        MakeSensitiveWord();
                    }
                    break;
            }

            if (m_SensitiveWordComponent != null)
            {
                m_SensitiveWordComponent.CheckSensitive();
            }
        }
    }
}
