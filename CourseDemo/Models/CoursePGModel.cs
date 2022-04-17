using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseDemo.Models
{
    public class CoursePGModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public long CreatedAt { get; set; }

        public CoursePGModel()
        {
        }

        public CoursePGModel(string id, string name, string content, long createdAt)
        {
            Id = id;
            Name = name;
            Content = content;
            CreatedAt = createdAt;
        }
    }
}
