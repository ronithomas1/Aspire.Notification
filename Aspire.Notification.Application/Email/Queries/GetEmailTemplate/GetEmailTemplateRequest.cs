using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspire.Notification.Application.Email.Queries.GetEmailTemplate
{
    public class GetEmailTemplateRequest
    {
        public required string Type { get; set; } = "Email";
        public string? Name { get; set; }
    }
}
