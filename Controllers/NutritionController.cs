using IaFit.Entities;
using IaFit.Models;
using IaFit.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DietSaaS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NutritionController : ControllerBase
    {
        private readonly INutritionService _nutritionService;
        private readonly IPaymentService _paymentService;

        public NutritionController(INutritionService nutritionService, IPaymentService paymentService)
        {
            _nutritionService = nutritionService;
            _paymentService = paymentService;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/api/nutrition/generate" }, "Google");
        }

        [Authorize]
        [HttpGet("generate")]
        public async Task<IActionResult> GenerateNutritionPlan([FromQuery] NutritionPlanDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var plan = new NutritionPlan
            {
                Name = dto.Name,
                Gender = dto.Gender,
                Age = dto.Age,
                Height = dto.Height,
                Weight = dto.Weight,
                Objective = dto.Objective,
                ActivityLevel = dto.ActivityLevel
            };

            var result = await _nutritionService.GenerateNutritionPlan(plan);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("pay")]
        public async Task<IActionResult> ProcessPayment([FromQuery] decimal amount)
        {
            if (amount <= 0)
                return BadRequest("O valor deve ser maior que zero.");

            var paymentResult = await _paymentService.ProcessPayment(amount);
            return Ok(new { PaymentCode = paymentResult });
        }
    }
}