using Microsoft.ML.Data;

namespace Freelancing.Models
{
    public class ReviewPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }

        public float Probability { get; set; }
        public float Score { get; set; }

    }
}
