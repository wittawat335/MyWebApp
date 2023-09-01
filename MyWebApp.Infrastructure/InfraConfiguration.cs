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
        public static void InjectDependence(this IServiceCollection services, IConfiguration configuration, string envName)
        {
            var connectionString = "";
            var server = "";
            var port = "";
            var databaseName = "";
            var user = "";
            var password = "";

            switch (envName)
            {
                case "Development":
                    connectionString = configuration.GetConnectionString(Constants.ConnnectionString.SqlServer);
                    break;

                case "Staging":
                    server = Environment.GetEnvironmentVariable("DB_HOST");
                    port = Environment.GetEnvironmentVariable("DB_POST") ?? "1433";
                    databaseName = Environment.GetEnvironmentVariable("DB_NAME");
                    user = Environment.GetEnvironmentVariable("DB_USER") ?? "sa";
                    password = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
                    connectionString =
                        $"Server={server},{port};" +
                        $"Database={databaseName};" +
                        $"User ID={user};" +
                        $"Password={password};" +
                        $"TrustServerCertificate=True;Trusted_Connection=false;MultipleActiveResultSets=True;";
                    break;

                case "Production":
                    connectionString = configuration.GetConnectionString(Constants.ConnnectionString.SqlServer);
                    break;
            }

            //DBConnection
            services.AddDbContext<NCLS_SITContext>(opt => opt.UseSqlServer(connectionString));
            //services.AddDbContext<NCLS_SITContext>(opt =>
            //{
            //    opt.UseSqlServer(connectionString,
            //        sqlServerOptionsAction: sqlOptions =>
            //        {
            //            sqlOptions.EnableRetryOnFailure(
            //            maxRetryCount: 10,
            //            maxRetryDelay: TimeSpan.FromSeconds(30),
            //            errorNumbersToAdd: null);
            //        });
            //});
            services.AddSingleton<DapperContext>(); // Dapper


            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IProgramRepository, ProgramRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<ITodoListRepository, TodoListRepository>();
            services.AddScoped<IReceiveCarRepository, ReceiveCarRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
        }
    }
}
