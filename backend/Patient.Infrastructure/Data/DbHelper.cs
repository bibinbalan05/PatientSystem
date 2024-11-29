using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;


namespace Patient.Infrastructure.Data
{
    public class DbHelper : IDbHelper
    {
        private readonly SqlConnection _connection;

        public DbHelper(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(connectionString);
        }

        public SqlConnection GetConnection()
        {
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();
            return _connection;
        }

        public void Dispose()
        {
            if (_connection.State == ConnectionState.Open)
                _connection.Close();
        }
    }
}
