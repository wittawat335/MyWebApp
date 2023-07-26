using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Model;
using MyWebApp.Core.Model.ViewModels.Role;
using MyWebApp.Core.Services.Contract;
using MyWebApp.Core.Utility;
using Newtonsoft.Json;

namespace MyWebApp.Web.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly IRoleService _service;
        private readonly IPermissionService _permissionService;
        private readonly IHttpContextAccessor _contextAccessor;
        Common common = new Common();
        public RoleController(IRoleService service, IPermissionService permissionService, IHttpContextAccessor contextAccessor)
        {
            _service = service;
            _permissionService = permissionService;
            _contextAccessor = contextAccessor;
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
        public async Task<IActionResult> Save(RoleViewModel model)
        {
            var response = new Response<M_ROLE>();
            try
            {
                if (model != null)
                {
                    if (model.action == Constants.Action.New)
                    {
                        response.value = await _service.Add(model.role);
                        response.message = Constants.StatusMessage.Create_Action;
                        response.status = Constants.Status.True;
                    }
                    else if (model.action == Constants.Action.Edit)
                    {
                        response.status = await _service.Update(model.role);
                        response.message = Constants.StatusMessage.Update_Action;
                    }
                }
            }
            catch(Exception ex)
            {
                response.status = Constants.Status.False;
                response.message = Constants.StatusMessage.No_Data;
            }
            return new JsonResult(response);
        }
        [HttpPost]
        public async Task<IActionResult> _DetailsPermission(string code)
        {
            var model = new PermissionViewModel();
            try
            {
                if (code != null)
                {
                    model.RoleCode = code;
                    model.permAdd = await _permissionService.GetPermission(common.UserRole, Constants.ProgramCode.Role,
                        Constants.ActCode.RoleAdd);
                    model.permEdit = await _permissionService
                        .GetPermission(common.UserRole, Constants.ProgramCode.Role, Constants.ActCode.RoleEdit);
                }

                return PartialView(model);
            }
            catch (Exception)
            {
                throw;
            }

        }
        //public async Task<List<DataPermissionJsonList>> GetListSp(string code)
        //{
        //    try
        //    {
        //        List<DataPermissionJsonList> objReturn = new List<DataPermissionJsonList>();
        //        List<SP_SEARCH_PERMISSION_BY_ROLE_Result> objData = await _service.GetPermissionData(code);
        //        if (objData != null && objData.Count > 0)
        //        {
        //            int? iCountTopLevel = objData.Count;
        //            for (int i = 0; i < iCountTopLevel; i++)
        //            {
        //                bool varSelect = false;
        //                if (objData[i].PERM_SELECT == "1")
        //                    varSelect = true;

        //                string strIcon;
        //                switch (objData[i].PERM_TEXT)
        //                {
        //                    case Constants.JsTreeConfig.TextAdd:
        //                        strIcon = Constants.JsTreeConfig.IconAdd;
        //                        break;
        //                    case Constants.JsTreeConfig.TextEdit:
        //                        strIcon = Constants.JsTreeConfig.IconEdit;
        //                        break;
        //                    case Constants.JsTreeConfig.TextView:
        //                        strIcon = Constants.JsTreeConfig.IconView;
        //                        break;
        //                    default:
        //                        {
        //                            strIcon = Constants.JsTreeConfig.IconDefault;
        //                            break;
        //                        }
        //                }

        //                string strParent = (string.IsNullOrEmpty(objData[i].PERM_PARENT)) ? "#" : objData[i].PERM_PARENT;
        //                bool booParentOpen = false;
        //                OptionState objStates = new OptionState { opened = booParentOpen, selected = varSelect };
        //                objReturn.Add(new DataPermissionJsonList()
        //                {
        //                    id = objData[i].PERM_ID.ToString(),
        //                    parent = strParent.ToString(),
        //                    text = objData[i].PERM_TEXT,
        //                    icon = strIcon,
        //                    state = objStates
        //                });
        //            }
        //        }
        //        return objReturn;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        public async Task<IActionResult> GetJsTree(string roleCode)
        {
            List<DataPermissionJsonList> objReturn = new List<DataPermissionJsonList>();
            try
            {
                if (roleCode != null)
                {
                    List<SP_SEARCH_PERMISSION_BY_ROLE_Result> objData = await _service.GetPermissionData(roleCode);
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
                }
                return new JsonResult(objReturn);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public IActionResult GetListPermission([FromBody] List<DataPermissionJsonInsertList> permissionData)
        {
            var response = new Response<List<DataPermissionJsonInsertList>>();
            try
            {
                response.value = permissionData;
                if (permissionData.Count() > 0)
                {
                    string session = JsonConvert.SerializeObject(permissionData);
                    _contextAccessor.HttpContext.Session.SetString("listSelectedPermission", session);
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
        [HttpPost]
        public async Task<IActionResult> SavePermission(string roleCode)
        {
            var response = new Response<DataPermissionJsonInsertList>();
            List<DataPermissionJsonInsertList> list = new List<DataPermissionJsonInsertList>();
            try
            {
                string session = _contextAccessor.HttpContext.Session.GetString("listSelectedPermission");
                if (session != null)
                    list = JsonConvert.DeserializeObject<List<DataPermissionJsonInsertList>>(session);
                if (list.Count() > 0)
                {
                    if (await _service.DeletePermissionListByCode(roleCode))
                    {
                        response.status = await _service.Childrenlist(roleCode, list);
                        if (response.status)
                            response.message = Constants.StatusMessage.Update_Action;
                        else
                            response.message = Constants.StatusMessage.Could_Not_Create;
                    }
                    _contextAccessor.HttpContext.Session.Remove("listSelectedPermission");
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
