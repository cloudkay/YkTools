using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.FrameworkTools.EmailService
{
    public interface IEmail
    {
        string SendEmail(EmailEntity obj);
        bool DeleteFiles(EmailEntity obj);
        string GetFileSize();
        List<UserFileInfo> GetFilesInfo(List<string> files);
    }
}
