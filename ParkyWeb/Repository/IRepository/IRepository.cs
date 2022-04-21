using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Repository.IRepository
{
    interface IRepository<T> where T : class
    {
        Task<T> GetAsync(string url, int Id);
        Task<IEnumerable<T>> GetAllAsync (string url);
        Task<bool> CreateAsync(string url, T objectToCreate);
        Task<bool> UpdateAsync(string url, T objectToUpdate);
        Task<bool> DeleteAsync(string url, int Id);
    }
}
