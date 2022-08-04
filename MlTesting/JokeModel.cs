using Microsoft.ML.Data;

namespace MlTesting
{
    
    public class JokeModel
    {
        [LoadColumn(0)]
        public string Category { get; set; }

        [LoadColumn(1)]
        public string Content { get; set; }
    }
}
