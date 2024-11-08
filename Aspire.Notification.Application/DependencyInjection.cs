using Aspire.Notification.Application.Common.Behaviours;
using Aspire.Notification.Application.Email.Commands.SendEmail;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Aspire.Notification.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssemblyContaining<SendEmailCommand>();
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            } );
           
            return services;
        }
    }
}
