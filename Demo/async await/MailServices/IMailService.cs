using System;
using System.Collections.Generic;
using System.Text;

namespace MailServices
{
    public interface IMailService
    {
        public void SendEmail(string title, string to, string body);
    }
}
