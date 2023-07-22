using Microsoft.AspNetCore.Mvc;
using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Model;
using MyWebApp.Core.Model.ViewModels.Role;
using MyWebApp.Core.Services;
using MyWebApp.Core.Services.Contract;
using MyWebApp.Core.Utility;

namespace MyWebApp.Web.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _service;
        private readonly IPermissionService _permissionService;
        Common common = new Common();   

        public RoleController(IRoleService service, IPermissionService permissionService)
        {
            _service = service;
            _permissionService = permissionService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new RoleViewModel();
            model.permAdd = await _permissionService
              .GetPermission(common.UserRole, Constants.ProgramCode.Role, Constants.ActCode.RoleAdd);
            model.permEdit = await _permissionService
                .GetPermission(common.UserRole, Constants.ProgramCode.Role, Constants.ActCode.RoleEdit);
            model.permView = await _permissionService
                .GetPermission(common.UserRole, Constants.ProgramCode.Role, Constants.ActCode.RoleView);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> _Search()
        {
            var response = new Response<List<M_ROLE>>();
            try
            {
                response.value = await _service.GetList();
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
        public async Task<IActionResult> _Detail(string code, string action)
        {
            var model = new RoleViewModel();
            try
            {
                if (code != null)
                    model.role = await _service.GetByCode(code);

                model.action = action;
                model.permAdd = await _permissionService
                    .GetPermission(common.UserRole, Constants.ProgramCode.User, Constants.ActCode.UserAdd);
                model.permEdit = await _permissionService
                    .GetPermission(common.UserRole, Constants.ProgramCode.User, Constants.ActCode.UserEdit);

                return PartialView(model);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public IActionResult _DetailsPermission(string mode, string code)
        {
            var model = new PermissionViewModel();
            try
            {
                model.RoleCode = code;
                model.Visibility = false;

                return PartialView(model);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpPost]
        public async Task<JsonResult> GetPermissionData(string code)
        {
            try
            {
                List<DataPermissionJsonList> objReturn = new List<DataPermissionJsonList>();
                List<SP_SEARCH_PERMISSION_BY_ROLE_Result> objData = await _service.GetPermissionData(code);
                if (objData != null && objData.Count > 0)
                {

                    int? iCountTopLevel = objData.Count;

                    for (int i = 0; i < iCountTopLevel; i++)
                    {

                        bool varSelect = false;
                        if (objData[i].PERM_SELECT == "1")
                            varSelect = true;

                        string strIcon;
                        switch (objData[i].PERM_TEXT)
                        {
                            case Constants.JsTreeConfig.TextAdd:
                                strIcon = Constants.JsTreeConfig.IconAdd;
                                break;
                            case Constants.JsTreeConfig.TextEdit:
                                strIcon = Constants.JsTreeConfig.IconEdit;
                                break;
                            case Constants.JsTreeConfig.TextView:
                                strIcon = Constants.JsTreeConfig.IconView;
                                break;
                            default:
                                {
                                    strIcon = Constants.JsTreeConfig.IconDefault;
                                    break;
                                }
                        }

                        string strParent = (string.IsNullOrEmpty(objData[i].PERM_PARENT)) ? "#" : objData[i].PERM_PARENT;
                        bool booParentOpen = false;
                        OptionState objStates = new OptionState { opened = booParentOpen, selected = varSelect };
                        objReturn.Add(new DataPermissionJsonList() { id = objData[i].PERM_ID, parent = strParent, 
                            text = objData[i].PERM_TEXT, icon = strIcon, state = objStates });
                    }
                }
                return Json(objReturn);
            }
            catch
            {
                throw;
            }
        }
    }
}
