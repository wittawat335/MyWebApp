using Microsoft.AspNetCore.Http;
using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Domain.RepositoryContracts;
using MyWebApp.Core.Model;
using MyWebApp.Core.Model.ViewModels.Role;
using MyWebApp.Core.Services.Contract;
using MyWebApp.Core.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApp.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly IGenericRepository<M_ROLE> _repository;
        private readonly IGenericRepository<M_PERMISSION> _perRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionService _permissionService;
        private readonly IHttpContextAccessor _contextAccessor;
        Common common = new Common();

        public RoleService(IGenericRepository<M_ROLE> repository, IGenericRepository<M_PERMISSION> perRepository,
            IRoleRepository roleRepository, IPermissionService permissionService, IHttpContextAccessor contextAccessor)
        {
            _repository = repository;
            _roleRepository = roleRepository;
            _perRepository = perRepository;
            _permissionService = permissionService;
            _contextAccessor = contextAccessor;
        }
        private string progCode;
        public Response<List<DataPermissionJsonInsertList>> GetListPermission(List<DataPermissionJsonInsertList> permissionData)
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

            return response;
        }
        public async Task<Response<DataPermissionJsonInsertList>> SavePermission(string roleCode)
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
                    if (await DeletePermissionListByCode(roleCode))
                    {
                        response.status = await Childrenlist(roleCode, list);
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

            return response;
        }
        public async Task<RoleViewModel> getIndex()
        {
            var model = new RoleViewModel();
            model.permAdd = await _permissionService
              .GetPermission(common.UserRole, Constants.ProgramCode.Role, Constants.ActCode.RoleAdd);
            model.permEdit = await _permissionService
                .GetPermission(common.UserRole, Constants.ProgramCode.Role, Constants.ActCode.RoleEdit);
            model.permView = await _permissionService
                .GetPermission(common.UserRole, Constants.ProgramCode.Role, Constants.ActCode.RoleView);

            return model;
        }

        public async Task<Response<List<M_ROLE>>> Search()
        {
            var response = new Response<List<M_ROLE>>();
            try
            {
                response.value = await GetList();
                response.status = Constants.Status.True;
            }
            catch (Exception ex)
            {
                response.status = Constants.Status.False;
                response.message = ex.Message;
            }
            return response;
        }

        public async Task<RoleViewModel> getDetail(string code, string action)
        {
            var model = new RoleViewModel();
            try
            {
                if (code != null)
                    model.role = await GetByCode(code);

                model.action = action;
                model.permAdd = await _permissionService
                    .GetPermission(common.UserRole, Constants.ProgramCode.User, Constants.ActCode.UserAdd);
                model.permEdit = await _permissionService
                    .GetPermission(common.UserRole, Constants.ProgramCode.User, Constants.ActCode.UserEdit);

                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<PermissionViewModel> getDetailsPermission(string code)
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

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DataPermissionJsonList>> GetJsTree(string roleCode)
        {
            List<DataPermissionJsonList> objReturn = new List<DataPermissionJsonList>();
            try
            {
                if (roleCode != null)
                {
                    List<SP_SEARCH_PERMISSION_BY_ROLE_Result> objData = await GetPermissionData(roleCode);
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
                return objReturn;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ResponseStatus> Save(RoleViewModel model)
        {
            var response = new ResponseStatus();
            try
            {
                if (model != null)
                {
                    switch (model.action)
                    {
                        case Constants.Action.New:
                            response = await Add(model.role);
                            break;

                        case Constants.Action.Edit:
                            response.Status = await Update(model.role);
                            response.Message = Constants.StatusMessage.Update_Action;
                            break;

                        default:
                            response.Status = Constants.Status.False;
                            response.Message = Constants.StatusMessage.No_Data;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = Constants.Status.False;
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<bool> DeletePermissionListByCode(string code)
        {
            try
            {
                var query = await _perRepository.GetAll(x => x.PERM_ROLE_CODE == code);
                if (query.ToList().Count() > 0)
                    await _perRepository.DeleteList(query.ToList());

                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<M_ROLE> GetByCode(string code)
        {
            try
            {
                return await _repository.Get(x => x.ROLE_CODE == code);
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<M_ROLE>> GetList()
        {
            try
            {
                var list = await _repository.GetAll();

                return list.ToList();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<SP_SEARCH_PERMISSION_BY_ROLE_Result>> GetPermissionData(string code)
        {
            try
            {
                var list = await _roleRepository.GetSP_SEARCH_PERMISSION(code);
                return list.ToList();
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> Childrenlist(string roleCode, List<DataPermissionJsonInsertList> model)
        {
            bool result = false;
            try
            {
                foreach (DataPermissionJsonInsertList list in model)
                {
                    if (list.children.Count() > 0)
                    {
                        progCode = list.id;
                        await Childrenlist(roleCode, list.children);
                    }
                    else
                    {
                        if (list.state.selected)
                        {
                            M_PERMISSION perm = new M_PERMISSION();
                            perm.PERM_ROLE_CODE = roleCode;
                            perm.PERM_PROG_CODE = progCode;
                            perm.PERM_ACT_CODE = list.id;
                            perm.PERM_USE_FLAG = "1";
                            perm.PERM_CREATE_BY = common.UserLogin;
                            perm.PERM_CREATE_DATE = common.SystemDate;
                            perm.PERM_UPDATE_BY = common.UserLogin;
                            perm.PERM_UPDATE_DATE = common.SystemDate;
                            perm.PERM_STATUS = Constants.Status.Active;
                            _perRepository.AddOnly(perm);
                        }
                    }
                }
                await _perRepository.SaveChangesAsync();
                result = true;
                return result;
            }
            catch
            {
                throw;
            }
        }
        public async Task<ResponseStatus> Add(M_ROLE model)
        {
            var response = new ResponseStatus();
            try
            {
                await _repository.Add(model);
                response.Status = Constants.Status.True;
                response.Message = Constants.StatusMessage.Create_Action;
            }
            catch
            {
                throw;
            }
            return response;
        }

        public async Task<bool> Update(M_ROLE model)
        {
            try
            {
                var upDate = await _repository.Get(x => x.ROLE_CODE == model.ROLE_CODE);
                if (upDate != null)
                {
                    upDate.ROLE_NAME = model.ROLE_NAME;
                    upDate.ROLE_DATA_LEVEL = model.ROLE_DATA_LEVEL;
                    upDate.ROLE_UPDATE_BY = model.ROLE_UPDATE_BY;
                    upDate.ROLE_UPDATE_DATE = model.ROLE_UPDATE_DATE;
                    upDate.ROLE_STATUS = model.ROLE_STATUS;

                    await _repository.Update(upDate);

                }
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Delete(string code)
        {
            throw new NotImplementedException();
        }
    }
}
