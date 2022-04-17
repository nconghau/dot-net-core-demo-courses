using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
namespace CourseDemo.CQRS.Logger
{
    public class LoggingHandler<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingHandler<TRequest, TResponse>> _logger;
        public LoggingHandler(ILogger<LoggingHandler<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            //Request
            _logger.LogInformation("[Request] " + typeof(TRequest).FullName);
            //Response
            return await next();
        }
    }
}
