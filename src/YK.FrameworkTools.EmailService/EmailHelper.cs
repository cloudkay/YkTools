using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using YK.FrameworkTools.ToolHelper;

namespace YK.FrameworkTools.EmailService
{
    /// <summary>
    /// 邮件帮助类
    /// </summary>
    public class EmailHelper : IEmail
    {
        private SmtpClient _smtp;
        private MailMessage _mail;
        public string Path = AppDomain.CurrentDomain.BaseDirectory + @"\ReceivePath\";
        private IEmailConfig m_EmailConfig;

        public EmailHelper(bool isDefault = true)
        {
            if(isDefault)
            {
                m_EmailConfig = new DefaultEmailConfig();
            }
            else
            {
                m_EmailConfig = new SettingEmailConfig();
            }
        }

        /// <summary>
        /// 发邮件
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string SendEmail(EmailEntity obj)
        {
            try
            {
                if (obj != null)
                {
                    if (ValidateAllFileSize(obj.Attachments))
                    {

                        obj.From = m_EmailConfig.SystemEmail;
                        obj.Smtp = GetSmtp();
                        obj.Port = GetPort();

                        if (!string.IsNullOrEmpty(obj.Smtp) && obj.Port > 0
                            && !string.IsNullOrEmpty(obj.From)
                            && (obj.ToList.Count > 0))
                        {
                            InitSmtp(obj);
                            InitMailMessage(obj);
                            AddAttchement(obj);

                            _smtp.SendAsync(_mail, null);
                            _smtp.SendCompleted += (sender, evt) =>
                            {
                                AttachmentsDispose();
                                //DeleteFiles(obj);
                            };
                            return string.Empty;
                        }

                        //return Culture.CnEn(obj.CultureName, "发送邮件失败.", "Fail to send the Email.");
                    }//end if (ValidateAllFileSize(obj.Attachments))

                    //return Culture.CnEn(obj.CultureName, "所有的附件大小不能大于7M", "The size of all attachments can't be bigger than 7m");
                }//end   if (obj != null)

                //return Culture.CnEn(obj.CultureName, "发送邮件失败.", "Fail to send the Email.");
            }
            catch (DomainServiceException ex)
            {
                // return Culture.CnEn(obj.CultureName, "发送邮件失败.", "Fail to send the Email."); ;
            }
            return string.Empty;
        }     

