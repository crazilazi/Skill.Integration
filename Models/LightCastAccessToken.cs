using System.Text.Json.Serialization;

namespace Skill.Integration.Models
{
    public class LightCastAccessToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = null!;

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        private int _tokenExpiresIn;

        [JsonPropertyName("expires_in")]
        public int TokenExpiresIn
        {
            get => _tokenExpiresIn;
            set
            {
                _tokenExpiresIn = value;
                ExpireAt = DateTime.Now.AddSeconds(_tokenExpiresIn);
            }
        }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        public DateTime ExpireAt { get; private set; }

        public bool IsValid()
        {
            return ExpireAt > DateTime.Now;
        }

    }
}
