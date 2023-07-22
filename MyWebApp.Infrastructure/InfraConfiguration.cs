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
        public static void InjectDependence(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<NCLS_SITContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(Constants.ConnnectionString.SqlServer));
            });
            services.AddTransient(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            services.AddScoped<IProgramRepository, ProgramRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<ITodoListRepository, TodoListRepository>();
            services.AddScoped<IReceiveCarRepository, ReceiveCarRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
        }
    }
}
