using Dapper;
using Skill.Integration.Models;

namespace Skill.Integration.Repositories
{
    public class SkillRepository : BaseRepository, ISkillRepository
    {
        public SkillRepository(IConfiguration configuration) : base(configuration) { }

        public IEnumerable<SkillData> GetAllSkills()
        {
            using (var db = CreateConnection())
            {
                const string query = "SELECT * FROM dbo.skills";
                return db.Query<SkillData>(query);
            }
        }
    }
}