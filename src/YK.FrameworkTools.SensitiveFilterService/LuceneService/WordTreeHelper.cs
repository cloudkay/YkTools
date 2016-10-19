using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Caching;

namespace YK.FrameworkTools.SensitiveFilterService
{
    internal abstract class WordTreeHelper
    {
        private const string CacheDictKey = "Sensitive_DictKey";
		public static double DictLoad_Span = 0.0;
        private const string m_RegexChinese = "[一-龥]";
        private const string m_RegexNumber = "[0-9]";
        private const string m_RegexEnglish = "[a-zA-Z]";

        protected static string m_DictPath;

        private static ICacheService Cache = new CacheService();

        protected static Hashtable DictHashtable;

        protected static HashSet<string> ExtraHashset;

        public HashSet<string> ExtraWordDict
        {
            get
            {
                if (!Cache.IsContainsKey(CacheDictKey))
                {
                    LoadWordDict();
                    return ExtraHashset;
                }
                else
                {
                    return ExtraHashset;
                }
            }
        }

        public Hashtable HashtableWordDict
        {
            get
            {
                if (!Cache.IsContainsKey(CacheDictKey))
                {
                    LoadWordDict();
                    return DictHashtable;
                }
                else
                {
                    return DictHashtable;
                }
            }
        }

        private void LoadWordDict()
        {
            DictHashtable = new Hashtable();
            ExtraHashset = new HashSet<string>();

            if (m_IsLoadExtra)
            {
                LoadExtraDict();
            }
            LoadNormalDict();

            CacheDependency templateCacheDependency = new CacheDependency(new string[] { m_DictPath + @"\Dict\ExtraDict.txt"
                                                              , m_DictPath + @"\Dict\NormalDict.txt", m_DictPath + @"\Dict\NoiseDict.txt" });
            Cache.Set(CacheDictKey, CacheDictKey, templateCacheDependency);
        }

        protected bool m_IsLoadExtra = true;

        public WordTreeHelper(bool isLoadExtra)
        {
            m_IsLoadExtra = isLoadExtra;
        }

		public static int GetCharType(string Char)
		{
			int result;
			if (new Regex(m_RegexChinese).IsMatch(Char))
			{
				result = 0;
			}
            else if (new Regex(m_RegexEnglish).IsMatch(Char))
			{
				result = 1;
			}
            else if (new Regex(m_RegexNumber).IsMatch(Char))
			{
				result = 2;
			}
			else
			{
				result = -1;
			}
			return result;
		}

        /// <summary>
        /// 初始化正常的数据字典
        /// </summary>
        protected abstract void LoadNormalDict();

        /// <summary>
        /// 初始化其他数据字典
        /// </summary>
        protected abstract void LoadExtraDict();

        protected void BuidNormalDictTree(TextReader streamReader)
		{
			string text = streamReader.ReadLine();
			while (!string.IsNullOrEmpty(text))
			{
                Hashtable hashtable = DictHashtable;
				for (int i = 0; i < text.Length; i++)
				{
					string key = text.Substring(i, 1);
					if (!hashtable.Contains(key))
					{
						hashtable.Add(key, new Hashtable());
					}
					hashtable = (Hashtable)hashtable[key];
				}
				if (!hashtable.Contains("word"))
				{
					hashtable.Add("word", null);
				}
				text = streamReader.ReadLine();
			}
			streamReader.Close();
		}

        protected void BuidExtraDictTree(TextReader streamReader)
        {
            string text = streamReader.ReadLine();
            while (!string.IsNullOrEmpty(text))
            {
                Hashtable hashtable = DictHashtable;
                for (int i = 0; i < text.Length; i++)
                {
                    string key = text.Substring(i, 1);
                    if (!hashtable.Contains(key))
                    {
                        hashtable.Add(key, new Hashtable());
                    }
                    hashtable = (Hashtable)hashtable[key];
                }
                if (!hashtable.Contains("word"))
                {
                    hashtable.Add("word", null);
                    ExtraHashset.Add(text);
                }
                text = streamReader.ReadLine();
            }
            streamReader.Close();
        }
	}
}
