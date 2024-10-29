using Skill.Integration.Models;

namespace Skill.Integration.Services
{
    public interface ILightCastTokenService
    {
        Task<string> GetValidTokenAsync(bool isOpen = false);
        LightCastAccessToken? CurrentToken { get; }
    }
}
