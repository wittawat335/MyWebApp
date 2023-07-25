using MyWebApp.Core.Model;
using System.Linq.Expressions;

namespace MyWebApp.Core.Domain.RepositoryContracts
{
    public interface IGenericRepository<T> where T :  class
    {
        Task<IQueryable<T>> GetAll(Expression<Func<T, bool>>? filter = null,
              int? skip = null, int? take = null);
        Task<T> Get(Expression<Func<T, bool>> filter);
        Task<T> Add(T model);
        T AddOnly(T model);
        Task<bool> AddList(List<T> model);
        Task<bool> Update(T model);
        Task<bool> Delete(T model);
        Task<bool> DeleteList(List<T> model);
        List<T> GetListBySP(string command);
        Task<List<T>> GetListAsyncBySP(string command);

        #region testAsync
        Task<List<T>> GetFilteredAsync(Expression<Func<T, bool>>[] filters, int? skip, int? take, params Expression<Func<T, object>>[] includes);
        Task<List<T>> GetAsync(int? skip, int? take, params Expression<Func<T, object>>[] includes);
        void InsertAsync(T model);
        void UpdateAsync(T model);
        void DeleteAsync(T model);
        Task SaveChangesAsync();
        #endregion
    }
}
