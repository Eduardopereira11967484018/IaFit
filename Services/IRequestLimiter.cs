
namespace IaFit.Services
{
    public interface IRequestLimiter
    {
        Task<bool> CanMakeRequest();
        Task LogRequest();
    }
}