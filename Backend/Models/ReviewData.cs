using Microsoft.ML.Data;

namespace Freelancing.Models
{
    public class ReviewData
    {
        [LoadColumn(0)]  // Index 0: Text column from CSV
        public string Text { get; set; }

        [LoadColumn(1)]  // Index 1: Label column from CSV
        public bool Label { get; set; }
        
    }
}
