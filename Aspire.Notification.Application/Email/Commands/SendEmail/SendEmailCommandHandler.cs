using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspire.Notification.Application.Email.Commands.SendEmail
{
    public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, Unit>
    {
        public async Task<Unit> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            request.notificationTemplate = 
                request.notificationTemplate.Replace("##CONTENT##", request.body);

            return Unit.Value;
        }
    }
}
