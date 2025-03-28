//using IaFit.Entities;
using Microsoft.EntityFrameworkCore;

namespace IaFit.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<NutritionPlan> NutritionPlans => Set<NutritionPlan>();
        public DbSet<Meal> Meals => Set<Meal>();
        public DbSet<UserRequestLog> UserRequestLogs => Set<UserRequestLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NutritionPlan>()
                .HasMany(np => np.Meals)
                .WithOne(m => m.NutritionPlan)
                .HasForeignKey(m => m.NutritionPlanId);

            modelBuilder.Entity<NutritionPlan>()
                .Property(np => np.Height)
                .HasColumnType("decimal(5,2)");

            modelBuilder.Entity<NutritionPlan>()
                .Property(np => np.Weight)
                .HasColumnType("decimal(5,2)");
        }
    }
}