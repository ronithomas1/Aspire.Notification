using Aspire.Notification.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Aspire.Notification.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
       this IServiceCollection services,
       IConfiguration configuration) =>
       services
           .AddDatabase(configuration);

        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            string? connection = configuration.GetConnectionString("AspireContextConnection");

            services.AddDbContext<AspireContext>(
             options =>
             {
                 options.UseSqlServer(connection);
             });

            return services;
        }
    }
}
