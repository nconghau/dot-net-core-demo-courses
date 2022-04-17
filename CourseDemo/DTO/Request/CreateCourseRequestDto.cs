using CourseDemo.Services;
using CourseDemo.Models;

namespace CourseDemo.DTO.Request
{
    public class CreateCourseRequestDto
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public CreateCourseRequestDto()
        {
        }
        public CreateCourseRequestDto(string name, string content)
        {
            Name = name;
            Content = content;
        }
        public CourseModel ToModel()
        {
            long CreatedAt = Utils.GetTimestamp();

            return new CourseModel(null, Name, Content, CreatedAt);
        }
        public CoursePGModel ToModelPG()
        {
            long CreatedAt = Utils.GetTimestamp();

            return new CoursePGModel(null, Name, Content, CreatedAt);
        }
    }
}
