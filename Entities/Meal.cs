namespace AiFit.Entities
{
    public class Meal
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public TimeSpan Time { get; set; }
        public string Foods { get; set; } = string.Empty; // JSON string
        public int NutritionPlanId { get; set; }
        public NutritionPlan NutritionPlan { get; set; } = null!;
    }
}