using CourseDemo.Services;
using CourseDemo.Models;

namespace CourseDemo.DTO.Request
{
    public class UpdateCourseRequestDto
    {
        public string Id { get; }
        public string Name { get; }
        public string Content { get; }
        public UpdateCourseRequestDto(string id, string name, string content)
        {
            Id = id;
            Name = name;
            Content = content;
        }
    }
}
