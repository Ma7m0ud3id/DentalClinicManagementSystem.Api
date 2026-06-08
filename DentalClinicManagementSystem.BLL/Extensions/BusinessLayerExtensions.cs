using DentalClinicManagementSystem.BLL.CurrentUser;
using DentalClinicManagementSystem.BLL.Security;
using DentalClinicManagementSystem.BLL.Services.Implementations;
using DentalClinicManagementSystem.BLL.Services.Interfaces;
using DentalClinicManagementSystem.DAL.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DentalClinicManagementSystem.BLL.Extensions
{
    public static class BusinessLayerExtensions
    {
        public static IServiceCollection AddBusinessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataAccess(configuration);

            // Configure JWT Settings
            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

            // Add HTTP Context Accessor
            services.AddHttpContextAccessor();

            // Register Current User Service
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            // Register Auth Service
            services.AddScoped<IAuthService, AuthService>();

            // Register Doctor Service
            services.AddScoped<IDoctorService, DoctorService>();

            // Register Patient Service
            services.AddScoped<IPatientService, PatientService>();

            // Register Appointment Service
            services.AddScoped<IAppointmentService, AppointmentService>();

            // Register Visit Service
            services.AddScoped<IVisitService, VisitService>();

            // Register Dashboard Service
            services.AddScoped<IDashboardService, DashboardService>();

            // Register User Service
            services.AddScoped<IUserService, UserService>();

            // Register Patient Auth Service
            services.AddScoped<IPatientAuthService, PatientAuthService>();

            // Register validators from assembly
            services.AddValidatorsFromAssemblyContaining<AuthService>();

            // Configure JWT Authentication
            var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();
            if (jwtSettings != null)
            {
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = jwtSettings.Issuer,
                            ValidAudience = jwtSettings.Audience,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                            ClockSkew = TimeSpan.Zero
                        };
                    });
            }

            // Add Authorization
            services.AddAuthorization();

            return services;
        }
    }
}