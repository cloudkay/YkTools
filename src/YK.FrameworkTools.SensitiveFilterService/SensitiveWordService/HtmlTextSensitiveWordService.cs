using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.FrameworkTools.SensitiveFilterService
{
    internal class HtmlTextSensitiveWordService : SensitiveWordComponent
    {
        internal HtmlTextSensitiveWordService(SensitiveWordEntity sensitiveWordEntity, SensitiveWordResult sensitiveWordResult, SensitiveWordComponent sensitiveWordComponent)
            : base(sensitiveWordEntity, sensitiveWordResult, sensitiveWordComponent)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(sensitiveWordEntity.Text);
            m_Text = doc.DocumentNode.InnerText.Replace(" ", "").Replace("\r\n", "");
        }
    }
}
