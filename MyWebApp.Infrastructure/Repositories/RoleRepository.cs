using Dapper;
using MyWebApp.Core.Domain.RepositoryContracts;
using MyWebApp.Core.DTO;
using MyWebApp.Core.Model.ViewModels.Role;
using MyWebApp.Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MyWebApp.Core.Model.ViewModels.User.UserViewModel;

namespace MyWebApp.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DapperContext _dapperContext;

        public RoleRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<List<SP_SEARCH_PERMISSION_BY_ROLE_Result>> GetSP_SEARCH_PERMISSION(string code)
        {
            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    var procedure = "SP_SEARCH_PERMISSION_BY_ROLE";
                    var parameters = new DynamicParameters(new { ROLE_CODE = code });
                    var results = await connection.QueryAsync<SP_SEARCH_PERMISSION_BY_ROLE_Result>(procedure, parameters, 
                        commandType: CommandType.StoredProcedure);

                    return results.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
