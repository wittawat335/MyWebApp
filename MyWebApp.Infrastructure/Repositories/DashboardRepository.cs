using Microsoft.EntityFrameworkCore;
using MyWebApp.Core.Domain.RepositoryContracts;
using MyWebApp.Core.Model.ViewModels.Dashboard;
using MyWebApp.Infrastructure.DBContext;

namespace MyWebApp.Infrastructure.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly NCLS_SITContext _dbContext;

        public DashboardRepository(NCLS_SITContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ChartsSP getChartsR3()
        {
            ChartsSP chartsR3 = new ChartsSP();
            try
            {
                var test = _dbContext.Database.ExecuteSqlRaw("[dbo].[SP_DASHBOARD_R3]");

                return chartsR3;
            }
            catch 
            {
                throw;
            }
        }
    }
}
