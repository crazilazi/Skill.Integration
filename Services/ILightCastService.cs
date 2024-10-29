using Skill.Integration.Models;

namespace Skill.Integration.Services
{
    public interface ILightCastService
    {
        Task<SkillsObject> GetSkillsAsync(string? version = null);
        Task<SkillsObject> GetSkillsAsync(SkillRequest requestIds, string? version = null);
        Task<SkillObject> GetSkillByIdAsync(string id, string? version = null);
        Task<dynamic> GetRelatedSkillsAsync(SkillRequest requestIds, string? version = null);
        Task<IEnumerable<string>> GetVersionsAsync();
        Task<dynamic> GetStatusAsync();
    }
}
