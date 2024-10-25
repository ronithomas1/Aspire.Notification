using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspire.Notification.Application.Common.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string from, List<string> to, List<string> cc, List<string> bcc,
            string subject, string body, string? displayName = null);
    }
}
