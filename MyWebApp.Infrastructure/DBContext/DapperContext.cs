using Microsoft.AspNetCore.Hosting.Server;
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
        private readonly string _server;
        private readonly string _user;
        private readonly string _password;
        private readonly string _databaseName;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _server = _configuration[Constants.DatabaseSetting.SqlServer.DBServer];
            _databaseName = _configuration[Constants.DatabaseSetting.SqlServer.DatabaseName];
            _user = _configuration[Constants.DatabaseSetting.SqlServer.DBUser];
            _password = _configuration[Constants.DatabaseSetting.SqlServer.DBPassword];
            _connectionString =
                $"Server={_server};" +
                $"Database={_databaseName};" +
                $"User ID={_user};" +
                $"Password={_password};" +
                $"TrustServerCertificate=True;Trusted_Connection=True;MultipleActiveResultSets=True;";
        }
        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
