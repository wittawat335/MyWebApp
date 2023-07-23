using Microsoft.AspNetCore.Mvc;
using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Model;
using MyWebApp.Core.Model.ViewModels.Role;
using MyWebApp.Core.Services;
using MyWebApp.Core.Services.Contract;
using MyWebApp.Core.Utility;
using Newtonsoft.Json;
using static MyWebApp.Core.Model.ViewModels.TreeViewInAspNetCor;

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
        public async Task<IActionResult> _DetailsPermission(string code)
        {
            List<DataPermissionJsonList> objReturn = new List<DataPermissionJsonList>();
            try
            {
                if (code != null)
                {
                    ViewBag.RoleCode = code;
                    ViewBag.Visibility = false;
                    objReturn = await GetListSp(code);
                }
              
                ViewBag.Json = JsonConvert.SerializeObject(objReturn);
               
                return PartialView();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<List<DataPermissionJsonList>> GetListSp(string code)
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
                        bool booParentOpen = (string.IsNullOrEmpty(objData[i].PERM_PARENT)) ? false : true;
                        OptionState objStates = new OptionState { opened = booParentOpen, selected = varSelect };
                        objReturn.Add(new DataPermissionJsonList()
                        {
                            id = objData[i].PERM_ID.ToString(),
                            parent = strParent.ToString(),
                            text = objData[i].PERM_TEXT,
                            icon = strIcon,
                            state = objStates
                        });
                    }
                }
                return objReturn;
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public IActionResult SavePermission(string selectedItems, string roleCode)
        {
            List<TreeViewNode> items = new List<TreeViewNode>();
            try
            {
                if (selectedItems != null)
                    items = JsonConvert.DeserializeObject<List<TreeViewNode>>(selectedItems);                    
            }
            catch
            {
                throw;
            }
            return RedirectToAction("Index");
        }             
    }
}
