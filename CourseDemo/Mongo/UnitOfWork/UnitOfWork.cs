using CourseDemo.Mongo.Context;
using System;
using System.Threading.Tasks;

namespace CourseDemo.Mongo.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> Commit();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoContext _context;

        public UnitOfWork(IMongoContext context)
        {
            _context = context;
        }

        public async Task<bool> Commit()
        {
            var changeAmount = await _context.SaveChanges();

            return changeAmount > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
