﻿using Aspire.Notification.Application.Common.Interfaces.Infrastructure;
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
        private readonly ITemplateRepository _templateRepository;
        public GetEmailTemplateQueryHandler(ITemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public async Task<EmailTemplateDto> Handle(GetEmailTemplateQuery request, CancellationToken cancellationToken)
        {
            var emailTemplate = await _templateRepository.GetTemplateAsync("Email", "Default");

            var  response = new EmailTemplateDto
            {
                From = emailTemplate.From,
                DisplayName = emailTemplate.DisplayName,
                IsActive = emailTemplate.IsActive,
                NotificationTemplate = emailTemplate.NotificationTemplate
            };
            return response;
        }
    }
}