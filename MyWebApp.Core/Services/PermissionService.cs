using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Domain.RepositoryContracts;
using MyWebApp.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MyWebApp.Core.Utility.Constants;

namespace MyWebApp.Core.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IGenericRepository<M_PERMISSION> _repository;

        public PermissionService(IGenericRepository<M_PERMISSION> repository)
        {
            _repository = repository;
        }

        public async Task<bool> GetPermission(string userRole, string programCode, string actCode)
        {
            try
            {
                var model = await _repository.GetAll(x => x.PERM_ROLE_CODE == userRole
                && x.PERM_ACT_CODE == actCode && x.PERM_PROG_CODE == programCode);

                if (model.ToList().Count > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
