
using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Domain.RepositoryContracts;
using MyWebApp.Core.Model.ViewModels.Dashboard;
using MyWebApp.Core.Services.Contract;
using MyWebApp.Core.Utility;

namespace MyWebApp.Core.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IGenericRepository<T_R3_DETAIL> _r3Repository;
        private readonly IGenericRepository<T_JOB_REPO> _repoRepository;
        private readonly IGenericRepository<M_MASTER> _masterRepository;
        private readonly IGenericRepository<M_STATUS> _statusRepository;
        private readonly IDashboardRepository _repository;
        Common common = new Common();

        public DashboardService(IGenericRepository<T_R3_DETAIL> r3Repository, IGenericRepository<T_JOB_REPO> repoRepository,
            IGenericRepository<M_MASTER> masterRepository, IGenericRepository<M_STATUS> statusRepository, IDashboardRepository repository)
        {
            _statusRepository = statusRepository;
            _repoRepository = repoRepository;
            _masterRepository = masterRepository;
            _r3Repository = r3Repository;
            _repository = repository;
        }
        public async Task<List<Charts>> ChartsR3()
        {
            var objList = new List<Charts>();
            try
            {
                List<ChartsSP> result = await GetChartsR3();

                foreach (var item in result)
                {
                    objList.Add(new Charts()
                    {
                        id = item.ID,
                        text = item.TEXT,
                        value = common.UnInt32Null(item.VALUE),
                        color = item.COLOR
                    });
                }

                return objList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Charts>> ChartsRepo()
        {
            var objList = new List<Charts>();
            try
            {
                List<ChartsSP> result = await GetChartsRepo();

                foreach (var item in result)
                {
                    objList.Add(new Charts()
                    {
                        id = item.ID,
                        text = item.TEXT,
                        value = common.UnInt32Null(item.VALUE),
                        color = item.COLOR
                    });
                }

                return objList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<object>> GetTotal()
        {
            IQueryable<T_R3_DETAIL> tbR3 = await _r3Repository.GetAll(x => x.R3_CASE_STATUS == "R330");
            IQueryable<M_MASTER> tbMaster = await _masterRepository.GetAll();
            List<object> list = new List<object>();
            List<string> label = tbR3.Select(x => x.R3_CASE_STATUS).ToList();
            var total = tbR3.Count();
            list.Add(label);
            list.Add(total);
            return list;
        }
        public async Task<List<ChartsSP>> GetChartsR3()
        {
            var caseStatus = new[] { "R320", "R330", "R340" };
            IQueryable<T_R3_DETAIL> tbR3 = await _r3Repository
                .GetAll(x => caseStatus.Contains(x.R3_CASE_STATUS));
            IQueryable<M_MASTER> tbMaster = await _masterRepository.GetAll();
            try
            {
                var r3 = (from x in tbR3
                          group x by x.R3_CASE_STATUS into g
                          select new ChartsSP
                          {
                              ID = g.Key,
                              TEXT = (from y in tbMaster
                                      where
                                      y.MASTER_CODE.Contains(g.Key) &&
                                      y.MASTER_TYPE == "R3Status"
                                      select y.MASTER_NAME_TH)
                                      .FirstOrDefault(),
                              VALUE = g.Count(),
                              COLOR = "#000000"
                          }).ToList();

                return r3;
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<ChartsSP>> GetChartsRepo()
        {
            try
            {
                var caseStatus = new[] { "RE01", "RE02", "RE03","RE04" };
                IQueryable<T_JOB_REPO> tbR3 = await _repoRepository
                    .GetAll(x => caseStatus.Contains(x.JOB_REPO_STATUS));
                IQueryable<M_STATUS> tb = await _statusRepository
                    .GetAll(x => x.STS_STATUS == "A");
                try
                {
                    var list = (from x in tbR3
                              group x by x.JOB_REPO_STATUS into g
                              select new ChartsSP
                              {
                                  ID = g.Key,
                                  TEXT = (from y in tb
                                          where
                                          y.STS_CODE.Contains(g.Key) 
                                          select y.STS_NAME_TH)
                                          .FirstOrDefault(),
                                  VALUE = g.Count(),
                                  COLOR = "#000000"
                              }).ToList();

                    return list;
                }
                catch
                {
                    throw;
                }

            }
            catch
            {
                throw;
            }
        }
    }
}
