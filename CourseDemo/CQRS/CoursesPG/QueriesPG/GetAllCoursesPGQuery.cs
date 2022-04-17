using CourseDemo.Domain.Common;
using CourseDemo.DTO.Response;
using CourseDemo.Models;
using CourseDemo.Repositories.Postgres;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static CourseDemo.Enums.ResponseMessageEnum;

namespace CourseDemo.Services.CoursesPG.QueriesPG
{
    public class GetAllCoursesPGQuery : IRequest<JsonResponse<GetAllCoursePGResponseDto>>
    {
        public int Page { get; }
        public int PageSize { get; }
        public string Keyword { get; }
        public GetAllCoursesPGQuery(int page, int pagesize, string keyword)
        {
            PageSize = pagesize;
            Page = page;
            Keyword = keyword;
        }
    };
    public class GetAllCoursesPGQueryHandler : IRequestHandler<GetAllCoursesPGQuery, JsonResponse<GetAllCoursePGResponseDto>>
    {
        private readonly ICoursesPGRepository _coursePGRepository;
        private readonly ILogger<GetAllCoursesPGQueryHandler> _logger;
        public GetAllCoursesPGQueryHandler(ICoursesPGRepository coursePGRepository, ILogger<GetAllCoursesPGQueryHandler> logger)
        {
            _coursePGRepository = coursePGRepository;
            _logger = logger;
        }
        public async Task<JsonResponse<GetAllCoursePGResponseDto>> Handle(GetAllCoursesPGQuery request, CancellationToken cancellationToken)
        {
            try
            {
                GetAllCoursePGResponseDto courses = await _coursePGRepository.GetAllAsync(request.Page, request.PageSize, request.Keyword);
                return new JsonResponse<GetAllCoursePGResponseDto>(true, courses, ResponseMessage.FindAllSuccess.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError("[Error] " + e);
                var datas = new List<CoursePGModel>().AsEnumerable();
                var pagination = new PaginationDto(0, 0, 0);
                GetAllCoursePGResponseDto getAllCourseDto = new GetAllCoursePGResponseDto(datas, pagination);
                return new JsonResponse<GetAllCoursePGResponseDto>(false, getAllCourseDto, e.Message.ToString());
            }
        }
    }
}
