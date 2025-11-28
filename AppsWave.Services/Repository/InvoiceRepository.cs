using AppsWave.Entites;
using AppsWave.Services.Data;
using AppsWave.Services.Repository.IRepository;

namespace AppsWave.Services.Repository;

public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
{
    public InvoiceRepository(AppDbContext db) : base(db) { }

    public async Task<Invoice?> GetInvoiceWithDetailsAsync(int invoiceId)
        => await GetAsync(i => i.InvoiceId == invoiceId, includeProperties: "Details.Product");

    public async Task<IEnumerable<Invoice>> GetUserInvoicesAsync(string userId)
     => await GetAllAsync(filter: i => i.UserId == userId, orderBy: q => q.OrderByDescending(i => i.Date), includeProperties: "Details.Product");
}