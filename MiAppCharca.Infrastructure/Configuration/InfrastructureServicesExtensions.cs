using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiAppCharca.Infrastructure.Data;
using MiAppCharca.Infrastructure.Repositories;
using MiAppCharca.Application.Interfaces;
using System;

namespace MiAppCharca.Infrastructure.Configuration
{
    public static class InfrastructureServicesExtensions
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // ===== DATABASE CONNECTION =====
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            string connectionString;

            if (!string.IsNullOrEmpty(databaseUrl))
            {
                // Producción (Render) - PostgreSQL
                connectionString = ConvertPostgresUrl(databaseUrl);
                services.AddDbContext<TicketeraDbContext>(options =>
                {
                    options.UseNpgsql(connectionString);
                });
            }
            else
            {
                // Desarrollo local - SQL Server
                connectionString = configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<TicketeraDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                    options.EnableSensitiveDataLogging();
                });
            }

            // ===== REPOSITORIES =====
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IResponseRepository, ResponseRepository>();

            return services;
        }

        private static string ConvertPostgresUrl(string databaseUrl)
        {
            var uri = new Uri(databaseUrl);
            return $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.Trim('/')};Username={uri.UserInfo.Split(':')[0]};Password={uri.UserInfo.Split(':')[1]};SSL Mode=Require;Trust Server Certificate=true";
        }
    }
}