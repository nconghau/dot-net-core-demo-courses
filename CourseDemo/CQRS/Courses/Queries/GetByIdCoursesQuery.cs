using CourseDemo.Domain.Common;
using CourseDemo.Models;
using CourseDemo.Repositories.Mongo;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using static CourseDemo.Enums.ResponseMessageEnum;

namespace CourseDemo.Services.Courses.Queries
{
    public class GetByIdCoursesQuery : IRequest<JsonResponse<CourseModel>>
    {
        public string Id { get; }
        public GetByIdCoursesQuery(string id)
        {
            Id = id;
        }
    };
    public class GetByIdCoursesQueryHandler : IRequestHandler<GetByIdCoursesQuery, JsonResponse<CourseModel>>
    {
        private readonly ICoursesRepository _courseRepository;
        private readonly ILogger<GetByIdCoursesQueryHandler> _logger;
        public GetByIdCoursesQueryHandler(ICoursesRepository courseRepository, ILogger<GetByIdCoursesQueryHandler> logger)
        {
            _courseRepository = courseRepository;
            _logger = logger;
        }
        public async Task<JsonResponse<CourseModel>> Handle(GetByIdCoursesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                CourseModel course = await _courseRepository.GetByIdAsync(request.Id);
                if (course != null)
                {
                    return new JsonResponse<CourseModel>(true, course, ResponseMessage.FindByIdSuccess.ToString());
                }
            }
            catch (Exception e)
            {
                _logger.LogError("[Error] " + e);
            }
            return new JsonResponse<CourseModel>(false, null, ResponseMessage.FindByIdNotFound.ToString() + " Id=" + request.Id);
        }
    }
}
