namespace AppsWave.Entites;

public class Invoice
{
    public int InvoiceId { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string UserId { get; set; } = string.Empty;
    public User User { get; set; } = null!;

    public ICollection<InvoiceDetail> Details { get; set; } = new List<InvoiceDetail>();
    public decimal TotalAmount => Details.Sum(d => d.Price * d.Quantity);
}
public class InvoiceDetail
{
    public int InvoiceDetailId { get; set; }
    public Invoice Invoice { get; set; } = null!;
    public int InvoiceId { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
