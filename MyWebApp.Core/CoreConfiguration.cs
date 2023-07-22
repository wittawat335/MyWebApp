using Microsoft.Extensions.DependencyInjection;
using MyWebApp.Core.AutoMapper;
using MyWebApp.Core.Services.Contract;
using MyWebApp.Core.Services;

namespace MyWebApp.Core
{
    public static class CoreConfiguration
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddScoped<IProgramService, ProgramService>();
            services.AddScoped<IMasterService, MasterService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IParameterService, ParameterService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<ITodoListService, TodoListService>();
            services.AddScoped<IReceiveCarService, ReceiveCarService>();
            services.AddScoped<IPermissionService, PermissionService>();
        }
    }
}
