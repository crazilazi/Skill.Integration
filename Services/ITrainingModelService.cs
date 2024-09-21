using Microsoft.ML;

namespace Skill.Integration.Services
{
    public interface ITrainingModelService
    {
        public ITransformer TrainModel();
    }
}
