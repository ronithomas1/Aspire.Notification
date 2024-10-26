using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspire.Notification.Application.Email.Commands.SendEmail
{
    public record SendEmailCommand : IRequest<Unit>
    {
        public required string From { get; set; }
        public required List<string> To { get; set; }
        public List<string>? Cc { get; set; }
        public List<string>? Bcc { get; set; }

        public required string subject { get; set; }
        public required string body { get; set; }
        public required string notificationTemplate { get; set; }
        public string displayName { get; set; }
    }    
}
