using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Model;
using MyWebApp.Core.Model.ViewModels.Program;
using MyWebApp.Core.Services.Contract;
using MyWebApp.Core.Utility;

namespace MyWebApp.Web.Controllers
{
    [Authorize]
    public class ProgramController : Controller
    {
        private readonly IProgramService _service;

        public ProgramController(IProgramService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetList()
        {
            return new JsonResult(await _service.GetList());
        }

        [HttpPost]
        public async Task<IActionResult> _Detail(string code, string action)
        {
            return PartialView(await _service.Detail(code, action));
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProgramViewModel model)
        {
            return new JsonResult(await _service.Save(model));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string code)
        {
            return new JsonResult(await _service.sendDelete(code));
        }
    }
}
