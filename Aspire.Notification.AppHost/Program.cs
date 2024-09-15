var builder = DistributedApplication.CreateBuilder(args);
var smtp = builder.AddSmtp4Dev("SmtpUri");
builder.AddProject<Projects.Aspire_Notification_Api>("aspire-notification-api");

builder.Build().Run();
