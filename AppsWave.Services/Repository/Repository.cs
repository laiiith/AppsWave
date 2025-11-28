using AppsWave.Services.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AppsWave.Services.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppsWave.Services.Data.AppDbContext _db;
    internal DbSet<T> dbSet;
    public Repository(AppsWave.Services.Data.AppDbContext db)
    {
        _db = db;
        this.dbSet = _db.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await dbSet.AddAsync(entity);
    }

    public void Add(T entity)
    {
        dbSet.Add(entity);
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
    {
        IQueryable<T> query = tracked ? dbSet : dbSet.AsNoTracking();

        query = query.Where(filter);

        if (!string.IsNullOrWhiteSpace(includeProperties))
        {
            foreach (var prop in includeProperties
                .Split([','], StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(prop.Trim());
            }
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeProperties = null, int? skip = null, int? take = null)
    {
        IQueryable<T> query = dbSet.AsNoTracking();

        if (filter != null)
            query = query.Where(filter);

        if (!string.IsNullOrWhiteSpace(includeProperties))
        {
            foreach (var prop in includeProperties.Split([','], StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(prop.Trim());
            }
        }

        if (orderBy != null)
            query = orderBy(query);

        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (take.HasValue)
            query = query.Take(take.Value);

        return await query.ToListAsync();
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, string? includeProperties = null)
        => await GetAsync(filter, includeProperties, tracked: false);

    public void Remove(T entity)
        => dbSet.Remove(entity);

    public void RemoveRange(IEnumerable<T> entities)
    => dbSet.RemoveRange(entities);

    public async Task<int> SaveChangesAsync()
    => await _db.SaveChangesAsync();
}
