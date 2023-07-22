using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Domain.RepositoryContracts;
using MyWebApp.Core.Model.ViewModels.TodoList;
using MyWebApp.Core.Services.Contract;

namespace MyWebApp.Core.Services
{
    public class TodoListService : ITodoListService
    {
        private readonly IGenericRepository<M_USER> _repository;
        private readonly IGenericRepository<M_USER_ROLE> _roleRepository;
        private readonly IGenericRepository<M_OA> _oaRepository;
        private readonly IGenericRepository<M_LEGAL_CASE> _legalRepository;
        private readonly IGenericRepository<T_JOB_ALL> _jobRepository;
        private readonly ITodoListRepository _todoListRepository;
        public TodoListService(
            ITodoListRepository todoListRepository,
            IGenericRepository<M_USER> repository,
            IGenericRepository<M_USER_ROLE> roleRepository,
            IGenericRepository<M_OA> oaRepository,
            IGenericRepository<M_LEGAL_CASE> legalRepository,
            IGenericRepository<T_JOB_ALL> jobRepository
            )
        {
            _repository = repository;
            _roleRepository = roleRepository;
            _oaRepository = oaRepository;
            _legalRepository = legalRepository;
            _jobRepository = jobRepository;
            _todoListRepository = todoListRepository;
        }
        public async Task<List<T_JOB_ALL>> Search(SearchViewModel model)
        {
            try
            { 
                var list = await _jobRepository.GetAll();
                if (model.refNo != null)
                    list = list.Where(x => 
                    x.JOB_ID == model.refNo);
                if (model.contractNo != null)
                    list = list.Where(x => 
                    x.JOB_CONTRACT_NO == model.contractNo);
                if (model.adminCode != null)
                    list = list.Where(x =>
                    x.JOB_ADMIN_CODE == model.adminCode);
                if (model.oaCode != null)
                    list = list.Where(x =>
                    x.JOB_OA_CODE  == model.oaCode);
                if (model.caseCode != null)
                    list = list.Where(x =>
                    x.JOB_CASE_CODE == model.caseCode);
                if (model.succStatus != null)
                    list = list.Where(x =>
                    x.JOB_STATUS == model.succStatus);
                if (model.adminDateStart != null && 
                    model.adminDateEnd != null)
                {
                    DateTime from = DateTime
                        .ParseExact(model.adminDateStart,
                        "dd/MM/yyyy", null);
                    DateTime to = DateTime
                        .ParseExact(model.adminDateEnd,
                        "dd/MM/yyyy", null);
                    list = list.Where(x =>
                    x.JOB_ASSIGN_ADMIN_DATE >= from &&
                    x.JOB_ASSIGN_ADMIN_DATE <= to);
                }
           

                return list.ToList();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<RefNo>> GetRefNo(string code)
        {
            try
            {
                var result = await _todoListRepository
                    .GetRefNoAsync(code);

                var list = result.DistinctBy(x => x.JOB_ID).ToList();
                return list;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<M_LEGAL_CASE>> GetListLegalCaseByGroup(
            string group = "")
        {
            try
            {
                var list = await _legalRepository
                    .GetAll(x => x.CASE_GROUP_CODE.Contains(group));
                list.OrderByDescending(x => x.CASE_SEQ);

                return list.ToList();
            }
            catch {
                throw;
            }
        }

        public async Task<List<M_OA>> GetListOA(string code)
        {
            try
            {
                var list = await _oaRepository
                    .GetAll(x => x.OA_CODE.Contains(code));
                return list.ToList();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<M_USER>> GetListUserByRole(
            string code = "")
        {
            IQueryable<M_USER> m_USERs = await _repository.GetAll();
            IQueryable<M_USER_ROLE> m_USER_ROLEs = await 
                _roleRepository.GetAll();
            var list = new List<M_USER>();
            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    var query = await _repository.GetAll();
                    list = query.ToList();
                }
                else
                {
                    var query = (from m in m_USERs
                                 from d in m_USER_ROLEs
                                    .Where(d =>
                                    d.USERROLE_USER_LOGIN 
                                    == m.USER_LOGIN &&
                                    d.USERROLE_ROLE_CODE 
                                    == code)
                                select m
                            );
                    list = query.ToList();
                }
                    return list;
            }
            catch {
                throw;
            }
        }

        public async Task<List<SP_SEARCH_CUSTOMER_Result>> GetCustomer(string code = "")
        {
            try
            {
                try
                {
                    var result = await _todoListRepository
                        .GetCustomer(code);
                   
                    return result;
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
