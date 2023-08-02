using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Model;
using MyWebApp.Core.Model.ViewModels.Master;
using MyWebApp.Core.Model.ViewModels.Parameter;
using MyWebApp.Core.Services.Contract;
using MyWebApp.Core.Utility;

namespace MyWebApp.Web.Controllers
{
    [Authorize]
    public class ParameterController : Controller
    {
        private readonly IParameterService _parameterService;

        public ParameterController(IParameterService parameterService)
        {
            _parameterService = parameterService;
        }

        public IActionResult Index()
        {                      
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> _Detail(string action)
        {
            return PartialView(await _parameterService.GetAll(action));
        }

        [HttpPost]
        public async Task<IActionResult> Save(List<M_PARAMETER> Para)
        {
            return new JsonResult(await _parameterService.postSave(Para));
        }
    }
}