        /// <summary>
        /// 删除邮件附件
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool DeleteFiles(EmailEntity obj)
        {
            try
            {
                if (obj.Attachments != null && obj.Attachments.Count > 0)
                {
                    foreach (String FilePath in obj.Attachments)
                    {
                        if (File.Exists(Path + FilePath))
                        {
                            File.Delete(Path + FilePath);
                        }
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //throw new DomainServiceException("Fail to delete files.", ex);
            }
            return false;
        }

        /// <summary>
        /// 获取所有文件的大小
        /// </summary>
        /// <returns></returns>
        public string GetFileSize()
        {
            string size = GetSingleFileSize();
            if (ValidateSingleFileSize(size))
            {
                string fileSize = size.Substring(0, size.Length - 1);
                int i = GetI(size);
                long intSize = Convert.ToInt64(fileSize.Trim()) * i;
                if (intSize > 1024 * 1024 * 1024)
                {
                    throw new DomainServiceException("\r\nA single file size is less than 1 G.");
                }
                size = intSize.ToString();
            }
            return size;

        }

        /// <summary>
        /// 把所有的文件构造成UserFileInfo实体
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public List<UserFileInfo> GetFilesInfo(List<string> files)
        {
            List<UserFileInfo> ufile = null;
            if (files != null)
            {
                ufile = new List<UserFileInfo>();
                foreach (string fileName in files)
                {
                    if (File.Exists(Path + fileName))
                    {
                        System.IO.FileInfo file = new System.IO.FileInfo(Path + fileName);
                        UserFileInfo uInfo = new UserFileInfo();
                        uInfo.FileName = fileName;
                        uInfo.Size = file.Length.ToString();
                        ufile.Add(uInfo);
                    }
                }
            }
            return ufile;
        }

        /// <summary>
        /// 释放邮件附件
        /// </summary>
        private void AttachmentsDispose()
        {
            if (_mail.Attachments != null && _mail.Attachments.Count > 0)
            {
                _mail.Attachments.Dispose();
                GC.Collect();
            }
        }

        /// <summary>
        /// 初始化Smtp
        /// </summary>
        /// <param name="obj"></param>
        private void InitSmtp(EmailEntity obj)
        {
            try
            {
                string domainUser = m_EmailConfig.UserName;
                string domainPws = DataEncrypt.DecryptFromDB(m_EmailConfig.Password);
                _smtp = new SmtpClient
                {
                    Host = obj.Smtp,
                    Port = obj.Port,
                    EnableSsl = obj.Ssl,
                    Credentials = new System.Net.NetworkCredential(domainUser, domainPws)
                };
            }
            catch (Exception ex)
            {
                //throw new DomainServiceException("InitSmtp Failed.", ex);
            }
        }

        /// <summary>
        /// 初始化MailMessage
        /// </summary>
        /// <param name="obj"></param>
        private void InitMailMessage(EmailEntity obj)
        {
            try
            {
                _mail = new MailMessage()
                {
                    From = new MailAddress(obj.From),
                    Subject = obj.Subject,
                    Body = obj.Content,
                    IsBodyHtml = obj.IsHTMLFormat,
                    BodyEncoding = obj.BodyEncoding,
                    Priority = obj.Priority,
                };

                if (obj.CcList != null)
                {
                    foreach (string c in obj.CcList)
                    {
                        _mail.CC.Add(c);
                    }
                }
                if (obj.BccList != null)
                {
                    foreach (var mailBcc in obj.BccList)
                    {
                        _mail.Bcc.Add(mailBcc);
                    }
                }                

                if (_mail != null)
                {
                    foreach (var item in obj.ToList)
                    {
                        if (!_mail.To.Contains(new MailAddress(item)))
                        {
                            _mail.To.Add(item);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //throw new DomainServiceException("Fail to InitMailMessage.", ex);
            }
        }

        /// <summary>
        /// 添加配置文件
        /// </summary>
        /// <param name="obj"></param>
        private void AddAttchement(EmailEntity obj)
        {
            if (obj.Attachments != null && obj.Attachments.Count > 0)
            {
                foreach (String fileName in obj.Attachments)
                {
                    if (File.Exists(fileName))
                    {
                        var att = new Attachment(fileName)
                        {
                            NameEncoding = obj.BodyEncoding,
                            Name = fileName.Substring(fileName.LastIndexOf('_') + 1)
                        };

                        _mail.Attachments.Add(att);
                    }
                }
            }
        }

        /// <summary>
        /// 判断所有文件的大小是否在允许的范围内
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        private bool ValidateAllFileSize(List<string> files)
        {
            if (files != null)
            {
                string allSize = GetAllFileSize();
                if (ValidateSingleFileSize(allSize))
                {
                    long size = GetAllSize(files);
                    if (Convert.ToDouble(allSize.Substring(0, allSize.Length - 1)) > size)
                    {
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 计算所有文件的总大小
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        private long GetAllSize(List<string> files)
        {
            long size = 0;
            if (files != null)
            {
                foreach (string fileName in files)
                {
                    if (File.Exists(Path + fileName))
                    {
                        System.IO.FileInfo file = new System.IO.FileInfo(Path + fileName);
                        size += file.Length;
                    }
                }
            }
            return size;
        }

        /// <summary>
        /// 获取配置文件中的Smtp信息
        /// </summary>
        /// <returns></returns>
        private string GetSmtp()
        {
            string smtp = string.Empty;
            if (!string.IsNullOrEmpty(m_EmailConfig.Smtp))
            {
                smtp = m_EmailConfig.Smtp;
                if (!string.IsNullOrEmpty(smtp))
                {
                    return smtp;
                }
                throw new DomainServiceException("\r\nsmtp is empty.");
            }
            smtp = "smtpscn1.huawei.com";
            return smtp;
        }

        /// <summary>
        /// 获取配置文件中的端口信息
        /// </summary>
        /// <returns></returns>
        private int GetPort()
        {
            if (!string.IsNullOrEmpty(m_EmailConfig.Port))
            {
                string port = m_EmailConfig.Port;
                if (!string.IsNullOrEmpty(port))
                {
                    return Convert.ToInt32(port);
                }
                throw new DomainServiceException("\r\nPort is empty.");
            }
            return 25;
        }

        /// <summary>
        /// 获取配置文件中单个文件的大小设置
        /// </summary>
        /// <returns></returns>
        private string GetSingleFileSize()
        {
            if (!string.IsNullOrEmpty(m_EmailConfig.SingleFileSize))
            {
                string size = m_EmailConfig.SingleFileSize;
                if (!string.IsNullOrEmpty(size))
                {
                    return size;
                }
                throw new DomainServiceException("\r\nSingle file size config error.");
            }
            throw new DomainServiceException("\r\nSingle file size config error.");
        }

        /// <summary>
        /// 获取配置文件中所有文件的大小设置
        /// </summary>
        /// <returns></returns>
        private string GetAllFileSize()
        {
            if (string.IsNullOrEmpty(m_EmailConfig.AllFileSize))
            {
                string size = m_EmailConfig.AllFileSize;
                if (!string.IsNullOrEmpty(size))
                {
                    string fileSize = size.Substring(0, size.Length - 1);
                    int i = GetI(size);
                    long intSize = Convert.ToInt64(fileSize.Trim()) * i;
                    size = intSize.ToString() + "b";
                    return size;
                }
            }
            return "7m";
        }

        /// <summary>
        /// 判断配置文件中文件大小设置是否正确，不正确抛出异常
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private bool ValidateSingleFileSize(string size)
        {
            if (!string.IsNullOrEmpty(size))
            {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^[0-9]+[bkmgBKMG]");
                if (!regex.IsMatch(size.Trim()))
                {
                    throw new DomainServiceException("\r\nSingle file size config error.\r\nFor example:500b\\50k\\50m\\1g.");
                }

                string fileSize = size.Substring(0, size.Length - 1);
                regex = new System.Text.RegularExpressions.Regex(@"^[0-9]*$");
                if (!regex.IsMatch(fileSize.Trim()))
                {
                    throw new DomainServiceException("\r\nSingle file size config error. \r\nDigital cannot contain characters.");
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 根据文件单位，把文件转换为byte
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private int GetI(string size)
        {
            string unit = size.Substring(size.Length - 1);
            int i = 1;
            switch (unit.ToLower())
            {
                case "b":
                    break;
                case "k":
                    i = 1024;
                    break;
                case "m":
                    i = 1024 * 1024;
                    break;
                case "g":
                    i = 1024 * 1024 * 1024;
                    break;
                default:
                    break;
            }
            return i;
        }
    }
}
