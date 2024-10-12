using Aspire.Notification.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspire.Notification.Domain.Template
{
    public sealed class Template : AuditableEntity, IIdentifiableEntity<int>
    {
        public int Id { get; set; }
        public required string Type { get; set; } // Email / SMS or any other 
        public required string Name { get; set; } // EmailBody 
        public string? Description { get; set; } // Info: Details of the template
        public string? From { get; set; }
        public string? DisplayName { get; set; }
        public string? NotificationTemplate { get; set; } // Structure of the template for email/ sms 
        public bool IsActive { get; set; }      
    }
}
