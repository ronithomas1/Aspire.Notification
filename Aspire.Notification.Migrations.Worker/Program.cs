using Aspire.Notification.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Aspire.Notification.Migrations.Worker;

// Refer this project for migrations sample
// https://github.com/dotnet/aspire-samples/blob/main/samples/DatabaseMigrations/README.md
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
        //  builder.AddSqlServerDbContext<AspireContext>("Aspire-Notification");
        // https://github.com/dotnet/aspire-samples/blob/main/samples/DatabaseMigrations/README.md
        builder.Services.AddDbContextPool<AspireContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("Aspire-Notification"), sqlOptions =>
       {
           sqlOptions.MigrationsAssembly("Aspire.Notification.Migrations.Worker");
           // Workaround for https://github.com/dotnet/aspire/issues/1023
           sqlOptions.ExecutionStrategy(c => new RetryingSqlServerRetryingExecutionStrategy(c));
       }));
        builder.EnrichSqlServerDbContext<AspireContext>(settings =>
        // Disable Aspire default retries as we're using a custom execution strategy
        settings.DisableRetry = true);

        var host = builder.Build();
        host.Run();
    }
}