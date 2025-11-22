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

            if (!string.IsNullOrEmpty(databaseUrl))
            {
                // Producción (Render) - PostgreSQL
                var connectionString = ConvertPostgresUrl(databaseUrl);
                services.AddDbContext<TicketeraDbContext>(options =>
                {
                    options.UseNpgsql(connectionString);
                });
            }
            else
            {
                // Desarrollo local - SQL Server
                var connectionString = configuration.GetConnectionString("DefaultConnection");
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
            // Render puede dar formato: 
            // postgres://user:password@host:port/dbname
            // o postgres://user:password@host/dbname (sin puerto)
            
            try
            {
                var uri = new Uri(databaseUrl);
                var host = uri.Host;
                var database = uri.AbsolutePath.TrimStart('/');
                var userInfo = uri.UserInfo.Split(':');
                var username = userInfo[0];
                var password = userInfo.Length > 1 ? userInfo[1] : "";
                
                // Puerto: si no está especificado, usar 5432 (default de PostgreSQL)
                var port = uri.Port > 0 ? uri.Port : 5432;

                return $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true";
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al convertir DATABASE_URL: {ex.Message}", ex);
            }
        }
    }
}