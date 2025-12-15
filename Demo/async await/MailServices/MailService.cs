using ConfigServices;
using LogServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace MailServices
{
    public class MailService : IMailService
    {
        private readonly ILogProvider _logProvider;

        private readonly IConfigService _configService;

        public MailService(ILogProvider logProvider, IConfigService configService)
        {
            _logProvider = logProvider;
            _configService = configService;
        }

        public void SendEmail(string title, string to, string body)
        {
            var ip = _configService.GetValue("UserName");
            _logProvider.LogInfo($"Preparing to send email to {to} with title {title},{ip}");
            Console.WriteLine($"Sending email to {to} with title {title} and body {body}");
            _logProvider.LogInfo($"Email sent to {to} with title {title}");
        }
    }
}
