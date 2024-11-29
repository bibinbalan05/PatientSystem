using Microsoft.Data.SqlClient;

namespace Patient.Infrastructure.Data
{
    public interface IDbHelper : IDisposable
    {
        public SqlConnection GetConnection();
    }
}
