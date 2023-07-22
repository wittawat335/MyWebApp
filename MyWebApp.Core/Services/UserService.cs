using AutoMapper;
using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Domain.RepositoryContracts;
using MyWebApp.Core.DTO;
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
        private readonly IMapper _mapper;
        Common common = new Common();
        public UserService(
            IGenericRepository<M_USER> repository, 
            IGenericRepository<M_USER_ROLE> userRoleRepository, 
            IGenericRepository<M_ROLE> roleRepository, 
            IMapper mapper)
        {
            _repository = repository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
        }


        public async Task<List<UserDTO>> GetAll()
        {
            try
            {
                var list = await _repository.GetAll();
                return _mapper.Map<List<UserDTO>>(list);
            }
            catch
            {
                throw;
            }
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



        public async Task<List<UserDTO>> Search(UserViewModel model)
        {
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


                return _mapper.Map<List<UserDTO>>(list);
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> Add(UserDTO model)
        {
            try
            {
                model.USER_PASSWORD = common.Encrypt(model.USER_PASSWORD);
                var insert = await _repository.Add(_mapper.Map<M_USER>(model));

                return true;
            }
            catch
            {
                throw;
            }
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
        public async Task<bool> Update(UserDTO model)
        {
            try
            {
                var mapper = _mapper.Map<M_USER>(model);
                var dataUpdate = await _repository.Get(x =>
                x.USER_LOGIN == mapper.USER_LOGIN);
                if (dataUpdate == null)
                    throw new TaskCanceledException("No Data");
                //dataUpdate = mapper; //รอดู
                dataUpdate.USER_FIRST_NAME = mapper.USER_FIRST_NAME;
                dataUpdate.USER_LAST_NAME = mapper.USER_LAST_NAME;
                dataUpdate.USER_UPDATE_BY = mapper.USER_UPDATE_BY;
                dataUpdate.USER_UPDATE_DATE = mapper.USER_UPDATE_DATE;
                dataUpdate.USER_STATUS = mapper.USER_STATUS;

                bool updated = await _repository.Update(dataUpdate);
                if (!updated)
                    throw new TaskCanceledException("Cannot Update Data");

                return updated;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> Delete(string code)
        {
            bool deleted = false;
            try
            {
                var queryUser = await _repository.Get(x => x.USER_LOGIN == code);
                var queryUserRole = await _userRoleRepository.GetAll(x => 
                x.USERROLE_USER_LOGIN == code);

                if (queryUser == null)
                    throw new TaskCanceledException("No Data");
                             
                if (deleted = await _repository.Delete(queryUser))
                    deleted = await _userRoleRepository.DeleteList(queryUserRole.ToList());

                return deleted;
            }
            catch
            {
                throw;
            }
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
