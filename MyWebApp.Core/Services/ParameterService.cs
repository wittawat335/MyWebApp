using AutoMapper;
using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Domain.RepositoryContracts;
using MyWebApp.Core.DTO;
using MyWebApp.Core.Model;
using MyWebApp.Core.Model.ViewModels.Master;
using MyWebApp.Core.Model.ViewModels.Parameter;
using MyWebApp.Core.Services.Contract;
using MyWebApp.Core.Utility;

namespace MyWebApp.Core.Services
{
    public class ParameterService : IParameterService
    {
        private readonly IGenericRepository<M_PARAMETER> _repository;
        private readonly IPermissionService _permissionService;
        private readonly IMapper _mapper;
        Common common = new Common();

        public ParameterService(IGenericRepository<M_PARAMETER> repository, IPermissionService permissionService, IMapper mapper)
        {
            _repository = repository;
            _permissionService = permissionService;
            _mapper = mapper;
        }
        public async Task<ResponseStatus> postSave(List<M_PARAMETER> Para)
        {
            var response = new ResponseStatus();
            try
            {
                response.Status = await Save(Para);
                response.Message = Constants.StatusMessage.Update_Action;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }
        public async Task<bool> Save(List<M_PARAMETER> model)
        {
            var result = false;
            try
            {
                foreach (var item in model)
                {
                    var query = await _repository.Get(x => x.PARA_CODE == item.PARA_CODE);
                    query.PARA_VALUE = item.PARA_VALUE;
                    query.PARA_UPDATE_BY = item.PARA_UPDATE_BY;
                    query.PARA_UPDATE_DATE = item.PARA_UPDATE_DATE;

                    var update = await _repository.Update(query);
                    result = update;
                }
                return result;
            }
            catch {
                throw;
            }
        }
        public async Task<ParameterViewModel> GetAll(string action)
        {
            var model = new ParameterViewModel();
            try
            {
                var list = await _repository.GetAll(x => x.PARA_STATUS == Constants.Status.Active);
                model.ParameterList = _mapper.Map<List<ParameterDTO>>(list);
                if (action == "Default")
                {
                    foreach (var m in model.ParameterList)
                    {
                        m.PARA_VALUE = m.PARA_DEFAULT_VALUE;
                    }
                }

                model.permEdit = await _permissionService.GetPermission
                    (common.UserRole, Constants.ProgramCode.SystemParameter, Constants.ActCode.SystemParameterEdit);

                model.action = Constants.Action.Edit;
            }
            catch 
            {
                throw;
            }

            return model;
        }
        public async Task<ParameterDTO> GetByCode(string code)
        {
            try
            {
                var list = await _repository.Get(x => x.PARA_CODE == code);
                return _mapper.Map<ParameterDTO>(list);
            }
            catch
            {
                throw;
            }
        }
    }
}
