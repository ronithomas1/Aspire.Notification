using Aspire.Notification.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Aspire.Notification.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
       this IServiceCollection services,
       IConfiguration configuration,
       IHostApplicationBuilder builder) =>
       services
           .AddDatabase(configuration, builder);

        private static IServiceCollection AddDatabase(this IServiceCollection services, 
            IConfiguration configuration, IHostApplicationBuilder builder)
        {
            // This is the name of the sqlDatabase in AppHost
            builder.AddSqlServerDbContext<AspireContext>("Aspire-Notification");
            return builder.Services;
        }
    }
}
