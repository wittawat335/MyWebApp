using AutoMapper;
using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Domain.RepositoryContracts;
using MyWebApp.Core.DTO;
using MyWebApp.Core.Services.Contract;
using MyWebApp.Core.Utility;

namespace MyWebApp.Core.Services
{
    public class ProgramService : IProgramService
    {
        private readonly IGenericRepository<M_PROGRAM> _programRepository;
        private readonly IGenericRepository<M_PERMISSION> _perRepository;
        private readonly IGenericRepository<M_ACTION> _actRepository;
        private readonly IProgramRepository _repository;
        private readonly IMapper _mapper;
        Common common = new Common();

        public ProgramService(IGenericRepository<M_PROGRAM> programRepository, IGenericRepository<M_ACTION> actRepository, IGenericRepository<M_PERMISSION> perRepository,
            IProgramRepository repository, IMapper mapper)
        {
            _repository = repository;
            _perRepository = perRepository;
            _programRepository = programRepository;
            _mapper = mapper;
            _actRepository = actRepository;
        }

        public async Task<List<M_PROGRAM>> GetAll()
        {
            try
            {
                var list = await _programRepository.GetAll(x => x.PROG_TARGET_URL != "#");
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
                var updateProgram = await _programRepository.Get(x => x.PROG_CODE == model.PROG_CODE);
                if (updateProgram != null)
                {
                    if (model.PROG_STATUS != updateProgram.PROG_STATUS)
                    {
                        var updatePerMission = await _perRepository.GetAll(x => x.PERM_PROG_CODE == model.PROG_CODE);
                        var updateAction = await _actRepository.GetAll(x => x.ACT_PROG_CODE == model.PROG_CODE);
                        if (updatePerMission.ToList().Count() > 0) //Update Status M_PROGRAM
                        {
                            foreach (var item in updatePerMission) 
                            {
                                item.PERM_STATUS = model.PROG_STATUS;
                                item.PERM_UPDATE_BY = model.PROG_UPDATE_BY;
                                item.PERM_UPDATE_DATE = model.PROG_UPDATE_DATE;

                               await _perRepository.Update(item);
                            }
                        }
                        if (updateAction.ToList().Count() > 0) //Update Status M_ACTION
                        {
                            foreach (var item in updateAction)
                            {
                                item.ACT_STATUS = model.PROG_STATUS;
                                item.ACT_UPDATE_BY = model.PROG_UPDATE_BY;
                                item.ACT_UPDATE_DATE = model.PROG_UPDATE_DATE;

                                await _actRepository.Update(item);
                            }
                        }
                    }
                    updateProgram.PROG_NAME = model.PROG_NAME;
                    updateProgram.PROG_LEVEL = model.PROG_LEVEL;
                    updateProgram.PROG_PARENT_CODE = model.PROG_PARENT_CODE;
                    updateProgram.PROG_TARGET_URL = model.PROG_TARGET_URL;
                    updateProgram.PROG_ORDER = model.PROG_ORDER;
                    updateProgram.PROG_ICON = model.PROG_ICON;
                    updateProgram.PROG_CREATE_BY = model.PROG_CREATE_BY;
                    updateProgram.PROG_CREATE_DATE = model.PROG_CREATE_DATE;
                    updateProgram.PROG_UPDATE_BY = model.PROG_UPDATE_BY;
                    updateProgram.PROG_UPDATE_DATE = model.PROG_UPDATE_DATE;
                    updateProgram.PROG_STATUS = model.PROG_STATUS;

                    result = await _programRepository.Update(updateProgram);
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
