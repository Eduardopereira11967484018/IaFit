using IaFit.Entities;
using Microsoft.EntityFrameworkCore;

namespace IaFit.Data
{
    public interface IApplicationDbContext
    {
        DbSet<NutritionPlan> NutritionPlans { get; }
        DbSet<Meal> Meals { get; }
        DbSet<UserRequestLog> UserRequestLogs { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}