using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.DTO;
using MyWebApp.Core.Model;
using MyWebApp.Core.Model.ViewModels.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApp.Core.Services.Contract
{
    public interface IRoleService
    {
        Task<List<M_ROLE>> GetList();
        Task<M_ROLE> GetByCode(string code);
        Task<List<SP_SEARCH_PERMISSION_BY_ROLE_Result>> GetPermissionData(string code);
        Task<bool> DeletePermissionListByCode(string code);
        Task<bool> Childrenlist(string role, List<DataPermissionJsonInsertList> model);
        Task<ResponseStatus> Add(M_ROLE model);
        Task<bool> Update(M_ROLE model);
        Task<bool> Delete(string code);
        Task<RoleViewModel> getIndex();
        Task<Response<List<M_ROLE>>> Search();
        Task<RoleViewModel> getDetail(string code, string action);
        Task<ResponseStatus> Save(RoleViewModel model);
        Task<PermissionViewModel> getDetailsPermission(string code);
        Task<List<DataPermissionJsonList>> GetJsTree(string roleCode);
        Response<List<DataPermissionJsonInsertList>> GetListPermission(List<DataPermissionJsonInsertList> permissionData);
        Task<Response<DataPermissionJsonInsertList>> SavePermission(string roleCode);
    }
}
