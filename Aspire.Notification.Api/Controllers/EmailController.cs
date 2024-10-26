using Aspire.Notification.Application.Email.Commands.SendEmail;
using Aspire.Notification.Application.Email.Queries.GetEmailTemplate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aspire.Notification.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EmailController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost()]
        [AllowAnonymous]
        public async Task<ActionResult> SendEmail([FromBody] SendEmailRequest email)
        {
            var emailTemplateQuery = new GetEmailTemplateQuery() { Type="Email",
               Name=email.templateName};
            var existingTemplate = await _mediator.Send(emailTemplateQuery);
           
            await _mediator.Send(new SendEmailCommand {
                From = existingTemplate.From,
                To = email.Recipients.To,
                Bcc = email.Recipients.Bcc,
                Cc = email.Recipients.Cc,
                subject = email.subject,
                body = email.body,
                notificationTemplate = existingTemplate.NotificationTemplate,
                displayName = existingTemplate.DisplayName,
            });
            return Ok();
        }
    }
}
