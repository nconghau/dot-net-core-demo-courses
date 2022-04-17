using CourseDemo.Domain.Common;
using CourseDemo.Models;
using CourseDemo.Repositories.Postgres;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using static CourseDemo.Enums.ResponseMessageEnum;

namespace CourseDemo.Services.CoursesPG.QueriesPG
{
    public class GetByIdCoursesPGQuery : IRequest<JsonResponse<CoursePGModel>>
    {
        public string Id { get; }
        public GetByIdCoursesPGQuery(string id)
        {
            Id = id;
        }
    };
    public class GetByIdCoursesPGQueryHandler : IRequestHandler<GetByIdCoursesPGQuery, JsonResponse<CoursePGModel>>
    {
        private readonly ICoursesPGRepository _coursePGRepository;
        private readonly ILogger<GetByIdCoursesPGQueryHandler> _logger;
        public GetByIdCoursesPGQueryHandler(ICoursesPGRepository coursePGRepository, ILogger<GetByIdCoursesPGQueryHandler> logger)
        {
            _coursePGRepository = coursePGRepository;
            _logger = logger;
        }
        public async Task<JsonResponse<CoursePGModel>> Handle(GetByIdCoursesPGQuery request, CancellationToken cancellationToken)
        {
            try
            {
                CoursePGModel course = await _coursePGRepository.GetByIdAsync(request.Id);
                if (course != null)
                {
                    return new JsonResponse<CoursePGModel>(true, course, ResponseMessage.FindByIdSuccess.ToString());
                }
            }
            catch (Exception e)
            {
                _logger.LogError("[Error] " + e);
            }
            return new JsonResponse<CoursePGModel>(false, null, ResponseMessage.FindByIdNotFound.ToString() + " Id=" + request.Id);
        }
    }
}
