using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Domain.RepositoryContracts;
using MyWebApp.Core.DTO;
using MyWebApp.Core.Model.ViewModels;
using MyWebApp.Core.Model;
using MyWebApp.Core.Services.Contract;
using MyWebApp.Core.Utility;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Security.Policy;
using static MyWebApp.Core.Model.ViewModels.User.UserViewModel;

namespace MyWebApp.Core.Services
{
    public class LoginService : ILoginService
    {
        private readonly IGenericRepository<M_USER_ROLE> _userRoleRepository;
        private readonly IGenericRepository<M_USER> _userRepository;
        private readonly IGenericRepository<M_ROLE> _roleRepository;
        private readonly IGenericRepository<T_CURRENT_LOGIN> _currentRepository;
        private readonly IGenericRepository<T_LOGIN_HISTORY> _loginHistoryRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUserService _userService;
        private readonly IProgramService _programService;
        private readonly IParameterService _parameterService;
        private readonly IMapper _mapper;
        Common common = new Common();

        public LoginService(
            IGenericRepository<M_USER_ROLE> userRoleRepository,
            IGenericRepository<M_USER> userRepository,
            IGenericRepository<M_ROLE> roleRepository,
            IGenericRepository<T_CURRENT_LOGIN> currentRepository,
            IGenericRepository<T_LOGIN_HISTORY> loginHistoryRepository,
            IUserService userService,
            IProgramService programService,
            IParameterService parameterService,
            IHttpContextAccessor contextAccessor,
            IMapper mapper
           )
        {
            _userRoleRepository = userRoleRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _currentRepository = currentRepository;
            _loginHistoryRepository = loginHistoryRepository;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
            _userService = userService;
            _programService = programService;
            _parameterService = parameterService;
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
                var colUser = await _userRepository.Get(x => x.USER_LOGIN == userLogin && x.USER_STATUS == Constants.Status.Active);
                if (colUser != null)
                {
                    typeUser = colUser.USER_AD_FLAG;
                    passworden = common.Encrypt(password);
                    if (colUser.USER_PASSWORD == passworden)
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
        public async Task<List<T_CURRENT_LOGIN>> getListCurrentRole(string userLogin, string ip = "")
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
        public async Task<UserRoleDTO> getCurrentRoleList2(string userLogin, string roleCode)
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
        public async Task<Response<UserDTO>> Login(LoginViewModel obj)
        {
            var response = new Response<UserDTO>();
            try
            {
                var ip = "192.128.101.1";
                if (await CheckUserPassOrAD(obj.UserLogin, obj.Password))
                {
                    var currentRoleList = await getListCurrentRole(obj.UserLogin, ip);
                    if (currentRoleList.Count() != 0)
                    {
                        var currentRole = currentRoleList.LastOrDefault();
                        response.message = Constants.Msg.LoginDuplicate;
                    }
                    else
                        response.message = Constants.Msg.LoginSucc;

                    if (await GetRoleListByUserLogin(obj.UserLogin))
                        response.status = Constants.Status.True;
                }
            }
            catch (Exception ex)
            {
                response.status = Constants.Status.False;
                response.message = ex.Message;
            }

            return response;
        }
        public async Task<bool> GetRoleListByUserLogin(string userLogin)
        {
            try
            {
                var list = await _userService.GetListRoleActiveOnly(userLogin);
                //-- Save list to session
                string listString = JsonConvert.SerializeObject(list);
                _contextAccessor.HttpContext.Session.SetString("RoleList", listString);

                return true;
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
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
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
        public async Task<bool> SetSessionLogin(string userLogin, string userName, string roleCode, string roleName, int dataLevel)
        {
            try
            {
                common.SetLoginSession(0, userLogin, userName, roleCode, roleName, dataLevel);
                common.SetLoginSession(await InsertCurrentLogin(), userLogin, userName, roleCode, roleName, dataLevel);

                return true;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<Response<UserDTO>> CheckLogin(LoginViewModel obj)
        {
            string msg = "Invalid user name or password";
            string initUrl = "~/Home/Index";
            string url = initUrl;
            string userAD = obj.UserLogin;
            var response = new Response<UserDTO>();
            try
            {
                var colUser = await findUser(userAD);
                if (colUser != null && colUser.USER_STATUS == Constants.Status.Active)
                {
                    //Set Config
                    var sysdate = await _parameterService.GetByCode(Constants.ParaCode.sysDate);
                    var pageSize = await _parameterService.GetByCode(Constants.ParaCode.pageSize);
                    string userName = colUser.USER_FIRST_NAME + " " + colUser.USER_LAST_NAME;
                    _contextAccessor.HttpContext.Session.SetString("SysDate", sysdate.PARA_VALUE);

                    //Check ว่าเคย Login เข้าระบบ ด้วย Role ไหนมาก่อนหรือไม่ ?
                    var currentRoleList = await getListCurrentRole(userAD);
                    if (!currentRoleList.Count.Equals(0))
                    {
                        var currentRole = currentRoleList.LastOrDefault();
                        var roleNow = await getRoleBycurrentRole(currentRole.CL_ROLE_CODE);
                        var CheckRoleOld = await getCurrentRoleList2(userAD, currentRole.CL_ROLE_CODE);
                        if (CheckRoleOld != null)
                        {
                            response.status = await SetSessionLogin(userAD, userName, roleNow.ROLE_CODE, roleNow.ROLE_NAME,
                                roleNow.ROLE_DATA_LEVEL);
                            if (response.status)
                            {
                                if (await SignIn(userAD, roleNow.ROLE_CODE))
                                {
                                    response.url = await _programService.GetMeneDefaultAsync(roleNow.ROLE_CODE);
                                    response.message = Constants.Msg.LoginSucc;
                                }
                            }
                        }
                        else
                        {
                            await CheckRoleLogin(userAD, userName, roleNow.ROLE_CODE);
                        }
                    }
                    else
                    {
                        await CheckRoleLogin(userAD, userName, "");
                    }
                }
                else if (colUser != null && colUser.USER_STATUS == Constants.Status.Inactive)
                {
                    response.status = Constants.Status.False;
                    response.message = Constants.Msg.UserInActive;
                }
                else //Check User ใน System ถ้าไม่มี ?
                {
                    response.status = Constants.Status.False;
                    response.message = Constants.Msg.UserNameInvalid;
                }
            }
            catch (Exception ex)
            {
                response.status = Constants.Status.False;
                response.message = ex.Message;
            }
            return response;
        }

        public async Task<Response<Role>> SelectRole(string role)
        {
            var response = new Response<Role>();
            string? userLogin = common.UserLogin;
            string? userName = common.UserName;
            try
            {
                if (role != null)
                {
                    var obj = await getRoleBycurrentRole(role);
                    await SetSessionLogin(userLogin, userName, obj.ROLE_CODE, obj.ROLE_NAME, obj.ROLE_DATA_LEVEL);
                    response.url = await _programService.GetMeneDefaultAsync(role);
                    response.status = Constants.Status.True;
                    response.message = Constants.Msg.SwitchRoleSucc;
                }
                else
                {
                    response.status = Constants.Status.False;
                    response.message = Constants.Msg.SwitchRoleFail;
                }
            }
            catch (Exception ex)
            {
                response.status = Constants.Status.False;
                response.message = ex.Message;
            }

            return response;
        }

        public async Task<Response<UserDTO>> CheckRoleLogin(string user, string userName, string roleCode)
        {
            var response = new Response<UserDTO>();
            try
            {
                var countRole = await getListUserRole(user);
                if (countRole.Count.Equals(1)) //Check Role = 1 ?
                {
                    foreach (var r in countRole)
                    {
                        var roles = await getRoleBycurrentRole(r.USERROLE_ROLE_CODE);
                        response.status = await SetSessionLogin(user, userName, roles.ROLE_CODE, roles.ROLE_NAME,
                            roles.ROLE_DATA_LEVEL);
                        if (response.status)
                        {
                            await SignIn(user, roleCode);
                            response.url = await _programService.GetMeneDefaultAsync(roles.ROLE_CODE);
                        }

                    }
                    response.message = Constants.Msg.LoginSucc;
                }
                else if (countRole.Count > 1) //Check Role > 1 ?
                {
                    var listRole = await _userService.GetListRoleActiveOnly(user);
                    var defaultRole = listRole.FirstOrDefault(x => x.RoleFlag);
                    var roless = await getRoleBycurrentRole(defaultRole.RoleCode);
                    response.status = await SetSessionLogin(user, userName, roless.ROLE_CODE, roless.ROLE_NAME, roless.ROLE_DATA_LEVEL);
                    response.url = await _programService.GetMeneDefaultAsync(roless.ROLE_CODE);
                    response.message = Constants.Msg.LoginSucc;
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return response;
        }
    }
}
