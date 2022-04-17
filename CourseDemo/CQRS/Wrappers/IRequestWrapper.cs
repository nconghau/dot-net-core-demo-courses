using CourseDemo.Domain.Common;
using MediatR;
namespace CourseDemo.Services
{
    public interface IRequestWrapper<T> : IRequest<JsonResponse<T>>
    {
        
    }
    public interface IHandlerWrapper<TIn, TOut> : IRequestHandler<TIn, JsonResponse<TOut>>
        where TIn : IRequestWrapper<TOut>
    { }
}
