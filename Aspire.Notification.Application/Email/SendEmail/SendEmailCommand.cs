using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspire.Notification.Application.Email.SendEmail
{
    public  record SendEmailCommand: IRequest<Unit>
    {
        public required SendEmailDto  SendEmail { get; set; }
    }
}
