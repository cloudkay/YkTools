using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.FrameworkTools.SensitiveFilterService
{
    /// <summary>
    /// 分词类型，Text或者Html
    /// </summary>
    public enum EFieldType
    {
        /// <summary>
        /// 分词内容是纯文本
        /// </summary>
        Text = 1,

        /// <summary>
        /// 分词内容是Html
        /// </summary>
        HtmlText = 2
    }

    /// <summary>
    /// 分词实体
    /// </summary>
    public class SensitiveWordEntity
    {
        /// <summary>
        /// 分词句子字段名称
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 分词内容
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 分词类型
        /// </summary>
        public EFieldType FieldType { get; set; }
    }
}
