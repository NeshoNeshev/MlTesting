using Microsoft.ML.Data;

namespace areaOfLawPredict
{
    public class AreaOfLawPredict
    {
        [ColumnName("PredictedLabel")]
        public string Category { get; set; }

        public float[] Score { get; set; }
    }
}
