using CourseDemo.DTO.Request;
using CourseDemo.DTO.Response;
using CourseDemo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseDemo.Repositories.Mongo
{
    public interface ICoursesRepository 
    {
        bool Create(CreateCourseRequestDto createCourseDto);
        Task<bool> CreateAsync(CreateCourseRequestDto createCourseDto);
        Task<bool> DeleteAsync(string id);
        List<CourseModel> GetAllByKeyWord(string keyword);
        Task<GetAllCourseResponseDto> GetAllAsync(int page, int pagesize, string keyword);
        Task<CourseModel> GetByIdAsync(string id);
        Task<bool> UpdateAsync(UpdateCourseRequestDto updateCourseDto);
    }
}