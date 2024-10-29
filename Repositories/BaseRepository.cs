using System.Data.SqlClient;
using System.Data;

namespace Skill.Integration.Repositories
{
    public class BaseRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        protected BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        protected IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
