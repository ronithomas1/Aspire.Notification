using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspire.Notification.Application.Email.Commands.SendEmail
{
    public class EmailTemplateDto
    {
        public string From { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string NotificationTemplate { get; set; } = "";
        public bool IsActive { get; set; }
    }
}
