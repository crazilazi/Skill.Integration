using Flurl.Http;
using Flurl;
using Skill.Integration.Models;
using System.Text.Json;

namespace Skill.Integration.Services
{
    public class LightCastService: ILightCastService
    {
        private readonly ILightCastTokenService _tokenService;
        private const string BaseUrl = "https://emsiservices.com/skills";
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        public LightCastService(ILightCastTokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<dynamic> GetStatusAsync()
        {
            var response = await CreateRequest("status")
                .GetStringAsync();
            return JsonSerializer.Deserialize<dynamic>(response, _jsonOptions)!;
        }

        public async Task<IEnumerable<string>> GetVersionsAsync()
        {
            var response = await CreateRequest("versions")
                .GetStringAsync();

            LightCastVersion result = JsonSerializer.Deserialize<LightCastVersion>(response, _jsonOptions)!;
            return result.Versions;
        }

        public async Task<SkillsObject> GetSkillsAsync(string? version = null)
        {
            var response = await CreateRequest($"versions/{GetVersion(version)}/skills")
                .SetQueryParam("limit", 10)
                .GetStringAsync();
            return JsonSerializer.Deserialize<SkillsObject>(response, _jsonOptions)!;
        }

        public async Task<SkillsObject> GetSkillsAsync(SkillRequest requestIds, string? version = null)
        {
            var response = await CreateRequest($"versions/{GetVersion(version)}/skills")
                .PostJsonAsync(requestIds)
                .ReceiveString();
            return JsonSerializer.Deserialize<SkillsObject>(response, _jsonOptions)!;
        }

        public async Task<SkillObject> GetSkillByIdAsync(string id, string? version = null)
        {
            var response = await CreateRequest($"versions/{GetVersion(version)}/skills/{id}")
                .GetStringAsync();
            return JsonSerializer.Deserialize<SkillObject>(response, _jsonOptions)!;
        }

        public async Task<dynamic> GetRelatedSkillsAsync(SkillRequest requestIds, string? version = null)
        {
            var response = await CreateRequest($"versions/{GetVersion(version)}/related")
                .PostJsonAsync(requestIds)
                .ReceiveString();
            return JsonSerializer.Deserialize<dynamic>(response, _jsonOptions)!;
        }

        private IFlurlRequest CreateRequest(string endpoint)
        {
            return BaseUrl.AppendPathSegment(endpoint)
                .WithHeader("Authorization", $"Bearer {_tokenService.GetValidTokenAsync().Result}");
        }

        private static string GetVersion(string? ver) => ver ?? "latest";
    }
}
