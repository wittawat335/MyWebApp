using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MyWebApp.Core.Utility;
using System.Data;

namespace MyWebApp.Infrastructure.DBContext
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString(Constants.ConnnectionString.SqlServer);
        }       
        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
