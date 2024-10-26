using Aspire.Notification.Application.Common.Interfaces.Infrastructure;
using Azure.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Aspire.Notification.Infrastructure.Email
{
    // PS Ref --> .NET Cloud-native Development: Working with Docker and Aspire
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpClient _client;
        public SmtpEmailSender(IConfiguration config)
        {
            var smtpUri = new Uri(config.GetConnectionString("SmtpUri")!);
            _client = new() { Host = smtpUri.Host, Port = smtpUri.Port };
        }
        public async Task SendEmailAsync(string from, List<string> to, List<string>? cc, 
            List<string>? bcc, string subject, string body, string? displayName = null)
        {
            string toEmail = string.Join(';', to);
            string ccEmail = cc== null ||  cc.Count == 0 ? "" :  string.Join(';', cc);
            string bccEmail = bcc == null || bcc.Count == 0 ? "" : string.Join(';', bcc);
            var mailMessage = new MailMessage
            {
                From = new MailAddress(from, displayName),
                To = { toEmail },
            //    CC = { ccEmail },
            //    Bcc = { bccEmail },
                Subject = subject,
                Body = body,                
                IsBodyHtml = true                
            };
            
            await _client.SendMailAsync(mailMessage);
        }
    }
}
