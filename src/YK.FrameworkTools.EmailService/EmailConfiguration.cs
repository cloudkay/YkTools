using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.FrameworkTools.EmailService
{
    public class EmailConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("SystemEmail", IsKey = true, IsRequired = true)]
        public string SystemEmail
        {
            get { return (string)base["SystemEmail"]; }
            set { SystemEmail = value; }
        }
        
        [ConfigurationProperty("UserName", IsKey = true, IsRequired = true)]
        public string UserName
        {
            get { return (string)base["UserName"]; }
            set { UserName = value; }
        }

        [ConfigurationProperty("Password", IsKey = true, IsRequired = true)]
        public string Password
        {
            get { return (string)base["Password"]; }
            set { Password = value; }
        }

        [ConfigurationProperty("Smtp", IsKey = true, IsRequired = true)]
        public string Smtp
        {
            get { return (string)base["Smtp"]; }
            set { Smtp = value; }
        }

        [ConfigurationProperty("Port", IsKey = true, IsRequired = true)]
        public string Port
        {
            get { return (string)base["Port"]; }
            set { Port = value; }
        }

        [ConfigurationProperty("SingleFileSize", IsRequired = false)]
        public string SingleFileSize
        {
            get { return (string)base["SingleFileSize"]; }
            set { SingleFileSize = value; }
        }

        [ConfigurationProperty("AllFileSize", IsRequired = false)]
        public string AllFileSize
        {
            get { return (string)base["AllFileSize"]; }
            set { AllFileSize = value; }
        }
    }
}
