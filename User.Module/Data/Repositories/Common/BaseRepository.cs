using User.Module.Domain.Common;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Module.Data.Repositories.Common
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAll(int offset, int size);
        Task<T> GetById(int id);
        Task Insert(T obj);
        Task Update(T obj);
        Task Delete(int id);
        Task<int> Count();
        Task Save();

        Task<IEnumerable<T>> QueryListAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default);
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default);
        Task<T> QuerySingleAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default);
        Task ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default);
    }

    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private UserDbContext _context = null;
        protected DbSet<T> table = null;
        public IDbConnection DbConn { get { return _context.Database.GetDbConnection(); } }
        public BaseRepository(UserDbContext _context)
        {
            this._context = _context;
            table = _context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAll(int offset, int size)
        {
            return await table.AsNoTracking().Skip(offset).Take(size).ToListAsync();
        }
        public async Task<T> GetById(int id)
        {
            return await table.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task Insert(T obj)
        {
            await table.AddAsync(obj);
            await Save();
        }
        public async Task Update(T obj)
        {
            table.Entry(obj).State = EntityState.Modified;
            await Save();
            table.Entry(obj).State = EntityState.Detached;
        }

        public async Task Delete(int id)
        {
            T existing = await GetById(id);
            table.Remove(existing);
            await Save();
        }
        public async Task<int> Count()
        {
            return await table.CountAsync();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> QueryListAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return (await DbConn.QueryAsync<T>(sql, param, transaction)).AsList();
        }
        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await DbConn.QueryFirstOrDefaultAsync<T>(sql, param, transaction);
        }
        public async Task<T> QuerySingleAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await DbConn.QuerySingleAsync<T>(sql, param, transaction);
        }
        public async Task ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            await DbConn.ExecuteAsync(sql, param, transaction);
        }

    }
}
