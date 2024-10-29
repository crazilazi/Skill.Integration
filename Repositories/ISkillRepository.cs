using Skill.Integration.Models;

namespace Skill.Integration.Repositories
{
    public interface ISkillRepository
    {
        /// <summary>
        /// Get all skills
        /// </summary>
        /// <returns></returns>
        IEnumerable<SkillData> GetAllSkills();
    }
}