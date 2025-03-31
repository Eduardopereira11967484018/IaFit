using IaFit.Entities;
using IaFit.Models;
using IaFit.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IaFit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NutritionController : ControllerBase
    {
        private readonly INutritionService _nutritionService;

        public NutritionController(INutritionService nutritionService)
        {
            _nutritionService = nutritionService;
        }

        [HttpGet("teste")]
        public IActionResult Teste()
        {
            string responseText = @"{
                ""nome"": ""Matheus"",
                ""sexo"": ""Masculino"",
                ""idade"": 28,
                ""altura"": 1.80,
                ""peso"": 74,
                ""objetivo"": ""Hipertrofia"",
                ""refeicoes"": [
                    { ""horario"": ""08:00"", ""nome"": ""Café da Manhã"", ""alimentos"": [""2 fatias de pão integral"", ""2 ovos mexidos"", ""1 banana"", ""200ml de leite desnatado""] },
                    { ""horario"": ""10:00"", ""nome"": ""Lanche da Manhã"", ""alimentos"": [""1 iogurte grego natural"", ""1 scoop de whey protein"", ""1 colher de sopa de granola""] },
                    { ""horario"": ""13:00"", ""nome"": ""Almoço"", ""alimentos"": [""150g de frango grelhado"", ""1 xícara de arroz integral"", ""1 xícara de brócolis cozido"", ""Salada verde à vontade""] },
                    { ""horario"": ""16:00"", ""nome"": ""Lanche da Tarde"", ""alimentos"": [""1 batata doce média"", ""1 scoop de whey protein""] },
                    { ""horario"": ""20:00"", ""nome"": ""Jantar"", ""alimentos"": [""150g de carne vermelha magra"", ""1 xícara de batata doce cozida"", ""1 xícara de couve refogada"", ""Salada verde à vontade""] },
                    { ""horario"": ""22:00"", ""nome"": ""Lanche antes de dormir"", ""alimentos"": [""200ml de leite desnatado"", ""1 scoop de caseína""] }
                ],
                ""suplementos"": [""Whey Protein"", ""Creatina"", ""BCAA"", ""Glutamina""]
            }";

            try
            {
                var jsonObject = JsonConvert.DeserializeObject<NutritionPlan>(responseText);
                return Ok(new { data = jsonObject });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { Message = "Erro ao processar o JSON", Details = ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] NutritionPlanDto dto)
        {
            var nutrition = await _nutritionService.GenerateNutritionPlan(new NutritionPlan
            {
                Name = dto.Name,
                Weight = double.TryParse(dto.Weight, out double weight) ? (decimal)weight : 0m,
                Height = double.TryParse(dto.Height, out double height) ? (decimal)height : 0m,
                Age = int.TryParse(dto.Age, out int age) ? age : 0,
                Gender = dto.Gender,
                Objective = dto.Objective,
                ActivityLevel = dto.Level,
                Frequency = int.TryParse(dto.Frequency, out int frequency) ? frequency : 0 // Conversão de Frequency
            });

            return Ok(nutrition);
        }
    }
}