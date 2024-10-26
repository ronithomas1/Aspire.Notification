using Aspire.Notification.Domain.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspire.Notification.Application.Common.Interfaces.Infrastructure
{
    public  interface ITemplateRepository: IAsyncRepository<Template>
    {
        Task<Template> GetTemplateAsync(string type, string name);
    }
}
