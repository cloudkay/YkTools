using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YK.FrameworkTools.EmailService;

namespace Test.EmailService
{
    [TestClass]
    public class EmailTest
    {
        [TestMethod]
        public void SendEmaiilTest()
        {
            IEmail emailService = new EmailHelper();
            EmailEntity emailEntity = new EmailEntity()
            {
                ToList = new System.Collections.Generic.List<string>() { "kuangqifu@yun-kai.com" },
                Attachments = new System.Collections.Generic.List<string>(){@"E:\test.html", @"E:\备忘录.txt"},
                Subject = "发送邮件组件测试",
                Content = "发送邮件组件测试内容",
                Priority = System.Net.Mail.MailPriority.High,
                IsHTMLFormat = false
            };

            emailService.SendEmail(emailEntity);
            Console.Read();
        }
    }
}
