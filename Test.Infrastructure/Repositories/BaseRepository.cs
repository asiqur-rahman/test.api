using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Interfaces.Repositories;
using Test.Infrastructure.Data;

namespace Test.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        protected DbSet<T> DbSet => _dbContext.Set<T>();

        public BaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<T>> GetAll(string query)
        {
            return await _dbContext.Set<T>().FromSqlRaw(query).AsNoTracking().ToListAsync();
        }


        public async Task<T> GetById<Tid>(Tid id)
        {
            var data = await _dbContext.Set<T>().FindAsync(id);
            if (data == null)
                throw new Exception("No data found");
            return data;
        }

        public async Task<bool> IsExists<Tvalue>(string key, Tvalue value)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, key);
            var constant = Expression.Constant(value);
            var equality = Expression.Equal(property, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);

            return await _dbContext.Set<T>().AnyAsync(lambda);
        }


        public async Task<T> Create(T model)
        {
            await _dbContext.Set<T>().AddAsync(model);
            await _dbContext.SaveChangesAsync();
            return model;
        }

        public async Task Update(T model)
        {
            _dbContext.Set<T>().Update(model);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(T model)
        {
            _dbContext.Set<T>().Remove(model);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SaveChangeAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<T>> ExecuteQuery(string query)
        {
            return await _dbContext.Set<T>().FromSqlRaw(query).ToListAsync();
        }

    }
}
