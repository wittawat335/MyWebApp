using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CodeMetrics;
using MyWebApp.Core.DTO;
using MyWebApp.Core.Model;
using MyWebApp.Core.Model.ViewModels.Master;
using MyWebApp.Core.Model.ViewModels.User;
using MyWebApp.Core.Services.Contract;
using MyWebApp.Core.Utility;

namespace MyWebApp.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _service;
        private readonly IPermissionService _permissionService;
        Common common = new Common();

        public UserController(IUserService service, IPermissionService permissionService)
        {
            _service = service;
            _permissionService = permissionService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _service.getIndex());
        }

        [HttpPost]
        public async Task<IActionResult> _Detail(string code, string action)
        {
            return PartialView(await _service.getDetail(code, action));
        }

        [HttpPost]
        public async Task<IActionResult> GetList(UserViewModel model)
        {
            return new JsonResult(await _service.Search(model));
        }

        [HttpPost]
        public async Task<IActionResult> Save(UserViewModel model)
        {
            //           var response = new Response<bool>();
            //           try
            //           {
            //               if (model != null)
            //               {
            //                   if (model.roleSelect.Count(x => x.RoleFlag) > 0)
            //                   {
            //                       if (model.action == Constants.Action.New)
            //                       {
            //                           if (await _service.CheckDuplicate(model.userDTO.USER_LOGIN))
            //                           {
            //                               response.status =
            //await _service.Add(model.userDTO);
            //                               if (response.status)
            //                                   await _service.AddRole(model.roleSelect,
            //                                       model.userDTO.USER_LOGIN);
            //                               response.message =
            //                                   Constants.StatusMessage.Create_Action;
            //                           }
            //                           else
            //                           {
            //                               response.status = Constants.Status.False;
            //                               response.message = Constants.StatusMessage.Duplicate_User;
            //                           }
            //                       }
            //                       else if (model.action == Constants.Action.Edit)
            //                       {
            //                           response.status =
            //                               await _service.Update(model.userDTO);
            //                           if (response.status)
            //                               await _service.AddRole(model.roleSelect,
            //                                   model.userDTO.USER_LOGIN);
            //                           response.message =
            //                               Constants.StatusMessage.Update_Action;
            //                       }
            //                       else
            //                       {
            //                           response.status = Constants.Status.False;
            //                           response.message =
            //                               Constants.StatusMessage.No_Data;
            //                       }
            //                   }
            //                   else
            //                   {
            //                       response.status = Constants.Status.False;
            //                       response.message =
            //                           Constants.Msg.ValidateSelectRole;
            //                   }

            //               }
            //               else
            //               {
            //                   response.status = Constants.Status.False;
            //                   response.message = 
            //                       Constants.StatusMessage.No_Data;
            //               }

            //           }
            //           catch (Exception ex)
            //           {
            //               response.status = Constants.Status.False;
            //               response.message = ex.Message;
            //           }

            return new JsonResult(await _service.Save(model));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string code)
        {
            //var response = new Response<bool>();
            //try
            //{
            //    if (code != null)
            //    {
            //        response.value = await _service.Delete(code);
            //        response.message = 
            //            Constants.StatusMessage.Delete_Action;
            //        response.status = Constants.Status.True;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    response.status = Constants.Status.False;
            //    response.message = ex.Message;
            //}
            return new JsonResult(await _service.Delete(code));
        }
    }
}
