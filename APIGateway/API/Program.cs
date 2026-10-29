
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace API
{
    public class Program
    {
        public static async Task Main(
            string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Load Ocelot configuration
            builder.Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

            // Configure CORS to allow any origin
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            // Register Ocelot
            builder.Services.AddOcelot(builder.Configuration);

            var app = builder.Build();

            // Apply CORS before Ocelot middleware
            app.UseCors("AllowAll");

            // Configure API Gateway
            await app.UseOcelot();

            await app.RunAsync();
        }
    }
}
