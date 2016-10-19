using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.FrameworkTools.SensitiveFilterService
{
    /// <summary>
    /// 查找敏感词方式
    /// </summary>
    public enum ESensitiveWordResultType
    {
        /// <summary>
        /// 找到一个就停止
        /// </summary>
        OnlyOne = 1,
        
        /// <summary>
        /// 每个句子找第一个敏感词
        /// </summary>
        PerOne = 2,

        /// <summary>
        /// 取出所有敏感词
        /// </summary>
        All = 3
    }

    /// <summary>
    /// 分词算法分词结果
    /// </summary>
    public class SensitiveWordResult
    {
        /// <summary>
        /// 是否是敏感词
        /// </summary>
        public bool IsSensitiveWord { get; set; }

        /// <summary>
        /// 分词结果枚举
        /// </summary>
        internal ESensitiveWordResultType SensitiveWordResultType { get; set; }

        /// <summary>
        /// 找到一个就停止，结果
        /// </summary>
        public KeyValuePair<string, string> OnlyOneDictErrorWord { get; set; }

        /// <summary>
        /// 每个句子找第一个敏感词，结果
        /// </summary>
        public Dictionary<string, string> PerOneDictErrorWord { get; set; }

        /// <summary>
        /// 取出所有敏感词，结果
        /// </summary>
        public Dictionary<string, List<string>> AllDictErrorWord { get; set; }
    }
}
