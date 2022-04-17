using CourseDemo.Domain.Common;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
namespace CourseDemo.CQRS.Validator
{
    public class ValidationHandler<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationHandler(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = _validators
                .Select(x => x.Validate(context))
                .SelectMany(x => x.Errors)
                .Where(x => x != null).
                ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }
            return next();
        }
    }
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                // Hanlde show error
                var code = HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)code;
                var jsonOptions = context.RequestServices.GetService<IOptions<JsonOptions>>();
                var jsonResponse = JsonSerializer.Serialize(
                    new JsonResponse<object>(false, null, e.Message.ToString()),
                    jsonOptions?.Value.JsonSerializerOptions);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
