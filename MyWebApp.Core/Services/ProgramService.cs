using AutoMapper;
using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Domain.RepositoryContracts;
using MyWebApp.Core.DTO;
using MyWebApp.Core.Model;
using MyWebApp.Core.Model.ViewModels.Program;
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
        public async Task<Response<List<M_PROGRAM>>> GetList()
        {
            var response = new Response<List<M_PROGRAM>>();
            try
            {
                response.value = await GetAll();
                response.status = Constants.Status.True;
            }
            catch (Exception ex)
            {
                response.status = Constants.Status.False;
                response.message = ex.Message;
            }

            return response;
        }

        public async Task<ProgramViewModel> Detail(string code, string action)
        {
            ProgramViewModel model = new ProgramViewModel();
            try
            {
                if (code != null)
                    model.program = await GetByCode(code);

                model.action = action;

                return model;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ResponseStatus> Save(ProgramViewModel model)
        {
            var response = new ResponseStatus();
            try
            {
                if (model != null)
                {
                    switch (model.action)
                    {
                        case Constants.Action.New:
                            response.Status = await Add(model.program);
                            response.Message = Constants.StatusMessage.Create_Action;
                            break;

                        case Constants.Action.Edit:
                            response.Status = await Update(_mapper.Map<ProgramDTO>(model.program));
                            response.Message = Constants.StatusMessage.Update_Action;
                            break;

                        default:
                            response.Status = Constants.Status.False;
                            response.Message = Constants.StatusMessage.No_Data;
                            break;
                    }
                }
                else
                {
                    response.Status = Constants.Status.False;
                    response.Message = Constants.StatusMessage.No_Data;
                }
            }
            catch (Exception ex)
            {
                response.Status = Constants.Status.False;
                response.Message = ex.Message;
            }

            return response;
        }
        public async Task<ResponseStatus> sendDelete(string code)
        {
            var response = new ResponseStatus();
            try
            {
                if (code != null)
                {
                    response.Status = await Delete(code);
                    response.Message = Constants.StatusMessage.Delete_Action;
                }
            }
            catch (Exception ex)
            {
                response.Status = Constants.Status.False;
                response.Message = ex.Message;
            }
            return response;
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
                var query = await _programRepository.Add(model);
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> Update(ProgramDTO model)
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
                        if (updatePerMission.ToList().Count() > 0) //Update Status M_PERMISSION
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
                    result = await _programRepository.Update(_mapper.Map(model, updateProgram));
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
