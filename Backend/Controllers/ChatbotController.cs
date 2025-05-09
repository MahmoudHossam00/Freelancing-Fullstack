using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.Crmf;
using System.Text;

namespace Freelancing.Controllers
{

    public class ChatResponse
    {
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
    public class ChatRequest
    {
        public string Prompt { get; set; }
       
    }

    [ApiController]
    [Route("api/[controller]")]
    public class ChatbotController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        //public ApplicationDbContext _context { get; }

        public ChatbotController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            //_context = applicationDbContext;
        }





        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] ChatRequest request)
        {
        
            if (string.IsNullOrEmpty(request.Prompt))
            {
                return BadRequest(new ChatResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Prompt is required."
                });
            }

            try
            {

                var client = _httpClientFactory.CreateClient();
                var apiKey = Environment.GetEnvironmentVariable("AI_API_KEY");
                var apiUrl = _configuration["DeepSeek:ApiUrl"];

                if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiUrl))
                {
                    return StatusCode(500, new ChatResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "API configuration missing."
                    });
                }
                //var profileJson = request.ClientInfo != null ? System.Text.Json.JsonSerializer.Serialize(request.ClientInfo) : "{}";

                var systemContent = "You are Prolance, a professional freelance website consultant and expert in helping clients and freelancers succeed on freelance platforms. Your primary role is to assist clients who want to post projects but are unsure what skills or technologies to request. You provide clear, practical guidance by analyzing the type of project and suggesting relevant skills, tools, and freelancer roles needed to complete it effectively. For example, if a client wants a library web application, you recommend skills such as front-end development (HTML, CSS, JavaScript, Angular or React), back-end development (Node.js, Django, or Laravel), database management (MySQL, MongoDB), and UI/UX design. You always speak in a friendly, professional tone, and your advice is tailored for non-technical clients looking to hire efficiently and cost-effectively. You help define project scopes, estimate budgets, and choose the right freelancer profiles. Identify your name as Prolance when introducing yourself or when appropriate. Prioritize clarity, accuracy, and usefulness in every response.";

                //var systemContent = "You are Prolance, a professional freelance website consultant and expert in building, optimizing, and scaling websites for freelancers, agencies, and small businesses. Your role is to answer user questions clearly, accurately, and with practical advice tailored to freelance website needs. You speak in a friendly yet professional tone, focusing on value, performance, and user goals. You help with website creation, design, SEO, content strategy, performance optimization, platform recommendations (such as WordPress, Webflow, Wix, Shopify), and client acquisition strategies for freelancers. Always prioritize clarity, quality, and usefulness in your responses. When answering: Always assume the user is either a freelancer or someone hiring a freelancer. Recommend tools and strategies that fit a lean, cost-effective freelance setup. Focus on practical outcomes: building trust, showcasing skills, improving conversion, and managing projects efficiently. You may include code snippets, layout ideas, content tips, or marketing strategies if relevant. Identify your name as Prolance in responses when introducing yourself or when appropriate.";

                var payload = new
                {
                    model = "deepseek/deepseek-chat-v3-0324:free",
                    messages = new[]
                    {
                    new { role = "system",content = systemContent  },
                    new { role = "user", content = request.Prompt }
                },
                    max_tokens = 4000
                };

                var content = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(payload),
                    Encoding.UTF8,
                    "application/json");

                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                client.DefaultRequestHeaders.Add("HTTP-Referer", "https://Prolanceapi.com");
                client.DefaultRequestHeaders.Add("X-Title", "Prolance");

                // Call API
                var response = await client.PostAsync(apiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, new ChatResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "Failed to get response. Check API key or quota."
                    });
                }

                // Parse response
                var responseContent = await response.Content.ReadAsStringAsync();
                using var jsonDoc = System.Text.Json.JsonDocument.Parse(responseContent);
                var root = jsonDoc.RootElement;

                if (!root.TryGetProperty("choices", out var choices) || choices.GetArrayLength() == 0)
                {
                    return StatusCode(500, new ChatResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "Invalid API response."
                    });
                }

                var message = choices[0].GetProperty("message").GetProperty("content").GetString() ?? string.Empty;

                return Ok(new ChatResponse
                {
                    IsSuccess = true,
                    Message = message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ChatResponse
                {
                    IsSuccess = false,
                    ErrorMessage = $"Error: {ex.Message}"
                });
            }
        }
    }
}
