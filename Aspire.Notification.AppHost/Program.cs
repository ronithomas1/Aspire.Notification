var builder = DistributedApplication.CreateBuilder(args);
// Set a known password for the Sql server which for now is being read from the appsettings
var sqlPassword = builder.AddParameter("sql-password");

var sqlServer = builder
                .AddSqlServer("sql", sqlPassword, port:1234)
                .WithDataVolume("Aspire-Notification");
// Adds intial catalog 'Aspire.Notification' to the connection string
var sqlDatabase = sqlServer.AddDatabase("Aspire-Notification");


var smtp = builder.AddSmtp4Dev("SmtpUri");
builder.AddProject<Projects.Aspire_Notification_Api>("aspire-notification-api");

builder.Build().Run();
