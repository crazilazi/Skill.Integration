using System.Text.Json.Serialization;

namespace Skill.Integration.Models
{
    public class SkillType
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
    }

    public class Tag
    {
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;
    }

    public class Category
    {
        public int Id { get; set; }     
        public string Name { get; set; } = null!;
    }

    public class Attribution
    {
        public string Name { get; set; } = null!;    
        public string Text { get; set; } = null!;
    }

    public class SkillData
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;   
        public string InfoUrl { get; set; } = null!;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SkillType Type { get; set; } = null!;

        [JsonIgnore]
        public List<Tag> Tags { get; set; } = new();

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool IsSoftware { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool IsLanguage {  get; set; }

        [JsonIgnore]
        public string?Description { get; set; }

        [JsonIgnore]
        public string? DescriptionSource { get; set; }

        [JsonIgnore]
        public Category? Category {  get; set; }

        [JsonIgnore]
        public Category? SubCategory { get; set; }
    }

    public class SkillsObject
    {
        public List<Attribution> Attributions { get; set; } = new();
        public List<SkillData> Data { get; set; } = new();
    }

    public class SkillObject
    {
        public List<Attribution> Attributions { get; set; } = new();
        public SkillData Data { get; set; } = null!;
    }

    public class SkillRequest
    {
        public List<string>? ids { get; set; }
    }
}
