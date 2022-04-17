using CourseDemo.Domain.Common;
using CourseDemo.DTO.Request;
using CourseDemo.DTO.Response;
using CourseDemo.Models;
using CourseDemo.Repositories.Mongo;
using CourseDemo.Repositories.Postgres;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using static CourseDemo.Enums.ResponseMessageEnum;

namespace CourseDemo.Servives
{
    public class CourseService : ICourseService
    {
        private readonly ICoursesRepository _courseRepository;
        private readonly ICoursesPGRepository _coursePSRepository;
        private readonly ILogger<CourseService> _logger;
        public CourseService(ICoursesRepository courseRepository, ICoursesPGRepository coursePSRepository, ILogger<CourseService> logger)
        {
            _courseRepository = courseRepository;
            _coursePSRepository = coursePSRepository;
            _logger = logger;
        }
        public JsonResponse<ImportExcelResponseDto> ImportExcelCourse(IFormFile formFile)
        {
            try
            {
                int recordInserted = 0;
                int recordFail = 0;
                MemoryStream ms = new MemoryStream();
                formFile.CopyToAsync(ms);
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                using (var package = new ExcelPackage(ms))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            CreateCourseRequestDto createCourseRequestDto = new CreateCourseRequestDto
                            {
                                Name = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                Content = worksheet.Cells[row, 2].Value.ToString().Trim()
                            };
                            // Insert
                            bool success = _courseRepository.Create(createCourseRequestDto);
                            if (success)
                            {
                                recordInserted += 1;
                            }
                            else
                            {
                                recordFail += 1;
                            }
                        }
                        catch (Exception e)
                        {
                            _logger.LogError("[Error] " + e.Message.ToString());
                            recordFail += 1;
                            continue;
                        }
                    }
                }
                return new JsonResponse<ImportExcelResponseDto>(true, new ImportExcelResponseDto(recordInserted, recordFail), ResponseMessage.CreateSuccess.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError("[Error] " + e.Message.ToString());
                return new JsonResponse<ImportExcelResponseDto>(false, null, e.Message.ToString());
            }
        }
        public MemoryStream ExportExcelCourse(string keyword)
        {
            List<CourseModel> courses = _courseRepository.GetAllByKeyWord(keyword);
            MemoryStream stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Courses");
                // Add Title
                using (var r = workSheet.Cells["A1:D1"])
                {
                    r.Style.Font.Bold = true;
                }
                // Add Datas
                workSheet.Cells.LoadFromCollection(courses, true);
                package.Save();
            }
            stream.Position = 0;
            return stream;
        }
        public JsonResponse<ImportExcelResponseDto> ImportExcelCoursePS(IFormFile formFile)
        {
            try
            {
                int recordInserted = 0;
                int recordFail = 0;
                MemoryStream ms = new MemoryStream();
                formFile.CopyToAsync(ms);
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                using (var package = new ExcelPackage(ms))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            CreateCourseRequestDto createCourseRequestDto = new CreateCourseRequestDto
                            {
                                Name = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                Content = worksheet.Cells[row, 2].Value.ToString().Trim()
                            };
                            // Insert
                            bool success = _coursePSRepository.Create(createCourseRequestDto);
                            if (success)
                            {
                                recordInserted += 1;
                            }
                            else
                            {
                                recordFail += 1;
                            }
                        }
                        catch (Exception e)
                        {
                            _logger.LogError("[Error] " + e.Message.ToString());
                            recordFail += 1;
                            continue;
                        }
                    }
                }
                return new JsonResponse<ImportExcelResponseDto>(true, new ImportExcelResponseDto(recordInserted, recordFail), ResponseMessage.CreateSuccess.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError("[Error] " + e.Message.ToString());
                return new JsonResponse<ImportExcelResponseDto>(false, null, e.Message.ToString());
            }
        }
        public MemoryStream ExportExcelCoursePS(string keyword)
        {
            List<CoursePGModel> courses = _coursePSRepository.GetAllByKeyWord(keyword);
            MemoryStream stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Courses");
                // Add Title
                using (var r = workSheet.Cells["A1:D1"])
                {
                    r.Style.Font.Bold = true;
                }
                // Add Datas
                workSheet.Cells.LoadFromCollection(courses, true);
                package.Save();
            }
            stream.Position = 0;
            return stream;
        }
    }
}
