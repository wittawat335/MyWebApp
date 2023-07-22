using AutoMapper;
using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Domain.RepositoryContracts;
using MyWebApp.Core.DTO;
using MyWebApp.Core.Services.Contract;

namespace MyWebApp.Core.Services
{
    public class ProgramService : IProgramService
    {
        private readonly IGenericRepository<M_PROGRAM> _programRepository;
        private readonly IGenericRepository<M_PERMISSION> _perRepository;
        private readonly IProgramRepository _repository; 
        private readonly IMapper _mapper;

        public ProgramService(IGenericRepository<M_PROGRAM> programRepository, IGenericRepository<M_PERMISSION> perRepository,
            IProgramRepository repository, IMapper mapper)
        {
            _repository = repository;
            _perRepository = perRepository;
            _programRepository = programRepository;
            _mapper = mapper;
        }

        public async Task<List<M_PROGRAM>> GetAll()
        {
            try
            {
                var list = await _programRepository.GetAll();
                return list.ToList();
            }
            catch
            {
                throw;
            }
        }
        public async Task<M_PROGRAM> GetByCode(string code)
        {
            try
            {
                var query = await 
                    _programRepository.Get(x => x.PROG_CODE == code);
                return query;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> Add(M_PROGRAM model)
        {
            try
            {
                var query = await 
                    _programRepository.Add(model);
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> Update(M_PROGRAM model)
        {
            bool result = false;
            try
            {
                var query = await _programRepository
                    .Get(x => x.PROG_CODE == model.PROG_CODE);
                if (query != null)
                {
                    query.PROG_NAME = model.PROG_NAME;
                    query.PROG_LEVEL = model.PROG_LEVEL;
                    query.PROG_PARENT_CODE = model.PROG_PARENT_CODE;
                    query.PROG_TARGET_URL = model.PROG_TARGET_URL;
                    query.PROG_ORDER = model.PROG_ORDER;
                    query.PROG_ICON = model.PROG_ICON;
                    query.PROG_CREATE_BY = model.PROG_CREATE_BY;
                    query.PROG_CREATE_DATE = model.PROG_CREATE_DATE;
                    query.PROG_UPDATE_BY = model.PROG_UPDATE_BY;
                    query.PROG_UPDATE_DATE = model.PROG_UPDATE_DATE;
                    query.PROG_STATUS = model.PROG_STATUS;

                    result = await _programRepository.Update(query);
                }

                return result;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> Delete(string code)
        {
            try
            {
                var query = await 
                    _programRepository.Get(x => x.PROG_CODE == code);
                if (query == null)
                    throw new TaskCanceledException("No Data");

                bool deleted = await _programRepository.Delete(query);

                return deleted;
            }
            catch
            {
                throw;
            }
        }       
        public List<ProgramDTO> GetByRole(string code)
        {
            try
            {
                IQueryable<M_PROGRAM> tbUser = _programRepository
                    .GetListBySP($"SP_GET_MENU_BY_ROLE_TEST {code}").AsQueryable();

                var list = tbUser.ToList();
                return _mapper.Map<List<ProgramDTO>>(list);
            }
            catch 
            {
                throw;
            }
        }
        public async Task<string> GetMeneDefaultAsync(string code)
        {
            string url = "";
            try
            {
                var result = await _repository.GetMenuDefault(code);
                if (result != null)
                    url = result;
                return url;
            }
            catch
            {
                return "~/Home/Index";
            }
        }

        public async Task<List<ProgramDTO>> GetByRoleAsync(string code)
        {
            try
            {
                var list = await _repository.GetMenuByRole(code);
                return list;
            }
            catch
            {
                throw;
            }
        }
    }
}
