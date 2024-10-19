using Aspire.Notification.Domain.Template;
using Aspire.Notification.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Threading;

namespace Aspire.Notification.Migrations.Worker;

public class DbInitializerWorker : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);
    private readonly ILogger<DbInitializerWorker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    public DbInitializerWorker(ILogger<DbInitializerWorker> logger,
        IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _hostApplicationLifetime = hostApplicationLifetime;

    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AspireContext>();

            await EnsureDatabaseAsync(dbContext, cancellationToken);
            await RunMigrationAsync(dbContext, cancellationToken);
            await SeedDataAsync(dbContext, cancellationToken);
        }
        catch (Exception ex)
        {
            activity?.RecordException(ex);
            throw;
        }
        _hostApplicationLifetime.StopApplication();
    }

    private static async Task EnsureDatabaseAsync(AspireContext dbContext, CancellationToken cancellationToken)
    {
        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Create the database if it does not exist.
            // Do this first so there is then a database to start a transaction against.
            if (!await dbCreator.ExistsAsync(cancellationToken))
            {
                await dbCreator.CreateAsync(cancellationToken);
            }
        });
    }
    private static async Task RunMigrationAsync(AspireContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Run migration in a transaction to avoid partial migration if it fails.
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            await dbContext.Database.MigrateAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }

    private static async Task SeedDataAsync(AspireContext dbContext, CancellationToken cancellationToken)
    {
        // Todo - Seed Data
        Template emailTemplate = new()
        {
            Type = "Email",
            Name = "Default",
            Description= "This is the default Email Template",
            From ="noreply@aspiretest.com",
            DisplayName = "Grace Corp",
            NotificationTemplate = "<!doctype html>\r\n<html lang=\\\"en\\\">\r\n<head>\r\n    <meta name=\\\"viewport\\\" content=\\\"width=device-width, initial-scale=1.0\\\">\r\n    <meta http-equiv=\\\"Content-Type\\\" content=\\\"text/html; charset=UTF-8\\\">\r\n    <title>Simple Transactional Email</title>\r\n    <style media=\\\"all\\\" type=\\\"text/css\\\">\r\n                /* -------------------------------------\r\n            GLOBAL RESETS\r\n        ------------------------------------- */\r\n\r\n                body {\r\n                    font-family: Helvetica, sans-serif;\r\n                    -webkit-font-smoothing: antialiased;\r\n                    font-size: 16px;\r\n                    line-height: 1.3;\r\n                    -ms-text-size-adjust: 100%;\r\n                    -webkit-text-size-adjust: 100%;\r\n                }\r\n\r\n                table {\r\n                    border-collapse: separate;\r\n                    mso-table-lspace: 0pt;\r\n                    mso-table-rspace: 0pt;\r\n                    width: 100%;\r\n                }\r\n\r\n                    table td {\r\n                        font-family: Helvetica, sans-serif;\r\n                        font-size: 16px;\r\n                        vertical-align: top;\r\n                    }\r\n                /* -------------------------------------\r\n            BODY & CONTAINER\r\n        ------------------------------------- */\r\n\r\n                body {\r\n                    background-color: #f4f5f6;\r\n                    margin: 0;\r\n                    padding: 0;\r\n                }\r\n\r\n                .body {\r\n                    background-color: #f4f5f6;\r\n                    width: 100%;\r\n                }\r\n\r\n                .container {\r\n                    margin: 0 auto !important;\r\n                    max-width: 600px;\r\n                    padding: 0;\r\n                    padding-top: 24px;\r\n                    width: 600px;\r\n                }\r\n\r\n                .content {\r\n                    box-sizing: border-box;\r\n                    display: block;\r\n                    margin: 0 auto;\r\n                    max-width: 600px;\r\n                    padding: 0;\r\n                }\r\n                /* -------------------------------------\r\n            HEADER, FOOTER, MAIN\r\n        ------------------------------------- */\r\n\r\n                .main {\r\n                    background: #ffffff;\r\n                    border: 1px solid #eaebed;\r\n                    border-radius: 16px;\r\n                    width: 100%;\r\n                }\r\n\r\n                .wrapper {\r\n                    box-sizing: border-box;\r\n                    padding: 24px;\r\n                }\r\n\r\n                .footer {\r\n                    clear: both;\r\n                    padding-top: 24px;\r\n                    text-align: center;\r\n                    width: 100%;\r\n                }\r\n\r\n                    .footer td,\r\n                    .footer p,\r\n                    .footer span,\r\n                    .footer a {\r\n                        color: #9a9ea6;\r\n                        font-size: 16px;\r\n                        text-align: center;\r\n                    }\r\n                /* -------------------------------------\r\n            TYPOGRAPHY\r\n        ------------------------------------- */\r\n\r\n                p {\r\n                    font-family: Helvetica, sans-serif;\r\n                    font-size: 16px;\r\n                    font-weight: normal;\r\n                    margin: 0;\r\n                    margin-bottom: 16px;\r\n                }\r\n\r\n                a {\r\n                    color: #0867ec;\r\n                    text-decoration: underline;\r\n                }\r\n                /* -------------------------------------\r\n            BUTTONS\r\n        ------------------------------------- */\r\n\r\n                .btn {\r\n                    box-sizing: border-box;\r\n                    min-width: 100% !important;\r\n                    width: 100%;\r\n                }\r\n\r\n                    .btn > tbody > tr > td {\r\n                        padding-bottom: 16px;\r\n                    }\r\n\r\n                    .btn table {\r\n                        width: auto;\r\n                    }\r\n\r\n                        .btn table td {\r\n                            background-color: #ffffff;\r\n                            border-radius: 4px;\r\n                            text-align: center;\r\n                        }\r\n\r\n                    .btn a {\r\n                        background-color: #ffffff;\r\n                        border: solid 2px #0867ec;\r\n                        border-radius: 4px;\r\n                        box-sizing: border-box;\r\n                        color: #0867ec;\r\n                        cursor: pointer;\r\n                        display: inline-block;\r\n                        font-size: 16px;\r\n                        font-weight: bold;\r\n                        margin: 0;\r\n                        padding: 12px 24px;\r\n                        text-decoration: none;\r\n                        text-transform: capitalize;\r\n                    }\r\n\r\n                .btn-primary table td {\r\n                    background-color: #0867ec;\r\n                }\r\n\r\n                .btn-primary a {\r\n                    background-color: #0867ec;\r\n                    border-color: #0867ec;\r\n                    color: #ffffff;\r\n                }\r\n\r\n                @media all {\r\n                    .btn-primary table td:hover {\r\n                        background-color: #ec0867 !important;\r\n                    }\r\n\r\n                    .btn-primary a:hover {\r\n                        background-color: #ec0867 !important;\r\n                        border-color: #ec0867 !important;\r\n                    }\r\n                }\r\n\r\n                /* -------------------------------------\r\n            OTHER STYLES THAT MIGHT BE USEFUL\r\n        ------------------------------------- */\r\n\r\n                .last {\r\n                    margin-bottom: 0;\r\n                }\r\n\r\n                .first {\r\n                    margin-top: 0;\r\n                }\r\n\r\n                .align-center {\r\n                    text-align: center;\r\n                }\r\n\r\n                .align-right {\r\n                    text-align: right;\r\n                }\r\n\r\n                .align-left {\r\n                    text-align: left;\r\n                }\r\n\r\n                .text-link {\r\n                    color: #0867ec !important;\r\n                    text-decoration: underline !important;\r\n                }\r\n\r\n                .clear {\r\n                    clear: both;\r\n                }\r\n\r\n                .mt0 {\r\n                    margin-top: 0;\r\n                }\r\n\r\n                .mb0 {\r\n                    margin-bottom: 0;\r\n                }\r\n\r\n                .preheader {\r\n                    color: transparent;\r\n                    display: none;\r\n                    height: 0;\r\n                    max-height: 0;\r\n                    max-width: 0;\r\n                    opacity: 0;\r\n                    overflow: hidden;\r\n                    mso-hide: all;\r\n                    visibility: hidden;\r\n                    width: 0;\r\n                }\r\n\r\n                .powered-by a {\r\n                    text-decoration: none;\r\n                }\r\n\r\n                /* -------------------------------------\r\n            RESPONSIVE AND MOBILE FRIENDLY STYLES\r\n        ------------------------------------- */\r\n\r\n                @media only screen and (max-width: 640px) {\r\n                    .main p,\r\n                    .main td,\r\n                    .main span {\r\n                        font-size: 16px !important;\r\n                    }\r\n\r\n                    .wrapper {\r\n                        padding: 8px !important;\r\n                    }\r\n\r\n                    .content {\r\n                        padding: 0 !important;\r\n                    }\r\n\r\n                    .container {\r\n                        padding: 0 !important;\r\n                        padding-top: 8px !important;\r\n                        width: 100% !important;\r\n                    }\r\n\r\n                    .main {\r\n                        border-left-width: 0 !important;\r\n                        border-radius: 0 !important;\r\n                        border-right-width: 0 !important;\r\n                    }\r\n\r\n                    .btn table {\r\n                        max-width: 100% !important;\r\n                        width: 100% !important;\r\n                    }\r\n\r\n                    .btn a {\r\n                        font-size: 16px !important;\r\n                        max-width: 100% !important;\r\n                        width: 100% !important;\r\n                    }\r\n                }\r\n                /* -------------------------------------\r\n            PRESERVE THESE STYLES IN THE HEAD\r\n        ------------------------------------- */\r\n\r\n                @media all {\r\n                    .ExternalClass {\r\n                        width: 100%;\r\n                    }\r\n\r\n                        .ExternalClass,\r\n                        .ExternalClass p,\r\n                        .ExternalClass span,\r\n                        .ExternalClass font,\r\n                        .ExternalClass td,\r\n                        .ExternalClass div {\r\n                            line-height: 100%;\r\n                        }\r\n\r\n                    .apple-link a {\r\n                        color: inherit !important;\r\n                        font-family: inherit !important;\r\n                        font-size: inherit !important;\r\n                        font-weight: inherit !important;\r\n                        line-height: inherit !important;\r\n                        text-decoration: none !important;\r\n                    }\r\n\r\n                    #MessageViewBody a {\r\n                        color: inherit;\r\n                        text-decoration: none;\r\n                        font-size: inherit;\r\n                        font-family: inherit;\r\n                        font-weight: inherit;\r\n                        line-height: inherit;\r\n                    }\r\n                }\r\n    </style>\r\n</head>\r\n<body>\r\n    <table role=\\\"presentation\\\" border=\\\"0\\\" cellpadding=\\\"0\\\" cellspacing=\\\"0\\\" class=\\\"body\\\">\r\n        <tr>\r\n            <td>&nbsp;</td>\r\n            <td class=\\\"container\\\">\r\n                <div class=\\\"content\\\">\r\n\r\n                    <!-- START CENTERED WHITE CONTAINER -->\r\n                    <span class=\\\"preheader\\\">This is preheader text. Some clients will show this text as a preview.</span>\r\n                    <table role=\\\"presentation\\\" border=\\\"0\\\" cellpadding=\\\"0\\\" cellspacing=\\\"0\\\" class=\\\"main\\\">\r\n\r\n                        ##CONTENT##\r\n                       \r\n                    </table>\r\n\r\n                    <!-- START FOOTER -->\r\n                    <div class=\\\"footer\\\">\r\n                        <table role=\\\"presentation\\\" border=\\\"0\\\" cellpadding=\\\"0\\\" cellspacing=\\\"0\\\">\r\n                            <tr>\r\n                                <td class=\\\"content-block\\\">\r\n                                    <span class=\\\"apple-link\\\">Carved Rock Fitness</span>\r\n                                </td>\r\n                            </tr>                            \r\n                        </table>\r\n                    </div>\r\n\r\n                    <!-- END FOOTER -->\r\n                    <!-- END CENTERED WHITE CONTAINER -->\r\n                </div>\r\n            </td>\r\n            <td>&nbsp;</td>\r\n        </tr>\r\n    </table>\r\n</body>\r\n</html>",
            IsActive= true,
            CreatedDate = DateTime.UtcNow
        };
        //    dbContext.Add(emailTemplate);
        //    await dbContext.SaveChangesAsync();
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Seed the database
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            await dbContext.Template.AddAsync(emailTemplate, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }
}
