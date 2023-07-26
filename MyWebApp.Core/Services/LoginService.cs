using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Domain.RepositoryContracts;
using MyWebApp.Core.DTO;
using MyWebApp.Core.Services.Contract;
using MyWebApp.Core.Utility;
using System.Security.Claims;

namespace MyWebApp.Core.Services
{
    public class LoginService : ILoginService
    {
        private readonly IGenericRepository<M_USER_ROLE> _userRoleRepository;
        private readonly IGenericRepository<M_USER> _userRepository;
        private readonly IGenericRepository<M_ROLE> _roleRepository;
        private readonly IGenericRepository<M_PARAMETER> _paraRepository;
        private readonly IGenericRepository<T_CURRENT_LOGIN> _currentRepository;
        private readonly IGenericRepository<T_LOGIN_HISTORY> _loginHistoryRepository;
        private readonly IGenericRepository<M_PROGRAM> _programRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;
        Common common = new Common();

        public LoginService(
            IGenericRepository<M_USER_ROLE> userRoleRepository,
            IGenericRepository<M_PROGRAM> programRepository,
            IGenericRepository<M_USER> userRepository,
            IGenericRepository<M_ROLE> roleRepository,
            IGenericRepository<M_PARAMETER> paraRepository,
            IGenericRepository<T_CURRENT_LOGIN> currentRepository,
            IGenericRepository<T_LOGIN_HISTORY> loginHistoryRepository,
            IHttpContextAccessor contextAccessor,
            IMapper mapper
           )
        {
            _programRepository = programRepository;
            _userRoleRepository = userRoleRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _paraRepository = paraRepository;
            _currentRepository = currentRepository;
            _loginHistoryRepository = loginHistoryRepository;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }
        public async Task<M_ROLE> getRoleBycurrentRole(string roleCode)
        {
            try
            {
                var role = await _roleRepository.Get(x => x.ROLE_CODE == roleCode && x.ROLE_STATUS == Constants.Status.Active);
                return role;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<UserDTO> findUser(string userLogin)
        {
            try
            {
                var user = await _userRepository.Get(x => x.USER_LOGIN == userLogin);
                return _mapper.Map<UserDTO>(user);
            }
            catch
            {
                throw;
            }
        }

        public async Task<UserDTO> getUser(string userLogin, string password)
        {
            try
            {
                var user = await _userRepository.Get(x => x.USER_LOGIN == userLogin
                && x.USER_PASSWORD == password);
                return _mapper.Map<UserDTO>(user);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> CheckUserPassOrAD(string userLogin, string password)
        {
            bool results = false;
            string passworden;
            string typeUser = "0";
            try
            {
                var colUser = await _userRepository
                    .Get(x => x.USER_LOGIN == userLogin && x.USER_STATUS == Constants.Status.Active);
                if (colUser != null)
                {
                    typeUser = colUser.USER_AD_FLAG;
                    passworden = common.Encrypt(password);
                    if(colUser.USER_PASSWORD == passworden)
                        results = true;
                    else
                        throw new TaskCanceledException(Constants.Msg.PasswordInvalid);
                }
                else                   
                    throw new TaskCanceledException(Constants.Msg.UserNameInvalid);
               
                return results;
            }
            catch 
            {
                throw;
            }
        }

        public async Task<List<T_CURRENT_LOGIN>> getListCurrentRole(string userLogin,
            string ip = "")
        {
            var list = new List<T_CURRENT_LOGIN>();
            try
            {
                if (ip != "")
                {
                    var user = await _currentRepository.GetAll(x =>
                    x.CL_USER_LOGIN == userLogin &&
                    x.CL_IP_ADDRESS == ip);
                    list = user.ToList();
                }
                else
                {
                    var user = await _currentRepository.GetAll(x =>
                    x.CL_USER_LOGIN == userLogin);
                    list = user.ToList();
                }


                return list;
            }
            catch
            {
                throw;
            }
        }

        public async Task<UserRoleDTO> getCurrentRoleList2(
            string userLogin,
            string roleCode)
        {
            var CheckRoleOld = new M_USER_ROLE();
            try
            {
                var currentRoleList = await _userRoleRepository.GetAll(x =>
                x.USERROLE_USER_LOGIN == userLogin &&
                x.USERROLE_ROLE_CODE == roleCode);

                CheckRoleOld = currentRoleList.FirstOrDefault();
                //if (currentRoleList.Count() != 0)
                //{
                //    var currentRole = currentRoleList.LastOrDefault();
                //    var role = await _roleRepository.Get(x => 
                //    x.ROLE_CODE == currentRole.CL_ROLE_CODE);

                //    CheckRoleOld = await _userRoleRepository.Get(x => 
                //    x.USERROLE_USER_LOGIN == userLogin &&
                //    x.USERROLE_ROLE_CODE == currentRole.CL_ROLE_CODE
                //    );
                //}
                return _mapper.Map<UserRoleDTO>(CheckRoleOld);
            }
            catch
            {
                throw;
            }
        }

        public async Task<long> InsertCurrentLogin()
        {            
            var userLogin = common.UserLogin;
            var userRole = common.UserRole;
            var Ip = common.Ip;
            var systemDate = common.SystemDate;
            try
            {
                var query = await _currentRepository.GetAll(x => x.CL_USER_LOGIN == userLogin);          
                if (query != null)
                    await _currentRepository.DeleteList(query.ToList());
           
                var model = new T_CURRENT_LOGIN();
                model.CL_USER_LOGIN = userLogin;
                model.CL_ROLE_CODE = userRole;
                model.CL_IP_ADDRESS = Ip;
                model.CL_LOGIN_DATE = systemDate;
                model.CL_LAST_ACT_DATE = systemDate;
                model.CL_STATUS = Constants.Status.Active;

                var modelH = new T_LOGIN_HISTORY();
                modelH.CL_USER_LOGIN = userLogin;
                modelH.CL_ROLE_CODE = userRole;
                modelH.CL_IP_ADDRESS = Ip;
                modelH.CL_LOGIN_DATE = systemDate;
                modelH.CL_LAST_ACT_DATE = systemDate;
                modelH.CL_STATUS = Constants.Status.Active;

                await _currentRepository.Add(model);
                await _loginHistoryRepository.Add(modelH);

                return model.CL_ID;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<M_USER_ROLE>> getListUserRole(string userLogin = "")
        {
            try
            {
                var list = await _userRoleRepository.GetAll(x => x.USERROLE_USER_LOGIN == userLogin);
                return list.ToList();
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> SignIn(string userLogin, string role)
        {
            try
            {
                List<Claim> claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, userLogin),
                    new Claim(ClaimTypes.Role, role)
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                 CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {

                    AllowRefresh = true,
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(15)
                };

                await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
