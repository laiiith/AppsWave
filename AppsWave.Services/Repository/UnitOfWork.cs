using AppsWave.Services.Data;
using AppsWave.Services.Repository.IRepository;

namespace AppsWave.Services.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;
    public IProductRepository Products { get; }
    public IInvoiceRepository Invoices { get; }

    public UnitOfWork(AppDbContext db, IProductRepository productRepository, IInvoiceRepository invoiceRepository)
    {
        _db = db;
        Products = productRepository;
        Invoices = invoiceRepository;
    }

    public async Task<int> SaveAsync() => await _db.SaveChangesAsync();
    public void Dispose() => _db.Dispose();
}