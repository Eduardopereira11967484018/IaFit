namespace IaFit.Models
{
    public class NutritionPlanDto
    {
        public string Name { get; set; } = string.Empty;
        public string Weight { get; set; } = string.Empty;
        public string Height { get; set; } = string.Empty;
        public string Age { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Objective { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty; // Adicionado para corresponder ao frontend
    }
}