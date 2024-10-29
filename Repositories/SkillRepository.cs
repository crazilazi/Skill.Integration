using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Skill.Integration.Models;

namespace Skill.Integration.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly string _connectionString = "Server=localhost\\SQLEXPRESS;Database=audit03_DEMO_PHX_01;Trusted_Connection=True;";


        public IEnumerable<SkillData> GetAllSkills()
        {

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM dbo.skills";
                return db.Query<SkillData>(query);
            }

        }
    }
}