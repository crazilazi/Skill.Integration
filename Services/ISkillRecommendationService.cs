using Skill.Integration.Models;

namespace Skill.Integration.Services
{
    public interface ISkillRecommendationService
    {
        /// <summary>
        /// Gets the skill recommendations.
        /// </summary>
        /// <param name="employeeId">The employee identifier.</param>
        /// <param name="topN">The top n.</param>
        /// <returns></returns>
        public List<SkillRecommendation> GetSkillRecommendations(string employeeId, int topN = 3);
    }
}
