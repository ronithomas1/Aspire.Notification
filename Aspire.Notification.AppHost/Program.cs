var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Aspire_Notification_Api>("aspire-notification-api");

builder.Build().Run();
