using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyWebApp.Core.Domain.RepositoryContracts;
using MyWebApp.Core.Utility;
using MyWebApp.Infrastructure.DBContext;
using MyWebApp.Infrastructure.Repositories;

namespace MyWebApp.Infrastructure
{
    public static class InfraConfiguration
    {
        public static void InjectDependence(this IServiceCollection services, IConfiguration configuration)
        {
            //var port = configuration["DatabaseSetting:SqlServer:DBPort"] ?? "5655";
            var server = configuration[Constants.DatabaseSetting.SqlServer.DBServer];
            var databaseName = configuration[Constants.DatabaseSetting.SqlServer.DatabaseName];
            var user = configuration[Constants.DatabaseSetting.SqlServer.DBUser];
            var password = configuration[Constants.DatabaseSetting.SqlServer.DBPassword];
            var connectionString =
                $"Server={server};" +
                $"Database={databaseName};" +
                $"User ID={user};" +
                $"Password={password};" +
                $"TrustServerCertificate=True;Trusted_Connection=True;MultipleActiveResultSets=True;";

            //DBConnection
            services.AddSingleton<DapperContext>(); // Dapper
            services.AddDbContext<NCLS_SITContext>(opt => opt.UseSqlServer(connectionString));

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IProgramRepository, ProgramRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<ITodoListRepository, TodoListRepository>();
            services.AddScoped<IReceiveCarRepository, ReceiveCarRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
        }
    }
}
