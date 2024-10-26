using Aspire.Notification.Application.Common.Interfaces.Infrastructure;
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
        private readonly IEmailSender _emailSender;
        public SendEmailCommandHandler(IEmailSender emailSender)
        {
                _emailSender = emailSender;
        }
        public async Task<Unit> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            request.notificationTemplate = 
                request.notificationTemplate.Replace("##CONTENT##", request.body);
            await _emailSender.SendEmailAsync(request.From, request.To, request.Cc, request.Bcc,
                request.subject, request.notificationTemplate, request.displayName);
            return Unit.Value;
        }
    }
}
