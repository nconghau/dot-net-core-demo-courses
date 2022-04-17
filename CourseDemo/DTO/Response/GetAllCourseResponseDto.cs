using CourseDemo.Models;
using System.Collections.Generic;

namespace CourseDemo.DTO.Response
{
    public class PaginationDto
    {
        public int TotalPage { get; }
        public int CurrentPage { get; }
        public int TotalRecord { get; }
        public PaginationDto()
        {
        }
        public PaginationDto(int totalPagel, int currentPagel, int totalRecord)
        {
            TotalPage = totalPagel;
            CurrentPage = currentPagel;
            TotalRecord = totalRecord;
        }
    }
    public class GetAllCourseResponseDto
    {
        public IEnumerable<CourseModel> Datas { get; }
        public PaginationDto Pagination { get; } 
        public GetAllCourseResponseDto(IEnumerable<CourseModel> datas, PaginationDto pagination)
        {
            Datas = datas;
            Pagination = pagination;
        }
    }
    public class GetAllCoursePGResponseDto
    {
        public IEnumerable<CoursePGModel> Datas { get; }
        public PaginationDto Pagination { get; }
        public GetAllCoursePGResponseDto(IEnumerable<CoursePGModel> datas, PaginationDto pagination)
        {
            Datas = datas;
            Pagination = pagination;
        }
    }
}
