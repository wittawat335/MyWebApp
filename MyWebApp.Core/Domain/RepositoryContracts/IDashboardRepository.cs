using MyWebApp.Core.Model.ViewModels.Dashboard;

namespace MyWebApp.Core.Domain.RepositoryContracts
{
    public interface IDashboardRepository
    {
        ChartsSP getChartsR3();
    }
}
