using MyWebApp.Core.Domain.Entities;
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
    }
}
