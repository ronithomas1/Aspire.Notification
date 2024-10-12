using Aspire.Notification.Domain.Template;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspire.Notification.Infrastructure.Database
{
    public sealed  class AspireContext: DbContext
    {
        public AspireContext(DbContextOptions<AspireContext> options) : base(options)
        {
                
        }
        public DbSet<Template> Template { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AspireContext).Assembly);
            modelBuilder.HasDefaultSchema(Schemas.Notification);           
        }
    }
}
