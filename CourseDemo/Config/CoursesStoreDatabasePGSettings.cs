namespace CourseDemo.Config
{
    public interface ICoursesStoreDatabasePGSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
    public class CoursesStoreDatabasePGSettings : ICoursesStoreDatabasePGSettings
    {
        public string ConnectionString { get; set; } = "Server=localhost;Port=5432;Database=CoursesStore;User Id=postgres;Password=nch";
        public string DatabaseName { get; set; } = "CourseStore";
    }
}
