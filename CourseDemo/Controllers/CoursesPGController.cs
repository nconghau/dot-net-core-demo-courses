using CourseDemo.Domain.Common;
using CourseDemo.DTO.Response;
using CourseDemo.Models;
using CourseDemo.Services.CoursesPG.CommandsPG;
using CourseDemo.Services.CoursesPG.QueriesPG;
using CourseDemo.Servives;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CourseDemo.Controllers
{
    [Route("api/postgres/[controller]")]
    [ApiController]
    public class CoursesPGController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICourseService _courseService;
        public CoursesPGController(IMediator mediator, ICourseService courseService)
        {
            _mediator = mediator;
            _courseService = courseService;
        }
        [HttpGet]
        public Task<JsonResponse<GetAllCoursePGResponseDto>> GetAll(int page = 1, int pagesize = 10, string keyword = "")
        {
            return _mediator.Send(new GetAllCoursesPGQuery(page, pagesize, keyword));
        }
        [HttpGet("{id}")]

        public Task<JsonResponse<CoursePGModel>> GetById(string id = "")
        {
            return _mediator.Send(new GetByIdCoursesPGQuery(id));
        }

        [HttpPost]
        public Task<JsonResponse<object>> Create([FromBody] CreateCoursePGCommand createCoursePGCommand)
        {
            return _mediator.Send(createCoursePGCommand);
        }

        [HttpPut]
        public Task<JsonResponse<object>> Update([FromBody] UpdateCoursePGCommand updateCoursePGCommand)
        {
            return _mediator.Send(updateCoursePGCommand);
        }

        [HttpDelete("{id}")]
        public Task<JsonResponse<object>> Delete(string id = "")
        {
            return _mediator.Send(new DeleteCoursePGCommand(id));
        }

        [HttpPost("importExcelCourse")]
        public JsonResponse<ImportExcelResponseDto> ImportExcelCourse(IFormFile formFile)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                return new JsonResponse<ImportExcelResponseDto>(false, null, "File is empty");
            }
            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return new JsonResponse<ImportExcelResponseDto>(false, null, "Not Support file extension");
            }
            return _courseService.ImportExcelCoursePS(formFile);
        }
        [HttpGet("exportExcelCourse")]

        public IActionResult ExportExcelCourse(string keyword)
        {
            MemoryStream stream = _courseService.ExportExcelCoursePS(keyword);
            string excelName = $"Courses-{DateTime.Now:ddMMyyyy}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
    }
}
