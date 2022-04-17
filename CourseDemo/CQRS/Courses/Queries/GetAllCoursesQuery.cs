using CourseDemo.Domain.Common;
using CourseDemo.DTO.Response;
using CourseDemo.Models;
using CourseDemo.Repositories.Mongo;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static CourseDemo.Enums.ResponseMessageEnum;

namespace CourseDemo.Services.Courses.Queries
{
    public class GetAllCoursesQuery : IRequest<JsonResponse<GetAllCourseResponseDto>>
    {
        public int Page { get; }
        public int PageSize { get; }
        public string Keyword { get; }
        public GetAllCoursesQuery(int page, int pagesize, string keyword)
        {
            PageSize = pagesize;
            Page = page;
            Keyword = keyword;
        }
    };
    public class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, JsonResponse<GetAllCourseResponseDto>>
    {
        private readonly ICoursesRepository _courseRepository;
        private readonly ILogger<GetAllCoursesQueryHandler> _logger;
        public GetAllCoursesQueryHandler(ICoursesRepository courseRepository, ILogger<GetAllCoursesQueryHandler> logger)
        {
            _courseRepository = courseRepository;
            _logger = logger;
        }
        public async Task<JsonResponse<GetAllCourseResponseDto>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                GetAllCourseResponseDto courses = await _courseRepository.GetAllAsync(request.Page, request.PageSize, request.Keyword);
                return new JsonResponse<GetAllCourseResponseDto>(true, courses, ResponseMessage.FindAllSuccess.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError("[Error] " + e);
                var datas = new List<CourseModel>().AsEnumerable();
                var pagination = new PaginationDto(0, 0, 0);
                GetAllCourseResponseDto getAllCourseDto = new GetAllCourseResponseDto(datas, pagination);
                return new JsonResponse<GetAllCourseResponseDto>(false, getAllCourseDto, e.Message.ToString());
            }
        }
    }
}
