using BetAPI.Data;
using BetAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BetAPI.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : AbstractEntity
    {
        protected readonly BetAPIContext _context;
        private DbSet<T> table;

        public GenericRepository(BetAPIContext _context)
        {
            this._context = _context;
            table = _context.Set<T>();
        }

        public Task<List<T>> GetAllAsync()
        {
            return table.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await table.FirstOrDefaultAsync(i => i.Id == id);
        }

        public T? GetById(int id)
        {
            return table.First(i => i.Id == id);
        }

        public async Task InsertAsync(T obj)
        {
            await table.AddAsync(obj);
        }

        public async Task UpdateAsync(T obj)
        {
            T? entity = await GetByIdAsync(obj.Id);
            table.Entry(entity).CurrentValues.SetValues(obj);
        }

        public void Delete(int id)
        {
            T? existing = GetById(id);
            table.Remove(existing);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IDbContextTransaction StartTransaction()
        {
            return _context.Database.BeginTransaction();
        }

    }
}
