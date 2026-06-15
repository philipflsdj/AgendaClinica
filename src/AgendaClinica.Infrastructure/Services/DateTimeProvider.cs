using AgendaClinica.Domain.Contracts.Services;


namespace AgendaClinica.Infrastructure.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
