using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyWebApp.Core.Domain.RepositoryContracts;
using MyWebApp.Core.Utility;
using MyWebApp.Infrastructure.DBContext;
using MyWebApp.Infrastructure.Repositories;
using System.Runtime.CompilerServices;

namespace MyWebApp.Infrastructure
{
    public static class InfraConfiguration
    {
        public static void InjectDependence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = "";
            var server =
                Environment.GetEnvironmentVariable("DB_HOST") ?? configuration[Constants.DatabaseSetting.SqlServer.DBServer];
            var port =
                Environment.GetEnvironmentVariable("DB_POST") ?? "1433";
            var databaseName =
                Environment.GetEnvironmentVariable("DB_NAME") ?? configuration[Constants.DatabaseSetting.SqlServer.DatabaseName];
            var user =
                configuration[Constants.DatabaseSetting.SqlServer.DBUser] ?? "sa";
            var password =
                Environment.GetEnvironmentVariable("DB_SA_PASSWORD") ?? configuration[Constants.DatabaseSetting.SqlServer.DBPassword];

            if (port != "")
                connectionString =
                    $"Server={server},{port};" +
                    $"Database={databaseName};" +
                    $"User ID={user};" +
                    $"Password={password};" +
                    $"TrustServerCertificate=True;Trusted_Connection=false;MultipleActiveResultSets=True;";
            else connectionString =
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
