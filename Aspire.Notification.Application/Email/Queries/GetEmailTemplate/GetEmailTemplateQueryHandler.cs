using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspire.Notification.Application.Email.Queries.GetEmailTemplate
{
    public class GetEmailTemplateQueryHandler :
        IRequestHandler<GetEmailTemplateQuery, EmailTemplateDto>
    {
        public Task<EmailTemplateDto> Handle(GetEmailTemplateQuery request, CancellationToken cancellationToken)
        {
            var  response = new EmailTemplateDto
            {
                DisplayName = "",
                IsActive = true,
                NotificationTemplate = ""
            };
            return Task.FromResult(response);

        }
    }
}
