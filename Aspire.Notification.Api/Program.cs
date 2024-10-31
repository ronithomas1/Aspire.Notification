using Aspire.Notification.Application;
using Aspire.Notification.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using System.Diagnostics;

namespace Aspire.Notification.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.
     
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services  
            .AddInfrastructureServices(builder.Configuration, builder,
            builder.Environment.IsDevelopment());
        builder.Services
            .AddApplicationServices();

        var app = builder.Build();

        app.MapDefaultEndpoints();
        app.UseSerilogRequestLogging();

        app.UseExceptionHandler();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
