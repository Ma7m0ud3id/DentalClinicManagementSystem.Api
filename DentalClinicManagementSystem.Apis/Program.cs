using DentalClinicManagementSystem.Apis.Extensions;
using DentalClinicManagementSystem.Apis.Middleware;
using DentalClinicManagementSystem.BLL.Extensions;
using Scalar.AspNetCore;

namespace DentalClinicManagementSystem.Apis
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services
            builder.Services.AddControllers();
            builder.Services.AddOpenApiWithBearer();
            builder.Services.AddBusinessLayer(builder.Configuration);

            var app = builder.Build();

            // Initialize database (migrate + seed)
            await app.InitializeDatabaseAsync();

            // Middleware pipeline order is critical
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                {
                    options.WithTitle("Dental Clinic API");
                    options.WithTheme(ScalarTheme.DeepSpace);
                    options.WithDefaultHttpClient(ScalarTarget.Http, ScalarClient.Http11);
                });
            }

            app.MapControllers();

            await app.RunAsync();
        }
    }
}
