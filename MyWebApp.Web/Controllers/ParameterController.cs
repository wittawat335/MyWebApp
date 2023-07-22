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
        private readonly IPermissionService _permissionService;
        Common common = new Common();

        public ParameterController(IParameterService parameterService, IPermissionService permissionService)
        {
            _parameterService = parameterService;
            _permissionService = permissionService;
        }

        public IActionResult Index()
        {                      
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> _Detail(string action)
        {
            var model = new ParameterViewModel();
            try
            {
                model.ParameterList = await _parameterService.GetAll();
                if(action == "Default")
                {
                    foreach (var m in model.ParameterList)
                    {
                        m.PARA_VALUE = m.PARA_DEFAULT_VALUE;
                    }
                }
              
                model.permEdit = await _permissionService
                    .GetPermission(common.UserRole, Constants.ProgramCode.MasterData, Constants.ActCode.MasterDataEdit);
               
                model.action = Constants.Action.Edit;
                
                return PartialView(model);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save(List<M_PARAMETER> Para)
        {
            var response = new Response<List<M_PARAMETER>>();
            try
            {
                response.status = await _parameterService.Save(Para);
                response.message = Constants.StatusMessage.Update_Action;
            }
            catch (Exception)
            {
                response.status = Constants.Status.False;
                response.message = Constants.StatusMessage.Cannot_Update_Data;
            }

             return new JsonResult(response);
        }
    }
}
