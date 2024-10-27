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
        public required List<string> To { get; set; }
        public List<string>? Cc { get; set; }
        public List<string>? Bcc { get; set; }
        public required string subject { get; set; }
        public required string body { get; set; }
        public string? templateName { get; set; }      
    }    
}
