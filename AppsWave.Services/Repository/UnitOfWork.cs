using AppsWave.Services.Data;
using AppsWave.Services.Repository.IRepository;

namespace AppsWave.Services.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;
    public IProductRepository Products { get; private set; }
    public IInvoiceRepository Invoices { get; private set; }

    public UnitOfWork(AppDbContext db)
    {
        _db = db;
        Products = new ProductRepository(db);
        Invoices = new InvoiceRepository(db);
    }

    public async Task<int> SaveAsync() => await _db.SaveChangesAsync();
    public void Dispose() => _db.Dispose();
}