using Aspire.Notification.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Aspire.Notification.Api.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using AspireContext dbContext =
                scope.ServiceProvider.GetRequiredService<AspireContext>();

            dbContext.Database.Migrate();
        }
    }
}
