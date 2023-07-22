using MyWebApp.Core.Model.ViewModels.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApp.Core.Domain.RepositoryContracts
{
    public interface IRoleRepository
    {
        Task<List<SP_SEARCH_PERMISSION_BY_ROLE_Result>> GetSP_SEARCH_PERMISSION(string code);
    }
}
