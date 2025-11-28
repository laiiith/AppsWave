using System.Linq.Expressions;

namespace AppsWave.Services.Repository.IRepository;

public interface IRepository<T> where T : class
{
    Task<T?> GetAsync(Expression<Func<T, bool>> filter,string? includeProperties = null,bool tracked = false);

    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null,Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,string? includeProperties = null,int? skip = null,int? take = null);

    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filter,string? includeProperties = null);

    Task AddAsync(T entity);

    void Add(T entity);

    void Remove(T entity);

    void RemoveRange(IEnumerable<T> entities);

    Task<int> SaveChangesAsync();
}
