using Microsoft.ML.Data;

namespace areaOfLawPredict
{
    public class CaseInputModel
    {
        [LoadColumn(0)]
        public string Category { get; set; }

        [LoadColumn(1)]
        public string Content { get; set; }
    }
}
