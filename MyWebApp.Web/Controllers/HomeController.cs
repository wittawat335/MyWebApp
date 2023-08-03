﻿using Microsoft.AspNetCore.Authentication.Cookies;
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
        private readonly ILoginService _loginService;
        private readonly IHttpContextAccessor _contextAccessor;

        public HomeController(ILoginService loginService, IHttpContextAccessor contextAccessor)
        {
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
            return new JsonResult(await _loginService.Login(obj));
        }
        [HttpPost]
        public async Task<IActionResult> CheckLogin(LoginViewModel obj)
        {
            var response = new Response<UserDTO>();
            response = await _loginService.CheckLogin(obj);
            response.url = Url.Content(response.url);

            return new JsonResult(response);
        }
      
        [HttpPost]
        public async Task<IActionResult> SelectRole(string role)
        {
            var response = new Response<Role>();
            response = await _loginService.SelectRole(role);
            response.url = Url.Content(response.url);

            return new JsonResult(response);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}