using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skill.Integration.Models;
using Skill.Integration.Services;

namespace Skill.Integration.Controllers
{
    [Route("api/resources")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceService resourceService;
        public ResourceController(IResourceService resourceService)
        {
            this.resourceService = resourceService;
        }

        /// <summary>
        /// Get All resources
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetResources()
        {
            return Ok(await this.resourceService.GetResources());
        }

        [HttpPut("{resourceId}/skills")]
        public async Task<IActionResult> AddSkill(int resourceId, IEnumerable<ResourceSkill> skills)
        {
            await this.resourceService.AddSkill(resourceId, skills);
            return Ok();
        }
    }
}
