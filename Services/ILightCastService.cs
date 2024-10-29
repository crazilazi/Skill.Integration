using RestSharp;
using Skill.Integration.Models;

namespace Skill.Integration.Services
{
    public interface ILightCastService
    {
        public Token? GetToken();
        public void SetToken(string token);
        public RestResponse GetStatus();
        public List<string> GetVersions();
        public SkillsObject GetSkills(string? version);
        public SkillsObject GetSkills(SkillRequest ids, string? version);
        public SkillObject GetSkillById(string id, string? version);
        public RestResponse GetRelatedSkills(SkillRequest requestIds, string? version);

        /// <summary>
        /// Get all skills
        /// </summary>
        /// <returns></returns>
        IEnumerable<SkillData> GetAllSkills();
    }
}
