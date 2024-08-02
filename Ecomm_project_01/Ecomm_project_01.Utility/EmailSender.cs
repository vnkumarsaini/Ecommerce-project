﻿using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm_project_01.Utility
{
    public class EmailSender : IEmailSender
    {
        private EmailSettings _emailsettings { get; }
        public EmailSender(IOptions<EmailSettings> emailsettings)
        {
            _emailsettings = emailsettings.Value;
        }

        public async Task Execute(string email, string subject, string message)
        {
            try
            {
                string toEmail = string.IsNullOrEmpty(email) ? _emailsettings.ToEmail : email;
                MailMessage mailMessage = new MailMessage()
                {
                    From = new MailAddress(_emailsettings.UsernameEmail, "My Email Name"),
                };
                mailMessage.To.Add(toEmail);
                mailMessage.CC.Add(_emailsettings.CcEmail);
                mailMessage.Subject = "Shopping App :" + subject;
                mailMessage.Body= message;
                mailMessage.IsBodyHtml= true;
                mailMessage.Priority = MailPriority.High;
                using(SmtpClient smtp = new SmtpClient(_emailsettings.PrimaryDomain,_emailsettings.PrimaryPort)) 
                {
                    smtp.Credentials = new NetworkCredential(_emailsettings.UsernameEmail, _emailsettings.UsernamePassword);
                        smtp.EnableSsl= true;
                    await smtp.SendMailAsync(mailMessage);
                };
            }
            catch(Exception ex)  
            {
                string str = ex.Message;
            }
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Execute(email, subject, htmlMessage).Wait();
            return Task.FromResult(0);
           
        }
    }
}
