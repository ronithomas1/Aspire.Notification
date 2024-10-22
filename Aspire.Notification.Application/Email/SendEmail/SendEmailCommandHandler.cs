using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspire.Notification.Application.Email.SendEmail
{
    public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, Unit>
    {
        public async Task<Unit> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            return Unit.Value;
        }
    }
}
