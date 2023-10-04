using BlogGPT.Application.Common.Interfaces.Services;

namespace BlogGPT.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
