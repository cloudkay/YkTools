using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.FrameworkTools.SensitiveFilterService
{
    internal class TextSensitiveWordService : SensitiveWordComponent
    {
        internal TextSensitiveWordService(SensitiveWordEntity sensitiveWordEntity, SensitiveWordResult sensitiveWordResult, SensitiveWordComponent sensitiveWordComponent)
            : base(sensitiveWordEntity, sensitiveWordResult, sensitiveWordComponent)
        {
            m_Text = sensitiveWordEntity.Text;
        }
    }
}
