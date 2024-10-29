using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Skill.Integration.Models;
using Skill.Integration.Services;

namespace Skill.Integration.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LightCastController : ControllerBase
{    /// <summary>
     /// Add This key to the request header and value that we get from GetToken
     /// </summary>
    private const string TokenName = "LightCastToken";
    private ILightCastService _lightCastService;

   public LightCastController(ILightCastService lightCastService)
    {
        _lightCastService = lightCastService;
    }


    /// <summary>
    /// Get Server Status From LightCast
    /// </summary>
    /// <returns>Access Token</returns>
    [HttpGet("GetStatus")]
    public async Task<IActionResult> GetStatus()
    {
        return Ok(await _lightCastService.GetStatusAsync());
    }

    /// <summary>
    /// Get Versions of Skill API
    /// </summary>
    /// <returns>List of Versions</returns>
    [HttpGet("versions")]
    public async Task<IActionResult> GetVersions()
    {
        return Ok(await _lightCastService.GetVersionsAsync());
    }

    /// <summary>
    /// Get All Skills data
    /// </summary>
    /// <param name="version"></param>
    /// <returns></returns>
    [HttpGet("skills")]
    public async Task<IActionResult> GetSkills(string? version)
    {
        return Ok(await _lightCastService.GetSkillsAsync(version));
    }

    /// <summary>
    /// Get Skills data by Id
    /// </summary>
    /// <param name="request"></param>
    /// <param name="version"></param>
    /// <returns></returns>
    [HttpPost("skills")]
    public async Task<IActionResult> GetSkills([FromBody] SkillRequest request, string? version)
    {
        if (request?.ids == null || request.ids.Count == 0)
        {
            return BadRequest("No IDs provided.");
        }
        return Ok(await _lightCastService.GetSkillsAsync(request, version));
    }

    /// <summary>
    /// Get Related Skill data by Id
    /// </summary>
    /// <param name="request"></param>
    /// <param name="version"></param>
    /// <remarks>This Endpoint has 50 limit request per month, check response header for more details</remarks>
    /// <returns></returns>
    [HttpPost("relatedskills")]
    public async Task<IActionResult> GetRelatedSkills([FromBody] SkillRequest request, string? version)
    {
        if (request?.ids == null || request.ids.Count == 0)
        {
            return BadRequest("No IDs provided.");
        }
        var response = await _lightCastService.GetRelatedSkillsAsync(request, version);
        //foreach (var header in response.Headers)
        //{
        //    Response.Headers.TryAdd(header.Name, header.Value.ToString());
        //}
        //var result = JsonConvert.DeserializeObject<SkillsObject>(response.Content);
        return Ok(response);
    }

    /// <summary>
    /// Get Skill data by Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="version"></param>
    /// <returns></returns>
    [HttpGet("skills/{id}")]
    public async Task<IActionResult> GetSkill(string id,string? version)
    {
        return Ok(await _lightCastService.GetSkillByIdAsync(id,version));
    }

    /// <summary>
    /// Get All Skills
    /// </summary>
    /// <returns></returns>
    [HttpGet("allskills")]
    public async Task<IActionResult> GetAllSkills()
    {
        return Ok(await _lightCastService.GetAllSkills());
    }

}
