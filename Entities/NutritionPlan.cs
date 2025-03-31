namespace IaFit.Entities
{
    public class NutritionPlan
    {
        public string Name { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public int Age { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public string Objective { get; set; } = string.Empty;
        public string ActivityLevel { get; set; } = string.Empty;
        public int Frequency { get; set; } // Adicionado como int no backend
        public List<Meal> Meals { get; set; } = new List<Meal>();
        public List<string> Supplements { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; }
    }

    public class Meal
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public List<string> Foods { get; set; } = new List<string>();
        public int NutritionPlanId { get; set; }
        public NutritionPlan NutritionPlan { get; set; } = null!;
    }
}