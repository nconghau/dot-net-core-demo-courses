using CourseDemo.DTO.Request;
using CourseDemo.DTO.Response;
using CourseDemo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseDemo.Repositories.Postgres
{
    public interface ICoursesPGRepository
    {
        bool Create(CreateCourseRequestDto createCourseDto);
        Task<bool> CreateAsync(CreateCourseRequestDto createCourseDto);
        Task<bool> DeleteAsync(string id);
        List<CoursePGModel> GetAllByKeyWord(string keyword);
        Task<GetAllCoursePGResponseDto> GetAllAsync(int page, int pagesize, string keyword);
        Task<CoursePGModel> GetByIdAsync(string id);
        Task<bool> UpdateAsync(UpdateCourseRequestDto updateCourseDto);
    }
}