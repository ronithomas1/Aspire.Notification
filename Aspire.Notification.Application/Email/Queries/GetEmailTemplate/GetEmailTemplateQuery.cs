using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspire.Notification.Application.Email.Queries.GetEmailTemplate
{
    public record GetEmailTemplateQuery : IRequest<EmailTemplateDto>
    {
        public required GetEmailTemplateRequest EmailTemplateRequest { get; set; }
    }
}
