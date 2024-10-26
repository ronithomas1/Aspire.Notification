using Aspire.Notification.Application.Common.Interfaces.Infrastructure;
using Aspire.Notification.Domain.Template;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspire.Notification.Infrastructure.Database.Repositories
{
    public class TemplateRepository : BaseRepository<Template>, ITemplateRepository
    {
        public TemplateRepository(AspireContext dbContext) : base(dbContext)
        {
                
        }
        public async Task<Template> GetTemplateAsync(string type, string name)
        {
            return await _dbContext.Template.Where(t => t.Type == type && t.Name == name)
                .SingleAsync(); 
        }
    }
}
