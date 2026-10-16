using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Core.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(string query);
        Task<T> GetById<Tid>(Tid id);
        Task<bool> IsExists<Tvalue>(string key, Tvalue value);
        Task<T> Create(T model);
        Task Update(T model);
        Task Delete(T model);
        Task<IEnumerable<T>> ExecuteQuery(string query);
        Task SaveChangeAsync();
    }
}
