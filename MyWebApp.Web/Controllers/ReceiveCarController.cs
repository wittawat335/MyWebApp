using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Core.Model;
using MyWebApp.Core.Model.ViewModels.ReceiveCar;
using MyWebApp.Core.Services;
using MyWebApp.Core.Services.Contract;
using MyWebApp.Core.Utility;
using static MyWebApp.Core.Model.ViewModels.ReceiveCar.ReceiveCarViewModel;

namespace MyWebApp.Web.Controllers
{
    [Authorize]
    public class ReceiveCarController : Controller
    {
        private readonly IReceiveCarService _service;
        private readonly IPermissionService _permissionService;
        Common common = new Common();

        public ReceiveCarController(IReceiveCarService service, IPermissionService permissionService)
        {
            _service = service;
            _permissionService = permissionService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetList(SearchModel model)
        {
            return new JsonResult(await _service.GetList(model));
        }

        [HttpPost]
        public async Task<IActionResult> SaveFile(List<FileUploadModel> model)
        {
            return new JsonResult(await _service.Save(model));
        }

        [HttpPost]
        public async Task<IActionResult> _popUpUpload()
        {
            var model = new ReceiveCarViewModel();
            model.permEdit = await _permissionService
                .GetPermission(common.UserRole, Constants.ProgramCode.ReceiveCar, Constants.ActCode.ReceiveCarEdit);

            return PartialView(model);
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
