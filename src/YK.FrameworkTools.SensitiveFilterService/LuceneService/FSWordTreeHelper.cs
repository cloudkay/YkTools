using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace YK.FrameworkTools.SensitiveFilterService
{
    internal class FSWordTreeHelper : WordTreeHelper
    {
        public FSWordTreeHelper(string path, bool isLoadExtra = true)
            :base(isLoadExtra)
        {
            if(string.IsNullOrEmpty(path))
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
                m_DictPath = filePath;
            }
            else
            {
                m_DictPath = path;
            }
        }

        protected override void LoadNormalDict()
        {
            var streamReader = new StreamReader(m_DictPath + @"\Dict\NormalDict.txt", Encoding.UTF8);
            BuidNormalDictTree(streamReader);
        }

        protected override void LoadExtraDict()
        {
            var streamReader = new StreamReader(m_DictPath + @"\Dict\ExtraDict.txt", Encoding.UTF8);
            BuidExtraDictTree(streamReader);
        }
    }
}
