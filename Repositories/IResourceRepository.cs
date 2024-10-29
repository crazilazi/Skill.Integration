using Skill.Integration.Models;

namespace Skill.Integration.Repositories
{
    public interface IResourceRepository
    {
        Task<IEnumerable<Resource>> GetResources();
        Task AddSkill(int resourceId, string[] skills);
    }
}
