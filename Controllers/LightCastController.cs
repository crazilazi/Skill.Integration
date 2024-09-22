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
    /// Get Access Token From LightCast
    /// </summary>
    /// <returns>Access Token</returns>
    [HttpGet("GetToken")]
    public IActionResult GetToken()
    {
        return Ok(_lightCastService.GetToken());
    }

    /// <summary>
    /// Get Server Status From LightCast
    /// </summary>
    /// <returns>Access Token</returns>
    [HttpGet("GetStatus")]
    public IActionResult GetStatus()
    {
        SetToken();
        return Ok(_lightCastService.GetStatus().Content);
    }

    /// <summary>
    /// Get Versions of Skill API
    /// </summary>
    /// <returns>List of Versions</returns>
    [HttpGet("versions")]
    public IActionResult GetVersions()
    {
        SetToken();
        return Ok(_lightCastService.GetVersions());
    }

    /// <summary>
    /// Get All Skills data
    /// </summary>
    /// <param name="version"></param>
    /// <returns></returns>
    [HttpGet("skills")]
    public IActionResult GetSkills(string? version)
    {
        SetToken(); 
        return Ok(_lightCastService.GetSkills(version));
    }

    /// <summary>
    /// Get Skills data by Id
    /// </summary>
    /// <param name="request"></param>
    /// <param name="version"></param>
    /// <returns></returns>
    [HttpPost("skills")]
    public IActionResult GetSkills([FromBody] SkillRequest request, string? version)
    {
        if (request?.ids == null || request.ids.Count == 0)
        {
            return BadRequest("No IDs provided.");
        }
        SetToken();
        return Ok(_lightCastService.GetSkills(request, version));
    }

    /// <summary>
    /// Get Related Skill data by Id
    /// </summary>
    /// <param name="request"></param>
    /// <param name="version"></param>
    /// <remarks>This Endpoint has 50 limit request per month, check response header for more details</remarks>
    /// <returns></returns>
    [HttpPost("relatedskills")]
    public IActionResult GetRelatedSkills([FromBody] SkillRequest request, string? version)
    {
        if (request?.ids == null || request.ids.Count == 0)
        {
            return BadRequest("No IDs provided.");
        }
        SetToken();
        var response = _lightCastService.GetRelatedSkills(request, version);
        foreach (var header in response.Headers)
        {
            Response.Headers.TryAdd(header.Name, header.Value.ToString());
        }
        var result = JsonConvert.DeserializeObject<SkillsObject>(response.Content);
        return Ok(result);
    }

    /// <summary>
    /// Get Skill data by Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="version"></param>
    /// <returns></returns>
    [HttpGet("skills/{id}")]
    public IActionResult GetSkill(string id,string? version)
    {
        SetToken();
        return Ok(_lightCastService.GetSkillById(id,version));
    }

    private void SetToken()
    {
        Request.Headers.TryGetValue(TokenName, out var accessToken);
        _lightCastService.SetToken(accessToken);
    }
}
