using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MyWebApp.Core.Utility;
using System.Data;
using static MyWebApp.Core.Utility.Constants;

namespace MyWebApp.Infrastructure.DBContext
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _environment;
        private readonly string _connectionString;
        private readonly string _server;
        private readonly string _port;
        private readonly string _user;
        private readonly string _password;
        private readonly string _databaseName;
        public DapperContext(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
            switch (environment.EnvironmentName)
            {
                case "Development":
                    _connectionString = configuration.GetConnectionString(Constants.ConnnectionString.SqlServer);
                    break;

                case "Staging":
                    _server = Environment.GetEnvironmentVariable("DB_HOST");
                    _port = Environment.GetEnvironmentVariable("DB_POST") ?? "1433";
                    _databaseName = Environment.GetEnvironmentVariable("DB_NAME");
                    _user = Environment.GetEnvironmentVariable("DB_USER") ?? "sa";
                    _password = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
                    _connectionString =
                        $"Server={_server},{_port};" +
                        $"Database={_databaseName};" +
                        $"User ID={_user};" +
                        $"Password={_password};" +
                        $"TrustServerCertificate=True;Trusted_Connection=false;MultipleActiveResultSets=True;";
                    break;

                case "Production":
                    _connectionString = configuration.GetConnectionString(Constants.ConnnectionString.SqlServer);
                    break;
            }
        }
        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
