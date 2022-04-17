using CourseDemo.Mongo.Context;
using MongoDB.Driver;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseDemo.Repositories.Mongo.Base
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        void Create(TEntity obj);
        Task<TEntity> GetById(string id);
        Task<IEnumerable<TEntity>> GetAll();
        void Update(TEntity obj);
        void Delete(string id);
    }
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly IMongoContext Context;
        protected IMongoCollection<TEntity> DbSet;

        protected BaseRepository(IMongoContext context)
        {
            Context = context;
            DbSet = Context.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public virtual void Create(TEntity obj)
        {
            Context.AddCommand(() => DbSet.InsertOneAsync(obj));
        }

        public virtual async Task<TEntity> GetById(string id)
        {
            var data = await DbSet.FindAsync(Builders<TEntity>.Filter.Eq("Id", id));
            return data.SingleOrDefault();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            var all = await DbSet.FindAsync(Builders<TEntity>.Filter.Empty);
            return all.ToList();
        }

        public virtual void Update(TEntity obj)
        {
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("Id", obj.GetId());
            Context.AddCommand(() => DbSet.ReplaceOneAsync(filter, obj, new ReplaceOptions { IsUpsert = true }));
        }

        public virtual void Delete(string id)
        {
            Context.AddCommand(() => DbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("Id", id)));
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
