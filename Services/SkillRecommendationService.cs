using Microsoft.ML;
using Skill.Integration.Helpers;
using Skill.Integration.Models;

namespace Skill.Integration.Services
{
    public class SkillRecommendationService : ISkillRecommendationService
    {
        /// <summary>
        /// The ml context
        /// </summary>
        private readonly MLContext mlContext;

        /// <summary>
        /// The model
        /// </summary>
        private ITransformer model;

        /// <summary>
        /// The employees
        /// </summary>
        private List<Employee> employees;

        /// <summary>
        /// The data generator
        /// </summary>
        private readonly DataGenerator dataGenerator;

        /// <summary>
        /// The training model service
        /// </summary>
        private readonly ITrainingModelService trainingModelService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillRecommendationService"/> class.
        /// </summary>
        public SkillRecommendationService(ITrainingModelService trainingModelService, DataGenerator dataGenerator)
        {
            this.employees = dataGenerator.Employees;
            this.mlContext = new MLContext(seed: 0);
            this.trainingModelService = trainingModelService;
            this.model = this.trainingModelService.TrainModel();
            this.dataGenerator = dataGenerator;
        }

        /// <summary>
        /// Gets the skill recommendations.
        /// </summary>
        /// <param name="employeeId">The employee identifier.</param>
        /// <param name="topN">The top n.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Model not trained or loaded. Please train or load the model first.</exception>
        /// <exception cref="System.ArgumentException">Employee not found.</exception>
        public List<SkillRecommendation> GetSkillRecommendations(string employeeId, int topN = 3)
        {
            if (this.model == null)
            {
                throw new InvalidOperationException("Model not trained or loaded. Please train or load the model first.");
            }

            var employee = employees.FirstOrDefault(e => e.Id == employeeId);
            if (employee == null)
                throw new ArgumentException("Employee not found.");

            var predictionEngine = this.mlContext.Model.CreatePredictionEngine<EmployeeSkillMatch, SkillPrediction>(this.model);
            var recommendedSkills = new List<SkillRecommendation>();

            // Predict skills for the employee based on historical data
            var skillsToEvaluate = GetPossibleSkills(employee.CurrentPosition); // Fetch skills dynamically based on the position
            foreach (var skill in skillsToEvaluate)
            {
                var prediction = predictionEngine.Predict(new EmployeeSkillMatch
                {
                    EmployeeSkills = string.Join(" ", employee.Skills),
                    RecommendedSkill = skill
                });

                if (prediction.IsRecommended)
                {
                    recommendedSkills.Add(new SkillRecommendation
                    {
                        Skill = skill,
                        Score = prediction.Probability
                    });
                }
            }

            // Return top N recommended skills
            return recommendedSkills.OrderByDescending(s => s.Score).Take(topN).ToList();
        }

        /// <summary>
        /// Dynamically gets the possible skills based on employee position.
        /// </summary>
        /// <param name="position">The position of the employee.</param>
        /// <returns>A list of possible skills for the position.</returns>
        private List<string> GetPossibleSkills(string position)
        {
            var positionSkills = this.dataGenerator.PositionSkills; // Get skills for all positions from DataGenerator
            if (positionSkills.ContainsKey(position))
            {
                return positionSkills[position];
            }

            // Default to a general set of skills if the position isn't found
            return positionSkills.SelectMany(kv => kv.Value).Distinct().ToList();
        }
    }

}
