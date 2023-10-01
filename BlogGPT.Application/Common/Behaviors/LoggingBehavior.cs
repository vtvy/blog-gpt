using BlogGPT.Application.Common.Interfaces.Identity;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace BlogGPT.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
    {
        private readonly ILogger _logger;
        private readonly IUser _user;
        private readonly IIdentityService _identityService;

        public LoggingBehavior(ILogger<TRequest> logger, IUser user, IIdentityService identityService)
        {
            _logger = logger;
            _user = user;
            _identityService = identityService;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _user.Id ?? string.Empty;
            string? userName = string.Empty;

            if (!string.IsNullOrEmpty(userId))
            {
                userName = await _identityService.GetUserNameAsync(userId);
            }

            _logger.LogInformation("BlogGPT Request: {@Name} {@UserId} {@UserName} {@Request}",
                requestName, userId, userName, request);
        }
    }
}
