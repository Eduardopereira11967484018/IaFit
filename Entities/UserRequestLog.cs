namespace IaFit.Entities
{
    public class UserRequestLog
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
    }
}