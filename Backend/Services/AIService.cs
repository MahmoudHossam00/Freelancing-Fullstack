//using Freelancing.DTOs;

//namespace Freelancing.Services
//{
//    public class AIService:IAIService
//    {
//        private readonly HttpClient _httpClient;
// namespace Freelancing.Services
// {
//     public class AIService : IAIService
//     {
//         private readonly HttpClient _httpClient;

//        public AIService(HttpClient httpClient)
//        {
//            _httpClient = httpClient;
//        }

//        public async Task<List<ProjectForAI_DTO>> GetRecommendedProjectsAsync(List<string> freelancerSkills, List<ProjectForAI_DTO> allProjects)
//        {
//            var requestBody = new
//            {
//                freelancerSkills = freelancerSkills,
//                projects = allProjects
//            };

//            var response = await _httpClient.PostAsJsonAsync("http://localhost:8000/recommend-projects", requestBody);

//            response.EnsureSuccessStatusCode();

//            var result = await response.Content.ReadFromJsonAsync<List<ProjectForAI_DTO>>();
//            return result ?? new List<ProjectForAI_DTO>();
//        }
//    }
//}
