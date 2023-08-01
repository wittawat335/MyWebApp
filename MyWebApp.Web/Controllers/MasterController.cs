using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MyWebApp.Core.DTO;
using MyWebApp.Core.Model;
using MyWebApp.Core.Model.ViewModels.Master;
using MyWebApp.Core.Services.Contract;
using MyWebApp.Core.Utility;

namespace MyWebApp.Web.Controllers
{
    [Authorize]
    public class MasterController : Controller
    {
        private readonly IMasterService _service;

        public MasterController(IMasterService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _service.getIndex());
        }

        [HttpPost]
        public async Task<IActionResult> GetList(MasterViewModel model)
        {
            return new JsonResult(await _service.Search(model));
        }

        [HttpPost]
        public async Task<IActionResult> _Detail(string code, string action)
        {
            return PartialView(await _service.getDetail(code, action));
        }

        [HttpPost]
        public async Task<IActionResult> Save(MasterViewModel model)
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
