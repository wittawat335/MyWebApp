
using AutoMapper;
using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Domain.RepositoryContracts;
using MyWebApp.Core.DTO;
using MyWebApp.Core.Model;
using MyWebApp.Core.Model.ViewModels.Master;
using MyWebApp.Core.Services.Contract;
using MyWebApp.Core.Utility;
using System.Reflection;

namespace MyWebApp.Core.Services
{
    public class MasterService : IMasterService
    {
        private readonly IGenericRepository<M_MASTER> _repository;
        private readonly IPermissionService _permissionService;
        private readonly IMapper _mapper;
        Common common = new Common();

        public MasterService(IGenericRepository<M_MASTER> repository, IPermissionService permissionService, IMapper mapper)
        {
            _repository = repository;
            _permissionService = permissionService;
            _mapper = mapper;
        }
        public async Task<Response<List<MasterDTO>>> GetAll()
        {
            var response = new Response<List<MasterDTO>>();
            try
            {
                var list = await _repository.GetAll();
                if (list.Count() > 0)
                {
                    response.value = _mapper.Map<List<MasterDTO>>(list);
                    response.status = Constants.Status.True;
                }
            }
            catch (Exception ex) {
                response.message = ex.Message;
            }

            return response;
        }
        public async Task<MasterViewModel> getIndex()
        {
            var model = new MasterViewModel();
            try
            {
                model.listMaster = await GetListMasterActiveOnly();
                model.permAdd = await _permissionService
                .GetPermission(common.UserRole, Constants.ProgramCode.MasterData, Constants.ActCode.MasterDataAdd);
                model.permEdit = await _permissionService
               .GetPermission(common.UserRole, Constants.ProgramCode.MasterData, Constants.ActCode.MasterDataEdit);
                model.permView = await _permissionService
                    .GetPermission(common.UserRole, Constants.ProgramCode.MasterData, Constants.ActCode.MasterDataView);               
            }
            catch
            {
                throw;
            }

            return model;
        }
        public async Task<MasterViewModel> getDetail(string code, string action)
        {
            var model = new MasterViewModel();
            try
            {
                if (code != null)
                    model.masterDTO = await GetByCode(code);

                model.listMaster = await GetListMasterActiveOnly();
                model.action = action;
                model.permAdd = await _permissionService
                    .GetPermission(common.UserRole, Constants.ProgramCode.User, Constants.ActCode.UserAdd);
                model.permEdit = await _permissionService
                    .GetPermission(common.UserRole, Constants.ProgramCode.User, Constants.ActCode.UserEdit);
            }
            catch 
            {
                throw;
            }

            return model;
        }
        public async Task<List<MasterDTO>> GetListMasterActiveOnly()
        {
            try
            {
                var list = await _repository.GetAll(x => x.MASTER_STATUS == "A");
                list = list.GroupBy(x => x.MASTER_TYPE).Select(x => x.FirstOrDefault());

                return _mapper.Map<List<MasterDTO>>(list);
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<MasterDTO>> GetListByMasterType(string code)
        {
            try
            {
                var list = await _repository.GetAll(x => x.MASTER_TYPE == code);
                return _mapper.Map<List<MasterDTO>>(list);
            }
            catch
            {
                throw;
            }
        }
        public async Task<MasterDTO> GetByCode(string code)
        {
            try
            {
                var list = await _repository.Get(x => x.MASTER_CODE == code);
                return _mapper.Map<MasterDTO>(list);
            }
            catch
            {
                throw;
            }
        }
        public async Task<Response<List<MasterDTO>>> Search(MasterViewModel model)
        {
            var response = new Response<List<MasterDTO>>();
            try
            {
                var list = await _repository.GetAll();
                if (model.masterType != null)
                    list = list.Where(x => x.MASTER_TYPE.Contains(model.masterType));
                if (model.masterCode != null)
                    list = list.Where(x => x.MASTER_CODE.Contains(model.masterCode));
                if (model.MasterNameTH != null)
                    list = list.Where(x =>
                    x.MASTER_NAME_TH.Contains(model.MasterNameTH));
                if (model.MasterNameEN != null)
                    list = list.Where(x =>
                    x.MASTER_NAME_EN.Contains(model.MasterNameEN));
                if (model.masterStatus != null)
                    list = list.Where(x =>
                    x.MASTER_STATUS.Contains(model.masterStatus));

                response.value = _mapper.Map<List<MasterDTO>>(list);
                response.status = Constants.Status.True;
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }

            return response;
        }
        public async Task<ResponseStatus> Save(MasterViewModel model)
        {
            var response = new ResponseStatus();
            try
            {
                if (model != null)
                {
                    switch (model.action)
                    {
                        case Constants.Action.New:
                            response = await Add(model.masterDTO);
                            break;

                        case Constants.Action.Edit:
                            response = await Update(model.masterDTO);
                            break;

                        default:
                            response.Status = Constants.Status.False;
                            response.Message = Constants.StatusMessage.No_Data;
                            break;
                    }
                }
                else
                {
                    response.Message = Constants.StatusMessage.No_Data;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }          
        public async Task<ResponseStatus> Add(MasterDTO model)
        {
            var response = new ResponseStatus();
            try
            {
                await _repository.Add(_mapper.Map<M_MASTER>(model));
                response.Status = Constants.Status.True;
                response.Message = Constants.StatusMessage.Create_Action;
            }
            catch
            {
                throw;
            }

            return response;
        }
        public async Task<ResponseStatus> Update(MasterDTO model)
        {
            var response = new ResponseStatus();
            try
            {
                var find = await _repository.Get(x => x.MASTER_CODE == model.MASTER_CODE);
                if (find != null)
                {
                    await _repository.Update(_mapper.Map(model, find));
                    response.Status = Constants.Status.True;
                    response.Message = Constants.StatusMessage.Update_Action;
                }
            }
            catch
            {
                throw;
            }

            return response;
        }
        public async Task<ResponseStatus> Delete(string code)
        {
            var response = new ResponseStatus();
            try
            {
                var query = await _repository.Get(x => x.MASTER_CODE == code);
                if (query != null)
                {
                    response.Status = await _repository.Delete(query);
                    response.Message = Constants.StatusMessage.Delete_Action;
                }            
            }
            catch
            {
                throw;
            }

            return response;
        }
    }
}
