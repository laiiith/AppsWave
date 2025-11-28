using AppsWave.Entites;

namespace AppsWave.Services.Repository.IRepository;

public interface IInvoiceRepository : IRepository<Invoice>
{
    Task<Invoice?> GetInvoiceWithDetailsAsync(int invoiceId);
    Task<IEnumerable<Invoice>> GetUserInvoicesAsync(string userId);
}
