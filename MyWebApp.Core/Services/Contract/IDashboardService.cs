using MyWebApp.Core.Model.ViewModels.Dashboard;

namespace MyWebApp.Core.Services.Contract
{
    public interface IDashboardService
    {
        Task<List<ChartsSP>> GetChartsR3();
        Task<List<ChartsSP>> GetChartsRepo();
        Task<List<object>> GetTotal();
        Task<List<Charts>> ChartsR3();
        Task<List<Charts>> ChartsRepo();
    }
}
