using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.FrameworkTools.SensitiveFilterService
{
    /// <summary>
    /// 获取敏感词帮助类
    /// </summary>
    public class SensitiveWordHelper
    {
        /// <summary>
        /// 获取敏感词
        /// </summary>
        /// <param name="sensitiveWordInfos">需要分析的句子集合</param>
        /// <param name="sensitiveWordResultType">分词结果获取类型</param>
        /// <returns>获取结果</returns>
        public static SensitiveWordResult GetSensitiveWordResult(List<SensitiveWordEntity> sensitiveWordInfos, ESensitiveWordResultType sensitiveWordResultType)
        {
            SensitiveWordResult sensitiveWordResult = new SensitiveWordResult();
            switch (sensitiveWordResultType)
            {
                case ESensitiveWordResultType.PerOne:
                    sensitiveWordResult.PerOneDictErrorWord = new Dictionary<string, string>();
                    break;
                case ESensitiveWordResultType.All:
                    sensitiveWordResult.AllDictErrorWord = new Dictionary<string, List<string>>();
                    break;
            }
            sensitiveWordResult.SensitiveWordResultType = sensitiveWordResultType;

            SensitiveWordComponent wrapSensitiveWordComponent = null;

            foreach(var sensitiveWordInfo in sensitiveWordInfos)
            {
                switch (sensitiveWordInfo.FieldType)
                {
                    case EFieldType.Text:
                        TextSensitiveWordService tempTextSensitiveWordService = new TextSensitiveWordService(sensitiveWordInfo, sensitiveWordResult, wrapSensitiveWordComponent);
                        wrapSensitiveWordComponent = tempTextSensitiveWordService;
                        break;
                    case EFieldType.HtmlText:
                        HtmlTextSensitiveWordService tempHtmlTextSensitiveWordService = new HtmlTextSensitiveWordService(sensitiveWordInfo, sensitiveWordResult, wrapSensitiveWordComponent);
                        wrapSensitiveWordComponent = tempHtmlTextSensitiveWordService;
                        break;
                    default:
                        TextSensitiveWordService tempDefaultTextSensitiveWordService = new TextSensitiveWordService(sensitiveWordInfo, sensitiveWordResult, wrapSensitiveWordComponent);
                        wrapSensitiveWordComponent = tempDefaultTextSensitiveWordService;
                        break;
                }
            }
            if(wrapSensitiveWordComponent != null)
            {
                wrapSensitiveWordComponent.CheckSensitive();
            }

            return sensitiveWordResult;
        }
    }
}
