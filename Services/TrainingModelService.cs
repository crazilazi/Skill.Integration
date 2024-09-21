using Microsoft.ML;
using Skill.Integration.Helpers;

namespace Skill.Integration.Services
{
    public class TrainingModelService : ITrainingModelService
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
        /// The data generator
        /// </summary>
        private readonly DataGenerator dataGenerator;

        public TrainingModelService(DataGenerator dataGenerator)
        {
            this.mlContext = new MLContext(seed: 0);
            this.dataGenerator = dataGenerator;
        }

        public ITransformer TrainModel()
        {
            if (this.model != null)
            {
                return this.model;
            }

            // 1. Load training data into IDataView
            var trainingData = this.dataGenerator.GenerateEmployeeSkillMatchData();
            var data = this.mlContext.Data.LoadFromEnumerable(trainingData);

            // 2. Define the pipeline for transforming the data and training the model
            var pipeline = this.mlContext.Transforms.Text.NormalizeText("EmployeeSkills")
                .Append(this.mlContext.Transforms.Text.TokenizeIntoWords("EmployeeSkillsTokens", "EmployeeSkills"))
                .Append(this.mlContext.Transforms.Text.RemoveDefaultStopWords("EmployeeSkillsTokens"))
                .Append(this.mlContext.Transforms.Conversion.MapValueToKey("EmployeeSkillsTokensKey", "EmployeeSkillsTokens")) // Convert tokens to keys
                .Append(this.mlContext.Transforms.Text.ProduceNgrams("EmployeeSkillsNGrams", "EmployeeSkillsTokensKey"))
                .Append(this.mlContext.Transforms.Text.NormalizeText("RecommendedSkill"))
                .Append(this.mlContext.Transforms.Text.TokenizeIntoWords("RecommendedSkillTokens", "RecommendedSkill"))
                .Append(this.mlContext.Transforms.Text.RemoveDefaultStopWords("RecommendedSkillTokens"))
                .Append(this.mlContext.Transforms.Conversion.MapValueToKey("RecommendedSkillTokensKey", "RecommendedSkillTokens")) // Convert tokens to keys
                .Append(this.mlContext.Transforms.Text.ProduceNgrams("RecommendedSkillNGrams", "RecommendedSkillTokensKey"))
                .Append(this.mlContext.Transforms.Concatenate("Features", "EmployeeSkillsNGrams", "RecommendedSkillNGrams"))
                //.Append(this.mlContext.Transforms.Conversion.MapValueToKey("Label", "IsRecommended")) // Convert label to key
                .Append(this.mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));

            // 3. Train the model
            this.model = pipeline.Fit(data);
            return this.model;
        }
    }
}
