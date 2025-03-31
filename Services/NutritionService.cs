using IaFit.Entities;
using System.Net.Http;
using System.Text.Json;

namespace IaFit.Services
{
    public class NutritionService : INutritionService
    {
        private readonly HttpClient _httpClient;

        private const string GeminiApiKey = "GEMINI_API_KEY";              

        public NutritionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<NutritionPlan> GenerateNutritionPlan(NutritionPlan plan)
        {
            var requestBody = new
            {
                prompt = $"Crie um plano de nutrição para uma pessoa com nome {plan.Name}, peso {plan.Weight}kg, altura {plan.Height}m, idade {plan.Age}, gênero {plan.Gender}, objetivo {plan.Objective}, nível de atividade {plan.ActivityLevel}, frequência de treino {plan.Frequency} vezes por semana.",
                model = "gemini-model" // Ajuste conforme a API
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.gemini.com/v1/completions") // URL fictícia, ajuste para a real
            {
                Headers = { { "Authorization", $"Bearer {GeminiApiKey}" } },
                Content = new StringContent(JsonSerializer.Serialize(requestBody), System.Text.Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                // Parsear a resposta do Gemini para preencher plan.Meals e plan.Supplements
                // Por enquanto, continuamos com o mock
            }

            // Mock temporário até integrar o Gemini
            plan.Meals = new List<Meal>
            {
                new Meal { Name = "Café da Manhã", Time = "08:00", Foods = new List<string> { "2 ovos", "1 pão integral" } }
            };
            if (plan.Frequency > 3)
            {
                plan.Meals.Add(new Meal { Name = "Lanche Pré-Treino", Time = "16:00", Foods = new List<string> { "1 banana", "1 scoop de whey protein" } });
            }
            plan.CreatedAt = DateTime.UtcNow;

            return plan;
        }
    }
}