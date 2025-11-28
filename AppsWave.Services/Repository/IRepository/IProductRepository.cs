using AppsWave.Entites;

namespace AppsWave.Services.Repository.IRepository;

public interface IProductRepository : IRepository<Product>
{
    Task UpdateAsync(Product entity);
}
