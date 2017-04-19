using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Calc.Repository.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class, new()
    {
        protected readonly DbContext context;
        protected readonly IDbSet<T> dbSet;
        public GenericRepository(DbContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return dbSet;
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(predicate.Compile());
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        public void AddOrUpdate(T obj)
        {
            dbSet.AddOrUpdate(obj);
            context.SaveChanges();
        }

        public void Delete(T obj)
        {
            dbSet.Remove(obj);
            context.SaveChanges();
        }

        public Task<int> AddOrUpdateAsync(T obj)
        {
            dbSet.AddOrUpdate(obj);
            return context.SaveChangesAsync();
        }
    }
}
