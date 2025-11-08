using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiAppCharca.Infrastructure.Data;
using MiAppCharca.Infrastructure.Repositories;
using MiAppCharca.Application.Interfaces;

namespace MiAppCharca.Infrastructure.Configuration
{
    public static class InfrastructureServicesExtensions
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // ===== DATABASE CONNECTION =====
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            services.AddDbContext<TicketeraDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
                // Opcional: agregar logging para queries SQL
                options.EnableSensitiveDataLogging(); // Solo para desarrollo
            });

            // ===== REPOSITORIES REGISTRATION =====
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IResponseRepository, ResponseRepository>();
            
            // Si implementas Unit of Work (opcional)
            // services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}