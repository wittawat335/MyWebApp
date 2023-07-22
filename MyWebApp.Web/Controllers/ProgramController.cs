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
            var response = new Response<List<M_PROGRAM>>();
            try
            {
                response.value = await _service.GetAll();
                response.status = Constants.Status.True;
            }
            catch (Exception ex)
            {
                response.status = Constants.Status.False;
                response.message = ex.Message;
            }

            return new JsonResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> _Detail(
            string code, string action)
        {
            ProgramViewModel model = new ProgramViewModel();
            try
            {
                if (code != null)
                    model.program = await _service.GetByCode(code);

                model.action = action;

                return PartialView(model);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProgramViewModel model)
        {
            var response = new Response<bool>();
            try
            {
                if (model != null)
                {
                    if (model.action == Constants.Action.New)
                    {
                        response.value = 
                            await _service.Add(model.program);
                        response.message = 
                            Constants.StatusMessage.Create_Action;
                        response.status = 
                            Constants.Status.True;

                    }
                    else if (model.action == Constants.Action.Edit)
                    {
                        response.value = 
                            await _service.Update(model.program);
                        response.message = 
                            Constants.StatusMessage.Update_Action;
                        response.status = 
                            Constants.Status.True;
                    }
                    else
                    {
                        response.status = 
                            Constants.Status.False;
                        response.message = 
                            Constants.StatusMessage.No_Data;
                    }
                }
                else
                {
                    response.status = 
                        Constants.Status.False;
                    response.message = 
                        Constants.StatusMessage.No_Data;
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
        public async Task<IActionResult> Delete(string code)
        {
            var response = new Response<bool>();
            try
            {
                if (code != null)
                {
                    response.value = await _service.Delete(code);
                    response.message = 
                        Constants.StatusMessage.Delete_Action;
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
    }
}
