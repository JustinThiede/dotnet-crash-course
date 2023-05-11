using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ModelMapping.Data
{
    public class DataContextDapper
    {
        private IConfiguration _config;
        public DataContextDapper(IConfiguration config)
        {
            _config = config;
        }
        // private string _connectionString = "Server=localhost;Database=DotNetCourseDatabase;TrustServerCertificate=true;Trusted_Connection=true;";

        public IEnumerable<T> LoadData<T>(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(connectionString: _config.GetConnectionString("DefaultConnection"));

            return dbConnection.Query<T>(sql);
        }
        public T LoadDataSingle<T>(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(connectionString: _config.GetConnectionString("DefaultConnection"));

            return dbConnection.QuerySingle<T>(sql);
        }
        public bool ExecuteSql(string sql, object? parameters = null)
        {
            IDbConnection dbConnection = new SqlConnection(connectionString: _config.GetConnectionString("DefaultConnection"));

            return dbConnection.Execute(sql, parameters) > 0;
        }

        public int ExecuteSqlWithRowCount(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(connectionString: _config.GetConnectionString("DefaultConnection"));

            return dbConnection.Execute(sql);
        }
    }
}