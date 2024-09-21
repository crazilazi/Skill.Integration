using Skill.Integration.Models;

namespace Skill.Integration.Helpers
{
    public class DataGenerator
    {
        private readonly Random _random;
        private readonly List<string> Names;
        private readonly List<string> Positions;

        public List<Employee> Employees { get; private set; }
        public Dictionary<string, List<string>> PositionSkills { get; private set; }

        public DataGenerator()
        {
            _random = new Random();

            // Initialize employee names and positions
            Names = new List<string>
        {
            "Rajeev Ranjan", "Nandish Sargur", "Chetan Kini", "Vani Umesh", "Sagar", "Chetan Murthy",
            "Naveen Hadagali", "Drax Patil", "Paul parker", "Kattapa Bahubali"
        };

            Positions = new List<string>
        {
            "Software Engineer", "DevOps Engineer", "Data Scientist", "Full Stack Developer",
            "Backend Developer", "Frontend Developer", "Cloud Engineer", "System Administrator"
        };

            // Initialize skills based on position
            PositionSkills = new Dictionary<string, List<string>>()
        {
            { "Software Engineer", new List<string> { "C#", "Java", "Python", "JavaScript", "SQL", "Git", "REST", "Docker", "Kubernetes" } },
            { "DevOps Engineer", new List<string> { "Azure", "AWS", "DevOps", "Docker", "Linux", "CI/CD", "Jenkins", "Kubernetes" } },
            { "Data Scientist", new List<string> { "Python", "R", "SQL", "Data Science", "Machine Learning", "NoSQL", "PostgreSQL", "MongoDB" } },
            { "Full Stack Developer", new List<string> { "React", "Angular", "Node.js", "TypeScript", "JavaScript", "HTML", "CSS", "SQL", "NoSQL" } },
            { "Backend Developer", new List<string> { "C#", "Java", "Node.js", "Python", "SQL", "REST", "PostgreSQL", "MongoDB", "Microservices" } },
            { "Frontend Developer", new List<string> { "JavaScript", "HTML", "CSS", "React", "Angular", "TypeScript" } },
            { "Cloud Engineer", new List<string> { "AWS", "Azure", "Docker", "Kubernetes", "DevOps", "Linux", "CI/CD", "NoSQL" } },
            { "System Administrator", new List<string> { "Linux", "Windows", "Azure", "AWS", "Docker", "DevOps", "CI/CD", "Jenkins" } }
        };

            // Generate employees
            Employees = GenerateEmployees(100); // Default to 100 employees
        }

        // Generate a list of employees
        public List<Employee> GenerateEmployees(int numberOfEmployees)
        {
            var employees = new List<Employee>();

            for (int i = 1; i <= numberOfEmployees; i++)
            {
                var position = Positions[_random.Next(Positions.Count)];

                var employee = new Employee
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Names[_random.Next(Names.Count)],
                    CurrentPosition = position,
                    Skills = GenerateRandomSkills(position)
                };

                employees.Add(employee);
            }

            return employees;
        }

        // Generate a random list of skills based on position
        public List<string> GenerateRandomSkills(string position)
        {
            var relevantSkills = PositionSkills.ContainsKey(position) ? PositionSkills[position] : new List<string>();
            int skillCount = _random.Next(1, 5); // Each employee has between 1 and 5 skills
            return relevantSkills.OrderBy(x => _random.Next()).Take(skillCount).ToList();
        }

        // Generate employee skill match data for training purposes
        public List<EmployeeSkillMatch> GenerateEmployeeSkillMatchData()
        {
            var trainingData = new List<EmployeeSkillMatch>();

            foreach (var employee in Employees)
            {
                foreach (var skill in PositionSkills[employee.CurrentPosition])
                {
                    trainingData.Add(new EmployeeSkillMatch
                    {
                        EmployeeSkills = string.Join(", ", employee.Skills), // Employee profile as a string of their skills
                        RecommendedSkill = skill,
                        IsRecommended = true // Recommended skill based on the employee's position
                    });
                }
            }

            return trainingData;
        }
    }
}
