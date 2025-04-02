using IaFit.Data;
using IaFit.Entities;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace IaFit.Services
{
    public class NutritionService : INutritionService
    {
        private readonly IApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly IRequestLimiter _requestLimiter;

        public NutritionService(
            IApplicationDbContext context,
            IConfiguration configuration,
            HttpClient httpClient,
            IRequestLimiter requestLimiter)
        {
            _context = context;
            _configuration = configuration;
            _httpClient = httpClient;
            _requestLimiter = requestLimiter;
        }

        public async Task<NutritionPlan> GenerateNutritionPlan(NutritionPlan plan)
        {
            if (!await _requestLimiter.CanMakeRequest())
                throw new InvalidOperationException("Limite diário de requisições atingido.");

            plan.Meals = await GenerateMealsFromGemini(plan);
            plan.CreatedAt = DateTime.UtcNow;

            await _requestLimiter.LogRequest();
            return plan;
        }

        private async Task<List<Meal>> GenerateMealsFromGemini(NutritionPlan plan)
        {
            var apiKey = _configuration["GeminiApi:ApiKey"];
            var prompt = $"Crie uma dieta completa para uma pessoa com nome: {plan.Name}, " +
                         $"sexo: {plan.Gender}, idade: {plan.Age} anos, altura: {plan.Height}m, " +
                         $"peso: {plan.Weight}kg, objetivo: {plan.Objective}, " +
                         $"nível de atividade: {plan.ActivityLevel}. " +
                         $"Retorne em JSON com 'meals' contendo array de objetos com 'name', 'time' (HH:mm), 'foods' (array).";

            var requestBody = new { prompt, model = "gemini-1.5-flash" };
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.gemini.google.com/v1/generate", content); // Ajustar URL
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var geminiResult = JsonConvert.DeserializeObject<dynamic>(responseString);
            var mealsJson = geminiResult["meals"]?.ToString();

            return string.IsNullOrEmpty(mealsJson)
                ? new List<Meal> { new Meal { Name = "Fallback", Time = TimeSpan.Parse("08:00"), Foods = "[\"2 ovos\"]" } }
                : JsonConvert.DeserializeObject<List<MealDto>>(mealsJson)
                    .Select(m => new Meal
                    {
                        Name = m.Name,
                        Time = TimeSpan.Parse(m.Time),
                        Foods = JsonConvert.SerializeObject(m.Foods)
                    }).ToList();
        }

        private class MealDto
        {
            public string Name { get; set; } = string.Empty;
            public string Time { get; set; } = string.Empty;
            public string[] Foods { get; set; } = Array.Empty<string>();
        }
    }
}