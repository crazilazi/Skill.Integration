using Microsoft.ML.Data;

namespace Skill.Integration.Models
{
    public class Employee
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CurrentPosition { get; set; }
        public List<string> Skills { get; set; }
    }

    public class SkillRecommendation
    {
        public string Skill { get; set; }
        public float Score { get; set; }
    }

    // Input data class for training
    public class EmployeeSkillMatch
    {
        [LoadColumn(0)]
        public string EmployeeSkills { get; set; } // Employee's current skills as text

        [LoadColumn(1)]
        public string RecommendedSkill { get; set; } // Skill to be recommended

        [LoadColumn(2), ColumnName("Label")]
        public bool IsRecommended { get; set; } // Whether the skill is a good match
    }

    // Prediction class
    public class SkillPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool IsRecommended { get; set; } // Predicted label for the skill recommendation

        public float Probability { get; set; } // Probability of being recommended
    }
}
