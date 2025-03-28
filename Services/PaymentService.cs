using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace IaFit.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentService(IConfiguration configuration, HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> ProcessPayment(decimal amount)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? throw new InvalidOperationException("Usuário não autenticado.");
            var request = new
            {
                email = _configuration["PagSeguro:Email"],
                token = _configuration["PagSeguro:Token"],
                paymentMode = "default",
                paymentMethod = "creditCard",
                receiverEmail = _configuration["PagSeguro:Email"],
                currency = "BRL",
                itemId1 = "0001",
                itemDescription1 = "Assinatura DietSaaS",
                itemAmount1 = amount.ToString("F2"),
                itemQuantity1 = "1",
                reference = userId
            };

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://ws.pagseguro.uol.com.br/v2/checkout", content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}