using BetAPI.DTO;
using BetAPI.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace BetAPI.Repositories
{
    public interface IGenericRepository<T> where T : AbstractEntity
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        T? GetById(int id);
        Task InsertAsync(T obj);
        Task UpdateAsync(T obj);
        void Delete(int id);
        void Save();
        Task<int> SaveAsync();
        IDbContextTransaction StartTransaction();
    }
}
