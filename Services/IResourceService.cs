using Skill.Integration.Models;

namespace Skill.Integration.Services
{
    public interface IResourceService
    {
        Task<IEnumerable<Resource>> GetResources();
        Task AddSkill(int resourceId, IEnumerable<ResourceSkill> skills);
    }
}
