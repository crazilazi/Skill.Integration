using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Skill.Integration.Models;
using Skill.Integration.Repositories;
using System.Net;

namespace Skill.Integration.Services;
public class LightCastService : ILightCastService
{
    private readonly IConfiguration _configuration;
    private readonly ISkillRepository _skillRepository;
    private string AccessToken { get; set; } = null!;

    private string getVersion(string? ver) => ver ?? "latest"; 
    public LightCastService(IConfiguration configuration, ISkillRepository skillRepository)
    {
        _configuration = configuration;
        _skillRepository = skillRepository;

    }

    public Token? GetToken()
    {
        var clientId = _configuration["LightCast:ClientId"];
        var clientSecret = _configuration["LightCast:ClientSecret"];
        var request = BuildRequest(true, Method.Post);
        request.AddParameter("application/x-www-form-urlencoded", $"client_id={clientId}&client_secret={clientSecret}&grant_type=client_credentials&scope=emsi_open", ParameterType.RequestBody);
        var response = CallLightCastAPI("https://auth.emsicloud.com/connect/token", request);
        return JsonConvert.DeserializeObject<Token>(response.Content);
    }

    public void SetToken(string? token)
    {
        if (String.IsNullOrEmpty(token))
        {
            throw new ArgumentNullException("Headers with Key LightCastToken is missing or invalid, Please Generate a new Token");
        }
        AccessToken = token;
    }

    public RestResponse GetStatus()
    {
        var request = BuildRequest();
        return CallLightCastAPI("https://emsiservices.com/skills/status", request);
    }

    public List<string> GetVersions()
    {
        var request = BuildRequest();
        var response = CallLightCastAPI("https://emsiservices.com/skills/versions",request);
        return ((JArray)JsonConvert.DeserializeObject<JObject>(response.Content)["data"]).ToObject<List<string>>();
    }

    public SkillsObject GetSkills(string? version)
    {
        var request = BuildRequest();
        var response = CallLightCastAPI($"https://emsiservices.com/skills/versions/{getVersion(version)}/skills?limit=10",request);
        return JsonConvert.DeserializeObject<SkillsObject>(response.Content);
    }

    public SkillsObject GetSkills(SkillRequest requestIds, string? version)
    {
        string body = JsonConvert.SerializeObject(requestIds);
        var request = BuildRequest(method: Method.Post);
        request.AddParameter("application/json", body, ParameterType.RequestBody);
        var response = CallLightCastAPI($"https://emsiservices.com/skills/versions/{getVersion(version)}/skills", request);
        return JsonConvert.DeserializeObject<SkillsObject>(response.Content);
    }

    public SkillObject GetSkillById(string id, string? version)
    {
        var request = BuildRequest();
        var response = CallLightCastAPI($"https://emsiservices.com/skills/versions/{getVersion(version)}/skills/{id}",request);
        return JsonConvert.DeserializeObject<SkillObject>(response.Content);
    }

    public RestResponse GetRelatedSkills(SkillRequest requestIds, string? version)
    {
        string body = JsonConvert.SerializeObject(requestIds);
        var request = BuildRequest(method:Method.Post);
        request.AddParameter("application/json", body, ParameterType.RequestBody);
       return CallLightCastAPI($"https://emsiservices.com/skills/versions/{getVersion(version)}/related", request);
    }

    public RestResponse CallLightCastAPI(string url, RestRequest request)
    {
        try
        {
            var client = new RestClient(url);
            var result = client.Execute(request);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(result.ErrorMessage);
            }
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    private RestRequest BuildRequest(bool TokenRequest = false, Method method = Method.Get)
    {
        var request = new RestRequest();
        request.Method = method;
        if (TokenRequest)
        {
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
        }
        else
        {
            request.AddHeader("Authorization", $"Bearer {AccessToken}");
        }

        return request;
    }

    /// <summary>
    /// Get all skills
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SkillData> GetAllSkills()
    {
        try
        {
            return _skillRepository.GetAllSkills();
        }
        catch (Exception)
        {
            throw;
        }
    }
}

