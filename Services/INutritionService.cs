using IaFit.Entities;

namespace IaFit.Services
{
    public interface INutritionService
    {
        Task<NutritionPlan> GenerateNutritionPlan(NutritionPlan plan);
    }
}