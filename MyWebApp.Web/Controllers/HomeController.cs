using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Core.DTO;
using MyWebApp.Core.Model.ViewModels;
using MyWebApp.Core.Services.Contract;
using Newtonsoft.Json;
using System.Security.Claims;
using MyWebApp.Core.Utility;
using MyWebApp.Core.Model;
using static MyWebApp.Core.Model.ViewModels.User.UserViewModel;
using System.Diagnostics;

namespace MyWebApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProgramService _programService;
        private readonly IParameterService _parameterService;
        private readonly ILoginService _loginService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _contextAccessor;
        Common common = new Common();

        public HomeController(
            IParameterService parameterService,
            IProgramService programService,
            IUserService userService,
            ILoginService loginService,
            IHttpContextAccessor contextAccessor)
        {
            _userService = userService;
            _parameterService = parameterService;
            _programService = programService;
            _loginService = loginService;
            _contextAccessor = contextAccessor;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            return View();
        }
        public async Task<IActionResult> Logout()
        {

            _contextAccessor.HttpContext.Session.Remove(Constants.SessionKey.LoginInfo);
            Response.Cookies.Delete(Constants.SessionKey.LoginInfo);
            Response.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel obj)
        {
            var response = new Response<UserDTO>();
            try
            {
                var ip = "192.128.101.1";
                if (await _loginService.CheckUserPassOrAD(obj.UserLogin, obj.Password))
                {
                    var currentRoleList = await _loginService.getListCurrentRole(obj.UserLogin, ip);
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

            return new JsonResult(response);
        }
        [HttpPost]
        public async Task<IActionResult> CheckLogin(LoginViewModel obj)
        {
            string msg = "Invalid user name or password";
            string initUrl = "~/Home/Index";
            string url = initUrl;
            string userAD = obj.UserLogin;
            var response = new Response<UserDTO>();
            try
            {
                var colUser = await _loginService.findUser(userAD);
                if (colUser != null && colUser.USER_STATUS == Constants.Status.Active)
                {
                    //Set Config
                    var sysdate = await _parameterService
                        .GetByCode(Constants.ParaCode.sysDate);
                    var pageSize = await _parameterService
                        .GetByCode(Constants.ParaCode.pageSize);
                    string userName = colUser.USER_FIRST_NAME
                        + " " + colUser.USER_LAST_NAME;
                    HttpContext.Session
                        .SetString("SysDate", sysdate.PARA_VALUE);

                    //Check ว่าเคย Login เข้าระบบ ด้วย Role ไหนมาก่อนหรือไม่ ?
                    var currentRoleList = await _loginService.getListCurrentRole(userAD);
                    if (!currentRoleList.Count.Equals(0))
                    {
                        var currentRole = currentRoleList.LastOrDefault();
                        var roleNow = await _loginService
                        .getRoleBycurrentRole(currentRole.CL_ROLE_CODE);
                        var CheckRoleOld = await _loginService
                            .getCurrentRoleList2(userAD, currentRole.CL_ROLE_CODE);
                        if (CheckRoleOld != null)
                        {
                            response.status = await SetSessionLogin(
                                userAD,
                                userName,
                                roleNow.ROLE_CODE,
                                roleNow.ROLE_NAME,
                                roleNow.ROLE_DATA_LEVEL);
                            if (response.status)
                            {
                                if (await _loginService.SignIn(userAD, roleNow.ROLE_CODE))
                                {
                                    response.url = Url
                                     .Content(await _programService
                                     .GetMeneDefaultAsync(roleNow.ROLE_CODE));
                                    response.message = Constants.Msg.LoginSucc;
                                }
                            }
                        }
                        else
                        {
                            ////////////////////////////////////CheckRoleLogin
                            //Check Role User
                            var countRole = await _loginService.getListUserRole(userAD);
                            if (countRole.Count.Equals(1)) //Check Role = 1 ?
                            {
                                foreach (var r in countRole)
                                {
                                    var roles = await _loginService
                                        .getRoleBycurrentRole(r.USERROLE_ROLE_CODE);
                                    response.status = await SetSessionLogin(
                                        userAD,
                                        userName,
                                        roles.ROLE_CODE,
                                        roles.ROLE_NAME,
                                        roles.ROLE_DATA_LEVEL);
                                    if (response.status)
                                    {
                                        await _loginService.SignIn(userAD, roleNow.ROLE_CODE);
                                        response.url = Url
                                     .Content(await _programService
                                     .GetMeneDefaultAsync(roles.ROLE_CODE));
                                    }

                                }
                                response.message = Constants.Msg.LoginSucc;
                            }
                            else if (countRole.Count > 1) //Check Role > 1 ?
                            {
                                var listRole = await _userService
                                    .GetListRoleActiveOnly(userAD);
                                var defaultRole = listRole
                                    .FirstOrDefault(x => x.RoleFlag == true);
                                var roless = await _loginService
                                    .getRoleBycurrentRole(defaultRole.RoleCode);
                                response.status = await SetSessionLogin(
                                    userAD,
                                    userName,
                                    roless.ROLE_CODE,
                                    roless.ROLE_NAME,
                                    roless.ROLE_DATA_LEVEL);
                                response.url = Url
                                    .Content(await _programService
                                    .GetMeneDefaultAsync(roless.ROLE_CODE));
                                response.message = Constants.Msg.LoginSucc;
                            }
                            ////////////////////////////////////CheckRoleLogin
                            else
                            {
                                response.message = "' " + userAD + " '" + "had no role";
                                response.status = Constants.Status.False;
                            }
                        }
                    }
                    else
                    {
                        ////////////////////////////////////CheckRoleLogin
                        //Check Role User
                        var countRole = await _loginService.getListUserRole(userAD);
                        if (countRole.Count.Equals(1)) //Check Role = 1 ?
                        {
                            foreach (var r in countRole)
                            {
                                var roles = await _loginService
                                    .getRoleBycurrentRole(r.USERROLE_ROLE_CODE);
                                response.status = await SetSessionLogin(
                                    userAD,
                                    userName,
                                    roles.ROLE_CODE,
                                    roles.ROLE_NAME,
                                    roles.ROLE_DATA_LEVEL);
                                if (response.status)
                                {
                                    await _loginService.SignIn(userAD, roles.ROLE_CODE);
                                    response.url = Url
                                 .Content(await _programService
                                 .GetMeneDefaultAsync(roles.ROLE_CODE));
                                }
                            }
                            response.message = Constants.Msg.LoginSucc;
                        }
                        else if (countRole.Count > 1) //Check Role > 1 ?
                        {
                            var listRole = await _userService
                                .GetListRoleActiveOnly(userAD);
                            var defaultRole = listRole
                                .FirstOrDefault(x => x.RoleFlag == true);
                            var roless = await _loginService
                                .getRoleBycurrentRole(defaultRole.RoleCode);
                            response.status = await SetSessionLogin(
                                userAD,
                                userName,
                                roless.ROLE_CODE,
                                roless.ROLE_NAME,
                                roless.ROLE_DATA_LEVEL);
                            if (response.status)
                            {
                                await _loginService.SignIn(userAD, roless.ROLE_CODE);
                                response.url = Url
                             .Content(await _programService
                             .GetMeneDefaultAsync(roless.ROLE_CODE));
                                response.message = Constants.Msg.LoginSucc;

                            }
                        }
                        ////////////////////////////////////CheckRoleLogin
                        else
                        {
                            response.message = "' " + userAD + " '" + "had no role";
                            response.status = Constants.Status.False;
                        }
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
            return new JsonResult(response);
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
        [HttpPost]
        public async Task<IActionResult> SelectRole(string role)
        {
            var response = new Response<Role>();
            string userLogin = common.UserLogin;
            string userName = common.UserName;
            try
            {
                if (role != null)
                {
                    var obj = await _loginService.getRoleBycurrentRole(role);
                    await SetSessionLogin(userLogin, userName, obj.ROLE_CODE, obj.ROLE_NAME, obj.ROLE_DATA_LEVEL);
                    response.url = Url.Content(await _programService.GetMeneDefaultAsync(role));
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

            return new JsonResult(response);
        }

        private async Task<bool> SetSessionLogin(string userLogin, string userName, string roleCode, string roleName, int dataLevel)
        {
            try
            {
                common.SetLoginSession(0, userLogin, userName, roleCode, roleName, dataLevel);
                common.SetLoginSession(await _loginService.InsertCurrentLogin(), userLogin, userName, roleCode, roleName, dataLevel);

                return true;

            }
            catch (Exception)
            {

                throw;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}