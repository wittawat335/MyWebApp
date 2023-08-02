using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Core.Model;
using MyWebApp.Core.Model.ViewModels.Dashboard;
using MyWebApp.Core.Services.Contract;
using MyWebApp.Core.Utility;

namespace MyWebApp.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _service;
        Common cm = new Common();

        public DashboardController(IDashboardService service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> _ChartsR3()
        {
            return PartialView(await _service.ChartsR3());
        }
        public async Task<IActionResult> _ChartsRepo()
        {
            return PartialView(await _service.ChartsRepo());
        }
        public async Task<IActionResult> _ChartsLegal()
        {
            return PartialView(cm.GetStudentMarkList());
        }
        public async Task<IActionResult> _ChartsLegalStatus()
        {
            return PartialView(cm.GetStudentScoreDetails());
        }
    }
}
