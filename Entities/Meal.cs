namespace IaFit.Entities
{
    public class Meal
    {
        public string? Name { get; set; }
        public string? Time { get; set; }
        public List<string>? Foods { get; set; }
        public int NutritionPlanId { get; set; }
        public NutritionPlan? NutritionPlan { get; set; }
    }
}