using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Aspire.Notification.Infrastructure.Database
{
    public class AspireContextFactory : IDesignTimeDbContextFactory<AspireContext>
    {
        public AspireContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AspireContext>();
            builder.UseSqlServer("data source=.\\sqlexpress;initial catalog=Aspire.NotificationManagement;Trusted_Connection=True; PoolBlockingPeriod=NeverBlock; TrustServerCertificate=True",
                optionsBuilder => optionsBuilder.
                MigrationsAssembly(typeof(AspireContext).GetTypeInfo().Assembly.GetName().Name));
            return new AspireContext(builder.Options);
        }
    }
}
