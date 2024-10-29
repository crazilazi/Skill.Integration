using Skill.Integration.Models;
using Skill.Integration.Repositories;

namespace Skill.Integration.Services
{
    public class ResourceService : IResourceService
    {
        private readonly IResourceRepository resourceRepository;
        public ResourceService(IResourceRepository resourceRepository)
        {
            this.resourceRepository = resourceRepository;  
        }

        public async Task AddSkill(int resourceId, IEnumerable<ResourceSkill> skills)
        {
          var skillIds = skills.Select(x => x.Id).ToArray();
          await  this.resourceRepository.AddSkill(resourceId, skillIds);
        }

        public async Task<IEnumerable<Resource>> GetResources()
        {
            try
            {
                return await this.resourceRepository.GetResources();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
