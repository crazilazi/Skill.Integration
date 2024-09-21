using Microsoft.AspNetCore.Mvc;
using Skill.Integration.Helpers;
using Skill.Integration.Services;

namespace Skill.Integration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillRecommendationController : ControllerBase
    {
        /// <summary>
        /// The skill recommendation service
        /// </summary>
        private readonly ISkillRecommendationService skillRecommendationService;

        /// <summary>
        /// The data generator
        /// </summary>
        private readonly DataGenerator dataGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillRecommendationController"/> class.
        /// </summary>
        /// <param name="skillRecommendationService">The skill recommendation service.</param>
        public SkillRecommendationController(ISkillRecommendationService skillRecommendationService, DataGenerator dataGenerator)
        {
            this.skillRecommendationService = skillRecommendationService;
            this.dataGenerator = dataGenerator;
        }

        /// <summary>
        /// Gets the employee.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("~/api/employee")]
        public IActionResult GetEmployee()
        {
            return Ok(this.dataGenerator.Employees);
        }

        /// <summary>
        /// Gets the skill recommendations.
        /// </summary>
        /// <param name="employeeId">The employee identifier.</param>
        /// <param name="topN">The top n.</param>
        /// <returns></returns>
        [HttpGet("recommend/{employeeId}")]
        public IActionResult GetSkillRecommendations(string employeeId, [FromQuery] int topN = 3)
        {
            try
            {
                var recommendations = this.skillRecommendationService.GetSkillRecommendations(employeeId, topN);
                return Ok(recommendations);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
