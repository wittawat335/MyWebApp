using AutoMapper;
using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Domain.RepositoryContracts;
using MyWebApp.Core.DTO;
using MyWebApp.Core.Model;
using MyWebApp.Core.Model.ViewModels.Master;
using MyWebApp.Core.Model.ViewModels.User;
using MyWebApp.Core.Services.Contract;
using MyWebApp.Core.Utility;
using System.Data;
using static MyWebApp.Core.Model.ViewModels.User.UserViewModel;

namespace MyWebApp.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<M_USER> _repository;
        private readonly IGenericRepository<M_USER_ROLE> _userRoleRepository;
        private readonly IGenericRepository<M_ROLE> _roleRepository;
        private readonly IPermissionService _permissionService;
        private readonly IMapper _mapper;
        Common common = new Common();
        public UserService(IGenericRepository<M_USER> repository, IGenericRepository<M_USER_ROLE> userRoleRepository, IGenericRepository<M_ROLE> roleRepository, 
            IMapper mapper, IPermissionService permissionService)
        {
            _repository = repository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _permissionService = permissionService;
            _mapper = mapper;
        }

        public async Task<UserViewModel> getIndex()
        {
            var model = new UserViewModel();
            try
            {
                model.permAdd = await _permissionService.GetPermission(common.UserRole, Constants.ProgramCode.MasterData, 
                    Constants.ActCode.MasterDataAdd);
                model.permEdit = await _permissionService.GetPermission(common.UserRole, Constants.ProgramCode.MasterData, 
                    Constants.ActCode.MasterDataEdit);
                model.permView = await _permissionService.GetPermission(common.UserRole, Constants.ProgramCode.MasterData, 
                    Constants.ActCode.MasterDataView);
            }
            catch
            {
                throw;
            }

            return model;
        }
        public async Task<UserViewModel> getDetail(string code, string action)
        {
            var model = new UserViewModel();
            try
            {
                if (code != null)
                    model.userDTO = await GetByCode(code);

                model.UserRoleList = await GetListRoleActiveOnly(model.userDTO == null ? "" : model.userDTO.USER_LOGIN);
                model.permAdd = await _permissionService.GetPermission(common.UserRole, Constants.ProgramCode.User, 
                    Constants.ActCode.UserAdd);
                model.permEdit = await _permissionService.GetPermission(common.UserRole, Constants.ProgramCode.User, Constants.ActCode.UserEdit);
                model.action = action;

            }
            catch
            {
                throw;
            }

            return model;
        }
        public async Task<Response<List<UserDTO>>> GetAll()
        {
            var response = new Response<List<UserDTO>>();
            try
            {
                var list = await _repository.GetAll();
                if (list.Count() > 0)
                {
                    response.value = _mapper.Map<List<UserDTO>>(list);
                    response.status = Constants.Status.True;
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }

            return response;
        }
        public async Task<UserDTO> GetByCode(string code)
        {
            try
            {
                var list = await _repository.Get(x => x.USER_LOGIN == code);
                return _mapper.Map<UserDTO>(list);
            }
            catch
            {
                throw;
            }
        }
        public async Task<Response<List<UserDTO>>> Search(UserViewModel model)
        {
            var response = new Response<List<UserDTO>>();
            try
            {
                var list = await _repository.GetAll();
                if (model.firstName != null)
                    list = list.Where(x => x.USER_FIRST_NAME.Contains(model.firstName));
                if (model.lastName != null)
                    list = list.Where(x => x.USER_LAST_NAME.Contains(model.lastName));
                if (model.loginId != null)
                    list = list.Where(x =>
                    x.USER_LOGIN.Contains(model.loginId));
                if (model.status != null)
                    list = list.Where(x =>
                    x.USER_STATUS.Contains(model.status));

                response.value = _mapper.Map<List<UserDTO>>(list);
                response.status = Constants.Status.True;
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }

            return response;
        }
        public async Task<ResponseStatus> Save(UserViewModel model)
        {
            var response = new ResponseStatus();
            try
            {
                if (model != null)
                {
                    if (model.roleSelect.Count(x => x.RoleFlag) > 0)
                    {
                        switch (model.action)
                        {
                            case Constants.Action.New:
                                if (await CheckDuplicate(model.userDTO.USER_LOGIN))
                                {
                                    response = await Add(model.userDTO);
                                    if (response.Status)
                                        await AddRole(model.roleSelect, model.userDTO.USER_LOGIN);
                                }
                                else
                                {
                                    response.Message = Constants.StatusMessage.Duplicate_User;
                                }
                                break;

                            case Constants.Action.Edit:
                                response = await Update(model.userDTO);
                                if (response.Status)
                                    await AddRole(model.roleSelect, model.userDTO.USER_LOGIN);
                                break;

                            default:
                                response.Status = Constants.Status.False;
                                response.Message = Constants.StatusMessage.No_Data;
                                break;
                        }
                    }
                    else
                    {
                        response.Message = Constants.Msg.ValidateSelectRole;
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
        public async Task<ResponseStatus> Add(UserDTO model)
        {
            var response = new ResponseStatus();
            try
            {
                model.USER_PASSWORD = common.Encrypt(model.USER_PASSWORD);
                await _repository.Add(_mapper.Map<M_USER>(model));
                response.Status = Constants.Status.True;
                response.Message = Constants.StatusMessage.Create_Action;
            }
            catch
            {
                throw;
            }

            return response;
        }
        public async Task<bool> AddRole(List<Role> model, string userLogin)
        {
            try
            {
                if (model.Count > 0)
                {
                    var findUser = await _userRoleRepository.GetAll(x => x.USERROLE_USER_LOGIN == userLogin);
                    await _userRoleRepository.DeleteList(findUser.ToList());

                    foreach (var item in model.Where(x => x.RoleFlag))
                    {
                        var userRole = new M_USER_ROLE();
                        userRole.USERROLE_USER_LOGIN = userLogin;
                        userRole.USERROLE_ROLE_CODE = item.RoleCode;
                        userRole.USERROLE_CREATE_BY = common.UserLogin;
                        userRole.USERROLE_CREATE_DATE = common.SystemDate;
                        userRole.USERROLE_UPDATE_BY = common.UserLogin;
                        userRole.USERROLE_UPDATE_DATE = common.SystemDate;
                        userRole.USERROLE_STATUS = Constants.Status.Active;
                        await _userRoleRepository.Add(userRole);
                    }
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<ResponseStatus> Update(UserDTO model)
        {
            var response = new ResponseStatus();
            try
            {
                var find = await _repository.Get(x => x.USER_LOGIN == model.USER_LOGIN);
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
                var queryUser = await _repository.Get(x => x.USER_LOGIN == code);
                var queryUserRole = await _userRoleRepository.GetAll(x => x.USERROLE_USER_LOGIN == code);
                if (queryUser != null)
                {
                    response.Status = await _repository.Delete(queryUser);
                    if (queryUserRole.Count() > 0)
                        await _userRoleRepository.DeleteList(queryUserRole.ToList());

                    response.Message = Constants.StatusMessage.Delete_Action;
                }
            }
            catch
            {
                throw;
            }

            return response;
        }
        public async Task<bool> CheckDuplicate(string code)
        {
            bool result = false;
            try
            {
                var model = await _repository.Get(x => x.USER_LOGIN == code);
                if (model == null)
                    result = true;

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Role>> GetListRoleActiveOnly(string userLogin = "")
        {
            IQueryable<M_ROLE> tbRole = await _roleRepository.GetAll();
            IQueryable<M_USER_ROLE> tbUserRole = await _userRoleRepository.GetAll();
            try
            {
                var query = (
                              from m in tbRole
                              from d in tbUserRole
                                  .Where(d => d.USERROLE_ROLE_CODE == m.ROLE_CODE
                                  && d.USERROLE_USER_LOGIN == userLogin)
                                  .DefaultIfEmpty()
                              select new Role
                              {
                                  RoleCode = m.ROLE_CODE,
                                  RoleName = m.ROLE_NAME,
                                  UserLogin = d == null ?
                                  string.Empty :
                                  d.USERROLE_USER_LOGIN,
                                  RoleFlag = d == null ? false : true
                              }
                          ).ToList();

                return _mapper.Map<List<Role>>(query);
            }
            catch
            {
                throw;
            }
        }
    }
}
