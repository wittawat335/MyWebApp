using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Model;
using MyWebApp.Core.Model.ViewModels.Role;
using MyWebApp.Core.Services.Contract;
using MyWebApp.Core.Utility;
using Newtonsoft.Json;

namespace MyWebApp.Web.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly IRoleService _service;
        private readonly IPermissionService _permissionService;
        private readonly IHttpContextAccessor _contextAccessor;
        Common common = new Common();
        public RoleController(IRoleService service, IPermissionService permissionService, IHttpContextAccessor contextAccessor)
        {
            _service = service;
            _permissionService = permissionService;
            _contextAccessor = contextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _service.getIndex());
        }
        [HttpPost]
        public async Task<IActionResult> _Search()
        {
            return new JsonResult(await _service.Search());
        }
        [HttpPost]
        public async Task<IActionResult> _Detail(string code, string action)
        {
            return PartialView(await _service.getDetail(code, action));
        }
        [HttpPost]
        public async Task<IActionResult> Save(RoleViewModel model)
        {
            return new JsonResult(await _service.Save(model));
        }
        [HttpPost]
        public async Task<IActionResult> _DetailsPermission(string code)
        {
            return PartialView(await _service.getDetailsPermission(code));
        }
        public async Task<IActionResult> GetJsTree(string roleCode)
        {
            return new JsonResult(await _service.GetJsTree(roleCode));
        }

        [HttpPost]
        public IActionResult GetListPermission([FromBody] List<DataPermissionJsonInsertList> permissionData)
        {
            return new JsonResult(_service.GetListPermission(permissionData));
        }
        [HttpPost]
        public async Task<IActionResult> SavePermission(string roleCode)
        {
            return new JsonResult(await _service.SavePermission(roleCode));
        }
    }
}
