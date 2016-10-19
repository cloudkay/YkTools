using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace YK.FrameworkTools.EmailService
{
    public class EmailEntity
    {
        /// <summary>
        /// 发送主题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 邮件正文
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 是否是Html内容
        /// </summary>
        public bool IsHTMLFormat { get; set; }

        /// <summary>
        /// 内容编码方式
        /// </summary>
        public Encoding BodyEncoding { get; set; }

        /// <summary>
        /// 邮件重要等级
        /// </summary>
        public MailPriority Priority { get; set; }

        /// <summary>
        /// 附件，绝对地址
        /// </summary>
        public List<string> Attachments { get; set; }

        /// <summary>
        /// 收件人集合
        /// </summary>
        public List<string> ToList { get; set; }

        /// <summary>
        /// 抄送人集合
        /// </summary>
        public List<string> CcList { get; set; }

        /// <summary>
        /// 秘送人集合
        /// </summary>
        public List<string> BccList { get; set; }

        /// <summary>
        /// 发件人
        /// </summary>
        internal string From { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        internal int Port { get; set; }

        /// <summary>
        /// 邮件Smtp协议
        /// </summary>
        internal string Smtp { get; set; }

        /// <summary>
        /// 是否使用Ssl加密
        /// </summary>
        internal bool Ssl { get; set; }
    }

    [Serializable]
    public class DomainServiceException : Exception
    {
        public DomainServiceException(string message)
            : base(message)
        { }

        public DomainServiceException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}
