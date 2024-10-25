using Aspire.Notification.Application.Email.Commands.SendEmail;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Aspire.Notification.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => 
                cfg.RegisterServicesFromAssemblyContaining<SendEmailCommand>());
            return services;
        }
    }
}
