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
        private readonly IPermissionService _permissionService;
        Common common = new Common();

        public MasterController(IMasterService service, IPermissionService permissionService)
        {
            _service = service;
            _permissionService = permissionService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new MasterViewModel();
            model.listMaster = await _service.GetListMasterActiveOnly();
            model.permAdd = await _permissionService
                .GetPermission(common.UserRole, Constants.ProgramCode.MasterData, Constants.ActCode.MasterDataAdd);
            model.permEdit = await _permissionService
                .GetPermission(common.UserRole, Constants.ProgramCode.MasterData, Constants.ActCode.MasterDataEdit);
            model.permView = await _permissionService
                .GetPermission(common.UserRole, Constants.ProgramCode.MasterData, Constants.ActCode.MasterDataView);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetList(MasterViewModel model)
        {
            var response = new Response<List<MasterDTO>>();
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

        [HttpPost]
        public async Task<IActionResult> _Detail(
            string code,
            string action)
        {
            MasterViewModel model = new MasterViewModel();
            try
            {
                if (code != null)
                    model.masterDTO = await _service.GetByCode(code);

                model.listMaster =
                    await _service.GetListMasterActiveOnly();
                model.action = action;

                return PartialView(model);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save(MasterViewModel model)
        {
            var response = new Response<bool>();
            try
            {
                if (model != null)
                {
                    if (model.action == Constants.Action.New)
                    {
                        response.value =
                            await _service.Add(model.masterDTO);
                        response.message =
                            Constants.StatusMessage.Create_Action;
                        response.status =
                            Constants.Status.True;

                    }
                    else if (model.action == Constants.Action.Edit)
                    {
                        response.value =
                            await _service.Update(model.masterDTO);
                        response.message =
                            Constants.StatusMessage.Update_Action;
                        response.status =
                            Constants.Status.True;

                    }
                    else
                    {
                        response.status = Constants.Status.False;
                        response.message =
                            Constants.StatusMessage.No_Data;
                    }
                }
                else
                {
                    response.status = Constants.Status.False;
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

        public IActionResult Test()
        {
            return View();
        }

    }
}
