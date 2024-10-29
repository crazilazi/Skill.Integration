namespace Skill.Integration.Models
{
    public class Resource
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? RoleName { get; set; } // Nullable to match SQL NULL

        public ICollection<ResourceSkill> Skills { get; set; } = [];
    }
    public class ResourceSkill
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string InfoUrl { get; set; } = null!;
    }
}
