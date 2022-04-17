using CourseDemo.Domain.Common;
using CourseDemo.DTO.Response;
using CourseDemo.Models;
using CourseDemo.Services.Courses.Commands;
using CourseDemo.Services.Courses.Queries;
using CourseDemo.Servives;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CourseDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICourseService _courseService;
        public CoursesController(IMediator mediator, ICourseService courseService)
        {
            _mediator = mediator;
            _courseService = courseService;
        }

        [HttpGet]
        public Task<JsonResponse<GetAllCourseResponseDto>> GetAll(int page = 1, int pagesize = 10, string keyword = "")
        {
            return _mediator.Send(new GetAllCoursesQuery(page, pagesize, keyword));
        }
        [HttpGet("{id:length(24)}")]

        public Task<JsonResponse<CourseModel>> GetById(string id = "")
        {
            return _mediator.Send(new GetByIdCoursesQuery(id));
        }

        [HttpPost]
        public Task<JsonResponse<object>> Create([FromBody] CreateCourseCommand createCourseCommand)
        {
            return _mediator.Send(createCourseCommand);
        }

        [HttpPut]
        public Task<JsonResponse<object>> Update([FromBody] UpdateCourseCommand updateCourseCommand)
        {
            return _mediator.Send(updateCourseCommand);
        }

        [HttpDelete("{id:length(24)}")]
        public Task<JsonResponse<object>> Delete(string id = "")
        {
            return _mediator.Send(new DeleteCourseCommand(id));
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
            return _courseService.ImportExcelCourse(formFile);
        }

        [HttpGet("exportExcelCourse")]
        public IActionResult ExportExcelCourse(string keyword = "")
        {
            MemoryStream stream = _courseService.ExportExcelCourse(keyword);
            string excelName = $"Courses-{DateTime.Now:ddMMyyyy}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
    }
}
