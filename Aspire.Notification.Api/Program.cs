
using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;

namespace Aspire.Notification.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.
        builder.Services.AddProblemDetails(opts => // built-in problem details support
            opts.CustomizeProblemDetails = (ctx) =>
            {
                if (!ctx.ProblemDetails.Extensions.ContainsKey("traceId"))
                {
                    string? traceId = Activity.Current?.Id ?? ctx.HttpContext.TraceIdentifier;
                    ctx.ProblemDetails.Extensions.Add(new KeyValuePair<string, object?>("traceId", traceId));
                }
                var exception = ctx.HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
                if (ctx.ProblemDetails.Status == 500)
                {
                    ctx.ProblemDetails.Detail = "An error occurred in our API. Use the trace id when contacting us.";
                }
            }
          );

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.MapDefaultEndpoints();

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
