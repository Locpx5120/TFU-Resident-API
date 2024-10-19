namespace TFU_Building_API.Core.Helper
{
    using System;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using TFU_Building_API.Core.Struct;

    public class EmailService
    {
        private readonly string smtpServer;
        private readonly int smtpPort;
        private readonly string smtpUsername;
        private readonly string smtpPassword;
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
            this.smtpServer = _config[AppSetting.AppSettings.SmtpServer];
            this.smtpPort = int.Parse(_config[AppSetting.AppSettings.SmtpPort]);
            this.smtpUsername = _config[AppSetting.AppSettings.SmtpUsername];
            this.smtpPassword = _config[AppSetting.AppSettings.SmtpPassword];
        }

        public EmailService(string smtpServer, int smtpPort, string smtpUsername, string smtpPassword)
        {
            this.smtpServer = smtpServer;
            this.smtpPort = smtpPort;
            this.smtpUsername = smtpUsername;
            this.smtpPassword = smtpPassword;
        }

        public async Task SendEmailAsync(string recipientEmail, string subject, string body)
        {
            using (var client = new SmtpClient(smtpServer, smtpPort))
            {
                client.EnableSsl = true; // Bật SSL (rất cần thiết khi dùng Gmail)
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(smtpUsername); // Địa chỉ email người gửi
                mailMessage.To.Add(recipientEmail); // Địa chỉ email người nhận
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true; // Nếu body là HTML

                try
                {
                    await client.SendMailAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                }
            }
        }
    }

}
