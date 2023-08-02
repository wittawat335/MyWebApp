using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CodeMetrics;
using MyWebApp.Core.DTO;
using MyWebApp.Core.Model;
using MyWebApp.Core.Model.ViewModels.Master;
using MyWebApp.Core.Model.ViewModels.User;
using MyWebApp.Core.Services.Contract;
using MyWebApp.Core.Utility;

namespace MyWebApp.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _service;
        private readonly IPermissionService _permissionService;
        Common common = new Common();

        public UserController(IUserService service, IPermissionService permissionService)
        {
            _service = service;
            _permissionService = permissionService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _service.getIndex());
        }

        [HttpPost]
        public async Task<IActionResult> _Detail(string code, string action)
        {
            return PartialView(await _service.getDetail(code, action));
        }

        [HttpPost]
        public async Task<IActionResult> GetList(UserViewModel model)
        {
            return new JsonResult(await _service.Search(model));
        }

        [HttpPost]
        public async Task<IActionResult> Save(UserViewModel model)
        {
            return new JsonResult(await _service.Save(model));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string code)
        {
            return new JsonResult(await _service.Delete(code));
        }
    }
}
