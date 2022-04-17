using CourseDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseDemo.Context.Postgres
{
    public class PGContext : DbContext
    {
        public PGContext(DbContextOptions<PGContext> options) : base(options)
        {
        }

        public PGContext()
        {
        }

        public DbSet<CoursePGModel> CoursePGModel { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            return base.SaveChanges();
        }
    }
}
