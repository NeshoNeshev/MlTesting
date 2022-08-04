using Microsoft.ML.Data;

namespace MlTesting
{
    public class JokeModelPrediction
    {
        [ColumnName("PredictedLabel")]
        public string Category { get; set; }

        public float[] Score { get; set; }
    }
}
