namespace Aifit.Entities
{
    public class NutritionPlan
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public int Age { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public string Objective { get; set; } = string.Empty;
        public string ActivityLevel { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<Meal> Meals { get; set; } = new();
    }
}