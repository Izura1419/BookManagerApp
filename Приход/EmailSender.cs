using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Приход
{
    public static class EmailSender
    {
        public static async Task SendEmailAsync(string toEmail, string subject, string body,
            string smtpServer, int smtpPort, string smtpUsername, string smtpPassword, bool enableSsl = true)
        {
            using (var mail = new MailMessage())
            {
                mail.From = new MailAddress(smtpUsername);
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    smtpClient.EnableSsl = enableSsl;

                    await smtpClient.SendMailAsync(mail);
                }
            }
        }
    }
}
