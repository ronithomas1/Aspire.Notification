using Aspire.Notification.Application.Common.Exceptions;
using Aspire.Notification.Application.Common.Interfaces.Infrastructure;
using Mapster;
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
        private readonly ITemplateRepository _templateRepository;
        public SendEmailCommandHandler(IEmailSender emailSender,
            ITemplateRepository templateRepository)
        {
            _emailSender = emailSender;
            _templateRepository = templateRepository;
        }
        public async Task<Unit> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var emailTemplate = await _templateRepository.GetTemplateAsync("Email",
            string.IsNullOrEmpty(request.templateName) ?
                                "Default" : request.templateName);          

            var response = emailTemplate.Adapt<EmailTemplateDto>();
            var htmlContent = response.NotificationTemplate.Replace("##CONTENT##", request.body);
            var displayName = response.DisplayName;
            var from = response.From;
            await _emailSender.SendEmailAsync(from, request.To, request.Cc, request.Bcc,
                request.subject, htmlContent, displayName);

            return Unit.Value;
        }
    }
}
