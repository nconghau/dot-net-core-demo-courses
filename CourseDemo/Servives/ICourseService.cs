using CourseDemo.Domain.Common;
using CourseDemo.DTO.Response;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace CourseDemo.Servives
{
    public interface ICourseService
    {
        MemoryStream ExportExcelCourse(string keyword);
        MemoryStream ExportExcelCoursePS(string keyword);
        JsonResponse<ImportExcelResponseDto> ImportExcelCourse(IFormFile formFile);
        JsonResponse<ImportExcelResponseDto> ImportExcelCoursePS(IFormFile formFile);
    }
}