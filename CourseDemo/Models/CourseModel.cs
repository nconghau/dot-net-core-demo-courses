using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace CourseDemo.Models
{
    public class CourseModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public long CreatedAt { get; set; }

        public CourseModel()
        {
        }

        public CourseModel(string id, string name, string content, long createdAt)
        {
            Id = id;
            Name = name;
            Content = content;
            CreatedAt = createdAt;
        }
    }
}
