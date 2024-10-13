using Aspire.Notification.Infrastructure.Database;

namespace Aspire.Notification.Migrations.Worker;

// Refer this project for migrations sample
// https://learn.microsoft.com/en-us/dotnet/aspire/database/ef-core-migrations
public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.AddServiceDefaults();
        builder.Services.AddHostedService<DbInitializerWorker>();

        builder.Services.AddOpenTelemetry()
        .WithTracing(tracing => tracing.AddSource(DbInitializerWorker.ActivitySourceName));

        // This is the name of the sqlDatabase in AppHost
        builder.AddSqlServerDbContext<AspireContext>("Aspire-Notification");

        var host = builder.Build();
        host.Run();
    }
}