using Dapper;
using MyWebApp.Core.Domain.RepositoryContracts;
using MyWebApp.Core.Model.ViewModels.TodoList;
using MyWebApp.Infrastructure.DBContext;
using System.Data;

namespace MyWebApp.Infrastructure.Repositories
{
    public class TodoListRepository : ITodoListRepository
    {
        private readonly NCLS_SITContext _dbContext;
        private readonly DapperContext _dapperContext;
     
        public TodoListRepository(
            NCLS_SITContext dbContext, 
            DapperContext dapperContext)
        {
            _dbContext = dbContext;
            _dapperContext = dapperContext;
        }

        public async Task<List<SP_SEARCH_CUSTOMER_Result>?> GetCustomer(string text)
        {
            try
            {
                using (var connection =
                   _dapperContext.CreateConnection())
                {
                    var procedure = "SP_SEARCH_CUSTOMER";
                    var parameters = new DynamicParameters
                        (new { SEARCH = text });
                    var results = await connection
                        .QueryAsync<SP_SEARCH_CUSTOMER_Result>(
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

        public async Task<List<RefNo>?> GetRefNoAsync(string text)
        {
            try
            {
                using (var connection =
                   _dapperContext.CreateConnection())
                {
                    var procedure = "SP_SEARCH_JOB";
                    var parameters = new DynamicParameters
                        (new { SEARCH = text });
                    var results = await connection
                        .QueryAsync<RefNo>(
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
    }
}
