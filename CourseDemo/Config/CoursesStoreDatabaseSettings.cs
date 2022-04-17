namespace CourseDemo.Config
{
    public interface ICoursesStoreDatabaseSettings
    {
        string CoursesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
    public class CoursesStoreDatabaseSettings : ICoursesStoreDatabaseSettings
    {
        public string CoursesCollectionName { get; set; } = "Courses";
        public string ConnectionString { get; set; } = "mongodb://localhost:27017";
        public string DatabaseName { get; set; } = "CourseStore";
    }
}
