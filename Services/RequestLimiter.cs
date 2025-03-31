using IaFit.Data;
using IaFit.Entities;
// Removed unused or invalid namespace reference
using Microsoft.EntityFrameworkCore;

namespace IaFit.Services
{
    public class RequestLimiter : IRequestLimiter
    {
        private readonly IApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const int DailyLimit = 3;

        public RequestLimiter(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> CanMakeRequest()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
                throw new InvalidOperationException("Usuário não autenticado.");

            var today = DateTime.UtcNow.Date;
            var requestCount = await _context.UserRequestLogs
                .CountAsync(log => log.UserId == userId && log.RequestDate.Date == today);
            return requestCount < DailyLimit;
        }

        public async Task LogRequest()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
                return;

            _context.UserRequestLogs.Add(new UserRequestLog
            {
                UserId = userId,
                RequestDate = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
        }
    }
}