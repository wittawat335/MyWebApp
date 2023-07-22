using Microsoft.EntityFrameworkCore;
using MyWebApp.Core.Domain.RepositoryContracts;
using MyWebApp.Core.Model;
using MyWebApp.Infrastructure.DBContext;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Dapper.SqlMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MyWebApp.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly NCLS_SITContext _dbContext;
        private DbSet<T> _dbSet;
        public GenericRepository(NCLS_SITContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }
        public async Task<T> Add(T model)
        {
            try
            {
                _dbContext.Set<T>().Add(model);
                await _dbContext.SaveChangesAsync();
                return model;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> AddList(List<T> model)
        {
            try
            {
                _dbContext.Set<T>().AddRange(model);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> Update(T model)
        {
            try
            {
                _dbContext.Set<T>().Update(model);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> DeleteList(List<T> model)
        {
            try
            {
                _dbContext.Set<T>().RemoveRange(model);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> Delete(T model)
        {
            try
            {
                _dbContext.Set<T>().Remove(model);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<T> Get(Expression<Func<T, bool>> filter)
        {
            try
            {
                T? result = await _dbContext.Set<T>().FirstOrDefaultAsync(filter);
                return result;
            }
            catch
            {
                throw;
            }
        }
        public async Task<IQueryable<T>> GetAll(Expression<Func<T, bool>>? filter = null, int? skip = null,int? take = null)
        {
            try
            {
                IQueryable<T> query = filter == null ? _dbContext.Set<T>() : _dbContext.Set<T>().Where(filter);

                if (skip != null)
                    query = query.Skip(skip.Value);

                if (take != null)
                    query = query.Take(take.Value);

                return query;
            }
            catch
            {
                throw;
            }
        }
        public List<T> GetListBySP(string command)
        {
            try
            {
                var result = _dbContext.Set<T>().FromSqlRaw(command).ToList();
                return result;
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<T>> GetListAsyncBySP(string command)
        {
            try
            {
                var result = await _dbContext.Set<T>().FromSqlRaw(command).ToListAsync();
                return result;
            }
            catch
            {
                throw;
            }
        }

        #region Async
        public async Task<List<T>> GetFilteredAsync(Expression<Func<T, bool>>[] filters, int? skip, int? take, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet;
                foreach (var filter in filters)
                    query = query.Where(filter);

                if (skip != null)
                    query = query.Skip(skip.Value);

                if (take != null)
                    query = query.Take(take.Value);

                return await query.ToListAsync();
            }
            catch
            {
                throw;
            }
          
        }
        public async Task<List<T>> GetAsync(int? skip, int? take, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet;
                foreach (var include in includes)
                    query = query.Include(include);

                if (skip != null)
                    query = query.Skip(skip.Value);

                if (take != null)
                    query = query.Take(take.Value);

                return await query.ToListAsync();
            }
            catch
            {
                throw;
            }            
        }
        public void InsertAsync(T model)
        {
            _dbSet.AddAsync(model);           
        }
        public void UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _dbSet.Entry(entity).State = EntityState.Modified;
        }
        public void DeleteAsync(T entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);
                _dbSet.Remove(entity);
        }
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        #endregion
    }
}
