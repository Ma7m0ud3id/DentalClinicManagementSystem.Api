using Microsoft.AspNetCore.OpenApi;

namespace DentalClinicManagementSystem.Apis.Extensions
{
    public static class OpenApiExtensions
    {
        public static IServiceCollection AddOpenApiWithBearer(this IServiceCollection services)
        {
            services.AddOpenApi();
            return services;
        }
    }
}