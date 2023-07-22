using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Domain.RepositoryContracts;
using MyWebApp.Infrastructure.DBContext;
using MyWebApp.Core.DTO;
using MyWebApp.Core.Model;

namespace MyWebApp.Infrastructure.Repositories
{
    public class ProgramRepository : IProgramRepository 
    {
        private readonly NCLS_SITContext _dbContext;
        private readonly DapperContext _dapperContext;

        public ProgramRepository(NCLS_SITContext dbContext, DapperContext dapperContext) 
        {
            _dbContext = dbContext;
            _dapperContext = dapperContext;
        }

        public string GetStringBySpAsync(string command)
        {
            try
            {
                var result = _dbContext.Database.ExecuteSqlRaw($"SP_GET_MENU_DEFAULT MA");

                return result.ToString();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<ProgramDTO>> GetMenuByRole(string role)
        {           
            try
            {
                using (var connection =
                  _dapperContext.CreateConnection())
                {
                    var procedure = "SP_GET_MENU_BY_ROLE_TEST";
                    var parameters = new DynamicParameters
                        (new { UserRole = role});
                    var results = await connection
                        .QueryAsync<ProgramDTO>(
                        procedure,
                        parameters,
                        commandType: CommandType.StoredProcedure);

                    return results.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> GetMenuDefault(string code)
        {
            try
            {
                using (var connection =
                  _dapperContext.CreateConnection())
                {
                    var procedure = "SP_GET_MENU_DEFAULT";
                    var parameters = new DynamicParameters
                        (new { ROLE_CODE = code });
                    var results = await connection
                        .QueryFirstAsync<string>(
                        procedure,
                        parameters,
                        commandType: CommandType.StoredProcedure);

                    return results;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
