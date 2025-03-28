using System.ComponentModel.DataAnnotations;

namespace IaFit.Models
{
    public class NutritionPlanDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Gender { get; set; } = string.Empty;

        [Range(1, 150)]
        public int Age { get; set; }

        [Range(0.5, 3.0)]
        public decimal Height { get; set; }

        [Range(20, 500)]
        public decimal Weight { get; set; }

        [Required]
        public string Objective { get; set; } = string.Empty;

        [Required]
        public string ActivityLevel { get; set; } = string.Empty;
    }
}