using AppsWave.Entites;
using AppsWave.Services.Data;
using AppsWave.Services.Repository.IRepository;

namespace AppsWave.Services.Repository
{

    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly AppDbContext _db;
        public ProductRepository(AppDbContext db) : base(db) => _db = db;
        public async Task UpdateAsync(Product entity)
        {
            _db.Products.Update(entity);
            await Task.CompletedTask;
        }
    }
}
