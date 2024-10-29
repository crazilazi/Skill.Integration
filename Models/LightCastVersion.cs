using System.Text.Json.Serialization;

namespace Skill.Integration.Models
{
    public class LightCastVersion
    {
        [JsonPropertyName("data")]
        public IEnumerable<string> Versions {  get; set; }
    }
}
