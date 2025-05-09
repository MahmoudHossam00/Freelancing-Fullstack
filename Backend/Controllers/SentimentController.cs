using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;

namespace Freelancing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SentimentController : ControllerBase
    {
        private readonly PredictionEngine<ReviewData, ReviewPrediction> _predictionEngine;

        public SentimentController()
        {
            var mlContext = new MLContext();
            var modelPath = Path.Combine(Directory.GetCurrentDirectory(),  "MLModels","sentimentModel.zip");
            var mlModel = mlContext.Model.Load(modelPath, out _);
            _predictionEngine = mlContext.Model.CreatePredictionEngine<ReviewData, ReviewPrediction>(mlModel);
        }

        [HttpPost("analyze")]
        public IActionResult AnalyzeSentiment([FromBody] ReviewData input)
        {
            var result = _predictionEngine.Predict(input);
            return Ok(new
            {
                Prediction = result.Prediction ? "Positive" : "Negative",
                result.Probability
            });
        }
    }
}

