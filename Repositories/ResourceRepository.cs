using Dapper;
using Skill.Integration.Models;

namespace Skill.Integration.Repositories
{
    public class ResourceRepository : BaseRepository, IResourceRepository
    {
        public ResourceRepository(IConfiguration configuration): base(configuration)
        {
                
        }

        public async Task AddSkill(int resourceId, string[] skills)
        {
            using var conn = CreateConnection();
            conn.Open(); // Ensure the connection is opened
            using var transaction = conn.BeginTransaction();

            try
            {
                // Step 1: Delete existing skills for the resource
                var deleteSql = "DELETE FROM resourceskills WHERE resource_id = @ResourceId";
                await conn.ExecuteAsync(deleteSql, new { ResourceId = resourceId }, transaction);

                // Step 2: Insert new skills for the resource
                var insertSql = "INSERT INTO resourceskills (resource_id, skill_id) VALUES (@ResourceId, @SkillId)";
                foreach (var skillId in skills)
                {
                    await conn.ExecuteAsync(insertSql, new { ResourceId = resourceId, SkillId = skillId }, transaction);
                }

                // Commit transaction only if everything was successful
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback(); // Rollback transaction on error
                throw; // Rethrow the exception for further handling if needed
            }
        }

        public async Task<IEnumerable<Resource>> GetResources()
        {
            var sql = @"
            SELECT r.id AS Id, r.email AS Email, r.firstname AS FirstName, r.lastname AS LastName, r.rolename AS RoleName,
                   s.Id AS Id, s.Name, s.InfoUrl
            FROM resources r
            LEFT JOIN resourceskills rs ON r.id = rs.resource_id
            LEFT JOIN skills s ON rs.skill_id = s.Id";

            var resourceDictionary = new Dictionary<int, Resource>();
            using var conn = CreateConnection();
            var resources = await conn.QueryAsync<Resource, ResourceSkill, Resource>(
                sql,
                (resource, skill) =>
                {
                    if (!resourceDictionary.TryGetValue(resource.Id, out var resourceEntry))
                    {
                        resourceEntry = resource;
                        resourceEntry.Skills = new List<ResourceSkill>();
                        resourceDictionary.Add(resourceEntry.Id, resourceEntry);
                    }

                    if (skill != null)
                    {
                        resourceEntry.Skills.Add(skill);
                    }

                    return resourceEntry;
                },
                splitOn: "Id"
            );

            return resourceDictionary.Values;
        }
    }
}
