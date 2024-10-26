using Aspire.Notification.Application.Common.Interfaces.Infrastructure;
using Aspire.Notification.Infrastructure.Database;
using Aspire.Notification.Infrastructure.Database.Repositories;
using Aspire.Notification.Infrastructure.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Aspire.Notification.Infrastructure
{
    public static class DependencyInjection
    {
       public static IServiceCollection AddInfrastructureServices(
       this IServiceCollection services,
       IConfiguration configuration,
       IHostApplicationBuilder builder, bool isDevelopment) =>
       services
           .AddDatabase(configuration, builder)
           .AddEmail();

        private static IServiceCollection AddDatabase(this IServiceCollection services, 
            IConfiguration configuration, IHostApplicationBuilder builder)
        {
            // This is the name of the sqlDatabase in AppHost
            builder.AddSqlServerDbContext<AspireContext>("Aspire-Notification");
            builder.Services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            builder.Services.AddScoped<ITemplateRepository, TemplateRepository>();
            return builder.Services;
        }

        private static IServiceCollection AddEmail(this IServiceCollection services)
        {
            services.AddScoped<IEmailSender, SmtpEmailSender>();
            return services;
        }
    }
}
