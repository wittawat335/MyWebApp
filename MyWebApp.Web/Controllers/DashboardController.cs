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
            decimal xx = cm.CalculateMonth(1000, 
                cm.SystemDate, cm.SystemDate.AddDays(3).AddMonths(3), 30);

            return View();
        }
        //[HttpPost]
        public async Task<IActionResult> _ChartsR3()
        {
            var response = new Response<List<Charts>>();
            var objList = new List<Charts>();
            try
            {
                List<ChartsSP> result = await _service.GetChartsR3();

                foreach (var item in result)
                {
                    objList.Add(new Charts()
                    {
                        id = item.ID,
                        text = item.TEXT,
                        value = cm.UnInt32Null(item.VALUE),
                        color = item.COLOR
                    });
                }
                //var scoreList = GetStudentScoreDetails();

                return PartialView(objList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<IActionResult> _ChartsRepo()
        {
            var response = new Response<List<Charts>>();
            var objList = new List<Charts>();
            try
            {
                List<ChartsSP> result = await _service.GetChartsRepo();

                foreach (var item in result)
                {
                    objList.Add(new Charts()
                    {
                        id = item.ID,
                        text = item.TEXT,
                        value = cm.UnInt32Null(item.VALUE),
                        color = item.COLOR
                    });
                }

                return PartialView(objList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<IActionResult> _ChartsLegal()
        {
            var response = new Response<List<Charts>>();
            var objList = new List<Charts>();
            try
            {        
                var markList = cm.GetStudentMarkList();

                return PartialView(markList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<IActionResult> _ChartsLegalStatus()
        {
            var response = new Response<List<Charts>>();
            var objList = new List<Charts>();
            try
            {
                var scoreList = cm.GetStudentScoreDetails();

                return PartialView(scoreList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
       
    }
}
