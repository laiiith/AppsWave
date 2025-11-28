namespace AppsWave.Services.Repository.IRepository;

public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }
    IInvoiceRepository Invoices { get; }
    Task<int> SaveAsync();
}