namespace IaFit.Services
{
    public interface IPaymentService
    {
        Task<string> ProcessPayment(decimal amount);
    }
}