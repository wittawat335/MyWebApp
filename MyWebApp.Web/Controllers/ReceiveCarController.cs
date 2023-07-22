using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Core.Model;
using MyWebApp.Core.Services.Contract;
using MyWebApp.Core.Utility;
using static MyWebApp.Core.Model.ViewModels.ReceiveCar.ReceiveCarViewModel;

namespace MyWebApp.Web.Controllers
{
    [Authorize]
    public class ReceiveCarController : Controller
    {
        private readonly IReceiveCarService _service;
        Common common = new Common();

        public ReceiveCarController(IReceiveCarService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetList(SearchModel model)
        {
            var response = new Response<List<SP_SEARCH_RC_Result>>();
            try
            {
                response.value = await _service.GetAllbySp(model);
                response.status = Constants.Status.True;
            }
            catch(Exception ex)
            {
                response.status = Constants.Status.False;
                response.message = ex.Message;  
            }

            return new JsonResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> SaveFile(
            List<FileUploadModel> model)
        {
            var response = new Response<FileUploadModel>();
            try
            {
                if (model.Count() > 0)
                    response.status = await _service.SaveFile(model);
                
                response.message = Constants.StatusMessage.Update_Action;
            }
            catch (Exception ex)
            {
                response.status = Constants.Status.False;
                response.message = ex.Message;
            }
            return new JsonResult(response);
        }

        [HttpPost]
        public IActionResult _popUpTracking(
            string mode, 
            string code, 
            string contractNo)
        {          
            return PartialView();
        }

        [HttpPost]
        public IActionResult _popUpUpload()
        {            
            return PartialView();
        }

        public string GenerateAndDownLoadExcel()
        {
            try
            {
                var file = _service.GenerateAndDownLoadExcel();
               
                return file;
            }
            catch (Exception ex)
            {
                throw;
            }
           
        }
    }
}
