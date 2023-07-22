using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Model;
using MyWebApp.Core.Model.ViewModels.TodoList;
using MyWebApp.Core.Services.Contract;
using MyWebApp.Core.Utility;

namespace MyWebApp.Web.Controllers
{
    [Authorize]
    public class TodoListController : Controller
    {
        private readonly ITodoListService _service;
        Common common = new Common();

        public TodoListController(ITodoListService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(string id ="0")
        {
            var model = new TodoListViewModel();
            try
            {
                model.DefaultCase = id;
                model.TrafficList = common.GetListTraffic();
                model.LegalCaseList = await _service
                    .GetListLegalCaseByGroup();

                return View(model);
            }
            catch {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetList(SearchViewModel model)
        {
            var response = new Response<List<T_JOB_ALL>>();
            try
            {
                response.value = await _service.Search(model);
                response.status = Constants.Status.True;
            }
            catch (Exception ex)
            {
                response.status = Constants.Status.False;
                response.message = ex.Message;
            }

            return new JsonResult(response);
        }

        #region Select2
        public async Task<IActionResult> searchRefNo(string query)
        {

            try
            {
                var m = await _service.GetRefNo(query);

                return new JsonResult(m);
            }
            catch 
            {
                throw;
            }

        }

        public async Task<IActionResult> searchCustomer(string query)
        {

            try
            {
                var m = await _service.GetCustomer(query);

                return new JsonResult(m);
            }
            catch
            {
                throw;
            }

        }

        public async Task<IActionResult> searchAdmin(string query)
        {

            try
            {
                var m = await _service.GetListUserByRole(query);

                return new JsonResult(m);
            }
            catch
            {
                throw;
            }

        }

        public async Task<IActionResult> searchOa(string query)
        {

            try
            {
                var m = await _service.GetListOA(query);

                return new JsonResult(m);
            }
            catch
            {
                throw;
            }

        }

        #endregion
    }
}
